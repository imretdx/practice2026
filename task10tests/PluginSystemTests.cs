using System; using System.Collections.Generic; using System.Linq; using Xunit; using task10;
namespace task10tests;
[PluginLoad("A")] public class PluginA : IPluginCommand { public void Execute() {} }
[PluginLoad("B", "A")] public class PluginB : IPluginCommand { public void Execute() {} }
public class PluginSystemTests {
    [Fact] public void SortPlugins_ShouldOrderDependencies() {
        var sorted = new PluginEngine().SortPlugins(new List<Type> { typeof(PluginB), typeof(PluginA) }).ToList();
        Assert.True(sorted.IndexOf(typeof(PluginA)) < sorted.IndexOf(typeof(PluginB)));
    }
}
