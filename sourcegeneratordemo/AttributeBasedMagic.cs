using SourceGenerator;

namespace SourceGeneratorDemo;

[SprinkleMagic]
public static partial class AttributeBasedMagic
{
    public static partial void SayHello();

    public static partial void SayHelloTo(string myLittleFriend);
}
