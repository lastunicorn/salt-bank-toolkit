using CsvHelper;

namespace DustInTheWind.SaltBank.ToolKit;

public class StatementDataException : StatementDocumentException
{
    public StatementDataException(ReaderException innerException)
        : base("The CSV data is invalid.", innerException)
    {
    }
}

