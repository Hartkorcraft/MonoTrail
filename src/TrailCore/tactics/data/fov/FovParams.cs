namespace TrailCore.tactics.data.fov;

public record FovParams(
GridPos Origin,
Func<GridPos, bool> CheckBlocking,  // TODO MAP DIMENSIONS
int RangeLimit,
int UncoverArround = 0);
