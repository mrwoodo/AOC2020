using System.Collections.Generic;
using System.IO;

namespace AOC2020
{
    public class Day24 : DayBase, ITwoPartQuestion
    {
        private readonly Lobby lobby = new Lobby();

        public Day24()
        {
            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            var blackTiles = lobby.FlipTiles(InputFileAsStringList);

            return $"Black Tiles = {blackTiles}";
        }

        public string Part2()
        {
            var days = 100;
            var blackTiles = lobby.ArtExhibit(days);

            return $"After {days} days, Black Tiles = {blackTiles}";
        }

        public class Lobby
        {
            private const int SIZE = 250;
            private readonly Tile[,] Tiles;
            private readonly int min;
            private readonly int max;

            public Lobby()
            {
                Tiles = new Tile[SIZE, SIZE];
                min = -SIZE / 2;
                max = -min;

                for (int y = min; y < max; y++)
                {
                    for (int x = min; x < max; x++)
                    {
                        //Using this approach, half the items in the array end up null
                        //That's why later we keep doing null checks
                        if (((x % 2 == 0) && (y % 2 == 0)) || ((x % 2 != 0) && (y % 2 != 0)))
                            Tiles[x + max, y + max] = new Tile();
                    }
                }

                LayTiles();
            }

            private void LayTiles()
            {
                for (int y = min + 2; y < max - 2; y++)
                {
                    for (int x = min + 2; x < max - 2; x++)
                    {
                        var t = Tiles[x + max, y + max];

                        if (t != null)
                        {
                            t.E = Tiles[x + max + 2, y + max];
                            t.W = Tiles[x + max - 2, y + max];
                            t.NE = Tiles[x + max + 1, y + max - 1];
                            t.SE = Tiles[x + max + 1, y + max + 1];
                            t.NW = Tiles[x + max - 1, y + max - 1];
                            t.SW = Tiles[x + max - 1, y + max + 1];
                        }
                    }
                }
            }

            public int FlipTiles(List<string> Instructions)
            {
                int blackTiles = 0;

                foreach (var line in Instructions)
                {
                    var current = Tiles[max, max];
                    var path = line;

                    while (path.Length > 0)
                    {
                        if ((path.Length > 1) && (path.Substring(0, 2) == "se"))
                        {
                            current = current.SE;
                            path = path[2..];
                        }
                        else if ((path.Length > 1) && (path.Substring(0, 2) == "sw"))
                        {
                            current = current.SW;
                            path = path[2..];
                        }
                        else if ((path.Length > 1) && (path.Substring(0, 2) == "nw"))
                        {
                            current = current.NW;
                            path = path[2..];
                        }
                        else if ((path.Length > 1) && (path.Substring(0, 2) == "ne"))
                        {
                            current = current.NE;
                            path = path[2..];
                        }
                        else if (path.Substring(0, 1) == "e")
                        {
                            current = current.E;
                            path = path[1..];
                        }
                        else if (path.Substring(0, 1) == "w")
                        {
                            current = current.W;
                            path = path[1..];
                        }
                    }

                    blackTiles += current.Flip();
                }

                return blackTiles;
            }

            public int ArtExhibit(int Days)
            {
                for (int day = 0; day < Days; day++)
                {
                    //Reset
                    foreach (var t in Tiles)
                    {
                        if (t != null)
                            t.NewBlack = t.Black;
                    }

                    //2 rules for flipping
                    foreach (var t in Tiles)
                    {
                        if (t != null)
                        {
                            var b = t.BlackTilesAdjacent;

                            if (t.Black && ((b == 0) || (b > 2)))
                                t.NewBlack = false;
                            else if (!t.Black && (b == 2))
                                t.NewBlack = true;
                        }
                    }

                    //Simultaneously flip
                    foreach (var t in Tiles)
                    {
                        if (t != null)
                        {
                            if (t.NewBlack != t.Black)
                                t.Black = t.NewBlack;
                        }
                    }
                }

                var result = 0;
                foreach (var t in Tiles)
                {
                    if (t != null)
                    {
                        if (t.Black)
                            result++;
                    }
                }

                return result;
            }
        }

        public class Tile
        {
            public Tile E { get; set; }
            public Tile SE { get; set; }
            public Tile SW { get; set; }
            public Tile W { get; set; }
            public Tile NW { get; set; }
            public Tile NE { get; set; }
            public bool Black { get; set; }
            public bool NewBlack { get; set; }

            public int BlackTilesAdjacent
            {
                get
                {
                    var  result = 0;

                    if ((E != null) && (E.Black)) result++;
                    if ((SE != null) && (SE.Black)) result++;
                    if ((SW != null) && (SW.Black)) result++;
                    if ((W != null) && (W.Black)) result++;
                    if ((NW != null) && (NW.Black)) result++;
                    if ((NE != null) && (NE.Black)) result++;

                    return result;
                }
            }

            public int Flip()
            {
                Black = !Black;
                return Black ? 1 : -1;
            }
        }
    }
}