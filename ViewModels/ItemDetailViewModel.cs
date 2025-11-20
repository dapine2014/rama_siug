using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using SIUGJ.Helpers;
using SIUGJ.Models;

namespace SIUGJ.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
       private string itemId = string.Empty;
        private string text = string.Empty;
        private string description = string.Empty;
        public string Id { get; private set; } = string.Empty;

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }        

        public async void LoadItemId(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                return;
            }

            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                if (item == null)
                {
                    return;
                }

                Id = item.Id;
                Text = item.Text;
                Description = item.Description;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
