using ProjectClock.BusinessLogic.Dtos.Organization;
using ProjectClock.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectClock.BusinessLogic.Dtos.Raport
{
    public class GetNameAndIdAllUsersDto
    {
    
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }

        public List<Database.Entities.User>? ListUsersForRaports { get; set; }

        public List<Database.Entities.WorkingTime>? List { get; set; } //= new List<Database.Entities.WorkingTime>();

        [Required(ErrorMessage = "Please provide your birthdate")]
        [DataType(DataType.Date)]
        public DateTime StartData { get; set; }

        public DateTime StopData { get; set; }











    }
}
