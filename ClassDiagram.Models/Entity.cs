using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace ClassDiagram.Models
{
    [Serializable]
    public class Entity : Base, IEntity, ISerializable
    {
        public Entity(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
        {
            Width = (int)info.GetValue("Width", typeof(int));
            Height = (int)info.GetValue("Height", typeof(int));
            /*var fs = ((List<Function>)info.GetValue("Functions", typeof(List<Function>)));
            var of = new ObservableCollection<Function>();
            fs.ForEach(x => of.Add(x));
            Functions = of;
            var ps = ((List<Property>)info.GetValue("Properties", typeof(List<Property>)));
            var op = new ObservableCollection<Property>();
            ps.ForEach(x => op.Add(x));
            Properties = op;*/
            Properties = ((List<Property>)info.GetValue("Properties", typeof(List<Property>)));
            Functions = ((List<Function>)info.GetValue("Functions", typeof(List<Function>)));
        }

        public Entity() : base()
        {
            // TODO: Complete member initialization
            X = Y = 100;
            Width = 200;
            Height = 100;
            Functions = new List<Function>();
            Properties = new List<Property>();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Width", this.Width);
            info.AddValue("Height", this.Height);
            info.AddValue("Functions", this.Functions.ToList());
            info.AddValue("Properties", this.Properties.ToList());
            base.GetObjectData(info, ctxt);
        }
        public List<Function> Functions { get; set; }
        public List<Property> Properties { get; set; }

        /*private ObservableCollection<Function> _functions = new ObservableCollection<Function>();
        public ObservableCollection<Function> Functions
        {
            get
            {
                return (_functions == null) ? _functions : _functions;
            }
            set
            {
                _functions = value;
            }
        }

        private ObservableCollection<Property> _properties = new ObservableCollection<Property>();
        public ObservableCollection<Property> Properties
        {
            get
            {
                return (_properties == null) ? _properties : _properties;
            }
            set
            {
                _properties = value;
            }
        }
        */
        public string BodyText
        {
            get
            {
                string str = "";
                if (Properties != null)
                {
                    Properties.ToList().ForEach(x =>
                    {
                        str += x + "\n";
                    });
                }
                if (Functions != null)
                {
                    Functions.ToList().ForEach(x =>
                    {
                        str += x + "\n";
                    });
                }
                return str;
            }
        }

        private int _width;
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        private int _height;
        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        public bool canInherit(IEntity model)
        {
            throw new System.NotImplementedException();
        }

        public int CanvasCenterX
        {
            get 
            {
                return this.X + this.Width / 2;
            }
            set 
            {
                this.X = value - this.Width / 2;
                NotifyPropertyChanged("X");
                NotifyPropertyChanged("CanvasCenterX");
            }
        }

        public int CanvasCenterY
        {
            get 
            {
                return this.Y + this.Height / 2;
            }
            set 
            {
                this.Y = value - this.Height / 2;
                NotifyPropertyChanged("Y");
                NotifyPropertyChanged("CanvasCenterY");
            }
        }

        public int CenterX
        {
            get 
            {
                return this.Width / 2;
            }
        }

        public int CenterY
        {
            get
            {
                return this.Height / 2;
            }
        }
    }
}
