using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Hearthstone_Deck_Tracker.Hearthstone;

namespace Hearthstone_Deck_Tracker.Stats
{
    public class DeckIteration
    {
        [XmlArray(ElementName = "Cards")]
        [XmlArrayItem(ElementName = "Card")]
        public List<Card> Cards;

        [XmlArray(ElementName = "Games")]
        [XmlArrayItem(ElementName = "Game")]
        public List<GameStats> GameStats;
        private GameStats _currentGame;

        public int Id;

        public DeckIteration()
        {
            GameStats = new List<GameStats>();
        }

        public DeckIteration(Deck deck, int id)
        {
            Id = id;
            GameStats = new List<GameStats>();
            Cards = ((Deck)deck.Clone()).Cards.ToList();
        }

        public void NewGame()
        {
            _currentGame = new GameStats();
        }

        public void GameEnd()
        {
            if(_currentGame != null)
            {
                GameStats.Add(_currentGame);
            }
        }

        public void GameResult(GameStats.Result result)
        {
            if(_currentGame != null)
            {
                _currentGame.GameResult = result;
                _currentGame.End = DateTime.Now;
            }
        }

        public GameStats.Result GetGameResult()
        {
            return _currentGame == null ? Stats.GameStats.Result.None : _currentGame.GameResult;
        }

        public void SetGameStats(GameStats stats)
        {
            _currentGame = stats;
        }

        public void SetOpponentHero(string opponent)
        {
            _currentGame.Opponent = opponent;
        }

        public void ClearGameStats()
        {
            _currentGame = null;
        }

        public GameStats GetGameStats()
        {
            return _currentGame;
        }

        public void GoingFirst()
        {
            _currentGame.Coin = true;
        }

        public void SetTurn(int turn)
        {
            if (_currentGame != null)
            {
                _currentGame.TotalTurns = turn;
            }
        }

        public void AddPlay(string type, string cardId)
        {
            _currentGame.AddPlay(type, cardId);
        }

        public override string ToString()
        {
            return "Iteration #" + Id + " (" + GameStats.Count + " games)";
        }
        
    }
}
