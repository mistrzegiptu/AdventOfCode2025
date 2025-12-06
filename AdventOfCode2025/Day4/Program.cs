class Program
{
    public static void Main()
    {
        var inputData = File.ReadLines("../../../input.txt").Select(x => x.ToCharArray()).ToArray();

        PartOne(inputData);
        PartTwo(inputData);
    }

    private static void PartOne(char[][] input)
    {
        int rollsCount = 0;

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input.GetLength(0); j++)
            {
                if(input[i][j] == '@' && GetAdjacentCount(input, i, j) < 4)
                    rollsCount++;
            }
        }

        Console.WriteLine(rollsCount);
    }

    private static void PartTwo(char[][] input)
    {
        int rollsToRmCount = 0;
        bool removed = true;

        while (removed)
        {
            removed = false;
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input.GetLength(0); j++)
                {
                    if (input[i][j] == '@' && GetAdjacentCount(input, i, j) < 4)
                    {
                        input[i][j] = '.';
                        rollsToRmCount++;
                        removed = true;
                    }
                }
            }
        }

        Console.WriteLine(rollsToRmCount);
    }

    private static int GetAdjacentCount(char[][] grid, int i, int j)
    {
        int adjacentRolls = 0;

        for(int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                if (i + dx < 0 || i + dx >= grid.Length)
                    continue;

                if (j + dy < 0 || j + dy >= grid.GetLength(0))
                    continue;

                adjacentRolls += grid[i + dx][j + dy] == '@' ? 1 : 0;
            }
        }

        return adjacentRolls;
    }
}