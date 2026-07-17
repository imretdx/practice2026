using System; using System.Collections.Concurrent; using System.Threading;
namespace task19;
public interface ICommand { void Execute(); }
public class TestCommand : ICommand {
    private readonly int _id; public int Counter { get; private set; } = 0;
    public TestCommand(int id) => _id = id;
    public void Execute() { Counter++; Console.WriteLine($"Поток {_id} вызов {Counter}"); }
}
public class HardStopCommand : ICommand {
    private readonly AdvancedSchedulerServerThread _srv; public HardStopCommand(AdvancedSchedulerServerThread srv) => _srv = srv;
    public void Execute() => _srv.HardStop();
}
public class AdvancedSchedulerServerThread {
    private readonly BlockingCollection<ICommand> _ext = new(); private readonly ConcurrentQueue<ICommand> _sch = new(); private readonly Thread _th; private bool _stop = false;
    public Thread UnderlyingThread => _th;
    public AdvancedSchedulerServerThread() => _th = new Thread(() => {
        while (!_stop) {
            while (_ext.TryTake(out var n)) _sch.Enqueue(n);
            if (!_sch.IsEmpty) {
                if (_sch.TryDequeue(out var c)) {
                    try { c.Execute(); } catch {}
                    if (_stop) break;
                    if (c is TestCommand t && t.Counter < 3) _sch.Enqueue(t);
                }
                Thread.Sleep(1);
            }
            else { try { if (_ext.TryTake(out var f, Timeout.Infinite)) _sch.Enqueue(f); } catch { break; } }
        }
    });
    public void Start() => _th.Start(); public void HardStop() => _stop = true;
    public void AddCommand(ICommand c) => _ext.Add(c);
}
