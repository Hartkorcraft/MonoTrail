using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoTrail.code.rendering.camera;
using MonoTrail.code.rendering.data.camera;
using MonoTrail.code.tactics.data.components;
using TrailCore.tactics.data.components;
using System.Collections.Generic;
using MonoTrail.code.tactics.logic;
using TrailCore.tactics.data;
using System;
using MonoTrail.code.global;

namespace MonoTrail.code.tactics.renderers;

public class HighlightRenderer : CameraRenderer, IDisposable
{
  private readonly Dictionary<EntityID, SpriteComponent> spriteComponents;
  private readonly Dictionary<EntityID, MapPosComponent> mapPosComponents;
  private readonly Dictionary<EntityID, HighlightComponent> highlightComponents;
  private readonly Dictionary<EntityID, OffsetComponent> offsetComponents;
  private readonly Camera camera;
  private readonly int tileSize;
  private Option<EntityID> curSelection = None;

  public HighlightRenderer(
    Dictionary<EntityID, SpriteComponent> spriteComponents,
    Dictionary<EntityID, MapPosComponent> mapPosComponents,
    Dictionary<EntityID, HighlightComponent> highlightComponents,
    Dictionary<EntityID, OffsetComponent> offsetComponents,
    Camera camera,
    int tileSize) : base(Globals.CreateNewSpriteBatch(), SpriteBatchEffect: null)
  {
    this.spriteComponents = spriteComponents;
    this.mapPosComponents = mapPosComponents;
    this.highlightComponents = highlightComponents;
    this.offsetComponents = offsetComponents;
    this.camera = camera;
    this.tileSize = tileSize;
    SelectionManagerService.ChangedSelection += RegisterSelectionChange;
  }

  public void Dispose()
  {
    SelectionManagerService.ChangedSelection -= RegisterSelectionChange;
    GC.SuppressFinalize(this);
  }

  protected override void Draw(RenderParams renderParams)
  {
    foreach (var (id, component) in highlightComponents)
    {
      if (component is not HighlightComponent highlightComponent || highlightComponent.DisplayInfo is false)
        continue;

      if (spriteComponents.TryGetValue(id, out var sp) is false || sp is not SpriteComponent spriteComponent)
        continue;

      if (mapPosComponents.TryGetValue(id, out var mp) is false || mp is not MapPosComponent mapPosComponent)
        continue;

      var mousePos = TilemapService.GetMouseMapPos(camera, tileSize);

      var isSelection = curSelection.Match(
              () => false,
              selection => selection == id);

      if (mapPosComponent.MapPos != mousePos && isSelection is false)
        continue;

      var color = isSelection ? Color.Yellow : Color.WhiteSmoke;
      var offset = Vector2.One;
      var pos = mapPosComponent.MapPos.ToVec2() * tileSize;
      var scale = tileSize + 2;

      offsetComponents
        .TryGetOpt(id)
        .Bind(x => x.Offsets)
        .MatchEffect(x => x.Foreach(offset => pos += offset.Value));

      spriteBatch.Draw(
       texture: Globals.GlobalContent.PixelRectangle,
       position: pos - offset,
       sourceRectangle: null,
       color: color,
       rotation: 0,
       origin: Vector2.Zero,
       scale: scale,
       effects: SpriteEffects.None,
       layerDepth: spriteComponent.Layer);
    }
  }

  void RegisterSelectionChange(object sender, ChangedSelectionArgs args)
    => curSelection = args.NewSelection;
}
