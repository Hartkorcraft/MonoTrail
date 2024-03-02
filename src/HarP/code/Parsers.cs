using System.Globalization;

namespace HarP.code;

public record ParserState(
    string ToBeParsed,
    object? Result,
    int Index = 0,
    bool IsError = false,
    object? Error = null);

public record ParserResultCondition<T>(Predicate<T?> Condition, object? Error);
public record ParserStateCondition(Predicate<ParserState> Condition, object? Error);

public readonly record struct ParserContext(Parser Parser, bool Finished = false)
{
    public static implicit operator ParserContext(Parser p) => new(p, false);
};

public delegate ParserState Parser(ParserState state);

public delegate object Transform(ParserState state);

public delegate ParserState Transform<T, G>(ParserState state);

public delegate Parser ParserGenerator(ParserState state);

public delegate Parser ParserBox(Parser state);

public delegate ParserState ParserLazyState();

public delegate IEnumerable<ParserContext> ContextualParserGenerator(ParserLazyState lazyState);

public static class Parsers
{
    public static Parser Char() => prev =>
    {
        var (toBeParsed, prevResult, index, IsError, Error) = prev;

        if (IsError) return prev;

        if (index >= toBeParsed.Length)
            return prev.UpdateParserError($"Char: index outside range {index}");

        return prev.UpdateParserState(index + 1, toBeParsed[index]);
    };

    public static Parser Success() => prev => prev;
    public static Parser Success(ParserState s) => s => s;
    public static Parser Success(ParserState prev, object? newResult) => s => s with { Result = newResult };


    public static Parser Maybe(this Parser parser) => Choice([parser, Success()]);

    public static Parser Char(Predicate<char> predicate, object? error)
    => Char().Where<char>(new(
        Condition: predicate,
        Error: error));

    public static Parser Chars(char expected, int ammount) => Str(new string(expected, ammount));

    public static Parser SpaceIndentation(int ammount) => Str(new string(' ', ammount));

    public static readonly Parser LetterOrDigit = Char(
        char.IsLetterOrDigit,
        "LetterOrDigit: Is a symbol");

    public static Parser Space(bool includeEndOfLine = true) => Char(
        c => c is ' ' || (includeEndOfLine && c is '\n' or '\r'),
        "Is not space");

    public static readonly Parser Letter = Char(
        char.IsLetter,
        "Letter: Is not a letter");

    public static readonly Parser Digit = Char(
        char.IsDigit,
        "Digit: Is not a Digits");

    public static readonly Parser Letters = Letter.Many1().Map(s => new string(s.Results<char>()));

    public static Parser Spaces(bool includeEndOfLine = true)
        => Space(includeEndOfLine)
            .Many()
            .Map(s => new string(s.Results<char>()));

    public static readonly Parser Digits = Digit.Many1().Map(s => int.Parse(new string(s.Results<char>())));

    public static Parser Char(char expected)
    => Str(new string([expected]))
        .Map(_ => expected);

    public static Parser Str(string expected, bool ignoreCase = true) => prev =>
    {
        var (toBeParsed, prevResult, index, IsError, Error) = prev;

        if (IsError) return prev;

        if (index > toBeParsed.Length)
            throw new ParsingException("Length is smaller than index");

        return toBeParsed[index..]
            .StartsWith(expected, ignoreCase, CultureInfo.InvariantCulture) is false
            ? prev.UpdateParserError(Expected(expected, prev))
            : prev.UpdateParserState(index + expected.Length, expected);
    };

    public static Parser Numbers(int expected)
        => Str(expected.ToString())
            .Map(r => int.Parse((string?)r.Result ?? throw new ParsingException()));

    public static (bool finished, Parser parser) ToContextState(this Parser p, bool finished = false) => (finished, p);

    public static Parser Contextual(ContextualParserGenerator parserGenerator)
    {
        return Success().Bind(state =>
        {
            Parser RunStep(ParserState nextValue)
            {
                ParserState iteratorResult = nextValue;//
                foreach (var (parser, finished) in parserGenerator(() => iteratorResult))
                {
                    if (finished) return Success(iteratorResult);
                    iteratorResult = parser(iteratorResult);
                }

                return RunStep(iteratorResult);//iteratorResult.Bind(RunStep);
            }
            return RunStep(state);
        });
    }

    public static Parser SequenceOf(params Parser[] parsers) => prev =>
    {
        if (prev.IsError) return prev;

        List<object> results = [];
        var nextState = prev;

        foreach (var p in parsers)
        {
            nextState = p(nextState);

            if (nextState.IsError)
                return nextState with { Result = results.ToArray() };

            if (nextState.Result is null)
                throw new ParsingException();

            results.Add(nextState.Result);
        }
        return nextState with { Result = results.ToArray() };
    };

    public static Parser Choice(params Parser[] parsers) => prev =>
   {
       if (prev.IsError) return prev;

       List<object?> errors = [];
       foreach (var p in parsers)
       {
           var nextState = p(prev);
           if (nextState.IsError)
           {
               errors.Add(nextState.Error);
               continue;
           }
           if (nextState.Result is null)
               throw new ParsingException();
           return nextState;
       }
       var errorSpace = Environment.NewLine + ">>>>";
       var errorsToDisplay = errorSpace + string.Join(errorSpace, errors.Select(e => e?.ToString()));
       return prev.UpdateParserError($"choice: Failed at index: {prev.Index},{Environment.NewLine}Errors: {errorsToDisplay}");
   };

    public static Parser Where<T>(this Parser parser, ParserResultCondition<T> parserCondition) => prev =>
    {
        if (prev.IsError) return prev;
        var nextState = parser(prev);
        return nextState.IsError is false && parserCondition.Condition((T?)nextState.Result)
            ? nextState
            : nextState.UpdateParserError(parserCondition.Error);
    };

    public static Parser Where(this Parser parser, ParserStateCondition parserStateCondition) => prev =>
    {
        if (prev.IsError) return prev;
        var nextState = parser(prev);
        return nextState.IsError is false && parserStateCondition.Condition(nextState)
            ? nextState
            : nextState.UpdateParserError(parserStateCondition.Error);
    };

    public static Parser Where<T, G>(this Parser parser, Transform<T, G> transform) => prev =>
    {
        if (prev.IsError) return prev;
        var nextState = parser(prev);
        return transform(nextState);
    };

    public static Parser Between(this Parser content, Parser left, Parser right)
    => SequenceOf(left, content, right).Recover(1);

    public static Parser SeperateBy(this Parser contentParser, Parser seperatorParser)
    => SequenceOf(Token(contentParser), Maybe(seperatorParser)).Recover(0);

    public static readonly ParserBox Token = content => content.Between(Spaces(), Spaces());

    public static readonly ParserBox LineToken = content => content.Between(Spaces(false), Spaces(false));

    public static readonly ParserBox EndLineToken = content => SequenceOf(
        content.Between(Spaces(false), Spaces(false)),
        Choice(EndOfLine, EndOfText)).Recover(0);

    public static readonly Parser EndOfLine = SequenceOf(
        Char('\r'),
        Char('\n'));

    public static readonly Parser EndOfText = Success()
        .Where(new ParserStateCondition(x => x.Index >= x.ToBeParsed.Length, "Not end of file"));

    public static readonly Parser EndOfLineOrEndOfText = Choice(EndOfLine, EndOfText);

    public static Parser ParseLine(this Parser parser) => SequenceOf(parser, EndOfLineOrEndOfText);

    public static Parser Many(this Parser parser) => prev =>
    {
        if (prev.IsError) return prev;

        var nextState = prev;

        List<object?> results = [];

        while (nextState.IsError is false)
        {
            var testState = parser(nextState);
            if (testState.IsError) break;
            nextState = parser(nextState);
            results.Add(nextState.Result);
        }

        return UpdateParserState(nextState, results.ToArray());
    };

    public static Parser Many1(this Parser parser) => prev =>
    {
        if (prev.IsError) return prev;
        var nextState = Many(parser)(prev);

        return nextState.Result is Array a && a.Length == 0
            ? UpdateParserError(nextState, $"Many1: Found no results at index: {prev.Index}")
            : UpdateParserState(nextState, nextState.Result);
    };

    public static Parser Recover<T>(this Parser parser, int i)
    => parser.Map(r => r.Results<T>()[i] ?? throw new ParsingException());

    public static Parser Recover(this Parser parser, int i)
    => parser.Map(r => r.Results<object>()[i] ?? throw new ParsingException());

    public static Parser Recover(this Parser parser, Range range)
    => parser.Map(r => r.Results<object>()[range] ?? throw new ParsingException());

    public static Parser Map(this Parser parser, Transform fn) => prev =>
    {
        var nextState = parser(prev);
        if (nextState.IsError) return nextState;
        var transformed = fn(nextState);
        return nextState.UpdateParserState(transformed);
    };

    // public static ParserState Map(this ParserState state, Transform fn)
    // {
    //     if (state.IsError) return state;
    //     var transformed = fn(state);
    //     return state.UpdateParserState(transformed);
    // }

    public static Parser Bind(this Parser parser, Func<ParserState, Parser> fn) => prev =>
    {
        var nextState = parser(prev);
        if (nextState.IsError) return nextState;
        var newParser = fn(nextState);
        return newParser(nextState);
    };

    public static Parser MapError(this Parser parser, Parser fn) => prev =>
    {
        var nextState = parser(prev);
        if (nextState.IsError is false) return nextState;
        var transformed = fn(nextState);
        return transformed.UpdateParserError(transformed.Error);
    };

    public static ParserState Run(this Parser parser, string input)
    {
        var initialState = new ParserState(
            ToBeParsed: input,
            Result: null,
            Index: 0);

        return parser(initialState);
    }

    public static T[] Results<T>(this ParserState parserState)
        => [.. ((object[])parserState.Result!).Cast<T>()];

    public static object[] Results(this ParserState parserState)
        => (object[])(parserState.Result ?? throw new ParsingException());

    public static T Result<T>(this ParserState parserState)
        => (T?)parserState.Result ?? throw new ParsingException();

    public static string Expected(string expected, ParserState parserState, int lookForward = 10)
    {
        var (toBeParsed, _, index, _, _) = parserState;
        var forward = index + 10;
        var range = index..(toBeParsed.Length > forward
            ? forward
            : toBeParsed.Length);
        return $"Tried to parse %{expected}% but got \"{toBeParsed[range]}\"";
    }

    public static ParserState UpdateParserState(this ParserState next, int index, object result)
    => next with
    {
        ToBeParsed = next.ToBeParsed,
        Result = result,
        Index = index
    };

    public static ParserState UpdateParserState(this ParserState next, object? result)
    => next with
    {
        ToBeParsed = next.ToBeParsed,
        Result = result,
    };

    public static ParserState UpdateParserError(this ParserState prev, object? error)
    => prev with
    {
        IsError = true,
        Error = error
    };
}
