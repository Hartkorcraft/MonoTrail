using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoTrail.code.data;
using MonoTrail.code.input;
using MonoTrail.code.logic;
using MonoTrail.code.rendering.camera;

namespace MonoTrail.code.story;

public class ButtonPressedSystem(IEnumerable<ButtonData> buttons, Camera camera) : ISystem
{
  public void Update(GameState gameState, GameTime gameTime)
  {
    foreach (var b in buttons)
    {
      if (MouseManager.IsLeftHasBeenJustReleased() && b.Rect.ContainsMouse(camera))
      {
        _ = gameState.ExecuteEvent(b.StoryEvent);
        return;
      }
    }
  }
}
