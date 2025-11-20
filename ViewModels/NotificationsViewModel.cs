using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;

using SIUGJ.Models;
using SIUGJ.Views;
using SIUGJ.Services;
using System.Linq;
using SIUGJ.Enums;
using SIUGJ.Helpers;

namespace SIUGJ.ViewModels
{
	public class NotificationsViewModel: BaseViewModel
    {

        public IDataStore<Notification> NotificationService => ServiceHelper.GetRequiredService<IDataStore<Notification>>();
        private Notification? _selectedItem;

        public ObservableCollection<Notification> ItemsView { get; }
        public Command LoadItemsCommand { get; }
        public Command<Notification> ItemTapped { get; }

        public bool ForceRequest { get; set; }

        public NotificationsViewModel()
        {
            Console.WriteLine("********** Invocando Constructor NotificationsViewModel " );
            Title = "Notificaciones";
            ItemsView = new ObservableCollection<Notification>();
            ForceRequest = false;
            LoadItemsCommand = new Command(async () => { await this.ExecuteLoadItemsCommand(); });
            ItemTapped = new Command<Notification>(OnItemSelected);
        }

        async Task ExecuteLoadItemsCommand()
        {
            Console.WriteLine("********** Invocando ExecuteLoadItemsCommand NotificationsViewModel ");
            IsBusy = true;

            try
            {
                ItemsView.Clear();
                var sectionController = Shell.Current?.CurrentItem?.CurrentItem as IShellSectionController;
                var currentPage = sectionController?.PresentedPage;
                var status = GetStatusFromTab(currentPage?.Parent?.ClassId);

                var items = await NotificationService.GetItemsAsync(ForceRequest);
                ForceRequest = true;
                foreach (var item in items.Where((item) => (string.IsNullOrEmpty(status) || item.taskstatus.Equals(status))).OrderByDescending(item => DateTime.Parse(item.fechaAsignacion)))
                {
                    ItemsView.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            Console.WriteLine("********** Invocando OnAppearing NotificationsViewModel ");
            IsBusy = true;
            SelectedItem = null;
            ForceRequest = false;
        }

        public Notification? SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(Notification? item)
        {
            Console.WriteLine("********** Invocando OnItemSelected NotificationsViewModel ");
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(NotificationDetailPage)}?{nameof(NotificationDetailViewModel.IdRegistro)}={item.idRegistro}");
        }

        private static string GetStatusFromTab(string? classId)
        {
            return classId switch
            {
                "0" => ((int)TaskStatusEnum.Active).ToString(),
                "1" => ((int)TaskStatusEnum.Attended).ToString(),
                "2" => string.Empty,
                _ => string.Empty
            };
        }
	}
}
