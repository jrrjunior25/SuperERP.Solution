using Microsoft.EntityFrameworkCore;
using SuperERP.PDV.Application.Interfaces;
using SuperERP.PDV.Application.Services;
using SuperERP.PDV.Domain.Entities;
using SuperERP.PDV.Domain.Interfaces;
using SuperERP.PDV.Infrastructure.Data;
using SuperERP.PDV.Infrastructure.Repositories;
using SuperERP.Web.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Entity Framework Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PdvDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddAutoMapper(typeof(SuperERP.PDV.Application.AutoMapper.PDVMappingProfile));

// Configuração da Injeção de Dependência para o Módulo PDV (Com EF Core)
builder.Services.AddScoped<ICaixaRepository, CaixaRepository>();
builder.Services.AddScoped<ISessaoCaixaRepository, SessaoCaixaRepository>();
builder.Services.AddScoped<IPdvVendaRepository, PdvVendaRepository>();
builder.Services.AddScoped<ICaixaAppService, CaixaAppService>();

// Configuração da Injeção de Dependência para o Módulo Principal
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutoAppService, ProdutoAppService>();


builder.Services.AddHttpClient("SuperERP.API", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5000");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Aplica as migrations e semeia os dados iniciais
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<PdvDbContext>();
        context.Database.Migrate();

        var caixaRepo = services.GetRequiredService<ICaixaRepository>();
        if (!caixaRepo.GetAll().Result.Any())
        {
            caixaRepo.Add(Caixa.Criar("Caixa Principal"));
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao aplicar as migrations ou semear os dados.");
    }
}


app.Run();
