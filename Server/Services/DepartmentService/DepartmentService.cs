namespace DeviceManagment.Server.Services.DepartmentService
{
    public class DepartmentService : IDepartmentService
    {
        private readonly DataContext _context;
        private readonly IHashids _hashids;
        private readonly IAuthService _authService;
        private bool disposed = false;
        public DepartmentService(DataContext context,
            IHashids hashids, IAuthService authService)
        {
            _context = context;
            _hashids = hashids;
            _authService = authService;
        }
        public async Task<ServiceResponse<bool>> AddDepartment(DepartmentResponse department)
        {
            var response = new ServiceResponse<bool>();
            var dbDepartment = new Department()
            {
                OfficeId = _hashids.Decode(department.OfficeId)[0],
                DepartmentName = department.DepartmentName,
                Description = department.Description,
            };

            if (await DepartmentExist(dbDepartment))
            {
                response.Success = false;
                response.Message = "Sorry, this department exists.";
            }
            else
            {
                _context.Departments?.Add(dbDepartment);
                await _context.SaveChangesAsync();
                response.Message = "Department was Successful added.";
            }
            return response;
        }

        public async Task<ServiceResponse<DepartmentResponse>> GetDepartmentById(string departmentId)
        {
            var response = new ServiceResponse<DepartmentResponse>();
            //changing the string id to interger
            var rawId = _hashids.Decode(departmentId)[0];
            var department = await _context.Departments
                            .Include(o => o.Offices)
                            .SingleOrDefaultAsync(d => d.DepartmentId
                            .Equals(rawId));

            if (department is null)
            {
                response.Success = false;
                response.Message = "Sorry , but this department does not exist.";
                return response;
            }
            else
            {
                response.Data = new DepartmentResponse
                {
                    DepartId = _hashids.Encode(department.DepartmentId),
                    DepartmentName = department.DepartmentName,
                    OfficeId = _hashids.Encode(department.OfficeId),
                    OfficeName = department.Offices?.OfficeName,
                    Location = department.Offices?.Location,
                    Description = department.Description
                };
            }

            return response;
        }

        public async Task<ServiceResponse<List<DepartmentResponse>>> GetDepartmentList()
        {
            var response = new ServiceResponse<List<DepartmentResponse>>();
            var departmentDTO = new List<DepartmentResponse>();
            var departments = new List<Department>();
            //already know who logged into the system
            var Role = _authService.GetRole();

            //get the list of every program enrolled
            var employee = await _context.Employees
                            .Where(d => d.EmployeeId
                            .Equals(_authService.GetEmployeeId()))
                            .ToListAsync();
            if (Role.Contains("admin"))
            {
                departments = await _context.Departments
                            .Include(o => o.Offices)
                            .ToListAsync();
            }
            else
            {
                foreach (var emp in employee)
                {
                    var _depart = await _context.Departments
                            .Include(o => o.Offices)
                            .Where(d => d.DepartmentId
                            .Equals(emp.DepartmentId))
                            .ToListAsync();

                    departments.AddRange(_depart);
                }
            }
            

            if (departments.Count == 0)
            {
                response.Success = false;
                response.Message = "Sorry , but list of departments does not exist.";
            }
            else
            {
                HashIdsFromDatabase(departmentDTO, departments);
                response.Data = departmentDTO;
            }
            return response;
        }

        private void HashIdsFromDatabase(List<DepartmentResponse> departmentDTO, List<Department>? departments)
        {
            departments.ForEach(depart =>
            {
                departmentDTO.Add(new DepartmentResponse
                {
                    DepartId = _hashids.Encode(depart.DepartmentId),
                    DepartmentName = depart.DepartmentName,
                    OfficeId = _hashids.Encode(depart.OfficeId),
                    OfficeName = depart.Offices?.OfficeName,
                    Location = depart.Offices?.Location,
                    Description = depart.Description
                });
            });
        }
        public async Task<ServiceResponse<PagePagination<DepartmentResponse>>> GetPagination(int page)
        {
            var response = new ServiceResponse<PagePagination<DepartmentResponse>>();
            var departmentDTO = new List<DepartmentResponse>();

            //Number of result per page
            var pageResults = 5f;
            var depart = await _context.Departments.ToListAsync();
            //How many number of pages are there
            var pageCount = Math.Ceiling(depart.Count / pageResults);

            //getting the department list and skip pages
            var departments = await _context.Departments
                            .Include(o => o.Offices)
                            .Skip((page - 1) * (int)pageResults)
                            .Take((int)pageResults)
                            .ToListAsync();

            if (departments.Count == 0)
            {
                response.Success = false;
                response.Message = "Sorry , but list of departments does not exist.";
            }
            else
            {
                HashIdsFromDatabase(departmentDTO, departments);
                response.Data = new PagePagination<DepartmentResponse>
                {
                    Pagination = departmentDTO,
                    CurrentPage = page,
                    Pages = (int)pageCount
                };
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> RemoveDepartment(string departmentId)
        {
            var response = new ServiceResponse<bool>();
            var newId = _hashids.Decode(departmentId)[0];
            var depart = await _context.Departments
                        .SingleOrDefaultAsync(d => d.DepartmentId
                        .Equals(newId));

            if (depart is null)
            {
                response.Success = false;
                response.Message = $"Department with {departmentId} not found.";
            }
            else
            {
                _context.Departments.Remove(depart);
                await _context.SaveChangesAsync();
                response.Message = "Department was removed successful.";
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateDepartment(string departmentId, DepartmentResponse department)
        {
            var response = new ServiceResponse<bool>();
            var newId = _hashids.Decode(departmentId)[0];

            var dbDepartment = await _context.Departments.FindAsync(newId);

            if (dbDepartment is null)
            {
                response.Success = false;
                response.Message = "Department not found.";
            }
            else
            {
                dbDepartment.DepartmentName = department.DepartmentName;
                dbDepartment.OfficeId = _hashids.Decode(department.OfficeId)[0];
                dbDepartment.Description = department.Description;

                _context.Departments.Update(dbDepartment);
                await _context.SaveChangesAsync();
                response.Message = "Department was updated successful.";
            }

            return response;
        }

        private async Task<bool> DepartmentExist(Department department)
        {
            var depart = await _context.Departments
                .AnyAsync(d => d.DepartmentName
                .ToLower().Equals(department.DepartmentName.ToLower()));

            if (depart)
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
