using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Commands;
using WpfApp1.Models;

namespace WpfApp1.ViewModels
{
    public class MainViewModel : NotifyModel
    {
        private readonly Database _db;
        public ObservableCollection<ItemExtractorModel> MachineTypes { get; } = new ObservableCollection<ItemExtractorModel>();
        public ObservableCollection<ItemExtractorModel> MachineModels { get; } = new ObservableCollection<ItemExtractorModel>();

        private List<TimePointModel> _allTimePoints = new List<TimePointModel>();
        public ObservableCollection<TimePointModel> UniqueDates { get; } = new ObservableCollection<TimePointModel>();//CB1: Даты
        public ObservableCollection<TimePointModel> AvailableTimeFrom { get; } = new ObservableCollection<TimePointModel>();//CB2: Время "ОТ"
        public ObservableCollection<TimePointModel> AvailableTimeTo { get; } = new ObservableCollection<TimePointModel>();//CB3: Время "ДО" 

        public ObservableCollection<LineChartModel> ChartData { get; } = new ObservableCollection<LineChartModel>();

        //хранение активных сообщений счпу
        public ObservableCollection<TableMessageItem> CncMessages { get; } = new ObservableCollection<TableMessageItem>();

        public ISeries[] TemperatureSeries { get; private set; }
        public Axis[] XAxis { get; private set; }
        public Axis[] YAxis { get; private set; }


        
        private bool _isHomeSelected;
        private bool _isMonitoringSelected = true;
        private bool _isAnalysisSelected;
        private bool _isReportSelected;

        public bool IsHomeSelected
        {
            get => _isHomeSelected;
            set { _isHomeSelected = value; OnPropertyChanged(); }
        }

        public bool IsMonitoringSelected
        {
            get => _isMonitoringSelected;
            set { _isMonitoringSelected = value; OnPropertyChanged(); }
        }

        public bool IsAnalysisSelected
        {
            get => _isAnalysisSelected;
            set { _isAnalysisSelected = value; OnPropertyChanged(); }
        }

        public bool IsReportSelected
        {
            get => _isReportSelected;
            set { _isReportSelected = value; OnPropertyChanged(); }
        }

        private string _headerText = "Выберите станок";
        public string HeaderText
        {
            get => _headerText;
            set { _headerText = value; OnPropertyChanged(); }
        }
        private ItemExtractorModel _selectedType;
        public ItemExtractorModel SelectedType
        {
            get => _selectedType;
            set
            {
                if (_selectedType != value)
                {
                    _selectedType = value;
                    OnPropertyChanged();
                    LoadModelsByType();
                }
            }
        }
        private ItemExtractorModel _selectedModel;
        public ItemExtractorModel SelectedModel
        {
            get => _selectedModel;
            set
            {
                if (_selectedModel != value)
                {
                    _selectedModel = value;
                    OnPropertyChanged();
                    OnModelSelected();
                }
            }
        }
        private TimePointModel _selectedDate;
        public TimePointModel SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged();
                    UpdateTimeFromCollection();
                }
            }
        }
        private TimePointModel _selectedTimeFrom;
        public TimePointModel SelectedTimeFrom
        {
            get => _selectedTimeFrom;
            set
            {
                _selectedTimeFrom = value;
                OnPropertyChanged();
                UpdateTimeToCollection();
            }
        }

        private TimePointModel _selectedTimeTo;
        public TimePointModel SelectedTimeTo
        {
            get => _selectedTimeTo;
            set { _selectedTimeTo = value; OnPropertyChanged(); }
        }


        public RelayCommand LoadMessagesCommand { get; }
        public RelayCommand LoadLineChartCommand { get; }
        public RelayCommand LoadColumnChartCommand { get; }

        public MainViewModel()
        {
            _db = new Database();
            LoadMessagesCommand = new RelayCommand(LoadCncMessages);
            LoadLineChartCommand = new RelayCommand((LoadLineChart));
            LoadColumnChartCommand = new RelayCommand((LoadColumnChart));
            LoadMachineTypes();
        }

        //загрузки активных сообщений из БД 
        private void LoadCncMessages()
        {
            CncMessages.Clear();
            if (SelectedModel == null) return;

            DataTable dt = _db.GetMachineStatusLogs(SelectedModel.ClearName);

            foreach (DataRow row in dt.Rows)
            {
                CncMessages.Add(new TableMessageItem
                {
                    View = row["name"]?.ToString(),
                    Time = DateTime.Now.ToString("HH:mm:ss"),
                    Channel = row["status"]?.ToString(),
                    Number = row["value"]?.ToString(),
                    Text = row["description"]?.ToString()
                });
            }
        }

        private void LoadMachineTypes()
        {
            var table = _db.GetTypesList();
            foreach (DataRow row in table.Rows)
            {
                MachineTypes.Add(new ItemExtractorModel
                {
                    ClearName = row[1]?.ToString(),
                    FullName = row[1]?.ToString()
                });
            }

            if (MachineTypes.Count == 0)
            {
                MessageBox.Show("Типов станков нет");
                return;
            }
            SelectedType = MachineTypes[0];
        }
        private void LoadModelsByType()
        {
            MachineModels.Clear();
            if (SelectedType == null) return;
            var table = _db.GetModelsByTypeName(SelectedType.ClearName);

            foreach (DataRow row in table.Rows)
            {
                MachineModels.Add(new ItemExtractorModel
                {
                    FullName = $"{row[0]} - {row[1]}",
                    ClearName = row[0]?.ToString(),
                    ImageName = row[2]?.ToString()
                });
            }
            if (MachineModels.Count == 0)
            {
                MessageBox.Show("Моделей станков нет");
                return;
            }
            SelectedModel = MachineModels[0];
        }
        private void OnModelSelected()
        {
            if (SelectedType == null || SelectedModel == null) return;
            HeaderText = $"{SelectedType.FullName}: {SelectedModel.FullName}";

            LoadTimePoints();
        }
        private void LoadTimePoints()
        {
            if (SelectedModel == null) return;
            var table = _db.GetMachineLoadHistory(SelectedModel.ClearName);
            _allTimePoints.Clear();
            foreach (DataRow row in table.Rows)
            {
                if (row[0] is DateTime dt)
                    _allTimePoints.Add(new TimePointModel { Value = dt });
            }
            UniqueDates.Clear();
            var uniqueDays = _allTimePoints
                .Select(p => p.Value.Date)
                .Distinct()
                .OrderBy(d => d)
                .Select(d => new TimePointModel { Value = d });

            foreach (var day in uniqueDays)
                UniqueDates.Add(day);
            if (UniqueDates.Any())
                SelectedDate = UniqueDates[0];
        }
        private void UpdateTimeFromCollection()
        {
            AvailableTimeFrom.Clear();
            AvailableTimeTo.Clear();

            if (SelectedDate == null) return;

            var targetDay = SelectedDate.Value.Date;

            var timesOfDay = _allTimePoints
                .Where(p => p.Value.Date == targetDay)
                .OrderBy(p => p.Value)
                .ToList();

            foreach (var t in timesOfDay)
                AvailableTimeFrom.Add(t);

            if (AvailableTimeFrom.Any())
                SelectedTimeFrom = AvailableTimeFrom[0];
        }
        private void UpdateTimeToCollection()
        {
            AvailableTimeTo.Clear();

            if (SelectedTimeFrom == null) return;

            var validToTimes = _allTimePoints
                .Where(p => p.Value >= SelectedTimeFrom.Value)
                .OrderBy(p => p.Value)
                .ToList();

            foreach (var t in validToTimes)
                AvailableTimeTo.Add(t);

            if (AvailableTimeTo.Any())
                SelectedTimeTo = AvailableTimeTo.Last();
        }
        private void LoadLineChart()
        {
            if (SelectedModel == null || SelectedTimeFrom == null || SelectedTimeTo == null) return;

            ChartData.Clear();

            var table = _db.GetMachineLoadHistoryByPeriod(
            SelectedModel.ClearName,
            SelectedTimeFrom.Value,
            SelectedTimeTo.Value);

            if (table.Rows.Count == 0) return;
            DateTime startTime = (DateTime)table.Rows[0][0];
            foreach (DataRow row in table.Rows)
            {
                DateTime currentTime = (DateTime)row[0];
                float temp = Convert.ToSingle(row[1]);
                float speed = Convert.ToSingle(row[2]);

                ChartData.Add(new LineChartModel
                {
                    Minutes = (currentTime - startTime).TotalMinutes,
                    Temperature = temp,
                    Speed = speed,
                    Timestamp = currentTime
                });
            }
            BuildChartSeries();
        }
        private void BuildChartSeries()
        {
            var tempSeries = new LineSeries<LineChartModel>
            {
                Values = ChartData,
                Mapping = (point, index) => new Coordinate(point.Minutes, point.Temperature),

                Name = "Температура, °C",
                Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 2 },
                Fill = null,
                GeometryFill = new SolidColorPaint(SKColors.Red),
                GeometryStroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 2 }
            };

            var speedSeries = new LineSeries<LineChartModel>
            {
                Values = ChartData,
                Mapping = (point, index) => new Coordinate(point.Minutes, point.Speed ?? 0),
                Name = "Скорость, об/мин",
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 2 },
                IsVisibleAtLegend = false
            };

            TemperatureSeries = new ISeries[] { tempSeries, speedSeries };

            XAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Время, мин",
                    LabelsRotation = 0,
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray) { StrokeThickness = 1 },
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray) {}
                }
            };

            YAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Температура, °C",
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray) { StrokeThickness = 1 },
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray) {},
                    MinLimit = 0,
                    MaxLimit = 100
                }
            };

            OnPropertyChanged(nameof(TemperatureSeries));
            OnPropertyChanged(nameof(XAxis));
            OnPropertyChanged(nameof(YAxis));
        }
        private void LoadColumnChart()
        {
            if (SelectedTimeFrom == null || SelectedTimeTo == null) return;

            System.Diagnostics.Debug.WriteLine($"[CHART] Столбчатый график: {SelectedTimeFrom.SqlParam} → {SelectedTimeTo.SqlParam}");
        }
    }
}