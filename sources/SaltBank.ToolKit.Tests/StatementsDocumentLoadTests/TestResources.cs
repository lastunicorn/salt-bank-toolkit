using System.Reflection;

namespace DustInTheWind.SaltBank.ToolKit.Tests.StatementsDocumentLoadTests;

public static class TestResources
{
    /// <summary>
    /// Loads an embedded CSV resource by test method name.
    /// Resource naming convention: LoadTests_<MethodName>.csv
    /// </summary>
    public static string GetEmbeddedTextFile(string testMethodName)
    {
        string resourceName = $"DustInTheWind.SaltBank.ToolKit.Tests.StatementsDocumentLoadTests.LoadTests_{testMethodName}.csv";
        Assembly assembly = typeof(LoadTests).Assembly;
        using Stream stream = assembly.GetManifestResourceStream(resourceName);
        
        if (stream == null)
            throw new InvalidOperationException($"Embedded CSV resource not found: {resourceName}");
        
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}