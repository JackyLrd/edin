﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todos.ViewModels
{
    class TodoItemViewModel
    {
        private ObservableCollection<Models.TodoItem> allItems = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> AllItems { get { return this.allItems; } }

        private Models.TodoItem selectedItem = default(Models.TodoItem);
        public Models.TodoItem SelectedItem { get { return selectedItem; } set { this.selectedItem = value; }  }

        public TodoItemViewModel()
        {
            // 加入两个用来测试的item
            this.allItems.Add(new Models.TodoItem("123", "123"));
            this.allItems.Add(new Models.TodoItem("456", "456"));
        }

        public void AddTodoItem(string title, string description)
        {
            this.allItems.Add(new Models.TodoItem(title, description));
        }

        public void RemoveTodoItem(string id)
        {
            // DIY

            // set selectedItem to null after remove
            this.selectedItem = null;
        }

        public void UpdateTodoItem(string id, string title, string description)
        {
            // DIY

            // set selectedItem to null after update
            this.selectedItem = null;
        }

    }
}
