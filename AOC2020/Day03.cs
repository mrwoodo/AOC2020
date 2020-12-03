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

            Console.WriteLine(Part1());
            Console.WriteLine(Part2());
        }

        public string Part1()
        {
            var world = new World(Lines);
            var sled = new Sled(world, 3, 1);
            var treesHit = sled.Slide();

            return treesHit.ToString();
        }

        public string Part2()
        {
            var world = new World(Lines);
            var sleds = new Sled[5] { 
                new Sled(world, 1, 1),
                new Sled(world, 3, 1),
                new Sled(world, 5, 1),
                new Sled(world, 7, 1),
                new Sled(world, 1, 2) 
            };

            long result = 1;
            foreach (var sled in sleds)
                 result *= sled.Slide();

            return result.ToString();
        }

        public class World
        {
            public int[,] Map { get; set; }
            public int Width => Map.GetLength(0);
            public int Height => Map.GetLength(1);

            public World(List<string> lines)
            {
                Map = new int[lines[0].Length, lines.Count];

                for (int x = 0; x < lines[0].Length; x++)
                    for (int y = 0; y < lines.Count; y++)
                        Map[x, y] = lines[y][x] == '#' ? 1 : 0;
            }
        }

        public class Sled
        {
            public int x = 0;
            public int y = 0;
            private readonly int right = 0;
            private readonly int down = 0;
            private readonly World world;
            private bool OnTheMap => y < world.Height;

            public Sled(World w, int Right, int Down)
            {
                world = w;
                right = Right;
                down = Down;
            }

            private int Move(int right, int down)
            {
                x += right;
                y += down;

                if (x >= world.Width) 
                    x -= world.Width;

                if (OnTheMap)
                    return world.Map[x, y];

                return 0;
            }

            public int Slide()
            {
                var treesHit = 0;

                while (OnTheMap)
                    treesHit += Move(right, down);

                return treesHit;
            }
        }
    }
}