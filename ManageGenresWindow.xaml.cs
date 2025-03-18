using System.Linq;
using System.Windows;

namespace libraryapp
{
    public partial class ManageGenresWindow : Window
    {
        private LibraryContext _context;
        public ManageGenresWindow()
        {
            InitializeComponent();
            _context = new LibraryContext();
            LoadData();
        }
        private void LoadData()
        {
            GenresDataGrid.ItemsSource = _context.Genres.ToList();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var genreWindow = new AddEditGenreWindow();
            if (genreWindow.ShowDialog() == true)
                LoadData();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (GenresDataGrid.SelectedItem == null)
                return;
            var genre = GenresDataGrid.SelectedItem as Genre;
            if (genre != null)
            {
                var genreWindow = new AddEditGenreWindow(genre);
                if (genreWindow.ShowDialog() == true)
                    LoadData();
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (GenresDataGrid.SelectedItem == null)
                return;
            var genre = GenresDataGrid.SelectedItem as Genre;
            if (genre != null)
            {
                try
                {
                    _context.Genres.Remove(genre);
                    _context.SaveChanges();
                    LoadData();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
