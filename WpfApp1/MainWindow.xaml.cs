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
        public string ImagePath;
        public MainWindow()
        {
            InitializeComponent();
            DB = new Database();
            headerText = new string[2];
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var table = DB.GetTypesList();
            if (table == null)
            {
                MessageBox.Show("SQL not connect");
            }
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
        private void cmbbx_typeOfMachine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbbx_nameOfMachine.Items.Clear();
            headerText[0] = cmbbx_typeOfMachine.SelectedItem.ToString();
            var table = DB.GetModelsByTypeName(cmbbx_typeOfMachine.Text);
            foreach (DataRow t in table.Rows)
            {
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
        private void cmbbx_nameOfMachine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            headerText[1] = cmbbx_nameOfMachine.SelectedItem.ToString();
            FillHeader();
            /*
            cb_dayTime.Items.Clear();
            cb_timeFrom.Items.Clear();
            cb_timeTo.Items.Clear();
            cb_dayTime.Text = string.Empty;
            cb_timeFrom.Text = string.Empty;
            cb_timeTo.Text = string.Empty;
            */
            var table = DB.GetMachineLoadHistory(((ItemExtractor)cmbbx_nameOfMachine.SelectedItem).ClearName);
            foreach (DataRow t in table.Rows)
            {
                /*
                cb_dayTime.Items.Add(t.ItemArray[0]);
                cb_timeFrom.Items.Add(t.ItemArray[0]);
                cb_timeTo.Items.Add(t.ItemArray[0]);
                */
            }
        }
        public void FillHeader()
        {
            lbl_Header.Content = string.Join(", ", headerText);
        }
    }
}
