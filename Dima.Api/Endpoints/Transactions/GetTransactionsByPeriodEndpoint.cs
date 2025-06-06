﻿using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Transactions;

public class GetTransactionsByPeriodEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithName("Transactions: Get By Periodo")
            .WithSummary("Recupera transações por período")
            .WithDescription("Recupera transações por período")
            .WithOrder(5)
            .Produces<PagedResponse<List<Transaction>?>>();
    
    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ITransactionHandler handler,
        [FromQuery]DateTime? startDate = null,
        [FromQuery]DateTime? endDate = null,
        [FromQuery]int pageNumber = 1,
        [FromQuery]int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetTransactionsByPeriodRequest
        {
            UserId = user.Identity?.Name ?? string.Empty,
            PageNumber = pageNumber,
            PageSize = pageSize,
            StartDate = startDate,
            EndDate = endDate
        };
        var result = await handler.GetByPeriodAsync(request);
        return result.IsSucess ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
    }
}