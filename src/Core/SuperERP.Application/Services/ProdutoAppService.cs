using AutoMapper;
using SuperERP.Application.Dtos;
using SuperERP.Application.Interfaces;
using SuperERP.Domain.Entities.Comercial;
using SuperERP.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperERP.Application.Services
{
    public class ProdutoAppService : IProdutoAppService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;

        public ProdutoAppService(IProdutoRepository produtoRepository, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProdutoDto>> GetAll()
        {
            var produtos = await _produtoRepository.GetAll();
            return _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
        }

        public async Task<ProdutoDto> GetById(Guid id)
        {
            var produto = await _produtoRepository.GetById(id);
            return _mapper.Map<ProdutoDto>(produto);
        }

        public async Task Create(ProdutoDto produtoDto)
        {
            // Para um cenário real, precisaríamos de um DTO de criação para não expor a entidade completa.
            // Por simplicidade, estamos reutilizando o ProdutoDto.
            var produto = Produto.Criar(produtoDto.Sku, produtoDto.Nome, produtoDto.PrecoVenda, 0); // PrecoCusto como 0
            // produto.Descricao = produtoDto.Descricao; // A entidade não tem setter público.
            await _produtoRepository.Add(produto);
        }

        public async Task Update(ProdutoDto produtoDto)
        {
            var produto = await _produtoRepository.GetById(produtoDto.Id);
            if (produto == null) throw new Exception("Produto não encontrado.");

            // A entidade Produto não possui setters públicos, o que é uma boa prática de DDD.
            // Para atualizar, precisaríamos de métodos públicos na entidade, como `produto.AlterarPreco(preco)`.
            // Como isso não existe, farei a atualização via AutoMapper para simular o processo,
            // embora o ideal fosse evoluir a entidade.

            // ATENÇÃO: O AutoMapper não deve ser usado para atualizar entidades diretamente em cenários complexos.
            // Isto é uma simplificação.
            _mapper.Map(produtoDto, produto);

            await _produtoRepository.Update(produto);
        }

        public async Task Remove(Guid id)
        {
            var produto = await _produtoRepository.GetById(id);
            if (produto == null) throw new Exception("Produto não encontrado.");
            await _produtoRepository.Remove(produto);
        }

        public async Task<IEnumerable<ProdutoDto>> SearchByName(string name)
        {
            var produtos = await _produtoRepository.SearchByName(name);
            return _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
        }
    }
}
