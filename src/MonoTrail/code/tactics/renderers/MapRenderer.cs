using Microsoft.Xna.Framework;
using MonoTrail.code.global;
using MonoTrail.code.rendering.camera;
using MonoTrail.code.rendering.data.camera;
using MonoTrail.code.tactics.data;
using TrailCore.logic.tactics.map.utils;
using TrailCore.tactics.data.map;

namespace MonoTrail.code.tactics.renderers;

public class MapRenderer(
    MapData mapData,
    TileSetData tileSetData) : CameraRenderer(Globals.CreateNewSpriteBatch(), SpriteBatchEffect: null)
{
  protected override void Draw(RenderParams renderParams)
  {
    var (_, _, textures) = renderParams;
    for (int x = 0; x < mapData.MapDimensions.x; x++)
    {
      for (int y = 0; y < mapData.MapDimensions.y; y++)
      {
        var tile = mapData.Tiles[x + (y * mapData.MapDimensions.x)].GroundType;
        spriteBatch.Draw(
          texture: textures[tileSetData.Name],
          destinationRectangle: FromTuple(MapUtils.TileSetRectangle(x, y, tileSetData.TileSize)),
          sourceRectangle: tileSetData.TileSet[tile],
          color: Color.White);
      }
    }
  }

  private static Rectangle FromTuple((int x, int y, int z, int v) t) => new(t.x, t.y, t.z, t.v);
}
