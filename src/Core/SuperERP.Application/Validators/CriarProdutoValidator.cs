using FluentValidation;
using SuperERP.Application.DTOs.Requests;

namespace SuperERP.Application.Validators;

public class CriarProdutoValidator : AbstractValidator<CriarProdutoRequest>
{
    public CriarProdutoValidator()
    {
        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("SKU é obrigatório")
            .MaximumLength(50).WithMessage("SKU deve ter no máximo 50 caracteres");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres");

        RuleFor(x => x.PrecoVenda)
            .GreaterThan(0).WithMessage("Preço de venda deve ser maior que zero");

        RuleFor(x => x.PrecoCusto)
            .GreaterThanOrEqualTo(0).WithMessage("Preço de custo não pode ser negativo");
    }
}