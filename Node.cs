
namespace dependency_resolver;

public class Node(string name)
{
    private readonly List<Node> _dependencies = [];
    private readonly List<string> _preScripts = [];
    private readonly List<string> _postScripts = [];

    public void AddDependency(Node dependency)
    {
        _dependencies.Add(dependency);
    }

    public string Name { get; set; } = name;
    public IEnumerable<Node> Dependencies => _dependencies;
    public IEnumerable<string> PreScripts => _preScripts;
    public IEnumerable<string> PostScripts => _postScripts;

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

    internal void AddPreScript(string scriptName)
    {
        _preScripts.Add(scriptName);
    }

    internal void AddPostScript(string scriptName)
    {
        _postScripts.Add(scriptName);
    }
}
