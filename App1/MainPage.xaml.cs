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
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.AccessCache;
using App1.database;
using System.Text;

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
            ViewModel.seleted = true;
            ViewModel.seleteItem = ViewModel.AllItems.IndexOf((ListItem)e.ClickedItem);
            ViewModel.editing_item = ViewModel.AllItems[ViewModel.seleteItem];
            ViewModel.editing_item.ImageString = ViewModel.AllItems[ViewModel.seleteItem].ImageString;

            if (right.Visibility == Visibility.Collapsed)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(newPage));
            }
            else
            {
                right.Navigate(typeof(newPage));
            }
        }


        private void Delete(object sender, RoutedEventArgs e)
        {
            var dc = (sender as FrameworkElement).DataContext;
            var item = (listview.ContainerFromItem(dc) as ListViewItem).Content as ListItem;

            ViewModel.seleted = true;
            ViewModel.seleteItem = ViewModel.AllItems.IndexOf(item);
            ViewModel.Delete();
            Db.GetInstance().Remove(item.id);
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

                    ViewModel.seleted = (bool)composite["seleted"];
                    ViewModel.seleteItem = (int)composite["seleted_item"];
                    ViewModel.editing_item = new ListItem((string)composite["title_right"], (string)composite["content_right"], 
                        (DateTimeOffset)composite["time_right"], (string)composite["img_right"]);
                    ApplicationData.Current.LocalSettings.Values.Remove("mainPage");
                }
            }

            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (((App)App.Current).issuspend)
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();

                composite["seleted"] = ViewModel.seleted;
                composite["seleted_item"] = ViewModel.seleteItem;
                composite["title_right"] = ViewModel.editing_item.Title;
                composite["content_right"] = ViewModel.editing_item.Content;
                composite["time_right"] = ViewModel.editing_item.Plan_date;
                composite["img_right"] = ViewModel.editing_item.ImageString;

                ApplicationData.Current.LocalSettings.Values["mainPage"] = composite;
            }

            DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;
        }

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            var dc = (sender as FrameworkElement).DataContext;
            var item = (listview.ContainerFromItem(dc) as ListViewItem).Content as ListItem;
            ViewModel.seleted = true;
            ViewModel.seleteItem = ViewModel.AllItems.IndexOf(item);

            DataTransferManager.ShowShareUI();
        }

        async void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var dp = args.Request.Data;
            var deferral = args.Request.GetDeferral();
            Models.ListItem item_share = ViewModel.AllItems[ViewModel.seleteItem];
            ViewModel.seleted = false;

            StorageFile photoFile = null;
            if (item_share.ImageString == "")
            {
                photoFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/StoreLogo.png"));
            }
            else
            {
                photoFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(item_share.ImageString);
            }
            dp.Properties.Title = item_share.Title;
            dp.Properties.Description = "share todo item";
            dp.SetText(item_share.Content);
            dp.SetStorageItems(new List<StorageFile> { photoFile });

            deferral.Complete();
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            var dc = (sender as FrameworkElement).DataContext;
            var item = (listview.ContainerFromItem(dc) as ListViewItem).Content as ListItem;
            ViewModel.seleted = true;
            ViewModel.seleteItem = ViewModel.AllItems.IndexOf(item);
            ViewModel.editing_item = ViewModel.AllItems[ViewModel.seleteItem];
            ViewModel.editing_item.ImageString = ViewModel.AllItems[ViewModel.seleteItem].ImageString;

            if (right.Visibility == Visibility.Collapsed)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(newPage));
            }
            else
            {
                right.Navigate(typeof(newPage));
            }

        }

        private async void GoSearch(object sender, RoutedEventArgs e)
        {
            string msg = Db.GetInstance().Search(Search.Text);


            var dialog = new MessageDialog(msg.ToString(), "Result");

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
