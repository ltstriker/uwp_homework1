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
using System.Collections.ObjectModel;
using App1.Models;
using System.Collections;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace App1
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 


    public sealed partial class MainPage : Page
    {
        ViewModels.ListItemViewModels ViewModel = new ViewModels.ListItemViewModels();

        public MainPage()
        {
            this.InitializeComponent();

            ArrayList param = new ArrayList
            {
                ViewModel,
                -1
            };

            if (App.viewModel.GetType().ToString() == "System.Object")
                App.viewModel=ViewModel;
            else
            {
                ViewModel = (ViewModels.ListItemViewModels)App.viewModel;
            }


            right.Navigate(typeof(newPage), param);
        }

        private void goToCreate(object sender, RoutedEventArgs e)
        {
            ArrayList param = new ArrayList
            {
                ViewModel,
                -1
            };

            if (right.Visibility == Visibility.Collapsed)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(newPage), param);
            }
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            delete.Visibility = Visibility.Visible;
            ViewModel.seleted = true;
            ViewModel.seleteItem = ViewModel.AllItems.IndexOf((ListItem)e.ClickedItem);
            ArrayList param = new ArrayList
            {
                ViewModel,
                ViewModel.AllItems.IndexOf((ListItem)e.ClickedItem)
            };
            if (right.Visibility == Visibility.Collapsed)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(newPage), param);
            }
            else
            {
                right.Navigate(typeof(newPage), param);
            }
        }

        private async void Delete(object sender, RoutedEventArgs e)
        {
            if( ViewModel.seleted)
            {
                ViewModel.Delete();
                ArrayList param = new ArrayList
                {
                  ViewModel,
                  -1
                 };

                if (right.Visibility == Visibility.Collapsed)
                {
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(MainPage));
                }
                else
                {
                    right.Navigate(typeof(newPage), param);
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
    }
}
