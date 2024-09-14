using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace ProjectClock.BusinessLogic.Dtos.AccountDtos
{
    public class LoginResultDto
    {

        public bool LoginFailed { get; set; }
        public bool AccountActive { get; set; }
        public bool UserExist { get; set; }

        public int? UserId { get; set; }

        public AuthenticationProperties? AuthProp { get; set; }

        public ClaimsIdentity? ClaimsIdentity { get; set; }
    }
}
