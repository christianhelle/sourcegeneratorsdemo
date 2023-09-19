
// using System.Text;
// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.Text;
// using NSwag;
// using NSwag.CodeGeneration.CSharp;

// namespace sourcegenerator;

// [Generator]
// public class ContractGenerator : IIncrementalGenerator
// {
//     public void Initialize(IncrementalGeneratorInitializationContext context)
//     {
//         var sourceFiles = context
//             .AdditionalTextsProvider
//             .Where(text => text.Path.EndsWith(".openapi.json", StringComparison.InvariantCultureIgnoreCase))
//             .Select(GenerateCode);

//         context.RegisterImplementationSourceOutput(
//             sourceFiles, 
//             (c, s) => c.AddSource("Contracts.g.cs", (SourceText)s));
//     }

//     private object GenerateCode(AdditionalText text, CancellationToken token)
//     {
//         var content = text.GetText(token);
//         var json = content!.ToString();
//         var document = OpenApiDocument
//             .FromJsonAsync(json, token)
//             .GetAwaiter()
//             .GetResult()!;

//         var csharpGenerator = new CSharpClientGenerator(
//             document, 
//             new CSharpClientGeneratorSettings
//             {
//                 GenerateClientClasses = false,
//                 GenerateDtoTypes = true,
//                 GenerateClientInterfaces = false,
//                 GenerateExceptionClasses = false,
//             });

//         var code = csharpGenerator.GenerateFile();
//         return SourceText.From(code, Encoding.UTF8);
//     }
// }