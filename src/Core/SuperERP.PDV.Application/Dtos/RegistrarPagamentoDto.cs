using SuperERP.PDV.Domain.Enums;
using System;

namespace SuperERP.PDV.Application.Dtos
{
    public class RegistrarPagamentoDto
    {
        public Guid PdvVendaId { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public decimal Valor { get; set; }
    }
}
