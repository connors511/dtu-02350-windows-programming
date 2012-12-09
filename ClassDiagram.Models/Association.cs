using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ClassDiagram.Models.Arrows
{
    public class Association : Base, ISerializable
    {
        private Entity _start;
        public Entity Start
        {
            get
            {
                return _start;
            }
            set
            {
                if (_start != value)
                {
                    _start = value;
                    NotifyPropertyChanged("Start");
                }
            }
        }
        private Entity _end;
        public Entity End
        {
            get
            {
                return _end;
            }
            set
            {
                if (_end != value)
                {
                    _end = value;
                    NotifyPropertyChanged("End");
                }
            }
        }

        #region Constructor
        public Association(SerializationInfo info, StreamingContext ctxt)
        {
            this.Start = (Entity)info.GetValue("Start", typeof(Entity));
            this.End = (Entity)info.GetValue("End", typeof(Entity));
            events();
        }

        public Association()
        {
            // TODO: Complete member initialization
            events();
        }

        private void events()
        {
            this.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler((s, e) =>
            {
                if (e.PropertyName == "Start")
                {
                    Start.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler((se, ev) =>
                    {
                        if (ev.PropertyName == "X" || ev.PropertyName == "Y")
                        {
                            // This gets wild
                            NotifyPropertyChanged("startX");
                            NotifyPropertyChanged("startY");
                            NotifyPropertyChanged("endX");
                            NotifyPropertyChanged("endY");
                        }
                    });
                }
                else if (e.PropertyName == "End")
                {
                    End.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler((se, ev) =>
                    {
                        if (ev.PropertyName == "X" || ev.PropertyName == "Y")
                        {
                            // This gets wild
                            NotifyPropertyChanged("ArrowData");
                            NotifyPropertyChanged("startX");
                            NotifyPropertyChanged("startY");
                            NotifyPropertyChanged("endX");
                            NotifyPropertyChanged("endY");
                        }
                    });
                }
            });
        }
        #endregion

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Start", this.Start);
            info.AddValue("End", this.End);
        }

        public string ArrowData
        {
            get
            {
                // M 5,5 l 10,0 l -5,6 Z
                return string.Format("M {0},{1} l 10,0 l -5,6 Z", End.CanvasCenterX, End.CanvasCenterY);
            }
        }

        public int startX
        {
            get
            {
                if (Start.X + Start.Width < End.X)
                {
                    return Start.X + Start.Width;
                }
                else if (End.X + End.Width < Start.X)
                {
                    return Start.X;
                }
                // Be aware of special case where startY also falls through if-statements
                return Start.X + Start.Width / 2;
            }
        }

        public int startY
        {
            get
            {
                if (Start.Y + Start.Height < End.Y)
                {
                    return Start.Y + Start.Height;
                }
                else if (End.Y + End.Height < Start.Y)
                {
                    return Start.Y;
                }
                // Be aware of special case where startX also falls through if-statements
                return Start.Y + Start.Height / 2;
            }
        }

        public int endX
        {
            get
            {
                
                if (End.X < Start.X + Start.Height / 2)
                {
                    return End.X;
                }

                return End.X + End.Height / 2;
            }
        }

        public int endY
        {
            get
            {
                return End.Y;
            }
        }
    }
}
