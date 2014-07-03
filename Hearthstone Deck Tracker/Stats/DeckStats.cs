using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using Hearthstone_Deck_Tracker.Hearthstone;

namespace Hearthstone_Deck_Tracker.Stats
{
    public class DeckStats
    {

        public string DeckName;

        [XmlArray(ElementName = "Iterations")]
        [XmlArrayItem(ElementName = "Iteration")]
        public List<DeckIteration> Iterations;


        public DeckStats()
        {
            Iterations = new List<DeckIteration>();
        }

        public DeckStats(Deck deck)
        {
            Iterations = new List<DeckIteration>();
            Iterations.Add(new DeckIteration(deck));
            DeckName = deck.Name;
        }

        public void CardDrawn(string cardId)
        {
            Debug.WriteLine("Draw " + cardId + "(" + DeckName + ")", "DeckStats");
            Iterations.Last().CardDrawn(cardId);
        }

        public void CardPlayed(string cardId)
        {
            Debug.WriteLine("Played " + cardId + "(" + DeckName + ")", "DeckStats");
            Iterations.Last().CardPlayed(cardId);
        }

        public void NewGame(string opponentClass)
        {
            Debug.WriteLine("Started recording new game (" + DeckName + ")", "DeckStats");
            Iterations.Last().NewGame(opponentClass);
        }

        public void GameEnd()
        {
            Debug.WriteLine("Finished recording game (" + DeckName + ")", "DeckStats");
            Iterations.Last().GameEnd();
        }

        public void NewDeckIteration(Deck deck)
        {
            Debug.WriteLine("New deck iteration (" + DeckName + ")", "DeckStats");
            Iterations.Add(new DeckIteration(deck));
        }

        public void CardMulliganed(string cardId)
        {
            Debug.WriteLine("Mulligan (" + DeckName + ")", "DeckStats");
            Iterations.Last().CardMulliganed(cardId);
        }

        public void SetGameResult(GameStats.Result result)
        {
            Debug.WriteLine("Gameresult set to: " + result + " (" + DeckName + ")", "DeckStats");
            Iterations.Last().GameResult(result);
        }
        public GameStats.Result GetGameResult()
        {
            return Iterations.Last().GetGameResult();
        }

        public GameStats GetGameStats()
        {
            return Iterations.Last().GetGameStats();
        }

        public void ClearGameStats()
        {
            Iterations.Last().ClearGameStats();
        }

        public void SetGameStats(GameStats stats)
        {
            Iterations.Last().SetGameStats(stats);
        }
    }
    
}
