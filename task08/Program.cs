using System; using System.IO; using System.Reflection;
namespace task08;
internal class Program {
    private static void Main() {
        Assembly asm = Assembly.LoadFrom(Assembly.GetExecutingAssembly().Location);
        string testDir = Path.Combine(Path.GetTempPath(), "AsmTestDir"); Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "1.txt"), "Hi");
        var cmd = Activator.CreateInstance(asm.GetType("task08.DirectorySizeCommand")!, testDir) as ICommand;
        cmd?.Execute(); Directory.Delete(testDir, true);
    }
}
