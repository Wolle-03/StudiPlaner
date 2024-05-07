namespace StudiPlaner.Headless;
class Screen(TextReader input, TextWriter output, int Width = 120, int Height = 30)
{
    private TextReader Input { get; set; } = input;
    private TextWriter Output { get; set; } = output;
    private int Width { get; set; } = Width;
    private int Height { get; set; } = Height;
    private readonly char[] buffer = Enumerable.Repeat(' ',Width*Height).ToArray();

    public Screen(Stream stream, int Width = 120, int Height = 30) : this(new StreamReader(stream), new StreamWriter(stream), Width, Height)
    {
        if (!stream.CanRead || !stream.CanWrite) throw new Exception("Stream cannot read and write");
    }
    public Screen(int Width = 120, int Height = 30) : this(Console.In, Console.Out, Width, Height) { }

    public void Init()
    {
        for (int i = 0; i < buffer.Length; i++)
            buffer[i] = ' ';
    }
    public void Clear()
    {
        Init();
        Print();
    }

    public void Print()
    { 
        for (int i = 0; i < buffer.Length; i++)
        {
            Output.Write(buffer[i]);
            if ((i + 1) % Width == 0 && i + 1 != Height * Width)
                Output.Write("\n");
        }
    }

    public void SetFrame()
    {
        buffer[0] = '+';
        buffer[Width - 1] = '+';
        buffer[Width * (Height - 1)] = '+';
        buffer[Width * Height - 1] = '+';
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
    }
}