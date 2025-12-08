internal class Program
{
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

        if (y == input.First().Length)
            return 1;

        if (x < 0 || y < 0 || x >= input.Length || y >= input.First().Length)
            return 0;

        if (!splitters.Any((index) => index.Item1 >= y && index.Item2 == x))
            return 1;

        if (input[y][x] == '^')
        {
            var leftSplitter = splitters.Where((left) => left.Item1 > y && left.Item2 == x - 1);
            var rightSplitter = splitters.Where((right) => right.Item1 > y && right.Item2 == x + 1);

            var evalLeft = leftSplitter.Count() > 0;
            var evalRight = rightSplitter.Count() > 0;

            if (evalLeft && evalRight)
                return DFS(input, rightSplitter.First(), splitters) + DFS(input, leftSplitter.First(), splitters);
            else if (evalLeft)
                return DFS(input, leftSplitter.First(), splitters) + 1;
            else if (evalRight)
                return DFS(input, rightSplitter.First(), splitters) + 1;
            else
                return 2;
        }
        else
            return DFS(input, (y + 1, x), splitters);
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