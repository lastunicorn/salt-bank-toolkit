namespace DustInTheWind.SaltBank.Toolkit;

public class HeaderLoadException : DocumentLoadException
{
    public HeaderLoadException(Exception innerException)
        : base("The CSV header is invalid.", innerException)
    {
    }
}

