using System;
using System.Linq;
using MonoTrail.code.tactics.data;
using MonoTrail.code.tactics.logic;
using TiledCS;
using TrailCore.data.tactics.map;
using TrailCore.tactics.data.map;
using TrailCore.tactics.logic;

namespace MonoTrail.code.tactics.initialization;

public static class TacticsInitialization
{
  public static TacticsSceneData TacticsSceneDataFromMap(TiledMap map, TileSetData tileSetData)
  {
    //TODO TRAILCORE INIT
    var groundLayer = map.Layers
      .First(x => x.name == "Ground");

    var mapData = new MapData(
      MapDimensions: (groundLayer.width, groundLayer.height),
      Tiles: groundLayer.data
      .Select(x => new Tile() { GroundType = (GroundType)x })
      .ToArray());

    var tacticsData = new TacticsSceneData(
        MapData: mapData,
        EntityData: new(
          Entities: [],
          ComponentStorage: new(),
          FovData: new()),
        TurnData: new() { CurrentTurnType = TurnType.Player },
        TileSetData: tileSetData,
        SelectorData: new()
        {
          Show = false,
          GridPos = (0, 0),
          Offset = Vec2.Zero
        },
        CameraData: new(
          CameraPosInfo: new(
            Pos: Vec2.One * 50,
            Target: Vec2.One * 50,
            Offset: Vec2.Zero),
          CameraZoomInfo: new(
            Zoom: 4)));

    var spawnsLayer = map.Layers
      .First(x => x.name == "Spawns");

    var players = spawnsLayer.objects
      .Where(x => x.name == "player")
      .ToArray();

    var enemies = spawnsLayer.objects
      .Where(x => x.name == "enemy")
      .ToArray();

    foreach (var entity in spawnsLayer.objects)
      SpawnEntity(entity, tacticsData);

    var sceneryLayer = map.Layers
      .First(x => x.name == "Scenery");

    SpawnScenery(sceneryLayer, tacticsData);

    tacticsData.ResetScene();

    return tacticsData;
  }

  static void SpawnEntity(TiledObject entity, data.TacticsSceneData tacticsData)
  {
    var properties = entity.properties
      .ToDictionary(x => x.name, x => (x.type, x.value));

    var enabled = properties
      .TryGetOpt("enabled")
      .Bind(x => bool.Parse(x.value))
      .MatchOr(() => throw new Exception("bruh"));

    if (enabled is false) return;

    _ = properties
      .TryGetOpt("entity_type")
      .Bind(x => x.value)
      .MatchOr(() => throw new Exception("bruh")) switch
    {
      "player_default" => tacticsData.CreateDefaultPlayerObject(entity),
      // "dziaders" => tacticsData.CreateDefaultDziaders(entity),
      // "zombie" => tacticsData.CreateDefaultZombie(entity),
      // { } x => tacticsData.CreateDefaultDziaders(entity).Log($"init entity_type fail {x}", Log.LogError),
      _ => null //throw new Exception("fail")
    };
  }

  static void SpawnScenery(TiledLayer sceneryLayer, data.TacticsSceneData tacticsData)
  {
    var mapData = tacticsData.MapData;
    for (int y = 0; y < mapData.MapDimensions.y; y++)
      for (int x = 0; x < mapData.MapDimensions.x; x++)
      {
        var sceneryType = (SceneryType)sceneryLayer.data[MovementManager.PosToTileIndex((x, y), mapData)] - 100;
        Action initScenery = sceneryType switch
        {
          SceneryType.Wall => () => tacticsData.AddWall(new WallInfo((x, y))),
          _ => EmptyAction,
        };
        initScenery();
      }
  }
}
