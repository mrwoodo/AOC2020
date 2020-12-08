using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day08 : DayBase, ITwoPartQuestion
    {
        private readonly List<string> Disk;
        private string[] Memory;
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
            try
            {
                Memory = new string[Disk.Count];
                Disk.CopyTo(Memory);
                RunProgram();
            }
            catch (InfiniteLoopException ex)
            {
                return ex.Message;
            }

            return "Failed";
        }

        public string Part2()
        {
            try
            {
                for (int i = 0; i < Disk.Count; i++)
                {
                    Disk.CopyTo(Memory);
                    var somethingChanged = false;

                    if (Memory[i].StartsWith("j"))
                    {
                        Memory[i] = Memory[i].Replace("jmp", "nop");
                        somethingChanged = true;
                    }
                    else if (Memory[i].StartsWith("n"))
                    {
                        Memory[i] = Memory[i].Replace("nop", "jmp");
                        somethingChanged = true;
                    }

                    if (somethingChanged)
                    {
                        try
                        {
                            RunProgram();
                        }
                        catch (InfiniteLoopException)
                        {
                            //Ignore these attempts at running
                        }
                    }
                }
            }
            catch (OutOfProgramException ex)
            {
                return ex.Message;
            }

            return "Failed";
        }

        private void RunProgram()
        {
            LineVisited = new HashSet<int>();
            Curr = Acc = 0;

            while (true)
            {
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
            if (lineNum >= Memory.Length)
                throw new OutOfProgramException($"Out of loop, Accumulator = {Acc}");

            if (LineVisited.Contains(lineNum))
                throw new InfiniteLoopException($"Infinite loop, Accumulator = {Acc}");

            LineVisited.Add(lineNum);
            var line = Memory[lineNum].Split(" ");

            return (line[0].Trim(), int.Parse(line[1]));
        }
    }

    public class InfiniteLoopException : Exception
    {
        public InfiniteLoopException() { }
        public InfiniteLoopException(string message) : base(message) { }
        public InfiniteLoopException(string message, Exception inner) : base(message, inner) { }
    }

    public class OutOfProgramException : Exception
    {
        public OutOfProgramException() { }
        public OutOfProgramException(string message) : base(message) { }
        public OutOfProgramException(string message, Exception inner) : base(message, inner) { }
    }
}