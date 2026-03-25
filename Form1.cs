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
            chart_LineDiagram.Series["Температура шпинделя, C"].Points.Clear();
            chart_LineDiagram.Series["Скорость шпинделя, об/мин"].Points.Clear();
            if (cb_dayTime.SelectedItem == null || cb_timeFrom.SelectedItem == null || cb_timeTo.SelectedItem == null)
            {
                var table = DB.GetMachineLoadHistory(((ItemExtractor)cmbbx_nameOfMachine.SelectedItem).ClearName);
                DateTime t1 = (DateTime)table.Rows[0].ItemArray[0];
                foreach (DataRow t in table.Rows)
                {
                    DateTime t2 = (DateTime)t.ItemArray[0];
                    chart_LineDiagram.Series["Температура шпинделя, C"].Points.AddXY(
                        (t2 - t1).TotalMinutes, (float)t.ItemArray[1]);
                    chart_LineDiagram.Series["Скорость шпинделя, об/мин"].Points.AddXY(
                        (t2 - t1).TotalMinutes, (float)t.ItemArray[2]);
                }
            }
            else
            {
                var table = DB.GetMachineLoadHistoryByPeriod(((ItemExtractor)cmbbx_nameOfMachine.SelectedItem).ClearName, 
                                                              (DateTime)cb_timeFrom.SelectedItem, 
                                                              (DateTime)cb_timeTo.SelectedItem);
                DateTime t1 = (DateTime)table.Rows[0].ItemArray[0];
                foreach (DataRow t in table.Rows)
                {
                    DateTime t2 = (DateTime)t.ItemArray[0];
                    chart_LineDiagram.Series["Температура шпинделя, C"].Points.AddXY(
                        (t2 - t1).TotalMinutes, (float)t.ItemArray[1]);
                    chart_LineDiagram.Series["Скорость шпинделя, об/мин"].Points.AddXY(
                        (t2 - t1).TotalMinutes, (float)t.ItemArray[2]);
                }
            }
        }

        private void btn_loadColumDiagram_Click(object sender, EventArgs e)
        {
            chart_ColumDiagram.Series.Clear();
            int s_counter = 0;
            bool normTempLegendVisible = true;
            bool critTempLegendVisible = true;
            bool notworkTempLegendVisible = true;
            if (cb_dayTime.SelectedItem == null || cb_timeFrom.SelectedItem == null || cb_timeTo.SelectedItem == null)
            {
                var table = DB.GetMachineLoadHistory(((ItemExtractor)cmbbx_nameOfMachine.SelectedItem).ClearName);
                for (int i = 0; i < table.Rows.Count - 1; i++)
                {
                    DateTime current = (DateTime)table.Rows[i][0];
                    DateTime next = (DateTime)table.Rows[i + 1][0];
                    var duration = (next - current).TotalMinutes;
                    float temp = (float)(table.Rows[i][1]);
                    string seriesName = "temp" + s_counter.ToString();
                    chart_ColumDiagram.Series.Add(seriesName);
                    chart_ColumDiagram.Series[seriesName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedBar;

                    if (temp < 20)
                    {
                        chart_ColumDiagram.Series[seriesName].Color = Color.Blue;
                        chart_ColumDiagram.Series[seriesName].LegendText = "Нерабочая температура";
                        chart_ColumDiagram.Series[seriesName].IsVisibleInLegend = notworkTempLegendVisible;
                        chart_ColumDiagram.Series[seriesName].SetCustomProperty("PixelPointWidth", "150");
                        notworkTempLegendVisible = false;
                    }
                    else if (temp <= 60)
                    {
                        chart_ColumDiagram.Series[seriesName].Color = Color.Green;
                        chart_ColumDiagram.Series[seriesName].LegendText = "Нормальная температура";
                        chart_ColumDiagram.Series[seriesName].IsVisibleInLegend = normTempLegendVisible;
                        chart_ColumDiagram.Series[seriesName].SetCustomProperty("PixelPointWidth", "150");
                        normTempLegendVisible = false;
                    }
                    else
                    {
                        chart_ColumDiagram.Series[seriesName].Color = Color.Red;
                        chart_ColumDiagram.Series[seriesName].LegendText = "Критическая температура";
                        chart_ColumDiagram.Series[seriesName].IsVisibleInLegend = critTempLegendVisible;
                        chart_ColumDiagram.Series[seriesName].SetCustomProperty("PixelPointWidth", "150");
                        critTempLegendVisible = false;
                    }
                    chart_ColumDiagram.Series[seriesName].Points.AddY(duration);
                    s_counter++;
                }
                chart_ColumDiagram.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart_ColumDiagram.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                chart_ColumDiagram.ChartAreas[0].AxisY.LabelStyle.Interval = 3;
            }
            else
            {
                var table = DB.GetMachineLoadHistoryByPeriod(((ItemExtractor)cmbbx_nameOfMachine.SelectedItem).ClearName,
                                                              (DateTime)cb_timeFrom.SelectedItem,
                                                              (DateTime)cb_timeTo.SelectedItem);
                for (int i = 0; i < table.Rows.Count - 1; i++)
                {
                    DateTime current = (DateTime)table.Rows[i][0];
                    DateTime next = (DateTime)table.Rows[i + 1][0];
                    var duration = (next - current).TotalMinutes;
                    float temp = (float)(table.Rows[i][1]);
                    string seriesName = "temp" + s_counter.ToString();
                    chart_ColumDiagram.Series.Add(seriesName);
                    chart_ColumDiagram.Series[seriesName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedBar;

                    if (temp < 20)
                    {
                        chart_ColumDiagram.Series[seriesName].Color = Color.Blue;
                        chart_ColumDiagram.Series[seriesName].LegendText = "Нерабочая температура";
                        chart_ColumDiagram.Series[seriesName].IsVisibleInLegend = notworkTempLegendVisible;
                        chart_ColumDiagram.Series[seriesName].SetCustomProperty("PixelPointWidth", "150");
                        notworkTempLegendVisible = false;
                    }
                    else if (temp <= 60)
                    {
                        chart_ColumDiagram.Series[seriesName].Color = Color.Green;
                        chart_ColumDiagram.Series[seriesName].LegendText = "Нормальная температура";
                        chart_ColumDiagram.Series[seriesName].IsVisibleInLegend = normTempLegendVisible;
                        chart_ColumDiagram.Series[seriesName].SetCustomProperty("PixelPointWidth", "150");
                        normTempLegendVisible = false;
                    }
                    else
                    {
                        chart_ColumDiagram.Series[seriesName].Color = Color.Red;
                        chart_ColumDiagram.Series[seriesName].LegendText = "Критическая температура";
                        chart_ColumDiagram.Series[seriesName].IsVisibleInLegend = critTempLegendVisible;
                        chart_ColumDiagram.Series[seriesName].SetCustomProperty("PixelPointWidth", "150");
                        critTempLegendVisible = false;
                    }
                    chart_ColumDiagram.Series[seriesName].Points.AddY(duration);
                    s_counter++;
                }
                chart_ColumDiagram.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart_ColumDiagram.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                chart_ColumDiagram.ChartAreas[0].AxisY.LabelStyle.Interval = 3;
            }
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

            cb_dayTime.Items.Clear();
            cb_timeFrom.Items.Clear();
            cb_timeTo.Items.Clear();
            cb_dayTime.Text = string.Empty;
            cb_timeFrom.Text = string.Empty;
            cb_timeTo.Text = string.Empty;
            var table = DB.GetMachineLoadHistory(((ItemExtractor)cmbbx_nameOfMachine.SelectedItem).ClearName);
            foreach (DataRow t in table.Rows)
            {
                cb_dayTime.Items.Add(t.ItemArray[0]);
                cb_timeFrom.Items.Add(t.ItemArray[0]);
                cb_timeTo.Items.Add(t.ItemArray[0]);
            }
        }
        public void FillHeader()
        {
            lbl_Header.Text = string.Join(", ", headerText);
        }
    }
}


