using System.Reflection; using Xunit; using task09;
namespace task09tests;
public class AssemblyAnalyzerTests {
    [Fact] public void AnalyzeAssembly_CurrentAssembly_ReturnsMetadata() {
        var res = new AssemblyAnalyzer().AnalyzeAssembly(Assembly.GetExecutingAssembly().Location);
        Assert.Contains("Класс: task09tests.AssemblyAnalyzerTests", res);
    }
}
