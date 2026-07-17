using System; using System.Diagnostics; using System.IO; using task14;
namespace task15;
internal class Program {
    public static double SolveSingleThread(double a, double b, Func<double, double> func, double step) {
        double sum = 0.0; double x = a;
        while (x < b) { double nextX = x + step; if (nextX > b) nextX = b; sum += (func(x) + func(nextX)) * (nextX - x) / 2.0; x = nextX; }
        return sum;
    }
    private static void Main() {
        double step = 1e-4; Stopwatch sw = Stopwatch.StartNew();
        SolveSingleThread(-100, 100, Math.Sin, step); sw.Stop();
        long t1 = sw.ElapsedMilliseconds; sw = Stopwatch.StartNew();
        DefiniteIntegral.Solve(-100, 100, Math.Sin, step, 4); sw.Stop();
        long t4 = sw.ElapsedMilliseconds;
        File.WriteAllText("performance_report.txt", $"Шаг: {step}\nОднопоток: {t1} мс\nМногопоток (4): {t4} мс");
        Console.WriteLine("Замеры завершены и сохранены.");
    }
}
