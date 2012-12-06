using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ClassDiagram.Models
{
    public class LoadSave
    {

        public static void Serialize(ObservableCollection<Base> bases, String filename)
        {
        }

        public static void Serialize(ObservableCollection<Base> bases)
        {

            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            //saveFileDialog1.AddExtension = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {

                    // To serialize the Circle, you must first open a stream for 
                    // writing. Use a file stream here.
                    //myStream = new FileStream(saveFileDialog1.FileName, FileMode.Create);

                    // Construct a BinaryFormatter and use it 
                    // to serialize the data to the stream.
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        formatter.Serialize(myStream, bases);
                    }
                    catch (SerializationException e)
                    {
                        Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                        throw;
                    }
                    finally
                    {
                        myStream.Close();
                    }

                }
            }
        }


        public static void Deserialize()
        {
            // Declare the Circle reference.
            /*Circle c = null;

            // Open the file containing the data that you want to deserialize.
            FileStream fs = new FileStream("DataFile.dat", FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                // Deserialize the Circle from the file and 
                // assign the reference to the local variable.
                c = (Circle)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }

            // To prove that the Circle deserialized correctly, display its area.
            Console.WriteLine("Object being deserialized: " + c.ToString());
            Console.WriteLine(c.Bjarne);
            Console.ReadKey();*/

        }

    }
}
