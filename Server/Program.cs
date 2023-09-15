global using DeviceManagment.Server.Data;
global using DeviceManagment.Shared;
global using DeviceManagment.Shared.Auth;
global using DeviceManagment.Shared.Responses;
global using Microsoft.EntityFrameworkCore;
global using HashidsNet;
global using DeviceManagment.Server.Services.EmployeeService;
global using DeviceManagment.Server.Services.AuthService;
global using DeviceManagment.Server.Services.PersonService;
global using DeviceManagment.Server.Services.DepartmentService;
global using DeviceManagment.Server.Services.OfficeServices;
global using DeviceManagment.Server.Services.DeviceTypeService;
global using DeviceManagment.Server.Services.DeviceService;
global using DeviceManagment.Server.Services.DeviceLoanService;
global using DeviceManagment.Server.Services.TicketService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DataContext"))
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//adding swagger to the application
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHashids>(_ => new Hashids("Axiumrocksbigtime", 11));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IOfficeServices, OfficeServices>();
builder.Services.AddScoped<IDeviceTypeService, DeviceTypeService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IDeviceLoanService, DeviceLoanService>();
builder.Services.AddScoped<ITicketService, TicketService>();

//Authentication middleware
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

//for accessing the userId we need AddHttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

//adding swagger
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//adding swagger 
app.UseSwagger();

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

//Add Middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
