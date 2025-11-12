using Microsoft.AspNetCore.Mvc;
using SuperERP.Domain.Interfaces.Repositories;

namespace SuperERP.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IVendaRepository _vendaRepository;

    public DashboardController(
        IClienteRepository clienteRepository,
        IProdutoRepository produtoRepository,
        IVendaRepository vendaRepository)
    {
        _clienteRepository = clienteRepository;
        _produtoRepository = produtoRepository;
        _vendaRepository = vendaRepository;
    }

    [HttpGet("metricas")]
    public async Task<IActionResult> ObterMetricas(CancellationToken cancellationToken)
    {
        var clientes = await _clienteRepository.GetAllAsync(cancellationToken);
        var produtos = await _produtoRepository.GetAllAsync(cancellationToken);
        var vendas = await _vendaRepository.GetAllAsync(cancellationToken);

        var metricas = new
        {
            totalClientes = clientes.Count,
            totalProdutos = produtos.Count,
            totalVendas = vendas.Count,
            vendasHoje = vendas.Count(v => v.DataVenda.Date == DateTime.Today),
            valorTotalVendas = vendas.Sum(v => v.ValorTotal),
            ticketMedio = vendas.Any() ? vendas.Average(v => v.ValorTotal) : 0
        };

        return Ok(metricas);
    }
}
