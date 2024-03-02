using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoTrail.code.input;
using MonoTrail.code.tactics.data.turn;
using MonoTrail.code.tactics.logic.turn.moves;

namespace MonoTrail.code.tactics.logic.turn;

public class Turn(
    IEnumerable<InputMove> WanderCommands,
    IEnumerable<InputMove> AimCommands)
{
  public Unit Execute(GameState gameState, GameTime gameTime)
    => gameState.Scene.Left.TurnData.CurrentChooseState switch
    {
      ChooseState.Wander => SelectMove(WanderCommands, gameState, gameTime),
      ChooseState.Aim => SelectMove(AimCommands, gameState, gameTime),
      _ => throw new NotImplementedException()
    };

  static Unit SelectMove(IEnumerable<InputMove> inputMoves, GameState gameState, GameTime gameTime)
  {
    var tactics = gameState.Scene.Left;
    var curSelection = tactics.TurnData.CurrentSelection
        .MatchOr(() => throw new InvalidOperationException());

    var args = new MoveCommandArgs(
        EntityID: curSelection);

    foreach (var c in inputMoves)
      if (c.Input.HasBeenJustPressed() && c.GetMoveCommand(args).Execute(gameState, gameTime))
        return default;
    return default;
  }
}
