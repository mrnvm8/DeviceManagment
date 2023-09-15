namespace DeviceManagment.Server.Services.OfficeServices
{
    public interface IOfficeServices : IDisposable
    {
        Task<ServiceResponse<PagePagination<OfficeResponse>>> GetPagination(int page);
        Task<ServiceResponse<List<OfficeResponse>>> GetOfficeList();
        Task<ServiceResponse<OfficeResponse>> GetOfficeById(string officeId);
        Task<ServiceResponse<bool>> AddOffice(OfficeResponse office);
        Task<ServiceResponse<bool>> UpdateOffice(string officeId, OfficeResponse office);
        Task<ServiceResponse<bool>> RemoveOffice(string officeId);
    }
}
