namespace DeviceManagment.Client.Services.DeviceTypeService
{
    public class DeviceTypeService : IDeviceTypeService
    {
        private readonly HttpClient _http;

        public List<DeviceTypeResponse> DeviceTypes { get; set; } = new List<DeviceTypeResponse>();
        public DeviceTypeService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ServiceResponse<bool>> AddDeviceType(DeviceTypeResponse type)
        {
            var result = await _http.PostAsJsonAsync("api/devicetype", type);
            return await ReturnMessage(result);
        }

        public async Task<ServiceResponse<DeviceTypeResponse>> GetDeviceTypeById(string typeId)
        {
            var response = await _http.GetAsync($"api/devicetype/{typeId}");
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return new ServiceResponse<DeviceTypeResponse>() { Message = $"The Id : {typeId} does not exist." };
            }
            else
            {
                return await response.Content.ReadFromJsonAsync<ServiceResponse<DeviceTypeResponse>>();
            }
        }

        public async Task GetDeviceTypes()
        {
            var result = await _http.GetAsync("api/devicetype");
            if (result.StatusCode != System.Net.HttpStatusCode.InternalServerError)
            {
                var response = await result.Content.ReadFromJsonAsync<ServiceResponse<List<DeviceTypeResponse>>>();
                DeviceTypes = response.Data;
            }
            else
            {
                DeviceTypes = new List<DeviceTypeResponse>();
            }
        }

        public async Task<ServiceResponse<bool>> RemoveDeviceType(string typeId)
        {
            var result = await _http.DeleteAsync($"api/devicetype/{typeId}");
            return await ReturnMessage(result);
        }

        public async Task<ServiceResponse<bool>> UpdateDeviceType(DeviceTypeResponse type)
        {
            var result = await _http.PutAsJsonAsync($"api/devicetype/{type.Id}", type);
            return await ReturnMessage(result);
        }

        private async Task<ServiceResponse<bool>> ReturnMessage(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
            if (response != null)
            {
                return response;
            }
            return null;
        }
    }
}
