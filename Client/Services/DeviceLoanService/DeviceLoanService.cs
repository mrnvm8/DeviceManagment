using DeviceManagment.Shared;

namespace DeviceManagment.Client.Services.DeviceLoanService
{
    public class DeviceLoanService : IDeviceLoanService
    {
        private readonly HttpClient _http;
        public List<DeviceLoanResponse> DeviceLoans { get; set; } = new List<DeviceLoanResponse>();
        public string Message { get; set; } = "Loading device loans list...";
        public List<DeviceHistoryResponse> _deviceHistory { get; set; }

        public DeviceLoanService(HttpClient http)
        {
            _http = http;
        }
        public async Task<ServiceResponse<bool>> AssignDevice(DeviceLoanResponse deviceLoans)
        {
            var result = await _http.PostAsJsonAsync("api/deviceloan", deviceLoans);
            return await ReturnResponse(result);
        }

        public async Task<ServiceResponse<DeviceLoanResponse>> GetDeviceLoanById(string deviceId)
        {
            var response = await _http.GetAsync($"api/deviceloan/{deviceId}");
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return new ServiceResponse<DeviceLoanResponse>() { Message = $"The Id : {deviceId} does not exist." };
            }
            else
            {
                return await response.Content.ReadFromJsonAsync<ServiceResponse<DeviceLoanResponse>>();
            }
        }

        public async Task GetDeviceLoanList()
        {
            var result = await _http.
                GetFromJsonAsync<ServiceResponse<List<DeviceLoanResponse>>>("api/deviceloan");
            if (result != null && result.Data != null)
            {
                DeviceLoans = result.Data;
            }
            if (DeviceLoans.Count == 0) Message = "No device loans list found.";
        }

        public async Task<ServiceResponse<bool>> UnassignDevice(DeviceLoanResponse deviceLoans)
        {
            var result = await _http.PutAsJsonAsync($"api/deviceloan/{deviceLoans.DeviceId}", deviceLoans);
            return await ReturnResponse(result);
        }
        private static async Task<ServiceResponse<bool>> ReturnResponse(HttpResponseMessage result)
        {
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }
        public async Task<ServiceResponse<List<DeviceHistoryResponse>>> GetHistory(string deviceId)
        {
            var response = await _http.GetAsync($"api/deviceloan/history/{deviceId}");
            if (response.StatusCode != System.Net.HttpStatusCode.InternalServerError)
            {
                return await response.Content.ReadFromJsonAsync<ServiceResponse<List<DeviceHistoryResponse>>>();

            }
            else
            {
                return new ServiceResponse<List<DeviceHistoryResponse>>();
            }
        }
    }
}
