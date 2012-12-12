using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ClassDiagram.Models.Arrows
{
    public class Inherit : Association
    {
        public Inherit()
            : base()
        {
        }

        public Inherit(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {

        }
    }
}
