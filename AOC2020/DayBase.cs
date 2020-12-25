using System;
using System.Diagnostics;
using System.IO;

namespace AOC2020
{
    public class DayBase
    {
        public DayBase()
        {
            Console.WriteLine("".PadLeft(80, '-'));
            Console.WriteLine($"{this.GetType().Name}\r\n");
        }

        public string InputFile => File.ReadAllText($"Input\\{this.GetType().Name}.txt");

        public void Run(Func<string> part1, Func<string> part2)
        {
            var s = new Stopwatch();

            s.Start();
            var part1Answer = part1();
            s.Stop();
            Console.WriteLine($"Part 1....{part1Answer} (took {s.ElapsedMilliseconds}ms)");

            s.Reset();
            s.Start();
            var part2Answer = part2();
            s.Stop();
            Console.WriteLine($"Part 2....{part2Answer} (took {s.ElapsedMilliseconds}ms)");

            if ((part1Answer.Length == 0) || (part2Answer.Length == 0))
                Console.WriteLine("!! IMCOMPLETE !!");
        }
    }
}