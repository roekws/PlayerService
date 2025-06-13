using Microsoft.EntityFrameworkCore;

namespace Players.Core.Data.Results;

public record Error(string Code, string Message);
