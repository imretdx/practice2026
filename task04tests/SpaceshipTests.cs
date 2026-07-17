using Xunit;
using task04;
namespace task04tests;
public class SpaceshipTests
{
    [Fact] public void Cruiser_ShouldHaveCorrectStats() => Assert.Equal(50, new Cruiser().Speed);
    [Fact] public void Fighter_ShouldBeFasterThanCruiser() => Assert.True(new Fighter().Speed > new Cruiser().Speed);
}
