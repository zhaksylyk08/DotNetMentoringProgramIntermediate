/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var rand = new Random();

            Task.Run(() =>
            {
                var numbersCount = 10;
                var numbers = new List<int>();

                for (int i = 0; i < numbersCount; i++)
                {
                    numbers.Add(rand.Next());
                }

                Console.WriteLine("Array of 10 random integers:");
                Print(numbers);

                return numbers;
            }).ContinueWith(antecedent =>
            {
                var randomNumber = rand.Next();
                var numbers = antecedent.Result.Select(number => number * randomNumber).ToList();

                Console.WriteLine("Multiplying array with random integer:");
                Print(numbers);

                return numbers;

            }).ContinueWith(antecedent =>
            {
                antecedent.Result.Sort();

                Console.WriteLine("Sorting array:");
                Print(antecedent.Result);

                return antecedent.Result;
            }).ContinueWith(antecedent =>
            {
                var average = antecedent.Result.Average();

                Console.WriteLine($"Average:{average}");
            });

            Console.ReadLine();
        }


        private static void Print(List<int> numbers)
        {
            numbers.ForEach(number => Console.WriteLine(number));
        }
    }
}
