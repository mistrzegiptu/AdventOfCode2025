using System.Text.RegularExpressions;

var inputData = File.ReadLines("../../../input.txt");

Dictionary<int, int> filled = [];
List<(int, int)> sizes = [];
List<List<int>> figureCount = [];

var result = 0;

const int figureArea = 9;
const string labelPattern = @"^(\d+):";
const string gridPattern = @"(\d+)x(\d+)[^\n]*";
const string linePattern = @"[.#]+";
var regexLabel = new Regex(labelPattern);
var regexGrid = new Regex(gridPattern);
var regexLine = new Regex(linePattern);

int label = 0;
foreach (var line in inputData)
{
    if(regexLabel.IsMatch(line))
    {
        label = int.Parse(regexLabel.Match(line).Groups[1].ToString());
    }
    else if(regexLine.IsMatch(line))
    {
        if(!filled.ContainsKey(label))
            filled.Add(label, 0);

        filled[label] += line.Where(c => c == '#').Count();
    }
    else if(regexGrid.IsMatch(line))
    {
        var gridSizeX = int.Parse(regexGrid.Match(line).Groups[1].ToString());
        var gridSizeY = int.Parse(regexGrid.Match(line).Groups[2].ToString());

        var counts = line.Split(" ").Skip(1).Select(int.Parse).ToList();

        sizes.Add((gridSizeX, gridSizeY));
        figureCount.Add(counts);
    }
}

for(int i = 0; i < sizes.Count; i++)
{
    var (gridSizeX, gridSizeY) = sizes[i];
    var area = gridSizeX * gridSizeY;

    var totalFiguresArea = 0;
    for(int j = 0; j < figureCount[i].Count; j++)
    {
        var count = figureCount[i][j];
        totalFiguresArea += figureArea * count;
    }

    if(totalFiguresArea <= area)
        result++;
}

Console.WriteLine(result);