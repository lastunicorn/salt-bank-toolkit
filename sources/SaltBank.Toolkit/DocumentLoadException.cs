namespace DustInTheWind.SaltBank.Toolkit;

public class DocumentLoadException : Exception
{
	private const string DefaultMessage = "The statement document is invalid.";

	public DocumentLoadException()
		: base(DefaultMessage)
	{
	}

	public DocumentLoadException(Exception innerException)
		: base(DefaultMessage, innerException)
	{
	}

	public DocumentLoadException(string message)
		: base(message)
	{
	}

	public DocumentLoadException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}