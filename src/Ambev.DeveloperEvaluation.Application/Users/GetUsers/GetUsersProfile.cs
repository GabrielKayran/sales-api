using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUsers;

/// <summary>
/// AutoMapper profile for GetUsers feature mapping
/// </summary>
public class GetUsersProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUsers
    /// </summary>
    public GetUsersProfile()
    {
        CreateMap<User, UserDto>();
    }
}
