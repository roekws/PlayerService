using Players.Core.Entities;

namespace Players.API.Models;

public record CharacterInfoDto(
  long Id,
  string Hero,
  int Level,
  int Experience,
  string CreatedAt
);
