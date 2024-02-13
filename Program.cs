using dependency_resolver;

var test =
"""
First
Second:First
Third:Second
Fourth:Second
Fifth:First,Second,Third
Sixth:Third,Fourth
Seventh:Fifth
""";

var parser = new DTParser();
var lanes = parser.Parse(test);

for (var index = 0; index < lanes.Count; index++)
{
    Console.WriteLine($"Lane {index + 1}");
    foreach (var node in lanes[index])
    {
        Console.WriteLine($"  {node.Name}");

        foreach (var dep in node.Dependencies)
        {
            Console.WriteLine($"   - {dep.Name}");
        }
    }
}
