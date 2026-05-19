using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace DustInTheWind.SaltBank.ToolKit;

/// <summary>
/// Contains a list of bank transactions. It is rendered as a csv file.
/// </summary>
public class StatementDocument : Collection<BankTransaction>
{
    public string Currency { get; set; }
    
    public static StatementDocument Load(string csv)
    {
        if (csv == null)
            throw new ArgumentNullException(nameof(csv));

        if (string.IsNullOrWhiteSpace(csv))
            throw new StatementDocumentException("The CSV content cannot be empty or whitespace.");

        using StringReader stringReader = new(csv);
        return Load(stringReader);
    }

    public static StatementDocument LoadFile(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        using StreamReader streamReader = File.OpenText(filePath);
        return Load(streamReader);
    }

    public static StatementDocument Load(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        using StreamReader streamReader = new(stream);
        return Load(streamReader);
    }

    public static StatementDocument Load(FileInfo fileInfo)
    {
        ArgumentNullException.ThrowIfNull(fileInfo);

        using StreamReader streamReader = fileInfo.OpenText();
        return Load(streamReader);
    }

    public static StatementDocument Load(StreamReader streamReader)
    {
        ArgumentNullException.ThrowIfNull(streamReader);

        return Load((TextReader)streamReader);
    }

    public static StatementDocument Load(TextReader textReader)
    {
        ArgumentNullException.ThrowIfNull(textReader);

        return LoadInternal(textReader);
    }

    private static StatementDocument LoadInternal(TextReader textReader)
    {
        CsvConfiguration csvConfiguration = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim,
            PrepareHeaderForMatch = args => args.Header.Trim()
        };

        using CsvReader csvReader = new(textReader, csvConfiguration);

        StatementDocument statementDocument = [];

        try
        {
            csvReader.Read();
            csvReader.ReadHeader();

            string currency = IdentifyCurrency(csvReader);
            csvReader.Context.RegisterClassMap(new BankTransactionMap(currency));

            statementDocument.Currency = currency;

            foreach (BankTransaction bankTransaction in csvReader.GetRecords<BankTransaction>())
                statementDocument.Add(bankTransaction);
        }
        catch (HeaderValidationException ex)
        {
            throw new StatementHeaderException(ex);
        }
        catch (ReaderException ex) when (ex is CsvHelper.MissingFieldException || ex.InnerException is HeaderValidationException or CsvHelper.MissingFieldException)
        {
            throw new StatementHeaderException(ex.InnerException);
        }
        catch (ReaderException ex)
        {
            throw new StatementDataException(ex);
        }
        catch (TypeConverterException ex)
        {
            throw new StatementDataException(ex);
        }
        catch (Exception ex)
        {
            throw new StatementDocumentException(ex);
        }

        return statementDocument;
    }

    private static string IdentifyCurrency(CsvReader csvReader)
    {
        string[] headers = csvReader.HeaderRecord;
        
        if (headers == null)
            throw new StatementDocumentException("The CSV file must contain a header record.");
        
        foreach (string header in headers)
        {
            Match match = Regex.Match(header.Trim(), @"^Amount \((.+)\)$");

            if (!match.Success)
                continue;
                
            return match.Groups[1].Value;
        }

        throw new StatementDocumentException("The currency cannot by identified from the header of the CSV file.");
    }
}