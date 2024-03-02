using Microsoft.Xna.Framework;
using MonoTrail.code.data;
using MonoTrail.code.global;
using MonoTrail.code.rendering;
using MonoTrail.code.rendering.camera;
using MonoTrail.code.rendering.data.camera;

namespace MonoTrail.code.story;

public class StoryMachine(Camera camera)
{
  readonly CameraRenderer[] sceneRenderers = [
    new StoryBoxRenderer(),
    new MouseRenderer()];


  public void Init(GameState gameState) { }

  public void Update(UpdateParams updateParams)
  {
  }

  public void Draw(GameState gameState, GameTime gameTime)
  {
    var renderParams = new RenderParams(
        gameTime,
        camera,
        Globals.GlobalContent.Textures);

    for (int i = 0; i < sceneRenderers.Length; i++)
      sceneRenderers[i].Render(renderParams);
  }
}

