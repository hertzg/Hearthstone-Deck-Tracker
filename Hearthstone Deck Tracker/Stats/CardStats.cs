using System.Collections.Generic;
using System.Diagnostics;

namespace Hearthstone_Deck_Tracker.Stats
{
    public class CardStats
    {
        public List<int> Draws;
        public List<int> Plays;
        public string CardId;
        public int Mulligan;

        public CardStats()
        {
            Draws = new List<int>();
            Plays = new List<int>();
        }

        public CardStats(string cardId)
        {
            CardId = cardId;
            Draws = new List<int>();
            Plays = new List<int>();
        }

        public void Drawn(int turn)
        {
            Draws.Add(turn);
        }

        public void Played(int turn)
        {
            Plays.Add(turn);
        }

        public void Mulliganed()
        {
            Mulligan++;
            Debug.Assert(Draws.Count > 0);
            Draws.RemoveAt(0);
        }
    }
}