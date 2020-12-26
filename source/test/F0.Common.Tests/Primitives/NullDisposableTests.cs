using System;
using System.Reflection;
using F0.Primitives;
using Xunit;

namespace F0.Tests.Primitives
{
	public class NullDisposableTests
	{
		[Fact]
		public void ConstructorIsInaccessible_SingletonPattern()
		{
			Type type = typeof(NullDisposable);
			ConstructorInfo[] publicConstructors = type.GetConstructors();

			Assert.Empty(publicConstructors);
		}

		[Fact]
		public void NullObjectIsStateless()
		{
			Type type = typeof(NullDisposable);
			BindingFlags lookup = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

			FieldInfo[] fields = type.GetFields(lookup);
			Assert.Empty(fields);
		}

		[Fact]
		public void Singleton_OneSingleInstanceIsShared()
		{
			IDisposable first = NullDisposable.Instance;
			IDisposable second = NullDisposable.Instance;

			Assert.Same(first, second);
		}

		[Fact]
		public void DisposingHasNoSideEffects()
		{
			IDisposable instance = new NullDisposable();
			int hashCode = instance.GetHashCode();

			instance.Dispose();
			Assert.Equal(hashCode, instance.GetHashCode());
		}

		[Fact]
		public void Dispose_MustNotThrowAnExceptionIfTheMethodIsCalledMultipleTimes()
		{
			IDisposable instance = new NullDisposable();

			instance.Dispose();
			instance.Dispose();
		}
	}
}
