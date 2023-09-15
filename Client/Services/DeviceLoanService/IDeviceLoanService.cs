namespace DeviceManagment.Client.Services.DeviceLoanService
{
    public interface IDeviceLoanService
    {
        List<DeviceLoanResponse> DeviceLoans { get; set; }
        string Message { get; set; }
        Task GetDeviceLoanList();
        Task<ServiceResponse<List<DeviceHistoryResponse>>> GetHistory(string deviceId);
        Task<ServiceResponse<DeviceLoanResponse>> GetDeviceLoanById(string deviceId);
        Task<ServiceResponse<bool>> AssignDevice(DeviceLoanResponse deviceLoans);
        Task<ServiceResponse<bool>> UnassignDevice(DeviceLoanResponse deviceLoans);
    }
}
