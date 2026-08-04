using System.Windows;
using GamePadPlus.Models;

namespace GamePadPlus
{
    public partial class GameWorkspace : Window
    {
        private Game CurrentGame;

        public GameWorkspace(Game game)
        {
            InitializeComponent();

            CurrentGame = game;

            GameTitle.Text = CurrentGame.Name;
        }
    }
}