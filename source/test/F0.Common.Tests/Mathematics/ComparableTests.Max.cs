using System;
using F0.Mathematics;
using Xunit;

namespace F0.Tests.Mathematics
{
	public partial class ComparableTests
	{
		[Fact]
		public void Max_Null_Throws()
		{
			Assert.Throws<ArgumentNullException>("x", () => Comparable.Max(null!, "y"));
			Assert.Throws<ArgumentNullException>("y", () => Comparable.Max("x", null!));
		}

		[Theory]
		[InlineData(-1, 0)]
		[InlineData(0, +1)]
		public void Max_LeftIsLessThanRight_ReturnsRight(int left, int right)
		{
			Assert.True(left < right);

			int max = Comparable.Max(left, right);

			Assert.Equal(right, max);
		}

		[Theory]
		[InlineData(0, -1)]
		[InlineData(+1, 0)]
		public void Max_LeftIsGreaterThanRight_ReturnsLeft(int left, int right)
		{
			Assert.True(left > right);

			int max = Comparable.Max(left, right);

			Assert.Equal(left, max);
		}

		[Fact]
		public void Max_LeftIsEqualToRight_ReturnsLeft()
		{
			Version left = new(0, 11, 0);
			Version right = new(0, 11, 0);
			Assert.Equal(left, right);
			Assert.NotSame(left, right);

			Version max = Comparable.Max(left, right);

			Assert.Same(left, max);
		}
	}
}
