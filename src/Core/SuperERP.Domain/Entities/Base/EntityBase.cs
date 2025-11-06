namespace SuperERP.Domain.Entities.Base;

public abstract class EntityBase
{
    public Guid Id { get; protected set; }
    public DateTime CriadoEm { get; protected set; }
    public DateTime? AtualizadoEm { get; protected set; }
    public Guid TenantId { get; protected set; }
    public bool Ativo { get; protected set; }

    protected EntityBase()
    {
        Id = Guid.NewGuid();
        CriadoEm = DateTime.UtcNow;
        Ativo = true;
    }

    public void Ativar() => Ativo = true;
    public void Desativar() => Ativo = false;
}
