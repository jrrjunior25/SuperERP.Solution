using System;
using System.Collections.Generic;

namespace SuperERP.PDV.Application.Dtos
{
    public class PdvVendaDto
    {
        public Guid Id { get; set; }
        public Guid SessaoCaixaId { get; set; }
        public decimal ValorTotal { get; set; }
        public bool Cancelada { get; set; }
        public List<PdvVendaItemDto> Itens { get; set; }
    }

    public class PdvVendaItemDto
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
