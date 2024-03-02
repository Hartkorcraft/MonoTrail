namespace TrailCore.tactics.data.fov;

public record FovData()
{
  public HashSet<GridPos> PlayerView { get; set; } = [];
  public HashSet<GridPos> EnemyView { get; set; } = [];
  public HashSet<GridPos> SeenByPlayer { get; set; } = [];

  public void Deconstruct(out HashSet<GridPos> playerView, out HashSet<GridPos> enemyView, out HashSet<GridPos> seenByPlayer)
  {
    playerView = PlayerView;
    enemyView = EnemyView;
    seenByPlayer = SeenByPlayer;
  }
}
