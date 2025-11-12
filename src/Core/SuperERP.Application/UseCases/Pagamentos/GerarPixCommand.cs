using MediatR;
using SuperERP.Domain.Entities.Financeiro;
using SuperERP.Domain.Interfaces;

namespace SuperERP.Application.UseCases.Pagamentos;

public record GerarPixCommand(
    Guid EmpresaId,
    string ChavePix,
    decimal Valor,
    int ExpiracaoMinutos = 30,
    Guid? ClienteId = null,
    Guid? VendaId = null,
    string? InfoAdicional = null
) : IRequest<GerarPixResult>;

public class GerarPixHandler : IRequestHandler<GerarPixCommand, GerarPixResult>
{
    private readonly IPixService _pixService;

    public GerarPixHandler(IPixService pixService)
    {
        _pixService = pixService;
    }

    public async Task<GerarPixResult> Handle(GerarPixCommand request, CancellationToken cancellationToken)
    {
        var pixRequest = new PixRequest
        {
            ChavePix = request.ChavePix,
            Valor = request.Valor,
            ExpiracaoSegundos = request.ExpiracaoMinutos * 60,
            InfoAdicional = request.InfoAdicional
        };

        var response = await _pixService.GerarPixAsync(pixRequest, cancellationToken);

        return new GerarPixResult
        {
            Sucesso = response.Sucesso,
            TxId = response.TxId,
            QRCode = response.QRCode,
            QRCodeBase64 = response.QRCodeBase64,
            PixCopiaECola = response.PixCopiaECola,
            DataExpiracao = response.DataExpiracao,
            Erro = response.Erro
        };
    }
}

public class GerarPixResult
{
    public bool Sucesso { get; set; }
    public string TxId { get; set; } = string.Empty;
    public string QRCode { get; set; } = string.Empty;
    public string QRCodeBase64 { get; set; } = string.Empty;
    public string PixCopiaECola { get; set; } = string.Empty;
    public DateTime DataExpiracao { get; set; }
    public string? Erro { get; set; }
}
