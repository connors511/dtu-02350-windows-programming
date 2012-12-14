using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Markup;
using System.Xml;
using System.Runtime.Serialization;
using System.Threading;

namespace ClassDiagram.Models
{
    public class SaveLoad
    {
        public static string Save(List<Base> bases, string file = "")
        {
            if (file == "")
            {
                file = saveText();
            }
            if (file == "")
            {
                throw new Exception();
            }
            
            //var se = new Serializer();
            //se.SerializeObject(file, bases);
            Serializer.Instance.filename = file;
            ThreadPool.QueueUserWorkItem(Serializer.SerializeObject, bases);

            return file;
        }

        public static void Load(out List<Base> bases, out string currentFile)
        {
            bases = new List<Base>();
            string file = openText();
            if (file == "")
            {
                throw new Exception();
            }
            currentFile = file;

            var se = Serializer.Instance;
            bases = se.DeSerializeObject(file);
        }

        private static string openText()
        {
            //First, declare a variable to hold the user’s file selection.
            string strFileName = "";

            //Create a new instance of the OpenFileDialog because it's an object.
            OpenFileDialog dialog = new OpenFileDialog();

            //Now set the file type
            dialog.Filter = "ClassDiagrammer files (*.cdf)|*.cdf|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;

            //Set the starting directory and the title.
            //dialog.InitialDirectory = "C:";
            dialog.Title = "Select a ClassDiagrammer file";

            //Present to the user.
            if (dialog.ShowDialog() == DialogResult.OK)
                strFileName = dialog.FileName;
            return strFileName;
        }

        private static string saveText()
        {
            //First, declare a variable to hold the user’s file selection.
            string strFileName = "";

            //Create a new instance of the OpenFileDialog because it's an object.
            SaveFileDialog dialog = new SaveFileDialog();

            //Now set the file type
            dialog.Filter = "ClassDiagrammer files (*.cdf)|*.cdf|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;

            //Set the starting directory and the title.
            //dialog.InitialDirectory = "C:";
            dialog.Title = "Select a ClassDiagrammer file";

            //Present to the user.
            if (dialog.ShowDialog() == DialogResult.OK)
                strFileName = dialog.FileName;
            return strFileName;
        }
        
        public class Serializer
        {
            public string filename { get; set; }

            private static Serializer instance;
            private Serializer()
            {
            }
            public static Serializer Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new Serializer();
                    }
                    return instance;
                }
            }

            public static void SerializeObject(object objectToSerialize)
            {
                Stream stream = File.Open(Serializer.Instance.filename, FileMode.Create);
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(stream, objectToSerialize);
                stream.Close();

            }

            public List<Base> DeSerializeObject(string filename)
            {
                Stream stream = File.Open(filename, FileMode.Open);
                BinaryFormatter bFormatter = new BinaryFormatter();
                var o = (List<Base>)bFormatter.Deserialize(stream);
                stream.Close();
                return o;
            }
        }
    }
}
