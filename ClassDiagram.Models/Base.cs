using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows;
using System.Text.RegularExpressions;

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
                if (this.GetType().Name == "Class")
                {
                    var cl = (ClassDiagram.Models.Entities.Class)this;
                    if (cl.Properties != null)
                    {
                        cl.Properties.ForEach(x =>
                        {
                            switch (x.Visibility)
                            {
                                case Visibility.Public:
                                    str += "+";
                                    break;
                                case Visibility.Protected:
                                    str += "#";
                                    break;
                                case Visibility.Private:
                                    str += "-";
                                    break;
                            }
                            str += x.Name + "()\n";
                        });
                    }
                }
                return str;
            }
            set
            {
                string str = value;
                if (str != this.EditText)
                {
                    Match m = Regex.Match(str, @"(abstract class|class|enum|struct) (.+)");
                    if (m.Success)
                    {
                        // m.Groups[1].Value contains type
                        Name = m.Groups[2].Value;
                        System.Console.WriteLine(Name);
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
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
