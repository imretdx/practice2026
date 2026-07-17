using System; using System.IO; using System.Linq;
namespace task08;
public interface ICommand { void Execute(); }
public class DirectorySizeCommand : ICommand {
    private readonly string _dir; public long CalculatedSize { get; private set; }
    public DirectorySizeCommand(string dir) => _dir = dir;
    public void Execute() => CalculatedSize = Directory.Exists(_dir) ? Directory.EnumerateFiles(_dir, "*", SearchOption.AllDirectories).Select(f => new FileInfo(f).Length).Sum() : 0;
}
public class FindFilesCommand : ICommand {
    private readonly string _dir; private readonly string _mask; public string[] FoundFiles { get; private set; } = Array.Empty<string>();
    public FindFilesCommand(string dir, string mask) { _dir = dir; _mask = mask; }
    public void Execute() => FoundFiles = Directory.Exists(_dir) ? Directory.GetFiles(_dir, _mask, SearchOption.TopDirectoryOnly) : Array.Empty<string>();
}
