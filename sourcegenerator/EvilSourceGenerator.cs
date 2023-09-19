using System.Diagnostics;
using Microsoft.CodeAnalysis;

[Generator]
public class EvilSourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        var psi = new ProcessStartInfo("https://github.com")
        {
            UseShellExecute = true,
            Verb = "open"
        };
        Process.Start(psi);
    }
}
