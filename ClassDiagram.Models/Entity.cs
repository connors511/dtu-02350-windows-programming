using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassDiagram.Models
{
    public abstract class Entity
    {
        private int width
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        private int height
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        private List<IEntity> inheritable
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
    }
}
