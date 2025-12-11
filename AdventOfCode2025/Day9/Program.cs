var inputData = File.ReadAllLines("../../../testInput.txt").Select(line => (Int64.Parse(line.Split(',')[0]), Int64.Parse(line.Split(',')[1])));

var coords = inputData.OrderBy(coord => coord.Item1).ThenBy(coord => coord.Item2).ToList();

Func<(long, long), (long, long), long> Area = (a, b) => Math.Abs(a.Item1 - b.Item1) * Math.Abs(a.Item2 - b.Item2);

var lowestCoordIndex = 0;
var highestCoordIndex = coords.Count - 1;

for(int i = 1; i < coords.Count / 2; i++)
{
    if (Area(coords[lowestCoordIndex], coords[highestCoordIndex]) < Area(coords[i], coords[highestCoordIndex]))
        lowestCoordIndex = i;
    if (Area(coords[lowestCoordIndex], coords[highestCoordIndex]) < Area(coords[lowestCoordIndex], coords[^i]))
        highestCoordIndex = coords.Count - i;
}

Console.WriteLine(coords[lowestCoordIndex].ToString() + coords[highestCoordIndex].ToString());
Console.WriteLine(Area(coords[lowestCoordIndex], coords[highestCoordIndex]));