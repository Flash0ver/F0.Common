using System;
using F0.Mathematics;
using Xunit;

namespace F0.Tests.Mathematics
{
	public partial class ComparableTests
	{
		[Fact]
		public void Clamp_Null_Throws()
		{
			Assert.Throws<ArgumentNullException>("value", () => Comparable.Clamp(null!, "", ""));
			Assert.Throws<ArgumentNullException>("min", () => Comparable.Clamp("", null!, ""));
			Assert.Throws<ArgumentNullException>("max", () => Comparable.Clamp("", "", null!));
		}

		[Fact]
		public void Clamp_MinGreaterThanMax_Throws()
		{
			ArgumentException f0Exception = Assert.Throws<ArgumentException>(() => Comparable.Clamp(2, 3, 1));

			string expectedMessage = GetMessage(2, 3, 1);

			Assert.Equal(expectedMessage, f0Exception.Message);

			static string GetMessage(int value, int min, int max)
			{
#if HAS_MATH_CLAMP
				ArgumentException mathException = Assert.Throws<ArgumentException>(() => Math.Clamp(value, min, max));
				return mathException.Message;
#else
				return $"'{min}' cannot be greater than {max}.";
#endif
			}
		}

		[Theory]
		[InlineData(1, 1, 3)]
		[InlineData(2, 1, 3)]
		[InlineData(3, 1, 3)]
		public void Clamp_ValueWithinInclusiveRange_ReturnsValue(int value, int min, int max)
		{
			int clamped = Comparable.Clamp(value, min, max);

			Assert.Equal(value, clamped);
		}

		[Theory]
		[InlineData(0, 1, 3)]
		public void Clamp_ValueLessThanMin_ReturnsMin(int value, int min, int max)
		{
			int clamped = Comparable.Clamp(value, min, max);

			Assert.Equal(min, clamped);
		}

		[Theory]
		[InlineData(4, 1, 3)]
		public void Clamp_ValueGreaterThanMax_ReturnsMax(int value, int min, int max)
		{
			int clamped = Comparable.Clamp(value, min, max);

			Assert.Equal(max, clamped);
		}

		[Fact]
		public void Clamp_LowerBoundEqualsUpperBound_ReturnsBound()
		{
			Assert.Equal(1, Comparable.Clamp(0, 1, 1));
			Assert.Equal(1, Comparable.Clamp(1, 1, 1));
			Assert.Equal(1, Comparable.Clamp(2, 1, 1));
		}

		[Fact]
		public void Clamp_TypeArgumentIsReferenceType_ReturnsPassedInstance()
		{
			string zero = NonInterned("0");
			string one = NonInterned("1");
			string two = NonInterned("2");
			string three = NonInterned("3");
			string four = NonInterned("4");

			string lowerBound = NonInterned("1");
			string upperBound = NonInterned("3");

			Assert.Same(lowerBound, Comparable.Clamp(zero, lowerBound, upperBound));
			Assert.Same(one, Comparable.Clamp(one, lowerBound, upperBound));
			Assert.Same(two, Comparable.Clamp(two, lowerBound, upperBound));
			Assert.Same(three, Comparable.Clamp(three, lowerBound, upperBound));
			Assert.Same(upperBound, Comparable.Clamp(four, lowerBound, upperBound));

			Assert.Same(one, Comparable.Clamp(zero, one, one));
			Assert.Same(one, Comparable.Clamp(one, one, one));
			Assert.Same(one, Comparable.Clamp(two, one, one));

			ArgumentException exception = Assert.Throws<ArgumentException>(() => Comparable.Clamp(two, three, one));
			Assert.Equal("'3' cannot be greater than 1.", exception.Message);
		}

		private static string NonInterned(string str)
		{
#if HAS_READONLY_SPAN
			string text = new(str);
#else
			Assert.Equal(1, str.Length);
			string text = new(str[0], 1);
#endif

			return text;
		}
	}
}
