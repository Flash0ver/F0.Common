namespace F0.Tests.Shared
{
	internal class NestingLevel1
	{
		internal class NestingLevel2
		{
			internal class NestingLevel3
			{
			}
		}
	}

	internal class NestingLevel1<T1>
	{
		internal class NestingLevel2<T2>
		{
			internal class NestingLevel3<T3>
			{
			}
		}
	}

	internal class NestingLevel1<T1, T2>
	{
		internal class NestingLevel2
		{
			internal class NestingLevel3<T3>
			{
			}
		}
	}
}
