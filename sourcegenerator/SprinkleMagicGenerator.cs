using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SourceGenerator;

public class SprinkleMagicAttribute : Attribute { }

[Generator]
public class SprinkleMagicGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var attributeSymbol = context.Compilation.GetTypeByMetadataName(typeof(SprinkleMagicAttribute).FullName);

        var classesWithAttributes = context.Compilation.SyntaxTrees
                    .Where(st => st
                        .GetRoot()
                        .DescendantNodes()
                        .OfType<ClassDeclarationSyntax>()
                        .Any(p => p.DescendantNodes().OfType<AttributeSyntax>().Any()));

        foreach (SyntaxTree tree in classesWithAttributes)
        {
            var semanticModel = context.Compilation.GetSemanticModel(tree);
            var declaredClasses = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(cd => cd.DescendantNodes().OfType<AttributeSyntax>().Any());

            foreach (var declaredClass in declaredClasses)
            {
                var nodes = declaredClass
                                .DescendantNodes()
                                .OfType<AttributeSyntax>()
                                .FirstOrDefault(a => a.DescendantTokens().Any(dt => dt.IsKind(SyntaxKind.IdentifierToken) && semanticModel.GetTypeInfo(dt.Parent).Type.Name == attributeSymbol.Name))
                                ?.DescendantTokens()
                                ?.Where(dt => dt.IsKind(SyntaxKind.IdentifierToken))
                                ?.ToList();

                if (nodes == null)
                {
                    continue;
                }

                var relatedClass = semanticModel.GetTypeInfo(nodes.Last().Parent);
                var stringBuilder = this.GenerateClass("SourceGeneratorDemo", declaredClass.Identifier.ToString());

                foreach (MethodDeclarationSyntax classMethod in declaredClass.Members.Where(m => m.IsKind(SyntaxKind.MethodDeclaration)).OfType<MethodDeclarationSyntax>())
                {
                    this.GenerateMethod(classMethod, ref stringBuilder);
                }

                this.CloseClass(stringBuilder);
                context.AddSource($"{declaredClass.Identifier}_{relatedClass.Type.Name}", SourceText.From(stringBuilder.ToString(), Encoding.UTF8));
            }
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // Nothing to do here
    }

    private void GenerateMethod(MethodDeclarationSyntax methodDeclaration, ref StringBuilder builder)
    {
        var signature = $"{methodDeclaration.Modifiers} {methodDeclaration.ReturnType} {methodDeclaration.Identifier}(";
        var parameters = methodDeclaration.ParameterList.Parameters;
        signature += string.Join(", ", parameters.Select(p => p.ToString())) + ")";
        builder.AppendLine(signature + " => System.Console.WriteLine(\"Greetings from magic sprinkle generator!\");");
    }

    private StringBuilder GenerateClass(string namespaceName, string className)
    {
        return new StringBuilder(
            """
            using System;
            using System.Collections.Generic;

            namespace [NAMESPACE]
            {
                public partial class [CLASS]
                {

            """
            .Replace("[NAMESPACE]", namespaceName)
            .Replace("[CLASS]", className));
    }

    private void CloseClass(StringBuilder generatedClass)
    {
        generatedClass.AppendLine(
@"    }
}");
    }
}