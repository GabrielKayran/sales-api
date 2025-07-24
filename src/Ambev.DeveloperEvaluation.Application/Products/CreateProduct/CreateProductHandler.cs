using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CreateProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateProductValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Price = request.Price,
            Description = request.Description,
            Category = request.Category,
            Image = request.Image,
            Rating = new ProductRating
            {
                Rate = request.Rating.Rate,
                Count = request.Rating.Count
            }
        };

        var createdProduct = await _productRepository.CreateAsync(product, cancellationToken);

        return _mapper.Map<CreateProductResult>(createdProduct);
    }
}
