using TrailCore.tactics.data.fov;
using TrailCore.tactics.ext;

namespace TrailCore.tactics.logic.fov;

public static class SymetricShadowCastingFov
{
  static readonly (int xx, int xy, int yx, int yy)[] transforms = [
      (1, 0, 0, 1), (-1, 0, 0, 1), (0, -1, 1, 0), (0, -1, -1, 0),
      (-1, 0, 0, -1), (1, 0, 0, -1), (0, 1, -1, 0), (0, 1, 1, 0),];

  // public HartFov(Func<Vector2i, bool> _checkBlocking)
  //     => checkBlocking = _checkBlocking;

  public static HashSet<GridPos> ComputeAllDirections(FovParams fovParams) => [
      fovParams.Origin,
      ..ComputeFov(fovParams.Origin, fovParams.CheckBlocking, 0, fovParams.RangeLimit),
      ..ComputeFov(fovParams.Origin, fovParams.CheckBlocking, 1, fovParams.RangeLimit),
      ..ComputeFov(fovParams.Origin, fovParams.CheckBlocking, 2, fovParams.RangeLimit),
      ..ComputeFov(fovParams.Origin, fovParams.CheckBlocking, 3, fovParams.RangeLimit),
      ..ComputeFov(fovParams.Origin, fovParams.CheckBlocking, 4, fovParams.RangeLimit),
      ..ComputeFov(fovParams.Origin, fovParams.CheckBlocking, 5, fovParams.RangeLimit),
      ..ComputeFov(fovParams.Origin, fovParams.CheckBlocking, 6, fovParams.RangeLimit),
      ..ComputeFov(fovParams.Origin, fovParams.CheckBlocking, 7, fovParams.RangeLimit),
      ];

  public static HashSet<GridPos> ComputeForDirection(FovParams fovParams, int dir) => [
      fovParams.Origin,
      .. ComputeFov(fovParams.Origin,fovParams.CheckBlocking,  dir % 8,fovParams.RangeLimit),
      .. ComputeFov(fovParams.Origin,fovParams.CheckBlocking, (dir + 1) % 8,fovParams.RangeLimit)];

  static HashSet<GridPos> ComputeFov(GridPos origin, Func<GridPos, bool> checkBlocking, int dir, int rangeLimit)
  {
    var uncovered = new HashSet<GridPos>();
    Scan(1, 0, 1);
    return uncovered;

    void Scan(int depth, double startSlope, double endSlope)
    {
      if (startSlope >= endSlope || depth > rangeLimit) return;
      var xMin = (int)Math.Round((depth - 0.5) * startSlope);
      var xMax = (int)Math.Ceiling(((depth + 0.5) * endSlope) - 0.5);
      for (var col = xMin; col <= xMax; col++)
      {
        var nx = origin.x + (transforms[dir].xx * col) + (transforms[dir].xy * depth);
        var ny = origin.y + (transforms[dir].yx * col) + (transforms[dir].yy * depth);
        var newPos = (nx, ny);
        if (checkBlocking(newPos) is false && col >= depth * startSlope && col <= depth * endSlope && origin.GetDistance(newPos) <= rangeLimit)
        {
          uncovered.Add(newPos);
        }
        else
        {
          if (col >= (depth - 0.5) * startSlope && col - 0.5 <= depth * endSlope)
            uncovered.Add(newPos);
          Scan(depth + 1, startSlope, (col - 0.5) / depth);
          startSlope = (col + 0.5) / depth;
          if (startSlope >= endSlope) return;
        }
      }
      Scan(depth + 1, startSlope, endSlope);
    }
  }
}
