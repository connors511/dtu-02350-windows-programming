using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

namespace ClassDiagram.Models
{
    [Serializable()]
    public abstract class Base : INotifyPropertyChanged, ISerializable
    {
        public Base(SerializationInfo info, StreamingContext ctxt)
        {
            this.Type = (eType)info.GetValue("Type", typeof(eType));
            this.X = (int)info.GetValue("X", typeof(int));
            this.Y = (int)info.GetValue("Y", typeof(int));
            this.Name = (string)info.GetValue("Name", typeof(string));
            //this.BorderColor = (Brush)info.GetValue("BorderColor", typeof(Brush));
            //this.Color = (Brush)info.GetValue("Color", typeof(Brush));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Type", this.Type);
            info.AddValue("X", this.X);
            info.AddValue("Y", this.Y);
            info.AddValue("Name", this.Name);
            //info.AddValue("BorderColor", this.BorderColor);
            //info.AddValue("Color", this.Color);
        }

        public Base()
        {

        }

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
                        cl.Properties.ToList().ForEach(x =>
                        {
                            str += x + "\n";
                        });
                    }
                    if (cl.Functions != null)
                    {
                        cl.Functions.ToList().ForEach(x =>
                        {
                            str += x + "\n";
                        });
                    }
                }
                return str;
            }
            set
            {
                // TODO: Reverse engineer the get method
                string str = value;
                if (str != this.EditText)
                {
                    List<string> lines = str.Split('\n').ToList();
                    Match m = Regex.Match(lines.ElementAt(0), @"(abstract class|class|enum|struct) (.+)", RegexOptions.IgnoreCase);
                    if (m.Success)
                    {
                        // m.Groups[1].Value contains type
                        Name = m.Groups[2].Value;
                        switch (m.Groups[1].Value)
                        {
                            case "class":
                                Type = eType.Class;
                                break;
                            case "abstract class":
                                Type = eType.AbstractClass;
                                break;
                            case "enum":
                                Type = eType.Enum;
                                break;
                            case "struct":
                                Type = eType.Struct;
                                break;
                        }
                        Console.WriteLine(Name + " : " + Type);
                    }
                    else
                    {
                        throw new FormatException();
                    }
                    if (lines.ElementAt(1) != "---")
                    {
                        throw new FormatException();
                    }
                    lines.RemoveRange(0, 2);
                    if (this.GetType() == typeof(Entity))
                    {
                        ((Entity)this).Functions.Clear();
                        ((Entity)this).Properties.Clear();
                    }
                    foreach (string line in lines)
                    {
                        // Match functions
                        Match ma = Regex.Match(line, @"([#+-])(.+?)\((.+)\)(?: ?: ?(.+))");
                        if (ma.Success)
                        {
                            var f = new Function();
                            switch (ma.Groups[1].Value)
                            {
                                case "+":
                                    f.Visibility = Visibility.Public;
                                    break;
                                case "-":
                                    f.Visibility = Visibility.Private;
                                    break;
                                case "#":
                                    f.Visibility = Visibility.Protected;
                                    break;
                            }
                            f.Name = ma.Groups[2].Value;
                            f.Type = ma.Groups[4].Value;
                            foreach (string arg in ma.Groups[3].Value.Split(',').ToList())
                            {
                                var a = new Argument();
                                Match mx = Regex.Match(arg, @"(.+?)(?: ?= ?(.+))? ?: ?(.+)");
                                string vl = mx.Groups[2].Value;
                                if (mx.Groups[2].Value == "")
                                {
                                    vl = null;
                                }
                                if (mx.Success)
                                {
                                    a.Name = mx.Groups[1].Value;
                                    a.Value = vl;
                                    a.Type = mx.Groups[3].Value;
                                }
                                f.Arguments.Add(a);
                            }
                            ((Entity)this).Functions.Add(f);
                            Console.WriteLine(((Entity)this).Functions.Count + " : " + f);

                        }
                        else
                        {
                            // Match properties
                            ma = Regex.Match(line, @"([#+-])(.+?)(?: ?= ?(.+?))?(?: ?: ?(.+))");
                            if (ma.Success)
                            {
                                string val = ma.Groups[3].Value;
                                if (ma.Groups[3].Value == "")
                                {
                                    val = null;
                                }
                                var p = new Property()
                                {
                                    Name = ma.Groups[2].Value,
                                    Type = ma.Groups[4].Value,
                                    Value = val
                                };
                                switch (ma.Groups[1].Value)
                                {
                                    case "+":
                                        p.Visibility = Visibility.Public;
                                        break;
                                    case "-":
                                        p.Visibility = Visibility.Private;
                                        break;
                                    case "#":
                                        p.Visibility = Visibility.Protected;
                                        break;
                                }
                                ((Entity)this).Properties.Add(p);
                            }
                        }
                    }
                }
                NotifyPropertyChanged("Name");
                NotifyPropertyChanged("Type");
                NotifyPropertyChanged("BodyText");
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
                if (_x != value)
                {
                    _x = value;
                    NotifyPropertyChanged("X");
                }
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
                if (_y != value)
                {
                    _y = value;
                    NotifyPropertyChanged("Y");
                }
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
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
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
                if (_bordercolor != value)
                {
                    _bordercolor = value;
                    NotifyPropertyChanged("BorderColor");
                }
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
                if (_color != value)
                {
                    _color = value;
                    NotifyPropertyChanged("Color");
                }
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
