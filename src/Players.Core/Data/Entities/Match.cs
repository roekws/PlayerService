﻿using Players.Core.Enums;

namespace Players.Core.Entities;

public class Match
{
  public long Id { get; set; }

  public long PlayerId { get; set; }
  public long? CharacterId { get; set; }
  public long? CityId { get; set; }

  public DateTime StartTime { get; set; } = DateTime.UtcNow;
  public DateTime? EndTime { get; set; }

  public int Level { get; set; } = 0;
  public int RatingChange { get; set; } = 0;

  public long GlobalPatchVersion { get; set; }
  public long BalancePatchVersion { get; set; }

  public MatchState State { get; set; } = MatchState.Active;

  // Navigation properties
  public Player Player { get; set; } = null!;
  public Character? Character { get; set; }
  public City? City { get; set; }
  public ICollection<MatchBattle> Battles { get; set; } = [];
}
