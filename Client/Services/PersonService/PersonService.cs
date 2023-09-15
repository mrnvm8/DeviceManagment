namespace DeviceManagment.Client.Services.PersonService
{
    public class PersonService : IPersonService
    {
        private readonly HttpClient _http;
        public List<PersonResponse> People { get; set; } = new List<PersonResponse>();
        
        public PersonService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ServiceResponse<bool>> AddPerson(PersonResponse person)
        {
            var result = await _http.PostAsJsonAsync("api/person", person);
            return await ReturnMessage(result);
        }

        public async Task GetPeopleList()
        {
           
            var result = await _http.GetAsync("api/person");
            if (result.StatusCode != System.Net.HttpStatusCode.InternalServerError)
            {
                var response = await result.Content.ReadFromJsonAsync<ServiceResponse<List<PersonResponse>>>();
                People = response.Data;
            }
            else
            {
                People = new List<PersonResponse>();
            }
        }

        public async Task<ServiceResponse<PersonResponse>> GetPersonById(string personId)
        {
            var response = await _http.GetAsync($"api/person/{personId}");
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return new ServiceResponse<PersonResponse>() { Message = $"The Id : {personId} does not exist." };
            }
            else
            {
                return await response.Content.ReadFromJsonAsync<ServiceResponse<PersonResponse>>();
            }
        }

        public async Task<ServiceResponse<bool>> RemovePerson(string personId)
        {
            var result = await _http.DeleteAsync($"api/person/{personId}");
            return await ReturnMessage(result);
        }

        public async Task<ServiceResponse<bool>> UpdatePerson(PersonResponse person)
        {
            var result = await _http.PutAsJsonAsync($"api/person/{person.PersonId}", person);
            return await ReturnMessage(result);
        }

        private static async Task<ServiceResponse<bool>> ReturnMessage(HttpResponseMessage result)
        {
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }
    }
}
