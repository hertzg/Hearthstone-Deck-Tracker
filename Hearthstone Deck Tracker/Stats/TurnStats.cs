using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hearthstone_Deck_Tracker.Stats
{
    public class TurnStats
    {
        public TurnStats()
        {
            Plays = new List<Play>();
        }

        public int Turn;

        [XmlArray(ElementName = "Plays")] 
        [XmlArrayItem(ElementName = "Play")] 
        public List<Play> Plays;

        public void SetTurn(int turn)
        {
            Turn = turn;
        }

        public void AddPlay(CardMovementType type, string cardId)
        {
            Plays.Add(new Play(type, cardId));
        }

        public class Play
        {
            public Play()
            {
                
            }
            public Play(CardMovementType type, string cardId)
            {
                Type = Enum.GetName(typeof(CardMovementType), type);
                CardId = cardId;
            }

            public Play(OpponentHandMovement type, string cardId)
            {
                Type = "Opponent" + Enum.GetName(typeof(OpponentHandMovement), type);
                CardId = cardId;
            }

            public string Type;
            public string CardId;
        }

        public void AddPlay(OpponentHandMovement type, string cardId)
        {
            Plays.Add(new Play(type, cardId));
        }
    }
}
