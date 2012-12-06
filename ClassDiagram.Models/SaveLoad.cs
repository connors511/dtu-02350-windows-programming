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
                // TODO: Throw exception?
                return "";
            }
            
            /*XmlSerializer x = new XmlSerializer(bases.GetType(), new Type[] { typeof(Entity), 
                                                                              typeof(System.Windows.Media.SolidColorBrush),
                                                                              typeof(System.Windows.Media.MatrixTransform)});
            Stream stream = File.Open(file, FileMode.Create);

            var b = new ListWrapper(bases);
            x.Serialize(stream, b.bases);*/
            var se = new Serializer();
            se.SerializeObject(file, bases);

            return file;
        }

        public static void Load(out List<Base> bases, out string currentFile)
        {
            bases = new List<Base>();
            string file = openText();
            currentFile = file;
            if (file == "")
            {
                // TODO: Throw exception?
                return;
            }
            
            /*XmlSerializer serializer = new XmlSerializer(typeof(List<Base>));
            
            StreamReader reader = new StreamReader(file);
            // Invalid XML error
            bases = (List<Base>)serializer.Deserialize(reader);
            reader.Close();*/

            var se = new Serializer();
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

            //Set the starting directory and the title.
            dialog.InitialDirectory = "C:";
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

            //Set the starting directory and the title.
            dialog.InitialDirectory = "C:";
            dialog.Title = "Select a ClassDiagrammer file";

            //Present to the user.
            if (dialog.ShowDialog() == DialogResult.OK)
                strFileName = dialog.FileName;
            return strFileName;
        }
        
        public class Serializer
        {
            public Serializer()
            {
            }

            public void SerializeObject(string filename, object objectToSerialize)
            {
                Stream stream = File.Open(filename, FileMode.Create);
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
