using MatchMaker.Application.Contracts.Dtos;
using MatchMaker.Application.MatchMaking.AssignServerToMatch;
using MatchMaker.Application.MatchMaking.DeleteMatch;
using MatchMaker.Application.MatchMaking.GetMatchById;
using MatchMaker.Application.MatchMaking.GetMatchForPlayer;
using MatchMaker.Application.MatchMaking.MatchPlayer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.Controllers;

[ApiController]
[Route("matches")]
public class MatchesController : ControllerBase
{
    private readonly ISender sender;

    public MatchesController(ISender sender)
    {
        this.sender = sender;
    }

    [HttpPost]
    public async Task<GameMatchResponse> JoinMatchAsync(JoinMatchRequest request)
    {
        return await sender.Send(new MatchPlayerCommand(request.PlayerId));
    }

    [HttpGet]
    public async Task<ActionResult<GameMatchResponse>> GetMatchForPlayerAsync([FromQuery] string playerId)
    {
        var match = await sender.Send(new GetMatchForPlayerQuery(playerId));
        return match is null ? NotFound() : match;
    }

    [HttpGet("{matchId}")]
    public async Task<ActionResult<GameMatchResponse>> GetMatchByIdAsync(int matchId)
    {
        return await sender.Send(new GetMatchByIdQuery(matchId));
    }    

    [HttpPut("{matchId}")]
    public async Task<IActionResult> AssignServerToMatchAsync(int matchId, AssignServerToMatchRequest request)
    {
        await sender.Send(new AssignServerToMatchCommand(matchId, request.IpAddress, request.Port));
        return NoContent();
    }

    [HttpDelete("{matchId}")]
    public async Task<IActionResult> DeleteMatchAsync(int matchId)
    {
        await sender.Send(new DeleteMatchCommand(matchId));
        return NoContent();
    }    
}