using DeviceManagment.Shared.Charts;

namespace DeviceManagment.Server.Services.DeviceService
{
    public interface IDeviceService: IDisposable
    {
        Task<ServiceResponse<List<DeviceResponse>>> GetDeviceListByType(string typeId);
        Task<ServiceResponse<List<DeviceResponse>>> GetDevicesList();
        Task<ServiceResponse<DeviceResponse>> GetDeviceById(string deviceId);
        Task<ServiceResponse<bool>> AddDevice(DeviceResponse device);
        Task<ServiceResponse<bool>> UpdateDevice(DeviceResponse device);
        Task<ServiceResponse<bool>> RemoveDevice(string deviceId);


        //Methods for Charts
        Task<ServiceResponse<List<DataItem>>> BoughtDeviceByYears();
    }
}
