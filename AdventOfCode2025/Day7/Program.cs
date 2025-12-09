internal class Program
{
    private static Dictionary<(int, int), long> _memo = [];

    private static void Main(string[] args)
    {
        var inputData = File.ReadAllLines("../../../input.txt");
        var splitters = PrecomputeSplitters(inputData);

        long splitCounter = 0;
        var startIndex = (0, inputData.First().Length / 2);
        var queue = new Queue<(int, int)>();
        var alreadySplitted = new HashSet<(int, int)>();
        queue.Enqueue(startIndex);

        while (queue.Count > 0)
        {
            var (y, x) = queue.Dequeue();
            if (x < 0 || y < 0 || x >= inputData.Length || y >= inputData.First().Length)
                continue;
            if (inputData[y + 1][x] == '^')
            {
                if (!alreadySplitted.Contains((y, x)))
                {
                    splitCounter++;
                    alreadySplitted.Add((y, x));
                }

                var right = (y, x + 1);
                var left = (y, x - 1);
                if (!queue.Contains(right))
                    queue.Enqueue(right);
                if (!queue.Contains(left))
                    queue.Enqueue(left);
            }
            else
            {
                queue.Enqueue((y + 1, x));
            }
        }

        Console.WriteLine(splitCounter);
        Console.WriteLine(DFS(inputData, startIndex, splitters));
    }
    private static long DFS(string[] input, (int, int) index, HashSet<(int, int)> splitters)
    {
        var (y, x) = index;

        if (_memo.TryGetValue((y, x), out var memoizedResult))
            return memoizedResult;

        if (y == input.Length)
        {
            _memo[(y, x)] = 1;
            return _memo[(y, x)];
        }

        if (x < 0 || y < 0 || x >= input[0].Length || y >= input.Length)
            return 0;

        if (!splitters.Any(index => index.Item1 >= y && index.Item2 == x))
        {
            _memo[(y, x)] = 1;
            return _memo[(y, x)];
        }

        if (input[y][x] == '^')
        {
            var leftSplitter = splitters.Where((left) => left.Item1 > y && left.Item2 == x - 1).OrderBy(left => left.Item2);
            var rightSplitter = splitters.Where((right) => right.Item1 > y && right.Item2 == x + 1).OrderBy(right => right.Item2);

            var evalLeft = leftSplitter.Any();
            var evalRight = rightSplitter.Any();

            if (evalLeft && evalRight)
                _memo[(y, x)] = DFS(input, rightSplitter.First(), splitters) + DFS(input, leftSplitter.First(), splitters);
            else if (evalLeft)
                _memo[(y, x)] = DFS(input, leftSplitter.First(), splitters) + 1;
            else if (evalRight)
                _memo[(y, x)] = DFS(input, rightSplitter.First(), splitters) + 1;
            else
                _memo[(y, x)] = 2;
        }
        else
            _memo[(y, x)] = DFS(input, (y + 1, x), splitters);

        return _memo[(y, x)];
    }

    private static HashSet<(int, int)> PrecomputeSplitters(string[] input)
    {
        var splitters = new HashSet<(int, int)>();

        for (int i = 0; i < input.Length; i++)
            for (int j = 0; j < input[i].Length; j++)
                if (input[i][j] == '^')
                    splitters.Add((i, j));

        return splitters;
    }
}