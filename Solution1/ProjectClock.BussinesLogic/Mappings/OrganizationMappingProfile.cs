using AutoMapper;
using ProjectClock.BusinessLogic.Dtos.Organization;
using ProjectClock.Database.Entities;

namespace ProjectClock.BusinessLogic.Mappings
{
    public class OrganizationMappingProfile : Profile
    {
        public OrganizationMappingProfile()
        {
            CreateMap<CreateOrganizationDto, Organization>();
            CreateMap<Organization,OrganizationDto>()
                .ForMember(dest => dest.OrganizationId, opt => opt.MapFrom(org => org.Id))
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(org => org.Name));
        }
    }
}