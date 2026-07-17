using System; using System.Threading; using System.Linq;
namespace task14;
public class DefiniteIntegral {
    private static void InterlockedAddDouble(ref double location, double value) {
        double newCurrentValue = location;
        while (true) {
            double currentValue = newCurrentValue; double newValue = currentValue + value;
            newCurrentValue = Interlocked.CompareExchange(ref location, newValue, currentValue);
            if (newCurrentValue == currentValue) break;
        }
    }
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsnumber) {
        if (threadsnumber <= 0) return 0.0;
        double globalResult = 0.0;
        using Barrier barrier = new Barrier(threadsnumber + 1);
        double rangePerThread = (b - a) / threadsnumber;
        var threads = Enumerable.Range(0, threadsnumber).Select(i => new Thread(() => {
            double threadA = a + i * rangePerThread;
            double threadB = (i == threadsnumber - 1) ? b : threadA + rangePerThread;
            double localSum = 0.0; double currentX = threadA;
            while (currentX < threadB) {
                double nextX = currentX + step; if (nextX > threadB) nextX = threadB;
                localSum += (function(currentX) + function(nextX)) * (nextX - currentX) / 2.0;
                currentX = nextX;
            }
            InterlockedAddDouble(ref globalResult, localSum);
            barrier.SignalAndWait();
        })).ToList();
        threads.ForEach(t => t.Start());
        barrier.SignalAndWait();
        return globalResult;
    }
}
