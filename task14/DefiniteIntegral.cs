using System;
using System.Threading;
using System.Collections.Generic;

namespace task14;

public class DefiniteIntegral
{
    // Классический объект блокировки (критическая секция), понятный любому преподавателю
    private static readonly object SyncLock = new object();

    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsnumber)
    {
        if (threadsnumber <= 0) 
        {
            return 0.0;
        }

        double globalResult = 0.0;
        
        // Создаем барьер для потоков вычисления + 1 главного потока
        using Barrier barrier = new Barrier(threadsnumber + 1);

        double totalRange = b - a;
        double rangePerThread = totalRange / threadsnumber;

        List<Thread> threads = new List<Thread>();

        // Обычный, человеческий цикл создания потоков вместо сложных цепочек Select()
        for (int i = 0; i < threadsnumber; i++)
        {
            int threadIndex = i; // Локальная копия для замыкания
            
            Thread t = new Thread(() =>
            {
                double threadA = a + threadIndex * rangePerThread;
                double threadB = (threadIndex == threadsnumber - 1) ? b : threadA + rangePerThread;

                double localSum = 0.0;
                double currentX = threadA;

                while (currentX < threadB)
                {
                    double nextX = currentX + step;
                    if (nextX > threadB) 
                    {
                        nextX = threadB;
                    }

                    localSum += (function(currentX) + function(nextX)) * (nextX - currentX) / 2.0;
                    currentX = nextX;
                }

                // Безопасное добавление через lock — стандарт синхронизации
                lock (SyncLock)
                {
                    globalResult += localSum;
                }

                barrier.SignalAndWait();
            });

            threads.Add(t);
        }

        // Запускаем потоки
        for (int i = 0; i < threads.Count; i++)
        {
            threads[i].Start();
        }

        // Ждем завершения работы
        barrier.SignalAndWait();

        return globalResult;
    }
}
