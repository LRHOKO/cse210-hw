using System;
using System.Collections.Generic;
using System.IO;

// program currently meets all expectations, but does not exceed core requirements.
public abstract class Goal
{
    public string Name { get; set; }
    public int Points { get; set; }
    protected int currentProgress;

    public Goal(string name, int points)
    {
        Name = name;
        Points = points;
        currentProgress = 0;
    }

    public abstract void RecordEvent();
    public abstract void DisplayGoalStatus();

    public int GetTotalPoints()
    {
        return Points * currentProgress;
    }

    public void IncreaseProgress()
    {
        currentProgress++;
    }
}

public class SimpleGoal : Goal
{
    public SimpleGoal(string name, int points) : base(name, points) { }

    public override void RecordEvent()
    {
        IncreaseProgress();
        Console.WriteLine($"You earned {Points} points for completing the goal: {Name}!");
    }

    public override void DisplayGoalStatus()
    {
        Console.WriteLine($"[ ] {Name} - {Points} points");
    }
}

public class EternalGoal : Goal
{
    public EternalGoal(string name, int points) : base(name, points) { }

    public override void RecordEvent()
    {
        IncreaseProgress();
        Console.WriteLine($"You earned {Points} points for recording the eternal goal: {Name}!");
    }

    public override void DisplayGoalStatus()
    {
        Console.WriteLine($"[ ] {Name} (Eternal Goal) - {Points} points each time");
    }
}

public class ChecklistGoal : Goal
{
    public int Target { get; set; }
    public int BonusPoints { get; set; }

    public ChecklistGoal(string name, int points, int target, int bonusPoints) : base(name, points)
    {
        Target = target;
        BonusPoints = bonusPoints;
    }

    public override void RecordEvent()
    {
        IncreaseProgress();
        if (currentProgress == Target)
        {
            Console.WriteLine($"You earned {Points * Target + BonusPoints} points for completing the checklist goal: {Name}!");
        }
        else
        {
            Console.WriteLine($"You earned {Points} points for completing part of the checklist goal: {Name}!");
        }
    }

    public override void DisplayGoalStatus()
    {
        Console.WriteLine($"[ ] {Name} - Completed {currentProgress}/{Target} times. {Points} points each time");
    }
}

public class GoalManager
{
    private List<Goal> goals = new List<Goal>();
    private int totalScore = 0;

public void CreateGoal()
{
    Console.WriteLine("Enter goal type (simple, eternal, checklist): ");
    string type = Console.ReadLine()?.Trim().ToLower();

    Console.WriteLine("Enter goal name: ");
    string name = Console.ReadLine();

    int points;
    while (true)
    {
        Console.Write("Enter points for goal: ");
        if (int.TryParse(Console.ReadLine(), out points) && points > 0)
            break;
        Console.WriteLine("Invalid input. Please enter a positive number.");
    }

    if (type == "simple")
    {
        goals.Add(new SimpleGoal(name, points));
    }
    else if (type == "eternal")
    {
        goals.Add(new EternalGoal(name, points));
    }
    else if (type == "checklist")
    {
        int target, bonus;
        while (true)
        {
            Console.Write("Enter target count for the checklist: ");
            if (int.TryParse(Console.ReadLine(), out target) && target > 0)
                break;
            Console.WriteLine("Invalid input. Please enter a positive number.");
        }

        while (true)
        {
            Console.Write("Enter bonus points for completing the checklist: ");
            if (int.TryParse(Console.ReadLine(), out bonus) && bonus >= 0)
                break;
            Console.WriteLine("Invalid input. Please enter a non-negative number.");
        }

        goals.Add(new ChecklistGoal(name, points, target, bonus));
    }
    else
    {
        Console.WriteLine("Invalid goal type.");
    }
}

public void RecordGoal()
    {
        Console.WriteLine("Enter the index of the goal to record: ");
        int index = int.Parse(Console.ReadLine()) - 1;

        if (index >= 0 && index < goals.Count)
        {
            goals[index].RecordEvent();
            totalScore += goals[index].Points;
        }
        else
        {
            Console.WriteLine("Invalid goal index.");
        }
}

    public void DisplayGoals()
    {
        Console.WriteLine("List of Goals: ");
        for (int i = 0; i < goals.Count; i++)
        {
            Console.Write($"{i + 1}. ");
            goals[i].DisplayGoalStatus();
        }
    }

    public void ShowTotalScore()
    {
        Console.WriteLine($"Your total score is: {totalScore}");
    }

    public void SaveGoals()
    {
        using (StreamWriter writer = new StreamWriter("goals.txt"))
        {
            foreach (var goal in goals)
            {
                writer.WriteLine(goal.Name);
                writer.WriteLine(goal.Points);
            }
        }

        Console.WriteLine("Goals saved to file.");
    }

    public void LoadGoals()
    {
        if (File.Exists("goals.txt"))
        {
            using (StreamReader reader = new StreamReader("goals.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string name = line;
                    int points = int.Parse(reader.ReadLine());
                    goals.Add(new SimpleGoal(name, points));
                }
            }

            Console.WriteLine("Goals loaded from file.");
        }
        else
        {
            Console.WriteLine("No saved goals found.");
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        GoalManager manager = new GoalManager();
        manager.LoadGoals();

        while (true)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Create a new goal");
            Console.WriteLine("2. Record an event");
            Console.WriteLine("3. Display goals");
            Console.WriteLine("4. Show total score");
            Console.WriteLine("5. Save goals");
            Console.WriteLine("6. Exit");

            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    manager.CreateGoal();
                    break;
                case "2":
                    manager.RecordGoal();
                    break;
                case "3":
                    manager.DisplayGoals();
                    break;
                case "4":
                    manager.ShowTotalScore();
                    break;
                case "5":
                    manager.SaveGoals();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}
