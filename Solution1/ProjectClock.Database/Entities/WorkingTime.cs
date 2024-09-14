using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectClock.Database.Entities
{
    public class WorkingTime
    {
        public int Id { get; set; }
        public Project? Project { get; set; }
        public int ProjectId { get; set; }
        public User? User { get; set; }
        public int UserId { get; set; }
        public DateTime? StartTime { get; set; } = DateTime.UtcNow;
        public DateTime? EndTime { get; set; }
        public string? Description { get; set; }

        public bool IsFinished => EndTime != null;

       
    }
}
