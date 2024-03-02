using Microsoft.Xna.Framework;
using MonoTrail.code.tactics.data.turn;

namespace MonoTrail.code.tactics.logic.turn.moves;

public record ChangeChooseState(ChooseState NewChooseState) : MoveCommand(0)
{
  public override bool Execute(GameState gameState, GameTime gameTime)
  {
    var tactics = gameState.Scene.Left;
    _ = tactics.ChangeChooseState(NewChooseState);
    return true;
  }
}
