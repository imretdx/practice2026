using System.Threading; using System.Collections.Generic; using Xunit; using task18;
namespace task18tests;
public class SchedulerTests {
    private class LongCmd : ICommand {
        private readonly string _n; private readonly List<string> _l; private int _s = 0;
        public bool IsCompleted => _s >= 2;
        public LongCmd(string n, List<string> l) { _n = n; _l = l; }
        public void Execute() { _s++; lock(_l) { _l.Add($"{_n}{_s}"); } }
    }
    [Fact] public void Scheduler_ShouldExecuteInRoundRobin() {
        var log = new List<string>(); var srv = new SchedulerServerThread(new RoundRobinScheduler());
        srv.AddCommand(new LongCmd("A", log)); srv.AddCommand(new LongCmd("B", log));
        srv.Start(); Thread.Sleep(100); srv.Stop(); srv.UnderlyingThread.Join(500);
        Assert.Equal(4, log.Count);
    }
}
