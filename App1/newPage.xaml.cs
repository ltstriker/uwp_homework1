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
        private string temp_img = "";

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
                    this.ViewModel.Add(title.Text, content.Text, dataPicker1.Date, temp_img);
                }
                else
                {
                    this.ViewModel.Update(title.Text, content.Text, dataPicker1.Date, temp_img);
                }

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

            
            temp_img = StorageApplicationPermissions.FutureAccessList.Add(file);
            /*
            StorageFile asd = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(token);
            */

            if (file != null)
            {       
                IRandomAccessStream ir = await file.OpenAsync(FileAccessMode.Read);
                BitmapImage bi = new BitmapImage();
                await bi.SetSourceAsync(ir);//should set source
                pic.Source = bi;
            }
        }

        private async void ReadImg(string ImageString)
        {
            StorageFile file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(ImageString);

            if (file != null)
            {
                IRandomAccessStream ir = await file.OpenAsync(FileAccessMode.Read);
                BitmapImage bi = new BitmapImage();
                await bi.SetSourceAsync(ir);//should set source
                pic.Source = bi;
            }
            else
            {
                pic.Source = new BitmapImage(new Uri("ms-appx:Assets/StoreLogo.png"));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.NavigationMode == NavigationMode.New)
            {
                ApplicationData.Current.LocalSettings.Values.Remove("newPage");

                int param = (int)e.Parameter;


                temp_img = "";

                if ((int)param != -1)
                {
                    ListItem item = this.ViewModel.AllItems[(int)param];
                    title.Text = item.Title;
                    content.Text = item.Content;
                    dataPicker1.Date = item.Plan_date;
                    pic.Source = item.Image;
                    temp_img = item.ImageString;
                    create.Content = "Update";
                }
            }
            else
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("newPage"))
                {
                    ApplicationDataCompositeValue composite = ApplicationData.Current.LocalSettings.Values["newPage"] as ApplicationDataCompositeValue;
                    title.Text = (string)composite["title"];
                    content.Text = (string)composite["content"];
                    dataPicker1.Date = (DateTimeOffset)composite["time"];
                    ReadImg((string)composite["img"]);
                    temp_img = (string)composite["img"];

                    ViewModel.seleted = (bool)composite["changing"];
                    ViewModel.seleteItem = (int)composite["changingItem"];

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
                composite["img"] = temp_img;
                composite["changing"] = ViewModel.seleted;
                composite["changingItem"] = ViewModel.seleteItem;
                ApplicationData.Current.LocalSettings.Values["newPage"] = composite;
            }
        }

        private void Reset()
        {
            title.Text = "";
            content.Text = "";
            temp_img = "";
            dataPicker1.Date = DateTimeOffset.Now;

            BitmapImage bi = new BitmapImage(new Uri("ms-appx:Assets/StoreLogo.png"));
            pic.Source = bi;
            create.Content = "Create";
        }

    }
}
