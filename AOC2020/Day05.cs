using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day05 : DayBase, ITwoPartQuestion
    {
        public List<string> Lines = new List<string>();
        public List<int> SeatIDs = new List<int>();

        public Day05()
        {
            Lines = (from line in File.ReadAllLines("Input\\Day05.txt")
                     select line).ToList();

            Console.WriteLine(Part1());
            Console.WriteLine(Part2());
        }

        public string Part1()
        {
            foreach (var ticket in Lines)
                SeatIDs.Add(GetSeatID(
                    rowPartition: ticket.Substring(0, 7), 
                    seatPartition: ticket[7..]));

            return SeatIDs.Max().ToString();
        }

        public string Part2()
        {
            var sorted = SeatIDs.OrderBy(x => x).ToList();

            for (int i = 0; i < sorted.Count; i++)
                if ((sorted[i] + 1) != sorted[i + 1])
                    return (sorted[i] + 1).ToString();

            return String.Empty;
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
            int max = Convert.ToInt32(Math.Pow((double)2, (double)partition.Length) - 1);

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