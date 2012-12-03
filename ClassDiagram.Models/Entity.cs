using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassDiagram.Models
{
    public abstract class Entity : Base, IEntity
    {
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

        protected List<IEntity> inheritable
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
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
