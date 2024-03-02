namespace MonoTrail.code.story;

public static class StoryEventManager
{
    public enum StoryEvent
    {
        CLOSE_VIEW,
    }

    public static Unit ExecuteEvent(this GameState gameState, StoryEvent storyEvent)
      => storyEvent switch
      {
          StoryEvent.CLOSE_VIEW => gameState.CloseView(),
          _ => default
      };

    static Unit CloseView(this GameState gameState)
    {
        gameState.View = None;
        LogInfo("Closed view");
        return default;
    }
}
