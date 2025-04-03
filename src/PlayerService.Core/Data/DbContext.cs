using Microsoft.EntityFrameworkCore;
using PlayerService.Core.Entities;
public class PlayerContext : DbContext
{
  public DbSet<Player> Players { get; set; }

  public PlayerContext(DbContextOptions<PlayerContext> options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Player>(entity =>
    {
      entity.HasKey(p => p.Id);

      entity.Property(p => p.Id)
        .ValueGeneratedOnAdd();

      entity.Property(p => p.DotaId)
        .IsRequired();
    });
  }
}
