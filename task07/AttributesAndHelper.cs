using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace task07;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class DisplayNameAttribute : Attribute
{
    public string DisplayName { get; }
    public DisplayNameAttribute(string displayName) => DisplayName = displayName;
}

[AttributeUsage(AttributeTargets.Class)]
public class VersionAttribute : Attribute
{
    public int Major { get; }
    public int Minor { get; }
    public VersionAttribute(int major, int minor) { Major = major; Minor = minor; }
}

[DisplayName("Пример класса")]
[Version(1, 0)]
public class SampleClass
{
    [DisplayName("Числовое свойство")]
    public int Number { get; set; }
    [DisplayName("Тестовый метод")]
    public void TestMethod() { }
}

public static class ReflectionHelper
{
    public static string PrintTypeInfo(Type type)
    {
        if (type == null) return string.Empty;
        var classDisplay = type.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? "Нет имени";
        var classVersion = type.GetCustomAttribute<VersionAttribute>();
        var versionStr = classVersion != null ? $"{classVersion.Major}.{classVersion.Minor}" : "0.0";
        var propsInfo = type.GetProperties().Select(p => p.GetCustomAttribute<DisplayNameAttribute>()).Where(a => a != null).Select(a => $"Свойство: {a!.DisplayName}");
        var methodsInfo = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(m => !m.IsSpecialName).Select(m => m.GetCustomAttribute<DisplayNameAttribute>()).Where(a => a != null).Select(a => $"Метод: {a!.DisplayName}");
        return $"Класс: {classDisplay}, Версия: {versionStr}\n" + string.Join("\n", propsInfo.Concat(methodsInfo));
    }
}
