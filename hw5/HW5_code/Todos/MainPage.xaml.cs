using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Windows.ApplicationModel.DataTransfer;
using Todos.Models;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Todos
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = new ViewModels.TodoItemViewModel();
            this.SizeChanged += (s, e) =>
            {
                /*if (e.NewSize.Width < 501)
                    Frame.Navigate(typeof(MainPage), ViewModel);*/
            };
        }

        private TodoItem shareitem;
        ViewModels.TodoItemViewModel ViewModel { get; set; }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
            if (e.Parameter.GetType() == typeof(ViewModels.TodoItemViewModel))
            {
                this.ViewModel = (ViewModels.TodoItemViewModel)(e.Parameter);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;
        }

        private void TodoItem_ItemClicked(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = (Models.TodoItem)(e.ClickedItem);
            if (this.ActualWidth > 800)
            {
                title.Text = ViewModel.SelectedItem.title;
                details.Text = ViewModel.SelectedItem.description;
                date.Date = ViewModel.SelectedItem.date.Date;
                img.Source = ViewModel.SelectedItem.img;
                if (ViewModel.SelectedItem != null)
                {
                    CreateButton.Content = "Update";
                    title.Text = ViewModel.SelectedItem.title;
                    details.Text = ViewModel.SelectedItem.description;
                    date.Date = ViewModel.SelectedItem.date;
                }
                else
                {
                    CreateButton.Content = "Create";
                }
            }
            else
            {
                Frame.Navigate(typeof(EditPage), ViewModel);
            }
        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EditPage), ViewModel);
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (title.Text == "")
            {
                var dialog = new MessageDialog("Title不能为空");
                dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                dialog.DefaultCommandIndex = 0;
                dialog.ShowAsync();
                return;
            }
            if (details.Text == "")
            {
                var dialog = new MessageDialog("Details不能为空");
                dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                dialog.DefaultCommandIndex = 0;
                dialog.ShowAsync();
                return;
            }
            if (date.Date.AddDays(1) >= DateTimeOffset.Now)
            {

            }
            else
            {
                var dialog = new MessageDialog("Date Due不能小于当前日期");
                dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                dialog.DefaultCommandIndex = 0;
                dialog.ShowAsync();
                return;
            }
            BitmapImage image = img.Source as BitmapImage;
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.UpdateTodoItem("", title.Text, details.Text, date.Date.Date, image);
            }
            else
            {
                ViewModel.AddTodoItem(title.Text, details.Text, date.Date.Date, image);
            }
            update();
            title.Text = "";
            details.Text = "";
            date.Date = DateTimeOffset.Now;
            CreateButton.Content = "Create";
            Frame.Navigate(typeof(MainPage), ViewModel);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            title.Text = "";
            details.Text = "";
            date.Date = DateTimeOffset.Now;
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.SelectedItem = null;
                Frame.Navigate(typeof(MainPage), ViewModel);
            }
            else
            {

            }
        }

        private async void SelectPictureButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".jpeg");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    // Set the image source to the selected bitmap 
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.DecodePixelWidth = 600; //match the target Image.Width, not shown
                    await bitmapImage.SetSourceAsync(fileStream);
                    img.Source = bitmapImage;
                }
            }
        }

        private void update()
        {
            //五分钟之后清除更新
            //tileNotification.ExpirationTime = DateTimeOffset.UtcNow.AddMinutes(5);

            //立即发送更新
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();
            var items = ViewModel.AllItems.Take(5);
            int i = 1;
            foreach (var n in items)
            {
                XmlDocument tileXml = new XmlDocument();
                tileXml.LoadXml(File.ReadAllText("tile.xml"));

                //磁贴文本，如果指定了显示应用名称，文本将会显示在应用名称上方
                XmlNodeList tileTextAttributes = tileXml.GetElementsByTagName("text");
                tileTextAttributes[0].InnerText = n.title;
                tileTextAttributes[1].InnerText = n.description;
                tileTextAttributes[2].InnerText = n.title;
                tileTextAttributes[3].InnerText = n.description;
                tileTextAttributes[4].InnerText = n.title;
                tileTextAttributes[5].InnerText = n.description;
                tileTextAttributes[6].InnerText = n.title;
                tileTextAttributes[7].InnerText = n.description;
                //磁贴图片
                XmlNodeList tileImageAttributes = tileXml.GetElementsByTagName("image");
                ((XmlElement)tileImageAttributes[0]).SetAttribute("src", "ms-appx:///Assets/abc.jpg");
                ((XmlElement)tileImageAttributes[1]).SetAttribute("src", "ms-appx:///Assets/abc.jpg");
                ((XmlElement)tileImageAttributes[2]).SetAttribute("src", "ms-appx:///Assets/abc.jpg");
                ((XmlElement)tileImageAttributes[3]).SetAttribute("src", "ms-appx:///Assets/abc.jpg");
                TileNotification tileNotification = new TileNotification(tileXml);
                tileNotification.Tag = i.ToString();
                updater.Update(tileNotification);
                i++;
            }
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            shareitem = (TodoItem)((MenuFlyoutItem)sender).DataContext;
            DataTransferManager.ShowShareUI();
        }

        async void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var dp = args.Request.Data;
            var deferral = args.Request.GetDeferral();
            var photoFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/abc.jpg"));
            dp.Properties.Title = shareitem.title;
            dp.Properties.Description = shareitem.description;
            dp.SetText("完成任务" + shareitem.title);
            dp.SetStorageItems(new List<StorageFile> { photoFile });
            //dp.SetWebLink(new Uri("http://seattletimes.com/ABPub/2006/01/10/2002732410.jpg"));
            deferral.Complete();
        }
    }
    public class Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
    public class Converter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool? ischecked = value as bool?;
            if (ischecked == null || ischecked == false)
            {
                return 0;
            }
            else
            {
                return 2;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class Converter3 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var source = value as ImageSource;
            return source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var source = value as BitmapImage;
            return source;
        }
    }
}
