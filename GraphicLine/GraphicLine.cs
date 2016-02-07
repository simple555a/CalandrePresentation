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
        }

        

        private void GraphicLine_Paint(object sender, PaintEventArgs e)
        {
            //global settings and vars
            Color color1 = new Color();
            Pen pen1;
            Pen pen2;
            System.Drawing.Font font_004 = new System.Drawing.Font("Arial", 9);
            System.Drawing.Font font_005 = new System.Drawing.Font("Arial", 30);
            SolidBrush brush_004 = new SolidBrush(Color.Black);
            SolidBrush brush_005 = new SolidBrush(Color.YellowGreen);
            String tempString = "";

            this.GraphicLineYTitlesWidth = 20;
            this.GraphicLineHeight = 60;
            this.GraphicLineX1 = this.LeftMargin;
            this.GraphicLineY1 = 0;
            this.GraphicLineX2 = this.Width - this.RightMargin;
            this.GraphicLineY2 = this.GraphicLineHeight;
            this.GraphicLineWidth = this.GraphicLineX2 - this.GraphicLineX1;



            //TODO: Too many magic numbers
            #region Draw polygon
            Random rnd_v = new Random();
            Point[] points_arr = new Point[100];
            points_arr[0] = new Point(GraphicLineX1 + this.GraphicLineYTitlesWidth, this.GraphicLineY2);
            points_arr[1] = new Point(GraphicLineX1 + this.GraphicLineYTitlesWidth, this.GraphicLineY2 - rnd_v.Next(60));
            for (int i = 2; i < 98; i++)
            {
                points_arr[i] = new Point(GraphicLineX1 + this.GraphicLineYTitlesWidth + 10 * i - 10, this.GraphicLineY2 - rnd_v.Next(20, 60));
            }
            points_arr[98] = new Point(GraphicLineX1 + this.GraphicLineYTitlesWidth + 10 * 98 - 10, this.GraphicLineY2);
            points_arr[99] = new Point(GraphicLineX1 + this.GraphicLineYTitlesWidth, this.GraphicLineY2);
            e.Graphics.FillPolygon(brush_005, points_arr);
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
