using System; using System.Collections.Concurrent; using System.Threading;
namespace task18;
public interface ICommand { void Execute(); bool IsCompleted { get; } }
public interface IScheduler { bool HasCommand(); ICommand Select(); void Add(ICommand cmd); }
public class RoundRobinScheduler : IScheduler {
    private readonly ConcurrentQueue<ICommand> _q = new();
    public bool HasCommand() => !_q.IsEmpty;
    public void Add(ICommand cmd) => _q.Enqueue(cmd);
    public ICommand Select() => _q.TryDequeue(out var cmd) ? cmd : throw new InvalidOperationException();
}
public class SchedulerServerThread {
    private readonly BlockingCollection<ICommand> _ext = new(); private readonly IScheduler _sch; private readonly Thread _th; private bool _stop = false;
    public Thread UnderlyingThread => _th;
    public SchedulerServerThread(IScheduler sch) { _sch = sch; _th = new Thread(() => {
        while (!_stop) {
            while (_ext.TryTake(out var n)) _sch.Add(n);
            if (_sch.HasCommand()) { var c = _sch.Select(); try { c.Execute(); } catch {} if (!c.IsCompleted) _sch.Add(c); Thread.Sleep(1); }
            else { try { if (_ext.TryTake(out var f, Timeout.Infinite)) _sch.Add(f); } catch { break; } }
        }
    });}
    public void Start() => _th.Start(); public void Stop() => _stop = true;
    public void AddCommand(ICommand c) => _ext.Add(c);
}
