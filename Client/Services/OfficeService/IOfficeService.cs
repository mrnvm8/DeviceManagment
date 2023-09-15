namespace DeviceManagment.Client.Services.OfficeService
{
    public interface IOfficeService
    {
        List<OfficeResponse> Offices { get; set; }
        Task GetOfficeList();
        Task<ServiceResponse<OfficeResponse>> GetOfficeById(string officeId);
        Task<ServiceResponse<bool>> AddOffice(OfficeResponse office);
        Task<ServiceResponse<bool>> UpdateOffice(OfficeResponse office);
        Task<ServiceResponse<bool>> RemoveOffice(string officeId);
    }
}
