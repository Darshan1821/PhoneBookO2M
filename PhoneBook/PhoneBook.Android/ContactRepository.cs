using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite.Net;
using System.Threading.Tasks;
using SQLite.Net.Platform.XamarinAndroid;
using SQLite.Net.Async;

namespace PhoneBook.Droid
{
    class ContactRepository
    {
        private readonly SQLiteAsyncConnection sqlConnection;

        public ContactRepository()
        {
            string path = FileAccessHelper.GetLocalFilePath("people.db3");
            sqlConnection = new SQLite.Net.Async.SQLiteAsyncConnection(new Func<SQLiteConnectionWithLock>(() => new SQLiteConnectionWithLock(new SQLitePlatformAndroid(), new SQLiteConnectionString(path, storeDateTimeAsTicks: false))));
            sqlConnection.CreateTableAsync<Contact>();
        }

        public async Task<List<Contact>> GetContacts()
        {
            return await sqlConnection.Table<Contact>().ToListAsync(); 
        }
        
        public async Task<Contact> GetContact(int id)
        {
            return await sqlConnection.Table<Contact>().Where(tdi => tdi.id == id).FirstOrDefaultAsync();
        }

        public async Task DeleteContact(Contact contact)
        {
            await sqlConnection.DeleteAsync(contact);
        }

        public async Task AddContact(Contact contact)
        {
            await sqlConnection.InsertAsync(contact);
        }
    }
}