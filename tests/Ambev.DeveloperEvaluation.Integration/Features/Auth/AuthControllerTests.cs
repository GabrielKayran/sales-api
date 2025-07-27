using Ambev.DeveloperEvaluation.Integration.Common;
using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Features.Auth;

/// <summary>
/// Integration tests for AuthController endpoints
/// </summary>
public class AuthControllerTests : IntegrationTestBase
{
    public AuthControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact(DisplayName = "POST /api/users - Should register new user successfully")]
    public async Task Register_WithValidData_ShouldReturnSuccess()
    {
        // Arrange
        var registerRequest = new CreateUserRequest
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Test@123456",
            Phone = "55389983658",
            Role = Domain.Enums.UserRole.Customer
        };

        // Act
        var response = await _client.PostAsync("/api/users", CreateJsonContent(registerRequest));
        var content = await response.Content.ReadAsStringAsync();

        // Assert with error message for debugging
        response.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected OK but got {response.StatusCode}. Response content: {content}");
        
        if (!string.IsNullOrEmpty(content))
        {
            var apiResponse = JsonSerializer.Deserialize<ApiResponseWithData<CreateUserResponse>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data!.Email.Should().Be(registerRequest.Email);
            apiResponse.Data.Name.Should().Be(registerRequest.Username);
        }
    }

    [Fact(DisplayName = "POST /api/users - Should return error for invalid email")]
    public async Task Register_WithInvalidEmail_ShouldReturnBadRequest()
    {
        // Arrange
        var registerRequest = new CreateUserRequest
        {
            Username = "testuser",
            Email = "invalid-email",
            Password = "Test@123456",
            Phone = "(11) 91234-5678",
            Role = Domain.Enums.UserRole.Customer
        };

        // Act
        var response = await _client.PostAsync("/api/users", CreateJsonContent(registerRequest));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    

    [Fact(DisplayName = "POST /api/auth - Should return error for invalid credentials")]
    public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginRequest = new AuthenticateUserRequest
        {
            Email = "nonexistent@example.com",
            Password = "WrongPassword123"
        };

        // Act
        var response = await _client.PostAsync("/api/auth", CreateJsonContent(loginRequest));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized); 
    }

    [Fact(DisplayName = "POST /api/users - Should return error for duplicate email")]
    public async Task Register_WithDuplicateEmail_ShouldReturnBadRequest()
    {
        // Arrange - First register a user
        var firstCreateUserRequest = new CreateUserRequest
        {
            Username = "firstuser",
            Email = "duplicate@example.com",
            Password = "Test@123456",
            Phone = "(11) 97654-3210",
            Role = Domain.Enums.UserRole.Customer
        };

        await _client.PostAsync("/api/users", CreateJsonContent(firstCreateUserRequest));

        // Try to register another user with same email
        var secondCreateUserRequest = new CreateUserRequest
        {
            Username = "seconduser",
            Email = "duplicate@example.com", // Same email
            Password = "Test@654321",
            Phone = "(11) 96543-2109",
            Role = Domain.Enums.UserRole.Customer
        };

        // Act
        var response = await _client.PostAsync("/api/users", CreateJsonContent(secondCreateUserRequest));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory(DisplayName = "POST /api/users - Should validate required fields")]
    [InlineData("", "admin@ambev.com", "Test@123456", "55389983658")] // Empty username
    [InlineData("testuser", "", "Test@123456", "55389983658")] // Empty email
    [InlineData("testuser", "admin@ambev.com", "", "55389983658")] // Empty password
    [InlineData("testuser", "admin@ambev.com", "Test@123456", "")] // Empty phone
    public async Task Register_WithMissingRequiredFields_ShouldReturnBadRequest(
        string username, string email, string password, string phone)
    {
        // Arrange
        var registerRequest = new CreateUserRequest
        {
            Username = username,
            Email = email,
            Password = password,
            Phone = phone,
            Role = Domain.Enums.UserRole.Customer
        };

        // Act
        var response = await _client.PostAsync("/api/users", CreateJsonContent(registerRequest));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
