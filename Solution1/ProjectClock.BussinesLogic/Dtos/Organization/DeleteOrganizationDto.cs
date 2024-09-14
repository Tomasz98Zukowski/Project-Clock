namespace ProjectClock.BusinessLogic.Dtos.Organization
{
    public class DeleteOrganizationDto
    {
        public ICollection<OrganizationDto> Organizations { get; set; } = new List<OrganizationDto>();
        
    }
}
