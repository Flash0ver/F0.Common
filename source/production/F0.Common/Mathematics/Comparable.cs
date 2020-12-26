using System;

namespace F0.Mathematics
{
	public static class Comparable
	{
		public static T Clamp<T>(T value, T min, T max)
			where T : IComparable<T>
		{
			if (value is null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			if (min is null)
			{
				throw new ArgumentNullException(nameof(min));
			}

			if (max is null)
			{
				throw new ArgumentNullException(nameof(max));
			}

			if (min.CompareTo(max) > 0)
			{
				throw new ArgumentException($"'{min}' cannot be greater than {max}.");
			}

			if (value.CompareTo(min) < 0)
			{
				return min;
			}
			else if (value.CompareTo(max) > 0)
			{
				return max;
			}

			return value;
		}
	}
}
