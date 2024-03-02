using TrailCore.data.tactics;

namespace TrailCore.tactics.data.components;

public record MapPosComponent : IComponent
{
    public required bool IsBlockingPos { get; set; }
    public required (int x, int y) MapPos { get; set; }
}