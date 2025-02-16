using System;
using System.Collections.Generic;
using System.Linq;

// To exceed requirements, I added a system where it chooses a random scripture between the 3 options written below.
class Program
{
    static void Main(string[] args)
    {
        Random rand = new Random();

        List<Scripture> scriptures = new List<Scripture>
        {
            new Scripture(new Reference("Proverbs", 3, 5, 6), "Trust in the Lord with all thine heart; and lean not unto thine own understanding. In all thy ways acknowledge him, and he shall direct thy paths."),
            new Scripture(new Reference("Philippians", 4, 13), "I can do all things through Christ which strengtheneth me."),
            new Scripture(new Reference("Isaiah", 41, 10), "Fear thou not; for I am with thee: be not dismayed; for I am thy God: I will strengthen thee; yea, I will help thee; yea, I will uphold thee with the right hand of my righteousness."),
        };

        Scripture scripture = scriptures[rand.Next(scriptures.Count)];
        while (true)
        {
            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine("\nPress Enter to continue or type 'quit' to finish:");

            string input = Console.ReadLine()?.Trim();
            if (input.ToLower() == "quit")
                break;

            scripture.HideRandomWords(rand, 2);
            
            if (scripture.AllWordsHidden())
            {
                Console.Clear();
                Console.WriteLine(scripture.GetDisplayText());
                Console.WriteLine("\nAll words are hidden. Press Enter to exit.");
                Console.ReadLine();
                break;
            }
        }
    }
}

class Scripture
{
    private Reference _reference;
    private List<Word> _words;

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = text.Split(' ').Select(w => new Word(w)).ToList();
    }

    public string GetDisplayText()
    {
        return $"{_reference.GetDisplayText()}\n{string.Join(" ", _words.Select(w => w.GetDisplayText()))}";
    }

    public void HideRandomWords(Random rand, int count)
    {
        var visibleWords = _words.Where(w => !w.IsHidden).ToArray(); 

        for (int i = 0; i < count && visibleWords.Length > 0; i++)
        {
            int index = rand.Next(visibleWords.Length);
            visibleWords[index].Hide();
        }
    }

    public bool AllWordsHidden()
    {
        return _words.All(w => w.IsHidden);
    }
}

class Reference
{
    private string _book;
    private int _chapter;
    private int _startVerse;
    private int? _endVerse;

    public Reference(string book, int chapter, int startVerse, int? endVerse = null)
    {
        _book = book;
        _chapter = chapter;
        _startVerse = startVerse;
        _endVerse = endVerse;
    }

    public string GetDisplayText()
    {
        return _endVerse.HasValue 
            ? $"{_book} {_chapter}:{_startVerse}-{_endVerse.Value}" 
            : $"{_book} {_chapter}:{_startVerse}";
    }
}

class Word
{
    private string _text;
    public bool IsHidden { get; private set; }

    public Word(string text)
    {
        _text = text;
        IsHidden = false;
    }

    public void Hide()
    {
        IsHidden = true;
    }

    public string GetDisplayText()
    {
        return IsHidden ? new string('_', _text.Length) : _text;
    }
}
