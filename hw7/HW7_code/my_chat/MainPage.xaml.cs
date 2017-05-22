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
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.UI.Popups;
using Windows.Data.Xml.Dom;

//“空白页”项模板在 http, //go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace my_chat
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage :  Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            initialize();
        }

        private XmlDocument document = new XmlDocument();
        private void Weather_Click(object sender, RoutedEventArgs e)
        {
            city.Text = "城市：";
            temp.Text = "温度：";
            WD.Text = "风向：";
            WS.Text = "风速：";
            SD.Text = "湿度：";
            time.Text = "更新时间：";
            GetWeather(City.Text);
            City.Text = "";
        }

        private void Phone_Click(object sender, RoutedEventArgs e)
        {
            phone.Text = "手机号：";
            area.Text = "手机号码所在地区区号：";
            postno.Text = "所在地区邮编：";
            att.Text = "手机号码归属地：";
            ctype.Text = "号码卡类型：";
            operators.Text = "所属运营商：";
            style_citynm.Text = "完整归属地：";
            GetPhone(num.Text);
            num.Text = "";
        }

        async void GetPhone(string PhoneNum)
        {
            Uri uri = new Uri("http://api.k780.com:88/?app=phone.get&phone=" + PhoneNum + "&appkey=10003&sign=b59bc3ef6191eb9f747dd4e83c99f2a4&format=xml");            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync(uri);
            document.LoadXml(result);
            XmlNodeList list = document.GetElementsByTagName("success");
            IXmlNode node = list.Item(0);
            if(node.InnerText == "")
            {
                var dialog = new MessageDialog("查询手机归属地失败，请重新输入");
                dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
                dialog.DefaultCommandIndex = 0;
                await dialog.ShowAsync();
            }
            else
            {
                list = document.GetElementsByTagName("phone");
                if(list.Count < 1)
                {
                    var dialog = new MessageDialog("查询手机归属地失败，请重新输入");
                    dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                    //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
                    dialog.DefaultCommandIndex = 0;
                    await dialog.ShowAsync();
                }
                else
                {
                    node = list.Item(0);
                    phone.Text += node.InnerText;
                    list = document.GetElementsByTagName("area");
                    if (list.Count < 1)
                    {
                        var dialog = new MessageDialog("查询手机归属地失败，请重新输入");
                        dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                        //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
                        dialog.DefaultCommandIndex = 0;
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        node = list.Item(0);
                        area.Text += node.InnerText;
                        list = document.GetElementsByTagName("postno");
                        if (list.Count < 1)
                        {
                            var dialog = new MessageDialog("查询手机归属地失败，请重新输入");
                            dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                            //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
                            dialog.DefaultCommandIndex = 0;
                            await dialog.ShowAsync();
                        }
                        else
                        {
                            node = list.Item(0);
                            postno.Text += node.InnerText;
                            list = document.GetElementsByTagName("att");
                            if (list.Count < 1)
                            {
                                var dialog = new MessageDialog("查询手机归属地失败，请重新输入");
                                dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                                //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
                                dialog.DefaultCommandIndex = 0;
                                await dialog.ShowAsync();
                            }
                            else
                            {
                                node = list.Item(0);
                                att.Text += node.InnerText;
                                list = document.GetElementsByTagName("ctype");
                                if (list.Count < 1)
                                {
                                    var dialog = new MessageDialog("查询手机归属地失败，请重新输入");
                                    dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                                    //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
                                    dialog.DefaultCommandIndex = 0;
                                    await dialog.ShowAsync();
                                }
                                else
                                {
                                    node = list.Item(0);
                                    ctype.Text += node.InnerText;
                                    list = document.GetElementsByTagName("operators");
                                    if (list.Count < 1)
                                    {
                                        var dialog = new MessageDialog("查询手机归属地失败，请重新输入");
                                        dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                                        //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
                                        dialog.DefaultCommandIndex = 0;
                                        await dialog.ShowAsync();
                                    }
                                    else
                                    {
                                        node = list.Item(0);
                                        operators.Text += node.InnerText;
                                        list = document.GetElementsByTagName("style_citynm");
                                        if (list.Count < 1)
                                        {
                                            var dialog = new MessageDialog("查询手机归属地失败，请重新输入");
                                            dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                                            //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
                                            dialog.DefaultCommandIndex = 0;
                                            await dialog.ShowAsync();
                                        }
                                        else
                                        {
                                            node = list.Item(0);
                                            style_citynm.Text += node.InnerText;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }                
            }           
        }

        async void GetWeather(string CityName)
        {
            if(CityDictionary.ContainsKey(CityName))
            {
                Uri uri = new Uri("http://www.weather.com.cn/data/sk/" + CityDictionary[CityName] + ".html");
                HttpClient client = new HttpClient();
                string result = await client.GetStringAsync(uri);
                JsonReader reader = new JsonTextReader(new StringReader(result));
                while (reader.Read())
                {
                    switch ((string)reader.Value)
                    {
                        default:
                            break;
                        case "city":
                            reader.Read();
                            city.Text += (reader.Value + "\n");
                            break;
                        case "temp":
                            reader.Read();
                            temp.Text += (reader.Value + "\n");
                            break;
                        case "WD":
                            reader.Read();
                            WD.Text += (reader.Value + "\n");
                            break;
                        case "WS":
                            reader.Read();
                            WS.Text += (reader.Value + "\n");
                            break;
                        case "SD":
                            reader.Read();
                            SD.Text += (reader.Value + "\n");
                            break;
                        case "time":
                            reader.Read();
                            time.Text += (reader.Value + "\n");
                            break;
                    }
                }
            }
            else
            {
                var dialog = new MessageDialog("查询城市天气失败，请重新输入");
                dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
                dialog.DefaultCommandIndex = 0;
                await dialog.ShowAsync();
            }
        }

        private Dictionary<string, string> CityDictionary = new Dictionary<string, string>();

        void initialize()
        {
            CityDictionary.Add("北京", "101010100");
            CityDictionary.Add("上海", "101020100");
            CityDictionary.Add("天津", "101030100");
            CityDictionary.Add("重庆", "101040100");
            CityDictionary.Add("济南", "101120101");
            CityDictionary.Add("澳门", "101330101");
            CityDictionary.Add("香港", "101320101");
            CityDictionary.Add("台北县", "101340101");
            CityDictionary.Add("合肥", "101220101");
            CityDictionary.Add("南京", "101190101");
            CityDictionary.Add("拉萨", "101140101");
            CityDictionary.Add("杭州", "101210101");
            CityDictionary.Add("福州", "101230101");
            CityDictionary.Add("广州", "101280101");
            CityDictionary.Add("南昌", "101240101");
            CityDictionary.Add("海口", "101310101");
            CityDictionary.Add("南宁", "101300101");
            CityDictionary.Add("贵阳", "101260101");
            CityDictionary.Add("长沙", "101250101");
            CityDictionary.Add("武汉", "101200101");
            CityDictionary.Add("成都", "101270101");
            CityDictionary.Add("昆明", "101290101");
            CityDictionary.Add("西宁", "101150101");
            CityDictionary.Add("郑州", "101180101");;
            CityDictionary.Add("西安", "101110101");
            CityDictionary.Add("太原", "101100101");
            CityDictionary.Add("银川", "101170101");
            CityDictionary.Add("兰州", "101160101");
            CityDictionary.Add("乌鲁木齐", "101130101");
            CityDictionary.Add("呼和浩特", "101080101");
            CityDictionary.Add("沈阳", "101070101");
            CityDictionary.Add("哈尔滨", "101050101");
            CityDictionary.Add("长春", "101060101");
            CityDictionary.Add("石家庄", "101090101");
        }      
    }
}
