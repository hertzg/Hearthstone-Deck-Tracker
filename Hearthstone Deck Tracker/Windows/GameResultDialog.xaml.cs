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

namespace Hearthstone_Deck_Tracker
{
    /// <summary>
    /// Interaction logic for DeckSelectionDialog.xaml
    /// </summary>
    public partial class GameResultDialog
    {
        public bool Victory;
        public GameResultDialog()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;           
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            Victory = true;
            Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
