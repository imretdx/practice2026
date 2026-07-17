using System; using Xunit; using task14;
namespace task14tests;
public class IntegralTests {
    [Fact] public void Solve_LinearFunctionSymmetricRange_ReturnsZero() {
        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, x => x, 1e-4, 2), 4);
    }
}
