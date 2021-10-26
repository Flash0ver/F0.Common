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
#if HAS_VALUETASK_COMPLETEDTASK
			return ValueTask.CompletedTask;
#else
			return default;
#endif
		}
	}
}
