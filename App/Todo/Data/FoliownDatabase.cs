using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace Todo.Data
{
	public class FoliownDatabase
	{
		readonly SQLiteAsyncConnection _database;

		public FoliownDatabase(string dbPath)
		{
			_database = new SQLiteAsyncConnection(dbPath);
			_database.CreateTableAsync<TodoItem>().Wait();
		    _database.CreateTableAsync<VideoClip>().Wait();
		}

		public Task<List<TodoItem>> GetItemsAsync()
		{
			return _database.Table<TodoItem>().ToListAsync();
		}

	    public Task<List<VideoClip>> GetClipsAsync()
	    {
	        return _database.Table<VideoClip>().ToListAsync();
	    }

		public Task<List<TodoItem>> GetItemsNotDoneAsync()
		{
			return _database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
		}

		public Task<TodoItem> GetItemAsync(int id)
		{
			return _database.Table<TodoItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
		}

		public Task<int> SaveItemAsync(TodoItem item)
		{
			if (item.ID != 0)
			{
				return _database.UpdateAsync(item);
			}
			else {
				return _database.InsertAsync(item);
			}
		}

	    public Task<int> SaveClip(VideoClip clip)
	    {
	        return _database.InsertAsync(clip);
	    }

		public Task<int> DeleteItemAsync(TodoItem item)
		{
			return _database.DeleteAsync(item);
		}
	}
}

