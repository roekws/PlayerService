using Microsoft.EntityFrameworkCore;
using Players.Core.Entities;

namespace Players.Core.Data;

public class PlayerContext : DbContext
{
  public DbSet<Player> Players { get; set; }
  public DbSet<Match> Matches { get; set; }
  public DbSet<Base> Bases { get; set; }
  public DbSet<Structure> Structures { get; set; }
  public DbSet<StructureAbility> StructureAbilities { get; set; }
  public DbSet<Character> Characters { get; set; }
  public DbSet<CharacterItem> CharacterItems { get; set; }
  public DbSet<CharacterAbility> CharacterAbilities { get; set; }
  public DbSet<MatchBattle> MatchBattles { get; set; }

  public PlayerContext(DbContextOptions<PlayerContext> options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Player>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedOnAdd();

      entity.Property(e => e.DotaId).IsRequired();
      entity.HasIndex(e => e.DotaId).IsUnique();

      entity.Property(e => e.SteamId).IsRequired();
      entity.HasIndex(e => e.SteamId).IsUnique();

      entity.HasIndex(e => e.CurrentMatchId);

      entity.HasMany(e => e.Matches)
        .WithOne(e => e.Player)
        .HasForeignKey(e => e.PlayerId)
        .IsRequired();

      entity.HasMany(e => e.MatchesBattles)
        .WithOne(e => e.EnemyPlayer)
        .HasForeignKey(e => e.EnemyPlayerId)
        .IsRequired();
    });

    modelBuilder.Entity<Match>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedOnAdd();

      entity.HasIndex(e => e.Level);

      entity.HasOne(e => e.Base)
        .WithOne(e => e.Match)
        .HasForeignKey<Match>(e => e.BaseId)
        .IsRequired();

      entity.HasOne(e => e.Character)
        .WithOne(e => e.Match)
        .HasForeignKey<Match>(e => e.CharacterId)
        .IsRequired();

      entity.HasMany(e => e.Battles)
        .WithOne(e => e.Match)
        .HasForeignKey(e => e.MatchId);
    });

    modelBuilder.Entity<Base>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedOnAdd();

      entity.HasMany(e => e.Structures)
        .WithOne(e => e.Base)
        .HasForeignKey(e => e.BaseId)
        .IsRequired();
    });

    modelBuilder.Entity<Structure>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedOnAdd();

      entity.HasMany(e => e.Abilities)
        .WithOne(e => e.Structure)
        .HasForeignKey(e => e.StructureId)
        .IsRequired();
    });

    modelBuilder.Entity<StructureAbility>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
    });

    modelBuilder.Entity<Character>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedOnAdd();

      entity.HasMany(e => e.Abilities)
        .WithOne(e => e.Character)
        .HasForeignKey(e => e.CharacterId)
        .IsRequired();

      entity.HasMany(e => e.Items)
        .WithOne(e => e.Character)
        .HasForeignKey(e => e.CharacterId)
        .IsRequired();
    });

    modelBuilder.Entity<CharacterItem>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
    });

    modelBuilder.Entity<CharacterAbility>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedOnAdd();
    });

    modelBuilder.Entity<MatchBattle>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id).ValueGeneratedOnAdd();

      entity.Property(e => e.EnemyCharacterSnapshotJson)
        .HasColumnType("jsonb");
      entity.Property(e => e.EnemyBaseSnapshotJson)
        .HasColumnType("jsonb");

      entity.Property(e => e.PlayerCharacterSnapshotJson)
        .HasColumnType("jsonb");
      entity.Property(e => e.PlayerBaseSnapshotJson)
        .HasColumnType("jsonb");
    });
  }
}
