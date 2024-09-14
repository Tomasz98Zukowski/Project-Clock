using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectClock.Database.Entities
{
    public class Account
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string PasswordHash { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }
        public string ActivationCode { get; set; }
        public bool IsActive { get; set; }

    }
}
