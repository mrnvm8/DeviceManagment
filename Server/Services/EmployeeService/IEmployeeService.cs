namespace DeviceManagment.Server.Services.EmployeeService
{
    public interface IEmployeeService : IDisposable
    {
        Task<ServiceResponse<PagePagination<EmployeeResponse>>> GetPagination(int page);
        Task<ServiceResponse<List<EmployeeResponse>>> GetEmployees();
        Task<ServiceResponse<EmployeeResponse>> GetEmployeeById(string EmployeeId);
        Task<ServiceResponse<bool>> AddEmployee(EmployeeResponse employee);
        Task<ServiceResponse<bool>> UpdateEmployee(string EmployeeId, EmployeeResponse employee);
        Task<ServiceResponse<bool>> RemoveEmployee(string EmployeeId);
    }
}
