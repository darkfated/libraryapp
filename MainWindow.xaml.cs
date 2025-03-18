using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace libraryapp
{
    public partial class MainWindow : Window
    {
        private LibraryContext _context;
        public MainWindow()
        {
            InitializeComponent();
            _context = new LibraryContext();
            _context.Database.EnsureCreated();
            LoadFilters();
            LoadData();
        }
        private void LoadFilters()
        {
            var authors = _context.Authors.ToList();
            authors.Insert(0, new Author { Id = 0, FirstName = "Все", LastName = "" });
            AuthorFilterComboBox.ItemsSource = authors;
            AuthorFilterComboBox.DisplayMemberPath = "FirstName";
            AuthorFilterComboBox.SelectedValuePath = "Id";
            AuthorFilterComboBox.SelectedIndex = 0;
            var genres = _context.Genres.ToList();
            genres.Insert(0, new Genre { Id = 0, Name = "Все", Description = "" });
            GenreFilterComboBox.ItemsSource = genres;
            GenreFilterComboBox.DisplayMemberPath = "Name";
            GenreFilterComboBox.SelectedValuePath = "Id";
            GenreFilterComboBox.SelectedIndex = 0;
        }
        private void LoadData()
        {
            var booksQuery = _context.Books.Include(b => b.Author).Include(b => b.Genre).AsQueryable();
            if (!string.IsNullOrEmpty(SearchTextBox.Text))
                booksQuery = booksQuery.Where(b => b.Title.Contains(SearchTextBox.Text));
            if (AuthorFilterComboBox.SelectedItem is Author selectedAuthor && selectedAuthor.Id != 0)
                booksQuery = booksQuery.Where(b => b.AuthorId == selectedAuthor.Id);
            if (GenreFilterComboBox.SelectedItem is Genre selectedGenre && selectedGenre.Id != 0)
                booksQuery = booksQuery.Where(b => b.GenreId == selectedGenre.Id);
            var books = booksQuery.ToList().Select(b => new
            {
                b.Id,
                b.Title,
                AuthorFullName = b.Author.FirstName + " " + b.Author.LastName,
                b.PublishYear,
                b.ISBN,
                b.Genre,
                b.QuantityInStock
            }).ToList();
            BooksDataGrid.ItemsSource = books;
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadData();
        }
        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addEditWindow = new AddEditBookWindow();
            if (addEditWindow.ShowDialog() == true)
                LoadData();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem == null)
                return;
            dynamic selected = BooksDataGrid.SelectedItem;
            int id = selected.Id;
            var book = _context.Books.Include(b => b.Author).Include(b => b.Genre).FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                var addEditWindow = new AddEditBookWindow(book);
                if (addEditWindow.ShowDialog() == true)
                    LoadData();
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem == null)
                return;
            dynamic selected = BooksDataGrid.SelectedItem;
            int id = selected.Id;
            var book = _context.Books.Find(id);
            if (book != null)
            {
                try
                {
                    _context.Books.Remove(book);
                    _context.SaveChanges();
                    LoadData();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void ManageAuthors_Click(object sender, RoutedEventArgs e)
        {
            var manageAuthorsWindow = new ManageAuthorsWindow();
            manageAuthorsWindow.ShowDialog();
            LoadFilters();
            LoadData();
        }
        private void ManageGenres_Click(object sender, RoutedEventArgs e)
        {
            var manageGenresWindow = new ManageGenresWindow();
            manageGenresWindow.ShowDialog();
            LoadFilters();
            LoadData();
        }
    }
}
