using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassDiagram.Models.Entities
{
    public class Class : Entity, ClassDiagram.Models.IEntity, ClassDiagram.Models.IFunction, ClassDiagram.Models.IProperty
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

        public List<Property> properties
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
