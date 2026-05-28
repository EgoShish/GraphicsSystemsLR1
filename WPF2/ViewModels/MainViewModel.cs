using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using WPF2.Commands;
using WPF2.Models;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Drawing;
using SkiaSharp;
using LiveChartsCore;
using LiveChartsCore.Painting;

namespace WPF2.ViewModels
{
    public class MainViewModel : Notifier
    {
        private readonly Database _db;

        public ObservableCollection<ItemExtractor> MachineTypes { get; } = new();
        public ObservableCollection<ItemExtractor> MachineModels { get; } = new();


        private List<TimePoint> _allTimePoints = new List<TimePoint>();
        public ObservableCollection<TimePoint> UniqueDates { get; } = new();    
        public ObservableCollection<TimePoint> AvailableTimeFrom { get; } = new(); 
        public ObservableCollection<TimePoint> AvailableTimeTo { get; } = new();  

        public ObservableCollection<LineChartPoint> LineChartData { get; } = new(); // точки линейного графика прогрева шпинделя

        public ObservableCollection<TempLineStepPoint> ColdTemperatureStep { get; } = new(); // точки для ступеней графика температур
        public ObservableCollection<TempLineStepPoint> NormalTemperatureStep { get; } = new();   
        public ObservableCollection<TempLineStepPoint> CriticalTemperatureStep { get; } = new();

        public ISeries[] LineChartSeries { get; private set; } = Array.Empty<ISeries>();
        public Axis[] LineXAxis { get; private set; } = Array.Empty<Axis>();
        public Axis[] LineYAxis { get; private set; } = Array.Empty<Axis>();

        public ISeries[] ColumnChartSeries { get; private set; } = Array.Empty<ISeries>();
        public Axis[] ColumnXAxis { get; private set; } = Array.Empty<Axis>();
        public Axis[] ColumnYAxis { get; private set; } = Array.Empty<Axis>();


        // Переменные с событиями
        private string _headerText = "Выберите станок";
        public string HeaderText
        {
            get => _headerText;
            set { _headerText = value; OnPropertyChanged(); }
        }
        private ItemExtractor _selectedType;
        public ItemExtractor SelectedType
        {
            get => _selectedType;
            set
            {
                if (_selectedType != value)
                {
                    _selectedType = value;
                    OnPropertyChanged();
                    LoadModelsByType(); // Как только выбрали тип -> грузим модели
                }
            }
        }
        private ItemExtractor _selectedModel;
        public ItemExtractor SelectedModel
        {
            get => _selectedModel;
            set
            {
                if (_selectedModel != value)
                {
                    _selectedModel = value;
                    OnPropertyChanged();
                    OnModelSelected(); // Как только выбрали модель -> обновляем заголовок
                }
            }
        }
        private TimePoint _selectedDate;
        public TimePoint SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged();
                    UpdateTimeFromCollection(); // При смене даты фильтруем время
                }
            }
        }
        private TimePoint _selectedTimeFrom;
        public TimePoint SelectedTimeFrom
        {
            get => _selectedTimeFrom;
            set
            {
                _selectedTimeFrom = value;
                OnPropertyChanged();
                UpdateTimeToCollection();
            }
        }

        private TimePoint _selectedTimeTo;
        public TimePoint SelectedTimeTo
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
            LoadMessagesCommand = new RelayCommand(() => { });
            LoadLineChartCommand = new RelayCommand((LoadLineChart));
            LoadColumnChartCommand = new RelayCommand((LoadColumnChart));
            LoadMachineTypes();
        }

        private void LoadMachineTypes()
        {
            var table = _db.GetTypesList();
            foreach (DataRow row in table.Rows)
            {
                MachineTypes.Add(new ItemExtractor
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
                MachineModels.Add(new ItemExtractor
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
                    _allTimePoints.Add(new TimePoint { Value = dt });
            }
            UniqueDates.Clear();
            var uniqueDays = _allTimePoints
                .Select(p => p.Value.Date)
                .Distinct()
                .OrderBy(d => d)
                .Select(d => new TimePoint { Value = d });

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

            LineChartData.Clear();

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

                LineChartData.Add(new LineChartPoint
                {
                    Minutes = (currentTime - startTime).TotalMinutes,
                    Temperature = temp,
                    Speed = speed,
                    Timestamp = currentTime
                });
            }
            BuildLineChartSeries();
        }
        private void BuildLineChartSeries()
        {
            var tempSeries = new LineSeries<LineChartPoint>
            {
                Values = LineChartData,
                Mapping = (point, index) => new(point.Minutes, point.Temperature),
                Name = "Температура, °C",
                Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 2 },
                Fill = null,
                GeometryFill = new SolidColorPaint(SKColors.Red),
                GeometryStroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 2 },
                ScalesYAt = 0,
                IsHoverable = true
            };
            var speedSeries = new LineSeries<LineChartPoint>
            {
                Values = LineChartData,
                Mapping = (point, index) => new(point.Minutes, point.Speed ?? 0),
                Name = "Скорость, об/мин",
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 2 },
                Fill = null,
                GeometryFill = new SolidColorPaint(SKColors.Blue),
                GeometryStroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 2 },
                ScalesYAt = 1,
                IsHoverable = true
            };

            LineChartSeries = new ISeries[] { tempSeries, speedSeries };
            LineYAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Температура, °C",
                    Position = LiveChartsCore.Measure.AxisPosition.Start,
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray),
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray),
                    TextSize = 10,
                    MinLimit = 0,
                    NameTextSize = 15
                },
                new Axis
                {
                    Name = "Скорость, об/мин",
                    Position = LiveChartsCore.Measure.AxisPosition.End,
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray),
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray),
                    TextSize = 10,
                    MinLimit = 0,
                    NameTextSize = 15
                }
            };

            LineXAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Время, мин",
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray),
                    TextSize = 10,
                    MinLimit = 0,
                    NameTextSize = 15
                }
            };

            OnPropertyChanged(nameof(LineChartSeries));
            OnPropertyChanged(nameof(LineXAxis));
            OnPropertyChanged(nameof(LineYAxis));
        }
        private void LoadColumnChart()
        {
            if (SelectedModel == null || SelectedTimeFrom == null || SelectedTimeTo == null) return;

            ColdTemperatureStep.Clear();
            NormalTemperatureStep.Clear();
            CriticalTemperatureStep.Clear();

            var table = _db.GetMachineLoadHistoryByPeriod(SelectedModel.ClearName, SelectedTimeFrom.Value, SelectedTimeTo.Value);
            if (table.Rows.Count < 1) return;
            DateTime startTime = (DateTime)table.Rows[0][0];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                double currentMin = ((DateTime)table.Rows[i][0] - startTime).TotalMinutes;
                float temp = Convert.ToSingle(table.Rows[i][1]);

                double nextMin = (i < table.Rows.Count - 1)
                    ? ((DateTime)table.Rows[i + 1][0] - startTime).TotalMinutes
                    : currentMin + 1; 

                var startPt = new TempLineStepPoint { Minutes = currentMin, Temperature = temp };
                var endPt = new TempLineStepPoint { Minutes = nextMin, Temperature = temp };

                AddToRange(startPt, temp);
                AddToRange(endPt, temp);
            }
            BuildColumnChartSeries();
        }
        private void AddToRange(TempLineStepPoint point, float temp)
        {
            if (temp < 20) ColdTemperatureStep.Add(point);
            else if (temp <= 60) NormalTemperatureStep.Add(point);
            else CriticalTemperatureStep.Add(point);
        }
        private void BuildColumnChartSeries()
        {
            var coldSeries = new LineSeries<TempLineStepPoint>
            {
                Values = ColdTemperatureStep,
                Mapping = (p, _) => new(p.Minutes, p.Temperature),
                Name = "< 20°C",
                LineSmoothness = 0,
                GeometrySize = 0,
                Fill = new SolidColorPaint(SKColors.Blue),
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 1 }
            };

            var normalSeries = new LineSeries<TempLineStepPoint>
            {
                Values = NormalTemperatureStep,
                Mapping = (p, _) => new(p.Minutes, p.Temperature),
                Name = "20-60°C",
                LineSmoothness = 0,
                GeometrySize = 0,
                Fill = new SolidColorPaint(SKColors.Green),
                Stroke = new SolidColorPaint(SKColors.Green) { StrokeThickness = 1 }
            };

            var criticalSeries = new LineSeries<TempLineStepPoint>
            {
                Values = CriticalTemperatureStep,
                Mapping = (p, _) => new(p.Minutes, p.Temperature),
                Name = "> 60°C",
                LineSmoothness = 0,
                GeometrySize = 0,
                Fill = new SolidColorPaint(SKColors.Red),
                Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 1 }
            };

            ColumnChartSeries = new ISeries[] { coldSeries, normalSeries, criticalSeries };

            ColumnXAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Время, мин",
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray),
                    Labeler = value => TimeSpan.FromMinutes(value).ToString(@"hh\:mm"),
                    MinLimit = 0,
                    TextSize = 10,
                    NameTextSize = 15
                }
            };
            ColumnYAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Температура, °C",
                    Position = LiveChartsCore.Measure.AxisPosition.Start,
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray),
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray),
                    MinLimit = 0,
                    TextSize = 10,
                    NameTextSize = 15
                }
            };
            OnPropertyChanged(nameof(ColumnChartSeries));
            OnPropertyChanged(nameof(ColumnXAxis));
            OnPropertyChanged(nameof(ColumnYAxis));
        }
    }
}
