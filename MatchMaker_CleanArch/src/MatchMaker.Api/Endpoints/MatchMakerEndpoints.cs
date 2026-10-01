using MatchMaker.Application.Contracts.Dtos;
using MatchMaker.Application.MatchMaking.AssignServerToMatch;
using MatchMaker.Application.MatchMaking.DeleteMatch;
using MatchMaker.Application.MatchMaking.GetMatchById;
using MatchMaker.Application.MatchMaking.GetMatchForPlayer;
using MatchMaker.Application.MatchMaking.MatchPlayer;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MatchMaker.Api.Endpoints
{
    public static class MatchMakerEndpoints
    {
        public static RouteGroupBuilder MapMatchMakerEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/matches-minimal");

            group.MapPost("/", MatchPlayerAsync);
            group.MapGet("/", GetMatchForPlayerAsync);
            group.MapGet("/{matchId}", GetMatchByIdAsync);            
            group.MapPut("/{matchId}", AssignServerToMatchAsync);
            group.MapDelete("/{matchId}", DeleteMatchAsync);            

            return group;
        }

        public static async Task<GameMatchResponse> MatchPlayerAsync(
            ISender sender, 
            JoinMatchRequest request)
        {
            return await sender.Send(new MatchPlayerCommand(request.PlayerId));
        }

        public static async Task<Results<NotFound, Ok<GameMatchResponse>>> GetMatchForPlayerAsync(
            ISender sender, 
            string playerId)
        {
            var match = await sender.Send(new GetMatchForPlayerQuery(playerId));
            return match is null ? TypedResults.NotFound() : TypedResults.Ok(match);
        }

        public static async Task<GameMatchResponse> GetMatchByIdAsync(
            ISender sender, 
            int matchId)
        {
            return await sender.Send(new GetMatchByIdQuery(matchId));
        }

        public static async Task<NoContent> AssignServerToMatchAsync(
            ISender sender, 
            int matchId, 
            AssignServerToMatchRequest request)
        {
            await sender.Send(new AssignServerToMatchCommand(matchId, request.IpAddress, request.Port));
            return TypedResults.NoContent();
        }

        public static async Task<NoContent> DeleteMatchAsync(
            ISender sender, 
            int matchId)
        {
            await sender.Send(new DeleteMatchCommand(matchId));
            return TypedResults.NoContent();
        }
    }
}