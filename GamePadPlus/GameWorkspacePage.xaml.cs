using GamePadPlus.Models;
using GamePadPlus.Services;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GamePadPlus
{
    public partial class GameWorkspacePage : Page
    {
        private readonly LibraryStorageService storageService =
            new LibraryStorageService();

        private Game CurrentGame;

        private ObservableCollection<Game> Games;

        private LibraryPage LibraryPage;

        private bool isLoadingNotes;

        private bool isResettingFormatting;

        private double selectedFontSize = 16.0;

        public GameWorkspacePage(
            Game game,
            ObservableCollection<Game> games,
            LibraryPage libraryPage)
        {
            InitializeComponent();

            CurrentGame = game;
            Games = games;
            LibraryPage = libraryPage;

            GameTitle.Text = CurrentGame.Name;

            LoadNotes();
            LoadCoverImage();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ResetFormattingButtons();
        }

        private void NotesBox_SelectionChanged(
            object sender,
            RoutedEventArgs e)
        {
            if (isLoadingNotes)
            {
                return;
            }

            UpdateFormattingButtons();
        }

        private void UpdateFormattingButtons()
        {
            object boldValue = NotesBox.Selection.GetPropertyValue(
                TextElement.FontWeightProperty);

            object italicValue = NotesBox.Selection.GetPropertyValue(
                TextElement.FontStyleProperty);

            object underlineValue = NotesBox.Selection.GetPropertyValue(
                Inline.TextDecorationsProperty);

            BoldButton.IsChecked =
                boldValue != DependencyProperty.UnsetValue &&
                boldValue.Equals(FontWeights.Bold);

            ItalicButton.IsChecked =
                italicValue != DependencyProperty.UnsetValue &&
                italicValue.Equals(FontStyles.Italic);

            UnderlineButton.IsChecked =
                underlineValue != null &&
                underlineValue != DependencyProperty.UnsetValue &&
                underlineValue.Equals(TextDecorations.Underline);
        }

        private void ResetFormattingButtons()
        {
            BoldButton.IsChecked = false;
            ItalicButton.IsChecked = false;
            UnderlineButton.IsChecked = false;
        }

        private void ResetEditorFormatting()
        {
            if (isResettingFormatting)
            {
                return;
            }

            isResettingFormatting = true;

            try
            {
                TextSelection selection = NotesBox.Selection;

                selection.ApplyPropertyValue(
                    TextElement.FontWeightProperty,
                    FontWeights.Normal);

                selection.ApplyPropertyValue(
                    TextElement.FontStyleProperty,
                    FontStyles.Normal);

                selection.ApplyPropertyValue(
                    Inline.TextDecorationsProperty,
                    null);

                selection.ApplyPropertyValue(
                    TextElement.FontSizeProperty,
                    16.0);

                BoldButton.IsChecked = false;
                ItalicButton.IsChecked = false;
                UnderlineButton.IsChecked = false;

                FontSizeBox.SelectedIndex = 2;
            }
            finally
            {
                isResettingFormatting = false;
            }
        }

        private void LoadNotes()
        {
            if (string.IsNullOrWhiteSpace(CurrentGame.Notes))
            {
                return;
            }

            isLoadingNotes = true;

            TextRange textRange = new TextRange(
                NotesBox.Document.ContentStart,
                NotesBox.Document.ContentEnd
            );

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    byte[] bytes =
                        System.Text.Encoding.UTF8.GetBytes(
                            CurrentGame.Notes);

                    stream.Write(bytes, 0, bytes.Length);
                    stream.Position = 0;

                    textRange.Load(stream, DataFormats.Xaml);
                }
            }
            catch
            {
                textRange.Text = CurrentGame.Notes;
            }

            isLoadingNotes = false;
        }

        private void LoadCoverImage()
        {
            if (string.IsNullOrWhiteSpace(CurrentGame.ImageFileName))
            {
                return;
            }

            string coverPath = Path.Combine(
                storageService.GetCoversFolder(),
                CurrentGame.ImageFileName
            );

            if (!File.Exists(coverPath))
            {
                return;
            }

            BitmapImage coverImage = new BitmapImage();

            coverImage.BeginInit();
            coverImage.UriSource = new Uri(coverPath);
            coverImage.CacheOption = BitmapCacheOption.OnLoad;
            coverImage.CreateOptions =
                BitmapCreateOptions.IgnoreImageCache;
            coverImage.EndInit();

            CoverImage.Source = coverImage;
        }

        private void SaveNotes()
        {
            TextRange textRange = new TextRange(
                NotesBox.Document.ContentStart,
                NotesBox.Document.ContentEnd
            );

            using (MemoryStream stream = new MemoryStream())
            {
                textRange.Save(stream, DataFormats.Xaml);

                stream.Position = 0;

                using (StreamReader reader =
                    new StreamReader(stream))
                {
                    CurrentGame.Notes = reader.ReadToEnd();
                }
            }
        }

        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            NotesBox.Focus();

            EditingCommands.ToggleBold.Execute(null, NotesBox);
        }

        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            NotesBox.Focus();

            EditingCommands.ToggleItalic.Execute(null, NotesBox);
        }

        private void Underline_Click(object sender, RoutedEventArgs e)
        {
            NotesBox.Focus();

            EditingCommands.ToggleUnderline.Execute(null, NotesBox);
        }

        private void Bullet_Click(object sender, RoutedEventArgs e)
        {
            NotesBox.Focus();

            EditingCommands.ToggleBullets.Execute(null, NotesBox);
        }

        private void FontSizeBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (FontSizeBox.SelectedItem is ComboBoxItem selectedItem)
            {
                if (double.TryParse(
                    selectedItem.Content.ToString(),
                    out double fontSize))
                {
                    TextSelection selection = NotesBox.Selection;

                    selection.ApplyPropertyValue(
                        TextElement.FontSizeProperty,
                        fontSize
                    );

                    NotesBox.Focus();
                }
            }
        }

        private void NotesBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            if (CurrentGame == null ||
                isLoadingNotes ||
                isResettingFormatting)
            {
                return;
            }

            TextRange textRange = new TextRange(
                NotesBox.Document.ContentStart,
                NotesBox.Document.ContentEnd);

            if (string.IsNullOrWhiteSpace(textRange.Text))
            {
                ResetEditorFormatting();
            }

            SaveNotes();
            storageService.SaveLibrary(Games);
        }

        private void ChooseCover_Click(
            object sender,
            RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = "Choose a cover image";
            dialog.Filter =
                "Image files|*.png;*.jpg;*.jpeg;*.webp";

            bool? result = dialog.ShowDialog();

            if (result != true)
            {
                return;
            }

            string selectedFile = dialog.FileName;

            string extension = Path.GetExtension(selectedFile);

            string fileName =
                CurrentGame.Id + extension;

            string coversFolder =
                storageService.GetCoversFolder();

            Directory.CreateDirectory(coversFolder);

            string destinationFile =
                Path.Combine(coversFolder, fileName);

            File.Copy(
                selectedFile,
                destinationFile,
                true
            );

            CurrentGame.ImageFileName = fileName;

            CoverImage.Source = null;

            BitmapImage coverImage =
                new BitmapImage();

            coverImage.BeginInit();
            coverImage.UriSource =
                new Uri(destinationFile);

            coverImage.CacheOption =
                BitmapCacheOption.OnLoad;

            coverImage.CreateOptions =
                BitmapCreateOptions.IgnoreImageCache;

            coverImage.EndInit();

            CoverImage.Source = coverImage;

            storageService.SaveLibrary(Games);

            MessageBox.Show("Cover image saved!");
        }

        private void Back_Click(
            object sender,
            RoutedEventArgs e)
        {
            SaveNotes();

            storageService.SaveLibrary(Games);

            LibraryPage.RefreshGames();

            NavigationService?.GoBack();
        }

        private void NotesBox_MouseLeftButtonUp(
    object sender,
    MouseButtonEventArgs e)
        {
            NotesBox.Focus();

            NotesBox.Selection.ApplyPropertyValue(
                TextElement.FontSizeProperty,
                selectedFontSize);

            UpdateFormattingButtons();
        }
    }
}