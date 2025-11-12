using Microsoft.EntityFrameworkCore;
using SuperERP.PDV.Data.Local;

namespace SuperERP.PDV.Services;

public class DatabaseService
{
    private readonly PDVDbContext _context;

    public DatabaseService()
    {
        _context = new PDVDbContext();
    }

    public async Task InicializarAsync()
    {
        await _context.Database.EnsureCreatedAsync();
    }

    public PDVDbContext ObterContexto() => _context;
}
