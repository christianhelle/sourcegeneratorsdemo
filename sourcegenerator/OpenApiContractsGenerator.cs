using System.IO;
using Microsoft.CodeAnalysis;
using NSwag;
using NSwag.CodeGeneration.CSharp;

[Generator]
public class OpenApiContractsGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        foreach (AdditionalText file in context.AdditionalFiles)
        {
            var document = OpenApiDocument.FromFileAsync(file.Path).GetAwaiter().GetResult();
            var generator = new CSharpClientGenerator(document, new CSharpClientGeneratorSettings
            {
                GenerateClientClasses = false,
                GenerateDtoTypes = true,
                GenerateClientInterfaces = false,
                GenerateExceptionClasses = false,
                CSharpGeneratorSettings =
                {
                    Namespace = "Generated.Contracts"
                }
            });
            var csharp = generator.GenerateFile();
            var fileInfo = new FileInfo(file.Path);
            context.AddSource(fileInfo.Name, csharp);
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
    }
}