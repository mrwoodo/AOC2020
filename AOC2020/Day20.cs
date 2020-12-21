using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Day20 : DayBase, ITwoPartQuestion
    {
        private readonly string Lines;
        private Photo photo;

        public Day20()
        {
            Lines = File.ReadAllText("Input\\Day20.txt").Replace('#', '1').Replace('.', '0');

            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            photo = new Photo(Lines);
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
            var curr = photo.Tiles.First(i => i.IsTopLeft);
            var lines = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(photo.Tiles.Count()))) * (curr.SideLength - 2);
            var sb = new StringBuilder[lines];
            var counter = 0;

            for (int i = 0; i < lines; i++)
                sb[i] = new StringBuilder();

            while (curr != null)
            {
                Console.Write($"{curr.ID}\t");
                var trimmed = curr.GetTrimmedInput();

                for (int i = 0; i < trimmed.GetLength(0); i++)
                    sb[counter + i].Append(trimmed[i]);

                var next = curr.NeighbourRight;
                if (next == null)
                {
                    curr = curr.LeftMost.NeighbourBottom;
                    if (curr != null)
                        counter += trimmed.GetLength(0);

                    Console.WriteLine();
                }
                else
                    curr = next;
            }

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
            while (Tiles.Count(t => !t.Used) > 0)
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

            if (c1.NeighbourTop == null)
            {
                for (int i = 0; i < 8; i++)
                {
                    c2.SwitchToCombo(i);
                    if (c2.side.Bottom == c1.side.Top)
                    {
                        c1.NeighbourTop = c2;
                        c2.NeighbourBottom = c1;
                        c1.Used = c2.Used = true;
                        break;
                    }
                }
            }

            if (c1.NeighbourRight == null)
            {
                for (int i = 0; i < 8; i++)
                {
                    c2.SwitchToCombo(i);
                    if (c2.side.Left == c1.side.Right)
                    {
                        c1.NeighbourRight = c2;
                        c2.NeighbourLeft = c1;
                        c1.Used = c2.Used = true;
                        break;
                    }
                }
            }

            if (c1.NeighbourBottom == null)
            {
                for (int i = 0; i < 8; i++)
                {
                    c2.SwitchToCombo(i);
                    if (c2.side.Top == c1.side.Bottom)
                    {
                        c1.NeighbourBottom = c2;
                        c2.NeighbourTop = c1;
                        c1.Used = c2.Used = true;
                        break;
                    }
                }
            }

            if (c1.NeighbourLeft == null)
            {
                for (int i = 0; i < 8; i++)
                {
                    c2.SwitchToCombo(i);
                    if (c2.side.Right == c1.side.Left)
                    {
                        c1.NeighbourLeft = c2;
                        c2.NeighbourRight = c1;
                        c1.Used = c2.Used = true;
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
        public string Input { get; set; }
        public int SideLength { get; set; }
        public Tile NeighbourTop
        {
            get { return Neighbour[0]; }
            set
            {
                Neighbour[0] = value;
            }
        }
        public Tile NeighbourRight
        {
            get { return Neighbour[1]; }
            set
            {
                Neighbour[1] = value;
            }
        }
        public Tile NeighbourBottom
        {
            get { return Neighbour[2]; }
            set
            {
                Neighbour[2] = value;
            }
        }
        public Tile NeighbourLeft
        {
            get { return Neighbour[3]; }
            set
            {
                Neighbour[3] = value;
            }
        }
        public int NeighboursPopulated => Neighbour.Count(x => x != null);
        public bool IsTopLeft => ((NeighbourTop == null) && (NeighbourLeft == null));
        public Tile LeftMost
        {
            get
            {
                var finished = false;
                Tile search = this;
                while (!finished)
                {
                    if (search.NeighbourLeft == null)
                        finished = true;
                    else
                        search = search.NeighbourLeft;
                }
                return search;
            }
        }
        private long[,] Combinations { get; set; }

        public Tile(string input)
        {
            var parse = input.ToString().Split("\r\n");
            var tileID = parse[0].Replace("Tile ", "").Replace(":", "");
            var sb = new StringBuilder();

            ID = int.Parse(tileID);
            for (int l = 1; l < parse.Length; l++)
                sb.Append(parse[l]);

            Input = sb.ToString();
            SideLength = Convert.ToInt32(Math.Sqrt(Input.Length));

            //Precalculate the 8 combinations of sides when rotated/flipped
            Combinations = new long[8, 4];
            for (int i = 0; i < 8; i++) //Initial, Rotate, Rotate, Rotate, Flip, Rotate, Rotate, Rotate
            {
                GetSides(i);
                if (i == 3)
                    Flip();
                else
                    Rotate();
            }

            Neighbour = new Tile[4];
            Used = false;
            SwitchToCombo(0);
        }

        public void SwitchToCombo(int combo)
        {
            if (!this.Used)
            {
                side = new Side(
                top: Combinations[combo, 0],
                right: Combinations[combo, 1],
                bottom: Combinations[combo, 2],
                left: Combinations[combo, 3]);
            }
        }

        private void Flip()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < SideLength; i++)
            {
                for (int j = SideLength - 1; j >= 0; j--)
                {
                    var c = Input.Substring(i * SideLength + j, 1);
                    sb.Append(c);
                }
            }

            Input = sb.ToString();
        }

        private void Rotate()
        {
            var sb = new StringBuilder();

            for (int j = 0; j < SideLength; j++)
            {
                for (int i = Input.Length - SideLength; i >= 0; i -= SideLength)
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

            Combinations[combo, 0] = Convert.ToInt16(Input.Substring(0, SideLength), 2);

            for (int i = SideLength - 1; i <= Input.Length - 1; i += SideLength)
                sb.Append(Input.Substring(i, 1));

            Combinations[combo, 1] = Convert.ToInt16(sb.ToString(), 2);
            Combinations[combo, 2] = Convert.ToInt16(Input.Substring(Input.Length - SideLength, SideLength), 2);

            sb.Clear();
            for (int i = 0; i <= Input.Length - SideLength; i += SideLength)
                sb.Append(Input.Substring(i, 1));
            Combinations[combo, 3] = Convert.ToInt16(sb.ToString(), 2);
        }

        public StringBuilder[] GetTrimmedInput()
        {
            var sb = new StringBuilder[SideLength - 2];

            for (int i = 1; i < SideLength - 1; i++)
                sb[i - 1] = new StringBuilder(Input.Substring(i * SideLength + 1, SideLength - 2));

            return sb;
        }
    }

    public class Side
    {
        public long Top { get; set; }
        public long Right { get; set; }
        public long Bottom { get; set; }
        public long Left { get; set; }

        public Side(long top, long right, long bottom, long left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }
    }
}