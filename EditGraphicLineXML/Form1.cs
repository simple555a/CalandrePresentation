using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphicLine;
using System.IO;
using System.Xml.Serialization;

namespace EditGraphicLineXML
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime T1 = get_T1(get_CURR());
            DateTime T2 = get_T2(get_CURR());

            GraphicLine.GLPoint[] a1 = new GLPoint[720]; //720 min + 1
            
            Random rnd_v = new Random();
            //for (int i = 0; i < a1.Length; i++)
            for (int i = 0; i < 60; i++)
            {
                a1[59 - i + 60 * 0] = new GLPoint(i, T1.AddMinutes(i));
            }
            for (int i = 0; i < 60; i++)
            {
                a1[59 - i + 60 * 1] = new GLPoint(i, T1.AddMinutes(i));
            }
            for (int i = 0; i < 60; i++)
            {
                a1[59 - i + 60 * 2] = new GLPoint(i, T1.AddMinutes(i));
            }
            for (int i = 0; i < 60; i++)
            {
                a1[59 - i + 60 * 3] = new GLPoint(i, T1.AddMinutes(i));
            }
            for (int i = 0; i < 60; i++)
            {
                a1[59 - i + 60 * 4] = new GLPoint(i, T1.AddMinutes(i));
            }
            for (int i = 0; i < 60; i++)
            {
                a1[59 - i + 60 * 5] = new GLPoint(i, T1.AddMinutes(i));
            }
            for (int i = 0; i < 60; i++)
            {
                a1[59 - i + 60 * 6] = new GLPoint(i, T1.AddMinutes(i));
            }
            for (int i = 0; i < 60; i++)
            {
                a1[59 - i + 60 * 7] = new GLPoint(i, T1.AddMinutes(i));
            }
            for (int i = 0; i < 60; i++)
            {
                a1[59 - i + 60 * 8] = new GLPoint(i, T1.AddMinutes(i));
            }
            for (int i = 0; i < 60; i++)
            {
                a1[59 - i + 60 * 9] = new GLPoint(i, T1.AddMinutes(i));
            }
            for (int i = 0; i < 60; i++)
            {
                a1[59 - i + 60 * 10] = new GLPoint(i, T1.AddMinutes(i));
            }
            for (int i = 0; i < 60; i++)
            {
                a1[59 - i + 60 * 11] = new GLPoint(i, T1.AddMinutes(i));
            }

            XmlSerializer serializer = new XmlSerializer(typeof(GLPoint[]));
            TextWriter writer = new StreamWriter(@"C:\Users\serzh\Documents\GitHub\CalandrePresentation\CalanderPresentation\bin\DebugToHost\GraphicLineData.xml");
            serializer.Serialize(writer, a1);
            writer.Dispose();
            

            //if (File.Exists(@"C:\Users\serzh\Documents\GitHub\CalandrePresentation\CalanderPresentation\bin\DebugToHost\GraphicLineData.xml"))
            //{
            //    XmlSerializer XmlSerializer1 = new XmlSerializer(typeof(Settings));
            //    TextReader reader1 = new StreamReader("settings.xml");
            //    Settings1 = (Settings)XmlSerializer1.Deserialize(reader1);
            //    reader1.Dispose();

            //    this.textBox1.Text = Settings1.SQLConnectionString;
            //    this.radioButton1.Checked = (Settings1.SQLWindowsAuthorization) ? true : false;
            //    this.radioButton2.Checked = (Settings1.SQLWindowsAuthorization) ? false : true;
            //    this.textBox5.Text = Settings1.SQLLogin;
            //    this.textBox6.Text = Settings1.SQLPassword;

            //    this.textBox3.Text = Settings1.OPCConnectionString;
            //    this.textBox4.Text = Settings1.OPCRingsCounterName;
            //}
        }

        /// <summary>
        /// Get EStart shift time
        /// </summary>
        /// <param name="in_StartTime">date of shift</param>
        /// <returns></returns>
        private DateTime get_T1(DateTime in_StartTime)
        {
            TimeSpan t1 = new TimeSpan(8, 0, 0);
            TimeSpan t2 = new TimeSpan(12, 0, 0);
            DateTime T1;

            if (radioButton1.Checked)
            {
                T1 = in_StartTime.Date + t1;
            }
            else
            {
                T1 = in_StartTime.Date + t1 + t2;
            }

            return T1;
        }

        /// <summary>
        /// Get End shift time
        /// </summary>
        /// <param name="in_StartTime">date of shift</param>
        /// <returns></returns>
        private DateTime get_T2(DateTime in_StartTime)
        {
            TimeSpan t1 = new TimeSpan(8, 0, 0);
            TimeSpan t2 = new TimeSpan(12, 0, 0);
            DateTime T2;

            if (radioButton1.Checked)
            {
                T2 = in_StartTime.Date + t1 + t2;
            }
            else
            {
                T2 = in_StartTime.Date + t1 + t2 + t2;
            }

            return T2;
        }

        private DateTime get_CURR()
        {
            DateTime CURR;

            CURR = DateTime.Now;
            return CURR;
        }
    }
}
