using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using task05;

namespace task05tests;

#pragma warning disable CS8618, CS0169
public class TestClass
{
    public int PublicField;
    private string _privateField;
    public int Property { get; set; }
    public void Method() { }
    public string ComplexMethod(int id, string name) => string.Empty;
}
#pragma warning restore CS8618, CS0169

[Serializable]
public class AttributedClass { }

public class ClassAnalyzerTests
{
    [Fact] public void GetPublicMethods_ReturnsCorrectMethods() => Assert.Contains("Method", new ClassAnalyzer(typeof(TestClass)).GetPublicMethods());
    [Fact] public void GetAllFields_IncludesPrivateFields() => Assert.Contains("_privateField", new ClassAnalyzer(typeof(TestClass)).GetAllFields());
    [Fact] public void GetProperties_ReturnsCorrectProperties() => Assert.Contains("Property", new ClassAnalyzer(typeof(TestClass)).GetProperties());
    [Fact] public void HasAttribute_ChecksCorrectAttributes() => Assert.True(new ClassAnalyzer(typeof(AttributedClass)).HasAttribute<SerializableAttribute>());
    
    [Fact]
    public void GetMethodParams_ReturnsCorrectSignature()
    {
        var info = new ClassAnalyzer(typeof(TestClass)).GetMethodParams("ComplexMethod").ToList();
        // Исправлено: сравниваем конкретную строчку из списка метаданных
        Assert.Equal("Return: String", info[0]);
    }
}
