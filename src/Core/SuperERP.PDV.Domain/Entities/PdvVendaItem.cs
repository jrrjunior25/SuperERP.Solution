using SuperERP.Domain.Entities.Base;

namespace SuperERP.PDV.Domain.Entities
{
    public class PdvVendaItem : EntityBase
    {
        public Guid PdvVendaId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; private set; }
        public decimal ValorTotal => Quantidade * ValorUnitario;

        private PdvVendaItem() { }

        public static PdvVendaItem Criar(Guid pdvVendaId, Guid produtoId, decimal quantidade, decimal valorUnitario)
        {
            return new PdvVendaItem
            {
                PdvVendaId = pdvVendaId,
                ProdutoId = produtoId,
                Quantidade = quantidade,
                ValorUnitario = valorUnitario
            };
        }
    }
}
