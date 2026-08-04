using System.Windows;

namespace GamePadPlus
{

    public partial class  MainWindow: Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new LibraryPage());
        }
    }
}