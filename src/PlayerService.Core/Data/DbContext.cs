using Microsoft.EntityFrameworkCore;
using PlayerService.Core.Entities;

namespace PlayerService.Core.Data;

public class PlayerContext : DbContext
{
  public DbSet<Player> Players { get; set; }
  public DbSet<Character> Characters { get; set; }

  public PlayerContext(DbContextOptions<PlayerContext> options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Player>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.Property(e => e.Id)
        .ValueGeneratedOnAdd();

      entity.Property(e => e.DotaId)
        .IsRequired();

      entity.HasMany(e => e.Characters)
        .WithOne(e => e.Player)
        .HasForeignKey(e => e.PlayerId)
        .IsRequired();
    });

    modelBuilder.Entity<Character>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.Property(e => e.Id)
        .ValueGeneratedOnAdd();
    });
  }
}
