using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoTrail.code.input;

public static class MouseManager
{
  static MouseState curMouseState;
  static MouseState prevMouseState;

  public static MouseState UpdateState()
  {
    prevMouseState = curMouseState;
    curMouseState = Mouse.GetState();
    return curMouseState;
  }

  public static Point MousePos => curMouseState.Position;

  public static bool IsLeftButtonPressed()
      => curMouseState.LeftButton == ButtonState.Pressed;

  public static bool IsLeftHasBeenJustPressed()
      => curMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton != ButtonState.Pressed;

  public static bool IsRightButtonPressed()
         => curMouseState.RightButton == ButtonState.Pressed;

  public static bool IsRightHasBeenJustPressed()
      => curMouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton != ButtonState.Pressed;

  public static bool IsLeftButtonReleased()
      => curMouseState.LeftButton == ButtonState.Released;

  public static bool IsLeftHasBeenJustReleased()
      => curMouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton != ButtonState.Released;

  public static bool IsRightButtonReleased()
         => curMouseState.RightButton == ButtonState.Released;

  public static bool IsRightHasBeenJustReleased()
      => curMouseState.RightButton == ButtonState.Released && prevMouseState.RightButton != ButtonState.Released;
}
