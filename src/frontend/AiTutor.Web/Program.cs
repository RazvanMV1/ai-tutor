using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Fluxor;
using Blazored.LocalStorage;
using AiTutor.Web;
using AiTutor.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HTTP Client – BaseAddress = originea paginii (nginx proxy /api/ → backend)
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

// MudBlazor
builder.Services.AddMudServices();

// Local Storage
builder.Services.AddBlazoredLocalStorage();

// Fluxor State Management
builder.Services.AddFluxor(options =>
{
    options.ScanAssemblies(typeof(Program).Assembly);
});

// Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<AiService>();

await builder.Build().RunAsync();
