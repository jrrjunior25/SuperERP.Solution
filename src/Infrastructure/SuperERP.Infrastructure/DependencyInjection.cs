using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuperERP.Domain.Interfaces.Repositories;
using SuperERP.Infrastructure.Data.Context;
using SuperERP.Infrastructure.Multitenancy;
using SuperERP.Infrastructure.Repositories;

namespace SuperERP.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<SuperERPDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IVendaRepository, VendaRepository>();
        
        services.AddScoped<ITenantService, TenantService>();

        return services;
    }
}
