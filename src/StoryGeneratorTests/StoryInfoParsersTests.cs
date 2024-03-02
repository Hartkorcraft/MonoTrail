using System.Linq;
using static StoryGenerator.code.parser.StoryInfoParsers;
using static HarP.code.Parsers;
using System.Text;

namespace StoryGeneratorTests;

public class StoryInfoParsersTests
{
    [Fact]
    public void SectionText_Works()
    {
        const string line1 = "Bruh";
        const string line2 = "asfgasgasgalkhk";

        var input = new StringBuilder();
        input.Append("TEXT:" + Environment.NewLine);
        input.Append($"    {line1}" + Environment.NewLine);
        input.Append($"    {line2}" + Environment.NewLine);

        var result = SectionText.Run(input.ToString());
        var text = (string[])result.Results()[1];

        Assert.Equal([line1, line2], text);
        Assert.False(result.IsError, result.Error?.ToString());
        Assert.Null(result.Error);
    }

    [Theory]
    [InlineData("ID: lol")]
    [InlineData("ID: more_allowed_characters")]
    [InlineData("ID: yy\r\n")]
    [InlineData("ID: gg  \r\n")]
    [InlineData("ID  : yoy")]
    [InlineData("ID:XD\r\n")]
    [InlineData("  ID  :t")]
    [InlineData("   ID   :    lol  \r\n")]
    [InlineData("   ID   : jj  \r\n")]
    public void SectionId_Works(string input)
    {
        var state = SectionId.Run(input);

        var result = state.Result<string>();

        var expected = input[(input.LastIndexOf(':') + 1)..].Trim();

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData($"ID: \r\n bruh")]
    [InlineData("   I D   :    lol   ")]
    [InlineData("   ID  l :    lol   ")]
    [InlineData("   ID   :  jj  lol   ")]
    public void SectionId_Fail(string input)
    {
        var r = SectionId
            .Run(input);

        Assert.True(r.IsError);
        Assert.NotNull(r.Error);
    }
}