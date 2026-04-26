namespace DustInTheWind.SaltBank.ToolKit;

public class StatementDocumentException : Exception
{
    public StatementDocumentException()
        : base("The statement document is invalid.")
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

