using System; using System.IO; using Xunit; using task08;
namespace task08tests;
public class FileSystemCommandsTests {
    [Fact] public void DirectorySizeCommand_ShouldCalculateSize() {
        var dir = Path.Combine(Path.GetTempPath(), "Size_" + Guid.NewGuid()); Directory.CreateDirectory(dir);
        File.WriteAllText(Path.Combine(dir, "1.txt"), "Hello");
        var cmd = new DirectorySizeCommand(dir); cmd.Execute();
        Assert.Equal(5, cmd.CalculatedSize); Directory.Delete(dir, true);
    }
}
