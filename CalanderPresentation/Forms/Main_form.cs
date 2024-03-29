﻿#define real_time
//#define bypass_opc_init
//#define hide_future_functional

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GraphicLine;
using System.IO;
using System.Xml.Serialization;
using CalanderPresentation.TYPES;
using System.Text;


/*
Problems:
1. When repainting TL and Gl - at first appear day before, and after this refresh correctly
2. (solved) Page navigation doesnt work properly
3. (solved - 11 as rest rule )0 and 11 state - how to calculate efficiency time
*/

namespace CalanderPresentation
{
    public partial class Main_form : Form
    {

        static Sql_class sql_obj = new Sql_class();
#if !bypass_opc_init
        static OPC_class opc_obj = new OPC_class();
#endif

#if !real_time
        public static DateTime fixed_CURR = new DateTime(2016, 03, 27, 19, 39, 00);
#endif
        static Timer Tick1sec = new Timer();
        static Timer Tick50msec = new Timer();
        static Timer Tick5sec = new Timer();
        static Timer Tick60sec = new Timer();
        static Timer TickGLDiscontinuity = new Timer();
        static Settings Settings1 = new Settings();
        static DateTime previous_time = new DateTime();
        static TimeSpan cal_exeeded_time = new TimeSpan(0, 0, 0);
        static bool RefreshAsNowInterlock = false;
        static int metersFromOrder = 0;
        static int prev_metersFromOrder, metersPerShift = 0;
        static int tick5sec_counter;


        //inteface global
        static bool MarketStartMoving = false;
        static double previous_efficiency_value = 0;

        #region GlobalDataVar
        static TimeSpan cal_green_time = new TimeSpan(0, 0, 0);
        static TimeLine.Section[] TLGlobalObject;
        static GraphicLine.GLGlobal GLGlobalObject = new GLGlobal();
        List<DataGridRow> DGGlobalObject = new List<DataGridRow>();
        #endregion

        //Statistic for shifts per day
        static ShiftStatisticClass NowStatictic = new ShiftStatisticClass();


        public Main_form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region Indication
            toolStripStatusLabel1.Text = "REAL TIME";
#if !real_time
            toolStripStatusLabel1.ForeColor = Color.Red;
#endif
#if real_time
            toolStripStatusLabel1.ForeColor = Color.Green;
#endif

            toolStripStatusLabel2.Text = "OPC Enagaged";
#if bypass_opc_init
            toolStripStatusLabel2.ForeColor = Color.Red;
#endif
#if !bypass_opc_init
            toolStripStatusLabel2.ForeColor = Color.Green;
#endif
            #endregion

#if hide_future_functional
            tableLayoutPanel9.Visible = false;
            button5.Visible = false;
#endif

            label9.Text = sql_obj.GetWCName();

            //set up components
            #region TimeLine
            timeLine1.LeftMargin = 20;
            timeLine1.RightMargin = 1;
            //timeLine1.TimeLineHeight = 30;
            #endregion
            #region GraphicLine
            graphicLine1.LeftMargin = 0;
            graphicLine1.RightMargin = 1;
            graphicLine1.SetpointSpeed = 40;
            graphicLine1.History.Filename = "graphicLine1Data.xml";
            GLGlobalObject.Discontinuity = graphicLine1.Discontinuity;
            GLGlobalObject.GraphicLineDataArr = graphicLine1.History.LoadFromXML();
            //MessageBox.Show(GLGlobalObject.GraphicLineDataArr[0].datetime.ToString());
            //MessageBox.Show(GLGlobalObject.GraphicLineDataArr[1].datetime.ToString());
            #endregion


            label5.Text = sql_obj.GetCurrentStatusAsString();

            Color temp_color_000 = sql_obj.GetCurrentStatusColor();
            label5.BackColor = temp_color_000;
            if ((((int)temp_color_000.R + (int)temp_color_000.G + (int)temp_color_000.B)) / 3 >= 170)
                label5.ForeColor = Color.Black;
            else
                label5.ForeColor = Color.White;

            //history browser
            tableLayoutPanel2.RowStyles[2].Height = 0;
            try
            {
                if (File.Exists("settings.xml"))
                {
                    XmlSerializer XmlSerializer1 = new XmlSerializer(typeof(Settings));
                    TextReader reader1 = new StreamReader("settings.xml");
                    Settings1 = (Settings)XmlSerializer1.Deserialize(reader1);
                    reader1.Dispose();

                    showHistoryBrowserToolStripMenuItem.Checked = Settings1.GENERALShowHistoryBrowser;
                    tableLayoutPanel2.RowStyles[2].Height = (Settings1.GENERALShowHistoryBrowser) ? 35 : 0;
                }
            }
            catch
            { }

            LabelsCenterPositioning(groupBox1);
            LabelsCenterPositioning(groupBox2);
            LabelsCenterPositioning(groupBox3);

            this.Text += " v1.1.14";

            //OPC
#if !bypass_opc_init
            //opc_obj.CurrentCounterOfMaterial = sql_obj.GetProductionCounter();
            opc_obj.CurrentCounterOfMaterial = sql_obj.GetProductionCounterFromOrder();
            opc_obj.AskAllValues();
            label4.Text = opc_obj.CurrentCounterOfMaterial.ToString();
            opc_obj.lockCount = (sql_obj.GetCurrentStatusAsInt() == 0) ? false : true;
#endif
            
            Tick1sec.Interval = 1000;
            Tick1sec.Tick += Tic1sec_Tick;
            Tick1sec.Start();

            Tick5sec.Interval = 5000;
            Tick5sec.Tick += Tick5sec_Tick;
            Tick5sec.Start();

            Tick60sec.Interval = 60000;
            Tick60sec.Tick += Tick60sec_Tick;
            Tick60sec.Start();

            TickGLDiscontinuity.Interval = graphicLine1.Discontinuity * 1000;
            TickGLDiscontinuity.Tick += TickGLDiscontinuity_Tick;
            TickGLDiscontinuity.Start();

            Tick50msec.Interval = 50;
            Tick50msec.Tick += Tick50msec_Tick;
            Tick50msec.Start();


            #region Check shift

            if (get_CURR().Hour >= 8 && get_CURR().Hour < 20)
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;
            #endregion
            #region Datetime picker current shift (day/night) and run GlobalPresenter() - value change event
#if !real_time
            DateTime BStartTime = fixed_CURR;
            dateTimePicker1.Value = BStartTime;
#endif
#if real_time
            //if (System.DateTime.Now.Hour < 9)
            //    dateTimePicker1.Value = System.DateTime.Now - TimeSpan.FromDays(1);
            //if (System.DateTime.Now.Hour >= 8)
            dateTimePicker1.Value = System.DateTime.Now;
#endif
            #endregion

            previous_time = get_CURR();
            //toolStripStatusLabel4.Text = dateTimePicker1.Value.ToString();

            DrawTotalScore();

            metersPerShift = LoadFromDumpData();
        }


        private void Tick50msec_Tick(object sender, EventArgs e)
        {
            if (MarketStartMoving)
            {
                label8.BackColor = Color.FromArgb((byte)((int)label8.BackColor.A - 5), label8.BackColor.R, label8.BackColor.G, label8.BackColor.B);
            }
            if (label8.BackColor.A < 5)
            {
                MarketStartMoving = false;
            }
        }

        private void Tic1sec_Tick(object sender, EventArgs e)
        {
            String year = System.DateTime.Now.Year.ToString();
            String month = System.DateTime.Now.ToString("MMMM");
            String day = System.DateTime.Now.Day.ToString();
            String hours = (System.DateTime.Now.TimeOfDay.Hours < 10) ? "0" + System.DateTime.Now.TimeOfDay.Hours.ToString() : System.DateTime.Now.TimeOfDay.Hours.ToString();
            String minutes = (System.DateTime.Now.TimeOfDay.Minutes < 10) ? "0" + System.DateTime.Now.TimeOfDay.Minutes.ToString() : System.DateTime.Now.TimeOfDay.Minutes.ToString();
            String seconds = (System.DateTime.Now.TimeOfDay.Seconds < 10) ? "0" + System.DateTime.Now.TimeOfDay.Seconds.ToString() : System.DateTime.Now.TimeOfDay.Seconds.ToString();
            label2.Text = year + " " + month + " " + day + "  " + hours + ":" + minutes + ":" + seconds;

            //opc
#if !bypass_opc_init
            try
            {
                opc_obj.AskAllValues();
                label6.Text = opc_obj.CurrentSpeed.ToString();
            }
            catch
            {

            }
            //label4.Text = opc_obj.CurrentCounterOfMaterial.ToString();
#endif

            //opc
#if !bypass_opc_init
            try
            {
                opc_obj.lockCount = (sql_obj.GetCurrentStatusAsInt() == 0) ? false : true;
            }
            catch
            {

            }
            //push current speed to GLHisory
            //opc_obj.AskAllValues();
            //GLGlobalObject.PushPoint(Tic1secCalltime, opc_obj.CurrentSpeed);
            //MessageBox.Show(GLGlobalObject.GraphicLineDataArr.Length.ToString());
#endif

            //graphicLine1.History.LoadToXML(GLGlobalObject.GraphicLineDataArr);
        }

        private void Tick5sec_Tick(object sender, EventArgs e)
        {
            if (!RefreshAsNowInterlock)
            {
                #region set SFI statuses
#if !bypass_opc_init
                if (Settings1.SQLAllowWriteToSFIDatabases)
                {

                    //700
                    if (opc_obj.CurrentSpeed < graphicLine1.SetpointSpeed && opc_obj.CurrentSpeed != 0 && sql_obj.GetCurrentStatusAsInt() == 0)
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"ChangeStatusLog.txt", true))
                        {
                            file.WriteLine("From " + sql_obj.GetCurrentStatusAsInt().ToString() + " to 700 " + DateTime.Now.ToString());
                        }
                        sql_obj.Set700Status();
                    }

                    //0
                    if (opc_obj.CurrentSpeed >= graphicLine1.SetpointSpeed && sql_obj.GetCurrentStatusAsInt() == 700)
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"ChangeStatusLog.txt", true))
                        {
                            file.WriteLine("From " + sql_obj.GetCurrentStatusAsInt().ToString() + "   to 0 " + DateTime.Now.ToString());
                        }
                        sql_obj.Set0Status();
                    }

                    //999
                    if (opc_obj.CurrentSpeed == 0 && sql_obj.GetCurrentStatusAsInt() == 700)
                    {
                        tick5sec_counter+=5;
                        if (tick5sec_counter >= 60)   //60sec
                        {
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"ChangeStatusLog.txt", true))
                            {
                                file.WriteLine("From " + sql_obj.GetCurrentStatusAsInt().ToString() + " to 999 " + DateTime.Now.ToString() + " " + tick5sec_counter);
                            }
                            sql_obj.Set999Status();
                            tick5sec_counter = 0;
                        }
                    }
                }
#endif
                #endregion
            }
            
            GlobalPresenter();
        }
        
        private void Tick60sec_Tick(object sender, EventArgs e)
        {

            graphicLine1.History.LoadToXML(GLGlobalObject.GraphicLineDataArr);

            #region  refresh all information

            if (!RefreshAsNowInterlock)
            {
                dateTimePicker1.Value = System.DateTime.Now;
                if (get_CURR().Hour >= 8 && get_CURR().Hour < 20)
                {
                    //MessageBox.Show("Day");
                    radioButton1.Checked = true;

                    //Push statistic
                    //PushStatisticData(NowStatictic);
                }
                if (get_CURR().Hour >= 20 && get_CURR().Hour < 24 || get_CURR().Hour >= 0 && get_CURR().Hour < 8)
                {
                    //MessageBox.Show("Night");
                    radioButton2.Checked = true;

                    //Push statistic
                    //PushStatisticData(NowStatictic);
                }
            }
            #endregion

            //MessageBox.Show(get_CURR().Hour.ToString() + " " + get_CURR().Minute.ToString());
            #region Push Statistic Data
            if ((get_CURR().Hour == 7 || get_CURR().Hour == 19) && (get_CURR().Minute == 59))
            {
                //MessageBox.Show("Push");
                PushStatisticData(NowStatictic);

                NowStatictic.AdditionalJobs = 0;
                NowStatictic.A_Rolls_amount = 0;
                NowStatictic.C_Rolls_amount = 0;
                NowStatictic.Efficiency = 0;
                NowStatictic.PeopleAmount = 0;
                NowStatictic.Prodused = 0;
                NowStatictic.ScrapAmount = 0;
                NowStatictic.ShiftName = "-1";
            }
            #endregion

            #region RefreshScore table
            if ((get_CURR().Hour == 8 || get_CURR().Hour == 20) && (get_CURR().Minute == 00))
            {
                DrawTotalScore();
            }
            #endregion

            loadToDumpData();
        }

        private void TickGLDiscontinuity_Tick(object sender, EventArgs e)
        {

            DateTime Tic1secCalltime = get_CURR();

#if !bypass_opc_init
            //push current speed to GLHisory
            opc_obj.AskAllValues();
            GLGlobalObject.PushPoint(Tic1secCalltime, opc_obj.CurrentSpeed);
            //MessageBox.Show(GLGlobalObject.GraphicLineDataArr.Length.ToString());
#endif
        }

        

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void connectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionForm ConnectionsForm1 = new ConnectionForm();
            ConnectionsForm1.Show();
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
            TimeSpan t3 = new TimeSpan(4, 0, 0);
            DateTime T1 = new DateTime();

            //if (radioButton1.Checked)
            //{
            //    T1 = in_StartTime.Date + t1;
            //}
            //else
            //{
            //    T1 = in_StartTime.Date + t1 + t2;
            //}

            if (in_StartTime.Hour < 8) T1 = in_StartTime.Date - t3;
            if (in_StartTime.Hour >= 20) T1 = in_StartTime.Date + t1 + t2;
            if (in_StartTime.Hour >= 8 && in_StartTime.Hour < 20) T1 = in_StartTime.Date + t1;

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
            TimeSpan t3 = new TimeSpan(4, 0, 0);
            DateTime T2 = new DateTime();

            //if (radioButton1.Checked)
            //{
            //    T2 = in_StartTime.Date + t1 + t2;
            //}
            //else
            //{
            //    T2 = in_StartTime.Date + t1 + t2 + t2;
            //}


            if (in_StartTime.Hour < 8) T2 = in_StartTime.Date + t1;
            if (in_StartTime.Hour >= 20) T2 = in_StartTime.Date + t1 + t2 + t2;
            if (in_StartTime.Hour >= 8 && in_StartTime.Hour < 20) T2 = in_StartTime.Date + t1 + t2;

            return T2;
        }

        private DateTime get_CURR()
        {
#if !real_time
            return fixed_CURR;
#endif
#if real_time
            return DateTime.Now;
#endif
        }

        private DateTime get_CURR_wo_seconds()
        {
            DateTime CURR;
#if !real_time
            CURR = fixed_CURR;
#endif
#if real_time
            CURR = DateTime.Now;
#endif
            return new DateTime(CURR.Year, CURR.Month, CURR.Day, CURR.Hour, CURR.Minute, 0);
        }

        private void TimeLinePresenter(TimeLine.TimeLine in_control, DateTime in_StartTime, TimeLine.Section[] a1)
        {

            DateTime T1 = get_T1(in_StartTime);
            DateTime T2 = get_T2(in_StartTime);
            DateTime CURR = get_CURR();
            //MessageBox.Show(in_StartTime.ToString());
            //MessageBox.Show(T1.ToString());

            in_control.SetEmpty();

            //for ASC sql order
            if (a1.Length != 0)
            {
                in_control.AddBasePeriod(T1, T2, false);
                //not empty left
                if (a1[0].StartTime < T1 && a1[0].EndTime != DateTime.MaxValue)
                    in_control.AddPeriod(a1[0].colorRed, a1[0].colorGreen, a1[0].colorBlue, T1, a1[0].EndTime, false);
                //empty left
                if (a1[0].StartTime >= T1)
                    in_control.AddPeriod(a1[0].colorRed, a1[0].colorGreen, a1[0].colorBlue, a1[0].StartTime, a1[0].EndTime, false);
                for (int i = 1; i < a1.Length - 1; i++)
                {
                    in_control.AddPeriod(a1[i].colorRed, a1[i].colorGreen, a1[i].colorBlue, a1[i].StartTime, a1[i].EndTime, false);
                }
                bool temp_is_last = false;
                //for last time
                if (a1[a1.Length - 1].EndTime == DateTime.MaxValue) temp_is_last = true;

                if (temp_is_last && T2 < CURR && a1[a1.Length - 1].StartTime > T1)
                    in_control.AddPeriod(a1[a1.Length - 1].colorRed, a1[a1.Length - 1].colorGreen, a1[a1.Length - 1].colorBlue, a1[a1.Length - 1].StartTime, T2, temp_is_last);
                if (temp_is_last && T2 < CURR && a1[a1.Length - 1].StartTime <= T1)
                    in_control.AddPeriod(a1[a1.Length - 1].colorRed, a1[a1.Length - 1].colorGreen, a1[a1.Length - 1].colorBlue, T1, T2, temp_is_last);
                if (temp_is_last && T1 < CURR && CURR < T2 && a1[a1.Length - 1].StartTime < T1)
                    in_control.AddPeriod(a1[a1.Length - 1].colorRed, a1[a1.Length - 1].colorGreen, a1[a1.Length - 1].colorBlue, T1, CURR, temp_is_last);
                if (temp_is_last && T1 < CURR && CURR < T2 && a1[a1.Length - 1].StartTime >= T1)
                    in_control.AddPeriod(a1[a1.Length - 1].colorRed, a1[a1.Length - 1].colorGreen, a1[a1.Length - 1].colorBlue, a1[a1.Length - 1].StartTime, CURR, temp_is_last);
                //MessageBox.Show(temp_is_last.ToString());
                if (!temp_is_last && a1[a1.Length - 1].StartTime >= T1)
                    in_control.AddPeriod(a1[a1.Length - 1].colorRed, a1[a1.Length - 1].colorGreen, a1[a1.Length - 1].colorBlue, a1[a1.Length - 1].StartTime, T2, temp_is_last);
                if (!temp_is_last && a1[a1.Length - 1].StartTime < T1)
                    in_control.AddPeriod(a1[a1.Length - 1].colorRed, a1[a1.Length - 1].colorGreen, a1[a1.Length - 1].colorBlue, T1, T2, temp_is_last);
            }
            if (a1.Length == 0)
            {
                in_control.AddBasePeriod(T1, T2, true);
            }
            in_control.Refresh();
        }


        private void GraphicLinePresenter(GraphicLine.GraphicLine in_control, DateTime in_StartTime)
        {
            DateTime T1 = get_T1(in_StartTime);
            DateTime T2 = get_T2(in_StartTime);
            DateTime CURR = get_CURR();



            in_control.SetEmpty();
            in_control.AddBasePeriod(T1, T2);

            for (int i = 0; i < GLGlobalObject.GraphicLineDataArr.Length; i++)
            {
                if (GLGlobalObject.GraphicLineDataArr[i] != null && GLGlobalObject.GraphicLineDataArr[i].datetime >= T1 && GLGlobalObject.GraphicLineDataArr[i].datetime <= T2)
                {
                    graphicLine1.Data.Add(GLGlobalObject.GraphicLineDataArr[i]);
                }
            }

            //MessageBox.Show(graphicLine1.Data[graphicLine1.Data.Count-1].datetime.ToString());
            in_control.Refresh();
        }

        private void DataGridPresenter(DataGridView in_control, DateTime in_StartTime, List<DataGridRow> a1)
        {
            DateTime T1 = get_T1(in_StartTime);
            DateTime T2 = get_T2(in_StartTime);
            DateTime CURR = get_CURR();


            in_control.AllowUserToAddRows = false;
            in_control.Rows.Clear();
            for (int i = 0; i < a1.Count; i++)
            {
                in_control.Rows.Add();
                in_control.Rows[i].Height = 50;
                in_control.Rows[i].Cells[0].Value = a1[i].MachineState;
                in_control.Rows[i].Cells[1].Style.BackColor = a1[i].Color;

                String hours = (TimeSpan.FromSeconds(Convert.ToDouble(a1[i].SummaryTime)).Hours < 10)
                    ? "0" + TimeSpan.FromSeconds(Convert.ToDouble(a1[i].SummaryTime)).Hours.ToString() + "h "
                    : TimeSpan.FromSeconds(Convert.ToDouble(a1[i].SummaryTime)).Hours.ToString() + "h ";
                String minutes = (TimeSpan.FromSeconds(Convert.ToDouble(a1[i].SummaryTime)).Minutes < 10)
                    ? "0" + TimeSpan.FromSeconds(Convert.ToDouble(a1[i].SummaryTime)).Minutes.ToString() + "min "
                    : TimeSpan.FromSeconds(Convert.ToDouble(a1[i].SummaryTime)).Minutes.ToString() + "min ";
                String seconds = (TimeSpan.FromSeconds(Convert.ToDouble(a1[i].SummaryTime)).Seconds < 10)
                    ? "0" + TimeSpan.FromSeconds(Convert.ToDouble(a1[i].SummaryTime)).Seconds.ToString() + "sec"
                    : TimeSpan.FromSeconds(Convert.ToDouble(a1[i].SummaryTime)).Seconds.ToString() + "sec";
                in_control.Rows[i].Cells[2].Value = hours + minutes + seconds;
                in_control.Rows[i].Cells[3].Value = a1[i].Status;
                in_control.Rows[i].Cells[4].Value = a1[i].Count;
                in_control.Rows[i].Cells[5].Value = TimeSpan.FromSeconds(Convert.ToDouble(a1[i].ExceededTime)).Hours.ToString() +
                    "h " + TimeSpan.FromSeconds(Convert.ToDouble(a1[i].ExceededTime)).Minutes.ToString() +
                    "min " + TimeSpan.FromSeconds(Convert.ToDouble(a1[i].ExceededTime)).Seconds.ToString() + "sec ";
                if (a1[i].ExceededTime != "0")
                    in_control.Rows[i].Cells[5].Style.BackColor = Color.Red;
                else
                    in_control.Rows[i].Cells[5].Style.BackColor = Color.GreenYellow;
            }
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            GlobalPresenter();
        }

        private void GlobalPresenter()
        {
            GetGlobalData(dateTimePicker1.Value);
            #region Operator name
            label1.Text = sql_obj.GetOperatorName();
            #endregion
            #region production counters
            //GetProductionCounterFromOrder (fromSFI)
            metersFromOrder = sql_obj.GetProductionCounterFromOrder();
            //metersFromOrder = Convert.ToInt32(textBox1.Text);
            label4.Text = @"Current order: " + metersFromOrder.ToString();
            //Calculate Counter per shift
            metersPerShift += (metersFromOrder >= prev_metersFromOrder) ? metersFromOrder - prev_metersFromOrder :  metersFromOrder;
            prev_metersFromOrder = metersFromOrder;
            label20.Text = @"Per shift: " + metersPerShift.ToString();
            //zeroing Counter per shift
            if ((DateTime.Now.Hour == 8 || DateTime.Now.Hour == 20) && DateTime.Now.Minute == 0)
            {
                metersPerShift = 0;
            }
            #endregion
            #region call components presenters
            TimeLinePresenter(timeLine1, dateTimePicker1.Value, TLGlobalObject);
            GraphicLinePresenter(graphicLine1, dateTimePicker1.Value);
            DataGridPresenter(dataGridView1, dateTimePicker1.Value, DGGlobalObject);
            #endregion
            #region Eficiency in %
            GetEficiency();
            #endregion
            #region Current status
            label5.Text = sql_obj.GetCurrentStatusAsString();
            label5.BackColor = sql_obj.GetCurrentStatusColor();
            #endregion
            //MessageBox.Show(dateTimePicker1.Value.ToString());
        }

        private void GetGlobalData(DateTime in_StartTime)
        {
            DateTime T1 = get_T1(in_StartTime);
            DateTime T2 = get_T2(in_StartTime);
            DateTime CURR = get_CURR();

            //MessageBox.Show(in_StartTime.ToString());
            //MessageBox.Show(T1.ToString());
            #region TimeLine
            TLGlobalObject = sql_obj.GetTimeLineData(T1, T2, CURR);
            #endregion

            #region GraphicLine (empty)
            #endregion

            #region DataGridPresenter
            /*
            #region  ONLY FOR CALANDER!!!
            //if speed of line low than setpoint - add exedeed time to final resultcalculate calander 0 status
            cal_green_time = new TimeSpan(0, 0, 0);

            string temp = "";
            for (int i = 0; i < TLGlobalObject.Length; i++)
            {
                //MessageBox.Show(GLGlobalObject.GraphicLineDataArr.Length.ToString());
                if (TLGlobalObject[i].MachineState == 0
                    && TLGlobalObject[i].StartTime >= T1
                    && TLGlobalObject[i].EndTime <= T2)
                {
                    //MessageBox.Show(1.ToString());
                    cal_green_time += GLGlobalObject.GetGreenTimeAboveSpeed(TLGlobalObject[i].StartTime, TLGlobalObject[i].EndTime, graphicLine1.SetpointSpeed);
                    //temp += (TLGlobalObject[i].EndTime - TLGlobalObject[i].StartTime).ToString() + " " + GLGlobalObject.GetGreenTimeAboveSpeed(TLGlobalObject[i].StartTime, TLGlobalObject[i].EndTime, graphicLine1.SetpointSpeed).ToString() + "\n";
                }
                //MessageBox.Show(TLGlobalObject[i].MachineState.ToString());
                if (TLGlobalObject[i].MachineState == 0
                    && TLGlobalObject[i].StartTime >= T1
                    && TLGlobalObject[i].EndTime == DateTime.MaxValue)
                {
                    //MessageBox.Show(2.ToString());
                    cal_green_time += GLGlobalObject.GetGreenTimeAboveSpeed(TLGlobalObject[i].StartTime, get_CURR(), graphicLine1.SetpointSpeed);
                }
                if (TLGlobalObject[i].MachineState == 0
                    && TLGlobalObject[i].StartTime < T1
                    && TLGlobalObject[i].EndTime == DateTime.MaxValue)
                {
                    //MessageBox.Show(3.ToString());
                    cal_green_time += GLGlobalObject.GetGreenTimeAboveSpeed(T1, get_CURR(), graphicLine1.SetpointSpeed);
                }
            }
            //MessageBox.Show(temp);

            #endregion
            */
            DGGlobalObject = sql_obj.GetTableStatistic(T1, T2, CURR);
            #region  ONLY FOR CALANDER!!!
            //calulating final exeeded time for calander
            //for (int i = 0; i < DGGlobalObject.Count; i++)
            //{
            //    if (DGGlobalObject[i].MachineState == "0")
            //    {
            //        DGGlobalObject[i].ExceededTime = (TimeSpan.FromSeconds(Convert.ToDouble(DGGlobalObject[i].SummaryTime)) - cal_green_time).TotalSeconds.ToString();
            //    }
            //}
            #endregion
            #endregion
        }

        private void GetEficiency()
        {
            #region Only For Calander
            //get summary balasted time for calander
            TimeSpan SummaryExeeded0Statustime = new TimeSpan(0, 0, 0);
            for (int i = 0; i < DGGlobalObject.Count; i++)
            {
                if (DGGlobalObject[i].MachineState == "0")
                {
                    SummaryExeeded0Statustime = TimeSpan.FromSeconds(Convert.ToDouble(DGGlobalObject[i].SummaryTime)) - cal_green_time;
                }
            }
            #endregion
            //1.real time
            if (get_T1(dateTimePicker1.Value) <= get_CURR() && get_CURR() < get_T2(dateTimePicker1.Value))
            {
                double current_efficiency_value =
                                Math.Round(
                                                (1 - ((sql_obj.GetBalastedTimes(get_T1(dateTimePicker1.Value),
                                                                                get_T2(dateTimePicker1.Value),
                                                                                get_CURR())/* + SummaryExeeded0Statustime*/).TotalSeconds / (get_CURR() - get_T1(dateTimePicker1.Value)).TotalSeconds
                                                                                )
                                                ) * 100, 1
                                            );
                label8.Text = current_efficiency_value.ToString() + "%";
                //MessageBox.Show(previous_efficiency_value.ToString() + "        " + current_efficiency_value.ToString());
                if (previous_efficiency_value < current_efficiency_value)
                {
                    MarketStartMoving = true;
                    label8.BackColor = Color.FromArgb(255, 0, 255, 0);
                }
                if (previous_efficiency_value > current_efficiency_value)
                {
                    MarketStartMoving = true;
                    label8.BackColor = Color.FromArgb(255, 255, 0, 0);
                }
                previous_efficiency_value = current_efficiency_value;
            }
            //2.history
            if (get_T2(dateTimePicker1.Value) <= get_CURR())
                label8.Text = (
                                Math.Round(
                                            (1 - ((sql_obj.GetBalastedTimes(get_T1(dateTimePicker1.Value), get_T2(dateTimePicker1.Value), get_CURR())/* + SummaryExeeded0Statustime*/).TotalSeconds / 43200)) * 100, 2
                                        )
                            ).ToString() + "%";
        }

        private void showHistoryBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showHistoryBrowserToolStripMenuItem.Checked = !showHistoryBrowserToolStripMenuItem.Checked;

            Settings1.GENERALShowHistoryBrowser = showHistoryBrowserToolStripMenuItem.Checked;
            tableLayoutPanel2.RowStyles[2].Height = (Settings1.GENERALShowHistoryBrowser) ? 35 : 0;

            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            TextWriter writer = new StreamWriter("settings.xml");
            serializer.Serialize(writer, Settings1);
            writer.Dispose();

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm AboutForm1 = new AboutForm();
            AboutForm1.Show();
        }

        private void LabelsCenterPositioning(GroupBox in_GroupBox)
        {
            int i = 0;
            
            foreach (Control ctrlChild in in_GroupBox.Controls)
            {
                ctrlChild.Location = new Point(in_GroupBox.Size.Width / 2 - ctrlChild.Size.Width / 2, in_GroupBox.Size.Height / 2 - (ctrlChild.Size.Height * in_GroupBox.Controls.Count) / 2 + ctrlChild.Size.Height * i + 7);
                i++;
            }
        }

        private void Main_form_Paint(object sender, PaintEventArgs e)
        {
            LabelsCenterPositioning(groupBox1);
            LabelsCenterPositioning(groupBox2);
            LabelsCenterPositioning(groupBox3);
        }

        private void radioButton2_MouseClick(object sender, MouseEventArgs e)
        {
            dateTimePicker1.Value += new TimeSpan(12, 0, 0);
        }

        private void radioButton1_MouseClick(object sender, MouseEventArgs e)
        {
            dateTimePicker1.Value -= new TimeSpan(12, 0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshAsNowInterlock = false;
            if (get_CURR().Hour >= 8 && get_CURR().Hour < 20)
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;
            dateTimePicker1.Value = DateTime.Now;
            //GlobalPresenter();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RefreshAsNowInterlock = true;
            if (radioButton1.Checked)
                radioButton2.Checked = true;
            else
                radioButton1.Checked = true;
            dateTimePicker1.Value -= new TimeSpan(12, 0, 0);
            //toolStripStatusLabel4.Text = dateTimePicker1.Value.ToString();
            //GlobalPresenter();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefreshAsNowInterlock = true;
            if (radioButton1.Checked)
                radioButton2.Checked = true;
            else
                radioButton1.Checked = true;
            dateTimePicker1.Value += new TimeSpan(12, 0, 0);
            //toolStripStatusLabel4.Text = dateTimePicker1.Value.ToString();
            //GlobalPresenter();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MarketStartMoving = true;
            label8.BackColor = Color.FromArgb(255, 255, 0, 0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MarketStartMoving = true;
            label8.BackColor = Color.FromArgb(255, 0, 255, 0);
        }

        private void label8_Paint(object sender, PaintEventArgs e)
        {
            LabelsCenterPositioning(groupBox3);
        }

        private void label6_Paint(object sender, PaintEventArgs e)
        {
            LabelsCenterPositioning(groupBox2);
        }

        private void label4_Paint(object sender, PaintEventArgs e)
        {
            LabelsCenterPositioning(groupBox1);
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            ShiftStatistic ShiftStatisticForm = new ShiftStatistic(NowStatictic);
            ShiftStatisticForm.Show();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            label21.Text = NowStatictic.AdditionalJobs.ToString();

            PushStatisticData(NowStatictic);
        }

        private void PushStatisticData(ShiftStatisticClass a)
        {
            //2190 records - 3 years
            List<ShiftStatisticClass> ShiftData_list = new List<ShiftStatisticClass>();
            ShiftStatisticClass ShiftData_list_entry = new ShiftStatisticClass();

            try
            {
                using (StreamReader sr = new StreamReader(@"ShiftStatisticData.txt"))
                {
                    String entire_str, part_str;
                    int local_cnt = 0;  //pointer of data in string 
                    while (!sr.EndOfStream)
                    {
                        //
                        entire_str = sr.ReadLine();
                        part_str = "";
                        for (int i = 0; i < entire_str.Length; i++)
                        {
                            if (entire_str[i] == ';')
                            {
                                i++;
                                local_cnt++;

                                if (local_cnt == 1) { ShiftData_list_entry.ShiftStartDateTime = Convert.ToDateTime(part_str); }
                                if (local_cnt == 2) { ShiftData_list_entry.ShiftName = part_str; }
                                if (local_cnt == 3) { ShiftData_list_entry.Prodused = /*0*/ Convert.ToDouble(part_str); }
                                if (local_cnt == 4) { ShiftData_list_entry.Efficiency = /*0*/ Convert.ToDouble(part_str); }
                                if (local_cnt == 5) { ShiftData_list_entry.ScrapAmount = Convert.ToDouble(part_str); }
                                if (local_cnt == 6) { ShiftData_list_entry.AdditionalJobs = Convert.ToDouble(part_str); }
                                if (local_cnt == 7) { ShiftData_list_entry.A_Rolls_amount = Convert.ToDouble(part_str); }
                                if (local_cnt == 8) { ShiftData_list_entry.C_Rolls_amount = Convert.ToDouble(part_str); }
                                if (local_cnt == 9) { ShiftData_list_entry.PeopleAmount = Convert.ToInt32(part_str); }

                                part_str = "";
                            }
                            else
                            {
                                part_str += entire_str[i];
                            }
                        }
                        ShiftData_list.Add(ShiftData_list_entry);
                    }
                }
            }
            catch { }


            try
            {
                using (StreamWriter sw = new StreamWriter(@"ShiftStatisticData.txt", true))
                {
                    for (int i=0;i<ShiftData_list.Count;i++)
                    {
                        sw.WriteLine(ShiftData_list[i].ShiftStartDateTime + ";"
                            + ShiftData_list[i].ShiftName + ";"
                            + ShiftData_list[i].Prodused + ";"
                            + ShiftData_list[i].Efficiency + ";"
                            + ShiftData_list[i].ScrapAmount.ToString() + ";"
                            + ShiftData_list[i].AdditionalJobs.ToString() + ";"
                            + ShiftData_list[i].A_Rolls_amount.ToString() + ";"
                            + ShiftData_list[i].C_Rolls_amount.ToString() + ";"
                            + ShiftData_list[i].PeopleAmount.ToString() + ";"
                            );
                    }


                    #region Only For Calander
                    //get summary balasted time for calander
                    TimeSpan SummaryExeeded0Statustime = new TimeSpan(0, 0, 0);
                    for (int i = 0; i < DGGlobalObject.Count; i++)
                    {
                        if (DGGlobalObject[i].MachineState == "0")
                        {
                            SummaryExeeded0Statustime = TimeSpan.FromSeconds(Convert.ToDouble(DGGlobalObject[i].SummaryTime)) - cal_green_time;
                        }
                    }
                    #endregion

                    sw.WriteLine(get_CURR().ToString() + ";"
                            + a.ShiftName.ToString() + ";"
                            + metersPerShift.ToString() + ";"/*"0;"*/
                            + Math.Round(
                                                (1 - ((sql_obj.GetBalastedTimes(get_T1(dateTimePicker1.Value),
                                                                                get_T2(dateTimePicker1.Value),
                                                                                get_CURR()) + SummaryExeeded0Statustime).TotalSeconds / (get_CURR() - get_T1(dateTimePicker1.Value)).TotalSeconds
                                                                                )
                                                ) * 100, 1
                                            ).ToString() + ";"
                            + a.ScrapAmount.ToString() + ";"
                            + a.AdditionalJobs.ToString() + ";"
                            + a.A_Rolls_amount.ToString() + ";"
                            + a.C_Rolls_amount.ToString() + ";"
                            + a.PeopleAmount.ToString() + ";"
                            );
                }
            }
            catch
            {

            }
        }

        public void DrawTotalScore()
        {
            #region Draw Total Score
            ShiftStatisticClass temp = new ShiftStatisticClass();
            int total_score_shift1 = 0,
                total_score_shift2 = 0,
                total_score_shift3 = 0,
                total_score_shift4 = 0;

            temp = new ShiftStatisticClass();
            try
            {
                total_score_shift1 = temp.GetScoreShiftPerMonth("1");
                label16.Text = total_score_shift1.ToString();
            }
            catch
            {
                label16.Text = "0";
            }

            temp = new ShiftStatisticClass();
            try
            {
                total_score_shift2 = temp.GetScoreShiftPerMonth("2");
                label17.Text = total_score_shift2.ToString();
            }
            catch
            {
                label17.Text = "0";
            }

            temp = new ShiftStatisticClass();
            try
            {
                total_score_shift3 = temp.GetScoreShiftPerMonth("3");
                label18.Text = total_score_shift3.ToString();
            }
            catch
            {
                label18.Text = "0";
            }

            temp = new ShiftStatisticClass();
            try
            {
                total_score_shift4 = temp.GetScoreShiftPerMonth("4");
                label19.Text = total_score_shift4.ToString();
            }
            catch
            {
                label19.Text = "0";
            }

            int max_total_score = 0;
            max_total_score = (total_score_shift1 > max_total_score) ? total_score_shift1 : max_total_score;
            max_total_score = (total_score_shift2 > max_total_score) ? total_score_shift2 : max_total_score;
            max_total_score = (total_score_shift3 > max_total_score) ? total_score_shift3 : max_total_score;
            max_total_score = (total_score_shift4 > max_total_score) ? total_score_shift4 : max_total_score;

            int red_factor = 0;
            int green_factor = 0;
            int add = 0;

            add = (max_total_score != 0) ? (int)(((double)total_score_shift1 / (double)max_total_score) * 511) : 0;
            red_factor = 0;
            green_factor = 0;
            if (add>=0 && add <= 255) { red_factor = 255; green_factor = add; }
            if (add > 255 && add<=511) { red_factor = 255 - (add - 256); green_factor = 255; }
            panel1.BackColor = Color.FromArgb(red_factor, green_factor, 0);
            panel1.Width = (int)(((double)total_score_shift1 / (double)max_total_score) * 100);
            if (panel1.Width == 0) panel1.Width = 3;

            add = (max_total_score != 0) ? (int)(((double)total_score_shift2 / (double)max_total_score) * 511) : 0;
            red_factor = 0;
            green_factor = 0;
            if (add >= 0 && add <= 255) { red_factor = 255; green_factor = add; }
            if (add > 255 && add <= 511) { red_factor = 255 - (add - 256); green_factor = 255; }
            panel2.BackColor = Color.FromArgb(red_factor, green_factor, 0);
            panel2.Width = (int)(((double)total_score_shift2 / (double)max_total_score) * 100);
            if (panel2.Width == 0) panel2.Width = 3;


            add = (max_total_score != 0) ? (int)(((double)total_score_shift3 / (double)max_total_score) * 511) : 0;
            red_factor = 0;
            green_factor = 0;
            if (add >= 0 && add <= 255) { red_factor = 255; green_factor = add; }
            if (add > 255 && add <= 511) { red_factor = 255 - (add - 256); green_factor = 255; }
            panel3.BackColor = Color.FromArgb(red_factor, green_factor, 0);
            panel3.Width = (int)(((double)total_score_shift3 / (double)max_total_score) * 100);
            if (panel3.Width == 0) panel3.Width = 3;

            add = (max_total_score != 0) ? (int)(((double)total_score_shift4 / (double)max_total_score) * 511) : 0;
            red_factor = 0;
            green_factor = 0;
            if (add >= 0 && add <= 255) { red_factor = 255; green_factor = add; }
            if (add > 255 && add <= 511) { red_factor = 255 - (add - 256); green_factor = 255; }
            panel4.BackColor = Color.FromArgb(red_factor, green_factor, 0);
            panel4.Width = (int)(((double)total_score_shift4 / (double)max_total_score) * 100);
            if (panel4.Width == 0) panel4.Width = 3;

            #endregion
        }


        public void loadToDumpData()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"Dump.txt", false))
                {
                    sw.WriteLine(metersPerShift.ToString());
                }
            }
            catch {  }
        }

        public int LoadFromDumpData()
        {
            try
            {
                using (StreamReader sr = new StreamReader(@"Dump.txt"))
                {
                    return  Convert.ToInt32(sr.ReadLine());
                }
            }
            catch { }

            return 0;
        }
    }
}
