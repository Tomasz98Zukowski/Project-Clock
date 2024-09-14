using ProjectClock.BusinessLogic.Dtos.Account.Dtos;
using ProjectClock.BusinessLogic.Dtos.AccountDtos;

namespace ProjectClock.BusinessLogic.Services.AccountServices
{
    public interface IAccountServices
    {
        Task<bool> DeleteAccount(DeleteAccountDto dto);
        Task<EditEmailResultDto> EditAccountEmail(EditEmailDto dto);
        Task<EditPasswordResultDto> EditAccountPassword(EditPasswordDto dto);
        Task<string> GetAccountEmail(int id);
        Task<AccountDto> GetAccountDetails(int id);
        Task<LoginResultDto> LoginAccount(LoginDto dto);
        Task<RegisterResultDto> RegisterAccount(RegisterDto dto);
        Task<int> GetUserIdFromAccountId(int id);
        Task<bool> ChangeUserStatus(ActiveAccountDto dto);
    }
}