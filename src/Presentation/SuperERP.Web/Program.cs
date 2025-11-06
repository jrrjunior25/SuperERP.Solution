using SuperERP.Web.Components;
using MudBlazor.Services;
using SuperERP.PDV.Application.Interfaces;
using SuperERP.PDV.Application.Services;
using SuperERP.PDV.Domain.Interfaces;
using SuperERP.PDV.Infrastructure.Repositories;
using SuperERP.PDV.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddAutoMapper(typeof(SuperERP.PDV.Application.AutoMapper.PDVMappingProfile));

// Configuração da Injeção de Dependência para o Módulo PDV (Em Memória)
builder.Services.AddSingleton<ICaixaRepository, CaixaRepositoryMemory>();
builder.Services.AddSingleton<ISessaoCaixaRepository, SessaoCaixaRepositoryMemory>();
builder.Services.AddSingleton<IPdvVendaRepository, PdvVendaRepositoryMemory>();
builder.Services.AddScoped<ICaixaAppService, CaixaAppService>();

// Adiciona um caixa padrão para fins de demonstração
var caixaRepo = new CaixaRepositoryMemory();
caixaRepo.Add(Caixa.Criar("Caixa Principal"));


builder.Services.AddHttpClient("SuperERP.API", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5000");
});

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
