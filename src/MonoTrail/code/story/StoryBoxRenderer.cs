using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTrail.code.global;
using MonoTrail.code.rendering.camera;
using MonoTrail.code.rendering.data.camera;

namespace MonoTrail.code.story;

public class StoryBoxRenderer() : CameraRenderer(Globals.CreateNewSpriteBatch(), null)
{
  const int MAX_LINE_LENGTH = 120;
  const float TEXT_SCALE = 0.64f;

  protected override void Draw(RenderParams renderParams)
  {
    var text = "Lorem ipsum XD";

    var color = Color.Black;
    var pos = Vec2.Zero;
    var scale = new Vec2((text.Length * 3) + 100, 100);

    pos.Y -= 70;
    pos.X -= scale.X / 2;

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

    spriteBatch.DrawString(
        spriteFont: Globals.GlobalContent.PixelFont,
        text: text,
        position: pos,
        color: Color.White,
        rotation: 0,
        origin: Vec2.Zero,
        scale: TEXT_SCALE,
        effects: SpriteEffects.None,
        layerDepth: 0);
  }
}
