using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoTrail.code.data;
using MonoTrail.code.rendering;
using MonoTrail.code.rendering.camera;
using MonoTrail.code.rendering.data.camera;

namespace MonoTrail.code.story;

public class StoryBoxView(IEnumerable<ButtonData> buttons, Camera camera) : IViewSystem
{
  readonly CameraRenderer[] renderers = [
    new StoryBoxRenderer(),
    new ButtonRender(buttons),
    new MouseRenderer()];

  readonly ISystem[] systems = [
    new ButtonPressedSystem(buttons,camera)
  ];

  public void Render(RenderParams renderParams)
  {
    for (int i = 0; i < renderers.Length; i++)
      renderers[i].Render(renderParams);
  }

  public void Update(GameState gameState, GameTime gameTime)
  {
    foreach (var s in systems)
      s.Update(gameState, gameTime);
  }
}
