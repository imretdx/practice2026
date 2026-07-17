using System; using System.IO; using System.Linq; using System.Reflection; using Microsoft.CodeAnalysis; using Microsoft.CodeAnalysis.CSharp;
namespace task11;
public interface ICalculator {
    int Add(int a, int b); int Minus(int a, int b); int Mul(int a, int b); int Div(int a, int b);
}
public static class DynamicCalculatorFactory {
    public static ICalculator CreateFromString(string classCode) {
        var syntaxTree = CSharpSyntaxTree.ParseText(classCode);
        var references = new MetadataReference[] {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime")).Location),
            MetadataReference.CreateFromFile(typeof(ICalculator).Assembly.Location)
        };
        var compilation = CSharpCompilation.Create("CalcAsm_" + Guid.NewGuid().ToString("N"), new[] { syntaxTree }, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var ms = new MemoryStream(); var result = compilation.Emit(ms);
        if (!result.Success) throw new InvalidOperationException("Ошибка компиляции.");
        ms.Seek(0, SeekOrigin.Begin); var assembly = Assembly.Load(ms.ToArray());
        var type = assembly.GetType("task11.Calculator") ?? throw new Exception("Класс не найден.");
        return (ICalculator)Activator.CreateInstance(type)!;
    }
}
