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
        public Models.ListItem editing_item;

        public int seleteItem;
        public ObservableCollection<Models.ListItem> AllItems{get { return this.allItems; } }

        private static ListItemViewModels _listItemViewModels;
        private ListItemViewModels()
        {
            allItems = new ObservableCollection<Models.ListItem>();
            seleted = false;
            editing_item = new Models.ListItem();
            seleteItem = -1;
        }

        static public ListItemViewModels getListItemViewModels()
        {
            if(_listItemViewModels ==null)
            {
                _listItemViewModels = new ListItemViewModels();
            }
            return _listItemViewModels;

        }

        public void Add(string title, string content, DateTimeOffset plan_date, string image)
        {
            this.allItems.Add(new Models.ListItem(title, content, plan_date, image));
        }

        public void Delete()
        {
            this.allItems.Remove(this.allItems[this.seleteItem]);
            this.seleted = false;
        }

        public void Update(string title, string content, DateTimeOffset plan_date, string image)
        {
            this.allItems[this.seleteItem].Title = title;
            this.allItems[this.seleteItem].Content = content;
            this.allItems[this.seleteItem].Plan_date = plan_date;
            this.allItems[this.seleteItem].ImageString = image;
            this.seleted = false;
        }

        public void Complete(int index, bool finish)
        {
            if(index>=0)
                this.allItems[index].Completed = finish;
        }
    }
}