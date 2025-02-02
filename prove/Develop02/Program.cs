using System;
using System.Collections.Generic;
using System.IO;

class JournalEntry
{
    public string Date { get; set; }
    public string Prompt { get; set; }
    public string Response { get; set; }
    public int MoodRating { get; set; }

    public JournalEntry(string date, string prompt, string response, int moodRating)
    {
        Date = date;
        Prompt = prompt;
        Response = response;
        MoodRating = moodRating;
    }
}

class Journal
{
    private List<JournalEntry> entries = new List<JournalEntry>();
    private List<string> prompts = new List<string>
    {
        "Who was the most interesting person I interacted with today?",
        "What was the best part of my day?",
        "What was the strongest emotion I felt today?",
        "If I had one thing I could do over today, what would it be?",
        "What is something new I learned today?",
        "What would I like to do tommorow?",
        "What attribute do I appreciate most about myself?"
    };

    public void AddEntry()
    {
        Random random = new Random();
        string prompt = prompts[random.Next(prompts.Count)];
        Console.WriteLine("Prompt: " + prompt);
        Console.Write("Your response: ");
        string response = Console.ReadLine();
        Console.Write("Mood rating (1-10): ");
        int moodRating;
        while (!int.TryParse(Console.ReadLine(), out moodRating) || moodRating < 1 || moodRating > 10)
        {
            Console.Write("Invalid input. Enter a mood rating between 1-10: ");
        }
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        entries.Add(new JournalEntry(date, prompt, response, moodRating));
        Console.WriteLine("Entry saved\n");
    }

    public void DisplayEntries()
    {
        if (entries.Count == 0)
        {
            Console.WriteLine("No journal entries found.\n");
            return;
        }
        
        foreach (var entry in entries)
        {
            Console.WriteLine($"Date: {entry.Date}\nPrompt: {entry.Prompt}\nResponse: {entry.Response}\nMood Rating: {entry.MoodRating}/10\n");
        }
    }
    public void SaveJournal()
    {
        Console.Write("Enter filename to save journal: ");
        string filename = Console.ReadLine();
        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (var entry in entries)
            {
                writer.WriteLine($"{entry.Date}|{entry.Prompt}|{entry.Response}|{entry.MoodRating}");
            }
        }
        Console.WriteLine("Journal saved\n");
    }

    public void LoadJournal()
    {
        Console.Write("Enter filename to load journal: ");
        string filename = Console.ReadLine();
        if (File.Exists(filename))
        {
            entries.Clear();
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 4 && int.TryParse(parts[3], out int moodRating))
                    {
                        entries.Add(new JournalEntry(parts[0], parts[1], parts[2], moodRating));
                    }
                }
            }
            Console.WriteLine("Journal loaded\n");
        }
        else
        {
            Console.WriteLine("File not found.\n");
        }
    }
}
class Program
{
    static void Main()
    {
        Journal journal = new Journal();
        while (true)
        {
            Console.WriteLine("Journal Menu:");
            Console.WriteLine("1. Write new entry");
            Console.WriteLine("2. Display journal");
            Console.WriteLine("3. Save journal to a file");
            Console.WriteLine("4. Load journal from a file");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
            
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    journal.AddEntry();
                    break;
                case "2":
                    journal.DisplayEntries();
                    break;
                case "3":
                    journal.SaveJournal();
                    break;
                case "4":
                    journal.LoadJournal();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.\n");
                    break;
            }
        }
    }
}
