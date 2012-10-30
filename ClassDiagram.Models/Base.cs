using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ClassDiagram.Models
{
    public abstract class Base : INotifyPropertyChanged
    {
        public int X
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int Y
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string name
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string color
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        // Event handler
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
