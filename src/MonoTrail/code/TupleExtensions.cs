using Microsoft.Xna.Framework;

namespace MonoTrail.code;

public static class TupleExtensions
{
  public static Vec2 ToVec2(this (int x, int y) value)
      => new(value.x, value.y);

  public static Point ToPoint(this (int x, int y) value)
      => new(value.x, value.y);

  public static Vec2 ToTileCenter(this (int x, int y) value, int tileSize)
      => new((value.x * tileSize) + (tileSize / 2), (value.y * tileSize) + (tileSize / 2));

  public static Vec2 ToTileWorldPos(this (int x, int y) value, int tileSize)
      => new(value.x * tileSize, value.y * tileSize);
}
