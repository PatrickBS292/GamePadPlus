using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using GamePadPlus.Models;

namespace GamePadPlus
{
    public partial class LibraryPage : Page
    {
        public ObservableCollection<Game> Games { get; set; }
            = new ObservableCollection<Game>();

        public LibraryPage()
        {
            InitializeComponent();

            GameList.ItemsSource = Games;
        }

        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            CreateGameWindow window = new CreateGameWindow(Games);
            window.Show();
        }

        private void GameCard_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            Game selectedGame = (Game)clickedButton.DataContext;

            NavigationService?.Navigate(new GameWorkspacePage(selectedGame));
        }
    }
}