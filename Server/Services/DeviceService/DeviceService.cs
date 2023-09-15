using DeviceManagment.Shared.Charts;

namespace DeviceManagment.Server.Services.DeviceService
{
    public class DeviceService : IDeviceService
    {
        private bool disposed = false;
        private readonly DataContext _context;
        private readonly IHashids _hashids;
        private readonly IDeviceLoanService _deviceLoan;
        private readonly IAuthService _authService;

        public DeviceService(DataContext context,
            IHashids hashids, 
            IDeviceLoanService deviceLoan,
            IAuthService authService)
        {
            _context = context;
            _hashids = hashids;
            _deviceLoan = deviceLoan;
            _authService = authService;
           
        }
        public async Task<ServiceResponse<bool>> AddDevice(DeviceResponse device)
        {
            var response = new ServiceResponse<bool>();
            var _device = new Device()
            {
                DepartmentId = _hashids.Decode(device.DepartmentId)[0],
                DeviceTypeId = _hashids.Decode(device.TypeId)[0],
                DeviceName = device.DeviceName,
                DeviceSerialNo = device.SerialNo,
                DeviceIMEINo = device.IMEINo,
                Condition = device.Condition,
                PurchasedPrice = device.PurchasedPrice,
                PurchasedDate = device.PurchasedDate,
            };

            if (await DeviceExist(_device))
            {
                response.Success = false;
                response.Message = "Sorry, Device already exist.";
            }
            else
            {
                _context.Devices?.Add(_device);
                await _context.SaveChangesAsync();
                response.Message = "Device was successful added.";
            }

            return response;
        }
        private async Task<bool> DeviceExist(Device device)
        {
            bool Exist = false;

            if (String.IsNullOrEmpty(device.DeviceSerialNo))
            {
                Exist = await _context.Devices
                    .AnyAsync(e => e.DeviceIMEINo
                    .Equals(device.DeviceIMEINo));
            }
            else
            {
                Exist = await _context.Devices
                   .AnyAsync(e => e.DeviceSerialNo
                   .Equals(device.DeviceSerialNo));
            }

            return Exist;
        }

        public async Task<ServiceResponse<DeviceResponse>> GetDeviceById(string deviceId)
        {
            var response = new ServiceResponse<DeviceResponse>();
            //get the Raw Id 
            var rawId = _hashids.Decode(deviceId)[0];
            //get the device from table base on Id
            var _device = await _context.Devices
                        .Include(d => d.Department)
                               .ThenInclude(o => o.Offices)
                        .Include(t => t.DeviceType)
                        .SingleOrDefaultAsync(e => e.DeviceId == rawId);

            if (_device is null)
            {
                response.Success = false;
                response.Message = "Sorry, device does not exist.";
            }
            else
            {
                response.Data = new DeviceResponse
                {
                    DeviceId = _hashids.Encode(_device.DeviceId),
                    DepartmentId = _hashids.Encode(_device.DepartmentId),
                    TypeId = _hashids.Encode(_device.DeviceTypeId),
                    DeviceName = _device.DeviceName,
                    SerialNo = _device.DeviceSerialNo,
                    IMEINo = _device.DeviceIMEINo,
                    Condition = _device.Condition,
                    PurchasedPrice = _device.PurchasedPrice,
                    PurchasedDate = _device.PurchasedDate,
                    Department = _device.Department.DepartmentName,
                    IdentityNumber = String.IsNullOrEmpty(_device.DeviceSerialNo) ?
                                 _device.DeviceIMEINo : _device.DeviceSerialNo,
                };
            }

            return response;
        }

        public async Task<ServiceResponse<List<DeviceResponse>>> GetDeviceListByType(string typeId)
        {
            var response = new ServiceResponse<List<DeviceResponse>>();
            var _PageList = new List<DeviceResponse>();
            var _devices = new List<Device>();
            //get the Raw device type Id
            var rawId = _hashids.Decode(typeId)[0];

            //already know who logged into the system
            var Role = _authService.GetRole();

            //get the list of every program enrolled
            var employee = await _context.Employees
                            .Where(d => d.EmployeeId
                            .Equals(_authService.GetEmployeeId()))
                            .ToListAsync();

            if (Role.Contains("admin")) 
            {
                //get list of device
                _devices = await _context.Devices
                                .Include(d => d.Department)
                                       .ThenInclude(o => o.Offices)
                                .Include(t => t.DeviceType)
                                .Where(t => t.DeviceTypeId
                                .Equals(rawId))
                                .ToListAsync();
            }
            else 
            {
                foreach (var depart in employee)
                {
                    var devices = await _context.Devices
                                .Include(d => d.Department)
                                       .ThenInclude(o => o.Offices)
                                .Include(t => t.DeviceType)
                                .Where(t => t.DeviceTypeId
                                .Equals(rawId) && t.DepartmentId
                                .Equals(depart.DepartmentId)
                                )
                                .ToListAsync();

                    _devices.AddRange(devices);
                }
            }


            if (_devices.Count == 0)
            {
                response.Success = false;
                response.Message = "Sorry , but list of device does not exist.";
            }
            else
            {
                DeviceResponseData(_PageList, _devices);
                response.Data = _PageList;
            }
            return response;
        }

        private void DeviceResponseData(List<DeviceResponse> _PageList, List<Device> _deviceList)
        {
            _deviceList.ForEach(_device => _PageList.Add(new DeviceResponse
            {
                DeviceId = _hashids.Encode(_device.DeviceId),
                TypeId = _hashids.Encode(_device.DeviceTypeId),
                DeviceName = (char.ToUpper(_device.DeviceName[0])
                            + _device.DeviceName.ToLower().Substring(1)),
                Condition = _device.Condition,
                Department = _device.Department.DepartmentName,
                Type = _device.DeviceType.Name,
                IdentityNumber = String.IsNullOrEmpty(_device.DeviceSerialNo) ?
                                 _device.DeviceIMEINo : _device.DeviceSerialNo,
                FullName = GetEmployeeName(_hashids.Encode(_device.DeviceId)).Result,
                PurchasedPrice = _device.PurchasedPrice,
                PurchasedDate = _device.PurchasedDate

            }));
        }

        public async Task<ServiceResponse<List<DeviceResponse>>> GetDevicesList()
        {
            var response = new ServiceResponse<List<DeviceResponse>>();
            var _responseList = new List<DeviceResponse>();
            var _devices = new List<Device>();

            //already know who logged into the system
            var Role = _authService.GetRole();

            //get the list of every program enrolled
            var employee = await _context.Employees
                            .Where(d => d.EmployeeId
                            .Equals(_authService.GetEmployeeId()))
                            .ToListAsync();


            if (Role.Contains("admin"))
            {
               _devices = await _context.Devices
                      .Include(d=> d.DeviceType)
                      .Include(d => d.Department)
                          .ThenInclude(o => o.Offices)
                      .Include(t => t.DeviceType)
                      .ToListAsync();
            }
            else
            {
                foreach(var depart in employee)
                {
                    var device = await _context.Devices
                        .Include(d => d.DeviceType)
                        .Include(d => d.Department)
                          .ThenInclude(o => o.Offices)
                        .Include(t => t.DeviceType)
                        .Where(d => d.DepartmentId
                        .Equals(depart.DepartmentId))
                        .ToListAsync();

                    _devices.AddRange(device);
                }
            }

            if (_devices.Count == 0)
            {
                response.Success = false;
                response.Message = "Sorry , but list of device does not exist.";
            }
            else
            {
                DeviceResponseData(_responseList, _devices);
                response.Data = _responseList;
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> RemoveDevice(string deviceId)
        {
            var response = new ServiceResponse<bool>();
            //get raw device Id
            var rawId = _hashids.Decode(deviceId)[0];
            //get the  device first
            var _device = await _context.Devices
                           .SingleOrDefaultAsync(d => d.DeviceId
                           .Equals(rawId));

            if (_device is null)
            {
                response.Success = false;
                response.Message = "Sorry, device does not exist.";
            }
            else
            {
                _context.Devices.Remove(_device);
                await _context.SaveChangesAsync();
                response.Message = "Device was successful deleted.";
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateDevice(DeviceResponse device)
        {
            var response = new ServiceResponse<bool>();
            var _device = new Device()
            {
                DepartmentId = _hashids.Decode(device.DepartmentId)[0],
                DeviceTypeId = _hashids.Decode(device.TypeId)[0],
                DeviceName = device.DeviceName,
                DeviceSerialNo = device.SerialNo,
                DeviceIMEINo = device.IMEINo,
                Condition = device.Condition,
                PurchasedPrice = device.PurchasedPrice,
                PurchasedDate = device.PurchasedDate,
            };

            //get raw id
            var rawId = _hashids.Decode(device.DeviceId)[0];

            var _dbDevice = await _context.Devices
                .SingleOrDefaultAsync(d => d.DeviceId
                .Equals(rawId));

            if (_dbDevice is null)
            {
                response.Success = false;
                response.Message = "Sorry, device does not exist;";
            }
            else
            {
                _dbDevice.DepartmentId = _device.DepartmentId;
                _dbDevice.DeviceTypeId = _device.DeviceTypeId;
                _dbDevice.DeviceName = (char.ToUpper(_device.DeviceName[0])
                                        + _device.DeviceName.ToLower().Substring(1));

                _dbDevice.DeviceSerialNo = _device.DeviceSerialNo;
                _dbDevice.DeviceIMEINo = _device.DeviceIMEINo;

                _dbDevice.Condition = _device.Condition;
                _dbDevice.PurchasedPrice = _device.PurchasedPrice;
                _dbDevice.PurchasedDate = _device.PurchasedDate;

                _context.Devices.Update(_dbDevice);
                await _context.SaveChangesAsync();
                response.Message = "Device was successful updated.";
            }

            return response;
        }

        private async Task<string> GetEmployeeName(string deviceId)
        {
            var _loaned = (await _deviceLoan.GetDeviceLoanById(deviceId)).Data;

            if (_loaned != null)
            {
                return _loaned.EmployeeName.ToUpper();
            }
            return string.Empty;
        }

        public async Task<ServiceResponse<List<DataItem>>> BoughtDeviceByYears()
        {
            var response = new ServiceResponse<List<DataItem>>();

            var DataList = new List<DataItem>();
            var Values = new List<ChartValues>();

            var result = from device in _context.Devices
                         group device by device.Year into dg
                         orderby dg.Key
                         select new
                         {
                             Year = dg.Key,
                             Items = dg.GroupBy(x => x.DeviceTypeId)
                                    .Select(x => new
                                    {
                                        Type = x.Key,
                                        Sum = x.Sum(x => x.PurchasedPrice),
                                    })
                         };


            foreach (var Year in result )
            {
                foreach (var items in Year.Items)
                {
                    DataList.Add(new DataItem
                    {
                        Year = Year.Year,
                        Items = new List<ChartValues>
                        {
                           new ChartValues
                           {
                               Type = items.Type,
                               Sum = items.Sum
                           }
                        }
                    });
                }
            }

            response.Data = DataList;

            return response;
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
