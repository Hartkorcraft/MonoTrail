using System;
using MonoTrail.code.tactics.data;
using MonoTrail.code.tactics.data.turn;
using TrailCore.extensions;
using TrailCore.tactics.data.components;

namespace MonoTrail.code.tactics.logic.turn;

public static class TurnManagerService
{
  public static (ChooseState prev, ChooseState next) ChangeChooseState(this TacticsSceneData tactics, ChooseState newState)
  {
    var prev = tactics.TurnData.CurrentChooseState;
    tactics.TurnData.CurrentChooseState = newState;

    _ = OnExit(prev, tactics);
    _ = OnEnter(newState, tactics);

    return (prev, newState);
  }

  static Unit OnExit(ChooseState c, TacticsSceneData t) => c switch
  {
    ChooseState.Wander => default,
    ChooseState.Aim => OnAimExit(t),
    _ => throw new NotImplementedException(),
  };

  static Unit OnEnter(ChooseState c, TacticsSceneData t) => c switch
  {
    ChooseState.Wander => default,
    ChooseState.Aim => OnAimEnter(t),
    _ => throw new NotImplementedException(),
  };

  static Unit OnAimEnter(TacticsSceneData tactics)
  {
    var tileSize = tactics.TileSetData.TileSize;
    tactics.TurnData.CurrentSelection
      .Bind(x => x.GetComponent<MapPosComponent>(tactics))
      .MatchEffect(m => tactics.SelectorData.TeleportSelectorTo(m.MapPos));

    tactics.SelectorData.Show = true;
    return default;
  }

  static Unit OnAimExit(TacticsSceneData tactics)
  {
    tactics.SelectorData.Show = false;
    return default;
  }
}
