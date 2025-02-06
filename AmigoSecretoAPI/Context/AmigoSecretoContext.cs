using AmigoSecretoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AmigoSecretoAPI.Context;

public class AmigoSecretoContext : DbContext
{
  public DbSet<Grupo> Grupos { get; set; }
  public DbSet<Participante> Participantes { get; set; }
  public AmigoSecretoContext(DbContextOptions<AmigoSecretoContext> options) : base(options) { }
   protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Participante>()
            .HasOne(p => p.AmigoSecreto)
            .WithMany()
            .HasForeignKey(p => p.AmigoSecretoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
