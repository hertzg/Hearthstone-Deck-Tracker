using System;
using System.Collections.Generic;
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

namespace Hearthstone_Deck_Tracker
{
    /// <summary>
    /// Interaction logic for DeckStatsWindow.xaml
    /// </summary>
    public partial class DeckStatsWindow
    {
        private readonly List<CardStatsObject> _cardStatsObjects; 
        public DeckStatsWindow(Deck deck)
        {
            InitializeComponent();
            _cardStatsObjects = new List<CardStatsObject>();
            foreach (var card in deck.Cards)
            {
                _cardStatsObjects.Add(new CardStatsObject(card, deck.Stats));
            }

            ListviewStats.ItemsSource = _cardStatsObjects;
        }
        
    }
}
