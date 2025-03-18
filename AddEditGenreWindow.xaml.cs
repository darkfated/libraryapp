using System.Windows;

namespace libraryapp
{
    public partial class AddEditGenreWindow : Window
    {
        private Genre _genre;
        public AddEditGenreWindow(Genre genre = null)
        {
            InitializeComponent();
            if (genre != null)
            {
                _genre = genre;
                FillFields();
            }
            else
            {
                _genre = new Genre();
            }
        }
        private void FillFields()
        {
            NameTextBox.Text = _genre.Name;
            DescriptionTextBox.Text = _genre.Description;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _genre.Name = NameTextBox.Text;
            _genre.Description = DescriptionTextBox.Text;
            try
            {
                using (var context = new LibraryContext())
                {
                    if (_genre.Id == 0)
                        context.Genres.Add(_genre);
                    else
                        context.Genres.Update(_genre);
                    context.SaveChanges();
                }
                DialogResult = true;
                Close();
            }
            catch (System.Exception ex)
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
