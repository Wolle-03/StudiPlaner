namespace StudiPlaner.Headless;

class App
{
    private static readonly App instance = new();
    public static App GetInstance() { return instance; }

    public static void Main() { GetInstance(); }

    private Screen screen;

    private App()
    {
        screen = new Screen();
        screen.SetFrame();
        screen.Print();
        Console.ReadKey();
    }
}