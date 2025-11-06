namespace SuperERP.Application.DTOs.Responses;

public record ClienteResponse(
    Guid Id,
    string Nome,
    string CpfCnpj,
    string Email,
    string Telefone,
    DateTime CriadoEm,
    bool Ativo
);