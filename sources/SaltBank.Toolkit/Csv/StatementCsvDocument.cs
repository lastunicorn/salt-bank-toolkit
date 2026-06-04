using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace DustInTheWind.SaltBank.Toolkit.Csv;

internal sealed class StatementCsvDocument : IDisposable
{
    private readonly CsvReader csvReader;
    private DocumentReadState state;
    private StatementCsvHeader statementCsvHeader;

    public StatementCsvDocument(TextReader textReader)
    {
        CsvConfiguration csvConfiguration = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim,
            PrepareHeaderForMatch = args => args.Header.Trim()
        };

        csvReader = new CsvReader(textReader, csvConfiguration);
    }

    public async Task<StatementCsvHeader> ReadHeaderRowAsync()
    {
        if (state != DocumentReadState.HeaderRow)
            throw new InvalidOperationException("CSV header row has already been read.");

        try
        {
            statementCsvHeader = await StatementCsvHeader.ParseAsync(csvReader);
            state = DocumentReadState.DataRow;
            return statementCsvHeader;
        }
        catch (DocumentLoadException)
        {
            state = DocumentReadState.Ended;
            throw;
        }
        catch (Exception ex)
        {
            state = DocumentReadState.Ended;
            throw new DocumentLoadException(ex);
        }
    }

    public async IAsyncEnumerable<BankTransaction> ReadTransactionsAsync()
    {
        if (state == DocumentReadState.HeaderRow)
            statementCsvHeader = await ReadHeaderRowAsync();

        if (state != DocumentReadState.DataRow)
            throw new InvalidOperationException("CSV document is not in a valid state to read transactions.");

        csvReader.Context.RegisterClassMap(new BankTransactionMap(statementCsvHeader?.Currency));

        await foreach (BankTransaction bankTransaction in csvReader.GetRecordsAsync<BankTransaction>())
            yield return bankTransaction;
    }

    public void Dispose()
    {
        csvReader?.Dispose();
    }
}