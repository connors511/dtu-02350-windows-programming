using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ClassDiagram.Models
{
    [Serializable()]
    public class Property : Argument, ISerializable
    {
        public Property(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
        {
            this.Visibility = (Visibility)info.GetValue("Visibility", typeof(Visibility));
        }

        public Property()
        {
            // TODO: Complete member initialization
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Visibility", this.Visibility);
            base.GetObjectData(info, ctxt);
        }

        private Visibility _visibility;
        public Visibility Visibility
        {
            get
            {
                return _visibility;
            }
            set
            {
                _visibility = value;
            }
        }

        public override string ToString()
        {
            string str = "";
            switch (Visibility)
            {
                case Visibility.Public:
                    str += "+";
                    break;
                case Visibility.Protected:
                    str += "#";
                    break;
                case Visibility.Private:
                    str += "-";
                    break;
            }
            return str + base.ToString();
        }
    }
}
