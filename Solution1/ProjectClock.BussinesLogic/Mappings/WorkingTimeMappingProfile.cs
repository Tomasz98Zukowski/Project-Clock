using AutoMapper;
using ProjectClock.BusinessLogic.Dtos.WorkingTime.WorkingTimeDtos;
using ProjectClock.Database.Entities;

namespace ProjectClock.BusinessLogic.Mapping
{
    public class WorkingTimeMappingProfile : Profile
    {
        public WorkingTimeMappingProfile()
        {
            CreateMap<WorkingTime, WorkingTimeDto>()
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
            .ForMember(dest => dest.IsFinished, opt => opt.MapFrom(src => src.IsFinished))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(dest => dest.Id))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(dest => dest.Description));           
        }
    }
}
