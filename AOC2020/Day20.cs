using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Day20 : DayBase, ITwoPartQuestion
    {
        private string Lines;

        public Day20()
        {
            Lines = File.ReadAllText("Input\\Day20.txt").Replace('#', '1').Replace('.', '0');

            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            var photo = new Photo(Lines);
            photo.Assemble();

            long answer = 1;
            var result = "Corners = ";

            foreach (var corner in photo.Tiles.Where(i => i.NeighboursPopulated == 2))
            {
                result += $"{corner.ID} x ";
                answer *= corner.ID;
            }

            return result[0..^2] + $"= {answer}";
        }

        public string Part2()
        {
            return $"";
        }
    }

    public class Photo
    {
        public List<Tile> Tiles;

        public Photo(string input)
        {
            Tiles = new List<Tile>();
            var tileString = input.Split("\r\n\r\n");

            for (var i = 0; i < tileString.Length; i++)
                Tiles.Add(new Tile(tileString[i]));
        }

        public void Assemble()
        {
            Tiles[0].Used = true;
            while (Tiles.Where(t => !t.Used).Count() > 0)
            {
                for (int i = 0; i < Tiles.Count; i++)
                {
                    if (Tiles[i].Used && Tiles[i].NeighboursPopulated < 4)
                    {
                        for (int j = 0; j < Tiles.Count; j++)
                            Compare(Tiles[i], Tiles[j]);
                    }
                }
            }
        }

        private void Compare(Tile c1, Tile c2)
        {
            if (c1 == c2)
                return;

            if (c2.NeighboursPopulated == 4)
                return;

            if (c1.Neighbour[0] == null)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (!c2.Used)
                        c2.SwitchToCombo(i);

                    if (c2.side.Bottom == c1.side.Top)
                    {
                        c1.Neighbour[0] = c2;
                        c2.Neighbour[2] = c1;
                        c2.Used = true;
                        c1.Used = true;
                        break;
                    }
                }
            }

            if (c1.Neighbour[1] == null)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (!c2.Used)
                        c2.SwitchToCombo(i);

                    if (c2.side.Left == c1.side.Right)
                    {
                        c1.Neighbour[1] = c2;
                        c2.Neighbour[3] = c1;
                        c2.Used = true;
                        c1.Used = true;
                        break;
                    }
                }
            }

            if (c1.Neighbour[2] == null)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (!c2.Used)
                        c2.SwitchToCombo(i);

                    if (c2.side.Top == c1.side.Bottom)
                    {
                        c1.Neighbour[2] = c2;
                        c2.Neighbour[0] = c1;
                        c2.Used = true;
                        c1.Used = true;
                        break;
                    }
                }
            }

            if (c1.Neighbour[3] == null)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (!c2.Used)
                        c2.SwitchToCombo(i);

                    if (c2.side.Right == c1.side.Left)
                    {
                        c1.Neighbour[3] = c2;
                        c2.Neighbour[1] = c1;
                        c2.Used = true;
                        c1.Used = true;
                        break;
                    }
                }
            }
        }
    }

    public class Tile
    {
        public int ID { get; set; }
        public bool Used { get; set; }
        public Tile[] Neighbour { get; set; }
        public Side side { get; set; }
        public int NeighboursPopulated => Neighbour.Where(x => x != null).Count();
        private string Input { get; set; }
        private int[,] Combinations { get; set; }

        public Tile(string input)
        {
            var parse = input.ToString().Split("\r\n");
            var tileID = parse[0].Replace("Tile ", "").Replace(":", "");
            var sb = new StringBuilder();

            ID = int.Parse(tileID);
            for (int l = 1; l < parse.Length; l++)
                sb.Append(parse[l]);

            Input = sb.ToString();

            //Precalculate the 8 combinations of sides when rotated/flipped
            Combinations = new int[8, 4];
            for (int i = 0; i < 8; i++) //Initial, Rotate, Rotate, Rotate, Flip, Rotate, Rotate, Rotate
            {
                GetSides(i);
                if (i == 3)
                    Flip();
                else
                    Rotate();
            }

            Used = false;
            Neighbour = new Tile[4];
            SwitchToCombo(0);
        }

        public void SwitchToCombo(int combo)
        {
            side = new Side(
                top: Combinations[combo, 0],
                right: Combinations[combo, 1],
                bottom: Combinations[combo, 2],
                left: Combinations[combo, 3]);
        }

        private void Flip()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 9; j >= 0; j--)
                {
                    var c = Input.Substring(i * 10 + j, 1);
                    sb.Append(c);
                }
            }

            Input = sb.ToString();
        }

        private void Rotate()
        {
            var sb = new StringBuilder();

            for (int j = 0; j < 10; j++)
            {
                for (int i = 90; i >= 0; i -= 10)
                {
                    var c = Input.Substring(i + j, 1);
                    sb.Append(c);
                }
            }

            Input = sb.ToString();
        }

        private void GetSides(int combo)
        {
            var sb = new StringBuilder();

            Combinations[combo, 0] = Convert.ToInt16(Input.Substring(0, 10), 2);

            for (int i = 9; i <= 99; i += 10)
                sb.Append(Input.Substring(i, 1));

            Combinations[combo, 1] = Convert.ToInt16(sb.ToString(), 2);
            Combinations[combo, 2] = Convert.ToInt16(Input.Substring(90, 10), 2);

            sb.Clear();
            for (int i = 0; i <= 90; i += 10)
                sb.Append(Input.Substring(i, 1));
            Combinations[combo, 3] = Convert.ToInt16(sb.ToString(), 2);
        }
    }

    public class Side
    {
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public int Left { get; set; }

        public Side(int top, int right, int bottom, int left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }
    }
}
