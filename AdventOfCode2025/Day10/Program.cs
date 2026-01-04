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
            minClicksToLight += ClicksRequired(_targetValue, _buttons).Select(x => long.PopCount(x)).Min();
            minClicksToJoltage += MinClicksToJoltageRequired(_joltageRequirements, _buttons);
        }

        Console.WriteLine(minClicksToLight);
        Console.WriteLine(minClicksToJoltage);
    }

    private static IEnumerable<long> ClicksRequired(long targetValue, List<long> buttons)
    {
        int n = buttons.Count;

        for (int i = 0; i < (1 << n); i++)
        {
            long startValue = 0L;
            for (int j = 0; j < n; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    startValue ^= buttons[j];
                }
            }
            if(startValue == targetValue)
            {
                yield return i;
            }
        }
    }

    private static long MinClicksToJoltageRequired(long[] joltageRequirements, List<long> buttons)
    {
        return SearchMinClicks(joltageRequirements, buttons, 0L);
    }

    private static long SearchMinClicks(long[] joltageRequirements, List<long> buttons, long clicked)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if ((clicked & (1 << i)) != 0)
            {
                for(int j = 0; j < joltageRequirements.Length; j++)
                { 
                    joltageRequirements[j] -= ((buttons[i] >> j) & 1);
                    
                    if (joltageRequirements[j] < 0)
                        return long.MaxValue;
                }      
            }
        }

        if (joltageRequirements.All(x => x == 0))
            return long.PopCount(clicked);

        var factor = 1L;
        if(joltageRequirements.All(x => x % 2 == 0))
        {
            factor = 2L;
            joltageRequirements = joltageRequirements.Select(x => x / 2).ToArray();
        }

        var resultClicks = long.MaxValue;
        foreach (var clicks in ClicksRequired(ConvertToEvenBin(joltageRequirements), buttons))
        {
            var result = SearchMinClicks((long[])joltageRequirements.Clone(), buttons, clicks);
            if (result != long.MaxValue)
            {
                result = factor * result + long.PopCount(clicked);
                resultClicks = Math.Min(resultClicks, result);
            }
        }

        return resultClicks;
    }

    private static long ConvertToEvenBin(long[] joltageRequirements)
    {
        long evenBin = 0L;
        for (int i = 0; i < joltageRequirements.Length; i++)
        {
            if((joltageRequirements[i] & 1) != 0)
            {
                evenBin |= 1L << i;
            }
        }

        return evenBin;
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