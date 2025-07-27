using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the UpdateProductHandler class.
/// Tests cover successful product updates, mapping validation, and error handling.
/// </summary>
public class UpdateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly UpdateProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the UpdateProductHandlerTests class.
    /// Sets up the test dependencies and creates the handler instance.
    /// </summary>
    public UpdateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new UpdateProductHandler(_productRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid update command When updating product Then returns updated product successfully")]
    public async Task Handle_ValidUpdateCommand_ReturnsUpdatedProductSuccessfully()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var existingProduct = new Product
        {
            Id = productId,
            Title = "Old Product",
            Price = 50.00m,
            Description = "Old Description",
            Category = "Old Category",
            Image = "https://example.com/old-image.jpg",
            Rating = new ProductRating { Rate = 3.0m, Count = 50 }
        };

        var updateCommand = new UpdateProductCommand
        {
            Id = productId,
            Title = "Updated Product",
            Price = 99.99m,
            Description = "Updated Description",
            Category = "Updated Category",
            Image = "https://example.com/updated-image.jpg",
            Rating = new UpdateProductRatingRequest
            {
                Rate = 4.5m,
                Count = 100
            }
        };

        var updatedProduct = new Product
        {
            Id = productId,
            Title = "Updated Product",
            Price = 99.99m,
            Description = "Updated Description",
            Category = "Updated Category",
            Image = "https://example.com/updated-image.jpg",
            Rating = new ProductRating { Rate = 4.5m, Count = 100 }
        };

        var expectedResult = new UpdateProductResult
        {
            Id = productId,
            Title = "Updated Product",
            Price = 99.99m,
            Description = "Updated Description",
            Category = "Updated Category",
            Image = "https://example.com/updated-image.jpg",
            Rating = new UpdateProductRatingResult
            {
                Rate = 4.5m,
                Count = 100
            }
        };

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns(existingProduct);

        _productRepository.UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(updatedProduct);
        _mapper.Map<UpdateProductResult>(Arg.Any<Product>()).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(updateCommand, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(productId);
        result.Title.Should().Be("Updated Product");
        result.Price.Should().Be(99.99m);
        result.Rating.Should().NotBeNull();
        result.Rating.Rate.Should().Be(4.5m);
        result.Rating.Count.Should().Be(100);

        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());

        await _productRepository.Received(1).UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<UpdateProductResult>(Arg.Any<Product>());
    }

    [Fact(DisplayName = "Given non-existent product ID When updating product Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentProductId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var updateCommand = new UpdateProductCommand
        {
            Id = productId,
            Title = "Updated Product",
            Price = 99.99m,
            Description = "Updated Description",
            Category = "Updated Category",
            Image = "https://example.com/updated-image.jpg"
        };

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // Act
        var act = () => _handler.Handle(updateCommand, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Produto com ID {productId} não foi encontrado");

        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map(Arg.Any<UpdateProductCommand>(), Arg.Any<Product>());
        await _productRepository.DidNotReceive().UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given update command with null rating When updating product Then handles null rating correctly")]
    public async Task Handle_UpdateCommandWithNullRating_HandlesNullRatingCorrectly()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var existingProduct = new Product
        {
            Id = productId,
            Title = "Product",
            Price = 50.00m,
            Description = "Description",
            Category = "Category",
            Image = "https://example.com/image.jpg",
            Rating = new ProductRating { Rate = 3.0m, Count = 50 }
        };

        var updateCommand = new UpdateProductCommand
        {
            Id = productId,
            Title = "Updated Product",
            Price = 99.99m,
            Description = "Updated Description",
            Category = "Updated Category",
            Image = "https://example.com/updated-image.jpg",
            Rating = null // No rating update
        };

        var updatedProduct = new Product
        {
            Id = productId,
            Title = "Updated Product",
            Price = 99.99m,
            Description = "Updated Description",
            Category = "Updated Category",
            Image = "https://example.com/updated-image.jpg",
            Rating = null
        };

        var expectedResult = new UpdateProductResult
        {
            Id = productId,
            Title = "Updated Product",
            Price = 99.99m,
            Description = "Updated Description",
            Category = "Updated Category",
            Image = "https://example.com/updated-image.jpg",
            Rating = null
        };

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns(existingProduct);

        _productRepository.UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(updatedProduct);
        _mapper.Map<UpdateProductResult>(Arg.Any<Product>()).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(updateCommand, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(productId);
        result.Rating.Should().BeNull();

        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());

        await _productRepository.Received(1).UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<UpdateProductResult>(Arg.Any<Product>());
    }

    [Fact(DisplayName = "Given repository throws exception on get When updating product Then propagates exception")]
    public async Task Handle_RepositoryThrowsExceptionOnGet_PropagatesException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var updateCommand = new UpdateProductCommand
        {
            Id = productId,
            Title = "Updated Product",
            Price = 99.99m,
            Description = "Updated Description",
            Category = "Updated Category",
            Image = "https://example.com/updated-image.jpg"
        };

        var expectedException = new InvalidOperationException("Database error on get");

        _productRepository.When(x => x.GetByIdAsync(productId, Arg.Any<CancellationToken>()))
            .Do(x => throw expectedException);

        // Act
        var act = () => _handler.Handle(updateCommand, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database error on get");

        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map(Arg.Any<UpdateProductCommand>(), Arg.Any<Product>());
        await _productRepository.DidNotReceive().UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given repository throws exception on update When updating product Then propagates exception")]
    public async Task Handle_RepositoryThrowsExceptionOnUpdate_PropagatesException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var existingProduct = new Product
        {
            Id = productId,
            Title = "Product",
            Price = 50.00m,
            Description = "Description",
            Category = "Category",
            Image = "https://example.com/image.jpg"
        };

        var updateCommand = new UpdateProductCommand
        {
            Id = productId,
            Title = "Updated Product",
            Price = 99.99m,
            Description = "Updated Description",
            Category = "Updated Category",
            Image = "https://example.com/updated-image.jpg"
        };

        var updatedProduct = new Product
        {
            Id = productId,
            Title = "Updated Product",
            Price = 99.99m,
            Description = "Updated Description",
            Category = "Updated Category",
            Image = "https://example.com/updated-image.jpg"
        };

        var expectedException = new InvalidOperationException("Database error on update");

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns(existingProduct);

        _productRepository.When(x => x.UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()))
            .Do(x => throw expectedException);

        // Act
        var act = () => _handler.Handle(updateCommand, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database error on update");

        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());

        await _productRepository.Received(1).UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<UpdateProductResult>(Arg.Any<Product>());
    }

    [Fact(DisplayName = "Given mapper throws exception When updating product Then propagates exception")]
    public async Task Handle_MapperThrowsException_PropagatesException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var existingProduct = new Product
        {
            Id = productId,
            Title = "Product",
            Price = 50.00m,
            Description = "Description",
            Category = "Category",
            Image = "https://example.com/image.jpg"
        };

        var updateCommand = new UpdateProductCommand
        {
            Id = productId,
            Title = "Updated Product",
            Price = 99.99m,
            Description = "Updated Description",
            Category = "Updated Category",
            Image = "https://example.com/updated-image.jpg"
        };

        var expectedException = new AutoMapperMappingException("Mapping error");

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns(existingProduct);
        _productRepository.UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(existingProduct);
        _mapper.When(x => x.Map<UpdateProductResult>(Arg.Any<Product>()))
            .Do(x => throw expectedException);

        // Act
        var act = () => _handler.Handle(updateCommand, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AutoMapperMappingException>()
            .WithMessage("Mapping error");

        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
        await _productRepository.Received(1).UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<UpdateProductResult>(Arg.Any<Product>());
    }
}
