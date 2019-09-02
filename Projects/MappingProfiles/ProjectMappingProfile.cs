using AutoMapper;
using TPC.Api.Model;
using TPC.Api.Projects.Dto;

namespace TPC.Api.Projects.MappingProfiles
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project, ProjectDto>();
        }
    }
}
