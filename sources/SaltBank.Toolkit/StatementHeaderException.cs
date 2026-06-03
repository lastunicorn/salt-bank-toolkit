namespace DustInTheWind.SaltBank.Toolkit;

public class StatementHeaderException : StatementDocumentException
{
    public StatementHeaderException(Exception innerException)
        : base("The CSV header is invalid.", innerException)
    {
    }
}

