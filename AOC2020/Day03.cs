using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day03 : DayBase, ITwoPartQuestion
    {
        public List<string> Lines = new List<string>();

        public Day03()
        {
            Lines = (from line in File.ReadAllLines("Input\\Day03.txt")
                     select line).ToList();

            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            var map = new Map(Lines);
            var sled = new Sled(map, 3, 1);
            var treesHit = sled.SlideDownMap();

            return $"Trees hit : {treesHit}";
        }

        public string Part2()
        {
            var map = new Map(Lines);
            var sleds = new Sled[5] {
                new Sled(map, 1, 1),
                new Sled(map, 3, 1),
                new Sled(map, 5, 1),
                new Sled(map, 7, 1),
                new Sled(map, 1, 2)
            };

            long result = 1;
            foreach (var sled in sleds)
                 result *= sled.SlideDownMap();

            return $"Combination of Trees hit : {result}";
        }

        public class Map
        {
            public int[,] Coords { get; set; }
            public int Width => Coords.GetLength(0);
            public int Height => Coords.GetLength(1);

            public Map(List<string> lines)
            {
                Coords = new int[lines[0].Length, lines.Count];

                for (int x = 0; x < lines[0].Length; x++)
                    for (int y = 0; y < lines.Count; y++)
                        Coords[x, y] = lines[y][x] == '#' ? 1 : 0;
            }
        }

        public class Sled
        {
            public int x = 0;
            public int y = 0;
            private readonly int right = 0;
            private readonly int down = 0;
            private readonly Map map;
            private bool OnTheMap => y < map.Height;

            public Sled(Map m, int Right, int Down)
            {
                map = m;
                right = Right;
                down = Down;
            }

            private int Move(int right, int down)
            {
                x += right;
                y += down;

                if (x >= map.Width)
                    x -= map.Width;

                if (OnTheMap)
                    return map.Coords[x, y];

                return 0;
            }

            public int SlideDownMap()
            {
                var treesHit = 0;

                while (OnTheMap)
                    treesHit += Move(right, down);

                return treesHit;
            }
        }
    }
}