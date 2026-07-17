using System;
using System.Reflection;
using Xunit;
using task07;

namespace task07tests;

public class AttributeReflectionTests
{
    [Fact] public void Class_HasDisplayNameAttribute() => Assert.Equal("Пример класса", typeof(SampleClass).GetCustomAttribute<DisplayNameAttribute>()?.DisplayName);
    [Fact] public void Method_HasDisplayNameAttribute() => Assert.Equal("Тестовый метод", typeof(SampleClass).GetMethod("TestMethod")?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName);
    [Fact] public void Property_HasDisplayNameAttribute() => Assert.Equal("Числовое свойство", typeof(SampleClass).GetProperty("Number")?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName);
    [Fact] public void Class_HasVersionAttribute() => Assert.Equal(1, typeof(SampleClass).GetCustomAttribute<VersionAttribute>()?.Major);
    [Fact] public void PrintTypeInfo_ReturnsCorrectFormattedString() => Assert.Contains("Класс: Пример класса", ReflectionHelper.PrintTypeInfo(typeof(SampleClass)));
}
