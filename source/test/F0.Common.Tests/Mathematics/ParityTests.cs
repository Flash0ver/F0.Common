using System;
using F0.Mathematics;
using Xunit;

namespace F0.Tests.Mathematics
{
	public class ParityTests
	{
		[Theory]
		[InlineData(SByte.MinValue)]
		[InlineData(-2)]
		[InlineData(0)]
		[InlineData(+2)]
		public void Even_8bitSignedInteger(sbyte integer)
		{
			Assert.True(Parity.IsEven(integer));
			Assert.False(Parity.IsOdd(integer));
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(+1)]
		[InlineData(SByte.MaxValue)]
		public void Odd_8bitSignedInteger(sbyte integer)
		{
			Assert.True(Parity.IsOdd(integer));
			Assert.False(Parity.IsEven(integer));
		}

		[Theory]
		[InlineData(Byte.MinValue)]
		[InlineData(2)]
		public void Even_8bitUnsignedInteger(byte integer)
		{
			Assert.True(Parity.IsEven(integer));
			Assert.False(Parity.IsOdd(integer));
		}

		[Theory]
		[InlineData(1)]
		[InlineData(Byte.MaxValue)]
		public void Odd_8bitUnsignedInteger(byte integer)
		{
			Assert.True(Parity.IsOdd(integer));
			Assert.False(Parity.IsEven(integer));
		}

		[Theory]
		[InlineData(Int16.MinValue)]
		[InlineData(-2)]
		[InlineData(0)]
		[InlineData(+2)]
		public void Even_16bitSignedInteger(short integer)
		{
			Assert.True(Parity.IsEven(integer));
			Assert.False(Parity.IsOdd(integer));
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(+1)]
		[InlineData(Int16.MaxValue)]
		public void Odd_16bitSignedInteger(short integer)
		{
			Assert.True(Parity.IsOdd(integer));
			Assert.False(Parity.IsEven(integer));
		}

		[Theory]
		[InlineData(UInt16.MinValue)]
		[InlineData(2)]
		public void Even_16bitUnsignedInteger(ushort integer)
		{
			Assert.True(Parity.IsEven(integer));
			Assert.False(Parity.IsOdd(integer));
		}

		[Theory]
		[InlineData(1)]
		[InlineData(UInt16.MaxValue)]
		public void Odd_16bitUnsignedInteger(ushort integer)
		{
			Assert.True(Parity.IsOdd(integer));
			Assert.False(Parity.IsEven(integer));
		}

		[Theory]
		[InlineData(Int32.MinValue)]
		[InlineData(-2)]
		[InlineData(0)]
		[InlineData(+2)]
		public void IfAnIntegerIsEvenlyDivisibleByTwo_ItIsEven_LeavingNoRemainder(int integer)
		{
			Assert.True(Parity.IsEven(integer));
			Assert.False(Parity.IsOdd(integer));
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(1)]
		[InlineData(Int32.MaxValue)]
		public void IfAnIntegerIsNotEvenlyDivisibleByTwo_ItIsOdd_LeavingARemainderOfOne(int integer)
		{
			Assert.True(Parity.IsOdd(integer));
			Assert.False(Parity.IsEven(integer));
		}

		[Theory]
		[InlineData(UInt32.MinValue)]
		[InlineData(2u)]
		public void Even_32bitUnsignedInteger(uint integer)
		{
			Assert.True(Parity.IsEven(integer));
			Assert.False(Parity.IsOdd(integer));
		}

		[Theory]
		[InlineData(1u)]
		[InlineData(UInt32.MaxValue)]
		public void Odd_32bitUnsignedInteger(uint integer)
		{
			Assert.True(Parity.IsOdd(integer));
			Assert.False(Parity.IsEven(integer));
		}

		[Theory]
		[InlineData(Int64.MinValue)]
		[InlineData(-2L)]
		[InlineData(0L)]
		[InlineData(+2L)]
		public void Even_64bitSignedInteger(long integer)
		{
			Assert.True(Parity.IsEven(integer));
			Assert.False(Parity.IsOdd(integer));
		}

		[Theory]
		[InlineData(-1L)]
		[InlineData(+1L)]
		[InlineData(Int64.MaxValue)]
		public void Odd_64bitSignedInteger(long integer)
		{
			Assert.True(Parity.IsOdd(integer));
			Assert.False(Parity.IsEven(integer));
		}

		[Theory]
		[InlineData(UInt64.MinValue)]
		[InlineData(2ul)]
		public void Even_64bitUnsignedInteger(ulong integer)
		{
			Assert.True(Parity.IsEven(integer));
			Assert.False(Parity.IsOdd(integer));
		}

		[Theory]
		[InlineData(1ul)]
		[InlineData(UInt64.MaxValue)]
		public void Odd_64bitUnsignedInteger(ulong integer)
		{
			Assert.True(Parity.IsOdd(integer));
			Assert.False(Parity.IsEven(integer));
		}

		[Theory]
		[MemberData(nameof(NativeSizedSignedIntegerData), true)]
		public void Even_NativeSizedSignedInteger(nint integer)
		{
			Assert.True(Parity.IsEven(integer));
			Assert.False(Parity.IsOdd(integer));
		}

		[Theory]
		[MemberData(nameof(NativeSizedSignedIntegerData), false)]
		public void Odd_NativeSizedSignedInteger(nint integer)
		{
			Assert.True(Parity.IsOdd(integer));
			Assert.False(Parity.IsEven(integer));
		}

		[Theory]
		[MemberData(nameof(NativeSizedUnsignedIntegerData), true)]
		public void Even_NativeSizedUnsignedInteger(nuint integer)
		{
			Assert.True(Parity.IsEven(integer));
			Assert.False(Parity.IsOdd(integer));
		}

		[Theory]
		[MemberData(nameof(NativeSizedUnsignedIntegerData), false)]
		public void Odd_NativeSizedUnsignedInteger(nuint integer)
		{
			Assert.True(Parity.IsOdd(integer));
			Assert.False(Parity.IsEven(integer));
		}

		private static TheoryData<nint> NativeSizedSignedIntegerData(bool isEven)
		{
			TheoryData<nint> data = new();
			if (isEven)
			{
#if HAS_INTPTR_MINVALUE
				data.Add(IntPtr.MinValue);
#endif
				data.Add(Int32.MinValue);
				data.Add(-2);
				data.Add(0);
				data.Add(+2);
			}
			else
			{
				data.Add(-1);
				data.Add(+1);
				data.Add(Int32.MaxValue);
#if HAS_INTPTR_MAXVALUE
				data.Add(IntPtr.MaxValue);
#endif
			}
			return data;
		}

		private static TheoryData<nuint> NativeSizedUnsignedIntegerData(bool isEven)
		{
			TheoryData<nuint> data = new();
			if (isEven)
			{
#if HAS_UINTPTR_MINVALUE
				data.Add(UIntPtr.MinValue);
#endif
				data.Add(UInt32.MinValue);
				data.Add(2u);
			}
			else
			{
				data.Add(1u);
				data.Add(UInt32.MaxValue);
#if HAS_UINTPTR_MAXVALUE
				data.Add(UIntPtr.MaxValue);
#endif
			}
			return data;
		}
	}
}
