internal class Program
{
    private const string _startingNode = "you";
    private const string _serverRackNode = "svr";
    private const string _outNode = "out";
    private const string _dacNode = "dac";
    private const string _fftNode = "fft";

    private static void Main(string[] args)
    {
        var inputData = File.ReadLines("../../../input.txt");

        Dictionary<string, List<string>> graph = [];
        Dictionary<string, int> incomingVertices = [];
        foreach (var line in inputData)
        {
            var splitted = line.Split(": ");

            var label = splitted.First();
            var connected = splitted.Last().Split(" ").ToList();
            graph.Add(label, connected);

            foreach (var v in connected)
            {
                incomingVertices.TryAdd(v, 0);
                incomingVertices[v]++;
            }
        }

        var outPaths = OutPathsCount(graph);

        Console.WriteLine(outPaths);

        incomingVertices.Add(_serverRackNode, 0);

        var topologicalOrder = TopologicalSort(graph, incomingVertices);

        var svrToFft = NumberOfPaths(_serverRackNode, _fftNode, graph, topologicalOrder);
        var fftToDac = NumberOfPaths(_fftNode, _dacNode, graph, topologicalOrder);
        var dacToOut = NumberOfPaths(_dacNode, _outNode, graph, topologicalOrder);

        var svrToDac = NumberOfPaths(_serverRackNode, _dacNode, graph, topologicalOrder);
        var dacToFft = NumberOfPaths(_dacNode, _fftNode, graph, topologicalOrder);
        var fftToOut = NumberOfPaths(_fftNode, _outNode, graph, topologicalOrder);

        var total = svrToFft * fftToDac * dacToOut + svrToDac * dacToFft * fftToOut;
        Console.WriteLine(total);
    }

    private static int OutPathsCount(Dictionary<string, List<string>> graph)
    {
        return DFS(_startingNode, graph, []);
    }

    private static int DFS(string currentNode, Dictionary<string, List<string>> graph, HashSet<string> visited)
    {
        if (currentNode == _outNode)
            return 1;
        if (visited.Contains(currentNode))
            return 0;

        visited.Add(currentNode);

        var sum = graph[currentNode].Sum(x => DFS(x, graph, visited));

        visited.Remove(currentNode);

        return sum;
    }

    private static List<string> TopologicalSort(Dictionary<string, List<string>> graph, Dictionary<string, int> incomingVertices)
    {
        List<string> topologicalOrder = [];
        Queue<string> queue = [];

        foreach (var key in graph.Keys)
            if (incomingVertices[key] == 0)
                queue.Enqueue(key);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();

            topologicalOrder.Add(node);

            if (node == _outNode)
                break;

            foreach (var connected in graph[node])
            {
                incomingVertices[connected] -= 1;
                if (incomingVertices[connected] == 0)
                    queue.Enqueue(connected);
            }
        }

        return topologicalOrder;
    }

    private static long NumberOfPaths(string source, string destination, Dictionary<string, List<string>> graph, List<string> topologicalOrder)
    {
        Dictionary<string, long> paths = [];
        topologicalOrder.ForEach(v => paths.Add(v, 0));
        paths[source] = 1L;

        foreach (var node in topologicalOrder)
        {
            if (node == _outNode)
                continue;

            foreach (var connected in graph[node])
                paths[connected] += paths[node];
        }

        return paths[destination];
    }
}