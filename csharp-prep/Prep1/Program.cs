using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("What is your first name? ");
        string first_Name = Console.ReadLine();

        Console.Write("What is your last name? ");
        string last_Name = Console.ReadLine();

        Console.WriteLine($"Your name is {last_Name}, {first_Name} {last_Name}.");
    }
}