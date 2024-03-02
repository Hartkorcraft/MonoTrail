namespace HarP.code;

public static class ParserConditions
{
    public static readonly Transform<string, int> StringNumberTransform = parserState =>
    {
        if (parserState.IsError) return parserState;
        return int.TryParse((string?)parserState.Result, out var number)
            ? parserState.UpdateParserState(number)
            : parserState.UpdateParserError($"StringIntMap: Failed to convert string result to int at index {parserState.Index}");
    };
}


// public static Parser StringIntMap(this Parser parser) => input =>
// {
//     var nextState = parser(input);
//     if (nextState.IsError) return nextState;
//     return int.TryParse((string?)nextState.Result, out var number)
//         ? nextState.UpdateParserState(number)
//         : nextState.UpdateParserError($"StringIntMap: Failed to convert string result to int at index {nextState.Index}");
// };