using Microsoft.Xna.Framework;
using System.Linq;
using FuNK.collections;
using TrailCore.tactics.data.components;
using TrailCore.tactics.data;

namespace MonoTrail.code.tactics.logic.turn.moves;

public record NextCharacter() : MoveCommand
{
  public override bool Execute(GameState gameState, GameTime gameTime)
  {
    var tactics = gameState.Scene.Left;
    var turnType = tactics.TurnData.CurrentTurnType;

    FactionAligment[] factions = turnType switch
    {
      data.TurnType.Player => [FactionAligment.Player],
      data.TurnType.Enemy => [FactionAligment.Monsters],
      _ => []
    };

    var minds = tactics.EntityData.ComponentStorage
      .GetComponents<MindComponent>()
      .Where(m => factions.Any(f => f == m.Value.FactionAligment))
      .Select(m => m.Key);

    var curSelection = tactics.TurnData.CurrentSelection;

    var next = minds.NextInList(curSelection.Or(minds.First()));

    _ = tactics.ChangeSelection(next);

    curSelection.MatchEffect(
        tactics.ResetScene,
        e => e.ResetEntity(tactics));

    return true;
  }
}
