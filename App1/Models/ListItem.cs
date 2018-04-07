using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace App1.Models
{
    class ListItem:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string id;
        private string TitleValue = "";
        private bool CompleteValue = false;

        public string Title
        {
            get
            {
                return this.TitleValue;
            }

            set
            {
                if (value != this.TitleValue)
                {
                    this.TitleValue = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Content { get; set; }
        public System.Nullable<bool> Completed {
            get
            {
                return (System.Nullable<bool>)this.CompleteValue;
            }
            set
            {
                if (value != this.CompleteValue)
                {
                    if(value.HasValue)
                    this.CompleteValue = value.Value;
                    else
                    {
                        this.CompleteValue = false;
                    }
                    NotifyPropertyChanged();
                }
            }
        }
        public DateTimeOffset Plan_date { get; set; }

        private string ImageStringValue { get; set; }
        public string ImageString {
            get
            {
                return ImageStringValue;
            }
            set
            {
                if (value != this.ImageStringValue)
                {
                    this.ImageStringValue = value;
                    ReadImg();
                    NotifyPropertyChanged();
                }
            }
        }

        private ImageSource ImageSourceValue;
        public ImageSource Image
        {
            get
            {
                return ImageSourceValue;
            }
            set
            {
                if(value != this.ImageSourceValue)
                {
                    this.ImageSourceValue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ListItem(string title, string content, DateTimeOffset plan_date, string image)
        {
            this.id = Guid.NewGuid().ToString();
            this.Title = title;
            this.Content = content;
            this.Completed = false;
            this.Plan_date = plan_date;
            this.ImageString = image;
        }

        private async void ReadImg()
        {
            if (ImageString == "")
            {
                Image = new BitmapImage(new Uri("ms-appx:Assets/StoreLogo.png"));
            }
            else
            {
                StorageFile file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(ImageString);
                if (file != null)
                {
                    IRandomAccessStream ir = await file.OpenAsync(FileAccessMode.Read);
                    BitmapImage bi = new BitmapImage();
                    await bi.SetSourceAsync(ir);//should set source
                    Image = bi;
                }
                else
                {
                    Image = new BitmapImage(new Uri("ms-appx:Assets/StoreLogo.png"));
                }
            }
        }

    }
}
