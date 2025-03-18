using System.Linq;
using System.Windows;

namespace libraryapp
{
    public partial class ManageAuthorsWindow : Window
    {
        private LibraryContext _context;
        public ManageAuthorsWindow()
        {
            InitializeComponent();
            _context = new LibraryContext();
            LoadData();
        }
        private void LoadData()
        {
            AuthorsDataGrid.ItemsSource = _context.Authors.ToList();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var authorWindow = new AddEditAuthorWindow();
            if (authorWindow.ShowDialog() == true)
                LoadData();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsDataGrid.SelectedItem == null)
                return;
            var author = AuthorsDataGrid.SelectedItem as Author;
            if (author != null)
            {
                var authorWindow = new AddEditAuthorWindow(author);
                if (authorWindow.ShowDialog() == true)
                    LoadData();
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsDataGrid.SelectedItem == null)
                return;
            var author = AuthorsDataGrid.SelectedItem as Author;
            if (author != null)
            {
                try
                {
                    _context.Authors.Remove(author);
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
