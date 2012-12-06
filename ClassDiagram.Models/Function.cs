using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ClassDiagram.Models
{
    [Serializable()]
    public class Function : ISerializable
    {
        public Function(SerializationInfo info, StreamingContext ctxt)
        {
            this.Visibility = (Visibility)info.GetValue("Visibility", typeof(Visibility));
            this.Name = (string)info.GetValue("Name", typeof(string));
            this.Type = (string)info.GetValue("Type", typeof(string));
            this.Arguments = (List<Argument>)info.GetValue("Arguments", typeof(List<Argument>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Visibility", this.Visibility);
            info.AddValue("Name", this.Name);
            info.AddValue("Type", this.Type);
            info.AddValue("Arguments", this.Arguments);
        }

        public Function()
        {
            Visibility = Models.Visibility.Public;
        }

        public Visibility Visibility
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public List<Argument> Arguments
        {
            get;
            set;
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
            List<string> args = new List<string>();
            Arguments.ForEach(a => args.Add(a.ToString()));
            return str + Name + "(" + ((args != null) ? String.Join(", ", args) : "") + ")" + ((Type != null) ? " : " + Type : "");
        }
    }
}
