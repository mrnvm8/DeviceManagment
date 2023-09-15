namespace DeviceManagment.Server.Services.DeviceTypeService
{
    public class DeviceTypeService : IDeviceTypeService
    {
        private readonly DataContext _context;
        private readonly IHashids _hashids;
        private readonly IDeviceService _deviceService;
        private bool disposed = false;


        public DeviceTypeService(DataContext context,
            IHashids hashids,
            IDeviceService deviceService)
        {
            _context = context;
            _hashids = hashids;
            _deviceService = deviceService;
        }
        public async Task<ServiceResponse<bool>> AddDeviceType(DeviceTypeResponse deviceType)
        {
            var response = new ServiceResponse<bool>();
            var type = new DeviceType()
            {
                Name = deviceType.Name,
                Description = deviceType.Description,
            };

            if (await TypeExist(type))
            {
                response.Success = false;
                response.Message = "Sorry, The type name exists.";
            }
            else
            {
                _context.DeviceTypes?.Add(type);
                await _context.SaveChangesAsync();
                response.Message = "Device type was successful added.";
            }
            return response;
        }

        public async Task<ServiceResponse<DeviceTypeResponse>> GetDeviceTypeById(string typeId)
        {
            var response = new ServiceResponse<DeviceTypeResponse>();
            var newId = _hashids.Decode(typeId)[0];

            var types = await _context.DeviceTypes
                        .SingleOrDefaultAsync(t => t.Id
                        .Equals(newId));

            if (types is null)
            {
                response.Success = false;
                response.Message = "Sorry , but this type name does not exist.";
            }
            else
            {
                response.Data = new DeviceTypeResponse
                {
                    Id = _hashids.Encode(types.Id),
                    Name = types.Name,
                    Description = types.Description,
                };
            }

            return response;
        }

        public async Task<ServiceResponse<List<DeviceTypeResponse>>> GetDeviceTypeList()
        {
            var response = new ServiceResponse<List<DeviceTypeResponse>>();
            var typesResponse = new List<DeviceTypeResponse>();

            var TypeList = await _context.DeviceTypes.ToListAsync();

            if (TypeList.Count == 0)
            {
                response.Success = false;
                response.Message = "Sorry , but list of device type does not exist..";
            }
            else
            {
                HashIdFromDatabase(typesResponse, TypeList);
                response.Data = typesResponse;
            }

            return response;
        }

        private async Task<int> CountDevice(int typeId)
        {
            var device = await _deviceService.GetDeviceListByType(_hashids.Encode(typeId));
            var count = 0;
            if(device is not null && device.Data is not null)
            {
                count = device.Data.Count();
            }
            return count;
        }

        private void HashIdFromDatabase(List<DeviceTypeResponse> typesResponse, List<DeviceType> TypeList)
        {


            TypeList.ForEach(type =>
            {
                typesResponse.Add(new DeviceTypeResponse
                {
                    Id = _hashids.Encode(type.Id),
                    Name = type.Name,
                    Description = type.Description,
                    Total =CountDevice(type.Id).Result,
                });
            });
        }
        public async Task<ServiceResponse<PagePagination<DeviceTypeResponse>>> GetPagination(int page)
        {
            var response = new ServiceResponse<PagePagination<DeviceTypeResponse>>();
            var typesResponse = new List<DeviceTypeResponse>();
            //Number of result per page
            var pageResults = 5f;
            var types = await _context.DeviceTypes.ToListAsync();
            //How many number of pages are there
            var pageCount = Math.Ceiling(types.Count / pageResults);

            //getting the department list and skip pages
            var TypeList = await _context.DeviceTypes
                            .Skip((page - 1) * (int)pageResults)
                            .Take((int)pageResults)
                            .ToListAsync();


            if (TypeList.Count == 0)
            {
                response.Success = false;
                response.Message = "Sorry , but list of device type does not exist..";
            }
            else
            {
                HashIdFromDatabase(typesResponse, TypeList);
                response.Data = new PagePagination<DeviceTypeResponse>
                {
                    Pagination = typesResponse,
                    CurrentPage = page,
                    Pages = (int)pageCount
                };
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> RemoveDeviceType(string typeId)
        {
            var response = new ServiceResponse<bool>();
            var newId = _hashids.Decode(typeId)[0];

            var type = await _context.DeviceTypes
                        .SingleOrDefaultAsync(t => t.Id
                        .Equals(newId));

            if (type is null)
            {
                response.Success = false;
                response.Message = $"Device type name with {typeId} not found.";
            }
            else
            {
                _context.DeviceTypes.Remove(type);
                await _context.SaveChangesAsync();
                response.Message = "Device type was successful removed.";
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateDeviceType(string typeId, DeviceTypeResponse deviceType)
        {
            var response = new ServiceResponse<bool>();
            var newId = _hashids.Decode(typeId)[0];

            var dbType = await _context.DeviceTypes.FindAsync(newId);

            if (dbType is null)
            {
                response.Success = false;
                response.Message = "Device type not found.";
            }
            else
            {
                dbType.Name = (char.ToUpper(deviceType.Name[0])
              + deviceType.Name.ToLower().Substring(1)).Trim();
                dbType.Description = deviceType.Description;

                _context.DeviceTypes.Update(dbType);
                await _context.SaveChangesAsync();
                response.Message = "Device type was updated successfully.";
            }
            return response;
        }

        private async Task<bool> TypeExist(DeviceType type)
        {
            var officeExist = await _context.DeviceTypes
                .AnyAsync(o => o.Name.ToLower()
                .Equals(type.Name.ToLower()));

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
