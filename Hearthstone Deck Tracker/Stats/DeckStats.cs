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
            var id = Iterations.Any() ? Iterations.Max(i => i.Id) + 1 : 0;
            Iterations.Add(new DeckIteration(deck, id));
            DeckName = deck.Name;
        }

        public void AddPlay(string type, string cardId)
        {
            Iterations.Last().AddPlay(type, cardId);
        }

        public void NewGame()
        {
            Debug.WriteLine("Started recording new game (" + DeckName + ")", "DeckStats");
            Iterations.Last().NewGame();
        }

        public void GameEnd()
        {
            Debug.WriteLine("Finished recording game (" + DeckName + ")", "DeckStats");
            Iterations.Last().GameEnd();
        }

        public void NewDeckIteration(Deck deck)
        {
            Debug.WriteLine("New deck iteration (" + DeckName + ")", "DeckStats");
            var id = Iterations.Any() ? Iterations.Max(i => i.Id) + 1 : 0;
            Iterations.Add(new DeckIteration(deck, id));
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
        public void SetOpponentHero(string opponent)
        {
            Iterations.Last().SetOpponentHero(opponent);
        }
        public void GoingFirst()
        {
            Iterations.Last().GoingFirst();
        }

        public void SetTurn(int turn)
        {
            Iterations.Last().SetTurn(turn);
        }

    }
    
}
