namespace DeviceManagment.Server.Services.DepartmentService
{
    public interface IDepartmentService : IDisposable
    {
        Task<ServiceResponse<PagePagination<DepartmentResponse>>> GetPagination(int page);
        Task<ServiceResponse<List<DepartmentResponse>>> GetDepartmentList();
        Task<ServiceResponse<DepartmentResponse>> GetDepartmentById(string departmentId);
        Task<ServiceResponse<bool>> AddDepartment(DepartmentResponse department);
        Task<ServiceResponse<bool>> UpdateDepartment(string departmentId, DepartmentResponse department);
        Task<ServiceResponse<bool>> RemoveDepartment(string departmentId);
    }
}
