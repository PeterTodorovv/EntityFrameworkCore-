using System;
using System.Linq;

namespace test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] array = new int[]
            {
                1, 2 ,3, 4, 4 , 4, 1
            };

            Console.WriteLine(array.Min() + array.Max());
        }
    }
}
