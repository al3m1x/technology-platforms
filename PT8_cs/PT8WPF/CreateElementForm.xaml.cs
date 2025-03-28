using System;
using System.IO;
using System.Windows;

namespace PT8WPF
{
    public partial class CreateElementForm : Window
    {
        public string ElementName { get; private set; }
        public bool IsFile { get; private set; }
        public bool IsReadOnly { get; private set; }
        public bool IsArchive { get; private set; }
        public bool IsHidden { get; private set; }
        public bool IsSystem { get; private set; }

        public CreateElementForm()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) // zatwierdzenie formularza i zczytanie wartości z niego
        {
            string elementName = nameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(elementName))
            {
                System.Windows.MessageBox.Show("Element name cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ElementName = elementName;

            if (!fileRadioButton.IsChecked.GetValueOrDefault() && !directoryRadioButton.IsChecked.GetValueOrDefault())
            {
                System.Windows.MessageBox.Show("Please select File or Directory.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IsFile = fileRadioButton.IsChecked.GetValueOrDefault();

            IsReadOnly = readOnlyCheckBox.IsChecked.GetValueOrDefault();
            IsArchive = archiveCheckBox.IsChecked.GetValueOrDefault();
            IsHidden = hiddenCheckBox.IsChecked.GetValueOrDefault();
            IsSystem = systemCheckBox.IsChecked.GetValueOrDefault();

            string path = Path.Combine(Environment.CurrentDirectory, IsFile ? "Files" : "Directories");

            DialogResult = true;
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
