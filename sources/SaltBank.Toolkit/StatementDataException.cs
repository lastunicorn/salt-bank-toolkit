namespace DustInTheWind.SaltBank.Toolkit;

public class StatementDataException : StatementDocumentException
{
    public StatementDataException(Exception innerException)
        : base("The CSV data is invalid.", innerException)
    {
    }
}

