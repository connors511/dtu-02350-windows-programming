using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ClassDiagram.Models
{
    [Serializable]
    public class Entity : Base, IEntity, ISerializable
    {
        public Entity(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
        {
            Width = (int)info.GetValue("Width", typeof(int));
            Height = (int)info.GetValue("Height", typeof(int));
            Inheritable = (List<Base>)info.GetValue("Inheritable", typeof(List<Base>)); // Needed?
            Functions = (List<Function>)info.GetValue("Functions", typeof(List<Function>));
            Properties = (List<Property>)info.GetValue("Properties", typeof(List<Property>));
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
            info.AddValue("Inheritable", this.Inheritable);
            info.AddValue("Functions", this.Functions);
            info.AddValue("Properties", this.Properties);
            base.GetObjectData(info, ctxt);
        }

        public string BodyText
        {
            get
            {
                string str = "";
                if (Properties != null)
                {
                    Properties.ForEach(x =>
                    {
                        str += x + "\n";
                    });
                }
                if (Functions != null)
                {
                    Functions.ForEach(x =>
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

        protected List<Base> Inheritable
        {
            get;
            set;
        }

        private List<Function> _functions;
        public List<Function> Functions
        {
            get
            {
                return (_functions == null) ? _functions : _functions.OrderBy(x => x.Visibility).ToList();
            }
            set
            {
                _functions = value;
            }
        }

        private List<Property> _properties;
        public List<Property> Properties
        {
            get
            {
                return (_properties == null) ? _properties : _properties.OrderBy(x => x.Visibility).ToList();
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
