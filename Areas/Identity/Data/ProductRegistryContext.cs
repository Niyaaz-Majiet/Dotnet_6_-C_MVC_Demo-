using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Product.Registry.Admin.Areas.Identity.Data;

namespace Product.Registry.Admin.Data;

public class ProductRegistryContext : IdentityDbContext<AdminUser>
{
    private readonly string _dbPath;

    public ProductRegistryContext(IConfiguration config) {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _dbPath = Path.Join(path,config.GetConnectionString("UserDbFilename")); 
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={_dbPath}");
}
