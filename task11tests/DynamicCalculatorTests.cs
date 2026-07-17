using System; using Xunit; using task11;
namespace task11tests;
public class DynamicCalculatorTests {
    private const string Code = "using task11; namespace task11; public class Calculator : ICalculator { public int Add(int a, int b) => a + b; public int Minus(int a, int b) => a - b; public int Mul(int a, int b) => a * b; public int Div(int a, int b) => a / b; }";
    [Fact] public void DynamicCalculator_ShouldPerformOperationsWithoutReflection() {
        ICalculator calc = DynamicCalculatorFactory.CreateFromString(Code);
        Assert.Equal(10, calc.Add(7, 3));
    }
}
