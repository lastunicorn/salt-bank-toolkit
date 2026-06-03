namespace DustInTheWind.SaltBank.Toolkit.Tests.Helpers;

/// <summary>
/// A <see cref="TextReader"/> that throws an <see cref="IOException"/> on the first read,
/// used to exercise the generic exception catch branch in <c>StatementsDocument.Load</c>.
/// </summary>
public class FailingTextReader : TextReader
{
    public override int Read(char[] buffer, int index, int count)
    {
        throw new IOException("Simulated I/O failure.");
    }

    public override int Read()
    {
        throw new IOException("Simulated I/O failure.");
    }
}

