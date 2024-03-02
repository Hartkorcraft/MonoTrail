using System.Linq;
using Microsoft.Xna.Framework;
using MonoTrail.code.drive;
using MonoTrail.code.global;
using MonoTrail.code.input;
using MonoTrail.code.tactics;
using MonoTrail.code.tactics.data;
using MonoTrail.code.tactics.initialization;
using MonoTrail.code.tactics.logic;
using TiledCS;
using TrailCore.data.tactics.map;
using TrailCore.logic.tactics.map.utils;
using TrailCore.tactics.data;
using TrailCore.tactics.data.components;
using MonoTrail.code.rendering.data.camera;
using MonoTrail.code.drive.data;
using TrailCore.data.story_data;

namespace MonoTrail.code;

public class Main
{
  IScene currentScene;

  readonly GameState gameState;
  readonly GameServiceContainer gameServiceContainer;

  public Main(GameServiceContainer gameServiceContainer)
  {
    this.gameServiceContainer = gameServiceContainer;
    gameState = new()
    {
      StoryData = new StoryData(TravelData: new()
      {
        DriveTime = 0,
        NonDriveTime = 0,
        VehicleSpeedKmPerH = 50,
        TraveledDistanceInMeters = 0,
        EventsOnRoad = [new DriveEventData(
            EventID: "TODO",
            Distance: 5_000)],
      }),
      Scene = InitializeDrive(gameServiceContainer)
    };
  }

  public void ChangeScene(IScene newScene)
  {
    var oldScene = currentScene;
    if (oldScene is not null) oldScene.RequestedSceneChangeEvent -= ChangeScene;
    currentScene = newScene;
    currentScene.Init(gameServiceContainer);
    currentScene.RequestedSceneChangeEvent += ChangeScene;
    LogInfo($"Changed scene from [{oldScene}] to [{newScene}]");
  }

  DriveData InitializeDrive(GameServiceContainer gameServiceContainer)
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
    ChangeScene(driveScene);

    return driveData;
  }

  tactics.data.TacticsSceneData InitializeTactics(GameServiceContainer gameServiceContainer)
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
    ChangeScene(tacticsScene);
    _ = tacticsSceneData.ChangeSelection(selection.Bind(x => x.Key));

    return tacticsSceneData;
  }

  public void Update(GameTime gameTime)
  {
    _ = MouseManager.UpdateState();
    _ = KeyboardManager.UpdateState();
    currentScene.Update(gameState, gameTime);
  }

  public void Draw(GameTime gameTime)
  {
    currentScene.Draw(gameState, gameTime);
  }
}
