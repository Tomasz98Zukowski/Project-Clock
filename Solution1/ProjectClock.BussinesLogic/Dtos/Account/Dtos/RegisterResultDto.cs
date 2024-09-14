using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectClock.BusinessLogic.Dtos.AccountDtos
{
    public class RegisterResultDto
    {
        public bool RegistrationFailed { get; set; }
        public bool EmailAlreadyInUse { get; set; }
    }
}
