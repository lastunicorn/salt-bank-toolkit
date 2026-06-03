namespace DustInTheWind.SaltBank.Toolkit;

public class StatementDocumentException : Exception
{
    private const string DefaultMessage = "The statement document is invalid.";

    public StatementDocumentException()
        : base(DefaultMessage)
    {
    }

    public StatementDocumentException(Exception innerException)
        : base(DefaultMessage, innerException)
    {
    }

    public StatementDocumentException(string message)
        : base(message)
    {
    }

    public StatementDocumentException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}