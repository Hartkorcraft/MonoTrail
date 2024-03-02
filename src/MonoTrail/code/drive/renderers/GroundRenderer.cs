using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTrail.code.global;
using MonoTrail.code.rendering.camera;
using MonoTrail.code.rendering.data.camera;

namespace MonoTrail.code.drive.renderers;

public class GroundRenderer() : CameraRenderer(Globals.CreateNewSpriteBatch())
{
  protected override void Draw(RenderParams renderParams)
  {
    var groundSize = 1000;

    var color = Color.Brown;
    var pos = new Vec2(-groundSize / 2, 20);
    var scale = new Vec2(groundSize, 500);

    spriteBatch.Draw(
        texture: Globals.GlobalContent.PixelRectangle,
        position: pos,
        sourceRectangle: null,
        color: color,
        rotation: 0,
        origin: Vec2.Zero,
        scale: scale,
        effects: SpriteEffects.None,
        layerDepth: 0);
  }
}
