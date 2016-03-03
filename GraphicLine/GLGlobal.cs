using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicLine
{
    public class GLGlobal
    {
        public GLGlobal()
        {

        }

        private static int HistoryDeep = 10800; //7 days
        public GLPoint[] GraphicLineDataArr = new GLPoint[HistoryDeep];
        
        public void PushPoint(DateTime in_datetime, int value)
        {
            Stack<GLPoint> Stack1 = new Stack<GLPoint>();
            for (int i=0;i<GraphicLineDataArr.Length;i++)
            {
                Stack1.Push(GraphicLineDataArr[i]);
            }
            Stack1.Push(new GLPoint(value, in_datetime));

            for (int i = GraphicLineDataArr.Length-1; i >=0; i--)
            {
                GraphicLineDataArr[i] = Stack1.Pop();
            }

        } 
    }
}
