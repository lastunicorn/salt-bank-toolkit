using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using DustInTheWind.SaltBank.Toolkit.Csv;

namespace DustInTheWind.SaltBank.Toolkit;

/// <summary>
/// Contains a list of bank transactions. It is rendered as a csv file.
/// </summary>
public class StatementDocument : Collection<BankTransaction>
{
    public string Currency { get; set; }

    public static async Task<StatementDocument> LoadFromFileAsync(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        try
        {
            using StreamReader streamReader = File.OpenText(filePath);
            return await LoadInternalAsync(streamReader);
        }
        catch (DocumentLoadException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new DocumentLoadException(ex);
        }
    }

    public static async Task<StatementDocument> LoadAsync(string csv)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(csv);

        try
        {
            using StringReader stringReader = new(csv);
            return await LoadInternalAsync(stringReader);
        }
        catch (DocumentLoadException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new DocumentLoadException(ex);
        }
    }

    public static async Task<StatementDocument> LoadAsync(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        try
        {
            using StreamReader streamReader = new(stream);
            return await LoadInternalAsync(streamReader);
        }
        catch (DocumentLoadException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new DocumentLoadException(ex);
        }
    }

    public static async Task<StatementDocument> LoadAsync(FileInfo fileInfo)
    {
        ArgumentNullException.ThrowIfNull(fileInfo);

        try
        {
            using StreamReader streamReader = fileInfo.OpenText();
            return await LoadInternalAsync(streamReader);
        }
        catch (DocumentLoadException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new DocumentLoadException(ex);
        }
    }

    public static Task<StatementDocument> LoadAsync(StreamReader streamReader)
    {
        ArgumentNullException.ThrowIfNull(streamReader);

        return LoadInternalAsync(streamReader);
    }

    public static Task<StatementDocument> LoadAsync(TextReader textReader)
    {
        ArgumentNullException.ThrowIfNull(textReader);

        return LoadInternalAsync(textReader);
    }

    private static async Task<StatementDocument> LoadInternalAsync(TextReader textReader)
    {
        try
        {
            StatementCsvDocument statementCsvDocument = new(textReader);
            StatementDocument statementDocument = new();

            StatementCsvHeader statementCsvHeader = await statementCsvDocument.ReadHeaderRowAsync();
            statementDocument.Currency = statementCsvHeader.Currency;

            await foreach (BankTransaction bankTransaction in statementCsvDocument.ReadTransactionsAsync())
                statementDocument.Add(bankTransaction);

            return statementDocument;
        }
        catch (DocumentLoadException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new DocumentLoadException(ex);
        }
    }
}