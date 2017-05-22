using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
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
        private BitmapImage _img;
        public BitmapImage img
        {
            get { return _img; }
            set
            {
                _img = value;
                if (img != value)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("img"));
                }
            }
        }
        public TodoItem(string title, string description, DateTime date, BitmapImage img)
        {
            this.id = Guid.NewGuid().ToString();
            this._title = title;
            this._description = description;
            this._completed = false;
            this._date = date.Date.Date;
            this._img = img;
        }
    }
}