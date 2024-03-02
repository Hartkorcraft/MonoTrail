using Microsoft.Xna.Framework;
using MonoTrail.code.data;

namespace MonoTrail.code;

public interface ISystem
{
  public void Update(GameState gameState, GameTime gameTime);
}
