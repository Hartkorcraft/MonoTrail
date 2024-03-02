using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTrail.code.global;
using MonoTrail.code.rendering.camera;
using MonoTrail.code.rendering.data.camera;
using MonoTrail.code.tactics.data;
using TrailCore.tactics.data.fov;

namespace MonoTrail.code.tactics.renderers;

public class FovRenderer(
    FovData fovData,
    TurnType curTurnType,
    GridPos mapDimensions,
    int tileSize) : CameraRenderer(Globals.CreateNewSpriteBatch())
{
  private const float FOG_TRANSPARENCY = 0.5f;
  private static readonly Color SeenFogColor = new(0, 0, 0.01f, FOG_TRANSPARENCY);
  private static readonly Color FogColor = new(16, 22, 28);

  protected override void Draw(RenderParams renderParams)
  {
    var (gameTime, camera, textures) = renderParams;
    // var (tacticsData, entityData, _, mapData, turnData) = gameState;
    var (playerView, enemyView, seenByPlayer) = fovData;
    var currentView = curTurnType switch
    {
      TurnType.Player => playerView,
      TurnType.Enemy => enemyView,
      _ => []
    };

    for (int y = 0; y < mapDimensions.y; y++) // TODO MULTITHEADING?
      for (int x = 0; x < mapDimensions.x; x++)
      {
        var mapPos = (x, y);
        if (playerView.Contains(mapPos)) continue;

        var color = seenByPlayer.Contains(mapPos) ? SeenFogColor : FogColor;
        DrawFog(mapPos, color);
      }
  }

  private void DrawFog((int x, int y) mapPos, Color color)
  {
    spriteBatch.Draw(
        texture: Globals.GlobalContent.PixelRectangle,
        position: mapPos.ToTileWorldPos(tileSize),
        sourceRectangle: null,
        color: color,
        rotation: 0,
        origin: Vector2.Zero,
        scale: new Vector2(tileSize, tileSize),
        effects: SpriteEffects.None,
        layerDepth: 0);
  }
}
