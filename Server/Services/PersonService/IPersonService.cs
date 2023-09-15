namespace DeviceManagment.Server.Services.PersonService
{
    public interface IPersonService : IDisposable
    {
        Task<ServiceResponse<PagePagination<PersonResponse>>> GetPagination(int page);
        Task<ServiceResponse<List<PersonResponse>>> GetPeopleList();
        Task<ServiceResponse<PersonResponse>> GetPersonById(string personId);
        Task<ServiceResponse<bool>> AddPerson(PersonResponse person);
        Task<ServiceResponse<bool>> UpdatePerson(string personId, PersonResponse person);
        Task<ServiceResponse<bool>> RemovePerson(string personId);
    }
}
