using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Stats;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Hearthstone_Deck_Tracker
{
    /// <summary>
    /// Interaction logic for GameHistoryWindow.xaml
    /// </summary>
    public partial class GameHistoryWindow
    {
        private bool _initialized;
        private Config _config;
        private DeckStats _currentDeckStats;
        private ObservableCollection<GameStats> _gameStats;
        private readonly Action _writeDeckStats;
        private readonly Action _writeConfig;
        private readonly Action _updateDeckList;
        private bool _appIsClosing;

        public GameHistoryWindow(Config config, Action writeDeckStats, Action writeConfig, Action updateDeckList)
        {
            _initialized = false;
            InitializeComponent();
            
            _writeDeckStats = writeDeckStats;
            _updateDeckList = updateDeckList;
            _writeConfig = writeConfig;
            _appIsClosing = false;
            _config = config;
            LoadConfig();
            _initialized = true;
        }

        private void LoadConfig()
        {
            CheckboxPlayerDraw.IsChecked = _config.DeckStats.ShowPlayerDraw;
            CheckboxOpponentDraw.IsChecked = _config.DeckStats.ShowOpponentDraw;
            CheckboxPlayerPlay.IsChecked = _config.DeckStats.ShowPlayerPlay;
            CheckboxOpponentPlay.IsChecked = _config.DeckStats.ShowOpponentPlay;
            CheckboxPlayerMulligan.IsChecked = _config.DeckStats.ShowPlayerMulligan;
            CheckboxOpponentMulligan.IsChecked = _config.DeckStats.ShowOpponentMulligan;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var game = DGrid.SelectedItem as GameStats;
            if (game == null) return;
            var iteration = _currentDeckStats.Iterations.FirstOrDefault(i => i.GameStats.Contains(game));
            if (iteration != null)
            {
                iteration.GameStats.Remove(game);
                _writeDeckStats();
                _updateDeckList();
            }
            if (_gameStats.Contains(game))
            {
                _gameStats.Remove(game);
            }
        }

        public void SetDeckStats(DeckStats deckStats)
        {
            _currentDeckStats = deckStats;

            ComboboxDeckIteration.Items.Clear();
            ComboboxDeckIteration.Items.Add("All");
            ComboboxDeckIteration.SelectedIndex = 0;
            foreach (var iteration in deckStats.Iterations)
            {
                ComboboxDeckIteration.Items.Add(iteration);
            }
            ReloadTreeView();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!_appIsClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        public void Shutdown()
        {
            _appIsClosing = true;
            Close();
        }

        private class WinLoss
        {
            private readonly List<GameStats> _stats;
            private int _total;
            public WinLoss(List<GameStats> stats, string result, int total)
            {
                _stats = stats;
                Result = result;
                _total = total;
            }

            public string Result { get; private set; }

            public string Total
            {
                get { return GetDisplayString(_stats.Count); }
            }

            public string Coin
            {
                get { return GetDisplayString(_stats.Count(gs => !gs.Coin)); }
            }

            public string NoCoin
            {
                get { return GetDisplayString(_stats.Count(gs => gs.Coin)); }
            }

            private string GetDisplayString(int count)
            {
                return count + " (" + Math.Round(count/(double)_total, 2)*100 + "%)";
            }
        }

        private void DGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadTreeView();
        }

        private void ReloadTreeView()
        {
            var selected = DGrid.SelectedItem as GameStats;
            if (selected != null)
            {
                var tvItemSource = new List<TreeViewItem>();
                foreach (var turn in selected.TurnStats)
                {
                    var treeViewTurn = new TreeViewItem();
                    treeViewTurn.Header = "Turn " + turn.Turn;
                    foreach (TreeViewItem item in TreeviewGameDetail.Items)
                    {
                        if (item.Header.Equals(treeViewTurn.Header))
                        {
                            treeViewTurn.IsExpanded = item.IsExpanded;
                            break;
                        }
                    }
                    foreach (var play in turn.Plays)
                    {
                        if ((play.Type == "PlayerPlay" || play.Type == "PlayerHandDiscard") && !_config.DeckStats.ShowPlayerPlay 
                            || (play.Type == "PlayerDraw" || play.Type == "PlayerGet" || play.Type == "PlayerDeckDiscard") && !_config.DeckStats.ShowPlayerDraw
                            || play.Type == "PlayerMulligan" && !_config.DeckStats.ShowPlayerMulligan
                            || (play.Type == "OpponentPlay" || play.Type == "OpponentSecretTrigger") && !_config.DeckStats.ShowOpponentPlay
                            || (play.Type == "OpponentDraw" || play.Type == "OpponentFromPlayerDeck" || play.Type == "OpponentPlayToHand" || play.Type == "OpponentDeckDiscard") && !_config.DeckStats.ShowOpponentDraw
                            || play.Type == "OpponentMulligan" && !_config.DeckStats.ShowOpponentMulligan)
                            continue;

                        treeViewTurn.Items.Add(new GameHistoryItem(play));
                    }
                    tvItemSource.Add(treeViewTurn);
                }
                TreeviewGameDetail.ItemsSource = tvItemSource;
            }

        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            FlyoutEditGame.IsOpen = true;
        }

        private void CheckboxPlayerPlay_Checked(object sender, RoutedEventArgs e)
        {
            if (!_initialized) return;
            _config.DeckStats.ShowPlayerPlay = true;
            _writeConfig();
            ReloadTreeView();
        }

        private void CheckboxPlayerPlay_Unchecked(object sender, RoutedEventArgs e)
        {

            if (!_initialized) return;
            _config.DeckStats.ShowPlayerPlay = false;
            _writeConfig();
            ReloadTreeView();
        }

        private void CheckboxOpponentPlay_Checked(object sender, RoutedEventArgs e)
        {
            if (!_initialized) return;
            _config.DeckStats.ShowOpponentPlay = true;
            _writeConfig();
            ReloadTreeView();
        }

        private void CheckboxOpponentPlay_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!_initialized) return;
            _config.DeckStats.ShowOpponentPlay = false;
            _writeConfig();
            ReloadTreeView();
        }

        private void CheckboxPlayerDraw_Checked(object sender, RoutedEventArgs e)
        {
            if (!_initialized) return;
            _config.DeckStats.ShowPlayerDraw = true;
            _writeConfig();
            ReloadTreeView();
        }

        private void CheckboxPlayerDraw_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!_initialized) return;
            _config.DeckStats.ShowPlayerDraw = false;
            _writeConfig();
            ReloadTreeView();
        }

        private void CheckboxOpponentDraw_Checked(object sender, RoutedEventArgs e)
        {
            if (!_initialized) return;
            _config.DeckStats.ShowOpponentDraw = true;
            _writeConfig();
            ReloadTreeView();
        }

        private void CheckboxOpponentDraw_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!_initialized) return;
            _config.DeckStats.ShowOpponentDraw = false;
            _writeConfig();
            ReloadTreeView();
        }

        private void CheckboxPlayerMulligan_Checked(object sender, RoutedEventArgs e)
        {
            if (!_initialized) return;
            _config.DeckStats.ShowPlayerMulligan = true;
            _writeConfig();
            ReloadTreeView();
        }

        private void CheckboxPlayerMulligan_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!_initialized) return;
            _config.DeckStats.ShowPlayerMulligan = false;
            _writeConfig();
            ReloadTreeView();
        }

        private void CheckboxOpponentMulligan_Checked(object sender, RoutedEventArgs e)
        {
            if (!_initialized) return;
            _config.DeckStats.ShowOpponentMulligan = true;
            _writeConfig();
            ReloadTreeView();
        }

        private void CheckboxOpponentMulligan_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!_initialized) return;
            _config.DeckStats.ShowOpponentMulligan = false;
            _writeConfig();
            ReloadTreeView();
        }

        private void ComboboxDeckIteration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_initialized) return;
            var selectedGame = DGrid.SelectedItem as GameStats;
            
            var cardStatsObjects = new List<CardStatsObject>();

            var item = ComboboxDeckIteration.SelectedItem;
            if (item == "All")
            {

                _gameStats =
                    new ObservableCollection<GameStats>(_currentDeckStats.Iterations.SelectMany(i => i.GameStats));

                //no idea how well this works
                foreach (var card in _currentDeckStats.Iterations.SelectMany(i => i.Cards))
                {
                    if (cardStatsObjects.All(cso => cso.CardName != card.LocalizedName))
                    {
                        cardStatsObjects.Add(new CardStatsObject(card, _gameStats));
                    }
                }
            }
            else if(item is DeckIteration)
            {
                _gameStats = new ObservableCollection<GameStats>(((DeckIteration) item).GameStats);
                foreach (var card in ((DeckIteration)item).Cards)
                {
                    if (cardStatsObjects.All(cso => cso.CardName != card.LocalizedName))
                    {
                        cardStatsObjects.Add(new CardStatsObject(card, _gameStats));
                    }
                }
            }

            DGrid.ItemsSource = _gameStats;
            DGrid.Items.SortDescriptions.Clear();
            DGrid.Items.SortDescriptions.Add(new SortDescription("Started", ListSortDirection.Descending));
            if (DGrid.Items.Count > 0)
            {
                DGrid.SelectedIndex = 0;
            }


            var total = _currentDeckStats.Iterations.Sum(i => i.GameStats.Count);
            var winLossList = new ObservableCollection<WinLoss>
                {
                    new WinLoss(_gameStats.Where(gs => gs.GameResult == GameStats.Result.Win).ToList(), "Win", total),
                    new WinLoss(_gameStats.Where(gs => gs.GameResult == GameStats.Result.Loss).ToList(), "Loss", total)
                };
            DataGridWinLoss.ItemsSource = winLossList;




            ListviewStats.ItemsSource = cardStatsObjects;

            if (DGrid.Items.Contains(selectedGame))
                DGrid.SelectedItem = selectedGame;
            else TreeviewGameDetail.ItemsSource = null;
        }

        private async void BtnDeleteIteration_Click(object sender, RoutedEventArgs e)
        {

            //to avoid having the latest iteration not match the deck
            //allow delete only if: iteration is not latest OR iteration is latest but matches the previous iteration (no actual changes)

            var iteration = ComboboxDeckIteration.SelectedItem as DeckIteration;
            if (iteration != null)
            {
                if (_currentDeckStats.Iterations.Last() == iteration)
                {
                    var secondToLast = _currentDeckStats.Iterations[_currentDeckStats.Iterations.Count - 2];
                    if (iteration.Cards.Any(c => secondToLast.Cards.Any(c2 => c2.Equals(c) && c2.Count != c.Count ) || !secondToLast.Cards.Any(c2 => c2.Equals(c))) ||
                        secondToLast.Cards.Any(c => iteration.Cards.Any(c2 => c2.Equals(c) && c2.Count != c.Count) || !iteration.Cards.Any(c2 => c2.Equals(c))))
                    {
                        await this.ShowMessageAsync("Error", "Can not delete latest iteration if it does not match the previous.");
                        return;
                    }
                }
                var settings = new MetroDialogSettings();
                settings.AffirmativeButtonText = "Yes";
                settings.NegativeButtonText = "No";
                var result = await this.ShowMessageAsync("Are you sure?", "This can not be undone.", MessageDialogStyle.AffirmativeAndNegative, settings);
                if (result != MessageDialogResult.Affirmative) return;

                _currentDeckStats.Iterations.Remove(iteration);
                _writeDeckStats();
                ComboboxDeckIteration.SelectedIndex = 0;
                ComboboxDeckIteration.Items.Remove(iteration);
            }
        }

        private class CardStatsObject
        {
            public ImageBrush CardBackground { get; set; }
            public string CardName { get; set; }
            public int CardCost { get; set; }
            private readonly IEnumerable<GameStats> _gameStats;
            private string _cardId;
            private Card _card;
            public CardStatsObject(Card card, IEnumerable<GameStats> currentGameStats)
            {
                CardBackground = card.Background;
                CardCost = card.Cost;
                CardName = card.LocalizedName;
                _gameStats = currentGameStats;
                _cardId = card.Id;
                _card = card;
            }

            public string[] CardStats
            {
                get { return _hsClasses.Select(GetCardStats).ToArray(); }
            }

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

            private string GetCardStats(string opponent)
            {
                var avgDrawTurn = new double[2];
                var drawCount = new int[2];
                var avgPlayTurn = new double[2];
                var playCount = new int[2];
                foreach (var gs in _gameStats.Where(gs => gs.Opponent == opponent || opponent == "All"))
                {
                    var foundDraw = false;
                    bool foundPlay = false;
                    var index = gs.GameResult == GameStats.Result.Win ? 0 : 1;
                    foreach (var ts in gs.TurnStats)
                    {
                        foreach (var play in ts.Plays)
                        {
                            if (play.CardId == _cardId)
                            {
                                if (play.Type == "PlayerDraw")
                                {
                                    avgDrawTurn[index] += ts.Turn;
                                    drawCount[index]++;
                                    foundDraw = true;
                                }
                                else if (play.Type == "PlayerPlay" || play.Type == "PlayerHandDiscard")
                                {
                                    avgPlayTurn[index] += ts.Turn;
                                    playCount[index]++;
                                    foundPlay = true;
                                }
                            }
                        }
                    }
                    if (!foundPlay && foundDraw)
                    {
                        avgPlayTurn[index] += gs.TotalTurns;
                        playCount[index]++;
                    }
                }

                avgPlayTurn[0] = playCount[0] > 0 ? avgPlayTurn[0] / playCount[0] : 0;
                avgDrawTurn[0] = drawCount[0] > 0 ? avgDrawTurn[0] / drawCount[0] : 0;
                avgPlayTurn[1] = playCount[1] > 0 ? avgPlayTurn[1] / playCount[1] : 0;
                avgDrawTurn[1] = drawCount[1] > 0 ? avgDrawTurn[1] / drawCount[1] : 0;

                var returnStrings = new string[2];
                returnStrings[0] = drawCount[0] > 0 ? Math.Round(avgPlayTurn[0] - avgDrawTurn[0], 2).ToString() : "-";
                returnStrings[1] = drawCount[1] > 0 ? Math.Round(avgPlayTurn[1] - avgDrawTurn[1], 2).ToString() : "-";

                return returnStrings[0] + "/" + returnStrings[1] + "\n" +
                    100 * drawCount[0] / _card.Count / _gameStats.Count(gs => gs.GameResult == GameStats.Result.Win) + "%/" + 100 * drawCount[1] / _card.Count / _gameStats.Count(gs => gs.GameResult == GameStats.Result.Loss) + "%\n" +
                    100 * playCount[0] / _card.Count / _gameStats.Count(gs => gs.GameResult == GameStats.Result.Win) + "%/" + 100 * playCount[1] / _card.Count / _gameStats.Count(gs => gs.GameResult == GameStats.Result.Loss) + "%\n";
            }
        }
    }
}
