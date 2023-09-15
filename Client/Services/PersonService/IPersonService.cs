namespace DeviceManagment.Client.Services.PersonService
{
    public interface IPersonService
    {
        List<PersonResponse> People { get; set; }
        Task GetPeopleList();
        Task<ServiceResponse<PersonResponse>> GetPersonById(string personId);
        Task<ServiceResponse<bool>> AddPerson(PersonResponse person);
        Task<ServiceResponse<bool>> UpdatePerson(PersonResponse person);
        Task<ServiceResponse<bool>> RemovePerson(string personId);
    }
}
