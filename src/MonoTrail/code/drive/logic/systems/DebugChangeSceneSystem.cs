using System;
using Microsoft.Xna.Framework;
using MonoTrail.code.input;
using MonoTrail.code.logic;

namespace MonoTrail.code.drive.logic.systems;

public class DebugChangeSceneSystem(Func<RequestedSceneChange> getSceneChangeEvent) : ISystem
{
  public void Update(GameState gameState, GameTime gameTime)
  {
    if (Inputs.DEBUG_KEY.HasBeenJustPressed())
    {
      getSceneChangeEvent()?.Invoke(gameState.InitTacticsScene());
    }
  }
}
