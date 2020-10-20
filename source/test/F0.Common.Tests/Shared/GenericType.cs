using System;
using System.Numerics;

namespace F0.Tests.Shared
{
	internal class GenericType<T1, T2>
	{
	}

	internal class GenericType<T> : GenericType<T, bool>
	{
	}

	internal class GenericType<T1, T2, T3> : GenericType<T1, Complex>
	{
	}

	internal class GenericType<T1, T2, T3, T4> : GenericType<T1, Func<T4>, T3>
	{
	}
}
