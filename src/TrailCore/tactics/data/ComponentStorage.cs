using TrailCore.data.tactics;

namespace TrailCore.tactics.data;

public class ComponentStorage
{
  readonly Dictionary<Type, IComponentStorage> dict = [];

  public Dictionary<EntityID, T> GetComponents<T>() where T : IComponent
    => dict.ContainsKey(typeof(T))
       ? ((ComponentStorage<T>)dict[typeof(T)]).Components
       : [];

  public (EntityID key, T comp) Add<T>(EntityID key, T comp) where T : IComponent
  {
    if (dict.ContainsKey(typeof(T)) is false)
      dict[typeof(T)] = new ComponentStorage<T>();

    ((ComponentStorage<T>)dict[typeof(T)]).Components.Add(key, comp);
    return (key, comp);
  }

  public T Get<T>(EntityID key) where T : IComponent
    => ((ComponentStorage<T>)dict[typeof(T)]).Components[key];

  public Option<T> GetMaybe<T>(EntityID key) where T : IComponent
    => dict.ContainsKey(typeof(T))
      ? ((ComponentStorage<T>)dict[typeof(T)]).Components.TryGetOpt(key)
      : None;

  public bool Has<T>(EntityID id) where T : IComponent
        => ((ComponentStorage<T>)dict[typeof(T)]).Components.ContainsKey(id);

  public bool TryGet<T>(EntityID id, out T component) where T : IComponent
  {
    if (dict.ContainsKey(typeof(T)) is false)
      dict[typeof(T)] = new ComponentStorage<T>();
    var found = ((ComponentStorage<T>)dict[typeof(T)]).Components.TryGetValue(id, out var c);
    component = c!;
    return found;
  }
}

public interface IComponentStorage;

public class ComponentStorage<T> : IComponentStorage where T : IComponent
{
  public readonly Dictionary<EntityID, T> Components = [];
}
