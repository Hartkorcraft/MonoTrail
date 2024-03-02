namespace TrailCore.tactics.data.fov;

public record ScanCell(GridPos Pos, int DistanceFromStart)
{
  public ScanCell Parent { get; set; }
  public int DistanceFromStart { get; set; } = DistanceFromStart;
  public GridPos Pos { get; } = Pos;

  public List<ScanCell> Traverse()
  {
    var next = this;
    var cells = new List<ScanCell>() { this };
    while (next.Parent is not null)
    {
      next = next.Parent;
      cells.Add(next);
    }

    return cells;
  }

  public ScanCell GetRoot()
  {
    var next = Parent;
    while (next.Parent is not null)
    {
      next = next.Parent;
    }
    return next;
  }
}
