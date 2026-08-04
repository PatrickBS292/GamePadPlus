using System.Windows;
using System.Windows.Controls;
using GamePadPlus.Models;

namespace GamePadPlus
{
    public partial class GameWorkspacePage : Page
    {
        private Game CurrentGame;

        public GameWorkspacePage(Game game)
        {
            InitializeComponent();

            CurrentGame = game;

            GameTitle.Text = CurrentGame.Name;
            NotesBox.Text = CurrentGame.Notes;
        }

        private void NotesBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CurrentGame != null)
            {
                CurrentGame.Notes = NotesBox.Text;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}