namespace MonoTrail.code.tactics.logic;

public static class GridPosExtensions
{
  public static (int x, int y) WorldToMap(this (float x, float y) pos, int tileSize)
    => ((int)pos.x / tileSize, (int)pos.y / tileSize);
}
