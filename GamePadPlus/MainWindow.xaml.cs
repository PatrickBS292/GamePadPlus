using System.Windows;

namespace GamePadPlus
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Navigate(new LibraryPage());
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new LibraryPage());
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new InfoPage());
        }
    }
}

