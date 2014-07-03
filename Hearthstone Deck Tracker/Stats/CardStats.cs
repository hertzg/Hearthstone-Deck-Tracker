using System.Collections.Generic;
using Hearthstone_Deck_Tracker.Hearthstone;

namespace Hearthstone_Deck_Tracker.Stats
{
    public class CardStats
    {
        //todo draw/play turn
        public string CardId;
        public int Drawn;
        public int Played;
        public int Mulliganed;
        public List<int> TimeInHand;

        public CardStats()
        {
            
        }
        public CardStats(string cardId)
        {
            CardId = cardId;
        }
    }
}
