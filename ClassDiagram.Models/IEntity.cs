using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassDiagram.Models
{
    public interface IEntity
    {
        int CanvasCenterX { get; set; }
        int CanvasCenterY { get; set; }
        int CenterX { get; }
        int CenterY { get; }
    }
}
