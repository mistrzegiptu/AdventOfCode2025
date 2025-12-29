using System.Text.RegularExpressions;

internal class Program
{
    private const string _inputPattern = @"([\(\[\{][\d,.#]*[\)\]\}])+";
    private static long _targetValue = 0L;
    private static long[] _joltageRequirements = [];
    private static readonly List<long> _buttons = [];

    private static void Main(string[] args)
    {
        var inputData = File.ReadLines("../../../input.txt");
        var minClicksToLight = 0L;
        var minClicksToJoltage = 0L;
        foreach (var line in inputData)
        {
            ParseLine(line);
            minClicksToLight += MinClicksRequired();
            minClicksToJoltage += MinClicksToJoltageRequired();
        }

        Console.WriteLine(minClicksToLight);
    }

    private static long MinClicksRequired()
    {
        int n = _buttons.Count;
        long minClicks = long.MaxValue;

        for (int i = 0; i < (1 << n); i++)
        {
            long startValue = 0L;
            for (int j = 0; j < n; j++)
            {
                if ((i & (1 << j)) == 1 << j)
                {
                    startValue ^= _buttons[j];
                }
            }
            if(startValue == _targetValue)
            {
                minClicks = Math.Min(minClicks, long.PopCount(startValue));
            }
        }

        return minClicks;
    }

    private static long MinClicksToJoltageRequired()
    {
        return 0;
    }

    #region Parsing
    private static void ParseLine(string line)
    {
        _buttons.Clear();

        var matches = Regex.Matches(line, _inputPattern);

        string targetValue = matches.First().ToString();
        string joltageRequirements = matches.Last().ToString();

        foreach (Match match in matches.Skip(1).SkipLast(1))
        {
            _buttons.Add(ParseButtons(match.ToString()));
        }

        _targetValue = ParseTarget(targetValue);
        _joltageRequirements = ParseJoltageRequirements(joltageRequirements);
    }

    private static long ParseTarget(string targetValue)
    {
        targetValue = targetValue.Trim('[', ']');

        long targetNumber = 0;
        for (int i = 0; i < targetValue.Length; i++)
        {
            if(targetValue[i] == '#')
            {
                targetNumber |= 1L << i;
            }
        }

        return targetNumber;
    }

    private static long ParseButtons(string buttons)
    {
        var splitButtons = buttons.Trim('(', ')').Split(',');

        long buttonValue = 0L;
        for (int i = 0; i < splitButtons.Length; i++)
        {
            buttonValue |= 1L << (int.Parse(splitButtons[i].ToString()));
        }

        return buttonValue;
    }

    private static long[] ParseJoltageRequirements(string joltageRequirements)
    {
        joltageRequirements = joltageRequirements.Trim('{', '}');

        return joltageRequirements.Split(',').Select(long.Parse).ToArray();
    }
    #endregion
}