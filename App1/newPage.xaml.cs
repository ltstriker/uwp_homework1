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
using Windows.UI.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using System.Collections;
using App1.Models;
using Windows.Storage.AccessCache;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace App1
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 


    public sealed partial class newPage : Page
    {
        ViewModels.ListItemViewModels ViewModel = ViewModels.ListItemViewModels.getListItemViewModels();

        public newPage()
        {
            this.InitializeComponent();
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
            }
            else if (content.Text == "")
            {
                var dialog = new MessageDialog("content 不应该为空", "错误");

                dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                dialog.Commands.Add(new UICommand("取消", cmd => { }, commandId: 1));

                //设置默认按钮，不设置的话默认的确认按钮是第一个按钮

                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                //获取返回值
                var result = await dialog.ShowAsync();
            }
            else if (dataPicker1.Date.CompareTo(localTime.Date) < 0)
            {
                var dialog = new MessageDialog("时间不该为已过去的时间", "错误");

                dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                dialog.Commands.Add(new UICommand("取消", cmd => { }, commandId: 1));

                //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                //获取返回值
                var result = await dialog.ShowAsync();
            }
            else
            {
                if(!ViewModel.seleted)
                {
                    this.ViewModel.Add(title.Text, content.Text, dataPicker1.Date, ViewModel.editing_item.ImageString);
                }
                else
                {
                    this.ViewModel.Update(title.Text, content.Text, dataPicker1.Date, ViewModel.editing_item.ImageString);
                }
                ActiveTile.ActiveTile.UpdateForAll();

                Frame rootFrame = Window.Current.Content as Frame;
                if (rootFrame.CanGoBack)
                {
                    rootFrame.GoBack();
                }
                else
                {
                    Reset();
                }
            }


        }

        private void goBack(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
            else
            {
                Reset();
            }
        }

        private async void selectPicture(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                ViewModel.editing_item.ImageString = StorageApplicationPermissions.FutureAccessList.Add(file);
                pic.Source = ViewModel.editing_item.Image;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.NavigationMode == NavigationMode.New)
            {
                ApplicationData.Current.LocalSettings.Values.Remove("newPage");
                if (ViewModel.seleted)
                {
                    create.Content = "Update";
                }
                else
                {
                    create.Content = "Create";
                }
            }
            else
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("newPage"))
                {
                    ApplicationDataCompositeValue composite = ApplicationData.Current.LocalSettings.Values["newPage"] as ApplicationDataCompositeValue;
                    ViewModel.editing_item = new ListItem((string)composite["title"], (string)composite["content"], (DateTimeOffset)composite["time"], 
                        (string)composite["img"]);
                    ViewModel.seleted = (bool)composite["seleted"];
                    ViewModel.seleteItem = (int)composite["seleted_item"];

                    ApplicationData.Current.LocalSettings.Values.Remove("newPage");
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (((App)App.Current).issuspend)
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                composite["title"] = title.Text;
                composite["content"] = content.Text;
                composite["time"] = dataPicker1.Date;
                composite["img"] = ViewModel.editing_item.ImageString;
                composite["seleted"] = ViewModel.seleted;
                composite["seleted_item"] = ViewModel.seleteItem;
                ApplicationData.Current.LocalSettings.Values["newPage"] = composite;
            }
        }

        private void Reset()
        {
            title.Text = "";
            content.Text = "";
            dataPicker1.Date = DateTimeOffset.Now;

            create.Content = "Create";

            ViewModel.seleted = false;
            ViewModel.editing_item = new Models.ListItem();

            BitmapImage bi = new BitmapImage(new Uri("ms-appx:Assets/StoreLogo.png"));
            pic.Source = bi;
        }

    }
}
