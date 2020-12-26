namespace F0.Mathematics
{
	public static class Parity
	{
		public static bool IsEven(int integer)
		{
			int remainder = integer % 2;
			return remainder == 0;
		}

		public static bool IsEven(uint integer)
		{
			uint remainder = integer % 2u;
			return remainder == 0u;
		}

		public static bool IsEven(long integer)
		{
			long remainder = integer % 2L;
			return remainder == 0L;
		}

		public static bool IsEven(ulong integer)
		{
			ulong remainder = integer % 2ul;
			return remainder == 0ul;
		}

		public static bool IsOdd(int integer)
		{
			int remainder = integer % 2;
			return remainder != 0;
		}

		public static bool IsOdd(uint integer)
		{
			uint remainder = integer % 2u;
			return remainder == 1u;
		}

		public static bool IsOdd(long integer)
		{
			long remainder = integer % 2L;
			return remainder != 0L;
		}

		public static bool IsOdd(ulong integer)
		{
			ulong remainder = integer % 2ul;
			return remainder == 1ul;
		}
	}
}
