using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the GetProductHandler class.
/// Tests cover successful product retrieval, mapping validation, and error handling.
/// </summary>
public class GetProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly GetProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the GetProductHandlerTests class.
    /// Sets up the test dependencies and creates the handler instance.
    /// </summary>
    public GetProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetProductHandler(_productRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid product ID When getting product Then returns product successfully")]
    public async Task Handle_ValidProductId_ReturnsProductSuccessfully()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            Title = "Test Product",
            Price = 99.99m,
            Description = "Test Description",
            Category = "Test Category",
            Image = "https://example.com/image.jpg",
            Rating = new ProductRating
            {
                Rate = 4.5m,
                Count = 100
            }
        };

        var expectedResult = new GetProductResult
        {
            Id = productId,
            Title = "Test Product",
            Price = 99.99m,
            Description = "Test Description",
            Category = "Test Category",
            Image = "https://example.com/image.jpg",
            Rating = new GetProductRatingResult
            {
                Rate = 4.5m,
                Count = 100
            }
        };

        var command = new GetProductQuery(productId);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(productId);
        result.Title.Should().Be("Test Product");
        result.Price.Should().Be(99.99m);
        result.Rating.Should().NotBeNull();
        result.Rating.Rate.Should().Be(4.5m);
        result.Rating.Count.Should().Be(100);

        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetProductResult>(product);
    }

    [Fact(DisplayName = "Given non-existent product ID When getting product Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentProductId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new GetProductQuery(productId);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Produto com ID {productId} não foi encontrado");

        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<GetProductResult>(Arg.Any<Product>());
    }

    [Fact(DisplayName = "Given valid product with null rating When getting product Then maps correctly")]
    public async Task Handle_ProductWithNullRating_MapsCorrectly()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            Title = "Product Without Rating",
            Price = 49.99m,
            Description = "Product Description",
            Category = "Category",
            Image = "https://example.com/image.jpg",
            Rating = new ProductRating { Rate = 0, Count = 0 } // Default rating
        };

        var expectedResult = new GetProductResult
        {
            Id = productId,
            Title = "Product Without Rating",
            Price = 49.99m,
            Description = "Product Description",
            Category = "Category",
            Image = "https://example.com/image.jpg",
            Rating = null
        };

        var command = new GetProductQuery(productId);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(productId);
        result.Rating.Should().BeNull();

        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetProductResult>(product);
    }

    [Fact(DisplayName = "Given repository throws exception When getting product Then propagates exception")]
    public async Task Handle_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new GetProductQuery(productId);
        var expectedException = new InvalidOperationException("Database error");

        _productRepository.When(x => x.GetByIdAsync(productId, Arg.Any<CancellationToken>()))
            .Do(x => throw expectedException);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database error");

        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<GetProductResult>(Arg.Any<Product>());
    }

    [Fact(DisplayName = "Given mapper throws exception When getting product Then propagates exception")]
    public async Task Handle_MapperThrowsException_PropagatesException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            Title = "Test Product",
            Price = 99.99m,
            Description = "Test Description",
            Category = "Test Category",
            Image = "https://example.com/image.jpg",
            Rating = new ProductRating { Rate = 4.5m, Count = 100 }
        };

        var command = new GetProductQuery(productId);
        var expectedException = new AutoMapperMappingException("Mapping error");

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns(product);
        _mapper.When(x => x.Map<GetProductResult>(product))
            .Do(x => throw expectedException);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AutoMapperMappingException>()
            .WithMessage("Mapping error");

        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetProductResult>(product);
    }

    [Fact(DisplayName = "Given empty GUID When getting product Then throws KeyNotFoundException")]
    public async Task Handle_EmptyGuid_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new GetProductQuery(Guid.Empty);

        _productRepository.GetByIdAsync(Guid.Empty, Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();

        await _productRepository.Received(1).GetByIdAsync(Guid.Empty, Arg.Any<CancellationToken>());
    }
}
