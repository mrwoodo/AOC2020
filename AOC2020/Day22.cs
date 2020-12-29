using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Day22 : DayBase, ITwoPartQuestion
    {
        private int _GameID = 0;
        public int GetNewGameID
        {
            get
            {
                _GameID++;
                return _GameID;
            }
        }
        private const int PLAYER_1 = 0;
        private const int PLAYER_2 = 1;
        private readonly Dictionary<int, HashSet<string>> HandCache = new Dictionary<int, HashSet<string>>();

        private (List<int>, List<int>) DefaultPack
        {
            get
            {
                var Pack = (new List<int>(), new List<int>());
                var Rows = InputFile.Replace("Player 1:", "").Replace("Player 2:", "").Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < Rows.Length; i++)
                    if (i < Rows.Count() / 2)
                        Pack.Item1.Add(int.Parse(Rows[i]));
                    else
                        Pack.Item2.Add(int.Parse(Rows[i]));

                return Pack;
            }
        }

        public Day22()
        {
            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            var Cards = DealCards(DefaultPack);
            while ((Cards[PLAYER_1].Count > 0) && (Cards[PLAYER_2].Count > 0))
            {
                var Card1 = Cards[PLAYER_1].Dequeue();
                var Card2 = Cards[PLAYER_2].Dequeue();

                if (Card1 > Card2)
                {
                    Cards[PLAYER_1].Enqueue(Card1);
                    Cards[PLAYER_1].Enqueue(Card2);
                }
                else
                {
                    Cards[PLAYER_2].Enqueue(Card2);
                    Cards[PLAYER_2].Enqueue(Card1);
                }
            }

            return ShowWinner(ref Cards);
        }

        public string Part2()
        {
            var Cards = DealCards(DefaultPack);
            var InfiniteGame = false;
            var Game = GetNewGameID;
            var Round = 0;

            while ((Cards[PLAYER_1].Count > 0) && (Cards[PLAYER_2].Count > 0) && !InfiniteGame)
            {
                RecursiveCombat(Game, Round, ref Cards, out InfiniteGame);
                Round++;
            }

            return ShowWinner(ref Cards);
        }

        private bool RecursiveCombat(int Game, int Round, ref Queue<int>[] Cards, out bool InfiniteGame)
        {
            Round++;
            Console.WriteLine($"\r\n-- Round {Round} (Game {Game}) --");

            var Hand = GetHand(ref Cards);

            InfiniteGame = false;
            if (!HandCache.ContainsKey(Game))
                HandCache.Add(Game, new HashSet<string>() { Hand });
            else if (HandCache[Game].Contains(Hand))
            {
                Console.WriteLine("Infinite game!!");
                InfiniteGame = true;
                //return true;
            }
            else
                HandCache[Game].Add(Hand);

            var Card1 = Cards[PLAYER_1].Dequeue();
            var Card2 = Cards[PLAYER_2].Dequeue();

            Console.WriteLine($"Player 1 plays: {Card1}");
            Console.WriteLine($"Player 2 plays: {Card2}");

            //Enough cards for a sub game?
            if ((Cards[PLAYER_1].Count >= Card1) && ((Cards[PLAYER_2].Count >= Card2)))
            {
                var InnerGame = GetNewGameID;
                var InnerRound = 0;
                Console.WriteLine($"Playing a sub-game to determine the winner...\r\n\r\n=== Game {InnerGame} ===");

                //Yes, deal the sub game pack
                var newPack = DealCards((Cards[PLAYER_1].Take(Card1).ToList(), Cards[PLAYER_2].Take(Card2).ToList()));
                var Player1Winner = false;

                while ((newPack[PLAYER_1].Count > 0) && (newPack[PLAYER_2].Count > 0) && !InfiniteGame)
                {
                    Player1Winner = RecursiveCombat(InnerGame, InnerRound, ref newPack, out InfiniteGame);

                    if (InfiniteGame)
                        return true;
                    InnerRound++;
                }

                //Sub game results in the adding of the "triggering round" cards to the winners hand
                if (Player1Winner || InfiniteGame)
                {
                    Cards[PLAYER_1].Enqueue(Card1);
                    Cards[PLAYER_1].Enqueue(Card2);
                    Console.WriteLine($"The winner of game {InnerGame} is player 1!\r\n\r\n...anyway, back to game {Game}.");
                    HandCache.Remove(InnerGame);
                    return true;
                }
                else
                {
                    Cards[PLAYER_2].Enqueue(Card2);
                    Cards[PLAYER_2].Enqueue(Card1);
                    Console.WriteLine($"The winner of game {InnerGame} is player 2!\r\n\r\n...anyway, back to game {Game}.");
                    HandCache.Remove(InnerGame);
                    return false;
                }
            }

            //Not a sub game, just use the regular rules
            if (Card1 > Card2)
            {
                Cards[PLAYER_1].Enqueue(Card1);
                Cards[PLAYER_1].Enqueue(Card2);
                Console.WriteLine($"Player 1 wins round {Round} of game {Game}!");
                return true;
            }
            else
            {
                Cards[PLAYER_2].Enqueue(Card2);
                Cards[PLAYER_2].Enqueue(Card1);
                Console.WriteLine($"Player 2 wins round {Round} of game {Game}!");
                return false;
            }
        }

        private Queue<int>[] DealCards((List<int>, List<int>) Pack)
        {
            var Result = new Queue<int>[2];
            Result[PLAYER_1] = new Queue<int>();
            Result[PLAYER_2] = new Queue<int>();

            for (int i = 0; i < Pack.Item1.Count; i++)
                Result[PLAYER_1].Enqueue(Pack.Item1[i]);

            for (int i = 0; i < Pack.Item2.Count; i++)
                Result[PLAYER_2].Enqueue(Pack.Item2[i]);

            return Result;
        }

        private string ShowWinner(ref Queue<int>[] Cards)
        {
            var Winner = (Cards[PLAYER_1].Count == 0) ? PLAYER_2 : PLAYER_1;
            var Score = 0;
            var CardValue = Cards[Winner].Count;

            while (Cards[Winner].Count > 0)
                Score += (Cards[Winner].Dequeue() * CardValue--);

            return $"Player {Winner + 1} won with card value of {Score}";
        }

        private string GetHand(ref Queue<int>[] Cards)
        {
            var sb = new StringBuilder();
            foreach (var p1 in Cards[PLAYER_1].ToList())
                sb.Append($"{p1}");
            sb.Append("x");
            foreach (var p2 in Cards[PLAYER_2].ToList())
                sb.Append($"{p2}");
            return sb.ToString();
        }
    }
}