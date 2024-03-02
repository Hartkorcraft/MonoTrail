using System;
using System.Collections.Generic;

namespace MonoTrail.extensions;

public class WeakEvent<TEventArgs>
{
    private readonly List<WeakReference> _listeners = [];

    public void AddListener(EventHandler<TEventArgs> handler)
    {
        _listeners.Add(new WeakReference(handler));
    }

    public void RemoveListener(EventHandler<TEventArgs> handler)
    {
        _ = _listeners.RemoveAll(wr => !wr.IsAlive || wr.Target.Equals(handler));
    }

    public void Raise(object sender, TEventArgs args)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            var weakReference = _listeners[i];
            if (weakReference.IsAlive)
            {
                ((EventHandler<TEventArgs>)weakReference.Target)?.Invoke(sender, args);
            }
            else
            {
                _listeners.RemoveAt(i);
            }
        }
    }
}
