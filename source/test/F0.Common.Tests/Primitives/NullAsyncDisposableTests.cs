using System;
using System.Reflection;
using System.Threading.Tasks;
using F0.Primitives;
using Xunit;

namespace F0.Tests.Primitives
{
	public class NullAsyncDisposableTests
	{
		[Fact]
		public void ConstructorIsInaccessible_SingletonPattern()
		{
			Type type = typeof(NullAsyncDisposable);
			ConstructorInfo[] publicConstructors = type.GetConstructors();

			Assert.Empty(publicConstructors);
		}

		[Fact]
		public void NullObjectIsStateless()
		{
			Type type = typeof(NullAsyncDisposable);
			BindingFlags lookup = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

			FieldInfo[] fields = type.GetFields(lookup);
			Assert.Empty(fields);
		}

		[Fact]
		public void Singleton_OneSingleInstanceIsShared()
		{
			IAsyncDisposable first = NullAsyncDisposable.Instance;
			IAsyncDisposable second = NullAsyncDisposable.Instance;

			Assert.Same(first, second);
		}

		[Fact]
		public async Task DisposingHasNoSideEffects()
		{
			IAsyncDisposable instance = new NullAsyncDisposable();
			int hashCode = instance.GetHashCode();

			await instance.DisposeAsync();
			Assert.Equal(hashCode, instance.GetHashCode());
		}

		[Fact]
		public async Task DisposeAsync_MustNotThrowAnExceptionIfTheMethodIsCalledMultipleTimes()
		{
			IAsyncDisposable instance = new NullAsyncDisposable();

			await instance.DisposeAsync();
			await instance.DisposeAsync();
		}

		[Fact]
		public void DisposeAsync_SynchronouslyReturnSuccessfullyCompletedValueTask()
		{
			IAsyncDisposable instance = new NullAsyncDisposable();

			ValueTask valueTask = instance.DisposeAsync();
			Assert.True(valueTask.IsCompletedSuccessfully);
		}
	}
}
