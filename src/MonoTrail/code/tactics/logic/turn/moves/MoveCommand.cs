using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TrailCore.tactics.data;

namespace MonoTrail.code.tactics.logic.turn.moves;

public abstract record MoveCommand(int Cost = 0)
{
  public abstract bool Execute(GameState gameState, GameTime gameTime);
}

public record MoveCommandArgs(EntityID EntityID);

public record InputMove(Keys Input, Func<MoveCommandArgs, MoveCommand> GetMoveCommand);
