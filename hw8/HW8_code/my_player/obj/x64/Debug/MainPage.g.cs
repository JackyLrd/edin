﻿#pragma checksum "D:\college\大二\大二下\现代操作系统应用开发\hw8\HW8_code\my_player\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "90F1AFC0E57D52E9000D9548435AF57D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace my_player
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.thePage = (global::Windows.UI.Xaml.Controls.Page)(target);
                }
                break;
            case 2:
                {
                    this.my_media = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                    #line 18 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.my_media).MediaOpened += this.my_media_MediaOpened;
                    #line 18 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.my_media).PointerMoved += this.my_media_PointerMoved;
                    #line default
                }
                break;
            case 3:
                {
                    this.Progress = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    #line 19 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Slider)this.Progress).PointerEntered += this.Progress_PointerEntered;
                    #line default
                }
                break;
            case 4:
                {
                    this.Bar = (global::Windows.UI.Xaml.Controls.CommandBar)(target);
                }
                break;
            case 5:
                {
                    this.PlayAndPause = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 23 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.PlayAndPause).Click += this.PlayAndPause_Click;
                    #line default
                }
                break;
            case 6:
                {
                    this.Stop = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 24 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.Stop).Click += this.Stop_Click;
                    #line default
                }
                break;
            case 7:
                {
                    this.VolumeControl = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                }
                break;
            case 8:
                {
                    this.FullScreen = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 32 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.FullScreen).Click += this.FullScreen_Click;
                    #line default
                }
                break;
            case 9:
                {
                    this.LocalFile = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 33 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.LocalFile).Click += this.LocalFile_Click;
                    #line default
                }
                break;
            case 10:
                {
                    this.VolumeSlider = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    #line 28 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Slider)this.VolumeSlider).ValueChanged += this.VolumeSlider_ValueChanged;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

