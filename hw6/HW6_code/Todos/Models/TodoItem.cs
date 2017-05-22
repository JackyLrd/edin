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
        private string _id;
        public string id
        {
            get { return _id; }
            set
            {
                _id = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("id"));
                }
            }
        }
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

        private Uri _ImageUri;
        public Uri ImageUri
        {
            get { return _ImageUri; }
            set
            {
                if (!object.Equals(_ImageUri, value))
                {
                    SetImage(value);
                    _ImageUri = value;
                }          
            }
        }

        private BitmapImage _ImageSource;
        public BitmapImage ImageSource
        {
            get { return _ImageSource; }
            set
            {
                _ImageSource = value;
            }
        }

        private async void SetImage(Uri targetImageUri)
        {
            if (targetImageUri == null)
            {
                ImageSource = null;
            }
            else
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(targetImageUri);
                var fileStream = await file.OpenAsync(FileAccessMode.Read);
                var img = new BitmapImage();
                img.SetSource(fileStream);
                ImageSource = img;
            }
        }
        public TodoItem(string title, string description, DateTime date, Uri imgsource)
        {
            this._id = Guid.NewGuid().ToString();
            this._title = title;
            this._description = description;
            this._completed = false;
            this._date = date.Date.Date;
            this._ImageUri = imgsource;
        }
        public TodoItem(string id, string title, string description, DateTime date, Uri imgsource)
        {
            this._id = id;
            this._title = title;
            this._description = description;
            this._completed = false;
            this._date = date.Date.Date;
            this._ImageUri = imgsource;
        }
    }
}