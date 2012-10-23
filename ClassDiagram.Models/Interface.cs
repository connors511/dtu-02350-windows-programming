using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassDiagram.Models.Entity
{
    public abstract class Interface : ClassDiagram.Models.IEntity, ClassDiagram.Models.IFunction
    {
        public List<Function> functions
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
