namespace DeviceManagment.Client.Services.DeviceService
{
    public class DeviceService : IDeviceService
    {
        private readonly HttpClient _http;
        public List<DeviceResponse> Devices { get; set; } = new List<DeviceResponse>();
        public DeviceService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ServiceResponse<bool>> AddDevice(DeviceResponse device)
        {
            var result = await _http.PostAsJsonAsync("api/device", device);
            if (result.StatusCode.Equals(System.Net.HttpStatusCode.InternalServerError))
            {
                return new ServiceResponse<bool> { Success = false, Message = "Operation failed." };
            }
            return await RetrunResult(result);
        }

        public async Task<ServiceResponse<DeviceResponse>> GetDeviceById(string deviceId)
        {
            var response = await _http.GetAsync($"api/device/{deviceId}");
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return new ServiceResponse<DeviceResponse>() { Message = $"The Id : {deviceId} does not exist." };
            }
            else
            {
                return await response.Content.ReadFromJsonAsync<ServiceResponse<DeviceResponse>>();
            }
        }

        public async Task GetDeviceList(string? deviceTypeId)
        {

            var result = String.IsNullOrEmpty(deviceTypeId) ? await _http.GetAsync("api/device") :
                await _http.GetAsync($"api/device/bytype/{deviceTypeId}");
            if (result.StatusCode != System.Net.HttpStatusCode.InternalServerError)
            {
                var response = await result.Content.ReadFromJsonAsync<ServiceResponse<List<DeviceResponse>>>();
                Devices = response.Data;
            }
            else
            {
                Devices = new List<DeviceResponse>();
            }
        }

        public async Task<ServiceResponse<bool>> RemoveDevice(string deviceId)
        {
            var result = await _http.DeleteAsync($"api/device/{deviceId}");
            if (result.StatusCode.Equals(System.Net.HttpStatusCode.InternalServerError))
            {
                return new ServiceResponse<bool> { Success = false, Message = "Operation failed." };
            }
            return await RetrunResult(result);
        }

        public async Task<ServiceResponse<bool>> UpdateDevice(DeviceResponse device)
        {
            var result = await _http.PutAsJsonAsync("api/device", device);
            if (result.StatusCode.Equals(System.Net.HttpStatusCode.InternalServerError))
            {
                return new ServiceResponse<bool> { Success = false, Message = "Operation failed." };
            }
            return await RetrunResult(result);
        }

        private static async Task<ServiceResponse<bool>> RetrunResult(HttpResponseMessage result)
        {
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<List<DataItem>>> GetDeviceByBoughtYear()
        {
            var response = await _http.GetAsync("api/device/items");
            if (response.StatusCode != System.Net.HttpStatusCode.InternalServerError)
            {
                return await response.Content.ReadFromJsonAsync<ServiceResponse<List<DataItem>>>();

            }
            else
            {
                return new ServiceResponse<List<DataItem>>();
            }
        }
    }
}
