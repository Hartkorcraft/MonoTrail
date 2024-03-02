using TrailCore.data.tactics;

namespace TrailCore.tactics.data.components;

public record MindComponent : IComponent
{
    public required string Name { get; init; }
    public required data.FactionAligment FactionAligment { get; set; }
    
    // public required TurnPoints TurnPoints { get; init; }
    // public required Brain Brain { get; init; }
    // public ValueDictionary<GridPos, ScanCell> AvailablePositions { get; set; } = [];
}
