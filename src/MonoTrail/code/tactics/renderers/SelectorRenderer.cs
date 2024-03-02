using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTrail.code.global;
using MonoTrail.code.rendering.camera;
using MonoTrail.code.rendering.data.camera;
using MonoTrail.code.tactics.content;
using MonoTrail.code.tactics.data;

namespace MonoTrail.code.tactics.renderers;

public class SelectorRenderer(
    SelectorData selectorData,
    int tileSize) : CameraRenderer(Globals.CreateNewSpriteBatch(), SpriteBatchEffect: null)
{
  protected override void Draw(RenderParams renderParams)
  {
    if (selectorData.Show is false) return;

    var tex = renderParams.Textures[TextureNames.TileSelectorTex];
    var pos = selectorData.GridPos.ToVec2() * tileSize;
    var offset = selectorData.Offset;
    var color = Color.White;

    pos += offset;

    spriteBatch.Draw(
      texture: tex,
      position: pos,
      sourceRectangle: null,
      color: color,
      rotation: 0,
      origin: Vec2.Zero,
      scale: 1,
      effects: SpriteEffects.None,
      layerDepth: 0);
  }
}
