namespace dependency_resolver;

public class DTParser
{
    private readonly Dictionary<string, Node> _nodes = [];
    public IEnumerable<Node> Parse(string input)
    {
        var sections = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var section in sections)
        {
            ProcessSection(section);
        }

        return CalculateOrder();
    }

    private void ProcessSection(string section)
    {
        var parts = section.Split(':');
        var name = parts[0];
        var depNames = parts.Length > 1 ? parts[1].Split(',') : [];

        if (!_nodes.TryGetValue(name, out var node))
        {
            node = new Node(name);
        }

        foreach (var depName in depNames)
        {
            ProcessDependency(node, depName);
        }

        _nodes.TryAdd(name, node);
    }

    private void ProcessDependency(Node node, string depName)
    {
        if (!_nodes.TryGetValue(depName, out var depNode))
        {
            depNode = new Node(depName);
        }

        node.AddDependency(depNode);

        _nodes.TryAdd(depName, depNode);
    }

    private IEnumerable<Node> CalculateOrder()
    {
        var nodes = _nodes.Values.ToList();
        var ordered = OrderNodes(nodes);
        return ordered;
    }

    private static IEnumerable<Node> OrderNodes(List<Node> nodes)
    {
        var ordered = new List<Node>();
        foreach (var node in nodes)
        {
            CrawlDependencies(node, ordered);
        }

        return ordered;
    }

    private static void CrawlDependencies(Node node, List<Node> ordered)
    {
        if (node.HasDependencies())
        {
            foreach (var dependency in node.Dependencies)
            {
                CrawlDependencies(dependency, ordered);
            }
        }

        if (ordered.Contains(node))
        {
            return;
        }

        ordered.Add(node);
    }

}
