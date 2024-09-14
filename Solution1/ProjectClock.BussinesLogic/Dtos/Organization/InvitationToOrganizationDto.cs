namespace ProjectClock.BusinessLogic.Dtos.Organization
{
    public class InvitationToOrganizationDto
    {
        public ICollection<OrganizationDto> InvitingOrganizations { get; set; } = new List<OrganizationDto>();
    }
}
