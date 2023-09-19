using AutoCtor;

namespace SourceGeneratorDemo;

public interface IFooService {}
public interface IBarService {}

[AutoConstruct]
public partial class AutoCtorExample
{
    private readonly IFooService fooService;
    private readonly IBarService barService;
}
