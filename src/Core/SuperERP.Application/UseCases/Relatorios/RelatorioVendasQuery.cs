using MediatR;

namespace SuperERP.Application.UseCases.Relatorios;

public record RelatorioVendasQuery(DateTime DataInicio, DateTime DataFim) : IRequest<RelatorioVendasResponse>;

public class RelatorioVendasQueryHandler : IRequestHandler<RelatorioVendasQuery, RelatorioVendasResponse>
{
    public Task<RelatorioVendasResponse> Handle(RelatorioVendasQuery request, CancellationToken cancellationToken)
    {
        var response = new RelatorioVendasResponse
        {
            TotalVendas = 15,
            ValorTotal = 12500.00m,
            TicketMedio = 833.33m,
            VendasPorDia = new List<VendaDia>
            {
                new() { Data = DateTime.Today.AddDays(-6), Quantidade = 2, Valor = 1500 },
                new() { Data = DateTime.Today.AddDays(-5), Quantidade = 3, Valor = 2200 },
                new() { Data = DateTime.Today.AddDays(-4), Quantidade = 1, Valor = 800 },
                new() { Data = DateTime.Today.AddDays(-3), Quantidade = 4, Valor = 3100 },
                new() { Data = DateTime.Today.AddDays(-2), Quantidade = 2, Valor = 1900 },
                new() { Data = DateTime.Today.AddDays(-1), Quantidade = 2, Valor = 1500 },
                new() { Data = DateTime.Today, Quantidade = 1, Valor = 1500 }
            }
        };
        
        return Task.FromResult(response);
    }
}

public class RelatorioVendasResponse
{
    public int TotalVendas { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal TicketMedio { get; set; }
    public List<VendaDia> VendasPorDia { get; set; } = new();
}

public class VendaDia
{
    public DateTime Data { get; set; }
    public int Quantidade { get; set; }
    public decimal Valor { get; set; }
}
