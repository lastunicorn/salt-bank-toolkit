using System.Text.RegularExpressions;
using CsvHelper;

namespace DustInTheWind.SaltBank.Toolkit.Csv;

internal class StatementCsvHeader
{
    public string[] Headers { get; init; }

    public Currency Currency { get; init; }

    public static async Task<StatementCsvHeader> ParseAsync(CsvReader csvReader)
    {
        await csvReader.ReadAsync();
        bool success = csvReader.ReadHeader();

        if (!success)
            throw new DocumentLoadException("The CSV file must contain a header record.");

        string[] headers = csvReader.HeaderRecord;

        if (headers == null)
            throw new DocumentLoadException("The CSV file must contain a header record.");

        Currency currency = IdentifyCurrency(headers);

        return new StatementCsvHeader
        {
            Headers = headers,
            Currency = currency
        };
    }

    private static Currency IdentifyCurrency(string[] headers)
    {
        foreach (string header in headers)
        {
            Match match = Regex.Match(header.Trim(), @"^Amount \((.+)\)$");

            if (!match.Success)
                continue;

            return match.Groups[1].Value;
        }

        throw new DocumentLoadException("The currency cannot by identified from the header of the CSV file.");
    }
}