using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTrail.code.global;
using MonoTrail.code.rendering.camera;
using MonoTrail.code.rendering.data.camera;
using MonoTrail.code.tactics.content;

namespace MonoTrail.code.rendering;

public class MouseRenderer() : CameraRenderer(Globals.CreateNewSpriteBatch(), SpriteBatchEffect: null)
{
  protected override void Draw(RenderParams renderParams)
  {
    var camera = renderParams.RenderingCamera;
    spriteBatch.Draw(
        texture: renderParams.Textures[TextureNames.DefaultMouseCursor],
        position: camera.ScreenToWorld(Mouse.GetState().Position.ToVector2()),
        sourceRectangle: null,
        color: Color.White,
        rotation: 0,
        origin: Vector2.Zero,
        scale: 1,
        effects: SpriteEffects.None,
        layerDepth: 0
        );
  }
}
