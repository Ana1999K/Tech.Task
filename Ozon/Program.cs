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
    var result = new List<int>();

    var index = 0;
    for (int i = 0; i < grid.Length - 2; i++)
    {
        while (index > -1)
        {
            index = Array.IndexOf(grid[i], '*', index);

            if (index == sizes[1] - 1 || index < 0)
                break;

            var (endX, endY) = FindPoints(grid, (index, i));
            if (endX != index && endY != i)
            {
                var point = new Data(new Point(index, i), new Point(endX, endY));
                result.Add(0);
                result.AddRange(FindNested(grid, point, 0));
                DeleteFigure(grid, point);
            }
            index = endX + 1;
        }

        index = 0;
    }

    answers.Add(string.Join(" ", result.OrderBy(r => r)));
    count--;
}

foreach (var item in answers)
{
    output.WriteLine(item);
}

static List<int> FindNested(char[][] grid, Data data, int k)
{
    var nested = new List<int>();
    var startIndex = data.Start.X + 1;
    var index = startIndex;
    for (int i = data.Start.Y + 1; i < data.End.Y - 2; i++)
    {
        while (index > -1)
        {
            index = Array.IndexOf(grid[i], '*', index, data.End.X - index);

            if (index < 0)
                break;

            var (endX, endY) = FindPoints(grid, (index, i));

            if (endX != index && endY != i)
            {
                var point = new Data(new Point(index, i), new Point(endX, endY));
                var currentK = k + 1;
                nested.Add(currentK);
                nested.AddRange(FindNested(grid, point, currentK));
                DeleteFigure(grid, point);
            }

            index = endX + 1;
        }

        index = startIndex;
    }

    return nested;
}

static void DeleteFigure(char[][] grid, Data data)
{
    for (int i = data.Start.Y; i < data.End.Y; i++)
    {
        grid[i][data.Start.X] = '.';
        grid[i][data.End.X] = '.';
    }

    for (int i = data.Start.X; i < data.End.X; i++)
    {
        grid[data.Start.Y][i] = '.';
        grid[data.End.Y][i] = '.';
    }
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


public class Point
{
    public int X { get; }
    public int Y { get; }
    public Point(int x, int y)
    {
        X = x; Y = y;
    }
}

public class Data
{
    public Point Start { get; set; }
    public Point End { get; set; }

    public Data(Point s, Point e)
    {
        Start = s;
        End = e;
    }
}
