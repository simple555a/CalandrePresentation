using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

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

        public TimeSpan GetExeededTimeBelowSpeed(DateTime StartTime, DateTime EndTime, int SetpointSpeed)
        {
            TimeSpan ret_value = new TimeSpan(0, 0, 0);
            TimeSpan ShiftDuration = new TimeSpan(12, 0, 0);
            TimeSpan add_value = new TimeSpan(0, 1, 0);
            for (int i=0; i<GraphicLineDataArr.Length;i++)
            {
                if (GraphicLineDataArr[i].datetime>StartTime && GraphicLineDataArr[i].datetime<=EndTime && GraphicLineDataArr[i].value>=SetpointSpeed)
                {
                    ret_value=ret_value.Add(add_value);
                }
            }
            ret_value = ShiftDuration - ret_value;

            //MessageBox.Show(ret_value.ToString());
            return ret_value;
        }
    }
}
