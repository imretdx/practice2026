using System; using System.Collections.Concurrent; using System.Threading;
namespace task17;
public interface ICommand { void Execute(); }
public class ServerThread {
    private readonly BlockingCollection<ICommand> _queue = new(); private readonly Thread _thread; private bool _stop = false;
    public Thread UnderlyingThread => _thread;
    public ServerThread() => _thread = new Thread(() => {
        while (!_stop) { if (_queue.TryTake(out var cmd, Timeout.Infinite)) { try { cmd.Execute(); } catch {} } }
    });
    public void Start() => _thread.Start();
    public void AddCommand(ICommand cmd) => _queue.Add(cmd);
    public void HardStop() { _stop = true; _queue.CompleteAdding(); }
    public void SoftStop() { _queue.CompleteAdding(); while (_queue.TryTake(out var cmd)) { try { cmd.Execute(); } catch {} } _stop = true; }
}
public class HardStopCommand : ICommand {
    private readonly ServerThread _srv; public HardStopCommand(ServerThread srv) => _srv = srv;
    public void Execute() { if (Thread.CurrentThread != _srv.UnderlyingThread) throw new InvalidOperationException(); _srv.HardStop(); }
}
public class SoftStopCommand : ICommand {
    private readonly ServerThread _srv; public SoftStopCommand(ServerThread srv) => _srv = srv;
    public void Execute() { if (Thread.CurrentThread != _srv.UnderlyingThread) throw new InvalidOperationException(); _srv.SoftStop(); }
}
