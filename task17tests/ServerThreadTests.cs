using System; using System.Threading; using Xunit; using task17;
namespace task17tests;
public class ServerThreadTests {
    private class MockCmd : ICommand { public bool Done { get; private set; } public void Execute() => Done = true; }
    [Fact] public void HardStop_ShouldStopImmediately() {
        var srv = new ServerThread(); var c1 = new MockCmd(); var c2 = new MockCmd();
        srv.AddCommand(c1); srv.AddCommand(new HardStopCommand(srv)); srv.AddCommand(c2);
        srv.Start(); srv.UnderlyingThread.Join(1000);
        Assert.True(c1.Done); Assert.False(c2.Done);
    }
}
