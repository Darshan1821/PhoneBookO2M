using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PhoneBook.Droid
{
	[Activity (Label = "Contacts", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
        Button add, showall, showemp;
        EditText name,num1,num2;
        ContactRepository repository;
        ListView contactListNames;
        List<string> contactList;
        EmployeeRepository emprepo;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

            name = FindViewById<EditText>(Resource.Id.empName);
            num1 = FindViewById<EditText>(Resource.Id.contactNum1);
            num2 = FindViewById<EditText>(Resource.Id.contactNum2);

            add = FindViewById<Button>(Resource.Id.addContact);
            contactListNames = FindViewById<ListView>(Resource.Id.contactList);
            showemp = FindViewById<Button>(Resource.Id.showAllEmployees);

            repository = new ContactRepository();
            emprepo = new EmployeeRepository();

            add.Click += async (sender, e) =>
            {
                List<Contact> empContact = new List<Contact>();
                if (name.Text.Length != 0 && (num1.Text.Length != 0 || num2.Text.Length != 0))
                {
                    if (num1.Text.Length != 0) { 
                        var newContact = new Contact { contactName = num1.Text.ToString() };
                        empContact.Add(newContact);
                    }
                    if (num2.Text.Length != 0)
                    {
                        var newContact1 = new Contact { contactName = num2.Text.ToString() };
                        empContact.Add(newContact1);
                    }

                    var emp = new Employee { empName = name.Text.ToString() , contactList = empContact};
                    await emprepo.InsertWithChild(emp);

                    contactList = new List<string>();
                    MyAdapter adapter = new MyAdapter(this, contactList);
                    contactListNames.Adapter = adapter;

                    name.Text = "";
                    num1.Text = "";
                    num2.Text = "";

                    Toast.MakeText(this, "Employee Added Successfully!!", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "Enter Values Please!!", ToastLength.Short).Show();
                }
            };

            showemp.Click += Showemp_Click;
        }

        private async void Showemp_Click(object sender, EventArgs e)
        {
            var emps = await emprepo.GetWithChild();
            string contactNo = "";

            contactList = new List<string>();

            foreach (var c in emps)
            {
                contactList.Add("Name: " +c.empName);

                foreach(var m in c.contactList) { contactNo += m.contactName + ", ";  }
                contactList.Add("Contact No: "+contactNo.Substring(0,contactNo.Length-2));
                contactNo = "";
                contactList.Add("");
            }

            MyAdapter adapter = new MyAdapter(this, contactList);
            contactListNames.Adapter = adapter;
        }
    }
}


