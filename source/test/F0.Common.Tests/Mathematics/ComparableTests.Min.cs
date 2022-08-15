using System;
using F0.Mathematics;
using Xunit;

namespace F0.Tests.Mathematics
{
	public partial class ComparableTests
	{
		[Fact]
		public void Min_Null_Throws()
		{
			Assert.Throws<ArgumentNullException>("x", () => Comparable.Min(null!, "y"));
			Assert.Throws<ArgumentNullException>("y", () => Comparable.Min("x", null!));
		}

		[Theory]
		[InlineData(-1, 0)]
		[InlineData(0, +1)]
		public void Min_LeftIsLessThanRight_ReturnsLeft(int left, int right)
		{
			Assert.True(left < right);

			int min = Comparable.Min(left, right);

			Assert.Equal(left, min);
		}

		[Theory]
		[InlineData(0, -1)]
		[InlineData(+1, 0)]
		public void Min_LeftIsGreaterThanRight_ReturnsRight(int left, int right)
		{
			Assert.True(left > right);

			int min = Comparable.Min(left, right);

			Assert.Equal(right, min);
		}

		[Fact]
		public void Min_LeftIsEqualToRight_ReturnsLeft()
		{
			Version left = new(0, 11, 0);
			Version right = new(0, 11, 0);
			Assert.Equal(left, right);
			Assert.NotSame(left, right);

			Version min = Comparable.Min(left, right);

			Assert.Same(left, min);
		}
	}
}
