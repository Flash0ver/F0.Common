using System;

namespace F0.Primitives
{
	public sealed class ImmediateProgress<T> : IProgress<T>
	{
		private readonly Action<T> handler;

		public ImmediateProgress(Action<T> handler)
		{
			this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
		}

		void IProgress<T>.Report(T value)
		{
			handler(value);
		}
	}
}
