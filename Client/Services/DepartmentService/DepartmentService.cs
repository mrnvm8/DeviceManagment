using System.Net.Http.Json;

namespace DeviceManagment.Client.Services.DepartmentService
{
    public class DepartmentService : IDepartmentService
    {
        private readonly HttpClient _http;
        public List<DepartmentResponse> Departments { get; set; } = new List<DepartmentResponse>();

        public DepartmentService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ServiceResponse<bool>> AddDepartment(DepartmentResponse department)
        {
            var result = await _http.PostAsJsonAsync("api/department", department);
            return await ReturnResponse(result);
        }

        public async Task<ServiceResponse<DepartmentResponse>> GetDepartmentById(string departId)
        {
            var response = await _http.GetAsync($"api/department/{departId}");
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return new ServiceResponse<DepartmentResponse>() { Message = $"The Id : {departId} does not exist." };
            }
            else
            {
                return await response.Content.ReadFromJsonAsync<ServiceResponse<DepartmentResponse>>();
            }
        }

        public async Task GetDepartmentList()
        {
            var result = await _http.GetAsync("api/department");
            if (result.StatusCode != System.Net.HttpStatusCode.InternalServerError)
            {
                var response = await result.Content.ReadFromJsonAsync<ServiceResponse<List<DepartmentResponse>>>();
                Departments = response.Data;
            }
            else
            {
               Departments = new List<DepartmentResponse>();
            }
        }

        
        public async Task<ServiceResponse<bool>> RemoveDepartment(string departId)
        {
            var result = await _http.DeleteAsync($"api/department/{departId}");
            return await ReturnResponse(result);
        }
        public async Task<ServiceResponse<bool>> UpdateDepartment(DepartmentResponse department)
        {
            var result = await _http.PutAsJsonAsync($"api/department/{department.DepartId}", department);
            return await ReturnResponse(result);
        }

        private static async Task<ServiceResponse<bool>> ReturnResponse(HttpResponseMessage result)
        {
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }
    }
}
