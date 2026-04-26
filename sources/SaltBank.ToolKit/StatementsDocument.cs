using System.Collections.ObjectModel;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace DustInTheWind.SaltBank.ToolKit;

/// <summary>
/// Contains a list of bank transactions. It is rendered as a csv file.
/// </summary>
public class StatementsDocument : Collection<BankTransaction>
{
    public static StatementsDocument Load(string csv)
    {
        if (csv == null)
            throw new ArgumentNullException(nameof(csv));

        if (string.IsNullOrWhiteSpace(csv))
            throw new StatementDocumentException("The CSV content cannot be empty or whitespace.");

        using StringReader stringReader = new(csv);
        return Load(stringReader);
    }

    public static StatementsDocument LoadFile(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        using StreamReader streamReader = File.OpenText(filePath);
        return Load(streamReader);
    }

    public static StatementsDocument Load(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        using StreamReader streamReader = new(stream);
        return Load(streamReader);
    }

    public static StatementsDocument Load(FileInfo fileInfo)
    {
        ArgumentNullException.ThrowIfNull(fileInfo);

        using StreamReader streamReader = fileInfo.OpenText();
        return Load(streamReader);
    }

    public static StatementsDocument Load(StreamReader streamReader)
    {
        ArgumentNullException.ThrowIfNull(streamReader);

        return Load((TextReader)streamReader);
    }

    public static StatementsDocument Load(TextReader textReader)
    {
        ArgumentNullException.ThrowIfNull(textReader);

        return LoadInternal(textReader);
    }

    private static StatementsDocument LoadInternal(TextReader textReader)
    {
        CsvConfiguration csvConfiguration = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim,
            PrepareHeaderForMatch = args => args.Header.Trim()
        };

        using CsvReader csvReader = new(textReader, csvConfiguration);
        csvReader.Context.RegisterClassMap<BankTransactionMap>();

        IEnumerable<BankTransaction> bankTransactions = csvReader.GetRecords<BankTransaction>();

        StatementsDocument statementsDocument = [];

        foreach (BankTransaction bankTransaction in bankTransactions)
            statementsDocument.Add(bankTransaction);

        return statementsDocument;
    }
}