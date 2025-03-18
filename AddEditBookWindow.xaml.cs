using System;
using System.Linq;
using System.Windows;

namespace libraryapp
{
    public partial class AddEditBookWindow : Window
    {
        private Book _book;
        public AddEditBookWindow(Book book = null)
        {
            InitializeComponent();
            LoadAuthors();
            LoadGenres();
            if (book != null)
            {
                _book = book;
                FillFields();
            }
            else
            {
                _book = new Book();
            }
        }
        private void LoadAuthors()
        {
            using (var context = new LibraryContext())
            {
                AuthorComboBox.ItemsSource = context.Authors.ToList();
            }
            AuthorComboBox.DisplayMemberPath = "FirstName";
            AuthorComboBox.SelectedValuePath = "Id";
        }
        private void LoadGenres()
        {
            using (var context = new LibraryContext())
            {
                GenreComboBox.ItemsSource = context.Genres.ToList();
            }
            GenreComboBox.DisplayMemberPath = "Name";
            GenreComboBox.SelectedValuePath = "Id";
        }
        private void FillFields()
        {
            TitleTextBox.Text = _book.Title;
            PublishYearTextBox.Text = _book.PublishYear.ToString();
            ISBNTextBox.Text = _book.ISBN;
            QuantityTextBox.Text = _book.QuantityInStock.ToString();
            AuthorComboBox.SelectedValue = _book.AuthorId;
            GenreComboBox.SelectedValue = _book.GenreId;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _book.Title = TitleTextBox.Text;
            _book.ISBN = ISBNTextBox.Text;
            _book.PublishYear = int.TryParse(PublishYearTextBox.Text, out int year) ? year : 0;
            _book.QuantityInStock = int.TryParse(QuantityTextBox.Text, out int quantity) ? quantity : 0;
            if (AuthorComboBox.SelectedValue != null)
                _book.AuthorId = (int)AuthorComboBox.SelectedValue;
            if (GenreComboBox.SelectedValue != null)
                _book.GenreId = (int)GenreComboBox.SelectedValue;
            try
            {
                using (var context = new LibraryContext())
                {
                    if (_book.Id == 0)
                        context.Books.Add(_book);
                    else
                        context.Books.Update(_book);
                    context.SaveChanges();
                }
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
