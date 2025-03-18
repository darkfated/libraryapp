using System;
using System.Windows;

namespace libraryapp
{
    public partial class AddEditAuthorWindow : Window
    {
        private Author _author;
        public AddEditAuthorWindow(Author author = null)
        {
            InitializeComponent();
            if (author != null)
            {
                _author = author;
                FillFields();
            }
            else
            {
                _author = new Author();
            }
        }
        private void FillFields()
        {
            FirstNameTextBox.Text = _author.FirstName;
            LastNameTextBox.Text = _author.LastName;
            BirthDatePicker.SelectedDate = _author.BirthDate;
            CountryTextBox.Text = _author.Country;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _author.FirstName = FirstNameTextBox.Text;
            _author.LastName = LastNameTextBox.Text;
            _author.BirthDate = BirthDatePicker.SelectedDate ?? DateTime.Now;
            _author.Country = CountryTextBox.Text;
            try
            {
                using (var context = new LibraryContext())
                {
                    if (_author.Id == 0)
                        context.Authors.Add(_author);
                    else
                        context.Authors.Update(_author);
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
