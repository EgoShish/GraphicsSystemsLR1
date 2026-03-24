using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicsPanelStanki
{
    public partial class Form1 : Form
    {
        private Database DB;
        private string[] headerText;
        public Form1()
        {
            InitializeComponent();
            DB = new Database();
            headerText = new string[2];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var table = DB.GetTypesList();
            foreach (DataRow t in table.Rows)
            {
                cmbbx_typeOfMachine.Items.Add((string)(t.ItemArray[1]));
            }
            cmbbx_typeOfMachine.SelectedIndex = 0;
        }

        private void btn_cnc_Click(object sender, EventArgs e)
        {
            var table = DB.GetMachineStatusLogs(((ItemExtractor)cmbbx_nameOfMachine.SelectedItem).ClearName);
            dataGrid_MachineMessages.DataSource = table;
        }

        private void btn_loadLineDiagram_Click(object sender, EventArgs e)
        {

        }

        private void btn_loadColumDiagram_Click(object sender, EventArgs e)
        {

        }

        private void cmbbx_typeOfMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbbx_nameOfMachine.Items.Clear();
            headerText[0] = cmbbx_typeOfMachine.Text;
            var table = DB.GetModelsByTypeName(cmbbx_typeOfMachine.Text);
            foreach (DataRow t in table.Rows)
            {
                cmbbx_nameOfMachine.Items.Add(new ItemExtractor { 
                                    FullName = t.ItemArray[0].ToString() + " - " + t.ItemArray[1].ToString(), 
                                    ClearName = t.ItemArray[0].ToString(),
                                    ImageName = t.ItemArray[2].ToString()
                });
            }
            cmbbx_nameOfMachine.SelectedIndex = 0;
        }

        private void cmbbx_nameOfMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            headerText[1] = cmbbx_nameOfMachine.Text;
            var image = ((ItemExtractor)cmbbx_nameOfMachine.SelectedItem).ImageName;
            pictureBox_stanki.Image = Image.FromFile("stankiDB/stankiDBpictures/" + image);
            FillHeader();
        }
        public void FillHeader()
        {
            lbl_Header.Text = string.Join(", ", headerText);
        }
    }
}


