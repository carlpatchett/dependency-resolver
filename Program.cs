// See https://aka.ms/new-console-template for more information
using dependency_resolver;

var test = 
"""
RiskClassification:LanguageDetection,LanguageTranslation,ExpressionScoring
ExpressionScoring:LanguageTranslation
LanguageDetection
LanguageTranslation:LanguageDetection
""";

var parser = new DTParser();
var nodes = parser.Parse(test);
foreach (var node in nodes)
{
    Console.WriteLine(node.Name);
    foreach (var dependency in node.Dependencies)
    {
        Console.WriteLine($"  Which relies on:{dependency.Name}");
    }
}
