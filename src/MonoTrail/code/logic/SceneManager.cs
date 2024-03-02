using System.Linq;
using Microsoft.Xna.Framework;
using MonoTrail.code.drive;
using MonoTrail.code.drive.data;
using MonoTrail.code.global;
using MonoTrail.code.rendering.data.camera;
using MonoTrail.code.tactics;
using MonoTrail.code.tactics.data;
using MonoTrail.code.tactics.initialization;
using MonoTrail.code.tactics.logic;
using TiledCS;
using TrailCore.data.tactics.map;
using TrailCore.logic.tactics.map.utils;
using TrailCore.tactics.data;
using TrailCore.tactics.data.components;

namespace MonoTrail.code.logic;

public static class SceneManager
{
  public static DriveScene InitDriveScene(this GameState gameState)
  {
    var cameraData = new CameraData(
        CameraPosInfo: new(
          Pos: Vec2.Zero,
          Target: Vec2.Zero,
          Offset: Vec2.Zero),
        CameraZoomInfo: new(
          Zoom: 5));

    var driveData = new DriveData(
        CameraData: cameraData);

    var driveScene = new DriveScene(driveData);
    gameState.Scene = driveData;

    return driveScene;
  }

  public static TacticsScene InitTacticsScene(this GameState gameState)
  {
    static Rectangle FromTuple((int x, int y, int z, int v) t) => new(t.x, t.y, t.z, t.v);

    const int tileSize = 8;
    var map = new TiledMap(Globals.GlobalContent.RootDirectory + "\\tiled\\testMap.tmx");
    var tileSetData = new TileSetData(
      Name: "ground",
      TileSize: tileSize,
      new()
      {
        [GroundType.Dirt] = FromTuple(MapUtils.TileSetRectangle(0, 0, tileSize)),
        [GroundType.Grass] = FromTuple(MapUtils.TileSetRectangle(1, 0, tileSize)),
        [GroundType.Gravel] = FromTuple(MapUtils.TileSetRectangle(2, 0, tileSize)),
      });

    var tacticsSceneData = TacticsInitialization.TacticsSceneDataFromMap(map, tileSetData);

    var minds = tacticsSceneData.EntityData.ComponentStorage.GetComponents<MindComponent>();

    var selection = minds
      .Select(t => (t.Key, m: t.Value))
      .FirstOrNone(t => t.m.FactionAligment == FactionAligment.Player);

    var tacticsScene = new TacticsScene(tacticsSceneData, tileSetData);
    _ = tacticsSceneData.ChangeSelection(selection.Bind(x => x.Key));
    gameState.Scene = tacticsSceneData;

    return tacticsScene;
  }
}
