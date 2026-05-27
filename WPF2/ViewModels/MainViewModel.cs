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
    public class MainViewModel : NotifyModel
    {
        private readonly Database _db;

        public ObservableCollection<ItemExtractorModel> MachineTypes { get; } = new();
        public ObservableCollection<ItemExtractorModel> MachineModels { get; } = new();


        private List<TimePointModel> _allTimePoints = new List<TimePointModel>();
        public ObservableCollection<TimePointModel> UniqueDates { get; } = new();    
        public ObservableCollection<TimePointModel> AvailableTimeFrom { get; } = new(); 
        public ObservableCollection<TimePointModel> AvailableTimeTo { get; } = new();  

        public ObservableCollection<LineChartModel> LineChartData { get; } = new();
        public ObservableCollection<ColumnChartModel> ColumnChartData { get; } = new();
        public ISeries[] TemperatureSeries { get; private set; }
        public Axis[] XAxis { get; private set; }
        public Axis[] YAxis { get; private set; }
        public ISeries[] ColumnChartSeries { get; private set; }
        public Axis[] ColumnXAxis { get; private set; }
        public Axis[] ColumnYAxis { get; private set; }


        // Переменные с событиями
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
                    LoadModelsByType(); // Как только выбрали тип -> грузим модели
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
                    OnModelSelected(); // Как только выбрали модель -> обновляем заголовок
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
                    UpdateTimeFromCollection(); // При смене даты фильтруем время
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
                .Select(d => new TimePointModel { Value = d }); // Создаём новые объекты с временем 00:00

            foreach (var day in uniqueDays)
                UniqueDates.Add(day);
            if (UniqueDates.Any())
                SelectedDate = UniqueDates[0];
        }
        private void UpdateTimeFromCollection()
        {
            AvailableTimeFrom.Clear();
            AvailableTimeTo.Clear(); // Очищаем "ДО", пока нет выбора "ОТ"

            if (SelectedDate == null) return;

            var targetDay = SelectedDate.Value.Date;

            // Берём все метки выбранного дня, сортируем по времени
            var timesOfDay = _allTimePoints
                .Where(p => p.Value.Date == targetDay)
                .OrderBy(p => p.Value)
                .ToList();

            foreach (var t in timesOfDay)
                AvailableTimeFrom.Add(t);

            // Автовыбор первой доступной метки как "ОТ" (запустит следующий каскад)
            if (AvailableTimeFrom.Any())
                SelectedTimeFrom = AvailableTimeFrom[0];
        }
        private void UpdateTimeToCollection()
        {
            AvailableTimeTo.Clear();

            if (SelectedTimeFrom == null) return;

            // Фильтруем: показываем только метки >= выбранного "ОТ"
            var validToTimes = _allTimePoints
                .Where(p => p.Value >= SelectedTimeFrom.Value)
                .OrderBy(p => p.Value)
                .ToList();

            foreach (var t in validToTimes)
                AvailableTimeTo.Add(t);

            // Автовыбор последней доступной метки как "ДО" (чтобы охватить максимум)
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

                LineChartData.Add(new LineChartModel
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
                Values = LineChartData,
                Mapping = (point, index) => new(point.Minutes, point.Temperature),
                Name = "Температура, °C",
                Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 2 },
                Fill = null,
                GeometryFill = new SolidColorPaint(SKColors.Red),
                GeometryStroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 2 },
                ScalesYAt = 0,
                IsHoverable = true,
                DataPadding = new LvcPoint(5, 10)
            };
            var speedSeries = new LineSeries<LineChartModel>
            {
                Values = LineChartData,
                Mapping = (point, index) => new(point.Minutes, point.Speed ?? 0),
                Name = "Скорость, об/мин",
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 2 },
                Fill = null,
                GeometryFill = new SolidColorPaint(SKColors.Blue),
                GeometryStroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 2 },
                ScalesYAt = 1,
                IsHoverable = true,
                DataPadding = new LvcPoint(5, 10)
            };

            TemperatureSeries = new ISeries[] { tempSeries, speedSeries };
            YAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Температура, °C",
                    Position = LiveChartsCore.Measure.AxisPosition.Start,
                    LabelsPaint = new SolidColorPaint(SKColors.Red),
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray)
                },
                new Axis
                {
                    Name = "Скорость, об/мин",
                    Position = LiveChartsCore.Measure.AxisPosition.End, 
                    LabelsPaint = new SolidColorPaint(SKColors.Blue),
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray)
                }
            };

            XAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Время, мин",
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray)
                }
            };

            OnPropertyChanged(nameof(TemperatureSeries));
            OnPropertyChanged(nameof(XAxis));
            OnPropertyChanged(nameof(YAxis));
        }
        private void LoadColumnChart()
        {
            if (SelectedModel == null || SelectedTimeFrom == null || SelectedTimeTo == null) return;

            ColumnChartData.Clear();

            var table = _db.GetMachineLoadHistoryByPeriod(SelectedModel.ClearName, SelectedTimeFrom.Value, SelectedTimeTo.Value);
            if (table.Rows.Count < 2) return;
            int index = 0;
            DateTime prevTime = (DateTime)table.Rows[0][0];
            for (int i = 1; i < table.Rows.Count; i++)
            {
                DateTime currentTime = (DateTime)table.Rows[i][0];
                float temp = Convert.ToSingle(table.Rows[i][1]);

                double duration = (currentTime - prevTime).TotalMinutes;

                if (duration > 0)
                {
                    var point = ColumnChartModel.Create(index, prevTime, temp, duration);
                    ColumnChartData.Add(point);
                    index++;
                }
                prevTime = currentTime;
            }
            BuildColumnChartSeries();
        }
        private void BuildColumnChartSeries()
        {
            if (!ColumnChartData.Any()) return;

            var columnSeries = new ColumnSeries<ColumnChartModel>
            {
                Values = ColumnChartData,

                Mapping = (point, index) => new(point.Index, point.DurationMinutes),

                Name = "Время, мин",
                Fill = (point, series) =>
                {
                    if (point.Context.DataSource is ColumnChartModel data)
                        return new SolidColorPaint(data.BarColor);
                    return new SolidColorPaint(SKColors.Gray); // Заглушка
                },
                Stroke = new SolidColorPaint(SKColors.Black) { StrokeThickness = 0.5f },
                Padding = 0.1f, // Расстояние между столбцами (0 = плотно, 1 = широко)
                MaxBarWidth = 50
            };

            ColumnChartSeries = new ISeries[] { columnSeries };

            ColumnXAxis = new Axis[]
            {
                new Axis
                {

                    Name = "Интервал",
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray),
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray),
                }
            };
            ColumnYAxis = new Axis[]
            {
                new Axis
                {
                    Name = "Длительность, мин",
                    Position = LiveChartsCore.Measure.AxisPosition.Start,
                    LabelsPaint = new SolidColorPaint(SKColors.DarkGray),
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray),
                    MinLimit = 0
                }
            };
            OnPropertyChanged(nameof(ColumnChartSeries));
            OnPropertyChanged(nameof(ColumnXAxis));
            OnPropertyChanged(nameof(ColumnYAxis));
        }
    }
}
