using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrailCore.logic.tactics.map.utils;

public static class MapUtils
{
    public static (int x, int y, int sizeX, int sizeY) TileSetRectangle(int x, int y, int tileSize)
        => (x * tileSize, y * tileSize, tileSize, tileSize);
}
