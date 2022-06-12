using System;
using System.Collections.Generic;
using System.Linq;

public class UniqueNumbers
{
    public static IEnumerable<int> FindUniqueNumbers(IEnumerable<int> numbers)
    {
        return numbers.GroupBy(n => n).Where(n => n.Count() == 1).Select(n => n.Key).ToList();
    }

    public static void Main(string[] args)
    {
        int[] numbers = new int[] { 1, 2, 1, 3 };
        foreach (var number in FindUniqueNumbers(numbers))
            Console.WriteLine(number);
    }
}