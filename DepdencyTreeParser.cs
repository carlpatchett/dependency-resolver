namespace dependency_resolver;

public class DTParser
{
    private readonly Dictionary<string, Node> _nodes = [];
    private List<List<object>> _lanes = [];

    public List<List<object>> Parse(string input)
    {
        var sections = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        var nodeSections = sections.Where(s => s.Split("::").First() == "Node");
        foreach (var nodeSection in nodeSections)
        {
            ProcessNodeSection(nodeSection);
        }

        _lanes.Add([]);

        foreach (var (name, node) in _nodes)
        {
            ProcessNode(node);
        }

        _lanes = AddPreAndPostLanes();

        var scriptSections = sections.Where(s => s.Split("::").First() == "Script");
        foreach (var scriptSection in scriptSections)
        {
            ProcessScriptSection(scriptSection);
        }

        RemoveEmptyLanes();

        return _lanes;
    }

    private void RemoveEmptyLanes()
    {
        var lanes = _lanes.ToList();
        foreach (var lane in lanes)
        {
            if (lane.Count == 0)
            {
                _lanes.Remove(lane);
            }
        }
    }

    private List<List<object>> AddPreAndPostLanes()
    {
        var newLanes = new List<List<object>>();
        for (var index = 0; index < _lanes.Count; index++)
        {
            newLanes.Add([]);
            newLanes.Add(_lanes[index]);
            newLanes.Add([]);
        }

        return newLanes;
    }

    private void ProcessScriptSection(string section)
    {
        var def = section.Split("::").Last();
        var nodeAndDef = def.Split(":");
        var lane = _lanes.FirstOrDefault(lane => lane.FirstOrDefault(o => o is Node n && n.Name == nodeAndDef.First()) != null);
        if (lane == null)
        {
            throw new InvalidOperationException("Could not find lane for node " + nodeAndDef.First());
        }

        var laneIndex = _lanes.IndexOf(lane);
        var phaseAndDef = nodeAndDef.Last().Split(",");
        if (phaseAndDef.First() == "Pre")
        {
            laneIndex--;
        }
        else
        {
            laneIndex++;
        }

        _lanes[laneIndex].Add(phaseAndDef.Last());
    }

    private void ProcessNodeSection(string section)
    {
        var parts = section.Split("::").Last().Split(':');
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
