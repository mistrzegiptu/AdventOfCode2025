var inputData = File.ReadLines("../../../input.txt").Select(line => { var split = line.Split(','); return (x: Int64.Parse(split[0]), y: Int64.Parse(split[1]), z: Int64.Parse(split[2])); }).ToList();

Func<(long, long, long), (long, long, long), long> Distance = (a, b) => (a.Item1 - b.Item1) * (a.Item1 - b.Item1)
                                                                        + (a.Item2 - b.Item2) * (a.Item2 - b.Item2)
                                                                        + (a.Item3 - b.Item3) * (a.Item3 - b.Item3);


PriorityQueue<((long, long, long), (long, long, long)), long> queue = new();
for(int i = 0; i < inputData.Count; i++)
{
    for(int j = i + 1; j < inputData.Count; j++)
    {
        var first = inputData[i];
        var second = inputData[j];
        queue.Enqueue((first, second), Distance(first, second));
    }
}

List<HashSet<(long, long, long)>> junctionBoxes = [];
int connections = 1000;
int n = inputData.Count;
while (junctionBoxes.Count != 1 || queue.Count > 0)
{
    if(connections == 0)
    {
        Console.WriteLine(junctionBoxes.OrderByDescending(x => x.Count).Take(3).Select(x => x.Count).Aggregate((x, next) => x * next));
    }
    var (first, second) = queue.Dequeue();
    var box = new HashSet<(long, long, long)>() { first, second };
    if(junctionBoxes.Any(x => x.Overlaps(box)))
    {
        var existingBox = junctionBoxes.Where(x => x.Overlaps(box)).ToList();
        foreach(var existing in existingBox)
        {
            junctionBoxes.Remove(existing);
            box.UnionWith(existing);
        }
        junctionBoxes.Add(box);
    }
    else
    {
        junctionBoxes.Add(box);
    }
    if (junctionBoxes.First().Count == n)
    {
        Console.WriteLine(first.Item1 * second.Item1);
        break;
    }
    connections--;
}