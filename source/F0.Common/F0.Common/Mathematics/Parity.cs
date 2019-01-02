namespace F0.Mathematics
{
	public static class Parity
	{
		public static bool IsEven(int integer)
		{
			return integer % 2 == 0;
		}

		public static bool IsOdd(int integer)
		{
			return integer % 2 != 0;
		}
	}
}
