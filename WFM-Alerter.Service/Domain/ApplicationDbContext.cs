using Microsoft.EntityFrameworkCore;
using WFM_Alerter.Service.Models;

namespace WFM_Alerter.Service.Domain;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() { }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => base.OnModelCreating(modelBuilder);

    // Database sets
    public DbSet<Alert> Alerts { get; set; }
}