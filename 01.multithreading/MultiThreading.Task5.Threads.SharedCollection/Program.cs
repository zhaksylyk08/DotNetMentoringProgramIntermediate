/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static List<int> numbers; 
        private static readonly object numbersLock = new object();

        static void Main()
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            numbers = new List<int>();

            //ThreadPool.QueueUserWorkItem(AddNumbers);
            //ThreadPool.QueueUserWorkItem(Print);

            new Thread(AddNumbers).Start();

            Console.ReadLine();
        }

        private static void AddNumbers(object state) {
            for (int i = 0; i < 10; i++) {
                lock (numbersLock)
                {
                    numbers.Add(i + 1);
                    Console.WriteLine($"{i + 1} added");
                }

                new Thread(Print).Start();

            }
        }

        private static void Print(object state)
        {
            lock (numbersLock) {
                foreach (var item in numbers)
                {
                    Console.Write($"{item} ");
                }

                Console.WriteLine();
            }
        }
    }
}
