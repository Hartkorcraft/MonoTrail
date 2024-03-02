namespace TrailCore.tactics.data.map;

public record MapData(
    (int x, int y) MapDimensions,
    Tile[] Tiles
);
