using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

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
        public bool Completed { get; set; }
        public DateTimeOffset Plan_date { get; set; }

        public ImageSource ImageValue;

        public ImageSource Image
        {
            get
            {
                return ImageValue;
            }
            set
            {
                if(value != this.ImageValue)
                {
                    this.ImageValue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ListItem(string title, string content, DateTimeOffset plan_date, ImageSource image)
        {
            this.id = Guid.NewGuid().ToString();
            this.Title = title;
            this.Content = content;
            this.Completed = false;
            this.Plan_date = plan_date;
            this.Image = image;
        }


    }
}
