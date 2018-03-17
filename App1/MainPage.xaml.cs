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
using Windows.UI.Popups;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace App1
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            /*var dialog = new MessageDialog("当前设置尚未保存，你确认要退出该页面吗?", "消息提示");

            dialog.Commands.Add(new UICommand("确定", cmd => { textBlock.Text += "hah: yes \n"; }, commandId: 0));
            dialog.Commands.Add(new UICommand("取消", cmd => { textBlock.Text += "hah: no \n"; }, commandId: 1));

            //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            //获取返回值
            var result = await dialog.ShowAsync();*/
            line.Visibility = Visibility.Visible;
        }

        private void CheckBox_unchecked(object sender, RoutedEventArgs e)
        {
            line.Visibility = Visibility.Collapsed;
        }

        private async void btn_click(object sender, RoutedEventArgs e)
        {
            var localTime = DateTimeOffset.Now;

            if (title.Text == "")
            {
                var dialog = new MessageDialog("title 不应该为空", "错误");

                dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                dialog.Commands.Add(new UICommand("取消", cmd => { }, commandId: 1));

                //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
                
                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;
           
                //获取返回值
                var result = await dialog.ShowAsync();
            } else if (content.Text == "") {
                var dialog = new MessageDialog("content 不应该为空", "错误");

                dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                dialog.Commands.Add(new UICommand("取消", cmd => { }, commandId: 1));

                //设置默认按钮，不设置的话默认的确认按钮是第一个按钮

                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                //获取返回值
                var result = await dialog.ShowAsync();
            } else if (dataPicker1.Date.CompareTo(localTime.Date) >= 0) {
                //textBlock.Text += title.Text+": "+content.Text+"\n   "+dataPicker1.Date.ToString()+"\n\n";
            } else {
                var dialog = new MessageDialog("时间不该为已过去的时间", "错误");

                dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                dialog.Commands.Add(new UICommand("取消", cmd => { }, commandId: 1));

                //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                //获取返回值
                var result = await dialog.ShowAsync();
            }


        }
    }
}
