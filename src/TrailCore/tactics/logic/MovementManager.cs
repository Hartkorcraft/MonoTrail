using FuNK;
using TrailCore.tactics.data;
using TrailCore.tactics.data.components;
using TrailCore.tactics.data.map;

namespace TrailCore.tactics.logic;

public delegate bool IsBlockingGridPos(GridPos gridPos);

public static class MovementManager
{
  public static void MoveTeleport(this EntityID entity, GridPos moveTo, TacticsSceneData tacticsData)
  {
    var mapPosComponent = tacticsData.EntityData.ComponentStorage.Get<MapPosComponent>(entity);
    PlaceMapObject(entity, mapPosComponent, moveTo, tacticsData.MapData);
  }

  public static bool MoveStep(this EntityID entity, GridPos moveBy, TacticsSceneData tacticsData)
  {
    var mapPosComponent = tacticsData.EntityData.ComponentStorage.Get<MapPosComponent>(entity);
    var mapData = tacticsData.MapData;
    var newPos = mapPosComponent.MapPos.Add(moveBy);

    if (IsPosUnavailable(newPos, mapData))
      return false;

    PlaceMapObject(entity, mapPosComponent, newPos, mapData);

    return true;
  }

  public static bool IsPosOnMapOccupied(this GridPos pos, MapData mapData)
    => IsOutsideMap(pos, mapData) is false && IsBlocked(pos, mapData);

  public static bool IsBlocked(this GridPos pos, MapData mapData)
      => mapData.Tiles[PosToTileIndex(pos, mapData)].Occupying != None;

  public static bool TryGetBlocked(this MapData mapData, (int x, int y) pos, out EntityID entityID)
  {
    var opt = mapData.Tiles[PosToTileIndex(pos, mapData)].Occupying;
    entityID = opt.Or(default!);
    return opt.IsSome;
  }

  public static bool IsOutsideMap(this GridPos gridPos, MapData mapData)
      => gridPos.x < 0 || gridPos.y < 0 || gridPos.x >= mapData.MapDimensions.x || gridPos.y >= mapData.MapDimensions.y;

  public static bool IsPosUnavailable(this GridPos gridPos, MapData mapData)
      => IsOutsideMap(gridPos, mapData) || IsBlocked(gridPos, mapData);

  public static int PosToTileIndex(this GridPos pos, MapData mapData)
      => pos.x + (pos.y * mapData.MapDimensions.x);

  private static void PlaceMapObject(EntityID entity, MapPosComponent mapPosComponent, GridPos newPos, MapData mapData)
  {
    mapData.Tiles[PosToTileIndex(mapPosComponent.MapPos, mapData)].Occupying = None;
    mapData.Tiles[PosToTileIndex(newPos, mapData)].Occupying = entity;
    mapPosComponent.MapPos = newPos;
  }

  public static GridPos[] Around(this GridPos vec)
    => [(vec.x, vec.y - 1), (vec.x, vec.y + 1), (vec.x - 1, vec.y), (vec.x + 1, vec.y),];
}
