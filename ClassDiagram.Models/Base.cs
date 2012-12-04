using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows;

namespace ClassDiagram.Models
{
    public abstract class Base : INotifyPropertyChanged
    {
        private bool _edit = false;
        public bool Edit
        {
            get
            {
                return _edit;
            }
            set
            {
                _edit = value;
                NotifyPropertyChanged("Edit");
                NotifyPropertyChanged("EditInvert");
            }
        }
        public bool EditInvert
        {
            get { return !_edit; }
        }

        public string EditText
        {
            get
            {
                string str = "";
                str += this.GetType().Name.ToString().ToLower() + " " + this.Name;
                str += "\n---\n";
                // render functions and fields
                return str;
            }
            set
            {
                Name = value;

            }
        }

        private int _x;
        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        private int _y;
        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private Brush _bordercolor = Brushes.Black;
        public Brush BorderColor
        {
            get
            {
                return _bordercolor;
            }
            set
            {
                _bordercolor = value;
            }
        }

        private Brush _color = Brushes.LightBlue;
        public Brush Color 
        { 
            get 
            { 
                return _color;
            }
            set
            {
                _color = value;
                NotifyPropertyChanged("Color");
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
