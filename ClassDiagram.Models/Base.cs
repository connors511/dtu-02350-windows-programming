﻿using System;
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
        private eType _type;
        public eType Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    switch (_type)
                    {
                        case eType.AbstractClass:
                            this.Color = Brushes.LightSteelBlue;
                            break;
                        case eType.Enum:
                            this.Color = Brushes.LightYellow;
                            break;
                        case eType.Struct:
                            this.Color = Brushes.LightSkyBlue;
                            break;
                        case eType.Class:
                        default:
                            this.Color = Brushes.LightBlue;
                            break;
                    }
                    NotifyPropertyChanged("Type");
                    NotifyPropertyChanged("Color");
                }
            }
        }

        private bool _edit = false;
        public bool Edit
        {
            get
            {
                return _edit;
            }
            set
            {
                if (_edit != value)
                {
                    _edit = value;
                    NotifyPropertyChanged("Edit");
                    NotifyPropertyChanged("EditInvert");
                }
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
                var tc = new TypeConverter();
                str += tc.Convert(this.Type,null,null,null) + " " + this.Name;
                str += "\n---\n";
                // render functions and fields
                if (this.GetType().Name == "Entity")
                {
                    var cl = (ClassDiagram.Models.Entity)this;
                    if (cl.Properties != null)
                    {
                        cl.Properties.ForEach(x =>
                        {
                            str += x + "\n";
                        });
                    }
                    if (cl.Functions != null)
                    {
                        cl.Functions.ForEach(x =>
                        {
                            str += x + "\n";
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
