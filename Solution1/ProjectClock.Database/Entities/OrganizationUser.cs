using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectClock.Database.Entities
{
    public class OrganizationUser
    {
        public Position Role { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public bool AcceptedInvitation { get; set; }
    }
}
