using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Globalization;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pliki CSV z separatorem (,)|*.csv|Pliki CSV z separatorem (;)|*.csv";
            openFileDialog.Title = "Otwórz plik CSV";

            if (openFileDialog.ShowDialog() == true)
            {
                listaUczniow.Items.Clear();
                string filePath = openFileDialog.FileName;
                int selectedFilterIndex = openFileDialog.FilterIndex;
                string delimiter = selectedFilterIndex == 1 ? "," : ";";

                try
                {
                    Encoding encoding = DetectEncoding(filePath);
                    var lines = File.ReadAllLines(filePath, encoding);

                    foreach (var line in lines)
                    {
                        string[] columns = ParseCsvLine(line, delimiter);
                        if (columns.Length >= 9)
                        {
                            Osoba uczen = new Osoba
                            {
                                m_strPesel = columns[0],
                                m_strImie = columns[1],
                                m_strDrugieImie = columns[2],
                                m_strNazwisko = columns[3],
                                m_strDataUr = columns[4],
                                m_strTelefon = columns[5],
                                m_strAdres = columns[6],
                                m_strMiejscowosc = columns[7],
                                m_strKodPocztowy = columns[8]
                            };
                            listaUczniow.Items.Add(uczen);
                        }
                    }
                    MessageBox.Show($"Wczytano {listaUczniow.Items.Count} uczniów", "Sukces",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas wczytywania pliku: {ex.Message}", "Błąd",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (listaUczniow.Items.Count == 0)
            {
                MessageBox.Show("Brak danych do zapisania", "Ostrzeżenie",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pliki CSV z separatorem (,)|*.csv|Pliki CSV z separatorem (;)|*.csv";
            saveFileDialog.Title = "Zapisz jako plik CSV";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                string delimiter = saveFileDialog.FilterIndex == 1 ? "," : ";";

                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                    {
                        // Nagłówki kolumn
                        writer.WriteLine($"PESEL{delimiter}Imię{delimiter}Drugie imię{delimiter}Nazwisko{delimiter}" +
                                       $"Data urodzenia{delimiter}Telefon{delimiter}Adres{delimiter}" +
                                       $"Miejscowość{delimiter}Kod pocztowy");

                        foreach (Osoba item in listaUczniow.Items)
                        {
                            string row = $"{EscapeCsv(item.m_strPesel, delimiter)}{delimiter}" +
                                         $"{EscapeCsv(item.m_strImie, delimiter)}{delimiter}" +
                                         $"{EscapeCsv(item.m_strDrugieImie, delimiter)}{delimiter}" +
                                         $"{EscapeCsv(item.m_strNazwisko, delimiter)}{delimiter}" +
                                         $"{EscapeCsv(item.m_strDataUr, delimiter)}{delimiter}" +
                                         $"{EscapeCsv(item.m_strTelefon, delimiter)}{delimiter}" +
                                         $"{EscapeCsv(item.m_strAdres, delimiter)}{delimiter}" +
                                         $"{EscapeCsv(item.m_strMiejscowosc, delimiter)}{delimiter}" +
                                         $"{EscapeCsv(item.m_strKodPocztowy, delimiter)}";
                            writer.WriteLine(row);
                        }
                    }
                    MessageBox.Show("Dane zostały zapisane", "Sukces",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas zapisywania pliku: {ex.Message}", "Błąd",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RemoveSel_Click(object sender, RoutedEventArgs e)
        {
            if (listaUczniow.SelectedItems.Count == 0)
            {
                MessageBox.Show("Wybierz uczniów do usunięcia", "Ostrzeżenie",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Czy na pewno chcesz usunąć zaznaczonych uczniów?", "Potwierdzenie",
                                       MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var selectedItems = listaUczniow.SelectedItems.Cast<Osoba>().ToList();
                foreach (var item in selectedItems)
                {
                    listaUczniow.Items.Remove(item);
                }
            }
        }

        private string[] ParseCsvLine(string line, string delimiter)
        {
            List<string> columns = new List<string>();
            bool inQuotes = false;
            StringBuilder currentColumn = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == delimiter[0] && !inQuotes)
                {
                    columns.Add(currentColumn.ToString());
                    currentColumn.Clear();
                }
                else
                {
                    currentColumn.Append(c);
                }
            }

            columns.Add(currentColumn.ToString());
            return columns.Select(s => s.Trim('"')).ToArray();
        }

        private string EscapeCsv(string value, string delimiter)
        {
            if (string.IsNullOrEmpty(value)) return "";

            if (value.Contains(delimiter) || value.Contains("\"") || value.Contains("\n"))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }
            return value;
        }

        private Encoding DetectEncoding(string filePath)
        {
            var bom = new byte[4];
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode;
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode;
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;

            return Encoding.GetEncoding(1250); // Domyślne kodowanie dla plików CSV w Windows
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aplikacja do zarządzania danymi uczniów\nWersja 1.0", "O programie",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}