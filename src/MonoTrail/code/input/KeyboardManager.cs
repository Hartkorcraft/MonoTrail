using Microsoft.Xna.Framework.Input;

namespace MonoTrail.code.input;

public static class KeyboardManager
{
    static KeyboardState currentKeyState;
    static KeyboardState previousKeyState;

    public static KeyboardState UpdateState()
    {
        previousKeyState = currentKeyState;
        currentKeyState = Keyboard.GetState();
        return currentKeyState;
    }

    public static bool IsPressed(this Keys key)
        => currentKeyState.IsKeyDown(key);

    public static bool HasBeenJustPressed(this Keys key)
        => currentKeyState.IsKeyDown(key) && previousKeyState.IsKeyDown(key) == false;

    public static bool IsReleased(this Keys key)
        => currentKeyState.IsKeyDown(key) is false;

    public static bool HasBeenJustReleased(this Keys key)
        => currentKeyState.IsKeyDown(key) == false && previousKeyState.IsKeyDown(key);
}
