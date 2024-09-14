using AutoMapper;
using ProjectClock.BusinessLogic.Dtos.Project.Dtos;
using ProjectClock.BusinessLogic.Dtos.Project.ProjectDtos;
using ProjectClock.Database.Entities;

namespace ProjectClock.BusinessLogic.Mappings
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Organization, opt => opt.MapFrom(src => src.Organization.Name));

            CreateMap<Project, ProjectWithAccessLevelDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Organization, opt => opt.MapFrom(src => src.Organization.Name));
            
        }
    }
}
