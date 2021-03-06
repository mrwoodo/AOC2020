﻿using System;
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
        private readonly Dictionary<int, HashSet<string>> Cache = new Dictionary<int, HashSet<string>>();

        private (List<int> Player1, List<int> Player2) DefaultPack
        {
            get
            {
                (List<int> Player1, List<int> Player2) Pack = (new List<int>(), new List<int>());
                var Rows = InputFile.Replace("Player 1:", "").Replace("Player 2:", "").Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < Rows.Length; i++)
                    if (i < Rows.Count() / 2)
                        Pack.Player1.Add(int.Parse(Rows[i]));
                    else
                        Pack.Player2.Add(int.Parse(Rows[i]));

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

                AddCardsToPlayerPack(Card1 > Card2, ref Cards, Card1, Card2);
            }

            return ShowWinner(ref Cards);
        }

        public string Part2()
        {
            var Cards = DealCards(DefaultPack);
            var Game = GetNewGameID;

            while ((Cards[PLAYER_1].Count > 0) && (Cards[PLAYER_2].Count > 0))
                RecursiveCombat(Game, ref Cards, out _);

            return ShowWinner(ref Cards);
        }

        private bool RecursiveCombat(int Game, ref Queue<int>[] Cards, out bool InfiniteGame)
        {
            //The cache tracks all the hands that were dealt for games. If we encounter the exact same hand twice
            //in a given game, that's an infinite loop and we break out of it.
            InfiniteGame = false;
            var Hand = GetHand(ref Cards);
            if (!Cache.ContainsKey(Game))
                Cache.Add(Game, new HashSet<string>() { Hand });
            else if (Cache[Game].Contains(Hand))
            {
                InfiniteGame = true;
                return true;
            }
            else
                Cache[Game].Add(Hand);

            var Card1 = Cards[PLAYER_1].Dequeue();
            var Card2 = Cards[PLAYER_2].Dequeue();

            //Enough cards for a sub game?
            if ((Cards[PLAYER_1].Count >= Card1) && ((Cards[PLAYER_2].Count >= Card2)))
            {
                //Yes, deal the sub game pack
                var InnerGame = GetNewGameID;
                var newPack = DealCards((Cards[PLAYER_1].Take(Card1).ToList(), Cards[PLAYER_2].Take(Card2).ToList()));
                var Player1Winner = false;
                var Infinite = false;

                while ((newPack[PLAYER_1].Count > 0) && (newPack[PLAYER_2].Count > 0) && !Infinite)
                    Player1Winner = RecursiveCombat(InnerGame, ref newPack, out Infinite);

                Cache.Remove(InnerGame); //We're exiting the sub-game, don't need the Cache for it anymroe

                //Sub game results in the adding of the "triggering round" cards to the winners hand
                return AddCardsToPlayerPack(Player1Winner, ref Cards, Card1, Card2);
            }

            //Not a sub game, just use the regular rules
            return AddCardsToPlayerPack(Card1 > Card2, ref Cards, Card1, Card2);
        }

        private bool AddCardsToPlayerPack(bool DidPlayerOneWin, ref Queue<int>[] Cards, int Card1, int Card2)
        {
            if (DidPlayerOneWin)
            {
                Cards[PLAYER_1].Enqueue(Card1);
                Cards[PLAYER_1].Enqueue(Card2);
            }
            else
            {
                Cards[PLAYER_2].Enqueue(Card2);
                Cards[PLAYER_2].Enqueue(Card1);
            }
            return DidPlayerOneWin;
        }

        private Queue<int>[] DealCards((List<int> Player1, List<int> Player2) Pack)
        {
            var Result = new Queue<int>[2];
            Result[PLAYER_1] = new Queue<int>();
            Result[PLAYER_2] = new Queue<int>();

            for (int i = 0; i < Pack.Player1.Count; i++)
                Result[PLAYER_1].Enqueue(Pack.Player1[i]);

            for (int i = 0; i < Pack.Player2.Count; i++)
                Result[PLAYER_2].Enqueue(Pack.Player2[i]);

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
                sb.Append($"{p1},");
            sb.Append("|");
            foreach (var p2 in Cards[PLAYER_2].ToList())
                sb.Append($"{p2},");
            return sb.ToString();
        }
    }
}