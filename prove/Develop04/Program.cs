using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    public class Activity
    {
        protected string _name;
        protected string _description;
        protected int _duration;

        public Activity(string name, string description)
        {
            _name = name;
            _description = description;
        }

        public void SetDuration()
        {
            Console.Write("Enter the duration of the activity in seconds: ");
            _duration = int.Parse(Console.ReadLine());
        }

        public void ShowStartMessage()
        {
            Console.WriteLine($"\nWelcome to the {_name} activity!");
            Console.WriteLine($"\n{_description}");
            SetDuration();
            Console.WriteLine("Get ready to begin!");
            PauseForSeconds(3);
        }

        public void ShowEndMessage()
        {
            Console.WriteLine($"\nWell done! You have completed the {_name} activity.");
            Console.WriteLine($"Duration: {_duration} seconds.");
            PauseForSeconds(3);
        }

        public void PauseForSeconds(int seconds)
        {
            for (int i = 0; i < seconds; i++)
            {
                Console.Write(".");
                Thread.Sleep(1000);
            }
            Console.WriteLine();
        }

        public virtual void ActivityProcess()
        {
           
        }

        public void Start()
        {
            ShowStartMessage();
            ActivityProcess();
            ShowEndMessage();
        }
    }

    // Breathing Activity subclass
    public class BreathingActivity : Activity
    {
        public BreathingActivity() : base("Breathing", "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.") { }

        public override void ActivityProcess()
        {
            DateTime endTime = DateTime.Now.AddSeconds(_duration);
            while (DateTime.Now < endTime)
            {
                Console.WriteLine("Breathe in...");
                PauseForSeconds(4);
                Console.WriteLine("Breathe out...");
                PauseForSeconds(4);
            }
        }
    }

    // Reflection Activity subclass
    public class ReflectionActivity : Activity
    {
        public ReflectionActivity() : base("Reflection", "This activity will help you reflect on times in your life when you have shown strength and resilience.") { }

        public override void ActivityProcess()
        {
            List<string> prompts = new List<string>
            {
                "Think of a time when you stood up for someone else.",
                "Think of a time when you did something really difficult.",
                "Think of a time when you helped someone in need.",
                "Think of a time when you did something truly selfless."
            };

            List<string> questions = new List<string>
            {
                "Why was this experience meaningful to you?",
                "Have you ever done anything like this before?",
                "How did you get started?",
                "How did you feel when it was complete?",
                "What made this time different than other times when you were not as successful?",
                "What is your favorite thing about this experience?",
                "What could you learn from this experience that applies to other situations?",
                "What did you learn about yourself through this experience?",
                "How can you keep this experience in mind in the future?"
            };

            Random rand = new Random();
            string prompt = prompts[rand.Next(prompts.Count)];
            Console.WriteLine($"\n{prompt}");
            PauseForSeconds(5);

            DateTime endTime = DateTime.Now.AddSeconds(_duration);
            while (DateTime.Now < endTime)
            {
                string question = questions[rand.Next(questions.Count)];
                Console.WriteLine(question);
                PauseForSeconds(5);
            }
        }
    }

    // Listing Activity subclass
    public class ListingActivity : Activity
    {
        public ListingActivity() : base("Listing", "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.") { }

        public override void ActivityProcess()
        {
            List<string> prompts = new List<string>
            {
                "Who are people that you appreciate?",
                "What are personal strengths of yours?",
                "Who are people that you have helped this week?",
                "When have you felt the Holy Ghost this month?",
                "Who are some of your personal heroes?"
            };

            Random rand = new Random();
            string prompt = prompts[rand.Next(prompts.Count)];
            Console.WriteLine($"\n{prompt}");
            PauseForSeconds(3);

            Console.WriteLine("Start listing your items...");
            List<string> items = new List<string>();
            DateTime startTime = DateTime.Now;

            while ((DateTime.Now - startTime).TotalSeconds < _duration)
            {
                Console.Write("Item: ");
                string item = Console.ReadLine();
                items.Add(item);
            }

            Console.WriteLine($"\nYou listed {items.Count} items!");
            PauseForSeconds(2);
        }
    }

    // Visualization Activity subclass
    public class VisualizationActivity : Activity
    {
        public VisualizationActivity() : base("Visualization", "This activity will guide you through a mental imagery exercise to help you relax and focus.") { }

        public override void ActivityProcess()
        {
            List<string> visualizations = new List<string>
            {
                "Imagine yourself walking along a peaceful beach, feeling the warm sand beneath your feet.",
                "Picture yourself in a quiet forest, surrounded by tall trees with sunlight filtering through the leaves.",
                "Visualize sitting on top of a mountain, taking in the fresh air and enjoying the breathtaking view of the landscape.",
                "Imagine floating on a calm lake, staring at the clear sky above."
            };

            Random rand = new Random();
            string visualization = visualizations[rand.Next(visualizations.Count)];
            Console.WriteLine($"\n{visualization}");
            PauseForSeconds(3);

            Console.WriteLine("Focus on this scene and imagine the details...");
            PauseForSeconds(_duration);
        }
    }


    // Menu System
    public static void DisplayMenu()
    {
        Console.WriteLine("\nMenu:");
        Console.WriteLine("1. Start Breathing Activity");
        Console.WriteLine("2. Start Reflection Activity");
        Console.WriteLine("3. Start Listing Activity");
        Console.WriteLine("4. Start Visualization Activity");
        Console.WriteLine("5. Quit");
    }

    // Main program loop
    static void Main(string[] args)
    {
        while (true)
        {
            DisplayMenu();
            Console.Write("Select an activity (1-5): ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Activity activity = new BreathingActivity();
                activity.Start();
            }
            else if (choice == "2")
            {
                Activity activity = new ReflectionActivity();
                activity.Start();
            }
            else if (choice == "3")
            {
                Activity activity = new ListingActivity();
                activity.Start();
            }

            else if (choice == "4")
            {
                Activity activity = new VisualizationActivity();
                activity.Start();
            }

            else if (choice == "5")
            {
                Console.WriteLine("Goodbye!");
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice, try again.");
            }
        }
    }
}
