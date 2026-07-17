using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using task19;

namespace task19tests;

public class AdvancedSchedulerTests
{
    [Fact]
    public void Scheduler_ShouldExecuteFiveCommandsThreeTimesAndStop()
    {
        var server = new AdvancedSchedulerServerThread();
        
        var cmds = Enumerable.Range(1, 5)
            .Select(id => new TestCommand(id))
            .ToList();

        cmds.ForEach(c => server.AddCommand(c));
        server.Start();

        // Вместо слепого ожидания Sleep, честно ждем выполнения условий (квантования шагов)
        int timeoutCounter = 0;
        while (cmds.Any(c => c.Counter < 3) && timeoutCounter < 100)
        {
            Thread.Sleep(10);
            timeoutCounter++;
        }

        // Теперь все команды гарантированно успели выполниться по 3 раза, посылаем HardStop
        server.AddCommand(new HardStopCommand(server));
        server.UnderlyingThread.Join(500);

        Assert.All(cmds, c => Assert.Equal(3, c.Counter));
    }
}
