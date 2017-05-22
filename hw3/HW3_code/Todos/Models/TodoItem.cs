using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Todos.Models
{
    class TodoItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string id;
        private string _title;
        public string title
        {
            get { return _title; }
            set
            {
                _title = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("title"));
                }
            }
        }
        private string _description;
        public string description
        {
            get { return _description; }
            set
            {
                _description = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("description"));
                }
            }
        }
        private bool _completed;
        public bool completed
        {
            get { return _completed; }
            set
            {
                _completed = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("completed"));
                }
            }
        }
        private DateTime _date;
        public DateTime date
        {
            get { return _date; }
            set
            {
                _date = value;
                if (date != value)
                {          
                    PropertyChanged(this, new PropertyChangedEventArgs("date"));
                }
            }
        }

        public TodoItem(string title, string description, DateTime date)
        {
            this.id = Guid.NewGuid().ToString();
            this._title = title;
            this._description = description;
            this._completed = false;
            this._date = date.Date.Date;
        }
    }
}