using System;
using System.Threading;
using System.Threading.Tasks;
using F0.Primitives;
using Xunit;

namespace F0.Tests.Primitives
{
	public class ImmediateProgressTests
	{
		[Fact]
		public void Callback_ToBeInvokedForEachReportedProgressValue_MustNotBeNull()
		{
			Assert.Throws<ArgumentNullException>("handler", () => new ImmediateProgress<bool>(null!));
		}

		[Fact]
		public void EachReportedProgressValueInvokesTheAction()
		{
			int value = 0;
			IProgress<int> progress = new ImmediateProgress<int>(v => value = v);

			Assert.Equal(0, value);
			progress.Report(240);
			Assert.Equal(240, value);
			progress.Report(-240);
			Assert.Equal(-240, value);
		}

		[Fact]
		public async Task CallbacksAreInvokedSynchronously_NeitherThroughSynchronizationContextNorOnTheThreadPool()
		{
			Thread? thread = null;
			IProgress<object?> progress = new ImmediateProgress<object?>(value => thread = Thread.CurrentThread);

			Assert.Null(thread);
			progress.Report(null);
			Assert.NotNull(thread);
			Assert.Same(Thread.CurrentThread, thread);

			TaskCompletionSource<int> tcs = new();
			progress = new Progress<object?>(value => tcs.SetResult(Thread.CurrentThread.ManagedThreadId));

			progress.Report(null);
			int threadId = await tcs.Task.ConfigureAwait(false);
			Assert.NotEqual(Thread.CurrentThread.ManagedThreadId, threadId);
		}
	}
}
