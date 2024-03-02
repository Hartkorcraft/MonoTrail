using TrailCore.data.tactics.map;
namespace TrailCore.tactics.data.map;

public record Tile()
{
    public GroundType GroundType { get; init; } = GroundType.Dirt;
    public Option<EntityID> Occupying { get; set; } = None;
}
