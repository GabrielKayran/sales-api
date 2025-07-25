using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// AutoMapper profile for UpdateUser feature mapping
/// </summary>
public class UpdateUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateUser
    /// </summary>
    public UpdateUserProfile()
    {
        CreateMap<User, UpdateUserResult>();
    }
}
