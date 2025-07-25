using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Users.GetUsers;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUsers;

/// <summary>
/// AutoMapper profile for GetUsers feature mapping between API and Application layers
/// </summary>
public class GetUsersProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUsers
    /// </summary>
    public GetUsersProfile()
    {
        CreateMap<GetUsersRequest, GetUsersQuery>();
        CreateMap<GetUsersResult, GetUsersResponse>();
        CreateMap<UserDto, GetUsersUserDto>();
        CreateMap<UserNameDto, GetUsersUserNameDto>();
        CreateMap<UserAddressDto, GetUsersUserAddressDto>();
        CreateMap<UserGeolocationDto, GetUsersUserGeolocationDto>();
    }
}
