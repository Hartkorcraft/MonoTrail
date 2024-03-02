using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTrail.code.data;
using MonoTrail.code.global;
using MonoTrail.code.logic;
using MonoTrail.code.rendering.camera;
using MonoTrail.code.rendering.data.camera;

namespace MonoTrail.code.story;

public class ButtonRender(IEnumerable<ButtonData> buttons) : CameraRenderer(Globals.CreateNewSpriteBatch(), null)
{
  protected override void Draw(RenderParams renderParams)
  {
    foreach (var button in buttons)
    {
      var pos = button.Rect.Location.ToVector2();
      var size = button.Rect.Size.ToVector2();

      var color = button.Rect.ContainsMouse(renderParams.RenderingCamera)
        ? Color.Lime
        : Color.DarkGray;

      spriteBatch.Draw(
        texture: Globals.GlobalContent.PixelRectangle,
        position: pos,
        sourceRectangle: null,
        color: color,
        rotation: 0,
        origin: Vec2.Zero,
        scale: size,
        effects: SpriteEffects.None,
        layerDepth: 0);

      /* spriteBatch.DrawString( */
      /*   spriteFont: Globals.GlobalContent.PixelFont, */
      /*   text: text, */
      /*   position: pos, */
      /*   color: Color.White, */
      /*   rotation: 0, */
      /*   origin: Vec2.Zero, */
      /*   scale: TEXT_SCALE, */
      /*   effects: SpriteEffects.None, */
      /*   layerDepth: 0); */

    }
  }
}
