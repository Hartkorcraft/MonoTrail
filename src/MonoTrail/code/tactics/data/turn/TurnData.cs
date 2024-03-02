using TrailCore.tactics.data;

namespace MonoTrail.code.tactics.data.turn;

public record TurnData
{
  public required TurnType CurrentTurnType { get; set; }
  public ChooseState CurrentChooseState { get; set; } = ChooseState.Wander;
  public Option<EntityID> CurrentSelection { get; set; } = None;
}

public enum ChooseState
{
  Wander, Aim
}
