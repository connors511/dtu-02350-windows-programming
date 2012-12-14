using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace ClassDiagram.Models
{
    [Serializable()]
    public class Argument : ISerializable, INotifyPropertyChanged
    {
        public Argument(SerializationInfo info, StreamingContext ctxt)
        {
            this.Name = (string)info.GetValue("Name", typeof(string));
            this.Type = (string)info.GetValue("Type", typeof(string));
            this.Value = (string)info.GetValue("Value", typeof(string));
        }

        public Argument()
        {
            // TODO: Complete member initialization
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Name", this.Name);
            info.AddValue("Type", this.Type);
            info.AddValue("Value", this.Value);
        }

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
        
        // Event handler
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }  

}
