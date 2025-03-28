using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Forms = System.Windows.Forms;

namespace PT8WPF
{
    public partial class MainWindow : Window
    {
        private string initialDirectoryPath;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClearTreeView()
        {
            treeView.Items.Clear();
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e) // okno wyboru folderu
        {
            using (var dialog = new Forms.FolderBrowserDialog())
            {
                dialog.Description = "Select directory to open";
                Forms.DialogResult result = dialog.ShowDialog();
                if (result == Forms.DialogResult.OK)
                {
                    string selectedPath = dialog.SelectedPath; // wybrana ścieżka przez eksplorator plików
                    initialDirectoryPath = selectedPath;
                    ClearTreeView(); // wyczyszczenie obecnego wyświelanego drzewa
                    FillTreeView(selectedPath);
                }
            }
        }

        private void FillTreeView(string rootPath)
        {
            string rootName = new DirectoryInfo(rootPath).Name;
            System.Windows.Controls.TreeViewItem rootNode = new System.Windows.Controls.TreeViewItem();
            rootNode.Header = rootName;
            rootNode.Tag = rootPath;
            FillTreeViewHelper(rootPath, rootNode);
            treeView.Items.Add(rootNode); // dodanie korzenia do treeView
        }

        private void FillTreeViewHelper(string path, System.Windows.Controls.TreeViewItem parentNode) // rekurencyjne uzupełnienie drzewa 
        {
            try
            {
                string[] directories = Directory.GetDirectories(path);
                foreach (string directory in directories) // dodajemy wszystkie foldery i wywołujemy dalej rekurencyjnie dla nich uzupełnienie drzewa
                {
                    System.Windows.Controls.TreeViewItem node = new System.Windows.Controls.TreeViewItem();
                    node.Header = System.IO.Path.GetFileName(directory);
                    node.Tag = directory;
                    node.MouseRightButtonDown += TreeViewItem_MouseRightButtonDown; // event handler
                    parentNode.Items.Add(node);
                    FillTreeViewHelper(directory, node);
                }

                string[] files = Directory.GetFiles(path);
                foreach (string file in files) // dodajemy wszystkie pliki
                {
                    System.Windows.Controls.TreeViewItem node = new System.Windows.Controls.TreeViewItem();
                    node.Header = System.IO.Path.GetFileName(file);
                    node.Tag = file;
                    node.MouseRightButtonDown += TreeViewItem_MouseRightButtonDown; // event handler
                    parentNode.Items.Add(node);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e) // wyjście z okna
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void TreeViewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                TreeViewItem selectedItem = sender as TreeViewItem;
                if (selectedItem != null && File.Exists(selectedItem.Tag.ToString())) // jeśli jest to plik, to umożliwiamy otworzenie
                {
                    ContextMenu contextMenu = new ContextMenu();

                    MenuItem openMenuItem = new MenuItem();
                    openMenuItem.Header = "Open";
                    openMenuItem.Click += OpenFileMenuItem_Click; // przy kliknięciu funkcja odpalająca okienko z treścią pliku

                    MenuItem deleteMenuItem = new MenuItem();
                    deleteMenuItem.Header = "Delete";
                    deleteMenuItem.Click += DeleteMenuItem_Click;

                    contextMenu.Items.Add(openMenuItem); // pokazanie Open
                    contextMenu.Items.Add(deleteMenuItem);

                    selectedItem.ContextMenu = contextMenu;

                    UpdateStatusBar(selectedItem.Tag.ToString());
                }
                else if (selectedItem != null && Directory.Exists(selectedItem.Tag.ToString())) // jeśli jest to directory, to odpalamy funkcję ustawiającą możliwość stworzenia pliku
                {
                    ContextMenu contextMenu = CreateContextMenu(isFolder: true);
                    selectedItem.ContextMenu = contextMenu;

                    UpdateStatusBar(selectedItem.Tag.ToString());
                }
            }
        }


        private ContextMenu CreateContextMenu(bool isFolder)
        {
            ContextMenu contextMenu = new ContextMenu();

            if (isFolder)
            {
                MenuItem createFileMenuItem = new MenuItem();
                createFileMenuItem.Header = "Create File";
                createFileMenuItem.Click += CreateFileMenuItem_Click; // na kliknięcie odpalana jest funkcja umożliwiająca wybranie opcji tworzonego pliku
                MenuItem deleteMenuItem = new MenuItem();
                deleteMenuItem.Header = "Delete";
                deleteMenuItem.Click += DeleteMenuItem_Click;
                contextMenu.Items.Add(createFileMenuItem);
                contextMenu.Items.Add(deleteMenuItem);
            }

            return contextMenu;
        }

        private bool IsValidFileName(string fileName) // sprawdzamy czy plik (nie katalog) ma poprawną nazwę i typ
        {
            string pattern = @"^[\w~.-]{1,12}\.(txt|php|html)$";
            return Regex.IsMatch(fileName, pattern);
        }

        private void CreateFileMenuItem_Click(object sender, RoutedEventArgs e) // tworzenie pliku
        {
            TreeViewItem selectedItem = treeView.SelectedItem as TreeViewItem;
            if (selectedItem != null && Directory.Exists(selectedItem.Tag.ToString()))
            {
                string selectedDirectory = selectedItem.Tag.ToString();
                CreateElementForm form = new CreateElementForm();
                if (form.ShowDialog() == true)
                {
                    string name = form.ElementName; // sprawdzamy ustawienia, nazwę i inne właściwości
                    bool isFile = form.IsFile;
                    bool isReadOnly = form.IsReadOnly;
                    bool isArchive = form.IsArchive;
                    bool isHidden = form.IsHidden;
                    bool isSystem = form.IsSystem;

                    try
                    {
                        string newPath = Path.Combine(selectedDirectory, name); // tworzymy ścieżkę z wybraną nazwą

                        if (isFile) // jeśli plik
                        {
                            if (!IsValidFileName(name)) // sprawdzanie regexu
                            {
                                System.Windows.MessageBox.Show("Invalid file name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            else
                            {
                                using (StreamWriter writer = new StreamWriter(newPath))
                                {

                                }
                                FileAttributes attributes = 0;
                                if (isReadOnly)
                                    attributes |= FileAttributes.ReadOnly;
                                if (isArchive)
                                    attributes |= FileAttributes.Archive;
                                if (isHidden)
                                    attributes |= FileAttributes.Hidden;
                                if (isSystem)
                                    attributes |= FileAttributes.System;

                                File.SetAttributes(newPath, attributes);
                            }
                        }
                        else // jeśli katalog
                        {
                            Directory.CreateDirectory(newPath);

                            DirectoryInfo directoryInfo = new DirectoryInfo(newPath);
                            FileAttributes attributes = 0;
                            if (isReadOnly)
                                attributes |= FileAttributes.ReadOnly;
                            if (isArchive)
                                attributes |= FileAttributes.Archive;
                            if (isHidden)
                                attributes |= FileAttributes.Hidden;
                            if (isSystem)
                                attributes |= FileAttributes.System;

                            directoryInfo.Attributes = attributes;
                        }

                        ClearTreeView(); // clearujemy i ustawiamy nowy widok
                        FillTreeView(initialDirectoryPath);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Error creating element: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e) // usuwanie pliku / directory
        {
            System.Windows.Controls.TreeViewItem selectedItem = treeView.SelectedItem as System.Windows.Controls.TreeViewItem;
            if (selectedItem != null)
            {
                string path = selectedItem.Tag.ToString();
                if (File.Exists(path)) // jeśli plik
                {
                    File.Delete(path);
                }
                else if (Directory.Exists(path)) // jeśli directory
                {
                    Directory.Delete(path, true);
                }
                else
                {
                    System.Windows.MessageBox.Show("Item not found.");
                }

                selectedItem.Items.Clear();
                if (selectedItem.Parent is System.Windows.Controls.TreeViewItem parentItem)
                {
                    parentItem.Items.Remove(selectedItem); // usuwamy z listy itemów rodzica
                }
            }
        }

        private void OpenFileMenuItem_Click(object sender, RoutedEventArgs e) // otwieranie zawartości pliku
        {
            MenuItem menuItem = sender as MenuItem;
            ContextMenu contextMenu = menuItem.Parent as ContextMenu;
            TreeViewItem selectedItem = contextMenu.PlacementTarget as TreeViewItem;

            if (selectedItem != null && File.Exists(selectedItem.Tag.ToString()))
            {
                string filePath = selectedItem.Tag.ToString();
                try
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string fileContent = sr.ReadToEnd(); // zczytujemy zawartość do stringa i go wyświetlamy
                        ShowFileContent(fileContent);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void ShowFileContent(string content) // tworzenie scrollowalnego okna do wyświetlenia zawartości 
        {
            Window fileContentWindow = new Window();
            fileContentWindow.Title = "File Content";
            fileContentWindow.Width = 400;
            fileContentWindow.Height = 300;

            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            TextBlock textBlock = new TextBlock();
            textBlock.Text = content;
            textBlock.TextWrapping = TextWrapping.Wrap;
            scrollViewer.Content = textBlock;
            fileContentWindow.Content = scrollViewer;
            fileContentWindow.ShowDialog();
        }

        private void UpdateStatusBar(string path) // pasek stanu plików i katalogów
        {
            if (File.Exists(path))
            {
                FileAttributes attributes = File.GetAttributes(path);
                string status = "";
                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    status += "r";
                else
                    status += "-";
                if ((attributes & FileAttributes.Archive) == FileAttributes.Archive)
                    status += "a";
                else
                    status += "-";
                if ((attributes & FileAttributes.System) == FileAttributes.System)
                    status += "s";
                else
                    status += "-";
                if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    status += "h";
                else
                    status += "-";
                statusTextBlock.Text = status;
            }
            else if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                string status = "";
                if ((directoryInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    status += "r";
                else
                    status += "-";
                if ((directoryInfo.Attributes & FileAttributes.Archive) == FileAttributes.Archive)
                    status += "a";
                else
                    status += "-";
                if ((directoryInfo.Attributes & FileAttributes.System) == FileAttributes.System)
                    status += "s";
                else
                    status += "-";
                if ((directoryInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    status += "h";
                else
                    status += "-";
                statusTextBlock.Text = status;
            }
            else
            {
                statusTextBlock.Text = "";
            }
        }
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) // wyświetlanie na barze zawartości ustalanej przez UpdateStatusBar(), odpalane przy zaznaczeniu elementu w drzewie
        {
            TreeViewItem selectedItem = treeView.SelectedItem as TreeViewItem;
            if (selectedItem != null)
            {
                string path = selectedItem.Tag.ToString();
                UpdateStatusBar(path);
            }
        }
    }
}
