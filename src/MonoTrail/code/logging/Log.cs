using System.Drawing;
using Console = Colorful.Console;

namespace MonoTrail.code.logging;

public static class Log
{
  public static Unit LogInfo(string t)
  {
    Console.WriteLine(t, Color.Lime);
    return default;
  }
}
