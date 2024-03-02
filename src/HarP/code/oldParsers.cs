// using HarP.code;

// namespace HarP.code;

// public delegate ParsingResult<T>[] Parser<T>(string scr);

// public static class Parsers
// {
//     public static Parser<T> Return<T>(T v) => inp => [new(v, inp)];
//     public static Parser<T> Failure<T>() => _ => [];

//     public static readonly Parser<char> Character =
//         i => string.IsNullOrEmpty(i)
//         ? Failure<char>()(i)
//         : Return(i[0])(i[1..]);

//     public static readonly Parser<char> Digit = Character.Where(char.IsDigit);

//     public static readonly Parser<char> Lower = Character.Where(char.IsLower);

//     public static readonly Parser<char> Upper = Character.Where(char.IsUpper);

//     public static readonly Parser<char> Letter = Character.Where(char.IsLetter);

//     public static readonly Parser<char> SpecialSymbol = Character.Where(char.IsSymbol);

//     public static readonly Parser<char> AlphaNum = Character.Where(char.IsLetterOrDigit);

//     public static ParsingResult<T>[] Parse<T>(this string input, Parser<T> p) => p(input);

//     public static Parser<T> Choice<T>(this Parser<T> p, Parser<T> q) //params
//         => inp => inp
//             .Parse(p)
//             .GetOut(out var result).Length == 0
//             ? inp.Parse(q)
//             : result;

//     public static Parser<T> Or<T>(this Parser<T> p, T or)
//         => p.Choice(Return(or));

//     public static Parser<T[]> Single<T>(this Parser<T> p)
//         => p.Bind(x => Return<T[]>([x]));

//     public static Parser<T> Choice<T>(this Parser<T> p, params Parser<T>[] q) //params
//         => input => ((Parser<T>[])[p, .. q])
//             .First(parser => parser(input).Length != 0)
//             .Choice(Failure<T>())(input);

//     public static Parser<T[]> Many<T>(this Parser<T> p)
//         => p.Aggregate(Array.Empty<T>(), (state, current) => [.. state, .. new[] { current }]);

//     public static Parser<T[]> Many1<T>(this Parser<T> p)
//         => p.Bind(v => p.Many().Bind(vs => Return<T[]>([v, .. vs])));

//     public static Parser<T[]> ManyMutRec<T>(this Parser<T> p)
//         => p.Many1().Choice(Return(Array.Empty<T>()));

//     public static Parser<T> Where<T>(this Parser<T> parser, Predicate<T> predicate)
//         => parser.Bind(c => predicate(c)
//             ? Return(c)
//             : Failure<T>());

//     public static Parser<string> Text(string str)
//     {
//         return inp => !inp.StartsWith(str) || inp.Length == 0
//             ? Failure<string>()(inp)
//             : Return(str)(inp[str.Length..]);
//     }

//     public static Parser<string> TextRec(string str)
//         => string.IsNullOrEmpty(str)
//             ? Return(string.Empty)
//             : from a in SpecificChar(str[0])
//               from b in Text(str[1..])
//               from result in Return(str)
//               select result;

//     public static Parser<TResult> Aggregate<TSource, TResult>(this Parser<TSource> p, TResult seed, Func<TResult, TSource, TResult> func)
//         => input =>
//         {
//             ParsingResult<TResult>[] state = [new ParsingResult<TResult>(seed, input)];
//             while (state.Length != 0)
//             {
//                 var newState = p(state[0].Unconsumed);
//                 if (newState.Length == 0)
//                     break;
//                 state = [new(func(state[0].Parsed, newState[0].Parsed), newState[0].Unconsumed)];

//             }
//             return state;
//         };

//     public static Parser<string> Identifier()
//         => from cs in AlphaNum.Many()
//            from result in Return(new string(cs))
//            select result;

//     public static Parser<T> Token<T>(this Parser<T> p)
//         => from before in Space
//            from token in p
//            from after in Space
//            from result in Return(token)
//            select result;

//     public static readonly Parser<string> IdentifierToken = Identifier().Token();

//     public static readonly Parser<int> NaturalToken = Natural!.Token();

//     public static Parser<string> Symbol(string xs)
//         => inp => inp.Parse(Text(xs).Token());

//     public static readonly Parser<int> Natural = Digit
//         .Many1()
//         .Bind(n => Return(int.Parse(string.Concat(n))));

//     public static readonly Parser<char> Space = Character
//         .Where(char.IsWhiteSpace)
//         .Many()
//         .Select(_ => ' ');

//     public static Parser<string> AtEndOfLine(string expected)
//         => from space in Space
//            from text in Text(expected)
//            from end in Text(Environment.NewLine).Many()
//            select text;

//     public static Parser<char> SpecificChar(char c)
//         => Character.Where(x => x == c);

//     public static Parser<char> AddOnNone(char c)
//         => Character.Where(x => x == c).Or(c);

//     public static Parser<T2> Bind<T1, T2>(this Parser<T1> p, Func<T1, Parser<T2>> f) => inp => inp
//         .Parse(p)
//         .GetOut(out var result).Length == 0
//             ? Failure<T2>()(inp)
//             : result[0].Unconsumed.Parse(f(result[0].Parsed));

//     public static Parser<T2> ChangeType<T1, T2>(this Parser<T1> p, Func<T1, T2> f)
//         => input => p(input)
//             .Select(result => new ParsingResult<T2>(f(result.Parsed), result.Unconsumed))
//             .ToArray();

//     public static readonly Parser<string> Line
//         = from chars in Character.Where(x => x != '\n' && x != '\r').Many1()
//            from _ in Text(Environment.NewLine).Many()
//            select new string(chars);

//     // public static Parser<string[]> IndentedLines(int depth)
//     //     => from result in (
//     //             from _ in Indentation(depth)
//     //             from line in Line()
//     //             select line).Many1()
//     //        select result;

//     public static Parser<T> WithIndentation<T>(this Parser<T> p, int depth)
//         => from _ in Indentation(depth)
//            from r in p
//            select r;

//     public static Parser<string> Indentation(int depth)
//         => from indent in Text(new string(' ', depth))
//            select indent;

//     public static Parser<TResult> Select<TSource, TResult>(this Parser<TSource> source, Func<TSource, TResult> selector)
//     {
//         return inp =>
//         {
//             var result = inp.Parse(source);
//             return result.Length == 0
//                 ? Failure<TResult>()(inp)
//                 : Return(selector(result[0].Parsed))(result[0].Unconsumed);
//         };
//     }

//     public static Parser<TResult> SelectMany<TSource, TValue, TResult>(
//         this Parser<TSource> source,
//         Func<TSource,
//         Parser<TValue>> valueSelector,
//         Func<TSource, TValue, TResult> resultSelector)
//         => source.Bind(s => valueSelector(s).Select(v => resultSelector(s, v)));
// }
