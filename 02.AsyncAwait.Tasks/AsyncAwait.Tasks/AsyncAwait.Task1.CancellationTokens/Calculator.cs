using System.Threading;

namespace AsyncAwait.Task1.CancellationTokens;

internal static class Calculator
{
    public static long Calculate(int n, CancellationToken token)
    {
        if (token.IsCancellationRequested) { 
            token.ThrowIfCancellationRequested();
        }

        long sum = 0;

        for (var i = 0; i < n; i++)
        {
            // i + 1 is to allow 2147483647 (Max(Int32)) 
            sum = sum + (i + 1);
            Thread.Sleep(10);

            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }
        }

        return sum;
    }
}
