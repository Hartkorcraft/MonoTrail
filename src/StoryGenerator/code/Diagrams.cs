using System.Drawing;
using CliWrap;
using Console = Colorful.Console;

namespace StoryGenerator.code;

public static class Diagrams
{
  const string TEMP_FILE_NAME = "diagram.tmp";
  const string DIAGRAMS_DIR_NAME = "storyDiagrams";

  public static async Task CreateDiagrams(string diagramText)
  {
    Console.WriteLine("Started creating diagrams!", Color.Lime);

    var dir = InitDiagramDir();

    File.WriteAllText(dir + TEMP_FILE_NAME, diagramText);

    _ = await Cli
      .Wrap("mmdc")
      .WithArguments($"--input {dir + TEMP_FILE_NAME} -o {dir}/diagram.svg -t dark -b transparent")
      .ExecuteAsync();

    Console.WriteLine("Successfuly created diagrams!");
  }

  static DirectoryInfo InitDiagramDir()
  {
    var dir = new DirectoryInfo(DIAGRAMS_DIR_NAME);

    if (!dir.Exists)
    {
      dir.Create();
    }
    else
    {
      foreach (FileInfo file in dir.GetFiles())
      {
        file.Delete();
      }
    }
    return dir;
  }
}
