using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Cart
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartProduct> Products { get; set; } = new List<CartProduct>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Cart()
    {
        Id = Guid.NewGuid();
        Date = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCart(Guid userId, DateTime date, List<CartProduct> products)
    {
        UserId = userId;
        Date = date;
        Products = products ?? new List<CartProduct>();
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddProduct(Guid productId, int quantity)
    {
        if (quantity <= 0)
            throw new ValidationException("Quantidade deve ser maior que zero");

        var existingProduct = Products.FirstOrDefault(p => p.ProductId == productId);
        if (existingProduct != null)
        {
            existingProduct.Quantity += quantity;
        }
        else
        {
            Products.Add(new CartProduct { ProductId = productId, Quantity = quantity });
        }
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveProduct(Guid productId)
    {
        var product = Products.FirstOrDefault(p => p.ProductId == productId);
        if (product != null)
        {
            Products.Remove(product);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void UpdateProductQuantity(Guid productId, int quantity)
    {
        if (quantity <= 0)
            throw new ValidationException("Quantidade deve ser maior que zero");

        var product = Products.FirstOrDefault(p => p.ProductId == productId);
        if (product != null)
        {
            product.Quantity = quantity;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

public class CartProduct
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
