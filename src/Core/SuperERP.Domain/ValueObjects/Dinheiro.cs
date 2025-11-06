namespace SuperERP.Domain.ValueObjects;

public record Dinheiro
{
    public decimal Valor { get; init; }
    public string Moeda { get; init; } = "BRL";

    public static Dinheiro Criar(decimal valor, string moeda = "BRL")
    {
        return new Dinheiro { Valor = valor, Moeda = moeda };
    }

    public Dinheiro Somar(Dinheiro outro)
    {
        if (Moeda != outro.Moeda)
            throw new InvalidOperationException("Não é possível somar valores de moedas diferentes");
        
        return new Dinheiro { Valor = Valor + outro.Valor, Moeda = Moeda };
    }

    public Dinheiro Subtrair(Dinheiro outro)
    {
        if (Moeda != outro.Moeda)
            throw new InvalidOperationException("Não é possível subtrair valores de moedas diferentes");
        
        return new Dinheiro { Valor = Valor - outro.Valor, Moeda = Moeda };
    }
}