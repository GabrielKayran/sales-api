using Ambev.DeveloperEvaluation.Application.Carts.GetMyCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts;

/// <summary>
/// Contains unit tests for the GetMyCartHandler class.
/// Tests cover successful cart retrieval, null handling, and error scenarios.
/// </summary>
public class GetMyCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly GetMyCartHandler _handler;

    /// <summary>
    /// Initializes a new instance of the GetMyCartHandlerTests class.
    /// Sets up the test dependencies and creates the handler instance.
    /// </summary>
    public GetMyCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetMyCartHandler(_cartRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid user ID with existing cart When getting my cart Then returns cart successfully")]
    public async Task Handle_ValidUserIdWithExistingCart_ReturnsCartSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var cartId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var cart = new Cart
        {
            Id = cartId,
            UserId = userId,
            Date = DateTime.UtcNow,
            Products = new List<CartProduct>
            {
                new CartProduct
                {
                    ProductId = productId,
                    Quantity = 2
                }
            }
        };

        var expectedResult = new GetMyCartResult
        {
            Id = cartId,
            UserId = userId,
            Date = cart.Date,
            Products = new List<GetMyCartProductResult>
            {
                new GetMyCartProductResult
                {
                    ProductId = productId,
                    Quantity = 2
                }
            }
        };

        var command = new GetMyCartQuery(userId);

        _cartRepository.GetCartsByUserIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(new List<Cart> { cart });
        _mapper.Map<GetMyCartResult>(cart).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(cartId);
        result.UserId.Should().Be(userId);
        result.Products.Should().NotBeEmpty();
        result.Products.Should().HaveCount(1);
        result.Products.First().ProductId.Should().Be(productId);
        result.Products.First().Quantity.Should().Be(2);

        await _cartRepository.Received(1).GetCartsByUserIdAsync(userId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetMyCartResult>(cart);
    }

    [Fact(DisplayName = "Given valid user ID with no carts When getting my cart Then returns null")]
    public async Task Handle_ValidUserIdWithNoCarts_ReturnsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new GetMyCartQuery(userId);

        _cartRepository.GetCartsByUserIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(new List<Cart>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();

        await _cartRepository.Received(1).GetCartsByUserIdAsync(userId, Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<GetMyCartResult>(Arg.Any<Cart>());
    }

    [Fact(DisplayName = "Given valid user ID with multiple carts When getting my cart Then returns most recent cart")]
    public async Task Handle_ValidUserIdWithMultipleCarts_ReturnsMostRecentCart()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var olderCartId = Guid.NewGuid();
        var newerCartId = Guid.NewGuid();

        var olderCart = new Cart
        {
            Id = olderCartId,
            UserId = userId,
            Date = DateTime.UtcNow.AddDays(-2),
            Products = new List<CartProduct>()
        };

        var newerCart = new Cart
        {
            Id = newerCartId,
            UserId = userId,
            Date = DateTime.UtcNow.AddDays(-1),
            Products = new List<CartProduct>()
        };

        var expectedResult = new GetMyCartResult
        {
            Id = newerCartId,
            UserId = userId,
            Date = newerCart.Date,
            Products = new List<GetMyCartProductResult>()
        };

        var command = new GetMyCartQuery(userId);

        // Repository returns carts in any order
        _cartRepository.GetCartsByUserIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(new List<Cart> { olderCart, newerCart });
        _mapper.Map<GetMyCartResult>(newerCart).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(newerCartId);
        result.Date.Should().Be(newerCart.Date);

        await _cartRepository.Received(1).GetCartsByUserIdAsync(userId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetMyCartResult>(newerCart);
        _mapper.DidNotReceive().Map<GetMyCartResult>(olderCart);
    }

    [Fact(DisplayName = "Given empty user ID When getting my cart Then returns null")]
    public async Task Handle_EmptyUserId_ReturnsNull()
    {
        // Arrange
        var command = new GetMyCartQuery(Guid.Empty);

        _cartRepository.GetCartsByUserIdAsync(Guid.Empty, Arg.Any<CancellationToken>())
            .Returns(new List<Cart>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();

        await _cartRepository.Received(1).GetCartsByUserIdAsync(Guid.Empty, Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<GetMyCartResult>(Arg.Any<Cart>());
    }

    [Fact(DisplayName = "Given repository throws exception When getting my cart Then propagates exception")]
    public async Task Handle_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new GetMyCartQuery(userId);
        var expectedException = new InvalidOperationException("Database error");

        _cartRepository.When(x => x.GetCartsByUserIdAsync(userId, Arg.Any<CancellationToken>()))
            .Do(x => throw expectedException);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database error");

        await _cartRepository.Received(1).GetCartsByUserIdAsync(userId, Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<GetMyCartResult>(Arg.Any<Cart>());
    }

    [Fact(DisplayName = "Given mapper throws exception When getting my cart Then propagates exception")]
    public async Task Handle_MapperThrowsException_PropagatesException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var cartId = Guid.NewGuid();

        var cart = new Cart
        {
            Id = cartId,
            UserId = userId,
            Date = DateTime.UtcNow,
            Products = new List<CartProduct>()
        };

        var command = new GetMyCartQuery(userId);
        var expectedException = new AutoMapperMappingException("Mapping error");

        _cartRepository.GetCartsByUserIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(new List<Cart> { cart });
        _mapper.When(x => x.Map<GetMyCartResult>(cart))
            .Do(x => throw expectedException);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AutoMapperMappingException>()
            .WithMessage("Mapping error");

        await _cartRepository.Received(1).GetCartsByUserIdAsync(userId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetMyCartResult>(cart);
    }

    [Fact(DisplayName = "Given cart with empty products list When getting my cart Then returns cart with empty products")]
    public async Task Handle_CartWithEmptyProducts_ReturnsCartWithEmptyProducts()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var cartId = Guid.NewGuid();

        var cart = new Cart
        {
            Id = cartId,
            UserId = userId,
            Date = DateTime.UtcNow,
            Products = new List<CartProduct>() // Empty products list
        };

        var expectedResult = new GetMyCartResult
        {
            Id = cartId,
            UserId = userId,
            Date = cart.Date,
            Products = new List<GetMyCartProductResult>()
        };

        var command = new GetMyCartQuery(userId);

        _cartRepository.GetCartsByUserIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(new List<Cart> { cart });
        _mapper.Map<GetMyCartResult>(cart).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(cartId);
        result.Products.Should().NotBeNull();
        result.Products.Should().BeEmpty();

        await _cartRepository.Received(1).GetCartsByUserIdAsync(userId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetMyCartResult>(cart);
    }

    [Fact(DisplayName = "Given cart with multiple products When getting my cart Then returns cart with all products")]
    public async Task Handle_CartWithMultipleProducts_ReturnsCartWithAllProducts()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var cartId = Guid.NewGuid();
        var product1Id = Guid.NewGuid();
        var product2Id = Guid.NewGuid();

        var cart = new Cart
        {
            Id = cartId,
            UserId = userId,
            Date = DateTime.UtcNow,
            Products = new List<CartProduct>
            {
                new CartProduct { ProductId = product1Id, Quantity = 2 },
                new CartProduct { ProductId = product2Id, Quantity = 5 }
            }
        };

        var expectedResult = new GetMyCartResult
        {
            Id = cartId,
            UserId = userId,
            Date = cart.Date,
            Products = new List<GetMyCartProductResult>
            {
                new GetMyCartProductResult { ProductId = product1Id, Quantity = 2 },
                new GetMyCartProductResult { ProductId = product2Id, Quantity = 5 }
            }
        };

        var command = new GetMyCartQuery(userId);

        _cartRepository.GetCartsByUserIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(new List<Cart> { cart });
        _mapper.Map<GetMyCartResult>(cart).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(cartId);
        result.Products.Should().HaveCount(2);
        result.Products.Should().Contain(p => p.ProductId == product1Id && p.Quantity == 2);
        result.Products.Should().Contain(p => p.ProductId == product2Id && p.Quantity == 5);

        await _cartRepository.Received(1).GetCartsByUserIdAsync(userId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetMyCartResult>(cart);
    }
}
