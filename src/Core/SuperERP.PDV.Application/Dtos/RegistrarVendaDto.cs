using System.Collections.Generic;

namespace SuperERP.PDV.Application.Dtos
{
    public class RegistrarVendaDto
    {
        public Guid SessaoCaixaId { get; set; }
        public List<ItemVendaDto> Itens { get; set; }
    }

    public class ItemVendaDto
    {
        public Guid ProdutoId { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
    }
}
