using Riok.Mapperly.Abstractions;

namespace SourceGeneratorDemo;

[Mapper]
public static partial class NotAutoMapper
{
    public static partial FooContract ToContract(this FooModel model);

    public static partial FooModel ToModel(this FooContract contract);
}
