using System;
using System.Diagnostics;

namespace AOC2020
{
    public class DayBase
    {
        public DayBase()
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine($"{this.GetType().Name}\r\n");
        }

        public void Run(Func<string> part1, Func<string> part2)
        {
            var s = new Stopwatch();

            s.Start();
            var part1Answer = part1();
            s.Stop();
            Console.WriteLine($"Part 1....{part1Answer} (took {s.ElapsedMilliseconds}ms)");

            s.Start();
            var part2Answer = part2();
            s.Stop();
            Console.WriteLine($"Part 2....{part2Answer} (took {s.ElapsedMilliseconds}ms)");
        }
    }
}