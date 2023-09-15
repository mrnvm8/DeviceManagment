namespace DeviceManagment.Server.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string email, string password);
        Task<ServiceResponse<bool>> ChangePassword(string newPassword, int id);
        int GetUserId();
        int GetEmployeeId();
        string GetRole();
    }
}
