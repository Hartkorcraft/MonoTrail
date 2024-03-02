using System;

namespace MonoTrail.code.ext;

public static class Vec2Extensions
{
  public static readonly Vec2 Up = new(0, -1);
  public static readonly Vec2 Down = new(0, 1);
  public static readonly Vec2 Left = new(-1, 0);
  public static readonly Vec2 Right = new(1, 0);

  public static GridPos DirTo(this GridPos from, GridPos to)
  {
    GridPos dif = (to.x - from.x, to.y - from.y);
    return Math.Abs(dif.x) > Math.Abs(dif.y)
        ? dif.x < 0 ? (-1, 0) : (1, 0)
        : dif.y < 0 ? (0, -1) : (0, 1);
  }

  public static float Distance(this Vec2 vec1, Vec2 vec2)
    => Vec2.Distance(vec1, vec2);

  public static Vec2 Lerp(this Vec2 vec1, Vec2 vec2, float weight)
    => Vec2.Lerp(vec1, vec2, weight);

  public static Vec2 Abs(this Vec2 value)
    => new(MathF.Abs(value.X), MathF.Abs(value.Y));

  public static bool ValuesUnderOrOnThreshold(this Vec2 vec, float tolerance)
    => vec.X <= tolerance && vec.Y <= tolerance;

  public static Vec2 Snap(this Vec2 value, Vec2 to, float tolerance)
    => (value - to)
      .Abs()
      .ValuesUnderOrOnThreshold(tolerance) ? to : value;

  public static GridPos[] Around(this GridPos vec)
    => [(vec.x, vec.y - 1), (vec.x, vec.y + 1), (vec.x - 1, vec.y), (vec.x + 1, vec.y)];
}
