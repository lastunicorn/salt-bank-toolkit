namespace DustInTheWind.SaltBank.Toolkit;

/// <summary>
/// Represents a bank transaction from Salt Bank.
/// </summary>
public record class BankTransaction
{
    public DateOnly Date { get; set; }

    public string CounterParty { get; set; }

    public string Reference { get; set; }

    public TransactionType Type { get; set; }

    public decimal Amount { get; set; }

    public decimal Balance { get; set; }

    public SpendingCategory SpendingCategory { get; set; }

    public string Notes { get; set; }
}