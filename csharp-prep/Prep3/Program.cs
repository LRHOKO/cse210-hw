using System;

class Program
{
    static void Main(string[] args)
    {
    

        Random randomGenerator = new Random();
        int number = randomGenerator.Next(1, 101);

        int guess_Number = 0;

        while (guess_Number != number)
        {
            Console.WriteLine("What is the magic number?");
            string guess = Console.ReadLine();
            guess_Number = int.Parse(guess);

            if (number > guess_Number)
            {
                Console.WriteLine("Higher");
            }
            else if (number < guess_Number)
            {
                Console.WriteLine("Lower");
            }
            else {
                Console.WriteLine("That's correct!");
            }
        }


    }
}