using SuperERP.Web.Components;
using SuperERP.Web.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 3000;
});

builder.Services.AddHttpClient("SuperERP.API", client =>
{
    var envBase = Environment.GetEnvironmentVariable("SUPERERP_API_BASE_URL");
    var configBase = builder.Configuration["ApiSettings:BaseUrl"];
    var baseUrl = !string.IsNullOrWhiteSpace(envBase)
        ? envBase
        : !string.IsNullOrWhiteSpace(configBase)
            ? configBase
            : "http://localhost:5277/";

    if (!baseUrl.EndsWith("/")) baseUrl += "/";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<SuperERP.Web.Auth.AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
