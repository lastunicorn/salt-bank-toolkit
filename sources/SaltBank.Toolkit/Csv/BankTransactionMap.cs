using CsvHelper.Configuration;

namespace DustInTheWind.SaltBank.Toolkit.Csv;

internal sealed class BankTransactionMap : ClassMap<BankTransaction>
{
    public BankTransactionMap(Currency currency)
    {
        Map(x => x.Date)
            .Name("Date")
            .TypeConverterOption.Format("dd/MM/yyyy");

        Map(x => x.CounterParty)
            .Name("Counter Party");
        
        Map(x => x.Reference)
            .Name("Reference");
        
        Map(x => x.Type)
            .Name("Type")
            .TypeConverter<TransactionTypeConverter>();
        
        Map(x => x.Amount)
            .Name($"Amount ({currency})");
        
        Map(x => x.Balance)
            .Name($"Balance ({currency})");
        
        Map(x => x.SpendingCategory)
            .Name("Spending Category")
            .TypeConverter<SpendingCategoryConverter>();
        
        Map(x => x.Notes)
            .Name("Notes")
            .Optional();
    }
}