
using var input = new StreamReader(Console.OpenStandardInput());
using var output = new StreamWriter(Console.OpenStandardOutput());

var count = int.Parse(input.ReadLine()!);
var answers = new List<string>();
while (count > 0)
{
    var sizes = input.ReadLine()!.Split(' ').Select(s => int.Parse(s)).ToArray();
    var grid = new char[sizes[0]][];
    for (int i = 0; i < sizes[0]; i++)
    {
        grid[i] = input.ReadLine()!.ToCharArray();
    }
    var points = new List<Data>();

    var index = 0;
    for (int i = 0; i < grid.Length - 2; i++)
    {
        for (int j = 0; j < grid[i].Length - 2; j++)
        {
            if (grid[i][j] == '*')
            {
                var (endX, endY) = FindPoints(grid, (j, i));
                if (endX == j || endY == i)
                    continue;
                points.Add(new Data(new Point(j, i), new Point(endX, endY)));
                j = endX;
            }
        }
    }

    Find(points);

    answers.Add(string.Join(" ", points.OrderBy(r => r.K).Select(r => r.K)));
    count--;
}

foreach (var item in answers)
{
    output.WriteLine(item);
}


static (int, int) FindPoints(char[][] grid, (int x, int y) point)
{
    var (x, y) = point;
    var (endX, endY) = (x, y);

    var xValid = true;
    var yValid = true;

    for (int j = x + 1, i = y + 1; j < grid[y].Length || i < grid.Length; j++, i++)
    {
        if (xValid && j < grid[y].Length)
        {
            if (grid[y][j] == '*')
            {
                endX = j;
            }
            else
                xValid = false;
        }
        if (yValid && i < grid.Length)
        {
            if (grid[i][x] == '*')
            {
                endY = i;
            }
            else
                yValid = false;
        }
        if (!xValid && !yValid)
            break;
    }
    return (endX, endY);
}


static void Find(List<Data> points)
{
    var stack = new Stack<Data>(points.OrderBy(x => x.S));

    while (stack.Count > 0)
    {
        var point = stack.Pop();

        foreach (var p in stack.Where(p => InInterval(point, p)))
        {
            p.K++;
        }
    }
}

static bool InInterval(Data d, Data d1)
{
    return d1.Start.X > d.Start.X && d1.Start.X < d.End.X && d1.End.X > d.Start.X && d1.End.X < d.End.X
        && d1.Start.Y > d.Start.Y && d1.Start.Y < d.End.Y && d1.End.Y > d.Start.Y && d1.End.Y < d.End.Y;
}

public class Point
{
    public int X;
    public int Y;
    public Point(int x, int y)
    {
        X = x; Y = y;
    }
}

public class Data
{
    public Point Start;
    public Point End;
    public int K;
    public double S => Math.Abs(Start.X - End.X) * Math.Abs(Start.Y - End.Y);
    public Data(Point s, Point e)
    {
        Start = s;
        End = e;
    }
}

