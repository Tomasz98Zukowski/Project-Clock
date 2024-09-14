namespace ProjectClock.Database.Entities
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<OrganizationUser>? OrganizationUsers { get; set; } = new List<OrganizationUser>();
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
