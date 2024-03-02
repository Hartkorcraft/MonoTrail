using Microsoft.Xna.Framework.Input;
using MonoTrail.code.rendering.camera;

namespace MonoTrail.code.tactics.logic;

public static class TilemapService
{
  public static (int x, int y) WorldToMap(this Vec2 pos, int tileSize)
      => ((int)pos.X / tileSize, (int)pos.Y / tileSize);

  public static GridPos GetMouseMapPos(Camera camera, int tileSize)
      => camera
        .ScreenToWorld(Mouse
        .GetState().Position
        .ToVector2()).WorldToMap(tileSize);
}
