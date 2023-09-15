namespace DeviceManagment.Server.Services.DeviceLoanService
{
    public class DeviceLoanService : IDeviceLoanService
    {
        private readonly DataContext _context;
        private readonly IHashids _hashids;
        private readonly IAuthService _authService;
        private bool disposed = false;

        public DeviceLoanService(DataContext context,
            IHashids hashids,
            IAuthService authService)
        {
            _context = context;
            _hashids = hashids;
            _authService = authService;
        }

        public async Task<ServiceResponse<bool>> AssignDevice(DeviceLoanResponse deviceLoan)
        {
            var response = new ServiceResponse<bool>();

            var _loan = new DeviceLoans
            {
                UserId = _authService.GetUserId(),
                DeviceId = _hashids.Decode(deviceLoan.DeviceId)[0],
                EmployeeId = _hashids.Decode(deviceLoan.EmployeeId)[0],
                AssignedDate = DateTime.Now,
                ReturnDate = DateTime.Parse("2009-01-01"),
                IsApproved = deviceLoan.IsApproved,
            };

            _context.DeviceLoans?.Add(_loan);
            await _context.SaveChangesAsync();
            response.Message = "Device was successful Assigned.";

            return response;
        }

        public async Task<ServiceResponse<DeviceLoanResponse>> GetDeviceLoanById(string deviceId)
        {
            var response = new ServiceResponse<DeviceLoanResponse>();
            //constant date
            var date = DateTime.Parse("2009-01-01");
            //get raw Id
            var rawId = _hashids.Decode(deviceId)[0];

            var _loan = await _context.DeviceLoans
                            .Include(_device => _device.Device)
                                .ThenInclude(d => d.Department)
                                    .ThenInclude(off => off.Offices)
                            .Include(_user => _user.User)
                            .Include(emp => emp.Employee)
                                .ThenInclude(per => per.Person)
                            .FirstOrDefaultAsync(d => d.DeviceId
                            .Equals(rawId) && d.ReturnDate.Equals(date));

            if (_loan is null)
            {
                response.Success = false;
                response.Message = "Sorry , but this device does not exist on loan.";
            }
            else
            {
                response.Data = new DeviceLoanResponse
                {
                    LoanId = _hashids.Encode(_loan.Id),
                    DeviceId = _hashids.Encode(_loan.DeviceId),
                    EmployeeId = _hashids.Encode(_loan.EmployeeId),
                    AssignedDate = _loan.AssignedDate,
                    IsApproved = _loan.IsApproved,
                    EmployeeName = $"{_loan.Employee?.Person?.FirstName}, " +
                                 $"{_loan.Employee?.Person?.LastName}"
                };
            }

            return response;
        }

        public async Task<ServiceResponse<List<DeviceLoanResponse>>> GetDeviceLoansList()
        {
            var response = new ServiceResponse<List<DeviceLoanResponse>>();
            var _loanResponse = new List<DeviceLoanResponse>();

            var _loan = await _context.DeviceLoans
                            .Include(_device => _device.Device)
                                .ThenInclude(d => d.Department)
                                    .ThenInclude(off => off.Offices)
                            .Include(_user => _user.User)
                            .Include(emp => emp.Employee)
                                .ThenInclude(per => per.Person)
                            .ToListAsync();

            if (_loan.Count == 0)
            {
                response.Success = false;
                response.Message = "Sorry , but list of device loans does not exist.";
            }
            else
            {
                _loan.ForEach(loan =>
                _loanResponse.Add(new DeviceLoanResponse
                {
                    LoanId = _hashids.Encode(loan.Id),
                    DeviceId = _hashids.Encode(loan.DeviceId),
                    EmployeeId = _hashids.Encode(loan.EmployeeId),
                    AssignedDate = loan.AssignedDate,
                    IsApproved = loan.IsApproved,
                }));

                response.Data = _loanResponse;
            }

            return response;

        }

        public async Task<ServiceResponse<bool>> UnassignDevice(string deviceId, DeviceLoanResponse deviceLoan)
        {
            var response = new ServiceResponse<bool>();
            //get raw Id
            var rawId = _hashids.Decode(deviceId)[0];
            //constant date
            var date = DateTime.Parse("2009-01-01");
            //get raw Id
            var _loan = await _context.DeviceLoans
                            .FirstOrDefaultAsync(d => d.DeviceId
                            .Equals(rawId) && d.ReturnDate.Equals(date));


            if (_loan is null)
            {
                response.Success = false;
                response.Message = "Sorry, Device is not assigned, try again";
            }
            else
            {
                _loan.ReturnToUserId = _authService.GetUserId();
                _loan.ReturnDate = DateTime.Now;
                _loan.IsApproved = true;

                _context.DeviceLoans.Update(_loan);
                await _context.SaveChangesAsync();
                response.Message = "Device was successful Unassigned.";
            }
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

        public async Task<ServiceResponse<List<DeviceHistoryResponse>>> HistoryOfDevice(string deviceId)
        {
            var response = new ServiceResponse<List<DeviceHistoryResponse>>();
            var _history = new List<DeviceHistoryResponse>();
            //get the deviceId
            var rawId = _hashids.Decode(deviceId)[0];

            //get all loan history of the device
            var _deviceLoans = await _context.DeviceLoans
                            .Include(d => d.Device)
                            .Include(e => e.Employee)
                                .ThenInclude(p => p.Person)
                            .Where(d => d.DeviceId
                            .Equals(rawId))
                            .ToListAsync();

            if (_deviceLoans.Count == 0 || _deviceLoans is null)
            {
                response.Message = "This device does not have history yet.";
                response.Success = false;
            }
            else
            {
                _deviceLoans.ForEach(his =>
                {
                    _history.Add(new DeviceHistoryResponse
                    {
                        PurchasedDate = his.Device!.PurchasedDate,
                        PurchasedPrice = his.Device!.PurchasedPrice,
                        DeviceName = his.Device!.DeviceName,
                        EmployeeName = $"{his.Employee?.Person?.FirstName}," +
                                       $"{his.Employee?.Person?.LastName}",
                        AssignedDate = his.AssignedDate,
                        UnassignedDate = his.ReturnDate,
                        CurrentValuePrice = CalcDepreciation(his.Device.PurchasedPrice, his.Device.PurchasedDate)
                    });
                });
                response.Data = _history;
            }

            return response;
        }

        private decimal CalcDepreciation(decimal purchasedPrice, DateTime purchasedDate)
        {
            const decimal _percentage = 0.333m;
            var _passedYear = DateTime.Now.Year - purchasedDate.Year;
            var _depreciationPrice = (purchasedPrice - (purchasedPrice * _percentage)) / 3;

            for (int i = 1; i<_passedYear; i++)
            {
                purchasedPrice = purchasedPrice - _depreciationPrice;
                if (i == 3) break;
            }
            return purchasedPrice;
        }
    }
}
