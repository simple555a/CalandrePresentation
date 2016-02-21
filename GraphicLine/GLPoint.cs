using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicLine
{
    public class GLPoint
    {
        public int value;
        public DateTime datetime;

        public GLPoint()
        { }

        public GLPoint(int value, DateTime datetime)
        {
            this.value = value;
            this.datetime = datetime;
        }
    }
}
