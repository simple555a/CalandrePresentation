using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace GraphicLine
{
    public class HistoryClass
    {
        public HistoryClass()
        {

        }

        public string Filename;

        public GLPoint[] LoadFromXML()
        {
            GLPoint[] GraphicLineDataArr;

            if (File.Exists(Filename))
            {
                XmlSerializer XmlSerializer1 = new XmlSerializer(typeof(GLPoint[]));
                TextReader reader1 = new StreamReader(Filename);
                GraphicLineDataArr = (GLPoint[])XmlSerializer1.Deserialize(reader1);
                reader1.Dispose();
                return GraphicLineDataArr;
            }

            return new GLPoint[0];
        }

        public void LoadToXML(GLPoint[] data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GLPoint[]));
            TextWriter writer = new StreamWriter(Filename);
            serializer.Serialize(writer, data);
            writer.Dispose();
        }
    }
}
