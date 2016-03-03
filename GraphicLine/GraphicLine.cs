using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;



namespace GraphicLine
{
    public partial class GraphicLine : UserControl
    {
        public GraphicLine()
        {
            InitializeComponent();

            this.History = new HistoryClass();
        }

        

        private void GraphicLine_Paint(object sender, PaintEventArgs e)
        {
            //global settings and vars
            Color color1 = new Color();
            Color color2 = new Color();
            Pen pen1;
            Pen pen2;
            Pen pen3;
            System.Drawing.Font font_004 = new System.Drawing.Font("Arial", 9);
            System.Drawing.Font font_005 = new System.Drawing.Font("Arial", 30);
            SolidBrush brush_004 = new SolidBrush(Color.Black);
            SolidBrush brush_005 = new SolidBrush(Color.YellowGreen);

            SolidBrush brush_006 = new SolidBrush(Color.Red);

            this.GraphicLineYTitlesWidth = 20;
            this.GraphicLineHeight = 60;
            this.GraphicLineX1 = this.LeftMargin;
            this.GraphicLineY1 = 0;
            this.GraphicLineX2 = this.Width - this.RightMargin;
            this.GraphicLineY2 = this.GraphicLineHeight;
            this.GraphicLineWidth = this.GraphicLineX2 - this.GraphicLineX1;



            //TODO: Too many magic numbers
            #region Draw polygon data
            if (this.Data.Count != 0)
            {
                List<Point> points_list = new List<Point>();
                List<Point> Gaps_list = new List<Point>();

                points_list.Add(new Point(this.GraphicLineX1 + this.GraphicLineYTitlesWidth, this.GraphicLineY2));
                int previous_distance = 0;
                for (int i = 0; i < this.Data.Count; i++)
                {
                    int curr_distance = (int)(this.Data[i].datetime - this.StartTime).TotalMinutes;
                    int temp = (int)((curr_distance * (this.GraphicLineWidth - this.GraphicLineYTitlesWidth)) / 720);
                    //there is no gaps
                    if ((curr_distance - previous_distance) == 1)
                        points_list.Add(new Point(GraphicLineX1 + this.GraphicLineYTitlesWidth + temp, this.GraphicLineY2 - this.Data[i].value));
                    //gaps
                    if ((curr_distance - previous_distance) > 1)
                    {
                        temp = (int)((previous_distance * (this.GraphicLineWidth - this.GraphicLineYTitlesWidth)) / 720);

                        points_list.Add(new Point(GraphicLineX1 + this.GraphicLineYTitlesWidth + temp, this.GraphicLineY2));
                        Gaps_list.Add(new Point(GraphicLineX1 + this.GraphicLineYTitlesWidth + temp, this.GraphicLineY1+3));

                        temp = (int)((curr_distance * (this.GraphicLineWidth - this.GraphicLineYTitlesWidth)) / 720);

                        points_list.Add(new Point(GraphicLineX1 + this.GraphicLineYTitlesWidth + temp, this.GraphicLineY2));
                        Gaps_list.Add(new Point(GraphicLineX1 + this.GraphicLineYTitlesWidth + temp, this.GraphicLineY1+3));

                        points_list.Add(new Point(GraphicLineX1 + this.GraphicLineYTitlesWidth + temp, this.GraphicLineY2 - this.Data[i].value));
                    }
                    previous_distance = curr_distance;
                   
                }
                points_list.Add(new Point(GraphicLineX1 + this.GraphicLineYTitlesWidth + (((int)(this.Data[this.Data.Count - 1].datetime - this.StartTime).TotalMinutes * (this.GraphicLineWidth - this.GraphicLineYTitlesWidth)) / 720), this.GraphicLineY2));
                Point[] points_arr = new Point[points_list.Count]; 
                for (int i=0;i<points_list.Count;i++)
                {
                    points_arr[i] = points_list[i];
                }
                e.Graphics.FillPolygon(brush_005, points_arr);

                for (int i=0;i<Gaps_list.Count;i+=2)
                {
                    color2 = Color.FromArgb(0, 0, 0);
                    pen3 = new Pen(color2);
                    pen3.Width = 6;
                    e.Graphics.DrawLine(pen3, Gaps_list[i], Gaps_list[i + 1]);
                }
            }
            #endregion

            //TODO: Too many magic numbers
            #region drawing grid

            color1 = Color.FromArgb(140, 140, 140);
            pen1 = new Pen(color1);
            pen1.Width = 1;
            //horisontal
            for (int i = GraphicLineY2; i > 0; i -= 10)
            {
                e.Graphics.DrawLine(pen1, GraphicLineX1 + this.GraphicLineYTitlesWidth, i, GraphicLineX2, i);
            }
            //vertical
            int total_hours = 12;
            for (int i = 1; i < total_hours; i++)
            {
                e.Graphics.DrawLine(pen1,
                    this.GraphicLineX1 + this.GraphicLineYTitlesWidth+ System.Convert.ToInt16(((i) * (this.GraphicLineWidth- this.GraphicLineYTitlesWidth)) / total_hours),
                    this.GraphicLineY1,
                    this.GraphicLineX1 + this.GraphicLineYTitlesWidth+ System.Convert.ToInt16(((i) * (this.GraphicLineWidth- this.GraphicLineYTitlesWidth)) / total_hours),
                    this.GraphicLineY2);
            }

            //for (int i = GraphicLineX1 + this.GraphicLineYTitlesWidth; i < GraphicLineX2; i += 80)
            //{
            //    e.Graphics.DrawLine(pen1, i, GraphicLineY1, i, GraphicLineY2);
            //}
            #endregion

            //TODO: Too many magic numbers
            #region Vertical Titles
            for (int i=10;i<70;i+=10)
                e.Graphics.DrawString(i.ToString(), font_004, brush_004,GraphicLineX1 , GraphicLineY2 - 2 -i);
            #endregion

            //TODO: Too many magic numbers
            #region Setpoint Line
            color1 = Color.Red;
            pen2 = new Pen(color1);
            pen2.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            pen2.DashPattern =new float[] {5,5};
            e.Graphics.DrawLine(pen2, GraphicLineX1 + this.GraphicLineYTitlesWidth, GraphicLineY2-35, this.GraphicLineX2, GraphicLineY2 - 35);
            #endregion


            

            //TODO: Too many magic numbers
            #region drawing black border rectangle
            color1 = Color.FromArgb(0, 0, 0);
            pen1 = new Pen(color1);
            pen1.Width = 1;
            e.Graphics.DrawRectangle(pen1, GraphicLineX1 + this.GraphicLineYTitlesWidth, 0, this.Width - (this.RightMargin + this.LeftMargin + this.GraphicLineYTitlesWidth), this.GraphicLineHeight);
            #endregion

        }

        private void GraphicLine_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }



        private void GraphicLine_Load(object sender, EventArgs e)
        {
           
        }


    }
}
