using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the DeleteProductHandler class.
/// Tests cover successful product deletion and error handling scenarios.
/// </summary>
public class DeleteProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly DeleteProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the DeleteProductHandlerTests class.
    /// Sets up the test dependencies and creates the handler instance.
    /// </summary>
    public DeleteProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new DeleteProductHandler(_productRepository);
    }

    [Fact(DisplayName = "Given valid product ID When deleting product Then deletes successfully")]
    public async Task Handle_ValidProductId_DeletesSuccessfully()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new DeleteProductCommand(productId);

        var existingProduct = new Product
        {
            Id = productId,
            Title = "Test Product",
            Price = 99.99m,
            Description = "Test Description",
            Category = "Test Category",
            Image = "https://example.com/image.jpg",
            Rating = new ProductRating { Rate = 4.5m, Count = 100 }
        };

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns(existingProduct);
        _productRepository.DeleteAsync(productId, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
        await _productRepository.Received(1).DeleteAsync(productId, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given non-existent product ID When deleting product Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentProductId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new DeleteProductCommand(productId);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Produto com ID {productId} não foi encontrado");

        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
        await _productRepository.DidNotReceive().DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given repository fails to delete When deleting product Then returns false")]
    public async Task Handle_RepositoryFailsToDelete_ReturnsFalse()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new DeleteProductCommand(productId);

        var existingProduct = new Product
        {
            Id = productId,
            Title = "Test Product",
            Price = 99.99m,
            Description = "Test Description",
            Category = "Test Category",
            Image = "https://example.com/image.jpg"
        };

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns(existingProduct);
        _productRepository.DeleteAsync(productId, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
        await _productRepository.Received(1).DeleteAsync(productId, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given repository throws exception on get When deleting product Then propagates exception")]
    public async Task Handle_RepositoryThrowsExceptionOnGet_PropagatesException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new DeleteProductCommand(productId);
        var expectedException = new InvalidOperationException("Database error on get");

        _productRepository.When(x => x.GetByIdAsync(productId, Arg.Any<CancellationToken>()))
            .Do(x => throw expectedException);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database error on get");

        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
        await _productRepository.DidNotReceive().DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given repository throws exception on delete When deleting product Then propagates exception")]
    public async Task Handle_RepositoryThrowsExceptionOnDelete_PropagatesException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new DeleteProductCommand(productId);

        var existingProduct = new Product
        {
            Id = productId,
            Title = "Test Product",
            Price = 99.99m,
            Description = "Test Description",
            Category = "Test Category",
            Image = "https://example.com/image.jpg"
        };

        var expectedException = new InvalidOperationException("Database error on delete");

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns(existingProduct);
        _productRepository.When(x => x.DeleteAsync(productId, Arg.Any<CancellationToken>()))
            .Do(x => throw expectedException);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database error on delete");

        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
        await _productRepository.Received(1).DeleteAsync(productId, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given empty GUID When deleting product Then throws KeyNotFoundException")]
    public async Task Handle_EmptyGuid_ThrowsKeyNotFoundException()
    {
        // Arrange
        var productId = Guid.Empty;
        var command = new DeleteProductCommand(productId);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Produto com ID {productId} não foi encontrado");

        await _productRepository.Received(1).GetByIdAsync(productId, Arg.Any<CancellationToken>());
        await _productRepository.DidNotReceive().DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }
}
