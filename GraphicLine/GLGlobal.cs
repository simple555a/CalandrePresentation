﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GraphicLine
{
    public class GLGlobal
    {
        public static int HistoryDeep = 70; //7 days
        public GLPoint[] GraphicLineDataArr;
        public GLGlobal()
        {

            GraphicLineDataArr = new GLPoint[HistoryDeep];

            //MessageBox.Show(GraphicLineDataArr.Length.ToString());
        }


        public void PushPoint(DateTime in_datetime, int in_value)
        {

            bool need_left_shift = true;
            for (int i = 0; i < GraphicLineDataArr.Length; i++)
            {
                if (GraphicLineDataArr[i]==null)
                {
                    GraphicLineDataArr[i] = new GLPoint();
                    GraphicLineDataArr[i].datetime = in_datetime;
                    GraphicLineDataArr[i].value = in_value;
                    need_left_shift = false;
                    break;
                }
            }

            if (need_left_shift)
            {

            }




            //if (need_left_shift==true)
            //{
            //    for (int i = 0; i < GraphicLineDataArr.Length-1; i++)
            //    {
            //        GraphicLineDataArr[i] = GraphicLineDataArr[i + 1];
            //    }
            //    GraphicLineDataArr[GraphicLineDataArr.Length - 1].datetime = in_datetime;
            //    GraphicLineDataArr[GraphicLineDataArr.Length - 1].value = in_value;
            //}

        }

        public TimeSpan GetGreenTimeAboveSpeed(DateTime StartTime, DateTime EndTime, int SetpointSpeed)
        {
            TimeSpan ret_value = new TimeSpan(0, 0, 0);
            TimeSpan ShiftDuration = new TimeSpan(12, 0, 0);
            TimeSpan add_value = new TimeSpan(0, 1, 0);
            for (int i = 0; i < GraphicLineDataArr.Length; i++)
            {
                try
                {
                    if (GraphicLineDataArr[i].datetime > StartTime && GraphicLineDataArr[i].datetime <= EndTime && GraphicLineDataArr[i].value >= SetpointSpeed)
                    {
                        ret_value = ret_value.Add(add_value);
                    }
                }
                catch
                {
                    //MessageBox.Show("catch");
                }
            }
            //ret_value = ShiftDuration - ret_value;
            return ret_value;
        }
    }
}
