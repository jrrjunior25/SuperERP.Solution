using SuperERP.Domain.Entities.Base;

namespace SuperERP.PDV.Domain.Entities;

public class Caixa : EntityBase
{
    public string Nome { get; private set; }
    public decimal Saldo { get; private set; }

    private Caixa() { }

    public static Caixa Criar(string nome)
    {
        return new Caixa
        {
            Nome = nome,
            Saldo = 0
        };
    }
}
