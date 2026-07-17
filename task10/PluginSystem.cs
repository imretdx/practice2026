using System; using System.Collections.Generic; using System.Linq; using System.Reflection;
namespace task10;
public interface IPluginCommand { void Execute(); }
[AttributeUsage(AttributeTargets.Class)]
public class PluginLoadAttribute : Attribute {
    public string PluginName { get; } public string[] Dependencies { get; }
    public PluginLoadAttribute(string name, params string[] deps) { PluginName = name; Dependencies = deps; }
}
public class PluginEngine {
    public IEnumerable<Type> SortPlugins(IEnumerable<Type> types) {
        var visited = new HashSet<string>(); var sorted = new List<Type>();
        var dict = types.Where(t => t.GetCustomAttribute<PluginLoadAttribute>() != null).ToDictionary(t => t.GetCustomAttribute<PluginLoadAttribute>()!.PluginName, t => t);
        void Visit(string name) {
            if (visited.Contains(name) || !dict.ContainsKey(name)) return;
            Array.ForEach(dict[name].GetCustomAttribute<PluginLoadAttribute>()!.Dependencies, Visit);
            visited.Add(name); sorted.Add(dict[name]);
        }
        dict.Keys.ToList().ForEach(Visit); return sorted;
    }
}
