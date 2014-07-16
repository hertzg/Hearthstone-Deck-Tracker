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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Stats;

namespace Hearthstone_Deck_Tracker
{
    /// <summary>
    /// Interaction logic for GameHistoryItem.xaml
    /// </summary>
    public partial class GameHistoryItem
    {

        public GameHistoryItem(TurnStats.Play play)
        {
            InitializeComponent();

            LblItem.Content = play.Type;
            if (!string.IsNullOrEmpty(play.CardId))
            {
                SetCard(play.CardId);
            }
            else
            {
                GridCard.Visibility = Visibility.Collapsed;
            }

        }

        public void SetCard(string cardId)
        {
            var card = Game.GetCardFromId(cardId);
            if (card != null)
            {
                GridCard.Background = card.Background;
                TxtCardCost.Text = card.Cost.ToString();
                TxtCardName.Text = card.Name;
                CardTooltip.SetValue(DataContextProperty, card);
            }
        }

    }
}
