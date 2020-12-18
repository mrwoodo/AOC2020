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
            Lines = (from line in File.ReadAllLines("Input\\Day17.txt")
                     select line).ToList();

            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            var world = new World3D(Lines);
            world.Play(6);

            return $"3D world Active Points: {world.ActivePoints}";
        }

        public string Part2()
        {
            var world = new World4D(Lines);
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

            public Point(int x, int y, int z, int a, bool active = false)
            {
                X = x;
                Y = y;
                Z = z;
                A = a;
                Active = active;
                NewActive = false;
                StateChanged = false;
            }

        }

        public class World3D
        {
            private readonly List<Point> Points = new List<Point>();
            public int ActivePoints => Points.Count(i => i.Active);
            private readonly Dictionary<string, int> PointCache = new Dictionary<string, int>();
            private int Xmin = 0;
            private int Xmax = 0;
            private int Ymin = 0;
            private int Ymax = 0;
            private int Zmin = 0;
            private int Zmax = 0;

            public World3D()
            {
                Points = new List<Point>();
            }

            public World3D(List<string> input) : base()
            {
                for (int y = 0; y < input.Count; y++)
                    for (int x = 0; x < input[y].Length; x++)
                        AddPoint(new Point(x, -y, 0, input[y][x] == '#'), true);
            }

            public void Play(int turns)
            {
                for (var turn = 1; turn <= turns; turn++)
                {
                    for (int x = Xmin - 1; x <= Xmax + 1; x++)
                        for (int y = Ymin - 1; y <= Ymax + 1; y++)
                            for (int z = Zmin - 1; z <= Zmax + 1; z++)
                                GetPoint(x, y, z, false);

                    var count = Points.Count;

                    for (int i = 0; i < count; i++)
                    {
                        var p = Points[i];
                        var neighbours = GetNeighbours(p);
                        var activeNeighbours = neighbours.Count(i => i.Active);

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

                    foreach (var p in Points.Where(i => i.StateChanged))
                    {
                        p.Active = p.NewActive;
                        p.StateChanged = false;
                    }
                }
            }

            private Point[] GetNeighbours(Point p)
            {
                var result = new Point[26];
                var counter = 0;

                for (int x = p.X - 1; x <= p.X + 1; x++)
                {
                    for (int y = p.Y - 1; y <= p.Y + 1; y++)
                    {
                        for (int z = p.Z - 1; z <= p.Z + 1; z++)
                        {
                            if ((x == p.X) && (y == p.Y) && (z == p.Z))
                            { }
                            else
                            {
                                result[counter] = GetPoint(x, y, z, false);
                                counter++;
                            }
                        }
                    }
                }

                return result;
            }

            private Point GetPoint(int x, int y, int z, bool updateBoundary)
            {
                var cacheKey = $"{x},{y},{z}";
                if (PointCache.ContainsKey(cacheKey))
                    return Points[PointCache[cacheKey]];

                var search = Points.Where(i => ((i.X == x) && (i.Y == y) && (i.Z == z))).FirstOrDefault();
                if (search == null)
                {
                    search = new Point(x, y, z);
                    AddPoint(search, updateBoundary);
                    PointCache[cacheKey] = Points.Count - 1;
                }

                return search;
            }

            private void AddPoint(Point p, bool updateBoundary)
            {
                Points.Add(p);

                if (updateBoundary)
                {
                    if (p.X < Xmin) Xmin = p.X;
                    if (p.X > Xmax) Xmax = p.X;
                    if (p.Y < Ymin) Ymin = p.Y;
                    if (p.Y > Ymax) Ymax = p.Y;
                    if (p.Z < Zmin) Zmin = p.Z;
                    if (p.Z > Zmax) Zmax = p.Z;
                }
            }
        }

        public class World4D
        {
            private readonly List<Point> Points = new List<Point>();
            public int ActivePoints => Points.Count(i => i.Active);
            private readonly Dictionary<string, int> PointCache = new Dictionary<string, int>();
            private int Xmin = 0;
            private int Xmax = 0;
            private int Ymin = 0;
            private int Ymax = 0;
            private int Zmin = 0;
            private int Zmax = 0;
            private int Amin = 0;
            private int Amax = 0;

            public World4D()
            {
                Points = new List<Point>();
            }

            public World4D(List<string> input) : base()
            {
                for (int y = 0; y < input.Count; y++)
                    for (int x = 0; x < input[y].Length; x++)
                        AddPoint(new Point(x, -y, 0, 0, input[y][x] == '#'), true);
            }

            public void Play(int turns)
            {
                for (var turn = 1; turn <= turns; turn++)
                {
                    for (int x = Xmin - 1; x <= Xmax + 1; x++)
                        for (int y = Ymin - 1; y <= Ymax + 1; y++)
                            for (int z = Zmin - 1; z <= Zmax + 1; z++)
                                for (int a = Amin - 1; a <= Amax + 1; a++)
                                    GetPoint(x, y, z, a, false);

                    var count = Points.Count;

                    for (int i = 0; i < count; i++)
                    {
                        var p = Points[i];
                        var neighbours = GetNeighbours(p);
                        var activeNeighbours = neighbours.Count(i => i.Active);

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

                    foreach (var p in Points.Where(i => i.StateChanged))
                    {
                        p.Active = p.NewActive;
                        p.StateChanged = false;
                    }
                }
            }

            private Point[] GetNeighbours(Point p)
            {
                var result = new Point[80];
                var counter = 0;

                for (int x = p.X - 1; x <= p.X + 1; x++)
                {
                    for (int y = p.Y - 1; y <= p.Y + 1; y++)
                    {
                        for (int z = p.Z - 1; z <= p.Z + 1; z++)
                        {
                            for (int a = p.A - 1; a <= p.A + 1; a++)
                            {
                                if ((x == p.X) && (y == p.Y) && (z == p.Z) && (a == p.A))
                                { }
                                else
                                {
                                    result[counter] = GetPoint(x, y, z, a, false);
                                    counter++;
                                }
                            }
                        }
                    }
                }

                return result;
            }

            private Point GetPoint(int x, int y, int z, int a, bool updateBoundary)
            {
                var cacheKey = $"{x},{y},{z},{a}";
                if (PointCache.ContainsKey(cacheKey))
                    return Points[PointCache[cacheKey]];

                var search = Points.Where(i => ((i.X == x) && (i.Y == y) && (i.Z == z) && (i.A == a))).FirstOrDefault();
                if (search == null)
                {
                    search = new Point(x, y, z, a);
                    AddPoint(search, updateBoundary);
                    PointCache[cacheKey] = Points.Count - 1;
                }

                return search;
            }

            private void AddPoint(Point p, bool updateBoundary)
            {
                Points.Add(p);

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
        }
    }
}