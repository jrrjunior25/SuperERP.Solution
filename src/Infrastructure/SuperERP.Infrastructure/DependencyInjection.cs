using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuperERP.Domain.Interfaces;
using SuperERP.Domain.Interfaces.Repositories;
using SuperERP.Infrastructure.Data;
using SuperERP.Infrastructure.Data.Context;
using SuperERP.Infrastructure.Integrations.NFe;
using SuperERP.Infrastructure.Integrations.Pagamento;
using SuperERP.Infrastructure.Integrations.TEF;
using SuperERP.Infrastructure.Messaging;
using SuperERP.Infrastructure.Multitenancy;
using SuperERP.Infrastructure.Repositories;
using SuperERP.Infrastructure.Services;

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
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // Messaging
        services.AddSingleton<IMessageBus>(sp => 
            new RabbitMQService(configuration.GetConnectionString("RabbitMQ") ?? "amqp://supererp:Super@ERP2025!@localhost:5672"));
        
        // Services
        services.AddSingleton<IEmailService>(sp => 
            new EmailService(
                configuration["Email:SmtpHost"] ?? "smtp.gmail.com",
                int.Parse(configuration["Email:SmtpPort"] ?? "587"),
                configuration["Email:SmtpUser"] ?? "",
                configuration["Email:SmtpPassword"] ?? "",
                configuration["Email:FromEmail"] ?? "noreply@supererp.com"
            ));
        
        services.AddSingleton<ICacheService, MemoryCacheService>();
        services.AddSingleton<IStorageService>(sp => 
            new LocalStorageService(configuration["Storage:BasePath"] ?? "./uploads"));
        
        // Integrations
        services.AddScoped<INFeService, NFeService>();
        services.AddScoped<ITEFService, TEFService>();
        services.AddScoped<IPagamentoService, PagamentoService>();

        return services;
    }
}
