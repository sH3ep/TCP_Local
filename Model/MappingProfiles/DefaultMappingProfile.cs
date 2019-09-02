using AutoMapper;

namespace TPC.Api.Model.MappingProfiles
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<Comment, Comment>();
            CreateMap<Feature, Feature>();
            CreateMap<Project, Project>();
            CreateMap<Sprint, Sprint>();
            CreateMap<TaskItem, TaskItem>();
            CreateMap<UserStory, UserStory>();
        }
    }
}
