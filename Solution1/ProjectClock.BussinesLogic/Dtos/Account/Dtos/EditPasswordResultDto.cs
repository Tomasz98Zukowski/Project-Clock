using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectClock.BusinessLogic.Dtos.AccountDtos
{
    public class EditPasswordResultDto
    {
        public bool EditPasswordFailed { get; set; }
        public bool WrongCurrentPassword { get; set; }
        public bool NewPasswordIsOldPassword { get; set; }
        public bool PasswordsAreEqual { get; set; }
    }
}
