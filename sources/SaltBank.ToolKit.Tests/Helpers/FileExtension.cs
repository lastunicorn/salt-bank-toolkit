namespace DustInTheWind.SaltBank.ToolKit.Tests.Helpers;

public readonly record struct FileExtension
{
    public static readonly FileExtension Csv = new("csv");
    public static readonly FileExtension Json = new("json");
    public static readonly FileExtension Txt = new("txt");
    
    public string Value { get; }

    public FileExtension(string extension)
    {
        Value = extension;
    }

    public override string ToString()
    {
        return Value;
    }
    
    public static implicit operator string(FileExtension extension)
    {
        return extension.Value;
    }
    
    public static implicit operator FileExtension(string extension)
    {
        return new FileExtension(extension);
    }
}