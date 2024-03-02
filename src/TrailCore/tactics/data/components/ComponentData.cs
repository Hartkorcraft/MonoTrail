// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;
// using Microsoft.Xna.Framework;
// using MonoXTrail.src.extensions.collections;
// using MonoXTrail.src.main.data;
// using MonoXTrail.src.main.scenes.tactics.data.entity;
// using MonoXTrail.src.main.scenes.tactics.logic.managers;

// namespace MonoXTrail.src.main.scenes.tactics.data.components;

// public record ComponentData
// {
//     public EntityID TileSelectorId { get; init; } = selectorId;
//     public Option<EntityID> CurrentSelection { get; set; } = None;
//     public Option<EntityID> InfoFocusedEntity { get; set; } = None;

//     private static readonly EntityID selectorId = new(Guid.NewGuid());

//     public ComponentData()
//     {
//         ComponentStorage.ClearStorages.ForeachAct();
//         Add(selectorId, new SpriteComponent()
//         {
//             Layer = SpriteComponent.TILE_SELECTOR_LAYER,
//             Visible = false,
//             SpriteName = TextureNames.TileSelectorTex,
//             Color = new Color(1, 1, 1, 0.0001f)
//         });
//         Add(selectorId, new MapPosComponent()
//         {
//             MapPos = (0, 0),
//             IsBlockingPos = false
//         });
//         Add(selectorId, new OffsetPosComponent()
//         {
//             IgnoreFov = true
//         });
//     }

//     public static ValueDictionary<EntityID, T> GetDict<T>() where T : IComponent
//         => ComponentStorage<T>.Dict;

//     public static T Add<T>(EntityID entityID, T component) where T : IComponent
//         => ComponentStorage<T>.Dict[entityID] = component; // TODO CHANGE TO .ADD

//     public static T Get<T>(EntityID id) where T : IComponent
//         => ComponentStorage<T>.Dict[id];

//     public static bool Has<T>(EntityID id) where T : IComponent
//         => ComponentStorage<T>.Dict.ContainsKey(id);

//     public static bool TryGet<T>(EntityID id, out T component) where T : IComponent
//     {
//         var found = ComponentStorage<T>.Dict.TryGetValue(id, out T _component);
//         component = _component;
//         return found;
//     }

//     public static Option<T> GetOption<T>(EntityID id) where T : IComponent
//         => ComponentStorage<T>.Dict.Dict.TryGetOpt(id);

//     public void RemoveEntity(EntityID id, TacticsData tacticsData)
//     {
//         GetDict<MapPosComponent>()
//             .TryGetOpt(id)
//             .MatchEffect(m => tacticsData.MapData.Tiles[TilemapManager.PosToTileIndex(m.MapPos, tacticsData.MapData)].Occupying = None);

//         CurrentSelection
//             .Where(curSelection => curSelection == id)
//             .MatchEffect(x => CurrentSelection = None);

//         InfoFocusedEntity
//             .Where(curSelection => curSelection == id)
//             .MatchEffect(x => CurrentSelection = None);

//         foreach (var type in ComponentStorage.Types)
//         {
//             var method = typeof(ComponentData).GetMethod("RemoveFromDict", BindingFlags.Static | BindingFlags.NonPublic);
//             var generic = method.MakeGenericMethod(type);
//             generic.Invoke(null, [id]);
//         }
//     }

//     public static ValueArray<object> ScrapeComponents()
//     {
//         return ComponentStorage.Types.Select(type =>
//         {
//             var method = typeof(ComponentData).GetMethod("Scrape", BindingFlags.Static | BindingFlags.NonPublic);
//             var generic = method.MakeGenericMethod(type);
//             return generic.Invoke(null, null);
//         }).ToValueArray();
//     }

//     static void RemoveFromDict<T>(EntityID id) where T : IComponent => GetDict<T>().Remove(id);
//     static ValueDictionary<EntityID, T> Scrape<T>() where T : IComponent => GetDict<T>();

//     static class ComponentStorage
//     {
//         public static HashSet<Action> ClearStorages = [];
//         public static HashSet<Type> Types = [];
//     }

//     static class ComponentStorage<T> where T : IComponent
//     {
//         public static ValueDictionary<EntityID, T> Dict { get; } = [];

//         public static ValueDictionary<EntityID, T> GetDict() => Dict;

//         static ComponentStorage()
//         {
//             ComponentStorage.Types.Add(typeof(T));
//             ComponentStorage.ClearStorages.Add(Dict.Clear);
//         }
//     }
// }