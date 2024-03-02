using System;

namespace FUnK.Option;

public readonly struct Some<T>
{
    internal T Value { get; }

    internal Some(T value)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));
        Value = value;
    }
}