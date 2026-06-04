namespace DustInTheWind.SaltBank.Toolkit;

public sealed record class SpendingCategory
{
    public static readonly SpendingCategory BillsAndServices = new("BILLS_AND_SERVICES");
    public static readonly SpendingCategory EatingOut = new("EATING_OUT");
    public static readonly SpendingCategory Entertainment = new("ENTERTAINMENT");
    public static readonly SpendingCategory General = new("GENERAL");
    public static readonly SpendingCategory Groceries = new("GROCERIES");
    public static readonly SpendingCategory Income = new("INCOME");
    public static readonly SpendingCategory Lifestyle = new("LIFESTYLE");
    public static readonly SpendingCategory Payments = new("PAYMENTS");
    public static readonly SpendingCategory Transfers = new("TRANSFERS");
    public static readonly SpendingCategory Transport = new("TRANSPORT");

    private static readonly Dictionary<string, SpendingCategory> KnownValues = new(StringComparer.OrdinalIgnoreCase)
    {
        [BillsAndServices.Value] = BillsAndServices,
        [EatingOut.Value] = EatingOut,
        [Entertainment.Value] = Entertainment,
        [General.Value] = General,
        [Groceries.Value] = Groceries,
        [Income.Value] = Income,
        [Lifestyle.Value] = Lifestyle,
        [Payments.Value] = Payments,
        [Transfers.Value] = Transfers,
        [Transport.Value] = Transport
    };

    public string Value { get; }

    public SpendingCategory(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator SpendingCategory(string value)
    {
        return value == null
            ? null
            : new SpendingCategory(value);
    }

    public static implicit operator string(SpendingCategory spendingCategory)
    {
        return spendingCategory?.Value;
    }
}