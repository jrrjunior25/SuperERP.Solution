using Hangfire;

namespace SuperERP.Infrastructure.BackgroundJobs;

public class SincronizacaoJob
{
    [AutomaticRetry(Attempts = 3)]
    public async Task SincronizarVendasPDVAsync()
    {
        await Task.Delay(100);
        // Sincronizar vendas do PDV com retaguarda
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task SincronizarProdutosAsync()
    {
        await Task.Delay(100);
        // Sincronizar produtos para PDV
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task ConciliacaoBancariaAsync()
    {
        await Task.Delay(100);
        // Conciliar pagamentos PIX
    }
}
