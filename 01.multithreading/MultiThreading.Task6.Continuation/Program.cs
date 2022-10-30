/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            #region a. Continuation task should be executed regardless of the result of the parent task.
            Task.Run(() => {
                Console.WriteLine("Hello from the parent task!");
            })
            .ContinueWith(parent => {
                Console.WriteLine($"Continuation task run when the parent task's status is {parent.Status}");
             })
            .Wait();
            #endregion

            #region b. Continuation task should be executed when the parent task finished without success.
            Task.Run(() => {
                Console.WriteLine("Throwing exception from parent task.");
                throw new Exception("Throwing exception from parent task.");
            })
            .ContinueWith(parent => {
                Console.WriteLine($"Continuation task run when the parent task's status is {parent.Status}");
            }, TaskContinuationOptions.NotOnRanToCompletion)
            .Wait();
            #endregion

            #region c. Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.
            Task.Run(() => {
                Console.WriteLine("Throwing exception from parent task.");
                throw new Exception("Throwing exception from parent task.");
            })
            .ContinueWith(parent => {
                Console.WriteLine($"Continuation task run when the parent task's status is {parent.Status}");
            }, TaskContinuationOptions.OnlyOnFaulted)
            .Wait();
            #endregion

            #region d. Continuation task should be executed outside of the thread pool when the parent task would be cancelled. 
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            var task = Task.Run(() => {
                if (token.IsCancellationRequested) {
                    token.ThrowIfCancellationRequested();
                }
            }, token);

            task.ContinueWith(parent =>
            {
                Console.WriteLine($"Continuation task run when the parent task's status is {parent.Status}");
            }, TaskContinuationOptions.OnlyOnCanceled);

            tokenSource.Cancel();

            #endregion

            Console.ReadLine();
        }
    }
}
