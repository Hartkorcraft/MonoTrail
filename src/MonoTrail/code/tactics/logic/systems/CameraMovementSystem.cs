using Microsoft.Xna.Framework;
using MonoTrail.code.ext;
using MonoTrail.code.tactics.data.components;
using TrailCore.tactics.data.components;

namespace MonoTrail.code.tactics.logic.systems;

public class CameraMovementSystem : ISystem
{
  public void Update(GameState gameState, GameTime gameTime)
  {
    var tactics = gameState.Scene.Left;
    var componentStorage = tactics.EntityData.ComponentStorage;

    var (posInfo, zoomInfo) = tactics.CameraData;

    var targetPos = posInfo.Target.Match(
        id => componentStorage
          .Get<MapPosComponent>(id).MapPos
          .ToTileCenter(tactics.TileSetData.TileSize) + componentStorage
          .GetMaybe<OffsetComponent>(id)
          .Bind(x => x.Offsets)
          .Match(
            () => Vector2.Zero, OffsetExtensions.AggregateOffsets),
            pos => posInfo.Pos);

    posInfo.Pos = posInfo.Pos.Lerp(targetPos + posInfo.Offset, posInfo.SpeedWeight);

    zoomInfo.Zoom = MathHelper.Clamp(
        value: zoomInfo.Zoom.Lerp(zoomInfo.Target, zoomInfo.ZoomWeight),
        min: zoomInfo.Min,
        max: zoomInfo.Max);
  }
}
