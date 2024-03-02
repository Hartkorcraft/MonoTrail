using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTrail.code.global;
using MonoTrail.code.rendering.camera;
using MonoTrail.code.rendering.data.camera;
using MonoTrail.code.tactics.data.components;
using TrailCore.tactics.data;
using TrailCore.tactics.data.components;

namespace MonoTrail.code.tactics.renderers;

public record EntityRendererParams(
    int TileSize,
    Dictionary<EntityID, SpriteComponent> SpriteComponents,
    Dictionary<EntityID, OffsetComponent> OffsetComponents,
    Dictionary<EntityID, MapPosComponent> MapPosComponents);

public class EntityRenderer(EntityRendererParams entityRendererParams) : CameraRenderer(Globals.CreateNewSpriteBatch())
{
  protected override void Draw(RenderParams renderParams)
  {
    var (gameTime, camera, textures) = renderParams;
    // var (tacticsData, entityData) = gameState;
    // var componentData = entityData.ComponentData;
    // var fovData = entityData.FovData;

    foreach (var (id, component) in entityRendererParams.SpriteComponents)
      DrawSprite(id, component, renderParams);

    //     if (id == componentData.TileSelectorId) // RENDER IN DIFFERENT SYSTEM
    //         continue;


    //     if (ComponentData.TryGet<MapPosComponent>(id, out var mapPosComponent))
    //         pos += mapPosComponent.MapPos.ToVec2() * MapData.TILE_SIZE;

    //     var hasOffsetComponent = ComponentData.TryGet<OffsetPosComponent>(id, out var offsetPosComponent);

    //     (var offset, var offsetOrigin) = hasOffsetComponent
    //         ? OffsetManager.GetOffsetAdjustment(offsetPosComponent, spriteDimensions)
    //         : (Vector2.Zero, Vector2.Zero);

    //     if (hasOffsetComponent)
    //         TryDrawHitBlackSquare(pos, textures, offsetPosComponent);

    //     pos += offset;
    //     origin += offsetOrigin;

    //     if (ComponentData.TryGet<OffsetColorComponent>(id, out var offsetColorComponent))
    //     {
    //         var (r, g, b, a) = color.ToVector4();
    //         foreach (var c in offsetColorComponent.Offsets)
    //         {
    //             var v = c.Value.ToVector4();
    //             r *= v.X;
    //             g *= v.Y;
    //             b *= v.Z;
    //             a *= v.W;
    //         }
    //         color = new Color(r, g, b, a);
    //     }

    //     if (FovManager.IsSeenByPlayer(id, fovData) is false && ComponentData.Has<MindComponent>(id))
    //         color = Color.Transparent;

    //     DrawItemInHand(renderParams.Textures, componentData, id, pos); //TODO ORDER

    //     var rotation = ComponentData
    //         .GetOption<RotationComponent>(id)
    //         .Match(0, x => x.Dir.ToAngle());

    //     spriteBatch.Draw(
    //         texture: spriteTexture,
    //         position: pos,
    //         sourceRectangle: null,
    //         color: color,
    //         rotation: rotation,
    //         origin: origin,
    //         scale: 1,
    //         effects: SpriteEffects.None,
    //         layerDepth: sprite.Layer);
    // }
  }

  private void DrawSprite(EntityID id, SpriteComponent sprite, RenderParams renderParams)
  {
    if (sprite.Visible == false) return;

    var tileSize = entityRendererParams.TileSize;
    var pos = Vector2.Zero;
    var origin = Vector2.Zero;
    var spriteTexture = renderParams.Textures[sprite.SpriteName];
    var spriteDimensions = new Vector2(spriteTexture.Width, spriteTexture.Height);
    var color = sprite.Color;
    var rotation = 0;

    entityRendererParams.MapPosComponents
      .TryGetOpt(id)
      .MatchEffect(x => pos += x.MapPos.ToVec2() * entityRendererParams.TileSize);

    entityRendererParams.OffsetComponents
      .TryGetOpt(id)
      .Bind(x => x.Offsets)
      .MatchEffect(x => x.Foreach(offset => pos += offset.Value));

    // (var offset, var offsetOrigin) = hasOffsetComponent
    //     ? OffsetManager.GetOffsetAdjustment(offsetPosComponent, spriteDimensions)
    //     : (Vector2.Zero, Vector2.Zero);

    // if (hasOffsetComponent)
    //     TryDrawHitBlackSquare(pos, textures, offsetPosComponent);

    // pos += offset;
    // origin += offsetOrigin;

    spriteBatch.Draw(
        texture: spriteTexture,
        position: pos,
        sourceRectangle: null,
        color: color,
        rotation: rotation,
        origin: origin,
        scale: 1,
        effects: SpriteEffects.None,
        layerDepth: sprite.Layer);
  }

  // private void TryDrawHitBlackSquare(Vector2 pos, SpritesTextures textures, OffsetPosComponent offsetPosComponent)
  // {
  //     const float LIMIT = 0.01f;
  //     var squarePos = pos;
  //     var draw = offsetPosComponent.Offsets
  //         .TryGet([OffsetPos.Hit, OffsetPos.Bump, OffsetPos.Recoil])
  //     .Any(x => Vector2.Distance(x.Value, Vector2.Zero) > LIMIT);
  //     if (draw is false) return;

  //     spriteBatch.Draw(texture: textures[TextureNames.DziadersTex], position: pos, Color.Black);
  // }

  // private void DrawItemInHand(SpritesTextures spriteTextures, ComponentData componentData, EntityID id, Vector2 pos)
  // {
  //     if (componentData.CurrentSelection == id && ComponentData.Get<SpriteComponent>(componentData.TileSelectorId).Visible && ComponentData.TryGet<InventoryComponent>(id, out var inventoryComponent))
  //     {
  //         var selectorPos = ComponentData.Get<MapPosComponent>(componentData.TileSelectorId).MapPos;

  //         inventoryComponent.ItemInHand.Bind(x => x.TextureName).MapEffect(textureName =>
  //         {
  //             var texture = spriteTextures[textureName];
  //             var itemPos = pos + new Vector2(MapData.TILE_SIZE / 2, MapData.TILE_SIZE / 2);
  //             var rotation = (selectorPos.ToTileCenter(MapData.TILE_SIZE) - itemPos).ToAngle();
  //             var flip = rotation.ConvertToDegree() > 90 || rotation.ConvertToDegree() < -90;
  //             spriteBatch.Draw(
  //                 texture: texture,
  //                 position: itemPos,
  //                 sourceRectangle: null,
  //                 color: Color.White,
  //                 rotation: rotation,
  //                 origin: new(-texture.Width / 4, texture.Height / 2),
  //                 scale: 1,
  //                 effects: flip ? SpriteEffects.FlipVertically : SpriteEffects.None,
  //                 layerDepth: SpriteComponent.TILE_SELECTOR_LAYER);
  //         });
  //     }
  // }
}
