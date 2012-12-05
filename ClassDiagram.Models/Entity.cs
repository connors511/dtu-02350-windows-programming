using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassDiagram.Models
{
    public class Entity : Base, IEntity
    {
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
