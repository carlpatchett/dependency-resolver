using dependency_resolver;

var nodeDeclarations =
"""
Node::First
Node::Second:First
Node::Third:Second
Node::Fourth:Second
Node::Fifth:First,Second,Third
Node::Sixth:Third,Fourth
Node::Seventh:Fifth
Script::First:Pre,FirstPreScript
Script::First:Post,FirstPostScript
Script::Second:Pre,SecondPreScript
Script::Second:Post,SecondPostScript
Script::Third:Pre,ThirdPreScript
Script::Third:Post,ThirdPostScript
Script::Fourth:Post,FourthPostScript
Script::Fifth:Pre,FifthPreScript
Script::Sixth:Post,SixthPostScript
Script::Seventh:Post,SeventhPostScript
""";

var parser = new DTParser(); 
var lanes = parser.Parse(nodeDeclarations);

for (var index = 0; index < lanes.Count; index++)
{
    Console.WriteLine($"Lane {index + 1}");
    foreach (var obj in lanes[index])
    {
        Console.WriteLine($"  {obj}");

        if (obj is Node node)
        {
            foreach (var dep in node.Dependencies)
            {
                Console.WriteLine($"   - {dep.Name}");
            }
        }
    }
}
