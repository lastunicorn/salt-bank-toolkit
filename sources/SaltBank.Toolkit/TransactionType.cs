namespace DustInTheWind.SaltBank.Toolkit;

public sealed record class TransactionType
{
    public static readonly TransactionType CardPayment = new("Card Payment");
    public static readonly TransactionType CardSubscription = new("Card Subscription");
    public static readonly TransactionType Contactless = new("Contactless");
    public static readonly TransactionType CurrencyTransfer = new("Currency Transfer");
    public static readonly TransactionType IntrabankPayment = new("INTRABANK PAYMENT");
    public static readonly TransactionType OnlinePayment = new("Online Payment");
    public static readonly TransactionType Transfer = new("Transfer");
    public static readonly TransactionType TransfondSentInstant = new("TRANSFOND SENT INSTANT");

    private static readonly Dictionary<string, TransactionType> KnownValues = new(StringComparer.OrdinalIgnoreCase)
    {
        [CardPayment.Value] = CardPayment,
        [CardSubscription.Value] = CardSubscription,
        [Contactless.Value] = Contactless,
        [CurrencyTransfer.Value] = CurrencyTransfer,
        [IntrabankPayment.Value] = IntrabankPayment,
        [OnlinePayment.Value] = OnlinePayment,
        [Transfer.Value] = Transfer,
        [TransfondSentInstant.Value] = TransfondSentInstant
    };

    public string Value { get; }

    public TransactionType(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator TransactionType(string value)
    {
        return value == null
            ? null
            : new TransactionType(value);
    }

    public static implicit operator string(TransactionType spendingCategory)
    {
        return spendingCategory?.Value;
    }
}