using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace GraphicLine
{
    class History
    {
        public History()
        {

        }

        public GLPoint[] LoadFromXML()
        {
            GLPoint[] GraphicLineDataArr;

            if (File.Exists("GraphicLineData.xml"))
            {
                XmlSerializer XmlSerializer1 = new XmlSerializer(typeof(GLPoint[]));
                TextReader reader1 = new StreamReader("GraphicLineData.xml");
                GraphicLineDataArr = (GLPoint[])XmlSerializer1.Deserialize(reader1);
                reader1.Dispose();
                
                return GraphicLineDataArr;
            }

            return new GLPoint[0];
        }

        public void LoadToXML(GLPoint[] data)
        {

        }
    }
}
