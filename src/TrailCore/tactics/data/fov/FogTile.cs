namespace TrailCore.tactics.data.fov;

public record FogTile(int Subdivide) // TODO
{
    public bool[][] Seeing = Enumerable.Range(0, Subdivide).Select(_ => new bool[Subdivide]).ToArray();
}
