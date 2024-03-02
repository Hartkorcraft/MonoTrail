using Microsoft.Xna.Framework;
using MonoTrail.code.data;
using MonoTrail.code.rendering.data.camera;

namespace MonoTrail.code.story;

public interface IViewSystem
{
  public void Render(RenderParams renderParams);
  public void Update(GameState gameState, GameTime gameTime);
}
