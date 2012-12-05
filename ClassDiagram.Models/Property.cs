using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassDiagram.Models
{
    public class Property : Argument
    {
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
