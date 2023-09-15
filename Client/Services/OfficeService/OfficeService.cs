namespace DeviceManagment.Client.Services.OfficeService
{
    public class OfficeService : IOfficeService
    {
        private readonly HttpClient _http;
        public List<OfficeResponse> Offices { get; set; } = new List<OfficeResponse>();

        public OfficeService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ServiceResponse<bool>> AddOffice(OfficeResponse office)
        {
            var result = await _http.PostAsJsonAsync("api/office", office);
            return await ReturnMessage(result);
        }

        public async Task<ServiceResponse<OfficeResponse>> GetOfficeById(string officeId)
        {
            var response = await _http.GetAsync($"api/office/{officeId}");
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return new ServiceResponse<OfficeResponse>() { Message = $"The Id : {officeId} does not exist." };
            }
            else
            {
                return await response.Content.ReadFromJsonAsync<ServiceResponse<OfficeResponse>>();
            }
        }

        public async Task GetOfficeList()
        {
            var result = await _http.GetAsync("api/office");
            if (result.StatusCode != System.Net.HttpStatusCode.InternalServerError)
            {
                var response = await result.Content.ReadFromJsonAsync<ServiceResponse<List<OfficeResponse>>>();
                Offices = response.Data;
            }
            else
            {
                Offices = new List<OfficeResponse>();
            }
        }


        public async Task<ServiceResponse<bool>> RemoveOffice(string officeId)
        {
            var result = await _http.DeleteAsync($"api/office/{officeId}");
            return await ReturnMessage(result);
        }

        public async Task<ServiceResponse<bool>> UpdateOffice(OfficeResponse office)
        {
            var result = await _http.PutAsJsonAsync($"api/office/{office.OfficeId}", office);
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
