using SuperERP.Domain.Entities.Base;
using SuperERP.PDV.Domain.Enums;
using System;

namespace SuperERP.PDV.Domain.Entities
{
    public class Pagamento : EntityBase
    {
        public Guid PdvVendaId { get; private set; }
        public FormaPagamento FormaPagamento { get; private set; }
        public decimal Valor { get; private set; }

        private Pagamento() { }

        public static Pagamento Criar(Guid pdvVendaId, FormaPagamento formaPagamento, decimal valor)
        {
            if (valor <= 0)
            {
                throw new ArgumentException("O valor do pagamento deve ser positivo.", nameof(valor));
            }

            return new Pagamento
            {
                PdvVendaId = pdvVendaId,
                FormaPagamento = formaPagamento,
                Valor = valor
            };
        }
    }
}
