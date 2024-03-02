using System;
using Microsoft.Xna.Framework.Input;
using MonoTrail.code.input;

namespace MonoTrail.code.ext;

public static class InputExtensions
{
    public static bool JustPressed(this Keys key)
        => key.HasBeenJustPressed();

    public static bool JustPressed<T>(this Keys key, Func<T> getT, out T tOut)
        => key.HasBeenJustPressed() ? (tOut = getT()).Get(true) : (tOut = default).Get(false);

    public static bool JustReleased<T>(this Keys key, Func<T> getT, out T tOut)
        => key.HasBeenJustReleased() ? (tOut = getT()).Get(true) : (tOut = default).Get(false);

    public static bool JustPressed<T>(this Keys key, T getT, out T tOut)
        => key.HasBeenJustPressed() ? (tOut = getT).Get(true) : (tOut = default).Get(false);

    public static bool JustReleased<T>(this Keys key, T getT, out T tOut)
        => key.HasBeenJustReleased() ? (tOut = getT).Get(true) : (tOut = default).Get(false);

    public static bool Pressing<T>(this Keys key, Func<T> getT, out T tOut)
        => key.IsPressed() ? (tOut = getT()).Get(true) : (tOut = default).Get(false);

    public static bool Pressing<T>(this Keys key, T getT, out T tOut)
        => key.IsPressed() ? (tOut = getT).Get(true) : (tOut = default).Get(false);
}
