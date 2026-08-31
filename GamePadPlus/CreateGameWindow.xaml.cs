using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GamePadPlus.Models;
using System.Collections.ObjectModel;
using GamePadPlus.Services;

namespace GamePadPlus
{
    public partial class CreateGameWindow : Window
    {
        private readonly LibraryStorageService storageService = new LibraryStorageService();

        public ObservableCollection<Game> Games { get; set; }

        public CreateGameWindow(ObservableCollection<Game> games)
        {
            InitializeComponent();

            Games = games;
        }

        private void CreateGame_Click(object sender, RoutedEventArgs e)
        {
            string GameName = GameNameBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(GameName))
            {
                MessageBox.Show(
                    "Please enter a game name",
                    "GamePad+",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                GameNameBox.Focus();
                return;
            }
            Game newGame = new Game();
            newGame.Name = GameName;
            Games.Add(newGame);
            storageService.SaveLibrary(Games);
            Close();
        }

        private void GameNameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CreateGame_Click(sender, e);
            }
        }

    }
}
