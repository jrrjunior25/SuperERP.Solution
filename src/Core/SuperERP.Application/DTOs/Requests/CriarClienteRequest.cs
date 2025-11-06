namespace SuperERP.Application.DTOs.Requests;

public record CriarClienteRequest(
    string Nome,
    string CpfCnpj,
    string Email,
    string Telefone
);