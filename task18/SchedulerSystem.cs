using System;
using System.Collections.Concurrent;
using System.Threading;

namespace task18;

public interface ICommand
{
    void Execute();
    bool IsCompleted { get; }
}

public interface IScheduler
{
    bool HasCommand();
    ICommand Select();
    void Add(ICommand cmd);
}

public class RoundRobinScheduler : IScheduler
{
    private readonly ConcurrentQueue<ICommand> _q = new ConcurrentQueue<ICommand>();

    public bool HasCommand()
    {
        return !_q.IsEmpty;
    }

    public void Add(ICommand cmd)
    {
        _q.Enqueue(cmd);
    }

    public ICommand Select()
    {
        if (_q.TryDequeue(out var cmd))
        {
            return cmd;
        }
        throw new InvalidOperationException("В планировщике отсутствуют доступные команды.");
    }
}

public class SchedulerServerThread
{
    private readonly BlockingCollection<ICommand> _ext = new BlockingCollection<ICommand>();
    private readonly IScheduler _sch;
    private readonly Thread _th;
    private bool _stop = false;

    public Thread UnderlyingThread
    {
        get 
        { 
            return _th; 
        }
    }

    public SchedulerServerThread(IScheduler sch)
    {
        _sch = sch;
        _th = new Thread(new ThreadStart(ExecuteLoopProcessing));
    }

    public void Start()
    {
        _th.Start();
    }

    public void Stop()
    {
        _stop = true;
    }

    public void AddCommand(ICommand c)
    {
        _ext.Add(c);
    }

    private void ExecuteLoopProcessing()
    {
        while (!_stop)
        {
            // Переносим поступившие извне команды в циклический планировщик
            while (_ext.TryTake(out var newCommand))
            {
                _sch.Add(newCommand);
            }

            if (_sch.HasCommand())
            {
                var currentCommand = _sch.Select();
                try
                {
                    currentCommand.Execute();
                }
                catch (Exception)
                {
                    // Обработка непредвиденных сбоев
                }

                if (!currentCommand.IsCompleted)
                {
                    _sch.Add(currentCommand);
                }
                
                Thread.Sleep(1);
            }
            else
            {
                try
                {
                    if (_ext.TryTake(out var firstCommand, Timeout.Infinite))
                    {
                        _sch.Add(firstCommand);
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }
        }
    }
}
