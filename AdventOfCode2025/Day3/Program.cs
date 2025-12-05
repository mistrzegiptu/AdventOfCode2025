class Program
{
    public static void Main()
    {
        var inputData = File.ReadAllLines("../../../input.txt").Select(x=>x.ToCharArray().Select(y => Convert.ToInt32(y-'0')).ToList());

        GetMax(inputData, 2);
        GetMax(inputData, 12);
    }

    public static void GetMax(IEnumerable<IEnumerable<int>> batteries, int count)
    {
        long joltageResult = 0;
        foreach(List<int> battery in batteries)
        {
            var stack = new Stack<int>();
            int k = battery.Count - count;
            foreach (var i in battery)
            {
                while (stack.Count > 0 && stack.Peek() < i && k > 0)
                {
                    stack.Pop();
                    k--;
                }
                stack.Push(i);
            }

            while(stack.Count > 0 && k-- > 0)
            {
                stack.Pop();
            }

            joltageResult += stack.Reverse().Aggregate(0L, (acc, next) => acc * 10L + next);
        }

        Console.WriteLine(joltageResult);
    }
}