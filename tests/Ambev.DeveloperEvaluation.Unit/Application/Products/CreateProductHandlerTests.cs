using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the CreateProductHandler class.
/// Tests cover successful product creation, validation, and error handling.
/// </summary>
public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly CreateProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the CreateProductHandlerTests class.
    /// Sets up the test dependencies and creates the handler instance.
    /// </summary>
    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateProductHandler(_productRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid create command When creating product Then returns created product successfully")]
    public async Task Handle_ValidCreateCommand_ReturnsCreatedProductSuccessfully()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var createCommand = new CreateProductCommand
        {
            Title = "Test Product",
            Price = 99.99m,
            Description = "Test Description",
            Category = "Test Category",
            Image = "https://example.com/image.jpg",
            Rating = new CreateProductRatingRequest
            {
                Rate = 4.5m,
                Count = 100
            }
        };

        var createdProduct = new Product
        {
            Id = productId,
            Title = "Test Product",
            Price = 99.99m,
            Description = "Test Description",
            Category = "Test Category",
            Image = "https://example.com/image.jpg",
            Rating = new ProductRating { Rate = 4.5m, Count = 100 },
            CreatedAt = DateTime.UtcNow
        };

        var expectedResult = new CreateProductResult
        {
            Id = productId,
            Title = "Test Product",
            Price = 99.99m,
            Description = "Test Description",
            Category = "Test Category",
            Image = "https://example.com/image.jpg",
            Rating = new CreateProductRatingResult
            {
                Rate = 4.5m,
                Count = 100
            },
            CreatedAt = createdProduct.CreatedAt
        };

        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(createdProduct);
        _mapper.Map<CreateProductResult>(Arg.Any<Product>()).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(createCommand, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(productId);
        result.Title.Should().Be("Test Product");
        result.Price.Should().Be(99.99m);
        result.Description.Should().Be("Test Description");
        result.Category.Should().Be("Test Category");
        result.Image.Should().Be("https://example.com/image.jpg");
        result.Rating.Should().NotBeNull();
        result.Rating.Rate.Should().Be(4.5m);
        result.Rating.Count.Should().Be(100);

        await _productRepository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<CreateProductResult>(Arg.Any<Product>());
    }

    [Fact(DisplayName = "Given create command with null rating When creating product Then throws NullReferenceException")]
    public async Task Handle_CreateCommandWithNullRating_ThrowsNullReferenceException()
    {
        // Arrange
        var createCommand = new CreateProductCommand
        {
            Title = "Test Product",
            Price = 99.99m,
            Description = "Test Description",
            Category = "Test Category",
            Image = "https://example.com/image.jpg",
            Rating = null
        };

        // Act
        var act = () => _handler.Handle(createCommand, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NullReferenceException>();

        await _productRepository.DidNotReceive().CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<CreateProductResult>(Arg.Any<Product>());
    }

    [Fact(DisplayName = "Given repository throws exception When creating product Then propagates exception")]
    public async Task Handle_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var createCommand = new CreateProductCommand
        {
            Title = "Test Product",
            Price = 99.99m,
            Description = "Test Description",
            Category = "Test Category",
            Image = "https://example.com/image.jpg"
        };

        var expectedException = new InvalidOperationException("Database error on create");

        _productRepository.When(x => x.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()))
            .Do(x => throw expectedException);

        // Act
        var act = () => _handler.Handle(createCommand, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database error on create");

        await _productRepository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<CreateProductResult>(Arg.Any<Product>());
    }

    [Fact(DisplayName = "Given mapper throws exception When creating product Then propagates exception")]
    public async Task Handle_MapperThrowsException_PropagatesException()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var createCommand = new CreateProductCommand
        {
            Title = "Test Product",
            Price = 99.99m,
            Description = "Test Description",
            Category = "Test Category",
            Image = "https://example.com/image.jpg"
        };

        var createdProduct = new Product
        {
            Id = productId,
            Title = "Test Product",
            Price = 99.99m,
            Description = "Test Description",
            Category = "Test Category",
            Image = "https://example.com/image.jpg",
            CreatedAt = DateTime.UtcNow
        };

        var expectedException = new AutoMapperMappingException("Mapping error");

        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(createdProduct);
        _mapper.When(x => x.Map<CreateProductResult>(Arg.Any<Product>()))
            .Do(x => throw expectedException);

        // Act
        var act = () => _handler.Handle(createCommand, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AutoMapperMappingException>()
            .WithMessage("Mapping error");

        await _productRepository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<CreateProductResult>(Arg.Any<Product>());
    }

    [Fact(DisplayName = "Given create command with negative price When creating product Then throws ValidationException")]
    public async Task Handle_CreateCommandWithNegativePrice_ThrowsValidationException()
    {
        // Arrange
        var createCommand = new CreateProductCommand
        {
            Title = "Test Product",
            Price = -10.00m,
            Description = "Test Description",
            Category = "Test Category",
            Image = "https://example.com/image.jpg"
        };

        // Act
        var act = () => _handler.Handle(createCommand, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Preço deve ser maior que zero*");

        await _productRepository.DidNotReceive().CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<CreateProductResult>(Arg.Any<Product>());
    }

    [Fact(DisplayName = "Given create command with empty title When creating product Then throws ValidationException")]
    public async Task Handle_CreateCommandWithEmptyTitle_ThrowsValidationException()
    {
        // Arrange
        var createCommand = new CreateProductCommand
        {
            Title = "",
            Price = 99.99m,
            Description = "Test Description",
            Category = "Test Category",
            Image = "https://example.com/image.jpg"
        };

        // Act
        var act = () => _handler.Handle(createCommand, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Título do produto é obrigatório*");

        await _productRepository.DidNotReceive().CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<CreateProductResult>(Arg.Any<Product>());
    }
}
