
using Microsoft.Xna.Framework.Graphics;
using MonoTrail.code.rendering.data.camera;

namespace MonoTrail.code.rendering.camera;

public abstract class CameraRenderer(SpriteBatch spriteBatch, Effect SpriteBatchEffect = null)
{
  protected readonly SpriteBatch spriteBatch = spriteBatch;
  protected readonly Effect effect = SpriteBatchEffect;

  public void Render(RenderParams renderParams)
  {
    Start(renderParams.RenderingCamera);
    Draw(renderParams);
    End(renderParams.RenderingCamera);
  }

  protected void Start(Camera camera)
  {
    spriteBatch.Begin(
        SpriteSortMode.FrontToBack,
        BlendState.AlphaBlend,
        SamplerState.PointClamp,
        DepthStencilState.None,
        RasterizerState.CullCounterClockwise,
        effect: effect,
        transformMatrix: camera.Transform);
  }

  protected abstract void Draw(RenderParams renderParams);

  protected void End(Camera _) => spriteBatch.End();
}
