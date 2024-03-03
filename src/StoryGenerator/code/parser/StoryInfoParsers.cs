using HarP.code;
using static HarP.code.Parsers;

namespace StoryGenerator.code.parser;

public static class StoryInfoParsers
{
    public static Parser SectionHeader(string sectionName, bool oneLine = false)
        => SequenceOf(
            Token(Str(sectionName)),
            LineToken(Char(':')),
            oneLine ? Success() : EndOfLineOrEndOfText);

    public static readonly Parser SectionId = SequenceOf(
        SectionHeader("ID", true),
        EndLineToken(Letters))
        .Recover(1);

    public static readonly Parser SectionText = SequenceOf(
        SectionHeader("TEXT"),
        SequenceOf(
            SpaceIndentation(4),
            EndLineToken(Letters)
            .Many1())
            .Recover(1))
        .Recover(1);
    // public static readonly Parser ParseSection
}

// using static HarP.code.Parsers;

// namespace HarP.code.mxt;

// public static class MxrParsers
// {
//   public static Parser<Section> Section()
//     => from name in SectionKeyword()
//        from section in SectionSelector(name)
//        select section;

//   public static Parser<string> SectionKeyword()
//     => from name in Identifier().Token()
//        from end in AtEndOfLine(":")
//        select name	;

//   public static readonly Parser<Section> ParseIdSection
//     = from id in Identifier().Token()
//       select (Section)new Section_ID(id);

//   public static readonly Parser<Section> ParseDescription
//     = from lines in Line.WithIndentation(4).Many1()
//       select (Section)new Section_Description(lines);

//   public static Parser<Section> SectionSelector(string name)
//     => name switch
//     {
//       "ID" => ParseIdSection,
//       "DESCRIPTION" => ParseDescription,
//       _ => throw new NotSupportedException()
//     };
// }

// public record Section;

// public record Section_Description(string[] Text) : Section;
// public record Section_ID(string Id) : Section;
// public record Section_Effects();
// public record Section_Params();
// public record StoryDeclaration();

// // public static readonly Parser<Section> ParseTextSection
// //   = from lines in IndentedLine.Many1()
// //     select (Section)new Section_Text(string.Join(Environment.NewLine, lines));
