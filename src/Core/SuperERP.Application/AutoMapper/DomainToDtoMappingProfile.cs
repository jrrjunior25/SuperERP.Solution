using AutoMapper;
using SuperERP.Application.Dtos;
using SuperERP.Domain.Entities.Comercial;

namespace SuperERP.Application.AutoMapper
{
    public class DomainToDtoMappingProfile : Profile
    {
        public DomainToDtoMappingProfile()
        {
            CreateMap<Produto, ProdutoDto>();
            CreateMap<ProdutoDto, Produto>(); // Mapeamento reverso para a atualização
        }
    }
}
