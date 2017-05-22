using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Windows;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace my_player
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        bool isfullscreen;

        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.ViewManagement.ApplicationView view = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            bool isInFullScreenMode = view.IsFullScreenMode;
            if(isInFullScreenMode)
            {
                view.ExitFullScreenMode();
                Bar.Visibility = Visibility.Visible;
                isfullscreen = false;
            }
            else
            {
                view.TryEnterFullScreenMode();
                Bar.Visibility = Visibility.Collapsed;
                isfullscreen = true;
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            my_media.Stop();
            PlayAndPause.Icon = new SymbolIcon(Symbol.Play);
        }

        private void PlayAndPause_Click(object sender, RoutedEventArgs e)
        {
            var state = my_media.CurrentState.ToString();
            if(state == "Playing")
            {
                my_media.Pause();
                PlayAndPause.Icon = new SymbolIcon(Symbol.Play);
                return;
            }
            else if(state == "Paused" || state == "Stopped")
            {
                my_media.Play();
                PlayAndPause.Icon = new SymbolIcon(Symbol.Pause);
                return;
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            my_media.Volume = VolumeSlider.Value;
        }

        private void my_media_MediaOpened(object sender, RoutedEventArgs e)
        {
            Progress.Maximum = my_media.NaturalDuration.TimeSpan.TotalSeconds;
            VolumeSlider.Value = my_media.Volume;
            PlayAndPause.Icon = new SymbolIcon(Symbol.Play);
        }

        private void my_media_MediaEnded(object sender, RoutedEventArgs e)
        {
            PlayAndPause.Icon = new SymbolIcon(Symbol.Play);
            Progress.Value = 0;
        }

        private async void LocalFile_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".wma");
            picker.FileTypeFilter.Add(".m4a");
            picker.FileTypeFilter.Add(".m4b");
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".avi");
            picker.FileTypeFilter.Add(".wmv");
            picker.FileTypeFilter.Add(".asf");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    my_media.SetSource(fileStream, file.ContentType);
                }
            }
        }

        private void Progress_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Bar.Visibility = Visibility.Visible;
        }

        private void my_media_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (isfullscreen)
            {
                Bar.Visibility = Visibility.Collapsed;
            }
            else
            {
                
            }
            
        }
    }
    public class ProgressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((TimeSpan)value).TotalSeconds;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return TimeSpan.FromSeconds((double)value);
        }
    }
}