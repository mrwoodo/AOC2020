using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day08 : DayBase, ITwoPartQuestion
    {
        private readonly List<string> Disk;
        private string[] MemoryStore;
        private HashSet<int> LineVisited;
        private int Curr;
        private int Acc;

        public Day08()
        {
            Disk = (from line in File.ReadAllLines("Input\\Day08.txt")
                    select line).ToList();

            Console.WriteLine(Part1());
            Console.WriteLine(Part2());
        }

        public string Part1()
        {
            MemoryStore = new string[Disk.Count];
            Disk.CopyTo(MemoryStore);
            RunProgram();

            return $"Part 1 Acc : {Acc}";
        }

        public string Part2()
        {
            //working backwards, assuming the corrupt code was closer to the end of the block
            for (int i = Disk.Count - 1; i >= 0; i--)
            {
                Disk.CopyTo(MemoryStore);
                var somethingChanged = true;

                if (MemoryStore[i].StartsWith("j"))
                    MemoryStore[i] = MemoryStore[i].Replace("jmp", "nop");
                else if (MemoryStore[i].StartsWith("n"))
                    MemoryStore[i] = MemoryStore[i].Replace("nop", "jmp");
                else
                    somethingChanged = false;

                if (somethingChanged)
                    if (RunProgram())
                        return $"Part 2 Acc : {Acc}";
            }

            return $"Part 2 Acc : {Acc}";
        }

        private bool RunProgram()
        {
            LineVisited = new HashSet<int>();
            Curr = Acc = 0;

            while (true)
            {
                if (Curr >= MemoryStore.Length)
                    return true; //reached end of code block

                if (LineVisited.Contains(Curr))
                    return false; //infinite loop

                LineVisited.Add(Curr);

                var ins = GetIntruction(Curr);

                switch (ins.Item1)
                {
                    case "nop":
                        Curr++;
                        break;
                    case "jmp":
                        Curr += ins.Item2;
                        break;
                    case "acc":
                        Acc += ins.Item2;
                        Curr++;
                        break;
                }
            }
        }

        private (string, int) GetIntruction(int lineNum)
        {
            var line = MemoryStore[lineNum].Split(" ");

            return (line[0].Trim(), int.Parse(line[1]));
        }
    }
}