namespace DeviceManagment.Client.Services.DeviceTypeService
{
    public interface IDeviceTypeService
    {
       
        List<DeviceTypeResponse> DeviceTypes { get; set; }
        Task GetDeviceTypes();
        Task<ServiceResponse<DeviceTypeResponse>> GetDeviceTypeById(string typeId);
        Task<ServiceResponse<bool>> AddDeviceType(DeviceTypeResponse type);
        Task<ServiceResponse<bool>> UpdateDeviceType(DeviceTypeResponse type);
        Task<ServiceResponse<bool>> RemoveDeviceType(string typeId);
    }
}
