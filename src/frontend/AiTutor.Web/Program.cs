using AiTutor.Web.Services;
using Fluxor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<AiTutor.Web.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ── HTTP Clients ─────────────────────────────────────────────────────────────
builder.Services.AddHttpClient("BackendApi", client =>
    client.BaseAddress = new Uri("http://localhost:5000"));

builder.Services.AddHttpClient("AiApi", client =>
    client.BaseAddress = new Uri("http://localhost:8000"));

// ── Services ──────────────────────────────────────────────────────────────────
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<AiService>();

// ── MudBlazor ─────────────────────────────────────────────────────────────────
builder.Services.AddMudServices();

// ── Fluxor ────────────────────────────────────────────────────────────────────
builder.Services.AddFluxor(options =>
{
    options.ScanAssemblies(typeof(Program).Assembly);
    // ReduxDevTools eliminat - evităm dependența opțională
});

await builder.Build().RunAsync();
