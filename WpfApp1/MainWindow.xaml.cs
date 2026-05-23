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

        private void MyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbbx_typeOfMachine.SelectedItem is ComboBoxItem selectedItem)
            {
                // Значение сохраняется в переменную
                string selectedParam = selectedItem.Content.ToString();

                // Здесь можно сразу вызвать метод, использующий эту переменную
                System.Diagnostics.Debug.WriteLine($"Выбрано: {selectedParam}");
            }
        }


    }
}
