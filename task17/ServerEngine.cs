using System;
using System.Collections.Concurrent;
using System.Threading;

namespace task17;

public interface ICommand
{
    void Execute();
}

public class ServerThread
{
    private readonly BlockingCollection<ICommand> _queue = new BlockingCollection<ICommand>();
    private readonly Thread _thread;
    private bool _stop = false;

    public Thread UnderlyingThread
    {
        get 
        { 
            return _thread; 
        }
    }

    public ServerThread()
    {
        _thread = new Thread(new ThreadStart(ProcessQueueLoop));
    }

    public void Start()
    {
        _thread.Start();
    }

    public void AddCommand(ICommand cmd)
    {
        _queue.Add(cmd);
    }

    public void HardStop()
    {
        _stop = true;
        _queue.CompleteAdding();
    }

    public void SoftStop()
    {
        _queue.CompleteAdding();
        
        // Переписываем однострочник в понятный классический цикл извлечения команд
        while (_queue.TryTake(out var cmd))
        {
            try
            {
                cmd.Execute();
            }
            catch (Exception)
            {
                // Игнорируем исключения по логике ЛР
            }
        }
        _stop = true;
    }

    private void ProcessQueueLoop()
    {
        while (!_stop)
        {
            // Извлекаем команду блокирующим образом с защитой от сбоев
            if (_queue.TryTake(out var cmd, Timeout.Infinite))
            {
                try
                {
                    cmd.Execute();
                }
                catch (Exception)
                {
                    // Обработка исключений
                }
            }
        }
    }
}

public class HardStopCommand : ICommand
{
    private readonly ServerThread _srv;

    public HardStopCommand(ServerThread srv)
    {
        _srv = srv;
    }

    public void Execute()
    {
        if (Thread.CurrentThread != _srv.UnderlyingThread)
        {
            throw new InvalidOperationException("Выполнение разрешено только внутри целевого потока.");
        }
        _srv.HardStop();
    }
}

public class SoftStopCommand : ICommand
{
    private readonly ServerThread _srv;

    public SoftStopCommand(ServerThread srv)
    {
        _srv = srv;
    }

    public void Execute()
    {
        if (Thread.CurrentThread != _srv.UnderlyingThread)
        {
            throw new InvalidOperationException("Выполнение разрешено только внутри целевого потока.");
        }
        _srv.SoftStop();
    }
}
