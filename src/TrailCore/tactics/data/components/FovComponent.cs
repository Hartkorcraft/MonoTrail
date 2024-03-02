
using TrailCore.data.tactics;

namespace TrailCore.tactics.data.components;

public record FovComponent : IComponent
{
  public bool IsSeeing { get; set; } = true;
  public Option<int> LookingDirection { get; set; } = None;  // TODO ENUM
  public HashSet<GridPos> Seeing { get; set; } = [];
  public HashSet<GridPos> Saw { get; set; } = [];
}
