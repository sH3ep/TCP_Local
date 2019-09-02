using AutoMapper;
using TPC.Api.Authentication.Dto;
using TPC.Api.Model;

namespace TPC.Api.Authentication.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterUserDto, User>();
        }
    }
}
