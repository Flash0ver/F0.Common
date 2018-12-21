using System;
using System.Reflection;
using F0.Primitives;
using Xunit;

namespace F0.Tests.Primitives
{
	public class NullProgressTests
	{
		[Fact]
		public void ConstructorIsInaccessible_SingletonPattern()
		{
			Type type = typeof(NullProgress<>);
			ConstructorInfo[] publicConstructors = type.GetConstructors();

			Assert.Empty(publicConstructors);
		}

		[Fact]
		public void NullObjectIsStateless()
		{
			Type type = typeof(NullProgress<>);
			BindingFlags lookup = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

			FieldInfo[] fields = type.GetFields(lookup);
			Assert.Empty(fields);
		}

		[Fact]
		public void Singleton_OneSingleInstanceIsShared()
		{
			IProgress<object> first = NullProgress<object>.Instance;
			IProgress<object> second = NullProgress<object>.Instance;

			Assert.Same(first, second);
		}

		[Fact]
		public void ReportingProgressUpdateHasNoSideEffects()
		{
			IProgress<int> instance = new NullProgress<int>();
			int hashCode = instance.GetHashCode();

			instance.Report(240);
			Assert.Equal(hashCode, instance.GetHashCode());
		}
	}
}
