using System;

namespace SuperERP.PDV.Application.Dtos
{
    public class FecharSessaoDto
    {
        public Guid SessaoCaixaId { get; set; }
        public decimal ValorContado { get; set; }
    }
}
