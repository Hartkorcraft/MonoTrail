using TrailCore.data.tactics;

namespace TrailCore.tactics.data.components;

public record HealthComponent(int Current, int Max) : IComponent
{
    public int Current { get; set; } = Current;
    public int Max { get; set; } = Max;
}
