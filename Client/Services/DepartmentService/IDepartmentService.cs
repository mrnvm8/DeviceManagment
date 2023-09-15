using DeviceManagment.Shared;

namespace DeviceManagment.Client.Services.DepartmentService
{
    public interface IDepartmentService
    {
        public List<DepartmentResponse> Departments { get; set; }
        Task GetDepartmentList();
        Task<ServiceResponse<DepartmentResponse>> GetDepartmentById(string departId);
        Task<ServiceResponse<bool>> AddDepartment(DepartmentResponse department);
        Task<ServiceResponse<bool>> UpdateDepartment(DepartmentResponse department);
        Task<ServiceResponse<bool>> RemoveDepartment(string departId);
    }
}
