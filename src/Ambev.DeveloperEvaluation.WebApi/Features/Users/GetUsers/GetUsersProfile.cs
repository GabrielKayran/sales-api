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
        
        // Mapeamento customizado para GetUsersResult -> GetUsersResponse
        CreateMap<GetUsersResult, GetUsersResponse>()
            .ConstructUsing(src => new GetUsersResponse(
                src.Data.Select(u => new GetUsersUserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Username = u.Username,
                    Password = u.Password,
                    Name = new GetUsersUserNameDto
                    {
                        Firstname = u.Name.Firstname,
                        Lastname = u.Name.Lastname
                    },
                    Address = new GetUsersUserAddressDto
                    {
                        City = u.Address.City,
                        Street = u.Address.Street,
                        Number = u.Address.Number,
                        Zipcode = u.Address.Zipcode,
                        Geolocation = new GetUsersUserGeolocationDto
                        {
                            Lat = u.Address.Geolocation.Lat,
                            Long = u.Address.Geolocation.Long
                        }
                    },
                    Phone = u.Phone,
                    Status = u.Status,
                    Role = u.Role
                }).ToList(),
                src.TotalItems,
                src.CurrentPage,
                src.Data.Count
            ));
        
        CreateMap<UserDto, GetUsersUserDto>();
        CreateMap<UserNameDto, GetUsersUserNameDto>();
        CreateMap<UserAddressDto, GetUsersUserAddressDto>();
        CreateMap<UserGeolocationDto, GetUsersUserGeolocationDto>();
    }
}
