using TrailCore.tactics.data.fov;

namespace TrailCore.tactics.data;

public record EntityData(
    HashSet<EntityID> Entities,
    ComponentStorage ComponentStorage,
    FovData FovData);
