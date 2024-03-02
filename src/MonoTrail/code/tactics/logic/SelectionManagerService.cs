using System;
using TrailCore.tactics.data;

namespace MonoTrail.code.tactics.logic;

public class ChangedSelectionArgs(Option<EntityID> prev, Option<EntityID> newS)
{
  public Option<EntityID> PreviousSelection { get; } = prev;
  public Option<EntityID> NewSelection { get; } = newS;
}

public static class SelectionManagerService
{
  public static event EventHandler<ChangedSelectionArgs> ChangedSelection;

  public static ChangedSelectionArgs ChangeSelection(this data.TacticsSceneData tactics, Option<EntityID> newSelection)
  {
    var turnData = tactics.TurnData;
    var selectionArgs = new ChangedSelectionArgs(turnData.CurrentSelection, turnData.CurrentSelection = newSelection);
    ChangedSelection?.Invoke(null, selectionArgs);

    return selectionArgs;
  }
}

