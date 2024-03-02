using Microsoft.Xna.Framework;
using MonoTrail.code.drive.content;
using MonoTrail.code.drive.data;
using MonoTrail.code.drive.logic.systems;
using MonoTrail.code.drive.renderers;
using MonoTrail.code.global;
using MonoTrail.code.rendering;
using MonoTrail.code.rendering.camera;
using MonoTrail.code.rendering.data.camera;

namespace MonoTrail.code.drive;

public class DriveScene : IScene
{
  public event RequestedSceneChange RequestedSceneChangeEvent;

  readonly CameraRenderer[] sceneRenderers = [];
  readonly Camera camera;
  readonly ISystem[] systems;

  DriveContent driveContent;

  public DriveScene(DriveData driveData)
  {
    camera = new();

    systems = [
      new DebugChangeSceneSystem(GetEvent),
      new TravelSystem(),
      new VisitLocationSystem(camera),
    ];

    sceneRenderers = [
      new GroundRenderer(),
      new DriveInfoRenderer(),
      new MouseRenderer(),
    ];
  }

  RequestedSceneChange GetEvent() => RequestedSceneChangeEvent;

  public void Init(GameServiceContainer gameServiceContainer)
  {
    driveContent = new(gameServiceContainer, Globals.GlobalContent.RootDirectory);
    driveContent.Load();
  }

  public void Update(GameState gameState, GameTime gameTime)
  {
    camera.CalculateTranslation(gameState.Scene.Right.CameraData);

    if (gameState.View.TryMatch(out var view))
    {
      view.Update(gameState, gameTime);
      return;
    }

    foreach (var system in systems)
      system.Update(gameState, gameTime);
  }

  public void Draw(GameState gameState, GameTime gameTime)
  {
    var renderParams = new RenderParams(
        gameTime,
        camera,
        driveContent.Textures);

    for (int i = 0; i < sceneRenderers.Length; i++)
      sceneRenderers[i].Render(renderParams);

    if (gameState.View.TryMatch(out var view))
      view.Render(renderParams);
  }
}
