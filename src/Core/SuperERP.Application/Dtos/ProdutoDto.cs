using System;

namespace SuperERP.Application.Dtos
{
    public class ProdutoDto
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal PrecoVenda { get; set; }
        public decimal EstoqueAtual { get; set; }
    }
}
