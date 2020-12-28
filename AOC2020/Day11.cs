using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day11 : DayBase, ITwoPartQuestion
    {
        public Day11()
        {
            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            return $"Occupied seats : {Run(mode: 1)}";
        }

        public string Part2()
        {
            return $"Occupied seats : {Run(mode: 2)}";
        }

        private int Run(int mode)
        {
            var room = new Room(InputFileAsStringList, mode);
            var stateChanged = true;
            var occupiedSeats = 0;

            while (stateChanged)
                stateChanged = room.Calculate(out occupiedSeats);

            return occupiedSeats;
        }

        public class Room
        {
            public int Mode;
            public Seat[,] Seats;
            public int OccupiedToEmptyAmount => (Mode == 1) ? 4 : 5;

            public int OccupiedSeats
            {
                get
                {
                    var occupied = 0;

                    foreach (var s in this.Seats)
                        if (s.State == '#')
                            occupied++;

                    return occupied;
                }
            }

            public Room(List<string> input, int mode)
            {
                Mode = mode;
                Seats = new Seat[input[0].Length, input.Count()];

                for (int x = 0; x < input[0].Length; x++)
                    for (int y = 0; y < input.Count(); y++)
                        Seats[x, y] = new Seat(this, x, y, Convert.ToChar(input[y].Substring(x, 1)));
            }

            public List<Seat> GetNeighbours(int X, int Y)
            {
                var result = new List<Seat>();

                if (Mode == 1)
                {
                    //Checking the 8 adjacents blocks around the given seat
                    for (int x = X - 1; x <= X + 1; x++)
                    {
                        for (int y = Y - 1; y <= Y + 1; y++)
                        {
                            if (!((x == X) && (y == Y)))
                            {
                                if (InRoom(x, y))
                                {
                                    if (Seats[x, y].State != '.')
                                        result.Add(Seats[x, y]);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Checking for the first occupied/empty seat in 8 lines of sight
                    result.Add(SearchLineOfSight(X, Y, +1, +0));
                    result.Add(SearchLineOfSight(X, Y, +1, +1));
                    result.Add(SearchLineOfSight(X, Y, +0, +1));
                    result.Add(SearchLineOfSight(X, Y, -1, +1));
                    result.Add(SearchLineOfSight(X, Y, -1, +0));
                    result.Add(SearchLineOfSight(X, Y, -1, -1));
                    result.Add(SearchLineOfSight(X, Y, +0, -1));
                    result.Add(SearchLineOfSight(X, Y, +1, -1));

                    result.RemoveAll(i => i == null);
                }

                return result;
            }

            private Seat SearchLineOfSight(int x, int y, int xStep, int yStep)
            {
                bool finished = false;
                int counter = 1;

                while (!finished)
                {
                    int checkX = xStep * counter + x;
                    int checkY = yStep * counter + y;

                    if (InRoom(checkX, checkY))
                    {
                        if (Seats[checkX, checkY].State != '.')
                            return Seats[checkX, checkY];
                        else
                            counter++;
                    }
                    else
                        finished = true;
                }

                return null; //fell off the map
            }

            private bool InRoom(int x, int y)
            {
                return ((x >= 0) && (y >= 0) && (x < Seats.GetLength(0)) && (y < Seats.GetLength(1)));
            }

            public bool Calculate(out int occupiedSeats)
            {
                occupiedSeats = 0;
                var stateChanged = false;

                foreach (var s in Seats)
                    s.PrepareToCalculate();

                foreach (var s in Seats)
                {
                    if (s.Calculate())
                        stateChanged = true;

                    if (s.State == '#')
                        occupiedSeats++;
                }

                return stateChanged;
            }
        }

        public class Seat
        {
            public char State { get; set; }
            private readonly Room _room;
            private readonly int X;
            private readonly int Y;
            private bool StateChanged { get; set; }
            private List<Seat> neighbours;

            public Seat(Room room, int x, int y, char state)
            {
                _room = room;
                X = x;
                Y = y;
                State = state;
            }

            public void PrepareToCalculate()
            {
                neighbours = new List<Seat>();

                foreach (var n in _room.GetNeighbours(X, Y))
                    neighbours.Add(new Seat(this._room, n.X, n.Y, n.State));
            }

            public bool Calculate()
            {
                this.StateChanged = false;

                // If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
                if ((this.State == 'L') && !neighbours.Any(i => i.State == '#'))
                {
                    this.State = '#';
                    this.StateChanged = true;
                }
                //If a seat is occupied (#) and four or more seats adjacent to it are also occupied, the seat becomes empty.
                else if ((this.State == '#') && (neighbours.Count(i => i.State == '#') >= _room.OccupiedToEmptyAmount))
                {
                    this.State = 'L';
                    this.StateChanged = true;
                }

                return this.StateChanged;
            }
        }
    }
}