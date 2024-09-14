using ProjectClock.BusinessLogic.Dtos.Organization;

namespace ProjectClock.BusinessLogic.Dtos.OrganizationDto
{
    public class ManageOrganizationDto
    {
        public int? SelectedOrganizationId { get; set; }
        public ICollection<int>? OrganizationIds { get; set; } = new List<int>();
        public string? OrganizationName { get; set; }
        public ICollection<string> OrganizationNames { get; set; }
        public ICollection<ChooseOrganizationDto> ChooseOrganizations { get; set; } = new List<ChooseOrganizationDto>();
        public ICollection<ChooseUserDto> ChooseUserDto { get; set; } = new List<ChooseUserDto>();
        public ICollection<string> OrganizationUserNames { get; set; } = new List<string>();

    }


}