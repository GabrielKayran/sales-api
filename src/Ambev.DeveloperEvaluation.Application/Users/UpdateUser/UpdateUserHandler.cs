using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Common.Security;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// Handler for processing UpdateUserCommand requests
/// </summary>
public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UpdateUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public UpdateUserHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the UpdateUserCommand request
    /// </summary>
    /// <param name="request">The UpdateUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated user result</returns>
    public async Task<UpdateUserResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateUserValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingUser = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingUser == null)
            throw new KeyNotFoundException($"User with ID {request.Id} not found");

        // Check if email is being changed and if it's already in use
        if (existingUser.Email != request.Email)
        {
            var emailExists = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (emailExists != null)
                throw new InvalidOperationException("Email already in use by another user");
        }

        // Update user properties
        existingUser.Username = request.Username;
        existingUser.Email = request.Email;
        existingUser.Phone = request.Phone;
        existingUser.Status = request.Status;
        existingUser.Role = request.Role;

        // Hash password if it's being changed
        if (!string.IsNullOrEmpty(request.Password))
        {
            existingUser.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }

        var updatedUser = await _userRepository.UpdateAsync(existingUser, cancellationToken);
        return _mapper.Map<UpdateUserResult>(updatedUser);
    }
}
