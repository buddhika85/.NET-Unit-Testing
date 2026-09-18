// using MatchMaker.Api.Contracts.Dtos;
// using MatchMaker.Api.Services;
// using Microsoft.AspNetCore.Http.HttpResults;

// namespace MatchMaker.Api.Endpoints
// {
//     public static class MatchMakerEndpoints
//     {
//         public static RouteGroupBuilder MapMatchMakerEndpoints(this IEndpointRouteBuilder routes)
//         {
//             var group = routes.MapGroup("/matches-minimal");

//             group.MapPost("/", MatchPlayerAsync);
//             group.MapGet("/", GetMatchForPlayerAsync);
//             group.MapGet("/{matchId}", GetMatchByIdAsync);            
//             group.MapPut("/{matchId}", AssignServerToMatchAsync);
//             group.MapDelete("/{matchId}", DeleteMatchAsync);            

//             return group;
//         }

//         public static async Task<GameMatchResponse> MatchPlayerAsync(
//             IGameMatcher matcher, 
//             JoinMatchRequest request)
//         {
//             return await matcher.MatchPlayerAsync(request);
//         }

//         public static async Task<Results<NotFound, Ok<GameMatchResponse>>> GetMatchForPlayerAsync(
//             IGameMatcher matcher, 
//             string playerId)
//         {
//             var match = await matcher.GetMatchForPlayerAsync(playerId);
//             return match is null ? TypedResults.NotFound() : TypedResults.Ok(match);
//         }

//         public static async Task<GameMatchResponse> GetMatchByIdAsync(
//             IGameMatcher matcher, 
//             int matchId)
//         {
//             return await matcher.GetMatchByIdAsync(matchId);
//         }

//         public static async Task<NoContent> AssignServerToMatchAsync(
//             IGameMatcher matcher, 
//             int matchId, 
//             AssignServerToMatchRequest request)
//         {
//             await matcher.AssignServerToMatchAsync(matchId, request);
//             return TypedResults.NoContent();
//         }

//         public static async Task<NoContent> DeleteMatchAsync(
//             IGameMatcher matcher, 
//             int matchId)
//         {
//             await matcher.DeleteMatchAsync(matchId);
//             return TypedResults.NoContent();
//         }
//     }
// }