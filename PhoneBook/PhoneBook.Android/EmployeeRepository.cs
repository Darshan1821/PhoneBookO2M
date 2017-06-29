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
using SQLiteNetExtensions;
using System.Threading.Tasks;
using SQLite.Net.Platform.XamarinAndroid;
using SQLiteNetExtensionsAsync.Extensions;
using SQLite.Net;

namespace PhoneBook.Droid
{
    class EmployeeRepository
    {
        private readonly SQLite.Net.Async.SQLiteAsyncConnection sqlConnection;

        public EmployeeRepository()
        {
            string path = FileAccessHelper.GetLocalFilePath("people.db3");
            sqlConnection = new SQLite.Net.Async.SQLiteAsyncConnection(new Func<SQLiteConnectionWithLock>(() => new SQLiteConnectionWithLock(new SQLitePlatformAndroid(),new SQLiteConnectionString(path, storeDateTimeAsTicks: false))));
            sqlConnection.CreateTableAsync<Employee>();
        }

        public async Task<List<Employee>> GetEmployees()
        {
            return await sqlConnection.Table<Employee>().ToListAsync();
        }

        public async Task<Employee> GetEmployee(int id)
        {
            return await sqlConnection.Table<Employee>().Where(tdi => tdi.empid == id).FirstOrDefaultAsync();
        }

        public async Task DeleteEmployee(Employee employee)
        {
            await sqlConnection.DeleteAsync(employee);
        }

        public async Task AddEmployee(Employee employee)
        {
            await sqlConnection.InsertAsync(employee);
        }

        public async Task UpdateEmployee(Employee employee)
        {
            await sqlConnection.UpdateWithChildrenAsync(employee);
        }
    }
}