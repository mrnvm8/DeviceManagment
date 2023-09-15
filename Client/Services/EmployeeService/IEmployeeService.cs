namespace DeviceManagment.Client.Services.EmployeeService
{
    public interface IEmployeeService
    {
        List<EmployeeResponse> Employees { get; set; }
        Task GetEmployeeList();
        Task<ServiceResponse<EmployeeResponse>> GetEmployeeById(string EmployeeId);
        Task<ServiceResponse<bool>> AddEmployee(EmployeeResponse employee);
        Task<ServiceResponse<bool>> UpdateEmployee(EmployeeResponse employee);
        Task<ServiceResponse<bool>> RemoveEmployee(string EmployeeId);
    }
}
