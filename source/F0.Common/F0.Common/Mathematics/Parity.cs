namespace F0.Mathematics
{
	public static class Parity
	{
		private const int two = 2;

		public static bool IsEven(int integer)
		{
			return integer % two == 0;
		}

		public static bool IsOdd(int integer)
		{
			return integer % two != 0;
		}
	}
}
