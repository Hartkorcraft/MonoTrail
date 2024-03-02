using TrailCore.data.tactics;

namespace TrailCore.tactics.initialization;

public delegate T ComponentInitializer<T>(TacticsInitData tacticsInitData) where T : IComponent;

public static class ComponentInitializationExtensions
{
  public static TacticsInitData AddComponent<T>(this TacticsInitData data, ComponentInitializer<T> init)
      where T : IComponent
      => data.TacticsSceneData.EntityData.ComponentStorage.Add(data.Id, init(data)).Get(data);
}
