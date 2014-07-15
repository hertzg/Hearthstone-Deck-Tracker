using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Hearthstone_Deck_Tracker.Stats;
using MahApps.Metro.Controls;

namespace Hearthstone_Deck_Tracker
{
    /// <summary>
    /// Interaction logic for GameHistoryWindow.xaml
    /// </summary>
    public partial class GameHistoryWindow
    {
        private DeckStats _currentDeckStats;
        private ObservableCollection<GameStats> _gameStats;
        private readonly Action _writeDeckStats;
        private readonly Action _updateDeckList;
        private bool _appIsClosing;

        public GameHistoryWindow(Action writeDeckStats, Action updateDeckList)
        {
            _writeDeckStats = writeDeckStats;
            _updateDeckList = updateDeckList;
            InitializeComponent();
            _appIsClosing = false;
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
            _gameStats = new ObservableCollection<GameStats>(deckStats.Iterations.SelectMany(i => i.GameStats));
            DGrid.ItemsSource = _gameStats;


            var total = deckStats.Iterations.Sum(i => i.GameStats.Count);
            var winLossList = new ObservableCollection<WinLoss>
                {
                    new WinLoss(
                        new List<GameStats>(
                            deckStats.Iterations.SelectMany(
                                i => i.GameStats.Where(gs => gs.GameResult == GameStats.Result.Win))), "Win", total),
                    new WinLoss(
                        new List<GameStats>(
                            deckStats.Iterations.SelectMany(
                                i => i.GameStats.Where(gs => gs.GameResult == GameStats.Result.Loss))), "Loss", total)
                };
            DataGridWinLoss.ItemsSource = winLossList;
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
            var selected = ((GameStats) ((DataGrid) sender).SelectedItem);
            var strings = new ObservableCollection<string>();
            if (selected != null)
            {
                foreach (var turn in selected.TurnStats)
                {
                    strings.Add("Turn " + turn.Turn);
                    foreach (var play in turn.Plays)
                    {
                        strings.Add(" " + play.Type + ": " + play.CardId);
                    }
                }
            }
            ListboxGameDetail.ItemsSource = strings;
        }
    }
}
