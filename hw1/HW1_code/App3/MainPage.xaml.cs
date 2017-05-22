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

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace App3
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

        private delegate void AnimalSaying(object sender, myEvenArgs e);
        private event AnimalSaying OnSaying;
        public interface Animal
        {
           void saying(object sender, myEvenArgs e);
        }
        public class dog : Animal
        {
            TextBlock word;
            public dog(TextBlock w)
            {
                this.word = w;
            }
            public void saying(object sender, myEvenArgs e)
            {
                this.word.Text += "Dog:I am a dog\n";
            }
        }
        public class cat : Animal
        {
            TextBlock word;
            public cat(TextBlock w)
            {
                this.word = w;
            }
            public void saying(object sender, myEvenArgs e)
            {
                this.word.Text += "Cat:I am a cat\n";
            }
        }
        public class pig : Animal
        {
            TextBlock word;
            public pig(TextBlock w)
            {
                this.word = w;
            }
            public void saying(object sender, myEvenArgs e)
            {
                this.word.Text += "Pig:I am a pig\n";
            }
        }

        private dog d;
        private cat c;
        private pig p;
        public class myEvenArgs : EventArgs
        {
            public int x;
            public myEvenArgs(int i)
            {
                this.x = i;
            }
        }

        private void button_ok_Click(object sender, RoutedEventArgs e)
        {
            switch (textBox.Text)
            {
                case "dog":
                    textBlock.Text += "Dog:I am a dog\n";
                    break;
                case "cat":
                    textBlock.Text += "Cat:I am a cat\n";
                    break;
                case "pig":
                    textBlock.Text += "Pig:I am a pig\n";
                    break;
                default:
                    break;
            }
            textBox.Text = "";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Random rd = new Random();
            int i = rd.Next(1, 4); //1到100之间的数，可任意更改
            switch(i)
            {
                case 1:
                    d = new dog(textBlock);
                    OnSaying = new AnimalSaying(d.saying);
                    break;
                case 2:
                    c = new cat(textBlock);
                    OnSaying = new AnimalSaying(c.saying);
                    break;
                case 3:
                    p = new pig(textBlock);
                    OnSaying = new AnimalSaying(p.saying);
                    break;
                default:
                    break;
            }
            OnSaying(this, new myEvenArgs(i));
        }
    }
}