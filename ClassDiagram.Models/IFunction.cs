using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassDiagram.Models
{
    public interface IFunction
    {
        List<Function> Functions
        {
            get;
            set;
        }
    }
}
