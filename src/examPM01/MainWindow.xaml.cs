using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace examPM01
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<City> inputCities;
        private CityControl cityControl;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ValidateCity(City c, int row)
        {
            if (string.IsNullOrWhiteSpace(c.Name))
                throw new Exception("Строка " + (row + 1) + ": не указано название города");

            if (string.IsNullOrWhiteSpace(c.FoundationDate))
                throw new Exception("Строка " + (row + 1) + ": не указана дата основания");

            int year;
            if (!int.TryParse(c.FoundationDate, out year) || year < 0 || year == 0)
                throw new Exception("Строка " + (row + 1) + ": дата основания не может быть отрицательной и нулевой");

            if (c.Population <= 0)
                throw new Exception("Строка " + (row + 1) + ": некорректное количество жителей");
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            int n;
            if (!int.TryParse(txtN.Text, out n) || n <= 0)
            {
                MessageBox.Show("Введите целое число больше нуля");
                return;
            }

            inputCities = new ObservableCollection<City>();
            for (int i = 0; i < n; i++)
                inputCities.Add(new City());

            dgInput.ItemsSource = inputCities;
            dgUnsorted.ItemsSource = null;
            dgSorted.ItemsSource = null;
        }

        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            if (inputCities == null || inputCities.Count == 0)
            {
                MessageBox.Show("Сначала создайте массив");
                return;
            }

            dgInput.CommitEdit(DataGridEditingUnit.Cell, true);
            dgInput.CommitEdit(DataGridEditingUnit.Row, true);
            dgInput.Items.Refresh();

            try
            {
                for (int i = 0; i < inputCities.Count; i++)
                    ValidateCity(inputCities[i], i);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            cityControl = new CityControl(inputCities.Count);
            for (int i = 0; i < inputCities.Count; i++)
            {
                cityControl.Cities[i].Name = inputCities[i].Name;
                cityControl.Cities[i].FoundationDate = inputCities[i].FoundationDate;
                cityControl.Cities[i].Population = inputCities[i].Population;
            }

            dgUnsorted.ItemsSource = cityControl.Cities.Select(c => new City
            {
                Name = c.Name,
                FoundationDate = c.FoundationDate,
                Population = c.Population
            }).ToList();

            cityControl.Sort();

            dgSorted.ItemsSource = cityControl.Cities.ToList();
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cityControl == null)
            {
                MessageBox.Show("Сначала выполните сортировку");
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Текстовый файл (*.txt)|*.txt";
            dialog.FileName = "cities.txt";

            if (dialog.ShowDialog() == true)
            {
                cityControl.SaveToFile(dialog.FileName);
                MessageBox.Show("Файл сохранён");
            }
        }
    }
}