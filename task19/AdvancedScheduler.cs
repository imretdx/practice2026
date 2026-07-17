using System;
using System.Collections.Concurrent;
using System.Threading;

namespace task19;

public interface ICommand
{
    void Execute();
}

public class TestCommand : ICommand
{
    private readonly int _id;
    private int _counter = 0;

    public int Counter
    {
        get 
        { 
            return _counter; 
        }
    }

    public TestCommand(int id)
    {
        _id = id;
    }

    public void Execute()
    {
        _counter++;
        Console.WriteLine($"Поток {_id} вызов {_counter}");
    }
}

public class HardStopCommand : ICommand
{
    private readonly AdvancedSchedulerServerThread _srv;

    public HardStopCommand(AdvancedSchedulerServerThread srv)
    {
        _srv = srv;
    }

    public void Execute()
    {
        _srv.HardStop();
    }
}

public class AdvancedSchedulerServerThread
{
    private readonly BlockingCollection<ICommand> _ext = new BlockingCollection<ICommand>();
    private readonly ConcurrentQueue<ICommand> _sch = new ConcurrentQueue<ICommand>();
    private readonly Thread _th;
    private bool _stop = false;

    public Thread UnderlyingThread
    {
        get 
        { 
            return _th; 
        }
    }

    public AdvancedSchedulerServerThread()
    {
        _th = new Thread(new ThreadStart(RunSchedulerLoop));
    }

    public void Start()
    {
        _th.Start();
    }

    public void HardStop()
    {
        _stop = true;
    }

    public void AddCommand(ICommand c)
    {
        _ext.Add(c);
    }

    private void RunSchedulerLoop()
    {
        while (!_stop)
        {
            while (_ext.TryTake(out var nextCommand))
            {
                _sch.Enqueue(nextCommand);
            }

            if (!_sch.IsEmpty)
            {
                if (_sch.TryDequeue(out var activeTask))
                {
                    try
                    {
                        activeTask.Execute();
                    }
                    catch (Exception)
                    {
                        // Сбой операции
                    }

                    if (_stop)
                    {
                        break;
                    }

                    if (activeTask is TestCommand testTask)
                    {
                        if (testTask.Counter < 3)
                        {
                            _sch.Enqueue(testTask);
                        }
                    }
                }
                Thread.Sleep(1);
            }
            else
            {
                try
                {
                    if (_ext.TryTake(out var standaloneCommand, Timeout.Infinite))
                    {
                        _sch.Enqueue(standaloneCommand);
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
