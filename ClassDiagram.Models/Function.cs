using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassDiagram.Models
{
    public class Function
    {
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
