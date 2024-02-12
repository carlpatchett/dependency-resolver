namespace dependency_resolver;

public class Node(string name)
{
    private readonly List<Node> _dependencies = [];

    public void AddDependency(Node dependency)
    {
        _dependencies.Add(dependency);
    }

    public string Name { get; set; } = name;
    public IEnumerable<Node> Dependencies => _dependencies;

    public void RemoveDependency(Node dependency)
    {
        _dependencies.Remove(dependency);
    }

    public bool HasDependencies()
    {
        return _dependencies.Count > 0;
    }

    public override string ToString()
    {
        return Name;
    }
}
