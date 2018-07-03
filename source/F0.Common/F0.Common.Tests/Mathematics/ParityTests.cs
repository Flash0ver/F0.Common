using System;
using F0.Mathematics;
using Xunit;

namespace F0.Tests.Mathematics
{
	public class ParityTests
	{
		[Theory]
		[InlineData(Int32.MinValue)]
		[InlineData(-10)]
		[InlineData(-8)]
		[InlineData(-6)]
		[InlineData(-4)]
		[InlineData(-2)]
		[InlineData(0)]
		[InlineData(12)]
		[InlineData(24)]
		[InlineData(36)]
		[InlineData(48)]
		[InlineData(50)]
		public void IfAnIntegerIsEvenlyDivisibleByTwo_ItIsEven_LeavingNoRemainder(int integer)
		{
			Assert.True(Parity.IsEven(integer));
			Assert.False(Parity.IsOdd(integer));
		}

		[Theory]
		[InlineData(-11)]
		[InlineData(-9)]
		[InlineData(-7)]
		[InlineData(-5)]
		[InlineData(-3)]
		[InlineData(-1)]
		[InlineData(1)]
		[InlineData(13)]
		[InlineData(25)]
		[InlineData(37)]
		[InlineData(49)]
		[InlineData(Int32.MaxValue)]
		public void IfAnIntegerIsNotEvenlyDivisibleByTwo_ItIsOdd_LeavingARemainderOfOne(int integer)
		{
			Assert.True(Parity.IsOdd(integer));
			Assert.False(Parity.IsEven(integer));
		}
	}
}
