using System;
using System.Collections.Generic;
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
            Lines = InputFile.Replace('#', '1').Replace('.', '0');

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
            /*
                              #
            #    ##    ##    ###
             #  #  #  #  #  #
            */
            var monster = new List<(int, int)> { (18, 0), (0, 1), (5, 1), (6, 1), (11, 1), (12, 1),
                                                (17, 1), (18, 1), (19, 1), (1, 2), (4, 2), (7, 2),
                                                (10, 2), (13, 2), (16, 2) };
            var image = photo.Generate();
            var monstersFound = 0;
            var counter = 0;
            var maxY = monster.Max(i => i.Item2);
            var maxX = monster.Max(i => i.Item1);

            while ((monstersFound == 0) && (counter < 8))
            {
                image.SwitchToCombo(counter);

                for (int y = 0; y < image.Pixels.Length - maxY; y++)
                {
                    for (int x = 0; x < image.Pixels.Length - maxX; x++)
                    {
                        var foundEveryPixel = true;
                        foreach (var m in monster)
                        {
                            if (image.Pixels[y + m.Item2][x + m.Item1] == '0')
                            {
                                foundEveryPixel = false;
                                break;
                            }
                        }
                        if (foundEveryPixel)
                            monstersFound++;
                    }
                }
                //Keep flipping/rotating until we find something
                if (monstersFound == 0)
                    counter++;
            }

            var allHashChars = 0;
            foreach (var p in image.Pixels)
                allHashChars += p.Replace("0", "").Length;

            return $"Water Roughness = {allHashChars - (monstersFound * monster.Count)}";
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
                    if (c2.Bottom == c1.Top)
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
                    if (c2.Left == c1.Right)
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
                    if (c2.Top == c1.Bottom)
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
                    if (c2.Right == c1.Left)
                    {
                        c1.NeighbourLeft = c2;
                        c2.NeighbourRight = c1;
                        c1.Used = c2.Used = true;
                        break;
                    }
                }
            }
        }

        public Tile Generate()
        {
            var curr = Tiles.First(i => i.IsTopLeft);
            var lines = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(Tiles.Count()))) * (curr.SideLength - 2);
            var sb = new StringBuilder[lines];
            var counter = 0;

            for (int i = 0; i < lines; i++)
                sb[i] = new StringBuilder();

            //Use Stringbuilders to make a large Tile composed of the original Tiles
            while (curr != null)
            {
                var borderless = curr.RemoveBorder();

                for (int i = 0; i < borderless.GetLength(0); i++)
                    sb[counter + i].Append(borderless[i]);

                var next = curr.NeighbourRight;
                if (next == null)
                {
                    curr = curr.LeftMost.NeighbourBottom;
                    if (curr != null)
                        counter += borderless.GetLength(0);
                }
                else
                    curr = next;
            }

            //Generate the large Tile
            var imageInput = new StringBuilder("Tile 0:\r\n");
            for (int i = 0; i < sb.Length; i++)
                imageInput.Append(sb[i] + "\r\n");

            var image = new Tile(imageInput.ToString());

            return image;
        }
    }

    public class Tile
    {
        private const int COMBOS = 8;
        public int ID { get; set; }
        public bool Used { get; set; }
        public long Top { get; set; }
        public long Right { get; set; }
        public long Bottom { get; set; }
        public long Left { get; set; }
        public StringBuilder[] Pixels { get; set; }
        public int SideLength { get; set; }
        public Tile NeighbourTop { get; set; }
        public Tile NeighbourRight { get; set; }
        public Tile NeighbourBottom { get; set; }
        public Tile NeighbourLeft { get; set; }
        public int NeighboursPopulated
        {
            get
            {
                int result = 0;
                if (NeighbourTop != null)
                    result++;
                if (NeighbourRight != null)
                    result++;
                if (NeighbourBottom != null)
                    result++;
                if (NeighbourLeft != null)
                    result++;

                return result;
            }
        }
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
        private StringBuilder[,] PixelCache { get; set; }

        public Tile(string input)
        {
            var parse = input.ToString().Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var tileID = parse[0].Replace("Tile ", "").Replace(":", "");
            ID = int.Parse(tileID);
            Pixels = new StringBuilder[parse.Length - 1];

            for (int l = 1; l < parse.Length; l++)
                Pixels[l - 1] = new StringBuilder(parse[l]);

            //Precalculate the 8 combinations of sides when rotated/flipped
            SideLength = Pixels[0].Length;
            PixelCache = new StringBuilder[COMBOS, SideLength];
            Combinations = new long[COMBOS, 4];
            for (int i = 0; i < COMBOS; i++)
            {
                CalculateSides(i);
                if (i == 3)
                    Flip();
                else
                    Rotate();
            }

            Used = false;
            SwitchToCombo(0);
        }

        public void SwitchToCombo(int combo)
        {
            if (!this.Used)
            {
                Top = Combinations[combo, 0];
                Right = Combinations[combo, 1];
                Bottom = Combinations[combo, 2];
                Left = Combinations[combo, 3];
                for (int j = 0; j < SideLength; j++)
                    Pixels[j] = PixelCache[combo, j];
            }
        }

        private void Flip()
        {
            for (int i = 0; i < Pixels.Length; i++)
                Pixels[i] = new StringBuilder(new string(Pixels[i].ToString().Reverse().ToArray()));
        }

        private void Rotate()
        {
            var temp = new string[Pixels.Length];
            for (int i = 0; i < Pixels.Length; i++)
            {
                temp[i] = Pixels[i].ToString();
                Pixels[i].Clear();
            }

            for (int x = 0; x < temp[0].Length; x++)
                for (int y = temp.Length - 1; y >= 0; y--)
                    Pixels[x].Append(temp[y].Substring(x, 1));
        }

        private void CalculateSides(int combination)
        {
            var sb = new StringBuilder();

            if (SideLength <= 32)
            {
                Combinations[combination, 0] = Convert.ToInt32(Pixels[0].ToString(), 2);

                for (int i = 0; i < SideLength; i++)
                    sb.Append(Pixels[i][SideLength - 1]);

                Combinations[combination, 1] = Convert.ToInt32(sb.ToString(), 2);
                Combinations[combination, 2] = Convert.ToInt32(Pixels[^1].ToString(), 2);

                sb.Clear();
                for (int i = 0; i < SideLength; i++)
                    sb.Append(Pixels[i][0]);

                Combinations[combination, 3] = Convert.ToInt32(sb.ToString(), 2);
            }

            for (int i = 0; i < SideLength; i++)
                PixelCache[combination, i] = new StringBuilder(Pixels[i].ToString());
        }

        public StringBuilder[] RemoveBorder()
        {
            var sb = new StringBuilder[SideLength - 2];

            for (int i = 1; i < SideLength - 1; i++)
                sb[i - 1] = new StringBuilder(Pixels[i].ToString().Substring(1, Pixels[i].Length - 2));

            return sb;
        }
    }
}