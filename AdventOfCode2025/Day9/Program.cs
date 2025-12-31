internal class Program
{
    private static void Main(string[] args)
    {
        var inputData = File.ReadAllLines("../../../input.txt").Select(line => (long.Parse(line.Split(',')[0]), long.Parse(line.Split(',')[1])));

        Func<(long, long), (long, long), long> Area = (a, b) => (Math.Abs(a.Item1 - b.Item1) + 1) * (Math.Abs(a.Item2 - b.Item2) + 1);
        Func<(long, long), (long, long), long> Distance = (a, b) => Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);

        var maxArea = inputData.SelectMany(x => inputData.Select(y => (x, y))).Max(pair => Area(pair.x, pair.y));

        Console.WriteLine(maxArea);

        var redTiles = new HashSet<(long, long)>();
        var greenLines = new List<((long, long), (long, long))>();

        var firstTile = inputData.First();
        var currentTile = firstTile;

        while(true)
        {
            redTiles.Add(currentTile);
            var connected = inputData.Where(c => c.Item1 == currentTile.Item1 ^ c.Item2 == currentTile.Item2 && !redTiles.Contains(c));
            (long, long) toConnect;
            if (connected.Any())
                toConnect = connected.MinBy(c => Distance(currentTile, c));
            else
                toConnect = firstTile;

            greenLines.Add((currentTile, toConnect));

            currentTile = toConnect;

            if (currentTile == firstTile)
                break;
        }

        var maxAreaGreen = inputData.SelectMany(x => inputData.Select(y => (x, y))).OrderByDescending(pair => Area(pair.x, pair.y)).First(pair => IsInsideBounds(greenLines, pair.Item1, pair.Item2));

        Console.WriteLine(Area(maxAreaGreen.x, maxAreaGreen.y));
    }

    private static bool IsInsideBounds(List<((long, long), (long, long))> greenLines, (long, long) rectPointA, (long, long) rectPointB)
    {
        var bottomLeft = (x: Math.Min(rectPointA.Item1,  rectPointB.Item1), y: Math.Min(rectPointA.Item2, rectPointB.Item2));
        var topRight = (x: Math.Max(rectPointA.Item1,  rectPointB.Item1), y: Math.Max(rectPointA.Item2, rectPointB.Item2));

        foreach (var line in greenLines)
        {
            (var a, var b) = line;

            var lineA = (x: Math.Min(a.Item1, b.Item1), y: Math.Min(a.Item2, b.Item2));
            var lineB = (x: Math.Max(a.Item1, b.Item1), y: Math.Max(a.Item2, b.Item2));

            if(lineA.x < topRight.x && lineB.x > bottomLeft.x && lineA.y < topRight.y && lineB.y > bottomLeft.y)
                return false;
        }

        return true;
    }
}