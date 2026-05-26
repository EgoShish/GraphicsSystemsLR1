using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Database DB;
        private string[] headerText;
        public MainWindow()
        {
            InitializeComponent();
            DB = new Database();
            headerText = new string[2];
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Ваш код из WinForms работает и здесь
            var table = DB.GetTypesList();
            foreach (DataRow t in table.Rows)
            {
                cmbbx_typeOfMachine.Items.Add((string)(t.ItemArray[1]));
            }

            if (cmbbx_typeOfMachine.Items.Count > 0)
            {
                cmbbx_typeOfMachine.SelectedIndex = 0;
            }
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("jvcyuwkvu");
        }

        private void Select(object sender, SelectionChangedEventArgs e)
        {
            if (Box_Click.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedText = selectedItem.Content.ToString();
                MessageBox.Show($"{selectedText}");
            }
        }

        private void MyComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbbx_typeOfMachine.SelectedItem == null) return;

            // В WPF получаем текст выбранного элемента (при условии, что там лежат строки)
            string selectedTypeText = cmbbx_typeOfMachine.SelectedItem.ToString();

            // Очищаем элементы второго комбобокса
            cmbbx_nameOfMachine.Items.Clear();

            // Сохраняем в ваш массив
            headerText[0] = selectedTypeText;

            // Запрашиваем данные из БД
            var table = DB.GetModelsByTypeName(selectedTypeText);

            foreach (DataRow t in table.Rows)
            {
                // Добавляем объекты точно так же, как в WinForms
                cmbbx_nameOfMachine.Items.Add(new ItemExtractor
                {
                    FullName = t.ItemArray[0].ToString() + " - " + t.ItemArray[1].ToString(),
                    ClearName = t.ItemArray[0].ToString(),
                    ImageName = t.ItemArray[2].ToString()
                });
            }

            // Выбираем первый элемент во втором списке, если он не пустой
            if (cmbbx_nameOfMachine.Items.Count > 0)
            {
                cmbbx_nameOfMachine.SelectedIndex = 0;
            }
        }
        private void MyComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Проверяем, что элемент действительно выбран, чтобы избежать ошибок
            if (cmbbx_nameOfMachine.SelectedItem == null) return;

            // Приводим выбранный элемент к типу ItemExtractor
            var selectedMachine = (ItemExtractor)cmbbx_nameOfMachine.SelectedItem;

            // Записываем отображаемый текст в массив
            headerText[1] = selectedMachine.FullName;

            // ЗАГРУЗКА ИЗОБРАЖЕНИЯ В WPF
            try
            {
                string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "stankiDB/stankiDBpictures/", selectedMachine.ImageName);

                if (System.IO.File.Exists(imagePath))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                    bitmap.EndInit();

                    pictureBox_stanki.Source = bitmap; // Отображаем картинку в WPF
                }
                else
                {
                    pictureBox_stanki.Source = null; // Если файла нет, очищаем
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки картинки: {ex.Message}");
                pictureBox_stanki.Source = null;
            }

            // Вызов вашего метода обновления шапки
            FillHeader();

            // ОЧИСТКА КОМБОБОКСОВ В WPF
            cb_dayTime.Items.Clear();
            cb_timeFrom.Items.Clear();
            cb_timeTo.Items.Clear();

            cb_dayTime.SelectedIndex = -1;
            cb_timeFrom.SelectedIndex = -1;
            cb_timeTo.SelectedIndex = -1;

            // ЗАПОЛНЕНИЕ ДАННЫМИ ИЗ БД
            var table = DB.GetMachineLoadHistory(selectedMachine.ClearName);
            foreach (DataRow t in table.Rows)
            {
                // В WPF элементы добавляются так же, но при выводе 
                // простых типов (строки/числа) они отобразятся корректно
                cb_dayTime.Items.Add(t.ItemArray[0]);
                cb_timeFrom.Items.Add(t.ItemArray[0]);
                cb_timeTo.Items.Add(t.ItemArray[0]);
            }
        }

    }
}
