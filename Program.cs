using System;
using System.Collections.Generic;
using System.IO;

class ExpenseTrackerApp
{
    private static List<Entry> Entries = new List<Entry>();
    private const string FileName = "expenses.txt";

    static void Main()
    {
        LoadEntries();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Rastreador de Despesas ===");
            Console.WriteLine("1. Adicionar valores de entrada");
            Console.WriteLine("2. Visualizar todas as entradas");
            Console.WriteLine("3. Mostrar saldo atual");
            Console.WriteLine("4. Sair");
            Console.Write("Escolha uma opção: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddEntry();
                    break;
                case "2":
                    ViewEntries();
                    break;
                case "3":
                    ShowBalance();
                    break;
                case "4":
                    SaveEntries();
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    static void LoadEntries()
    {
        if (!File.Exists(FileName)) return;

        foreach (string line in File.ReadAllLines(FileName))
        {
            string[] parts = line.Split('|');
            if (parts.Length == 4 && double.TryParse(parts[2], out double amount))
            {
                Entries.Add(new Entry
                {
                    Type = parts[0],
                    Category = parts[1],
                    Amount = amount,
                    Note = parts[3]
                });
            }
        }
    }

    static void SaveEntries()
    {
        List<string> lines = new List<string>();
        foreach (var entry in Entries)
        {
            lines.Add($"{entry.Type}|{entry.Category}|{entry.Amount}|{entry.Note}");
        }
        File.WriteAllLines(FileName, lines);
    }

    static void AddEntry()
    {
        Console.Write("Type (income/expense): ");
        string type = Console.ReadLine().ToLower();

        if (type != "income" && type != "expense")
        {
            Console.WriteLine("Invalid type.");
            return;
        }

        Console.Write("Category (e.g., Food, Salary): ");
        string category = Console.ReadLine();

        Console.Write("Amount: ");
        if (!double.TryParse(Console.ReadLine(), out double amount))
        {
            Console.WriteLine("Invalid amount.");
            return;
        }

        Console.Write("Note (optional): ");
        string note = Console.ReadLine();

        Entries.Add(new Entry
        {
            Type = type,
            Category = category,
            Amount = amount,
            Note = note
        });

        Console.WriteLine("Entry added!");
    }

    static void ViewEntries()
    {
        Console.WriteLine("\nAll Entries:");
        foreach (var e in Entries)
        {
            Console.WriteLine($"{e.Type} | {e.Category} | {e.Amount:C} | {e.Note}");
        }
    }

    static void ShowBalance()
    {
        double income = 0, expenses = 0;
        foreach (var e in Entries)
        {
            if (e.Type == "income") income += e.Amount;
            else expenses += e.Amount;
        }

        Console.WriteLine($"\nTotal Income: {income:C}");
        Console.WriteLine($"Total Expenses: {expenses:C}");
        Console.WriteLine($"Balance: {(income - expenses):C}");
    }
}

class Entry
{
    public string Type { get; set; }
    public string Category { get; set; }
    public double Amount { get; set; }
    public string Note { get; set; }
}
