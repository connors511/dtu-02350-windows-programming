using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassDiagram.Models.Entities
{
    public class Enum : Entity, ClassDiagram.Models.IEntity
    {
        public Enum()
        {
            X = Y = 100;
            Width = Height = 100;
        }
    }
}
