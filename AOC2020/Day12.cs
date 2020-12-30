using System;

namespace AOC2020
{
    public class Day12 : DayBase, ITwoPartQuestion
    {
        public Day12()
        {
            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            var ship = new ShipType1();

            NavigateShip(ship);
            return $"Manhattan distance = {ship.ManhattanDistance}";
        }

        public string Part2()
        {
            var ship = new ShipType2();

            NavigateShip(ship);
            return $"Manhattan distance = {ship.ManhattanDistance}";
        }

        private void NavigateShip(ShipBase ship)
        {
            foreach (var line in InputFileAsStringList)
            {
                var cmd = line.Substring(0, 1);
                var amt = Convert.ToInt32(line[1..]);

                switch (cmd)
                {
                    case "N":
                        ship.Move(0, 1, amt);
                        break;
                    case "S":
                        ship.Move(0, -1, amt);
                        break;
                    case "E":
                        ship.Move(1, 0, amt);
                        break;
                    case "W":
                        ship.Move(-1, 0, amt);
                        break;
                    case "L":
                        ship.Turn(amt);
                        break;
                    case "R":
                        ship.Turn(-amt);
                        break;
                    case "F":
                        ship.Forward(amt);
                        break;
                }
            }
        }

        public abstract class ShipBase
        {
            public double X = 0;
            public double Y = 0;
            public double ManhattanDistance => Convert.ToInt32(Math.Abs(X) + Math.Abs(Y));

            public abstract void Move(double x, double y, int amount);
            public abstract void Turn(int degrees);
            public abstract void Forward(int amount);
        }

        public class ShipType1 : ShipBase
        {
            public int FacingDegrees = 0;

            public override void Move(double x, double y, int amount)
            {
                X += (x * amount);
                Y += (y * amount);
            }

            public override void Turn(int degrees)
            {
                FacingDegrees += degrees;

                if (FacingDegrees >= 360)
                    FacingDegrees -= 360;
                else if (FacingDegrees < 0)
                    FacingDegrees += 360;
            }

            public override void Forward(int amount)
            {
                var rad = FacingDegrees * Math.PI / 180;

                X += Convert.ToInt32(amount * Math.Cos(rad));
                Y += Convert.ToInt32(amount * Math.Sin(rad));
            }
        }

        public class ShipType2 : ShipBase
        {
            private double wayPointX = 10;
            private double wayPointY = 1;

            public override void Move(double x, double y, int amount)
            {
                wayPointX += (x * amount);
                wayPointY += (y * amount);
            }

            public override void Turn(int degrees)
            {
                var rad = degrees * Math.PI / 180;
                var rotX = Math.Cos(rad) * wayPointX - Math.Sin(rad) * wayPointY;
                var rotY = Math.Sin(rad) * wayPointX + Math.Cos(rad) * wayPointY;

                wayPointX = rotX;
                wayPointY = rotY;
            }

            public override void Forward(int amount)
            {
                X += (amount * this.wayPointX);
                Y += (amount * this.wayPointY);
            }
        }
    }
}