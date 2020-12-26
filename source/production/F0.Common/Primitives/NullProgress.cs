using System;

namespace F0.Primitives
{
	public sealed class NullProgress<T> : IProgress<T>
	{
		public static IProgress<T> Instance { get; } = new NullProgress<T>();

		internal NullProgress()
		{
		}

		void IProgress<T>.Report(T value)
		{
			//no-op
		}
	}
}
