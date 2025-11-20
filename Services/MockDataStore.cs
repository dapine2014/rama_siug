using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SIUGJ.Models;

namespace SIUGJ.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>()
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            };
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            if (oldItem != null)
            {
                items.Remove(oldItem);
            }
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            if (oldItem != null)
            {
                items.Remove(oldItem);
            }

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            var item = items.FirstOrDefault(s => s.Id == id);
            if (item == null)
            {
                throw new InvalidOperationException($"No se encontró el elemento {id}.");
            }

            return await Task.FromResult(item);
        }

        public Item GetItem(string id)
        {
            var item = items.FirstOrDefault(s => s.Id == id);
            if (item == null)
            {
                throw new InvalidOperationException($"No se encontró el elemento {id}.");
            }

            return item;
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}
