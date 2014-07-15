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
            Win,
            Loss
        };

        public Result GameResult { get; set; }
        public string Opponent;
        public bool Coin { get; set; }
        public int TotalTurns { get; set; }
        public DateTime Start;
        public DateTime End;

        [XmlIgnore]
        public string Duration
        {
            get
            {
                var duration = (int) ((End - Start).TotalMinutes);
                return (duration >= 0 ? duration : 0) + " min";
            }
        }

        [XmlIgnore]
        public string Started
        {
            get { return Start.ToShortDateString() + " " + Start.ToShortTimeString(); }
        }
        
        [XmlArray(ElementName = "Turns")]
        [XmlArrayItem(ElementName = "Turn")]
        public List<TurnStats> TurnStats;

        public GameStats()
        {
            TurnStats = new List<TurnStats>();
            GameResult = Result.None;
            Coin = false;
            TotalTurns = 0;
            Start = DateTime.Now;
        }

        public void AddPlay(CardMovementType type, string cardId)
        {
            var turnStats = TurnStats.FirstOrDefault(t => t.Turn == TotalTurns);
            if (turnStats == null)
            {
                turnStats = new TurnStats() {Turn = TotalTurns};
                TurnStats.Add(turnStats);
            }
            turnStats.AddPlay(type, cardId);
        }

        public void AddPlay(OpponentHandMovement type, string cardId)
        {
            var turnStats = TurnStats.FirstOrDefault(t => t.Turn == TotalTurns);
            if (turnStats == null)
            {
                turnStats = new TurnStats() { Turn = TotalTurns };
                TurnStats.Add(turnStats);
            }
            turnStats.AddPlay(type, cardId);
        }
    }
}
