using AutoMapper;
using SuperERP.Application.DTOs.Responses;
using SuperERP.Domain.Entities.Comercial;

namespace SuperERP.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Cliente, ClienteResponse>();
        CreateMap<Produto, ProdutoResponse>();
        CreateMap<Venda, VendaResponse>();
    }
}