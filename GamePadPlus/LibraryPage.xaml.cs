using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using GamePadPlus.Models;
using GamePadPlus.Services;

namespace GamePadPlus
{
    public partial class LibraryPage : Page
    {
        private readonly LibraryStorageService storageService = new LibraryStorageService();

        public void RefreshGames()
        {
            GameList.ItemsSource = null;
            GameList.ItemsSource = Games;
        }

        public ObservableCollection<Game> Games { get; set; }
            = new ObservableCollection<Game>();

        public LibraryPage()
        {
            InitializeComponent();

            List<Game> savedGames = storageService.LoadLibrary();

            foreach (Game game in savedGames)
            {
                Games.Add(game);
            }

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

            NavigationService?.Navigate(
            new GameWorkspacePage(selectedGame, Games, this));
        }

        private void DeleteGame_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            Game selectedGame = (Game)clickedButton.DataContext;

            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete \"{selectedGame.Name}\"?",
                "Delete Game",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Games.Remove(selectedGame);

                storageService.SaveLibrary(Games);
            }
        }

    }
}
