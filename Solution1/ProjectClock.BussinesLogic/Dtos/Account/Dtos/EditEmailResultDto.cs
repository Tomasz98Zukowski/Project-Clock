using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectClock.BusinessLogic.Dtos.AccountDtos
{
    public class EditEmailResultDto
    {
        public bool EditEmailFailed { get; set; }
        public bool NewEmailIsCurrentEmail { get; set; }
        public bool NewEmailAlreadyInUse { get; set; }
        public bool CurrentEmailIsIncorrect { get; set; }
        public bool EmailsArentEqual { get; set; }
    }
}
