global using DeviceManagment.Client;
global using DeviceManagment.Client.Services.AuthService;
global using DeviceManagment.Client.Services.EmployeeService;
global using DeviceManagment.Client.Services.PersonService;
global using DeviceManagment.Client.Services.DepartmentService;
global using DeviceManagment.Client.Services.OfficeService;
global using DeviceManagment.Client.Services.DeviceTypeService;
global using DeviceManagment.Client.Services.DeviceService;
global using DeviceManagment.Client.Services.DeviceLoanService;
global using DeviceManagment.Client.Services.TicketService;
global using Blazored.LocalStorage;
global using Blazored.Toast;
global using DeviceManagment.Shared.Auth;
global using DeviceManagment.Shared.Responses;
global using System.Net.Http.Json;
global using DeviceManagment.Shared.Charts;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IPersonService, PersonService>();
builder.Services.AddTransient<IDepartmentService, DepartmentService>();
builder.Services.AddTransient<IOfficeService, OfficeService>();
builder.Services.AddTransient<IDeviceTypeService, DeviceTypeService>();
builder.Services.AddTransient<IDeviceService, DeviceService>();
builder.Services.AddTransient<IDeviceLoanService, DeviceLoanService>();
builder.Services.AddTransient<ITicketService, TicketService>();
//Authorization
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();
