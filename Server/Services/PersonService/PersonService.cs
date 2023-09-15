namespace DeviceManagment.Server.Services.PersonService
{
    public class PersonService : IPersonService
    {
        private bool disposed = false;
        private readonly DataContext _context;
        private readonly IHashids _hashids;

        public PersonService(DataContext context, IHashids hashids)
        {
            _context = context;
            _hashids = hashids;
        }

        public async Task<ServiceResponse<PagePagination<PersonResponse>>> GetPagination(int page)
        {
            var response = new ServiceResponse<PagePagination<PersonResponse>>();
            var personDTOList = new List<PersonResponse>();
            //Number of result per page
            var pageResults = 5f;
            var people = await _context.People.ToListAsync();
            //How many number of pages are there
            var pageCount = Math.Ceiling(people.Count / pageResults);

            //getting the person list and skip pages
            var peopleList = await _context.People
                            .Skip((page - 1) * (int)pageResults)
                            .Take((int)pageResults)
                            .ToListAsync();

            if (peopleList.Count ==0)
            {
                response.Success = false;
                response.Message = "Sorry , but list of employees does not exist.";
                return response;
            }
            else
            {
                HashIds(peopleList, personDTOList);
                response.Data = new PagePagination<PersonResponse>
                {
                    Pagination = personDTOList,
                    CurrentPage = page,
                    Pages = (int)pageCount
                };
            }

            return response;
        }
        private void HashIds(List<Person>? people, List<PersonResponse> personDTOList)
        {
            people?.ForEach(person =>
            {
                personDTOList.Add(new PersonResponse
                {
                    PersonId = _hashids.Encode(person.PersonId),
                    Name = person.FirstName,
                    Surname = person.LastName,
                    Gender = person.Gender,
                });
            });
        }
        public async Task<ServiceResponse<List<PersonResponse>>> GetPeopleList()
        {
            var personDTOList = new List<PersonResponse>();
            var response = new ServiceResponse<List<PersonResponse>>();
            //Getting all employee
            var people = await _context.People.ToListAsync();

            if (people.Count == 0)
            {
                response.Success = false;
                response.Message = "Sorry , but list of people does not exist.";
            }
            else
            {
                HashIds(people, personDTOList);
                response.Data = personDTOList;
            }
            return response;
        }

        public async Task<ServiceResponse<PersonResponse>> GetPersonById(string personId)
        {
            var response = new ServiceResponse<PersonResponse>();

            //Converting the string to an integer
            var newId = _hashids.Decode(personId)[0];

            var person = await _context.People
                        .SingleOrDefaultAsync(p => p.PersonId
                        .Equals(newId));


            if (person is null)
            {
                response.Success = false;
                response.Message = "Sorry , but this person does not exist.";
            }
            else
            {
                response.Data = new PersonResponse
                {
                    PersonId = _hashids.Encode(person.PersonId),
                    Name = person.FirstName,
                    Surname = person.LastName,
                    Gender = person.Gender,
                };
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> AddPerson(PersonResponse person)
        {
            var response = new ServiceResponse<bool>();

            var dbPerson = new Person()
            {
                FirstName = person.Name,
                LastName = person.Surname,
                Gender = person.Gender,
            };

            if (await PersonExist(dbPerson))
            {
                response.Success = false;
                response.Message = "Sorry, The person exist on the database.";
            }
            else
            {
                //Changing the format of the information
                //the first alphabetic will be uppcase while the rest will be lowercase
                FormatDataToUpperCase(dbPerson);

                //adding the person to the database if they don't exist
                _context.People?.Add(dbPerson);
                await _context.SaveChangesAsync();
                response.Message = "Person successfully Added.";
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> UpdatePerson(string personId, PersonResponse person)
        {
            var response = new ServiceResponse<bool>();
            //Converting the string to an integer
            var newId = _hashids.Decode(personId)[0];
            //getting the person by Id
            var dbPerson = await _context.People
                        .SingleOrDefaultAsync(p => p.PersonId.Equals(newId));

            if (dbPerson is null)
            {
                response.Success = false;
                response.Message = "Person not found.";
            }
            else
            {
                dbPerson.FirstName = person.Name;
                dbPerson.LastName = person.Surname;
                dbPerson.Gender = person.Gender;
                //Changing the format of the information
                //the first alphabetic will be uppcase while the rest will be lowercase
                FormatDataToUpperCase(dbPerson);

                _context.People.Update(dbPerson);
                await _context.SaveChangesAsync();
                response.Message = "Person updated successful.";
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> RemovePerson(string personId)
        {
            var response = new ServiceResponse<bool>();
            //Converting the string to an integer
            var newId = _hashids.Decode(personId)[0];
            //getting the person by Id
            var person = await _context.People
                        .SingleOrDefaultAsync(p => p.PersonId
                        .Equals(newId));

            if (person is null)
            {
                response.Success = false;
                response.Message = $"Person with {personId} not found.";
            }
            else
            {
                _context.People.Remove(person);
                await _context.SaveChangesAsync();
                response.Message = "Person removed successful.";
            }
            return response;
        }

        private void FormatDataToUpperCase(Person person)
        {
            person.FirstName = (char.ToUpper(person.FirstName[0])
                + person.FirstName.ToLower().Substring(1)).Trim();

            person.LastName = (char.ToUpper(person.LastName[0])
                + person.LastName.ToLower().Substring(1)).Trim();

        }

        private async Task<bool> PersonExist(Person person)
        {
            var personExist = await _context.People
                 .AnyAsync(p => p.FirstName.ToLower().Equals(person.FirstName.ToLower())
                 && p.LastName.ToLower().Equals(person.LastName.ToLower()));
            if (personExist)
            {
                return true;
            }
            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
