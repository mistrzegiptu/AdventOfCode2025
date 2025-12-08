using System.Text.RegularExpressions;

var lines = File.ReadAllLines("../../../input.txt");
var inputData = lines.Select(line => Regex.Replace(line, @"\s+", " ")).Select(line => line.Trim().Split(" ")).ToList();

long totalResult = 0;
for(int j = 0; j < inputData[0].Length; j++)
{
    long columnResult = Convert.ToInt64(inputData[0][j]);
    for (int i = 1; i < inputData.Count - 1; i++)
    {
        if (inputData.Last()[j] == "+")
            columnResult += Convert.ToInt64(inputData[i][j]);
        else if (inputData.Last()[j] == "*")
            columnResult *= Convert.ToInt64(inputData[i][j]);
    }

    totalResult += columnResult;
}

Console.WriteLine(totalResult);


long totalResultP2 = 0;
var previousOperation = 0;
var numbersByColumn = new List<long>();

for (int j = 0; j < lines[0].Length; j++)
{
    var potentialOperator = lines.Last()[j];
    if (potentialOperator != ' ')
    {
        if (previousOperation == '*')
            totalResultP2 += numbersByColumn.Aggregate((x, next) => x * next);
        else if (previousOperation == '+')
            totalResultP2 += numbersByColumn.Sum();

        previousOperation = potentialOperator;
        numbersByColumn.Clear();
    }
    long number = 0;
    for (int i = 0; i < lines.GetLength(0)-1; i++)
    {
        long digit = 0;
        if (Int64.TryParse(lines[i][j].ToString(), out digit))
            number = number * 10L + digit; 
    }
    if(number != 0)
        numbersByColumn.Add(number);
}

if (previousOperation == '*')
    totalResultP2 += numbersByColumn.Aggregate((x, next) => x * next);
else if (previousOperation == '+')
    totalResultP2 += numbersByColumn.Sum();

Console.WriteLine(totalResultP2);