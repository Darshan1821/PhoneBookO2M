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
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace PhoneBook.Droid
{
    [Table("Employee")]
    class Employee
    {
        [PrimaryKey, AutoIncrement]
        public int empid { get; set; }

        [MaxLength(250)]
        public string empName { get; set;}

        [OneToMany("empid")]
        public List<Contact> contactList { get; set; }
    }
}