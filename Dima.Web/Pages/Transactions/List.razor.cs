using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Transactions;

public partial class ListTransactionsPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public List<Transaction> Transactions { get; set; } = [];
    public string SearchTerm { get; set; } = string.Empty;
    public int CurrentYear { get; set; } = DateTime.Now.Year;
    public int CurrentMonth { get; set; } = DateTime.Now.Month;
    public int[] Years { get; set; } =
    {
        DateTime.Now.Year,
        DateTime.Now.AddYears(-1).Year,
        DateTime.Now.AddYears(-2).Year,
        DateTime.Now.AddYears(-3).Year,
    };
    #endregion

    #region Services

    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    
    [Inject] public IDialogService DialogService { get; set; } = null!;

    [Inject] public ITransactionHandler Handler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync() => await GetTransactionsAsync();

    #endregion

    #region Public Methods

    public async void OnDeleteButtonClickedAsync(long id, string title)
    {
        var result = await DialogService.ShowMessageBox(
            "ATENÇÃO",
            $"A operação de exclusão é irreversível. Tem certeza que deseja excluir a transação {title}?",
            yesText: "Excluir",
            cancelText: "Cancelar");

        if (result is true)
            await OnDeleteAsync(id, title);
        
        StateHasChanged();
    }
    
    public Func<Transaction, bool> Filter => transaction =>
    {
        if(string.IsNullOrEmpty(SearchTerm))
            return true;
        
        return transaction.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) || transaction.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);
    };
    
    #endregion
    
    #region Private Methods
    
    public async Task OnSearchAsync()
    {
        await GetTransactionsAsync();
        StateHasChanged();
    }
    private async Task GetTransactionsAsync()
    {
        IsBusy = true;

        try
        {
            var request = new GetTransactionsByPeriodRequest
            {
                StartDate = DateTime.Now.GetFirstDay(CurrentYear, CurrentMonth),
                EndDate = DateTime.Now.GetLastDay(CurrentYear, CurrentMonth),
                PageNumber = 1,
                PageSize = 100
            };
            var result = await Handler.GetByPeriodAsync(request);

            if (result.IsSucess)
            {
                Transactions = result.Data ?? [];
            }
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task OnDeleteAsync(long id, string title)
    {
        IsBusy = true;

        try
        {
            var result = await Handler.DeleteAsync(new DeleteTransactionRequest { Id = id });
            if (result.IsSucess)
            {
                Snackbar.Add($"Lançamento {title} removido com sucesso!", Severity.Success);
                Transactions.RemoveAll(x => x.Id == id);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    #endregion
}