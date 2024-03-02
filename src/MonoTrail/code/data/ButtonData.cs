using Microsoft.Xna.Framework;
using static MonoTrail.code.story.StoryEventManager;

namespace MonoTrail.code.data;

public record ButtonData(
    StoryEvent StoryEvent,
    string Text,
    Rectangle Rect);
