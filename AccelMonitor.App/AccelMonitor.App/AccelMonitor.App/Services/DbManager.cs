using AccelMonitor.App.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AccelMonitor.App.Services
{
    public class DbManager
    {
        readonly SQLiteAsyncConnection database;

        public DbManager(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<TodoItem>().Wait();
            database.CreateTableAsync<DataTable>().Wait();

        }

        public Task<List<TodoItem>> GetItemsAsync()
        {
            return database.Table<TodoItem>().ToListAsync();
        }

        public Task<List<DataTable>> GetTimeVsDataAsync()
        {
            return database.Table<DataTable>().OrderBy(c => c.dateTime).ToListAsync();
        }

        public Task<List<DataTable>> GetTimeVsDataByTimeAsync(DateTime startDT, DateTime endDT)
        {
            return database.Table<DataTable>().Where(c => c.dateTime >= startDT && c.dateTime <= endDT)
                .OrderBy(c => c.dateTime)
                .ToListAsync();
        }
        public Task<List<DataTable>> GetTopTimeVsDataAsync(int top)
        {
            return database.Table<DataTable>().OrderBy(c => c.dateTime).Take(top).ToListAsync();
        }

        public async void DeleteTimeVsData()
        {
            await database.ExecuteAsync("DELETE FROM DataTable");

        }

        public Task<List<TodoItem>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public Task<TodoItem> GetItemAsync(int id)
        {
            return database.Table<TodoItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<DataTable> GetTimeVsDataAsync(int id)
        {
            return database.Table<DataTable>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(TodoItem item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> SaveTimeVsDataItemAsync(DataTable item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(TodoItem item)
        {
            return database.DeleteAsync(item);
        }

        public Task<int> DeleteTimeVsDataItemAsync(DataTable item)
        {
            return database.DeleteAsync(item);
        }
    }
}
