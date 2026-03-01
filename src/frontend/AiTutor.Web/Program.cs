using AiTutor.Web;
using AiTutor.Web.Services;
using Fluxor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// ── Root Components ───────────────────────────────────────────────────────────
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ── HTTP Clients ──────────────────────────────────────────────────────────────
builder.Services.AddHttpClient("BackendApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient("AiApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:8000");
    client.Timeout = TimeSpan.FromSeconds(60);
});

// ── Services ──────────────────────────────────────────────────────────────────
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<AiService>();

// ── MudBlazor ─────────────────────────────────────────────────────────────────
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 4000;
    config.SnackbarConfiguration.HideTransitionDuration = 300;
    config.SnackbarConfiguration.ShowTransitionDuration = 300;
});

// ── Fluxor ────────────────────────────────────────────────────────────────────
builder.Services.AddFluxor(options =>
{
    options.ScanAssemblies(typeof(Program).Assembly);
});

await builder.Build().RunAsync();
