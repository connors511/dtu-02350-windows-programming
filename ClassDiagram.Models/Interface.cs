﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassDiagram.Models.Entities
{
    public abstract class Interface : ClassDiagram.Models.IEntity, ClassDiagram.Models.IFunction
    {
        public List<Function> Functions
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
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int CenterY
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
    }
}
