using App1.database;
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

        public string id;
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
                    Db.GetInstance().Complete(this.id, this.CompleteValue);
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
        public string ImagePath;

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

        public ListItem(string title, string content, DateTimeOffset plan_date, string image, string id="", bool finish = false)
        {
            if(id == "")
                this.id = Guid.NewGuid().ToString();
            else
                this.id = id;
            this.Title = title;
            this.Content = content;
            this.Completed = finish;
            this.Plan_date = plan_date;
            this.ImageString = image;
        }

        public ListItem()
        {
            this.id = Guid.NewGuid().ToString();
            this.Title = "";
            this.Content = "";
            this.Completed = false;
            this.Plan_date = DateTimeOffset.Now;
            this.ImageString = "";
        }


        private async void ReadImg()
        {
            if (ImageString == "")
            {
                Image = new BitmapImage(new Uri("ms-appx:Assets/StoreLogo.png"));
                ImagePath = "Assets/StoreLogo.png";
            }
            else
            {
                StorageFile file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(ImageString);
                if (file != null)
                {
                    ImagePath = file.Path;
                    IRandomAccessStream ir = await file.OpenAsync(FileAccessMode.Read);
                    BitmapImage bi = new BitmapImage();
                    await bi.SetSourceAsync(ir);//should set source
                    Image = bi;
                }
                else
                {
                    ImagePath = "Assets/StoreLogo.png";
                    Image = new BitmapImage(new Uri("ms-appx:Assets/StoreLogo.png"));
                }
            }
        }

    }
}
