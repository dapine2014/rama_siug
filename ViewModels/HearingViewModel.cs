using Microsoft.Maui.Controls;
using SIUGJ.Helpers;
using SIUGJ.Models;
using SIUGJ.Views;
using SIUGJ.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SIUGJ.ViewModels
{
    public class HearingViewModel : BasePageViewModel
    {
        private readonly List<Hearing> _allHearings = new();

        public IDataStore<Hearing> HearingService => ServiceHelper.GetRequiredService<IDataStore<Hearing>>();

        public ObservableCollection<Hearing> DayHearings { get; } = new();

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private DateTime _selectedDate = DateTime.Today;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (SetProperty(ref _selectedDate, value))
                {
                    UpdateDayHearings();
                    OnPropertyChanged(nameof(SelectedDateText));
                }
            }
        }

        public string SelectedDateText => SelectedDate.ToString("dd MMMM yyyy, dddd", Culture);

        public CultureInfo Culture { get; }

        public bool ForceRequest { get; set; }

        public ICommand EventSelectedCommand { get; }
        public ICommand SelectPreviousDayCommand { get; }
        public ICommand SelectNextDayCommand { get; }
        public ICommand SelectTodayCommand { get; }
        public ICommand RefreshCommand { get; }

        public bool HasHearings => DayHearings.Count > 0;

        public HearingViewModel()
        {
            Culture = CultureInfo.CreateSpecificCulture("es");
            EventSelectedCommand = new Command<Hearing>(async item => await ExecuteEventSelectedCommand(item));
            SelectPreviousDayCommand = new Command(() => SelectedDate = SelectedDate.AddDays(-1));
            SelectNextDayCommand = new Command(() => SelectedDate = SelectedDate.AddDays(1));
            SelectTodayCommand = new Command(() => SelectedDate = DateTime.Today);
            RefreshCommand = new Command(async () => await LoadHearingsAsync(true));

            _ = LoadHearingsAsync(false);
        }

        private async Task LoadHearingsAsync(bool forceRefresh)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            try
            {
                var items = await HearingService.GetItemsAsync(forceRefresh);
                _allHearings.Clear();
                _allHearings.AddRange(items.OrderBy(a => a.Starting));

                if (_allHearings.Count > 0 && SelectedDate == default)
                {
                    SelectedDate = DateTime.Today;
                }

                UpdateDayHearings();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void UpdateDayHearings()
        {
            DayHearings.Clear();
            foreach (var hearing in _allHearings.Where(a => a.Starting.Date == SelectedDate.Date))
            {
                DayHearings.Add(hearing);
            }

            OnPropertyChanged(nameof(HasHearings));
        }

        private async Task ExecuteEventSelectedCommand(Hearing item)
        {
            if (item == null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(HearingDetail)}?{nameof(Hearing.IdEvento)}={item.IdEvento}");
        }
    }
}
