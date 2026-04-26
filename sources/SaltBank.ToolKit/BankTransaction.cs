namespace DustInTheWind.SaltBank.ToolKit;

public class BankTransaction
{
    public DateOnly Date { get; set; }

    public string CounterParty { get; set; } = string.Empty;

    public string Reference { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public double Amount { get; set; }

    public double Balance { get; set; }

    public string SpendingCategory { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;
}