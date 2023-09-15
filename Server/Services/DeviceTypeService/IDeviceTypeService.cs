namespace DeviceManagment.Server.Services.DeviceTypeService
{
    public interface IDeviceTypeService : IDisposable
    {
        Task<ServiceResponse<List<DeviceTypeResponse>>> GetDeviceTypeList();
        Task<ServiceResponse<PagePagination<DeviceTypeResponse>>> GetPagination(int page);
        Task<ServiceResponse<DeviceTypeResponse>> GetDeviceTypeById(string typeId);
        Task<ServiceResponse<bool>> AddDeviceType(DeviceTypeResponse deviceType);
        Task<ServiceResponse<bool>> UpdateDeviceType(string typeId, DeviceTypeResponse deviceType);
        Task<ServiceResponse<bool>> RemoveDeviceType(string typeId);
    }
}
