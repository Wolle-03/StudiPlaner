namespace StudiPlaner.Headless;

public class Screen(int Width = 120, int Height = 30)
{
    private int Width { get; set; } = Width;
    private int Height { get; set; } = Height;
    private readonly Char[] buffer = Enumerable.Repeat<Char>(' ', Width * Height).ToArray();

    private List<Thread> threads = [];

    public Screen Init() { Array.Fill(buffer, ' '); return this; }

    public void Clear() { Init().Frame().Print(); }

    public Screen Frame()
    {
        buffer[0] = buffer[Width - 1] = buffer[Width * (Height - 1)] = buffer[Width * Height - 1] = '+';
        for (int i = 1; i < Width - 1; i++)
        {
            buffer[i] = '-';
            buffer[i + Width * (Height - 1)] = '-';
        }
        for (int i = 1; i < Height - 1; i++)
        {
            buffer[i * Width] = '|';
            buffer[(i + 1) * Width - 1] = '|';
        }
        return this;
    }

    public void Print()
    {
        Console.Clear();
        bool flag = !(bool)buffer[0];
        for (int i = 0; i < buffer.Length; i++)
        {
            if (flag != (flag = (bool)buffer[i]))
                Console.ForegroundColor = flag ? ConsoleColor.White : ConsoleColor.DarkGray;
            Console.Write(buffer[i]);
            if ((i + 1) % Width == 0 && i + 1 != Height * Width)
                Console.Write("\n");
        }
    }

    public Screen Date()
    {
        Run(buffer =>
        {
            string date = DateTime.Now.ToString();
            for (int i = 0; i < date.Length; i++)
                buffer[Width + 1] = date[i];
            Console.SetCursorPosition(1, 1);
            Console.Write(date);
            Console.SetCursorPosition(Width - 1, Height - 1);
        }, 1000);
        return TextField(DateTime.Now.ToString(), 0, 0);
    }

    public delegate void Runnable(Char[] buffer);

    public Screen Run(Runnable r, int interval)
    {
        Thread t = new(new ThreadStart(() =>
        {
            while (true)
            {
                Thread.Sleep(interval);
                r(buffer);
            }
        }));
        t.Start();
        threads.Add(t);
        return this;
    }

    public Screen TextField(string text, int x, int y, bool enabled = true)
    {
        int column = x >= 0 ? x : (Width - text.Length + 2) / 2;
        int row = y >= 0 ? y : (Height - 2) / 2;
        int width = text.Length + 1;
        buffer[Width * row + column] = buffer[Width * row + column + width] =
        buffer[Width * (row + 2) + column] = buffer[Width * (row + 2) + column + width] = ('+', enabled);
        buffer[Width * (row + 1) + column] =
         buffer[Width * (row + 1) + width + column] = ('|', enabled);
        for (int i = 1; i <= text.Length; i++)
        {
            buffer[Width * (row + 1) + column + i] = (text[i - 1], enabled);
            buffer[Width * row + column + i] = buffer[Width * (row + 2) + column + i] = ('-', enabled);
        }
        return this;
    }

    public Screen Button(char key, string text, int column, int row, bool enabled = true)
    {
        TextField($" {key} | {text}", column, row, enabled);
        buffer[row * Width + column + 4] = buffer[(row + 2) * Width + column + 4] = ('+', enabled);
        return this;
    }

    public Screen LoginOutButton()
    {
        string text = App.LoggedIn ? "Logout " : "Login ";
        return Button('L', text, Width - text.Length - 7, 0);
    }

    public readonly struct Char(char c, bool enabled = true)
    {
        char C { get; init; } = c;
        bool Enabled { get; init; } = enabled;

        public static implicit operator Char(char c) => new(c);

        public static implicit operator Char((char c, bool b) t) => new(t.c, t.b);

        public static implicit operator char(Char c) => c.C;
        public static explicit operator bool(Char c) => c.Enabled;

        public static implicit operator (char, bool)(Char c) => (c.C, c.Enabled);
    }
}