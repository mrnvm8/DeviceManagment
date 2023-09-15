

namespace DeviceManagment.Client.Services.DeviceService
{
    public interface IDeviceService
    {
        public List<DeviceResponse> Devices { get; set; }
        Task GetDeviceList(string? deviceTypeId);
        Task<ServiceResponse<DeviceResponse>> GetDeviceById(string deviceId);
        Task<ServiceResponse<bool>> AddDevice(DeviceResponse device);
        Task<ServiceResponse<bool>> UpdateDevice(DeviceResponse device);
        Task<ServiceResponse<bool>> RemoveDevice(string deviceId);
        Task<ServiceResponse<List<DataItem>>> GetDeviceByBoughtYear();
    }
}
