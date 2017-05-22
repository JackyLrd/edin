using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using System.ComponentModel;
using SQLitePCL;

namespace Todos.ViewModels
{
    class TodoItemViewModel
    {
        private ObservableCollection<Models.TodoItem> allItems = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> AllItems { get { return this.allItems; } }

        private Models.TodoItem selectedItem = default(Models.TodoItem);

        public Models.TodoItem SelectedItem { get { return selectedItem; } set { this.selectedItem = value; } }

        public TodoItemViewModel()
        {
            var db = App.conn;
            using (var statement = db.Prepare("SELECT Id, Title, Description, Time, Imguri FROM TodoItem"))
            {
                StringBuilder result = new StringBuilder();
                SQLiteResult r = statement.Step();
                while (SQLiteResult.ROW == r)
                {
                    for (int num = 0; num < statement.DataCount; num += 5)
                    {
                        allItems.Add(new Models.TodoItem((string)statement[num], (string)statement[num + 1], (string)statement[num + 2], Convert.ToDateTime((string)statement[num + 3]), new Uri((string)statement[num + 4])));
                    }
                    r = statement.Step();
                }
                if (SQLiteResult.DONE == r)
                {
                    
                }    
            }
        }

        public void AddTodoItem(string title, string description, DateTime date, Uri img)
        {
            img = img == null ? new Uri("ms-appx:///Assets/abc.jpg") : img;
            var str = img.ToString();
            if(str.IndexOf("Assets") == -1)
            {
                img = new Uri("ms-appx:///Assets/abc.jpg");
            }
            else
            {
                img = new Uri("ms-appx:///" + str.Substring(str.IndexOf("Assets/")));
            } 
            var todoitem = new Models.TodoItem(title, description, date, img);
            insert(todoitem.id, title, description, date.ToString(), img.ToString());
            this.allItems.Add(todoitem);
        }

        public void RemoveTodoItem(string id)
        {
            // DIY
            this.allItems.Remove(selectedItem);
            delete();

            // set selectedItem to null after remove
            this.selectedItem = null;
        }

        public void UpdateTodoItem(string id, string title, string description, DateTime date, Uri img)
        {
            // DIY
            this.selectedItem.title = title;
            this.selectedItem.description = description;
            this.selectedItem.date = date;
            img = img == null ? selectedItem.ImageUri : img;
            var str = img.ToString();
            if (str.IndexOf("Assets") == -1)
            {
                img = new Uri("ms-appx:///Assets/abc.jpg");
            }
            else
            {
                img = new Uri("ms-appx:///" + str.Substring(str.IndexOf("Assets/")));
            }
            this.selectedItem.ImageUri = img;
            update();

            // set selectedItem to null after update
            this.selectedItem = null;
        }

        public void insert(string id, string title, string description, string date, string img)
        {
            // SqlConnection was opened in App.xaml.cs and exposed through property conn
            var db = App.conn;
            using (var statement = db.Prepare("INSERT INTO TodoItem (Id, Title, Description, Time, Imguri) VALUES (?, ?, ?, ?, ?);"))
            {
                statement.Bind(1, id);
                statement.Bind(2, title);
                statement.Bind(3, description);
                statement.Bind(4, date);
                statement.Bind(5, img);
                statement.Step();
            }
        }

        public void delete()
        {
            var db = App.conn;
            using (var statement = db.Prepare("DELETE FROM TodoItem WHERE Id = ?;"))
            {
                // NOTE when using anonymous parameters the first has an index of 1, not 0.
                statement.Bind(1, selectedItem.id);
                statement.Step();
            }
        }

        public void update()
        {
            var db = App.conn;
            using (var statement = db.Prepare("UPDATE TodoItem SET Title = ?, Description = ?, Time = ?, Imguri = ? WHERE Id = ?;"))
            {
                // NOTE when using anonymous parameters the first has an index of 1, not 0.
                statement.Bind(1, selectedItem.title);
                statement.Bind(2, selectedItem.description);
                statement.Bind(3, selectedItem.date.ToString());
                var img = selectedItem.ImageUri == null ? new Uri("ms-appx:///Assets/abc.jpg") : selectedItem.ImageUri;
                statement.Bind(4, img.ToString());
                statement.Bind(5, selectedItem.id);
                statement.Step();
            }
        }
    }
}