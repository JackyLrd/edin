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
            // 加入两个用来测试的item
            var img = new BitmapImage(new Uri("ms-appx:///Assets/abc.jpg"));
            this.allItems.Add(new Models.TodoItem("1", "1", DateTime.Now, img));
            this.allItems.Add(new Models.TodoItem("2", "2", DateTime.Now, img));
            this.allItems.Add(new Models.TodoItem("3", "3", DateTime.Now, img));
            this.allItems.Add(new Models.TodoItem("4", "4", DateTime.Now, img));
            this.allItems.Add(new Models.TodoItem("5", "5", DateTime.Now, img));
        }

        public void AddTodoItem(string title, string description, DateTime date, BitmapImage img)
        {
            this.allItems.Add(new Models.TodoItem(title, description, date, img));
        }

        public void RemoveTodoItem(string id)
        {
            // DIY
            this.allItems.Remove(selectedItem);
            // set selectedItem to null after remove
            this.selectedItem = null;
        }

        public void UpdateTodoItem(string id, string title, string description, DateTime date, BitmapImage img)
        {
            // DIY
            this.selectedItem.title = title;
            this.selectedItem.description = description;
            this.selectedItem.date = date;
            this.selectedItem.img = img;
            // set selectedItem to null after update
            this.selectedItem = null;
        }
    }
}