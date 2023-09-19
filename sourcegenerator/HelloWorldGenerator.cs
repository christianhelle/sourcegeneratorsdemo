using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

[Generator]
public class HelloWorldGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        context.AddSource(
            "HelloWorldGenerator.g",
            SourceText.From(
                """
                namespace HelloWorldGenerated 
                {
                    public static class HelloWorld 
                    {
                        public static void SayHello() =>
                            Console.WriteLine("Hello from generated code!");
                    }
                }
                """,
                Encoding.UTF8));
    }

    public void Initialize(GeneratorInitializationContext context)
    {
    }
}