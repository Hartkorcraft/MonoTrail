using Microsoft.Xna.Framework;

namespace MonoTrail.code.ext;

public static class GameExtensions
{
  public static float ToDelta(this GameTime gameTime)
    => (float)gameTime.ElapsedGameTime.TotalSeconds;

  public static float Lerp(this float x, float y, float weight)
    => MathHelper.Lerp(x, y, weight);
}
