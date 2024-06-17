using Core.Data;
using StudiPlaner.Core;
using StudiPlaner.Core.Data;
using System;
using System.ComponentModel.Design;
using System.IO;
using System.Xml.Linq;

namespace StudiPlaner.Headless;

abstract class App
{
    public static void Main() { Start(); }

    private static readonly Screen screen = new();
    public static bool LoggedIn { get; private set; } = false;
    private static Profile? Profile;
    private static string? username, password;

    private static void Start()
    {
        // screen.Frame().LoginOutButton().Date().TextField(" Please log in ",-1,-1,false).Print();
        do
        {
            Console.WriteLine("\nPlease log in:");
            Console.Write($"Username: {Environment.UserName}/");
            username = Console.ReadLine() ?? "";
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StudiPlaner", username + ".profile")))
            {
                Console.Write("Password: ");
                password = Console.ReadLine() ?? "";
                if (Login(username, password))
                    Console.WriteLine($"Successfully logged in as {username}");
                else
                    Console.WriteLine($"Password incorrect");
            }
            else
            {
                Console.Write($"Create a new user {username} (Y/n): ");
                if (Console.ReadKey().Key != ConsoleKey.N)
                {
                    Console.Write("\nPassword: ");
                    password = Console.ReadLine() ?? "";
                    Console.Write("Confirm Password: ");
                    if (password == Console.ReadLine())
                    {
                        LoggedIn = true;
                        SaveFile.Save(Profile = new Profile(username), username, password);
                        Console.WriteLine($"Successfully logged in as {username}");
                    }
                    else
                        Console.WriteLine($"Passwords do not match");
                }
                else
                    Console.WriteLine();
            }
        } while (!LoggedIn);
        do
        {
            Menu();
            Console.Clear();
            Console.Write("Are You sure you want to exit? (y/N): ");
        } while (Console.ReadKey().Key != ConsoleKey.Y);
        Console.WriteLine();
        Save();
    }

    static void Menu()
    {
        ConsoleKey key;
        do
        {
            Console.Clear();
            Console.WriteLine("Menu");
            Console.WriteLine("C - Calendar");
            Console.WriteLine("T - TimeTable");
            Console.WriteLine("G - Grades");
            Console.WriteLine("E - Exit");
            key = Console.ReadKey().Key;
            Console.Clear();
            switch (key)
            {
                case ConsoleKey.C:
                    Calendar();
                    break;
                case ConsoleKey.T:
                    TimeTable();
                    break;
                case ConsoleKey.G:
                    Grades();
                    break;
            }
        } while (key != ConsoleKey.E);
    }

    static void Calendar()
    {
        ConsoleKey key;
        do
        {
            Console.Clear();
            Console.WriteLine("Calendar");
            Console.WriteLine("N - New Entry");
            Console.WriteLine("D - Delete Entry");
            Console.WriteLine("E - Exit\n");
            Console.WriteLine(Profile!.Calendar);
            key = Console.ReadKey().Key;
            Console.Clear();
            switch (key)
            {
                case ConsoleKey.N:
                    string name;
                    DateTime start;
                    string? description;
                    Console.WriteLine("New Appointment");
                    do
                        Console.Write("Please enter Name: ");
                    while ((name = Console.ReadLine() ?? "") == "");
                    do
                        Console.Write("Please enter Date: ");
                    while (!DateTime.TryParse(Console.ReadLine(), out start));
                    Console.Write("Please enter Description (optional): ");
                    description = Console.ReadLine();
                    Profile!.Calendar.Appointments.Add(new(name, start, description));
                    Save();
                    break;
                case ConsoleKey.D:
                    Console.WriteLine(Profile!.Calendar);
                    Console.Write("Please enter ID (-1 to cancel): ");
                    int index;
                    while (!int.TryParse(Console.ReadLine(), out index)) ;
                    if (index >= 0 && index < Profile!.Calendar.Appointments.Count)
                        Profile!.Calendar.Appointments.RemoveAt(index);
                    Save();
                    break;
            }
        } while (key != ConsoleKey.E);
    }

    static void TimeTable()
    {
        ConsoleKey key;
        do
        {
            Console.Clear();
            Console.WriteLine("TimeTable");
            Console.WriteLine("N - New Course");
            Console.WriteLine("D - Delete Course");
            Console.WriteLine("F - Finish Course");
            Console.WriteLine("E - Exit\n");
            Console.WriteLine(Profile!.TimeTable);
            key = Console.ReadKey().Key;
            Console.Clear();
            int index;
            switch (key)
            {
                case ConsoleKey.N:
                    string name;
                    int semester, day, time;
                    List<TimeSlot> timeSlots = [];
                    Console.WriteLine("New Course");
                    do
                        Console.Write("Please enter Name: ");
                    while ((name = Console.ReadLine() ?? "") == "");
                    do
                        Console.Write("Please enter semester: ");
                    while (!int.TryParse(Console.ReadLine(), out semester));
                    do
                    {
                        do
                            Console.Write("\nPlease enter day of week (1 - 7): ");
                        while (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out day));
                        do
                        {
                            Console.WriteLine("\n1:  8:00 -  9:30");
                            Console.WriteLine("2:  9:45 - 11:15");
                            Console.WriteLine("3: 11:30 - 13:00");
                            Console.WriteLine("4: 14:00 - 15:30");
                            Console.WriteLine("5: 15:45 - 17:15");
                            Console.WriteLine("6: 17:30 - 19:00");
                            Console.WriteLine("7: 19:15 - 20:45");
                            Console.Write("Please timeslot (1-7): ");
                        } while (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out time));
                        timeSlots.Add(new((Day)day, (Time)time));
                        Console.Write("\nAnother Timeslot (y/N): ");
                    } while (Console.ReadKey().Key == ConsoleKey.Y);
                    try
                    {
                        Profile!.TimeTable.Add(new(name, semester, timeSlots));
                        Save();
                    }
                    catch (TimeSlotBlockedException e)
                    {
                        Console.Clear();
                        Console.WriteLine($"{e}\n\nPlease press a key...");
                        Console.ReadKey();
                    }
                    break;
                case ConsoleKey.D:
                    Console.WriteLine(Profile!.TimeTable);
                    Console.Write("Please enter ID (-1 to cancel): ");
                    while (!int.TryParse(Console.ReadLine(), out index)) ;
                    if (index >= 0 && index < Profile!.TimeTable.Count())
                        Profile!.TimeTable.Remove(index);
                    Save();
                    break;
                case ConsoleKey.F:
                    Console.WriteLine(Profile!.TimeTable.PrintCourses());
                    Console.Write("Please enter ID (-1 to cancel): ");
                    while (!int.TryParse(Console.ReadLine(), out index)) ;
                    double grade;
                    Console.Write("Please enter grade: ");
                    while (!double.TryParse(Console.ReadLine(), out grade)) ;
                    if (index >= 0 && index < Profile!.TimeTable.Count())
                        Profile!.FinishedCourses.Add(Profile!.TimeTable.Finish(index, grade));
                    Save();
                    break;
            }
        } while (key != ConsoleKey.E);
    }

    static void Grades()
    {
        ConsoleKey key;
        do
        {
            Console.Clear();
            Console.WriteLine("Grades");
            Console.WriteLine("N - New Grade");
            Console.WriteLine("D - Delete Grade");
            Console.WriteLine("E - Exit\n");
            Console.WriteLine(Profile!.Grades());
            key = Console.ReadKey().Key;
            Console.Clear();
            switch (key)
            {
                case ConsoleKey.N:
                    string name;
                    int semester;
                    double grade;
                    Console.WriteLine("New Grade");
                    do
                        Console.Write("Please enter name: ");
                    while ((name = Console.ReadLine() ?? "") == "");
                    do
                        Console.Write("Please enter semester: ");
                    while (!int.TryParse(Console.ReadLine(), out semester));
                    do
                        Console.Write("Please enter grade: ");
                    while (!double.TryParse(Console.ReadLine(), out grade));
                    Profile!.FinishedCourses.Add(new(name, semester, grade));
                    Save();
                    break;
                case ConsoleKey.D:
                    int index;
                    Console.WriteLine(Profile!.GradesIDs());
                    Console.Write("\nPlease enter ID (-1 to cancel): ");
                    while (!int.TryParse(Console.ReadLine(), out index)) ;
                    if (index >= 0 && index < Profile!.FinishedCourses.Count)
                        Profile!.FinishedCourses.RemoveAt(index);
                    Save();
                    break;
            }
        } while (key != ConsoleKey.E);
    }

    static bool Login(string name, string password)
    {
        Profile = SaveFile.Load<Profile>(name, password);
        return LoggedIn = Profile != null;
    }

    static void Save()
    {
        SaveFile.Save(Profile!, username!, password!);
    }
}