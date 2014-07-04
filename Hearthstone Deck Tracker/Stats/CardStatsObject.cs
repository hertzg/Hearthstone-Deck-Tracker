using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Hearthstone_Deck_Tracker.Hearthstone;

namespace Hearthstone_Deck_Tracker.Stats
{
    public class CardStatsObject
    {
        public Card Card;
        public DeckStats DeckStats;
        private readonly List<string> _hsClasses = new List<string>()
            {
                "All", 
                "Druid", 
                "Hunter", 
                "Mage", 
                "Paladin", 
                "Priest", 
                "Rogue", 
                "Shaman", 
                "Warlock", 
                "Warrior", 
            };

        public CardStatsObject(Card card, DeckStats deckStats)
        {
            Card = card;
            DeckStats = deckStats;
        }

        public ImageBrush CardBackground
        {
            get { return Card.Background; }
        }

        public string CardName
        {
            get { return Card.Name; }
        }

        public int CardCost
        {
            get { return Card.Cost; }
        }

        public string GetPlays(string opponent)
        {
            int wins = 0;
            int losses = 0;
            foreach (var gs in DeckStats.Iterations.Last().GameStats.Where(gs1 => gs1.OpponentClass == opponent || opponent == "All"))
            {
                var stats = gs.CardStats.FirstOrDefault(c => c.CardId == Card.Id);
                if (stats == null) continue;

                switch (gs.GameResult)
                {
                    case GameStats.Result.Victory:
                        wins += stats.Plays.Count;
                        break;
                    case GameStats.Result.Loss:
                        losses += stats.Plays.Count;
                        break;
                }
            }
            return wins + " / " + losses;
        }
        public string GetDraws(string opponent)
        {
            int wins = 0;
            int losses = 0;
            foreach (var gs in DeckStats.Iterations.Last().GameStats.Where(gs1 => gs1.OpponentClass == opponent || opponent == "All"))
            {
                var stats = gs.CardStats.FirstOrDefault(c => c.CardId == Card.Id);
                if (stats == null) continue;

                switch (gs.GameResult)
                {
                    case GameStats.Result.Victory:
                        wins += stats.Draws.Count;
                        break;
                    case GameStats.Result.Loss:
                        losses += stats.Draws.Count;
                        break;
                }
            }
            return wins + " / " + losses;
        }
        public string GetMulligans(string opponent)
        {
            int wins = 0;
            int losses = 0;
            foreach (var gs in DeckStats.Iterations.Last().GameStats.Where(gs1 => gs1.OpponentClass == opponent || opponent == "All"))
            {
                var stats = gs.CardStats.FirstOrDefault(c => c.CardId == Card.Id);
                if (stats == null) continue;

                switch (gs.GameResult)
                {
                    case GameStats.Result.Victory:
                        wins += stats.Mulligan;
                        break;
                    case GameStats.Result.Loss:
                        losses += stats.Mulligan;
                        break;
                }
            }
            return wins + " / " + losses;
        }
        public string GetTimeInHand(string opponent)
        {
            double wins = 0;
            double losses = 0;
            int gameCount = 0;
            foreach (var gs in DeckStats.Iterations.Last().GameStats.Where(gs1 => gs1.OpponentClass == opponent || opponent == "All"))
            {
                //todo
                var stats = gs.CardStats.FirstOrDefault(c => c.CardId == Card.Id);
                if (stats == null) continue;

                gameCount++;
                switch (gs.GameResult)
                {
                    case GameStats.Result.Victory:
                        for (int i = 0; i < stats.Draws.Count-1; i++)
                        {
                            wins += (stats.Plays.Count <= i ? gs.Turns : stats.Plays[i]) - stats.Draws[i];
                        }
                        break;
                    case GameStats.Result.Loss: 
                        for (int i = 0; i < stats.Draws.Count-1; i++)
                        {
                            losses += (stats.Plays.Count <= i ? gs.Turns : stats.Plays[i]) - stats.Draws[i];
                        }
                        break;
                }
            }
            return (gameCount != 0 ? Math.Round(wins / gameCount, 2) : 0 )+ " / " + (gameCount != 0 ? Math.Round(losses / gameCount, 2) : 0);
        }

        public string[] Plays
        {
            get { return _hsClasses.Select(GetPlays).ToArray(); }
        }

        public string[] Draws
        {

            get { return _hsClasses.Select(GetDraws).ToArray(); }
        }

        public string[] Mulligans
        {
            get { return _hsClasses.Select(GetMulligans).ToArray(); }
        }
        public string[] TimeInHand
        {
            get { return _hsClasses.Select(GetTimeInHand).ToArray(); }
        }


    }
}
