using SuperERP.Domain.Entities.Base;
using SuperERP.PDV.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperERP.PDV.Domain.Entities
{
    public class PdvVenda : EntityBase
    {
        public Guid SessaoCaixaId { get; private set; }
        public decimal ValorTotal { get; private set; }
        public StatusVenda Status { get; private set; }
        public decimal ValorPago => Pagamentos.Sum(p => p.Valor);
        public decimal ValorPendente => ValorTotal - ValorPago;

        private readonly List<PdvVendaItem> _itens = new();
        public IReadOnlyCollection<PdvVendaItem> Itens => _itens.AsReadOnly();

        private readonly List<Pagamento> _pagamentos = new();
        public IReadOnlyCollection<Pagamento> Pagamentos => _pagamentos.AsReadOnly();

        private PdvVenda() { }

        public static PdvVenda Criar(Guid sessaoCaixaId)
        {
            return new PdvVenda
            {
                SessaoCaixaId = sessaoCaixaId,
                Status = StatusVenda.EmAberto
            };
        }

        public void AdicionarItem(Guid produtoId, decimal quantidade, decimal valorUnitario)
        {
            if (Status != StatusVenda.EmAberto) throw new Exception("Não é possível adicionar itens a uma venda que não está em aberto.");

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

        public void AdicionarPagamento(FormaPagamento formaPagamento, decimal valor)
        {
            if (Status != StatusVenda.EmAberto) throw new Exception("Não é possível adicionar pagamentos a uma venda que não está em aberto.");
            if (ValorPendente <= 0) throw new Exception("A venda já foi totalmente paga.");

            var valorAPagar = Math.Min(valor, ValorPendente);
            _pagamentos.Add(Pagamento.Criar(Id, formaPagamento, valorAPagar));

            if (ValorPendente <= 0)
            {
                FinalizarPagamento();
            }
        }

        private void FinalizarPagamento()
        {
            Status = StatusVenda.Paga;
        }

        public void Cancelar()
        {
            if (Status == StatusVenda.Paga) throw new Exception("Não é possível cancelar uma venda que já foi paga.");
            Status = StatusVenda.Cancelada;
        }

        private void RecalcularTotal()
        {
            ValorTotal = _itens.Sum(i => i.ValorTotal);
        }
    }
}
