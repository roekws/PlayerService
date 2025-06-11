using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Players.API.Infrastructure.Errors;
using Players.API.Models;
using Players.Core.Services;

namespace Players.API.Controllers;

[ApiController]
[Route("api/match")]
public class MatchController(IMatchService matchService) : ControllerBase
{
  private readonly IMatchService matchService = matchService;

  [AllowAnonymous]
  [HttpGet("{id}")]
  [ProducesResponseType<MatchDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
  public async Task<ActionResult> GetMatchById(long id, bool detailed)
  {
    var match = await matchService.GetByIdAsync(id, detailed);

    if (match == null)
    {
      return NotFound(new ErrorResponse(ApiErrors.MatchNotFound));
    }

    return Ok(new MatchDto(match));
  }
}
