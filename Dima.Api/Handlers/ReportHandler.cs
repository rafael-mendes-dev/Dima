using Dima.Api.Data;
using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class ReportHandler (AppDbContext context) : IReportHandler
{
    public async Task<Response<List<IncomesAndExpenses>?>> GetIncomesAndExpensesReportAsync(GetIncomesAndExpensesRequest request)
    {
        try
        {
            var data = await context.IncomesAndExpenses.AsNoTracking().Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.Year).ThenBy(x => x.Month).ToListAsync();
            return new Response<List<IncomesAndExpenses>?>(data);
        }
        catch
        {
            return new Response<List<IncomesAndExpenses>?>(null, 500, "Falha ao obter entradas e saídas");
        }
    }

    public async Task<Response<List<IncomesByCategory>?>> GetIncomesByCategoryReportAsync(GetIncomesByCategoryRequest request)
    {
        try
        {
            var data = await context.IncomesByCategories.AsNoTracking().Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.Year).ThenBy(x => x.Category).ToListAsync();

            return new Response<List<IncomesByCategory>?>(data);
        }
        catch
        {
            return new Response<List<IncomesByCategory>?>(null, 500, "Falha ao obter entradas por categoria");
        }
    }

    public async Task<Response<List<ExpensesByCategory>?>> GetExpensesByCategoryReportAsync(GetExpensesByCategoryRequest request)
    {
        try
        {
            var data = await context.ExpensesByCategories.AsNoTracking().Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.Year).ThenBy(x => x.Category).ToListAsync();

            return new Response<List<ExpensesByCategory>?>(data);
        }
        catch
        {
            return new Response<List<ExpensesByCategory>?>(null, 500,"Falha ao obter saídas por categoria");
        }
    }

    public async Task<Response<FinancialSummary?>> GetFinancialSummaryReportAsync(GetFinancialSummaryRequest request)
    {
        try
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var data = await context.Transactions.AsNoTracking().Where(
                    x => x.UserId == request.UserId && x.PaidOrReceivedAt >= startDate &&
                         x.PaidOrReceivedAt <= DateTime.Now).GroupBy(x => 1)
                .Select(x => new FinancialSummary(request.UserId,
                    x.Where(x => x.Type == ETransactionType.Deposit).Sum(t => t.Amount),
                    x.Where(x => x.Type == ETransactionType.Withdraw).Sum(t => t.Amount)))
                .FirstOrDefaultAsync();

            return new Response<FinancialSummary?>(data);
        }
        catch
        {
            return new Response<FinancialSummary?>(null, 500, "Falha ao obter resumo financeiro");
        }
    }
}