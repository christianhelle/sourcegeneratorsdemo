namespace SourceGeneratorDemo;

internal static partial class Program
{
    private static void Main()
    {
        HelloWorldGenerated.HelloWorld.SayHello();

        var model = new FooModel("John Doe", 42, "foo@email.com");
        var contract = model.ToContract();
        Console.WriteLine($"Hello {contract.Name}!");

        AttributeBasedMagic.SayHello();
        
        new Generated.Contracts.Customer();
    }
}