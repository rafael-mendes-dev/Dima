﻿using System.Net.Http.Json;
using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Web.Handlers;

public class TransactionHandler(IHttpClientFactory httpClientFactory) : ITransactionHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/transactions", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() ?? new Response<Transaction?>(null, 400, "Erro ao criar transação.");
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        var result = await _client.PutAsJsonAsync($"v1/transactions/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() ?? new Response<Transaction?>(null, 400, "Erro ao atualizar transação.");
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        var result = await _client.DeleteAsync($"v1/transactions/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() ??
               new Response<Transaction?>(null, 400, "Erro ao excluir transação.");
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request) =>
        await _client.GetFromJsonAsync<Response<Transaction?>>($"v1/transactions/{request.Id}")
            ?? new Response<Transaction?>(null, 400, "Erro ao obter transação.");
    

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
    {
        const string format = "yyyy-MM-dd";
        var startDate = request.StartDate is not null ? request.StartDate.Value.ToString(format)
            : DateTime.Now.GetFirstDay().ToString(format);
        
        var endDate = request.EndDate is not null ? request.EndDate.Value.ToString(format)
            : DateTime.Now.GetLastDay().ToString(format);

        var url = $"v1/transactions?startDate={startDate}&endDate={endDate}";

        return await _client.GetFromJsonAsync<PagedResponse<List<Transaction>?>>(url)
            ?? new PagedResponse<List<Transaction>?>(null, 400, "Erro ao carregar transações por período");
    }
}