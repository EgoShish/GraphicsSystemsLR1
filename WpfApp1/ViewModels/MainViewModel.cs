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

        public RelayCommand LoadMessagesCommand { get; }
        public RelayCommand LoadLineChartCommand { get; }
        public RelayCommand LoadColumnChartCommand { get; }

        public MainViewModel()
        {
            _db = new Database();
            LoadMessagesCommand = new RelayCommand(() => { });
            LoadLineChartCommand = new RelayCommand(() => { });
            LoadColumnChartCommand = new RelayCommand(() => { });
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
            if (SelectedType == null || SelectedModel == null)
            {
                MessageBox.Show("Тип или модель станка не задана");
                return;
            }
            HeaderText = $"{SelectedType.FullName}: {SelectedModel.FullName}";
        }
    }
}
