using static HarP.code.Parsers;
using static HarP.code.ParserConditions;
using HarP.code;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;

namespace HarP_Tests.code;

public partial class ParsersTests
{
	[Theory]
	[InlineData("XDDD")]
	[InlineData("")]
	[InlineData("123 abc")]
	[InlineData("@#$!%^")]
	public void Str_Success(string input)
	{
		var result = Str(input).Run(input);
		Assert.Equal(result.Result, input);
		Assert.Null(result.Error);
		Assert.False(result.IsError);
	}

	[Theory]
	[InlineData("[0]")]
	[InlineData("[1,2,3]")]
	[InlineData("[ 55325    ,    32   ,    3   ]")]
	[InlineData("[0000,       123]")]
	public void SeperateBy_Success(string input)
	{
		var result = Digits
			.SeperateBy(Char(','))
			.Many1()
			.Between(Char('['), Char(']'))
			.Run(input);

		var expected = FindNumbers()
			.Split(input)
			.Where(x => string.IsNullOrEmpty(x) is false)
			.Select(int.Parse)
			.ToArray();

		Assert.Equal(expected, result.Result);
		Assert.Null(result.Error);
		Assert.False(result.IsError);
	}

	[Fact]
	public void Bind_Success()
	{
		var parser = SequenceOf([Letters, Char(':')])
			.Map(result => result.Results()[0])
			.Bind(type => type.Result is "string"
			? Letters : type.Result is "int"
			? Digits : throw new ParsingException());

		var expected1 = "XDDD";
		var input1 = $"string:{expected1}";
		var result1 = parser.Run(input1);

		Assert.Equal(expected1, result1.Result);
		Assert.Null(result1.Error);
		Assert.False(result1.IsError);

		var expected2 = 2137;
		var input2 = $"int:{expected2}";
		var result2 = parser.Run(input2);

		Assert.Equal(expected2, result2.Result);
		Assert.Null(result2.Error);
		Assert.False(result2.IsError);
	}

	[Theory]
	[InlineData("0")]
	[InlineData("XDDD")]
	[InlineData("123 abc")]
	[InlineData("@#$!%^")]
	public void Str_Fail(string input)
	{
		var result = Str("Bruh").Run(input);
		Assert.Null(result.Result);
		Assert.True(result.IsError);
		Assert.NotNull(result.Error);
	}

	[Theory]
	[InlineData("XDDD", "Bruh")]
	[InlineData("123 abc", "Lmao")]
	[InlineData("@#$!%^", "Gda")]
	public void SequenceOf_Success(string input1, string input2)
	{
		var p = SequenceOf(
			Str(input1),
			Str(input2));

		var result = p.Run(input1 + input2);

		Assert.True(result.Results<string>().SequenceEqual([input1, input2]));
		Assert.Null(result.Error);
		Assert.False(result.IsError);
	}

	[Theory]
	[InlineData("XDDD", "Bruh")]
	[InlineData("123 abc", "Lmao")]
	[InlineData("@#$!%^", "Gda")]
	public void SequenceOf_Fail(string input1, string input2)
	{
		var p = SequenceOf([
			Str(input1),
			Str("xxxxxxxxxxxx")]);

		var result = p.Run(input1 + input2);

		Assert.True(result.Results<string>().SequenceEqual([input1]));
		Assert.True(result.IsError);
		Assert.NotNull(result.Error);
	}

	[Theory]
	[InlineData("G")]
	[InlineData("B")]
	[InlineData("1")]
	[InlineData("$")]
	[InlineData("asdasdfasga")]
	public void Char_Success(string input)
	{
		var result = Char(input[0]).Run(input);
		Assert.Equal(input[0], result.Result);
		Assert.Null(result.Error);
		Assert.False(result.IsError);
	}

	[Fact]
	public void Choice_Success()
	{
		var input1 = 'x';
		var input2 = 69;

		var p = Choice([
			Char('x'),
			Parsers.Numbers(69)]);

		var result1 = p.Run(input1.ToString());
		var result2 = p.Run(input2.ToString());

		Assert.Equal(input1, result1.Result);
		Assert.Null(result1.Error);
		Assert.False(result1.IsError);

		Assert.Equal(input2, result2.Result);
		Assert.Null(result2.Error);
		Assert.False(result2.IsError);
	}

	[Fact]
	public void Choice_Fail()
	{
		var p = Choice([
			Char('x'),
			Parsers.Numbers(69)]);

		var result = p.Run("Bruh");

		Assert.True(result.IsError);
		Assert.NotNull(result.Error);
	}

	[Fact]
	public void Between_Success()
	{
		const string input = "(hello)";
		var result = LetterOrDigit
			.Many()
			.Between(Char('('), Char(')'))
			.Run(input);

		Assert.Equal("hello", result.Result);
		Assert.Null(result.Error);
		Assert.False(result.IsError);
	}


	[Fact]
	public void Map_Success()
	{
		const string input = "bruh lol 123";
		var result = Str(input).Map(s => s.Result<string>().ToUpper()).Run(input);

		Assert.Equal(input.ToUpper(), result.Result<string>());
		Assert.Null(result.Error);
		Assert.False(result.IsError);
	}

	[Fact]
	public void MapError_Success()
	{
		const string input = "bruh lol 123";
		var result = Str("XD").MapError(s => s with { Error = "error" }).Run(input);

		Assert.Equal("error", result.Error);
		Assert.True(result.IsError);
	}

	[Theory]
	[InlineData("1234")]
	[InlineData("-69")]
	[InlineData("0")]
	public void IsNumber_Success(string input)
	{
		var result = Str(input).Where(StringNumberTransform).Run(input);
		Assert.Equal(int.Parse(input), result.Result);
		Assert.Null(result.Error);
		Assert.False(result.IsError);
	}

	[Theory]
	[InlineData("a")]
	[InlineData("aaaa")]
	[InlineData("abaaa")]
	[InlineData("bbbbaaa")]
	[InlineData("@#%@#%aaaa")]
	[InlineData("")]
	public void Many_Success(string input)
	{
		var result = Char('a').Many().Run(input);
		var expected = new string(input.TakeWhile(c => c == 'a').ToArray());

		Assert.Equal(expected.Length, (result.Result as Array)?.Length);
		Assert.Equal(expected, new string(result.Results<char>()));
		Assert.Null(result.Error);
		Assert.False(result.IsError);
	}

	[Theory]
	[InlineData("a")]
	[InlineData("aaaa")]
	[InlineData("ab")]
	public void Many1_Success(string input)
	{
		var result = Char('a').Many1().Run(input);
		var expected = new string(input.TakeWhile(c => c == 'a').ToArray());

		Assert.Equal(expected.Length, (result.Result as Array)?.Length);
		Assert.Equal(expected, new string(result.Results<char>()));
		Assert.Null(result.Error);
		Assert.False(result.IsError);
	}

	[Theory]
	[InlineData("gsaa")]
	[InlineData("@$#!$")]
	[InlineData("123")]
	[InlineData("")]
	public void Many1_Fail(string input)
	{
		var result = Char('a').Many1().Run(input);

		Assert.NotNull(result.Error);
		Assert.True(result.IsError);
	}

	[Fact]
	public void Contextual_Success()
	{
		static IEnumerable<ParserContext> VariableGenerator(ParserLazyState nextState)
		{
			yield return Choice(
				Str("int"),
				Str("string"));

			var type = nextState().Result<string>();

			yield return Token(Letters);
			var name = nextState().Result<string>();

			yield return Token(Char('='));

			yield return type switch
			{
				"int" => Token(Digits),
				"string" => Token(Letters),
				_ => throw new ParsingException()
			};
			var value = nextState().Result;

			yield return new(Success(nextState(), (type, name, value)), true);
		}

		var input = "int number = 123";

		var p = Contextual(VariableGenerator);

		var result = p.Run(input);

		// Assert.Equal(int.Parse(input), result.Result);
		Assert.Null(result.Error);
		Assert.False(result.IsError);
	}

	[GeneratedRegex(@"[^\d]")]
	private static partial Regex FindNumbers();
}