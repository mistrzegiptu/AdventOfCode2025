class Program
{
    static void Main()
    {
        string[] inputData = File.ReadAllText("../../../input.txt").Split([Environment.NewLine + Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
        var ranges = inputData.First().Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries).Select(x => (Convert.ToInt64(x.Split('-')[0]), Convert.ToInt64(x.Split('-')[1]))).ToList();
        var ids = inputData.Last().Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x));

        ranges = ranges.OrderBy(x => x.Item1).ThenBy(x => x.Item2).ToList();

        PartOne(ranges, ids);

        PartTwo(ranges);
    }

    static void PartOne(List<(long, long)> ranges, IEnumerable<long> ids)
    {
        int counter = 0;
        foreach (var id in ids)
        {
            bool valid = false;
            int left = 0, right = ranges.Count - 1;

            while (left < right)
            {
                int mid = left + (right - left + 1) / 2;
                if (id >= ranges[mid].Item1)
                    left = mid;
                else if (id < ranges[mid].Item1)
                    right = mid - 1;
            }
            
            while(right >= 0 && ranges[right].Item1 <= id)
            {
                if (ranges[right].Item2 >= id)
                {
                    valid = true;
                }
                right -= 1;
            }

            counter += valid ? 1 : 0;
        }

        Console.WriteLine(counter);
    }

    static void PartTwo(List<(long, long)> ranges)
    {
        for(int i = 0; i < ranges.Count - 1; i++)
        {
            if (ranges[i].Item2 >= ranges[i + 1].Item1)
            {
                ranges[i] = (ranges[i].Item1, Math.Max(ranges[i].Item2, ranges[i + 1].Item2));
                ranges.Remove(ranges[i + 1]);
                i--;
            }
        }

        long idCount = 0;
        foreach(var range in ranges)
        {
            idCount += range.Item2 - range.Item1 + 1;
        }

        Console.WriteLine(idCount);
    }
}