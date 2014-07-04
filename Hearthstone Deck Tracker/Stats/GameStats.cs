using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hearthstone_Deck_Tracker.Stats
{
    public class GameStats
    {
        public enum Result
        {
            None,
            Victory,
            Loss
        };

        public Result GameResult;
        public string OpponentClass;
        public bool First;
        public int Turns;

        [XmlArray(ElementName = "Cards")]
        [XmlArrayItem(ElementName = "Card")]
        public List<CardStats> CardStats;

        public GameStats()
        {
            CardStats = new List<CardStats>();
            GameResult = Result.None;
            First = false;
            Turns = 0;
        }
    }
}
