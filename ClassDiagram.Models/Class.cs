using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassDiagram.Models.Entities
{
    public class Class : Entity, ClassDiagram.Models.IEntity, ClassDiagram.Models.IFunction, ClassDiagram.Models.IProperty
    {
        public Class()
        {
            X = Y = 200;
            Width = Height = 100;
        }

        private List<Function> _functions;
        public List<Function> Functions
        {
            get
            {
                return _functions;
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
    }
}
