﻿using System;
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
using System.Collections.ObjectModel;
using App1.Models;
using System.Collections;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace App1
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 


    public sealed partial class MainPage : Page
    {
        ViewModels.ListItemViewModels ViewModel = ViewModels.ListItemViewModels.getListItemViewModels();

        public MainPage()
        {
            this.InitializeComponent();

            if (ViewModel.AllItems.Count==0)
            {
                ViewModel.Add("test", "test", DateTimeOffset.Now, "");
            }

            right.Navigate(typeof(newPage), -1);
        }

        private void goToCreate(object sender, RoutedEventArgs e)
        {
            if (right.Visibility == Visibility.Collapsed)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(newPage), -1);
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            delete.Visibility = Visibility.Visible;
            ViewModel.seleted = true;
            ViewModel.seleteItem = ViewModel.AllItems.IndexOf((ListItem)e.ClickedItem);
            if (right.Visibility == Visibility.Collapsed)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(newPage), ViewModel.AllItems.IndexOf((ListItem)e.ClickedItem));
            }
            else
            {
                right.Navigate(typeof(newPage), ViewModel.AllItems.IndexOf((ListItem)e.ClickedItem));
            }
        }

        private async void Delete(object sender, RoutedEventArgs e)
        {
            if( ViewModel.seleted)
            {
                ViewModel.Delete(); 

                if (right.Visibility == Visibility.Collapsed)
                {
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(MainPage));
                }
                else
                {
                    right.Navigate(typeof(newPage), -1);
                }

                delete.Visibility = Visibility.Collapsed;
            }
            else
            {
                var dialog = new MessageDialog("内容未创建，不能删除", "错误");

                dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                dialog.Commands.Add(new UICommand("取消", cmd => { }, commandId: 1));

                //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                //获取返回值
                var result = await dialog.ShowAsync();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                ApplicationData.Current.LocalSettings.Values.Remove("mainPage");
            }
            else
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("mainPage"))
                {
                    ApplicationDataCompositeValue composite = ApplicationData.Current.LocalSettings.Values["mainPage"] as ApplicationDataCompositeValue;
                    ViewModel.Complete(0, (bool)composite["completed"]);
                    ViewModel.seleted = true;
                    ViewModel.seleteItem = 0;
                    ViewModel.Update((string)composite["title"], (string)composite["content"], (DateTimeOffset)composite["time"], (string)composite["img"]);
                    ApplicationData.Current.LocalSettings.Values.Remove("mainPage");
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (((App)App.Current).issuspend)
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                composite["completed"] = ViewModel.AllItems[0].Completed;
                composite["title"] = ViewModel.AllItems[0].Title;
                composite["time"] = ViewModel.AllItems[0].Plan_date;
                composite["img"] = ViewModel.AllItems[0].ImageString;
                ApplicationData.Current.LocalSettings.Values["mainPage"] = composite;
            }
        }
    }
}
