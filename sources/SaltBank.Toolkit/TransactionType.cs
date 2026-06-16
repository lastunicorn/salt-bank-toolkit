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

	public static readonly IReadOnlyCollection<TransactionType> KnownValues =
	[
		CardPayment,
		CardSubscription,
		Contactless,
		CurrencyTransfer,
		IntrabankPayment,
		OnlinePayment,
		Transfer,
		TransfondSentInstant
	];

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