namespace DustInTheWind.SaltBank.ToolKit;

public class StatementDataException : StatementDocumentException
{
    public StatementDataException(Exception innerException)
        : base("The CSV data is invalid.", innerException)
    {
    }
}

