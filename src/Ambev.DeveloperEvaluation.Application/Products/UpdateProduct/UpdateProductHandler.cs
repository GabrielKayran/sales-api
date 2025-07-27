using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public UpdateProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateProductValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingProduct = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingProduct == null)
            throw new KeyNotFoundException($"Produto com ID {request.Id} não foi encontrado");

        ProductRating? rating = null;
        if (request.Rating != null)
        {
            rating = new ProductRating
            {
                Rate = request.Rating.Rate,
                Count = request.Rating.Count
            };
        }

        existingProduct.UpdateProduct(
            request.Title,
            request.Price,
            request.Description,
            request.Category,
            request.Image,
            rating);

        var updatedProduct = await _productRepository.UpdateAsync(existingProduct, cancellationToken);

        return _mapper.Map<UpdateProductResult>(updatedProduct);
    }
}
