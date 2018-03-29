using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace App1.ViewModels
{ 
    class ListItemViewModels : INotifyPropertyChanged
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

        private ObservableCollection<Models.ListItem> allItems { get; set; }
        private bool seletedValue= false;
        public bool seleted
        {
            get { return seletedValue; }
            set
            {
                if(value!= seletedValue)
                {
                    seletedValue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int seleteItem;
        public ObservableCollection<Models.ListItem> AllItems{get { return this.allItems; } }

        public Visibility CanDelete { get { return seleted ? Visibility.Visible : Visibility.Collapsed; } }

        public ListItemViewModels()
        {
            this.allItems = new ObservableCollection<Models.ListItem>();
            seleted = false;
            seleteItem = -1;
        }

        public void Add(string title, string content, DateTimeOffset plan_date, ImageSource image)
        {
            this.allItems.Add(new Models.ListItem(title, content, plan_date, image));
        }

        public void Delete()
        {
            this.allItems.Remove(this.allItems[this.seleteItem]);
            this.seleted = false;
        }

        public void Update(string title, string content, DateTimeOffset plan_date, ImageSource image)
        {
            this.allItems[this.seleteItem].Title = title;
            this.allItems[this.seleteItem].Content = content;
            this.allItems[this.seleteItem].Plan_date = plan_date;
            this.allItems[this.seleteItem].Image = image;
            this.seleted = false;
        }
    }
}