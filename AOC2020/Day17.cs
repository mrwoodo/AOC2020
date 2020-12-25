using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day17 : DayBase, ITwoPartQuestion
    {
        public List<string> Lines = new List<string>();

        public Day17()
        {
            Lines = InputFile.Split("\r\n").ToList();

            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            var world = new World(Lines, fourD: false);
            world.Play(6);

            return $"3D world Active Points: {world.ActivePoints}";
        }

        public string Part2()
        {
            var world = new World(Lines, fourD: true);
            world.Play(6);

            return $"4D world Active Points: {world.ActivePoints}";
        }

        public class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public int A { get; set; }
            public bool Active { get; set; }
            public bool NewActive { get; set; }
            public bool StateChanged { get; set; }

            public Point(int x, int y, int z, bool active = false)
            {
                X = x;
                Y = y;
                Z = z;
                Active = active;
                NewActive = false;
                StateChanged = false;
            }

            public Point(int x, int y, int z, int a, bool active = false) : this(x, y, z, active)
            {
                A = a;
            }
        }

        public class World
        {
            //Key in the Points dict is {x},{y},{z},{a}.
            private readonly Dictionary<string, Point> Points = new Dictionary<string, Point>();
            public int ActivePoints => Points.Values.Count(i => i.Active);
            private int Xmin = 0;
            private int Xmax = 0;
            private int Ymin = 0;
            private int Ymax = 0;
            private int Zmin = 0;
            private int Zmax = 0;
            private int Amin = 0;
            private int Amax = 0;
            private bool FourD = false;

            public World(bool fourD)
            {
                FourD = fourD;
            }

            public World(List<string> input, bool fourD)
            {
                FourD = fourD;
                for (int y = 0; y < input.Count; y++)
                {
                    for (int x = 0; x < input[y].Length; x++)
                    {
                        var p = GetPoint(x, -y, 0, 0, updateBoundary: true);
                        p.Active = input[y][x] == '#';
                    }
                }
            }

            public void Play(int turns)
            {
                for (var turn = 1; turn <= turns; turn++)
                {
                    //Make sure the world is at least 1 unit larger on all sides of our known points
                    //This is because some of these edge points might become active in this turn
                    for (int x = Xmin - 1; x <= Xmax + 1; x++)
                        for (int y = Ymin - 1; y <= Ymax + 1; y++)
                            for (int z = Zmin - 1; z <= Zmax + 1; z++)
                                for (int a = (FourD ? (Amin - 1) : 0); a <= (FourD ? (Amax + 1) : 0); a++)
                                    GetPoint(x, y, z, a);

                    var count = Points.Count;

                    for (int i = 0; i < count; i++)
                    {
                        var p = Points.ElementAt(i).Value;
                        var activeNeighbours = GetNeighbours(p);

                        //If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active.Otherwise, the cube becomes inactive.
                        if (p.Active && ((activeNeighbours < 2) || (activeNeighbours > 3)))
                        {
                            p.NewActive = false;
                            p.StateChanged = true;
                        }

                        //If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active. Otherwise, the cube remains inactive.
                        if (!p.Active && (activeNeighbours == 3))
                        {
                            p.NewActive = true;
                            p.StateChanged = true;
                        }
                    }

                    //Change the state of points only at the end, otherwise it will affect the rules above
                    foreach (var p in Points.Values.Where(i => i.StateChanged))
                    {
                        p.Active = p.NewActive;
                        p.StateChanged = false;
                    }
                }
            }

            private int GetNeighbours(Point p)
            {
                var counter = 0;
                var Active = 0;

                for (int x = p.X - 1; x <= p.X + 1; x++)
                {
                    for (int y = p.Y - 1; y <= p.Y + 1; y++)
                    {
                        for (int z = p.Z - 1; z <= p.Z + 1; z++)
                        {
                            for (int a = (FourD ? (p.A - 1) : 0); a <= (FourD ? (p.A + 1) : 0); a++)
                            {
                                if ((x == p.X) && (y == p.Y) && (z == p.Z) && (a == p.A))
                                { }
                                else
                                {
                                    var n = GetPoint(x, y, z, a);

                                    if (n.Active)
                                        Active++;
                                    counter++;
                                }
                            }
                        }
                    }
                }

                return Active;
            }

            private Point GetPoint(int x, int y, int z, int a, bool updateBoundary = false)
            {
                var key = $"{x},{y},{z},{a}";

                if (!Points.ContainsKey(key))
                {
                    var p = new Point(x, y, z, a);

                    Points[key] = p;

                    if (updateBoundary)
                    {
                        if (p.X < Xmin) Xmin = p.X;
                        if (p.X > Xmax) Xmax = p.X;
                        if (p.Y < Ymin) Ymin = p.Y;
                        if (p.Y > Ymax) Ymax = p.Y;
                        if (p.Z < Zmin) Zmin = p.Z;
                        if (p.Z > Zmax) Zmax = p.Z;
                        if (p.A < Amin) Amin = p.A;
                        if (p.A > Amax) Amax = p.A;
                    }
                }

                return Points[key];
            }
        }
    }
}