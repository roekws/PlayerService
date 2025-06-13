namespace Players.Core.Data.Errors;

public record Error(ErrorType ErrorType, string Code, string Description);
