using Microsoft.Xna.Framework;

namespace MonoTrail.code.tactics.logic.turn.moves;

public record MoveSelector(
    GridPos MoveBy,
    int TileSize) : MoveCommand(0)
{
  public override bool Execute(GameState gameState, GameTime gameTime)
  {
    var tactics = gameState.Scene.Left;
    var selector = tactics.SelectorData;
    _ = selector.MoveSelectorBy(MoveBy, TileSize);
    return true;
  }
}
