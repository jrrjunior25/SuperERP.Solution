using SuperERP.Domain.Entities.Base;
using System.Collections.Generic;

namespace SuperERP.PDV.Domain.Entities
{
    public class PdvVenda : EntityBase
    {
        public Guid SessaoCaixaId { get; private set; }
        public decimal ValorTotal { get; private set; }
        public bool Cancelada { get; private set; }

        private readonly List<PdvVendaItem> _itens = new();
        public IReadOnlyCollection<PdvVendaItem> Itens => _itens.AsReadOnly();

        private PdvVenda() { }

        public static PdvVenda Criar(Guid sessaoCaixaId)
        {
            return new PdvVenda
            {
                SessaoCaixaId = sessaoCaixaId,
                Cancelada = false
            };
        }

        public void AdicionarItem(Guid produtoId, decimal quantidade, decimal valorUnitario)
        {
            var itemExistente = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);

            if (itemExistente != null)
            {
                itemExistente.Quantidade += quantidade;
            }
            else
            {
                _itens.Add(PdvVendaItem.Criar(Id, produtoId, quantidade, valorUnitario));
            }

            RecalcularTotal();
        }

        public void Cancelar()
        {
            Cancelada = true;
        }

        private void RecalcularTotal()
        {
            ValorTotal = _itens.Sum(i => i.ValorTotal);
        }
    }
}
