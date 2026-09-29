using DimDim.Models;
using Microsoft.EntityFrameworkCore;
namespace DimDim.Data;

public class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> options) : base(options) { }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Cliente>(e =>
        {
            e.ToTable("Cliente");
            e.HasIndex(c => c.Email).IsUnique();
        });
        mb.Entity<Transacao>(e =>
        {
            e.ToTable("Transacao");
            e.Property(t => t.Valor).HasColumnType("decimal(12,2)");
            e.HasOne(t => t.Cliente).WithMany(c => c.Transacoes)
             .HasForeignKey(t => t.ClienteId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
