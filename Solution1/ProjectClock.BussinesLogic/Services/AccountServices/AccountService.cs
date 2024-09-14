using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using ProjectClock.BusinessLogic.Dtos.Account.Dtos;
using ProjectClock.BusinessLogic.Dtos.AccountDtos;
using ProjectClock.BusinessLogic.Email.Models.Email;
using ProjectClock.BusinessLogic.Services.EmailHostedServices;
using ProjectClock.BusinessLogic.Services.UserServices;
using ProjectClock.Database;
using ProjectClock.Database.Entities;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using User = ProjectClock.Database.Entities.User;

namespace ProjectClock.BusinessLogic.Services.AccountServices
{


    public class AccountService : IAccountServices
    {
        private readonly ProjectClockDbContext _dbContext;
        private readonly IUserServices _userService;
        private readonly IEmailHostedServices _emailHostedServices;

        public AccountService(ProjectClockDbContext dbContext, IUserServices userServices, EmailHostedServices.EmailHostedServices emailHostedServices)
        {
            _dbContext = dbContext;
            _userService = userServices;
            _emailHostedServices = emailHostedServices;
        }

        public async Task<RegisterResultDto> RegisterAccount(RegisterDto dto)
        {
            var resultDto = new RegisterResultDto();

            if (await _dbContext.Accounts
                    .Select(u => u.Email)
                    .ContainsAsync(dto.Email))
            {
                resultDto.EmailAlreadyInUse = true;
                resultDto.RegistrationFailed = true;
            }
            else
            {
                resultDto.EmailAlreadyInUse = false;
            }

            if (resultDto.RegistrationFailed)
            {
                return resultDto;
            }

            var salt = GeneratePasswordSalt();
            var passwordHash = GetHashedPassword(dto.Password, salt);
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);


            if (user == null)
            {
                await _userService.Create(new User(dto.FirstName, dto.LastName, dto.Email));
                user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Name == dto.FirstName && u.Email == dto.Email);
            }
            else
            {
                user.Name = dto.FirstName;
                user.Surname = dto.LastName;
                user.IsActive = true;
            }

            

            var newAccount = new Account
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordSalt = salt,
                PasswordHash = passwordHash,
                User = user,
                ActivationCode = GenerateCode(),
                IsActive = false
            };

            string emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <title>Twój kod weryfikacyjny</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            background-color: #245d6c;
            padding: 20px;
            color: #ffffff;
        }}
        .container {{
            max-width: 600px;
            margin: auto;
            background-color: #248eb7;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }}
        h1 {{
            color: #ffffff;
            border-bottom: 2px solid #ffffff;
            padding-bottom: 10px;
        }}
        p {{
            margin-top: 15px;
            margin-bottom: 15px;
            color: #ffffff; /* Ustawienie koloru tekstu na biały */
        }}
        strong {{
            color: #ffffff;
        }}
        .footer {{
            margin-top: 20px;
            border-top: 1px solid #ffffff;
            padding-top: 10px;
            font-size: 0.9em;
        }}
    </style>
</head>
<body>

    <div class='container'>
        
        <h1>Witaj!</h1>
        
        <p>Oto Twój <strong>5-znakowy kod weryfikacyjny</strong>:</p>
        
        <p style='font-size: 24px; font-weight: bold;'>{newAccount.ActivationCode}</p>
        
        <p>Proszę użyć tego kodu do weryfikacji.</p>
        
        <div class='footer'>
            <p>Pozdrawiam,<br>
            Tomasz Żukowski - Project Clock</p>
        </div>

    </div>

</body>
</html>";


            await _emailHostedServices.SendMailAsync(new EmailModel()
            {
                EmailAdress = dto.Email,
                Subject = "Project clock - Activation code",
                Body = emailBody
            });
            await _dbContext.Accounts.AddAsync(newAccount);
            await _dbContext.SaveChangesAsync();

            return resultDto;
        }

        public async Task<LoginResultDto> LoginAccount(LoginDto dto)
        {
            var user = await _dbContext.Accounts
                .Where(u => u.Email == dto.Email)
                .Select(u => new { u.Id, u.PasswordHash, u.PasswordSalt, name = $"{u.FirstName} {u.LastName}", u.IsActive})
                .FirstOrDefaultAsync();

            var resultDto = new LoginResultDto();

            if (user is null)
            {
                resultDto.UserExist = false;
                resultDto.LoginFailed = true;

                return resultDto;
            }

            if (user.PasswordHash != GetHashedPassword(dto.Password, user.PasswordSalt))
            {
                resultDto.LoginFailed = true;
                resultDto.UserExist = true;

                return resultDto;
            }
            if (!user.IsActive)
            {
                resultDto.LoginFailed = true;
                resultDto.UserExist = true;
                resultDto.AccountActive = false;

                return resultDto;
            }

            resultDto.LoginFailed = false;
            resultDto.UserExist = true;
            resultDto.UserId = user.Id;
            resultDto.ClaimsIdentity = GetClaimsIdentity(user.Id, user.name);
            resultDto.AuthProp = GetAuthProp(dto.RememberMe);

            return resultDto;
        }

        public async Task<string> GetAccountEmail(int id)
        {
            var email = await _dbContext.Accounts
                .Where(u => u.Id == id)
                .Select(u => u.Email)
                .FirstAsync();

            return email;
        }

        public async Task<EditEmailResultDto> EditAccountEmail(EditEmailDto dto)
        {
            var user = await _dbContext.Accounts.Include(u => u.User).FirstAsync(u => u.Id == dto.Id);

            var resultDto = new EditEmailResultDto();
            if (dto.CurrentEmail != user.Email)
            {
                resultDto.CurrentEmailIsIncorrect = true;
                resultDto.EditEmailFailed = true;
                return resultDto;
            }

            if (dto.NewEmail == user.Email)
            {
                resultDto.EditEmailFailed = true;
                resultDto.NewEmailIsCurrentEmail = true;
                return resultDto;
            }
            if (dto.NewEmail != dto.NewEmailRepeat)
            {
                resultDto.EditEmailFailed = true;
                resultDto.EmailsArentEqual = true;
                return resultDto;
            }

            if (await _dbContext.Accounts
                    .Select(u => u.Email)
                    .ContainsAsync(dto.NewEmail))
            {
                resultDto.EditEmailFailed = true;
                resultDto.NewEmailAlreadyInUse = true;
                return resultDto;
            }

            user.Email = dto.NewEmail;
            user.User.Email = dto.NewEmail;
            await _dbContext.SaveChangesAsync();

            return resultDto;
        }

        public async Task<EditPasswordResultDto> EditAccountPassword(EditPasswordDto dto)
        {
            var user = await _dbContext.Accounts.FirstAsync(u => u.Id == dto.UserId);

            var resultDto = new EditPasswordResultDto();

            if (user.PasswordHash != GetHashedPassword(dto.CurrentPassword, user.PasswordSalt))
            {

                resultDto.EditPasswordFailed = true;
                resultDto.WrongCurrentPassword = true;
            }

            if (dto.NewPassword == dto.CurrentPassword)
            {
                resultDto.EditPasswordFailed = true;
                resultDto.NewPasswordIsOldPassword = true;
            }

            if (dto.NewPassword != dto.ConfirmNewPassword)
            {

                resultDto.EditPasswordFailed = true;
                resultDto.PasswordsAreEqual = false;
            }
            else
            {
                resultDto.PasswordsAreEqual = true;
            }

            if (resultDto.EditPasswordFailed)
            {
                return resultDto;
            }

            var salt = GeneratePasswordSalt();
            var passwordHash = GetHashedPassword(dto.NewPassword, salt);

            user.PasswordSalt = salt;
            user.PasswordHash = passwordHash;
            await _dbContext.SaveChangesAsync();

            return resultDto;
        }

        public async Task<bool> DeleteAccount(DeleteAccountDto dto)
        {
            var account = await _dbContext.Accounts
                .FirstAsync(u => u.Id == dto.Id);

            if (account.PasswordHash != GetHashedPassword(dto.Password, account.PasswordSalt))
            {
                return false;
            }

            _dbContext.Accounts.Remove(account);

            var userId = await GetUserIdFromAccountId(dto.Id);

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            user.IsActive = false;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        private static byte[] GeneratePasswordSalt()
        {
            return RandomNumberGenerator.GetBytes(128 / 8);
        }

        private static string GetHashedPassword(string password, byte[] salt)
        {
            var hash = KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                100_000,
                256 / 8);

            return Convert.ToBase64String(hash);
        }

        private static AuthenticationProperties GetAuthProp(bool rememberMe)
        {
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = rememberMe
            };

            return authProperties;
        }
        private static ClaimsIdentity GetClaimsIdentity(int userId, string name)
        {
            var claims = new List<Claim>
        {
            new(ClaimTypes.Role, "User"),
            new(ClaimTypes.Name, name),
            new("UserId", userId.ToString())
        };

            return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<AccountDto> GetAccountDetails(int id)
        {
            var account = await _dbContext.Accounts
                .Where(u => u.Id == id)
                .Select(u => new AccountDto { FirstName = u.FirstName, LastName = u.LastName, Email = u.Email })
                .FirstOrDefaultAsync();
            return account;
        }

        public async Task<int> GetUserIdFromAccountId(int id)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(e => e.Id == id) ?? new Account() { };
            var userId = account.UserId;

            return userId;
        }

        public async Task<bool> ChangeUserStatus(ActiveAccountDto dto)
        {
            var user = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Email == dto.Email);
            if (user != null)
            {
                if(user.ActivationCode == dto.Code)
                {
                    user.IsActive = true;
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        private string GenerateCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
