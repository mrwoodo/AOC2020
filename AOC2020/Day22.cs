using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Day22 : DayBase, ITwoPartQuestion
    {
        private readonly string Lines;
        private readonly Queue<int>[] Cards;

        public Day22()
        {
            Lines = File.ReadAllText("Input\\Day22.txt").Replace("Player 1:", "").Replace("Player 2:", "");
            Cards = new Queue<int>[2];
            Cards[0] = new Queue<int>();
            Cards[1] = new Queue<int>();
            var rows = Lines.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < rows.Length; i++)
                Cards[i < rows.Length / 2 ? 0 : 1].Enqueue(int.Parse(rows[i]));

            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            while ((Cards[0].Count > 0) && (Cards[1].Count > 0))
            {
                var card0 = Cards[0].Dequeue();
                var card1 = Cards[1].Dequeue();

                if (card0 > card1)
                {
                    Cards[0].Enqueue(card0);
                    Cards[0].Enqueue(card1);
                }
                else
                {
                    Cards[1].Enqueue(card1);
                    Cards[1].Enqueue(card0);
                }
            }

            var winner = (Cards[0].Count == 0) ? 1 : 0;
            var score = 0;
            var cardValue = Cards[winner].Count;

            while (Cards[winner].Count > 0)
            {
                score += (Cards[winner].Dequeue() * cardValue);
                cardValue--;
            }

            return $"Player {winner + 1} won with card value of {score}";
        }

        public string Part2()
        {
            return "";
        }
    }
}