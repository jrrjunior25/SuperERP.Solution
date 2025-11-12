using SuperERP.Application.UseCases.Vendas;

namespace SuperERP.Infrastructure.Repositories;

public class EmpresaRepository : IEmpresaRepository
{
    public async Task<EmpresaEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        
        return new EmpresaEntity
        {
            Cnpj = "12345678000190",
            RazaoSocial = "EMPRESA TESTE LTDA",
            NomeFantasia = "LOJA TESTE",
            Logradouro = "RUA TESTE",
            Numero = "123",
            Bairro = "CENTRO",
            Cidade = "SAO PAULO",
            UF = "SP",
            CEP = "01234567",
            CertificadoDigital = Array.Empty<byte>(),
            SenhaCertificado = "senha123",
            AmbienteHomologacao = true
        };
    }
}
