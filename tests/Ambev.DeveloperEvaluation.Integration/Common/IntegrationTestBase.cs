using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Common;

/// <summary>
/// Base class for integration tests providing common setup and utilities
/// </summary>
public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly WebApplicationFactory<Program> _factory;
    protected readonly HttpClient _client;

    public IntegrationTestBase(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing DbContext registration
                services.RemoveAll(typeof(DbContextOptions<DefaultContext>));
                services.RemoveAll(typeof(DefaultContext));

                // Add InMemory database for testing
                services.AddDbContext<DefaultContext>(options =>
                {
                    options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}");
                });

                // Build service provider and ensure database is created
                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<DefaultContext>();
                context.Database.EnsureCreated();
            });
        });

        _client = _factory.CreateClient();
    }

    /// <summary>
    /// Creates HTTP content from an object using JSON serialization
    /// </summary>
    protected static StringContent CreateJsonContent(object obj)
    {
        var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    /// <summary>
    /// Deserializes HTTP response content to specified type
    /// </summary>
    protected static async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        });
    }

    /// <summary>
    /// Sets authorization header with Bearer token
    /// </summary>
    protected void SetAuthorizationHeader(string token)
    {
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    /// <summary>
    /// Clears authorization header
    /// </summary>
    protected void ClearAuthorizationHeader()
    {
        _client.DefaultRequestHeaders.Authorization = null;
    }
}
