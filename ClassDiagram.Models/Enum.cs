using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassDiagram.Models.Entities
{
    public abstract class Enum : ClassDiagram.Models.IEntity
    {
        public int CanvasCenterX
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int CanvasCenterY
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int CenterX
        {
            get { throw new NotImplementedException(); }
        }

        public int CenterY
        {
            get { throw new NotImplementedException(); }
        }
    }
}
