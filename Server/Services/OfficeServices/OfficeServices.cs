using DeviceManagment.Shared;
using DeviceManagment.Shared.Responses;

namespace DeviceManagment.Server.Services.OfficeServices
{
    public class OfficeServices: IOfficeServices
    {
        private readonly DataContext _context;
        private readonly IHashids _hashids;
        private bool disposed = false;
        public OfficeServices(DataContext context, IHashids hashids)
        {
            _context = context;
            _hashids = hashids;
        }

        public async Task<ServiceResponse<bool>> AddOffice(OfficeResponse office)
        {
            var response = new ServiceResponse<bool>();
            var dbOffices = new Office()
            {
                OfficeName = office.Name,
                Location = office.Location,
            };

            if (await OfficeExist(dbOffices))
            {
                response.Success = false;
                response.Message = "Sorry, The office name exists.";
            }
            else
            {
                _context.Offices?.Add(dbOffices);
                await _context.SaveChangesAsync();
                response.Message = "Office was successful added.";
            }
            return response;
        }

        public async Task<ServiceResponse<OfficeResponse>> GetOfficeById(string officeId)
        {
            var response = new ServiceResponse<OfficeResponse>();
            var newId = _hashids.Decode(officeId)[0];

            var office = await _context.Offices
                        .SingleOrDefaultAsync(x => x.OfficeId
                        .Equals(newId));

            if (office is null)
            {
                response.Success = false;
                response.Message = "Sorry , but this office name does not exist.";
            }
            else
            {
                response.Data = new OfficeResponse
                {
                    OfficeId = _hashids.Encode(office.OfficeId),
                    Name = office.OfficeName,
                    Location = office.Location,
                };
            }

            return response;
        }

        public async Task<ServiceResponse<List<OfficeResponse>>> GetOfficeList()
        {
            var response = new ServiceResponse<List<OfficeResponse>>();
            var officeList = new List<OfficeResponse>();

            var offices = await _context.Offices.ToListAsync();

            if (offices.Count == 0)
            {
                response.Success = false;
                response.Message = "Sorry , but list of offices does not exist.";
            }
            else
            {
                HasIdFromDatabase(officeList, offices);
                response.Data = officeList;
            }
            return response;
        }
        private void HasIdFromDatabase(List<OfficeResponse> officeList, List<Office>? offices)
        {
            offices.ForEach(x =>
            {
                officeList.Add(new OfficeResponse
                {
                    OfficeId = _hashids.Encode(x.OfficeId),
                    Name = x.OfficeName,
                    Location = x.Location,
                });
            });
        }
        public async Task<ServiceResponse<PagePagination<OfficeResponse>>> GetPagination(int page)
        {
            var response = new ServiceResponse<PagePagination<OfficeResponse>>();
            var office = new List<OfficeResponse>();
            //Number of result per page
            var pageResults = 5f;
            var offices = await _context.Offices.ToListAsync();
            //How many number of pages are there
            var pageCount = Math.Ceiling(offices.Count / pageResults);

            //getting the department list and skip pages
            var officeList = await _context.Offices
                            .Skip((page - 1) * (int)pageResults)
                            .Take((int)pageResults)
                            .ToListAsync();

            if (offices.Count == 0)
            {
                response.Success = false;
                response.Message = "Sorry , but list of offices does not exist.";
            }
            else
            {
                HasIdFromDatabase(office, officeList);

                response.Data = new PagePagination<OfficeResponse>
                {
                    Pagination = office,
                    CurrentPage = page,
                    Pages = (int)pageCount
                };
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> RemoveOffice(string officeId)
        {
            var response = new ServiceResponse<bool>();
            var newId = _hashids.Decode(officeId)[0];

            var office = await _context.Offices
                        .SingleOrDefaultAsync(x => x.OfficeId
                        .Equals(newId));

            if (office is null)
            {
                response.Success = false;
                response.Message = $"Office name with {officeId} not found.";
            }
            else
            {
                _context.Offices.Remove(office);
                await _context.SaveChangesAsync();
                response.Message = $"Office was removed successful.";
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateOffice(string officeId, OfficeResponse office)
        {
            var response = new ServiceResponse<bool>();
            var newId = _hashids.Decode(officeId)[0];

            var dbOffice = await _context.Offices
                           .FindAsync(newId);

            if (dbOffice is null)
            {
                response.Success = false;
                response.Message = "Office not found.";
            }
            else
            {
                dbOffice.OfficeName = office.Name;
                dbOffice.Location = office.Location;

                _context.Offices.Update(dbOffice);
                await _context.SaveChangesAsync();
                response.Message = $"Office was updated successful.";
            }
            return response;
        }

        private async Task<bool> OfficeExist(Office office)
        {
            var officeExist = await _context.Offices
                .AnyAsync(o => o.OfficeName.ToLower()
                .Equals(office.OfficeName.ToLower()));

            if (officeExist)
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
