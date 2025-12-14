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

        var maxAreaGreen = inputData.SelectMany(x => inputData.Select(y => (x, y))).OrderByDescending(pair => Area(pair.x, pair.y)).First(pair => IsInsideBounds(greenLines, EnumerateRect(pair.Item1, pair.Item2)));

        Console.WriteLine(Area(maxAreaGreen.x, maxAreaGreen.y));
    }

    private static IEnumerable<(long, long)> EnumerateRect((long, long) a, (long, long) b)
    {
        for (long i = Math.Min(a.Item1, b.Item1); i <= Math.Max(a.Item1, b.Item1); i++)
            for (long j = Math.Min(a.Item2, b.Item2); j <= Math.Max(a.Item2, b.Item2); j++)
                yield return (i, j);
    }

    private static bool IsInsideBounds(List<((long, long), (long, long))> greenLines, IEnumerable<(long, long)> rect)
    {
        foreach (var point in rect)
        {
            int hitCounter = 0;
            foreach (var line in greenLines)
            {
                if (point.Item1 == line.Item1.Item1 && point.Item1 == line.Item2.Item1)
                {
                    hitCounter = 1;
                    break;
                }
                if (point.Item2 == line.Item1.Item2 && point.Item2 == line.Item2.Item2)
                {
                    hitCounter = 1;
                    break;
                }
                if (line.Item1.Item2 == line.Item2.Item2)
                {
                    if (point.Item1 > line.Item1.Item1)
                    {
                        long xMin = Math.Min(line.Item1.Item1, line.Item2.Item1);
                        long xMax = Math.Max(line.Item1.Item1, line.Item2.Item1);

                        if (point.Item1 >= xMin && point.Item1 <= xMax)
                            hitCounter++;
                    }
                }
                else if (line.Item1.Item1 == line.Item2.Item1)
                {
                    if (point.Item1 > line.Item1.Item1)
                    {
                        long yMin = Math.Min(line.Item1.Item2, line.Item2.Item2);
                        long yMax = Math.Max(line.Item1.Item2, line.Item2.Item2);

                        if (point.Item2 >= yMin && point.Item2 <= yMax)
                            hitCounter++;
                    }
                }
            }

            if (hitCounter % 2 == 0)
                return false;
        }

        return true;
    }
}