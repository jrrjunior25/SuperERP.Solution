using Microsoft.EntityFrameworkCore;

namespace SuperERP.PDV.Data.Local;

public class PDVDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "supererp_pdv.db3");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }
}
