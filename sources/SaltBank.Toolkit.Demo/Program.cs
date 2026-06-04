using System.Globalization;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.SaltBank.Toolkit;

namespace SaltBank.Toolkit.Demo;

internal static class Program
{
    private static async Task Main(string[] args)
    {const string fileName = "statement.csv";

        try
        {
            StatementDocument document = await StatementDocument.LoadFromFileAsync(fileName);

            DataGrid dataGrid = Display(document);
            dataGrid.Display();
        }
        catch (DocumentLoadException ex)
        {
            await Console.Error.WriteLineAsync($"Failed to read '{fileName}': {ex}");
            Environment.ExitCode = 1;
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"Unexpected error: {ex}");
            Environment.ExitCode = 1;
        }
    }

    private static DataGrid Display(StatementDocument document)
    {
        DataGrid dataGrid = new()
        {
            Title = $"Transactions ({document.Currency})",
            BorderTemplate = BorderTemplate.PlusMinusBorderTemplate,
            Footer = $"Count: {document.Count}"
        };

        dataGrid.Columns.Add("Date");
        dataGrid.Columns.Add("CounterParty");
        dataGrid.Columns.Add("Reference");
        dataGrid.Columns.Add("Type");
        dataGrid.Columns.Add("Amount", HorizontalAlignment.Right);
        dataGrid.Columns.Add("Balance", HorizontalAlignment.Right);
        dataGrid.Columns.Add("SpendingCategory");
        dataGrid.Columns.Add("Notes");

        foreach (BankTransaction transaction in document)
        {
            dataGrid.Rows.Add(
                transaction.Date.ToString("yyyy-MM-dd"),
                transaction.CounterParty,
                transaction.Reference,
                transaction.Type,
                transaction.Amount.ToString(CultureInfo.CurrentUICulture),
                transaction.Balance.ToString(CultureInfo.CurrentUICulture),
                transaction.SpendingCategory,
                transaction.Notes);
        }

        return dataGrid;
    }
}