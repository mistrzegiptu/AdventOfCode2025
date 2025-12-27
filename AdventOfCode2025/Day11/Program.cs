internal class Program
{
    private const string _startingNode = "you";
    private const string _serverRackNode = "svr";
    private const string _outNode = "out";
    private const string _dacNode = "dac";
    private const string _ddtNode = "fft";

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
                if (!incomingVertices.TryAdd(v, 0))
                    incomingVertices[v] += 1;
        }

        var outPaths = OutPathsCount(graph);
        
        Console.WriteLine(outPaths);

        incomingVertices.Add(_serverRackNode, 0);
        var topologicalOrder = TopologicalSort(graph, incomingVertices);

        //foreach (var key in graph.Keys)
        //{
        //    Console.Write($"{key} -> ");
        //    foreach (var connection in graph[key])
        //    {
        //        Console.Write($"{connection}, ");
        //    }
        //    Console.WriteLine();
        //}
    }

    private static int OutPathsCount(Dictionary<string, List<string>> graph)
    {
        return DFS(_startingNode, graph, []);
    }

    private static int DacAndFftPathsCount(Dictionary<string, List<string>> graph)
    {
        return DFSWithTracking(_serverRackNode, graph, [], false, false);
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

    private static int DFSWithTracking(string currentNode, Dictionary<string, List<string>> graph, HashSet<string> visited, bool dacVisited, bool fftVisited)
    {
        if (currentNode == _outNode)
            return dacVisited && fftVisited ? 1 : 0;
        if (visited.Contains(currentNode))
            return 0;
        if (currentNode == _dacNode)
            dacVisited = true;
        else if (currentNode == _ddtNode)
            fftVisited = true;

        if (!graph.TryGetValue(currentNode, out var neighbors))
            return 0;

        visited.Add(currentNode);

        int sum = 0;
        for (int i = 0; i < neighbors.Count; i++)
        {
            sum += DFSWithTracking(neighbors[i], graph, visited, dacVisited, fftVisited);
        }

        visited.Remove(currentNode);

        return sum;
    }
}