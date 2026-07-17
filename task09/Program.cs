using System;
namespace task09;
internal class Program {
    private static void Main(string[] args) {
        if (args.Length > 0) Console.WriteLine(new AssemblyAnalyzer().AnalyzeAssembly(args[0]));
    }
}
