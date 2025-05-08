namespace Players.API.Models;

public record PlayerInfoDto(string PublicName, bool IsPublic, long? DotaId);
