using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace DeviceManagment.Server.Services.TicketService
{
    public class TicketService : ITicketService
    {
        private bool disposed = false;
        private readonly DataContext _context;
        private readonly IHashids _hashids;
        private readonly IAuthService _authService;
        private readonly IEmployeeService _employee;
        private readonly IDeviceService _device;
        private readonly IConfiguration _config;

        public TicketService(DataContext context, IHashids hashids, IAuthService authService,
            IEmployeeService employee, IDeviceService device, IConfiguration config)
        {
            _context = context;
            _hashids = hashids;
            _authService = authService;
            _employee = employee;
            _device = device;
            _config = config;
        }

        public async Task<ServiceResponse<List<TicketResponse>>> GetTickets()
        {
            var response = new ServiceResponse<List<TicketResponse>>();
            var _responseTicket = new List<TicketResponse>();
            var tickets = new List<Ticket>();
            //already know who logged into the system
            var Role = _authService.GetRole();

            //get the list of every program enrolled
            var employee = await _context.Employees!
                            .Where(d => d.EmployeeId
                            .Equals(_authService.GetEmployeeId()))
                            .ToListAsync();

            if (Role.Contains("admin"))
            {
                tickets = await _context.Tickets!
                            .Include(t => t.Types)
                            .Include(u => u.Users)
                            .Include(d => d.Devices)
                                .ThenInclude(part => part!.Department)
                            .ToListAsync();
            }
            else
            {

                foreach (var depart in employee)
                {
                    var _tickets = await _context.Tickets!
                            .Include(t => t.Types)
                            .Include(u => u.Users)
                            .Include(d => d.Devices)
                                .ThenInclude(part => part!.Department)
                            .Where(d => d.Devices!.DepartmentId
                            .Equals(depart.DepartmentId))
                            .ToListAsync();

                    tickets.AddRange(_tickets);
                }

            }



            if (tickets is null || tickets.Count == 0)
            {
                response.Message = "No ticket exist yet.";
                response.Success = false;
            }
            else
            {
                tickets.ForEach(ticket =>
                _responseTicket.Add(new TicketResponse
                {
                    TicketId = _hashids.Encode(ticket.TicketId),
                    DeviceTypeId = _hashids.Encode(ticket.DeviceTypeId),
                    DeviceName = ticket.Devices!.DeviceName,
                    DeviceType = ticket.Types!.Name,
                    TicketTitle = ticket.TicketTitle,
                    TicketDate = ticket.TicketDate,
                    FixedDate = ticket.FixedDate,
                    Department = ticket.Devices?.Department?.DepartmentName,
                    ArangedDate = ticket.ArangedDate,
                    TicketYear = ticket.TicketYear,
                    TicketIssue = ticket.TicketIssue,
                    TicketUpdate = ticket.TicketUpdate,
                    IssueSolved = ticket.IssueSolved,
                    Updated = ticket.Updated,
                }));

                response.Data = _responseTicket;
            }

            return response;
        }

        public async Task<ServiceResponse<TicketResponse>> GetTicketById(string ticketId)
        {
            var response = new ServiceResponse<TicketResponse>();
            var rawId = _hashids.Decode(ticketId)[0];

            var ticket = await _context.Tickets!
                            .Include(t => t.Types)
                            .Include(u => u.Users)
                            .Include(d => d.Devices)
                                .ThenInclude(part => part!.Department)
                            .FirstOrDefaultAsync(x => x.TicketId
                            .Equals(rawId));

            if (ticket is null)
            {
                response.Message = "The tickets does not exist.";
                response.Success = false;
            }
            else
            {
                response.Data = new TicketResponse
                {
                    TicketId = _hashids.Encode(ticket.TicketId),
                    DeviceId = _hashids.Encode(ticket.DeviceId),
                    DeviceTypeId = _hashids.Encode(ticket.DeviceTypeId),
                    DeviceName = ticket.Devices!.DeviceName,
                    DeviceType = ticket.Types!.Name,
                    TicketTitle = ticket.TicketTitle,
                    TicketDate = ticket.TicketDate,
                    FixedDate = ticket.FixedDate,
                    ArangedDate = ticket.ArangedDate,
                    Department = ticket.Devices?.Department?.DepartmentName,
                    TicketIssue = ticket.TicketIssue,
                    TicketUpdate = ticket.TicketUpdate,
                    IssueSolved = ticket.IssueSolved,
                    TicketSolution = ticket.TicketSolution,
                    Updated = ticket.Updated,
                };
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateTicket(TicketResponse ticket, string ticketId)
        {
            var response = new ServiceResponse<bool>();
            var rawId = _hashids.Decode(ticketId)[0];

            var dbTicket = await _context.Tickets!
                            .FirstOrDefaultAsync(x => x.TicketId
                            .Equals(rawId));

            if (dbTicket is null)
            {
                response.Message = "The tickets does not exist.";
                response.Success = false;
            }
            else
            {
                dbTicket.DeviceId = _hashids.Decode(ticket.DeviceId)[0];
                dbTicket.TicketIssue = ticket.TicketIssue!;
                dbTicket.TicketTitle = ticket.TicketTitle!;

                _context.Tickets!.Update(dbTicket);
                await _context.SaveChangesAsync();
                response.Message = "Ticket information was updated.";
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> AddTicket(TicketResponse ticket)
        {
            var response = new ServiceResponse<bool>();

            var dbTicket = new Ticket
            {
                DeviceId = _hashids.Decode(ticket.DeviceId)[0],
                UserId = _authService.GetUserId(),
                DeviceTypeId = _hashids.Decode(ticket.DeviceTypeId)[0],
                TicketTitle = ticket.TicketTitle!,
                TicketIssue = ticket.TicketIssue!,
                TicketSolution = "Waiting to be updated.",
                ArangedDate = DateTime.Parse("2009-01-01"),
                FixedDate = DateTime.Parse("2009-01-01"),
                TicketYear = DateTime.Now.Year.ToString(),
            };

            if (await TicketExist(dbTicket))
            {
                response.Message = "Ticket with this Title exist, try a new name.";
                response.Success = false;
            }
            else
            {
                _context.Tickets?.Add(dbTicket);
                await _context.SaveChangesAsync();
                // sending and email to the tech
                SendTicketEmail(await ComposeEmailDetails(ticket));
                response.Message = "Ticket created successful.";
            }

            return response;
        }

        private async Task<bool> TicketExist(Ticket ticket)
        {
            var exist = await _context.Tickets!
                .AnyAsync(x => x.DeviceId.Equals(ticket.DeviceId)
                 && x.IssueSolved.Equals(false));

            if (exist)
            {
                return true;
            }
            return false;
        }

        public async Task<ServiceResponse<bool>> DeleteTicket(string ticketId)
        {
            var response = new ServiceResponse<bool>();
            var newId = _hashids.Decode(ticketId)[0];

            var ticket = await _context.Tickets!
                        .SingleOrDefaultAsync(x => x.TicketId
                        .Equals(newId));

            if (ticket is null)
            {
                response.Success = false;
                response.Message = $"ticket with {ticketId} not found.";
            }
            else
            {
                _context.Tickets!.Remove(ticket);
                await _context.SaveChangesAsync();
                response.Message = $"Ticket was removed successful.";
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> TicketArchived(TicketResponse ticket)
        {
            var response = new ServiceResponse<bool>();
            var rawId = _hashids.Decode(ticket.TicketId)[0];
            var emailContent = new EmailResponse();

            var dbTicket = await _context.Tickets!
                            .FirstOrDefaultAsync(x => x.TicketId
                            .Equals(rawId));
            var employee = (await _employee.GetEmployeeById(_hashids.Encode(_authService.GetEmployeeId()))).Data;

            if (dbTicket is null)
            {
                response.Message = "The tickets does not exist.";
                response.Success = false;
            }
            else
            {
                //get user, so we can get ticket creator
                var User = await _context.Users!
                                .Include(emp => emp.Employee)
                                    .ThenInclude(p => p!.Person)
                                .FirstOrDefaultAsync(x => x.UserId
                                .Equals(dbTicket.UserId));

                dbTicket.TicketSolution = ticket.TicketSolution;
                dbTicket.IssueSolved = true;
                dbTicket.FixedDate = DateTime.Now;

                _context.Tickets!.Update(dbTicket);
                await _context.SaveChangesAsync();

                if (User is not null)
                {
                    //send email to the ticket creator
                    emailContent.Subject = dbTicket.TicketTitle;

                    emailContent.Body = $"<h4> Good Day {User.Employee?.Person?.FirstName}</h4>" +
                                         $"<br/>I have Looked at the Ticket Title: {dbTicket.TicketTitle}" +
                                         $"<br/><h4>Here is the Ticket Solution(s):</4><br/> {ticket.TicketSolution} " +
                                         $"<br/><br/> Best regards <br/> {employee?.FullName}";

                    emailContent.Email = User.Employee?.WorkEmail!;
                    SendTicketEmail(emailContent);
                }
                response.Message = "Ticket was archiced successful.";
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> TicketAcknowledge(TicketResponse ticket)
        {
            var response = new ServiceResponse<bool>();
            var rawId = _hashids.Decode(ticket.TicketId)[0];

            var dbTicket = await _context.Tickets!
                            .FirstOrDefaultAsync(x => x.TicketId
                            .Equals(rawId));

            if (dbTicket is null)
            {
                response.Message = "The tickets does not exist.";
                response.Success = false;
            }
            else
            {
                dbTicket.TicketUpdate = ticket.TicketUpdate;
                dbTicket.Updated = true;
                dbTicket.ArangedDate = ticket.ArangedDate;

                _context.Tickets!.Update(dbTicket);
                await _context.SaveChangesAsync();
                response.Message = "Ticket was acknowledged successful.";
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

        private async Task<EmailResponse> ComposeEmailDetails(TicketResponse ticket)
        {
            var EmailContext = new EmailResponse();
            var employee = (await _employee.GetEmployeeById(_hashids.Encode(_authService.GetEmployeeId()))).Data;
            var device = (await _device.GetDeviceById(ticket.DeviceId)).Data;

            if (employee is not null && device is not null)
            {
                EmailContext.Subject = ticket.TicketTitle!;
                EmailContext.Body = $"Good day Tech Team <br/>" +
                                    $"<br/>I have created a Ticket about work device :<br/>" +
                                    $"<b>Device Name:</b> {device.DeviceName}<br/>" +
                                    $"<b>Identity Number:</b> {device.IdentityNumber}<br/>" +
                                    $"<h4>Here is the Issue:</h4>{ticket.TicketIssue}" +
                                    $"<br/><br/>Best regards<br/> {employee.FullName}";

                EmailContext.Email = "techservices@axiumeducation.org";
            }

            return EmailContext;
        }

        public void SendTicketEmail(EmailResponse request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(this._config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.Email));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
