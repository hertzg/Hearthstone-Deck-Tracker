using System;
using System.ComponentModel;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using MahApps.Metro.Controls;

namespace Hearthstone_Deck_Tracker
{
	/// <summary>
	/// Interaction logic for ExternalFlyout.xaml
	/// </summary>
	public partial class ExternalFlyout : MetroWindow
	{
		public ExternalFlyout()
		{
			InitializeComponent();
		}

		private bool _mainWindowMoved;
		private void MetroWindow_LocationChanged(object sender, EventArgs e)
		{
			if(!IsLoaded || !IsVisible) return;
			if(!_mainWindowMoved)
				Helper.MainWindow.SetLocation(Top, Left - Helper.MainWindow.Width);
		}

		public void SetLocation(double top, double left)
		{
			_mainWindowMoved = true;
			Top = top;
			Left = left;
			_mainWindowMoved = false;
		}

		private bool closeStoryBoardCompleted = false;

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			if (!closeStoryBoardCompleted)
			{
				var a = FindResource("StoryboardClose") as Storyboard;
				a.Begin();
				e.Cancel = true;
			}
		}

		private void closeStoryBoard_Completed(object sender, EventArgs e)
		{
			closeStoryBoardCompleted = true;
			this.Close();
		}

	}
}