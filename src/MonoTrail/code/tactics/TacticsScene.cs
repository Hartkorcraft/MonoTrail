using Microsoft.Xna.Framework;
using MonoTrail.code.global;
using MonoTrail.code.rendering;
using MonoTrail.code.rendering.camera;
using MonoTrail.code.rendering.data.camera;
using MonoTrail.code.tactics.content;
using MonoTrail.code.tactics.data;
using MonoTrail.code.tactics.data.components;
using MonoTrail.code.tactics.logic.systems;
using MonoTrail.code.tactics.logic.turn;
using MonoTrail.code.tactics.renderers;
using TrailCore.tactics.data.components;

namespace MonoTrail.code.tactics;

public class TacticsScene : IScene
{
  public event RequestedSceneChange RequestedSceneChangeEvent;

  readonly CameraRenderer[] sceneRenderers = [];
  readonly Camera camera;
  readonly TurnMachine turnMachine;

  readonly ISystem[] systems = [
    new AdjustOffsetsSystem(),
    new CameraTargetSelectionSystem(),
    new CameraInputSystem(),
    new CameraMovementSystem()];

  TacticsContent tacticsContent;

  public TacticsScene(TacticsSceneData tacticsSceneData, TileSetData tileSetData)
  {
    camera = new();
    turnMachine = new TurnMachine(camera, tacticsContent, tileSetData.TileSize);

    var components = tacticsSceneData.EntityData.ComponentStorage;

    sceneRenderers = [
        new MapRenderer(tacticsSceneData.MapData, tileSetData),
            new HighlightRenderer(
              spriteComponents: components.GetComponents<SpriteComponent>(),
              mapPosComponents:components.GetComponents<MapPosComponent>(),
              highlightComponents:components.GetComponents<HighlightComponent>(),
              offsetComponents:components.GetComponents<OffsetComponent>(),
              camera,
              tileSetData.TileSize),
            new EntityRenderer(new(
              TileSize: tileSetData.TileSize,
              SpriteComponents:components.GetComponents<SpriteComponent>(),
              OffsetComponents:components.GetComponents<OffsetComponent>(),
              MapPosComponents:components.GetComponents<MapPosComponent>())),
            new FovRenderer(
              fovData: tacticsSceneData.EntityData.FovData,
              curTurnType: tacticsSceneData.TurnData.CurrentTurnType,
              mapDimensions: tacticsSceneData.MapData.MapDimensions,
              tileSize: tacticsSceneData.TileSetData.TileSize),
            new SelectorRenderer(
                selectorData: tacticsSceneData.SelectorData,
                tileSize:tileSetData.TileSize),
            new MouseRenderer(),
    ];
  }

  public void Init(GameServiceContainer gameServiceContainer)
  {
    tacticsContent = new(gameServiceContainer, Globals.GlobalContent.RootDirectory);
    tacticsContent.Load();
  }

  public void Update(GameState gameState, GameTime gameTime)
  {
    camera.CalculateTranslation(gameState.Scene.Left.CameraData);
    turnMachine.Update(gameState, gameTime);
    foreach (var s in systems)
      s.Update(gameState, gameTime);
  }

  public void Draw(GameState gameState, GameTime gameTime)
  {
    var renderParams = new RenderParams(
        gameTime,
        camera,
        tacticsContent.Textures);

    for (int i = 0; i < sceneRenderers.Length; i++)
      sceneRenderers[i].Render(renderParams);
  }
}

