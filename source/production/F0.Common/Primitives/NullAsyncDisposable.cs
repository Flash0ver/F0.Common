using System;
using System.Threading.Tasks;

namespace F0.Primitives
{
	public sealed class NullAsyncDisposable : IAsyncDisposable
	{
		public static IAsyncDisposable Instance { get; } = new NullAsyncDisposable();

		internal NullAsyncDisposable()
		{
		}

		ValueTask IAsyncDisposable.DisposeAsync()
		{
			return default;
		}
	}
}
