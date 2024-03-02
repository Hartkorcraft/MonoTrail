using TrailCore.tactics.data.fov;

namespace TrailCore.tactics.logic.fov;

public static class MapScanning
{
  public static Dictionary<GridPos, ScanCell> ScanMap(GridPos startPos, int maxDistance, IsBlockingGridPos isBlockingGridPos)
  {
    var openSet = new List<ScanCell>();
    var closedSet = new Dictionary<GridPos, ScanCell>();

    var ignore = new HashSet<object>();

    var start = new ScanCell(startPos, 0);
    openSet.Add(start);

    var index = 0;
    while (openSet.Count != 0)
    {
      index++;
      var cell = openSet[0];
      for (int i = 1; i < openSet.Count; i++)
      {
        if (openSet[i].DistanceFromStart < cell.DistanceFromStart) cell = openSet[i];
      }

      openSet.Remove(cell);
      closedSet[cell.Pos] = cell;

      if (isBlockingGridPos(cell.Pos) && cell != start)
      {
        continue;
      }

      var neighbours = cell
          .Pos
          .Around()
          .Select(pos => closedSet.TryGetValue(pos, out var n) ? n : new ScanCell(pos, cell.DistanceFromStart + 1))
          .ToArray();

      foreach (var neighbour in neighbours)
      {
        var newCostToNeighbour = cell.DistanceFromStart + 1;

        if (closedSet.ContainsKey(neighbour.Pos))
          continue;

        if (newCostToNeighbour > maxDistance)
          continue;

        if (isBlockingGridPos(neighbour.Pos))
          continue;

        if (newCostToNeighbour < neighbour.DistanceFromStart)
        {
          neighbour.Parent = cell;
          neighbour.DistanceFromStart = newCostToNeighbour;
        }
        else if (openSet.Any(x => x.Pos == neighbour.Pos) is false)
        {
          if (closedSet.ContainsKey(neighbour.Pos)) { throw new Exception("lol"); }

          neighbour.Parent = cell;
          openSet.Add(neighbour);
        }
      }
    }
    // LogInfoAI($"Map scanned: {index}");
    return closedSet;
  }
}
