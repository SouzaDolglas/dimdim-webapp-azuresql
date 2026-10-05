using DimDim.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Web.Data;

// As tabelas são criadas pelo DDL em scripts/ddl.sql; o EF só lê e grava.
public class DimDimContext(DbContextOptions<DimDimContext> options) : DbContext(options)
{
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Conta> Contas => Set<Conta>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Cliente>().HasIndex(c => c.Cpf).IsUnique();
        b.Entity<Conta>().HasIndex(c => c.Numero).IsUnique();
        b.Entity<Conta>()
            .HasOne(c => c.Cliente)
            .WithMany(c => c.Contas)
            .HasForeignKey(c => c.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
