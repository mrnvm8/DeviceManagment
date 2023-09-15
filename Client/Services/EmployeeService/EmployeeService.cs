using DeviceManagment.Shared;
using System.Net.Http.Json;

namespace DeviceManagment.Client.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient _http;

        public EmployeeService(HttpClient http)
        {
            _http = http;
        }

        public List<EmployeeResponse> Employees { get; set; } = new List<EmployeeResponse>();

        public async Task<ServiceResponse<bool>> AddEmployee(EmployeeResponse employee)
        {
            var result = await _http.PostAsJsonAsync("api/employee", employee);
            return await ReturnResponse(result);
        }

        public async Task<ServiceResponse<EmployeeResponse>> GetEmployeeById(string EmployeeId)
        {
            var response = await _http.GetAsync($"api/employee/{EmployeeId}");
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return new ServiceResponse<EmployeeResponse>() { Message = $"The Id : {EmployeeId} does not exist." };
            }
            else
            {
                return await response.Content.ReadFromJsonAsync<ServiceResponse<EmployeeResponse>>();
            }
        }

        public async Task GetEmployeeList()
        {
            var result = await _http.GetAsync("api/employee");
            if (result.StatusCode != System.Net.HttpStatusCode.InternalServerError)
            {
                var response = await result.Content.ReadFromJsonAsync<ServiceResponse<List<EmployeeResponse>>>();
               Employees = response.Data;
            }
            else
            {
                Employees = new List<EmployeeResponse>();
            }
        }

        public async Task<ServiceResponse<bool>> RemoveEmployee(string EmployeeId)
        {
            var result = await _http.DeleteAsync($"api/employee/{EmployeeId}");
            return await ReturnResponse(result);
        }

        public async Task<ServiceResponse<bool>> UpdateEmployee(EmployeeResponse employee)
        {
            var result = await _http.PutAsJsonAsync($"api/employee/{employee.EmplyeeId}", employee);
            return await ReturnResponse(result);
        }
        private static async Task<ServiceResponse<bool>> ReturnResponse(HttpResponseMessage result)
        {
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }
    }
}
