using System;

namespace F0.Primitives
{
	public sealed class NullDisposable : IDisposable
	{
		public static NullDisposable Instance { get; } = new NullDisposable();

		internal NullDisposable()
		{
		}

		void IDisposable.Dispose()
		{
			//no-op
		}
	}
}
