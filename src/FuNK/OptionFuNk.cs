using System.Collections.Generic;
using System.Linq;
using FUnK.Option;
 
namespace FUnK;
  
public static partial class FuNk
{
	public static Option.SubTypes.None None => Option.SubTypes.None.Default;
	public static Some<T> Some<T>(T value) => new(value);
	public static Option<T> TryGetOpt<R, T>(this IDictionary<R, T> dict, R key) => dict.TryGetValue(key, out var value) ? value : None;
 
	public static Option<T> TryPop<T>(this Stack<T> stack) => stack.Count != 0 ? stack.Pop() : None;
	public static Option<T> TryPeek<T>(this Stack<T> stack) => stack.Count != 0 ? stack.Peek() : None;
}
