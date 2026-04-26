using CsvHelper.Configuration;

namespace DustInTheWind.SaltBank.ToolKit;

internal sealed class BankTransactionMap : ClassMap<BankTransaction>
{
    public BankTransactionMap()
    {
        Map(x => x.Date)
            .Name("Date")
            .TypeConverterOption.Format("dd/MM/yyyy");

        Map(x => x.CounterParty).Name("Counter Party");
        Map(x => x.Reference).Name("Reference");
        Map(x => x.Type).Name("Type");
        Map(x => x.Amount).Name("Amount (RON)");
        Map(x => x.Balance).Name("Balance (RON)");
        Map(x => x.SpendingCategory).Name("Spending Category");
        Map(x => x.Notes).Name("Notes").Optional();
    }
}