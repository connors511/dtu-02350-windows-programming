using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassDiagram.Models
{
    public class Argument
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private string _type;
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        private string _value;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public override string ToString()
        {
            return Name + ((Value != null) ? " = " + (Value == "" ? "\"\"" : Value) : "") + ((Type != null) ? " : " + Type : "");
        }
    }
}
