class Program
{
    public static void Main()
    {
        var inputData = File.ReadAllText("../../../input.txt");

        var palindromeSum = inputData.Split(",").Select(range => (from: Convert.ToInt64(range.Split("-")[0]), to: Convert.ToInt64(range.Split("-")[1])))
            .SelectMany(x => EnumerableLongRange(x.from, x.to))
            .Where(num => { var len = Math.Ceiling(Math.Log10(num)); return len % 2 == 0 && (int)(num / (int)Math.Pow(10, len / 2)) == (num % (int)Math.Pow(10, len / 2)); }).Sum();

        var patternBasedSum = inputData.Split(",").Select(range => (from: Convert.ToInt64(range.Split("-")[0]), to: Convert.ToInt64(range.Split("-")[1])))
            .SelectMany(x => EnumerableLongRange(x.from, x.to))
            .Where(num => Enumerable.Range(1, num.ToString().Length - 1).Select(y => string.Concat(Enumerable.Repeat(num.ToString()[..y], num.ToString().Length / y))).Any(z => z.Equals(num.ToString()))).Sum();

        Console.WriteLine(palindromeSum);
        Console.WriteLine(patternBasedSum);
    }
    
    private static IEnumerable<long> EnumerableLongRange(long from, long to)
    {
        for (long i = from; i <= to; i++) yield return i;
    }
}