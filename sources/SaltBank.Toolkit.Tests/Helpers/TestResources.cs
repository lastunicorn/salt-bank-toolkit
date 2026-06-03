using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DustInTheWind.SaltBank.Toolkit.Tests.Helpers;

public static class TestResources
{
    /// <summary>
    /// Loads an embedded text resource. The Resource file must be in the "&lt;TestClass&gt;.resources" subdirectory.
    /// Resource naming convention: &lt;CallerNamespace&gt;.&lt;CallerTypeName&gt;.resources.&lt;CallerMethodName&gt;.&lt;Extension&gt;
    /// </summary>
    /// <param name="fileName">The name of the resource file, including extension (e.g., "data.csv").</param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetEmbeddedResourceAsText(string fileName)
    {
        MethodBase callerMethod = GetCallingMethod();
        Type declaringType = callerMethod.DeclaringType;
        
        if (declaringType == null)
            throw new InvalidOperationException("Unable to determine the declaring type of the calling test method.");

        string resourceName = $"{declaringType.Namespace}.{declaringType.Name}.resources.{fileName}";
        Assembly assembly = declaringType.Assembly;
        using Stream stream = assembly.GetManifestResourceStream(resourceName);
        
        if (stream == null)
            throw new InvalidOperationException($"Embedded resource not found: {resourceName}");

        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
    
    /// <summary>
    /// Loads an embedded text resource. The Resource file must be in the "&lt;TestClass&gt;.resources" subdirectory.
    /// Resource naming convention: &lt;CallerNamespace&gt;.&lt;CallerTypeName&gt;.resources.&lt;CallerMethodName&gt;.&lt;Extension&gt;
    /// </summary>
    /// <param name="extension">The file extension of the resource (e.g., "csv", "json", "txt").</param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetEmbeddedResourceAsText(FileExtension extension)
    {
        MethodBase callerMethod = GetCallingMethod();
        Type declaringType = callerMethod.DeclaringType;
        
        if (declaringType == null)
            throw new InvalidOperationException("Unable to determine the declaring type of the calling test method.");

        string resourceName = $"{declaringType.Namespace}.{declaringType.Name}.resources.{callerMethod.Name}.{extension}";
        Assembly assembly = declaringType.Assembly;
        using Stream stream = assembly.GetManifestResourceStream(resourceName);
        
        if (stream == null)
            throw new InvalidOperationException($"Embedded resource not found: {resourceName}");

        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static MethodBase GetCallingMethod()
    {
        StackTrace stackTrace = new(false);

        foreach (StackFrame frame in stackTrace.GetFrames())
        {
            MethodBase method = frame.GetMethod();
            Type declaringType = method?.DeclaringType;

            if (declaringType is null)
                continue;

            if (declaringType == typeof(TestResources))
                continue;

            return method;
        }

        throw new InvalidOperationException("Unable to determine the calling test method from the current stack trace.");
    }
}