using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectClock.Database.Entities
{
    public class UserProject
    {
        public int Id { get; set; }       
        public User User { get; set; }
        public int UserId { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
    }
}
