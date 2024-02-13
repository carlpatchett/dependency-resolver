namespace dependency_resolver;

public class DTParser
{
    private readonly Dictionary<string, Node> _nodes = [];
    private readonly List<List<Node>> _lanes = [];

    public List<List<Node>> Parse(string input)
    {
        var sections = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var section in sections)
        {
            ProcessSection(section);
        }

        _lanes.Add([]);

        foreach (var (name, node) in _nodes)
        {
            ProcessNode(node);
        }

        return _lanes;
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

    private void ProcessNode(Node node)
    {
        if (!node.HasDependencies())
        {
            _lanes.First().Add(node);
            return;
        }

        var depsMet = 0;
        for (var index = 0; index < _lanes.Count; index++)
        {
            foreach (var dependency in node.Dependencies)
            {
                if (_lanes[index].Contains(dependency))
                {
                    depsMet++;
                }
            }

            if (depsMet != node.Dependencies.Count())
            {
                continue;
            }

            if (_lanes.ElementAtOrDefault(index + 1) != null)
            {
                _lanes[index + 1].Add(node);
                break;
            }

            _lanes.Add([node]);
            break;
        }
    }
}
