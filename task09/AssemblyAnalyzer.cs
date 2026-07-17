using System; using System.IO; using System.Linq; using System.Reflection;
namespace task09;
public class AssemblyAnalyzer {
    public string AnalyzeAssembly(string path) {
        if (!File.Exists(path)) return "Ошибка: файл не найден.";
        var types = Assembly.LoadFrom(path).GetTypes().Where(t => t.IsClass);
        return string.Join("\n", types.Select(t => $"Класс: {t.FullName}\n" + 
            string.Join("\n", t.GetConstructors().Select(c => $"  Конструктор: {t.Name}({string.Join(", ", c.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"))})")) + "\n" +
            string.Join("\n", t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(m => !m.IsSpecialName).Select(m => $"  Метод: {m.Name}"))));
    }
}
