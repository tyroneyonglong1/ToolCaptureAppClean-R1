using Microsoft.EntityFrameworkCore;

namespace ToolCaptureAppClean.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<ToolTest> ToolTests => Set<ToolTest>();
        public DbSet<BrandResult> BrandResults => Set<BrandResult>();
    }
}
