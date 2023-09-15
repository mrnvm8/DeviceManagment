using DeviceManagment.Shared.Responses;
using DeviceManagment.Shared.Auth;

namespace DeviceManagment.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> Registration(UserRegister request);
        Task<ServiceResponse<string>> Login(UserLogin request);
        //Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request, int? id);
        //Task<ServiceResponse<bool>> RemoveUser(int id);
        Task<bool> IsUserAuthenticated();
    }
}
