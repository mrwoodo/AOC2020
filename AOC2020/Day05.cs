using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day05 : DayBase, ITwoPartQuestion
    {
        public List<string> Input = new List<string>();
        public SortedSet<int> Tickets = new SortedSet<int>();

        public Day05()
        {
            Input = (from line in File.ReadAllLines("Input\\Day05.txt")
                     select line).ToList();

            Console.WriteLine(Part1());
            Console.WriteLine(Part2());
        }

        public string Part1()
        {
            foreach (var ticket in Input)
                Tickets.Add(GetSeatID(
                    rowPartition: ticket.Substring(0, 7),
                    seatPartition: ticket[7..]));

            return $"Highest ticket number : {Tickets.Max()}";
        }

        public string Part2()
        {
            var check = new Dictionary<int, bool>();

            //Whole range of tickets sold
            foreach (var seat in Enumerable.Range(Tickets.First(), Tickets.LastOrDefault()))
                check.Add(seat, false);

            //Tickets checked in
            foreach (var t in Tickets)
                check[t] = true;

            //Our ticket that wasn't checked in
            return $"Our ticket : {check.Where(i => !i.Value).ElementAt(0).Key}";
        }

        private int GetSeatID(string rowPartition, string seatPartition)
        {
            var rowNum = GetNumberFromPartition(rowPartition, 'F');
            var seatNum = GetNumberFromPartition(seatPartition, 'L');

            return rowNum * 8 + seatNum;
        }

        private int GetNumberFromPartition(string partition, char low)
        {
            int min = 0;
            int max = Convert.ToInt32(Math.Pow(2.0, (double)partition.Length) - 1);

            foreach (var c in partition)
            {
                if (c == low)
                    max = (max - min) / 2 + min;
                else
                    min = (max - min) / 2 + min + 1;
            }

            return min;
        }
    }
}