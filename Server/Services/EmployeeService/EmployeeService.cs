namespace DeviceManagment.Server.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DataContext _context;
        private readonly IHashids _hashids;
        private bool disposed = false;
        public EmployeeService(DataContext context,
            IHashids hashids)
        {
            _context = context;
            _hashids = hashids;
        }

        public async Task<ServiceResponse<bool>> AddEmployee(EmployeeResponse employee)
        {
            var response = new ServiceResponse<bool>();
            var dbEmployee = new Employee
            {
                PersonId = _hashids.Decode(employee.PersonId)[0],
                DepartmentId = _hashids.Decode(employee.DepartmentId)[0],
                WorkEmail = employee.Email
            };

            //checking if employee exist base personId
            if (await EmployeeExist(dbEmployee))
            {
                response.Success = false;
                response.Message = "Sorry, Employee already exist.";
            }
            else
            {
                _context.Employees?.Add(dbEmployee);
                await _context.SaveChangesAsync();
                response.Message = $"Employee suceessfuly added";
            }

            return response;
        }

        public async Task<ServiceResponse<EmployeeResponse>> GetEmployeeById(string EmployeeId)
        {
            var response = new ServiceResponse<EmployeeResponse>();
            //Converting EmployeeId To integer Id
            var rawId = _hashids.Decode(EmployeeId)[0];

            //Getting the employee from the database
            var dbEmployee = await _context.Employees
                            .Include(d => d.Department)
                                .ThenInclude(o => o.Offices)
                            .Include(p => p.Person)
                            .SingleOrDefaultAsync(e => e.EmployeeId
                            .Equals(rawId));


            //Checking if employee is found
            if (dbEmployee is null)
            {
                response.Success = false;
                response.Message = "Sorry, this employee does not exist.";
            }
            else
            {
                response.Data = new EmployeeResponse
                {
                    EmplyeeId = _hashids.Encode(dbEmployee.EmployeeId),
                    PersonId = _hashids.Encode(dbEmployee.PersonId),
                    DepartmentId = _hashids.Encode(dbEmployee.DepartmentId),
                    IsActive = dbEmployee.IsEmployeeActive,
                    FullName = $"{dbEmployee.Person?.FirstName}, " +
                               $"{dbEmployee.Person?.LastName}",
                    DepartmentName = dbEmployee.Department?.DepartmentName,
                    OfficeName = dbEmployee.Department?.Offices?.OfficeName,
                    Email = dbEmployee.WorkEmail
                };
            }

            return response;
        }

        public async Task<ServiceResponse<List<EmployeeResponse>>> GetEmployees()
        {
            var response = new ServiceResponse<List<EmployeeResponse>>();
            var employeesList = new List<EmployeeResponse>();

            //get all the employees 
            var employees = await _context.Employees
                            .Include(d => d.Department)
                                .ThenInclude(o => o.Offices)
                            .Include(p => p.Person)
                            .ToListAsync();
            //check if there are existing employees
            if (employees.Count == 0)
            {
                response.Success = false;
                response.Message = "Sorry , but list of employees does not exist.";
            }
            else
            {
                HashIdFromDatabase(employeesList, employees);
                response.Data = employeesList;
            }
            return response;
        }

        private void HashIdFromDatabase(List<EmployeeResponse> employee, List<Employee> employees)
        {
            employees.ForEach(emp =>
            {
                employee.Add(new EmployeeResponse
                {
                    EmplyeeId = _hashids.Encode(emp.EmployeeId),
                    IsActive = emp.IsEmployeeActive,
                    FullName = $"{emp.Person?.FirstName}, " +
                               $"{emp.Person?.LastName}",
                    DepartmentName = emp.Department?.DepartmentName,
                    OfficeName = emp.Department?.Offices?.OfficeName,
                    Email = emp.WorkEmail
                });
            });
        }

        public async Task<ServiceResponse<PagePagination<EmployeeResponse>>> GetPagination(int page)
        {
            var response = new ServiceResponse<PagePagination<EmployeeResponse>>();
            var responseEmployee = new List<EmployeeResponse>();
            // //Number of result to be displayed per page
            var pageResults = 5f;
            //get the list of employees
            //var employees = (await GetEmployees()).Data; => this statment works only when there is data
            var employees = await _context.Employees.ToListAsync();

            //How many number of pages are there
            var pageCount = Math.Ceiling(employees.Count / pageResults);

            //getting the employee list and skip pages
            var employeesList = await _context.Employees
                            .Include(d => d.Department)
                                .ThenInclude(o => o.Offices)

                            .Include(p => p.Person)
                            .Skip((page - 1) * (int)pageResults)
                            .Take((int)pageResults)
                            .OrderBy(p => p.PersonId)
                            .ToListAsync();


            //check if there are existing employees
            if (employeesList.Count == 0)
            {
                response.Success = false;
                response.Message = "Sorry , but list of employees does not exist.";
            }
            else
            {
                HashIdFromDatabase(responseEmployee, employeesList);
                response.Data = new PagePagination<EmployeeResponse>
                {
                    Pagination = responseEmployee,
                    CurrentPage = page,
                    Pages = (int)pageCount
                };
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> RemoveEmployee(string EmployeeId)
        {
            var response = new ServiceResponse<bool>();
            //Converting EmployeeId To integer Id
            var rawId = _hashids.Decode(EmployeeId)[0];

            var employee = await _context.Employees
                        .SingleOrDefaultAsync(e => e.EmployeeId
                        .Equals(rawId));

            if (employee is null)
            {
                response.Success = false;
                response.Message = $"Employee with {EmployeeId} not found";
            }
            else
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                response.Message = $"Successfully removing the employee with Id {EmployeeId}";
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateEmployee(string EmployeeId, EmployeeResponse employee)
        {
            var response = new ServiceResponse<bool>();
            //Converting EmployeeId To integer Id
            var rawId = _hashids.Decode(EmployeeId)[0];
            //getting an employee from the db
            var dbEmployee = await _context.Employees.FindAsync(rawId);

            if (dbEmployee is null)
            {
                response.Success = false;
                response.Message = $"Employee with Id {EmployeeId} not found";
            }
            else
            {
                dbEmployee.WorkEmail = employee.Email;
                dbEmployee.PersonId = _hashids.Decode(employee.PersonId)[0];
                dbEmployee.DepartmentId = _hashids.Decode(employee.DepartmentId)[0];
                dbEmployee.IsEmployeeActive = employee.IsActive;

                _context.Employees.Update(dbEmployee);
                await _context.SaveChangesAsync();
                response.Message = "Successfully update information ";
            }
            return response;
        }

        private async Task<bool> EmployeeExist(Employee employee)
        {

            if (employee is not null)
            {
                var sameEmployee = await _context.Employees
                .AnyAsync(e => e.PersonId
                .Equals(employee.PersonId));

                if (sameEmployee)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
