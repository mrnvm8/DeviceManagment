namespace DeviceManagment.Server.Services.DeviceLoanService
{
    public interface IDeviceLoanService : IDisposable
    {
        Task<ServiceResponse<List<DeviceLoanResponse>>> GetDeviceLoansList();
        Task<ServiceResponse<DeviceLoanResponse>> GetDeviceLoanById(string deviceId);
        Task<ServiceResponse<bool>> AssignDevice(DeviceLoanResponse deviceLoan);
        Task<ServiceResponse<bool>> UnassignDevice(string deviceId, DeviceLoanResponse deviceLoan);
        Task<ServiceResponse<List<DeviceHistoryResponse>>> HistoryOfDevice(string deviceId);
    }
}
