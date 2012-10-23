using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassDiagram.Models
{
    public interface IProperty
    {
        List<Property> properties
        {
            get;
            set;
        }
    }
}
