using TrailCore.data.tactics;
using TrailCore.tactics.data;

namespace TrailCore.extensions;

public static class ComponentExtensions
{
  public static T GetComponent<T>(this EntityID e, TacticsSceneData t) where T : IComponent
    => t.EntityData.ComponentStorage.Get<T>(e);
}
