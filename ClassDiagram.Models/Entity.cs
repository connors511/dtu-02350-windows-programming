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
            ((List<Base>)info.GetValue("Inheritable", typeof(List<Base>))).ForEach(x => Inheritable.Add(x));
            ((List<Function>)info.GetValue("Functions", typeof(List<Function>))).ForEach(x => Functions.Add(x));
            ((List<Property>)info.GetValue("Properties", typeof(List<Property>))).ForEach(x => Properties.Add(x));
        }

        public Entity() : base()
        {
            // TODO: Complete member initialization
            X = Y = 100;
            Width = Height = 200;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Width", this.Width);
            info.AddValue("Height", this.Height);
            info.AddValue("Inheritable", this.Inheritable.ToList());
            info.AddValue("Functions", this.Functions.ToList());
            info.AddValue("Properties", this.Properties.ToList());
            base.GetObjectData(info, ctxt);
        }

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

        protected ObservableCollection<Base> Inheritable
        {
            get;
            set;
        }

        private ObservableCollection<Function> _functions = new ObservableCollection<Function>();
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
