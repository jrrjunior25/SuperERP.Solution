namespace SuperERP.Infrastructure.Multitenancy;

public interface ITenantService
{
    Guid GetTenantId();
    void SetTenantId(Guid tenantId);
}

public class TenantService : ITenantService
{
    private Guid _tenantId;

    public Guid GetTenantId() => _tenantId;

    public void SetTenantId(Guid tenantId) => _tenantId = tenantId;
}