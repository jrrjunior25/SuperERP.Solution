using System;

namespace SuperERP.PDV.Application.Dtos
{
    public class ResumoFechamentoDto
    {
        public Guid SessaoCaixaId { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime DataFechamento { get; set; }
        public decimal ValorAbertura { get; set; }
        public decimal TotalVendas { get; set; }
        public decimal TotalPagamentosDinheiro { get; set; }
        public decimal ValorEsperado { get; set; }
        public decimal ValorContado { get; set; }
        public decimal Diferenca { get; set; }
    }
}
