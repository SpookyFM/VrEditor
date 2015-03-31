using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrEditor
{

    public enum SerializationMode
    {
        SerializeEditor,
        SerializeGame
    }
    
    public struct ImageFileHolder : System.Xml.Serialization.IXmlSerializable
    {
        private string value;

        public static SerializationMode SerializationMode;

        public ImageFileHolder(string s)
        {
            value = s;
        }

        static public implicit operator ImageFileHolder(string s)
        {
            return new ImageFileHolder(s);
        }

        static public implicit operator String(ImageFileHolder i)
        {
            return i.value;
        }

        



        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            value = reader.ReadString();
            reader.ReadEndElement();
            
        }




        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (SerializationMode == VrEditor.SerializationMode.SerializeEditor)
            {
                writer.WriteString(value);
            }
            else
            {
                writer.WriteString(Path.GetFileNameWithoutExtension(value));
            }
        }
    }
}