using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Day22 : DayBase, ITwoPartQuestion
    {
        private const int PLAYER_1 = 0;
        private const int PLAYER_2 = 1;
        private readonly string Lines;
        private readonly Queue<int>[] Cards;

        public Day22()
        {
            Lines = InputFile.Replace("Player 1:", "").Replace("Player 2:", "");
            Cards = new Queue<int>[2];
            Cards[PLAYER_1] = new Queue<int>();
            Cards[PLAYER_2] = new Queue<int>();
            var rows = Lines.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < rows.Length; i++)
                Cards[i < rows.Length / 2 ? PLAYER_1 : PLAYER_2].Enqueue(int.Parse(rows[i]));

            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            while ((Cards[PLAYER_1].Count > 0) && (Cards[PLAYER_2].Count > 0))
            {
                var card1 = Cards[PLAYER_1].Dequeue();
                var card2 = Cards[PLAYER_2].Dequeue();

                if (card1 > card2)
                {
                    Cards[PLAYER_1].Enqueue(card1);
                    Cards[PLAYER_1].Enqueue(card2);
                }
                else
                {
                    Cards[PLAYER_2].Enqueue(card2);
                    Cards[PLAYER_2].Enqueue(card1);
                }
            }

            var winner = (Cards[PLAYER_1].Count == 0) ? PLAYER_2 : PLAYER_1;
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