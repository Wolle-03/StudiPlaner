using StudiPlaner.Core;
using StudiPlaner.Core.Data;

namespace StudiPlaner.Headless;

abstract class App
{
    public static void Main() { Start(); }

    private static readonly Screen screen = new();
    public static bool LoggedIn { get; private set; } = false;
    private static Profile? Profile;

    private static void Start()
    {
        screen.Frame().Login().Date().Print();
        Console.ReadKey();
    }

    public static async Task<bool> Login(string name, string password)
    {
        Profile = await SaveFile.AsyncLoad(name, password);
        return LoggedIn = Profile != null;
    }
}