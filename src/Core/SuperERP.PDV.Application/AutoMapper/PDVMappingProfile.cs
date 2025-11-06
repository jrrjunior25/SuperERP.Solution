using AutoMapper;
using SuperERP.PDV.Application.Dtos;
using SuperERP.PDV.Domain.Entities;

namespace SuperERP.PDV.Application.AutoMapper;

public class PDVMappingProfile : Profile
{
    public PDVMappingProfile()
    {
        CreateMap<SessaoCaixa, SessaoCaixaDto>();
        CreateMap<PdvVenda, PdvVendaDto>();
        CreateMap<PdvVendaItem, PdvVendaItemDto>();
    }
}
