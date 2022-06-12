using System;
using System.Collections.Generic;

public class Classroom
{
    public IEnumerable<string> Students { get; private set; }
    public Classroom(List<string> students)
    {
        Students = students;
    }




public static void Main(string[] args)
    {
        List<string> students = new List<string>() { "John", "Ana", "Carol" };
        Classroom classroom = new Classroom(students);

        foreach (string student in classroom.Students)
        {
            Console.WriteLine(student);
        }
    }
}