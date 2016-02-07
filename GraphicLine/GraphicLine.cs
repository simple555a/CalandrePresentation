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
            System.Drawing.Font font_004 = new System.Drawing.Font("Arial", 9);
            System.Drawing.Font font_005 = new System.Drawing.Font("Arial", 30);
            SolidBrush brush_004 = new SolidBrush(Color.Black);
            String tempString = "";

            this.GraphicLineYTitlesWidth = 20;
            this.GraphicLineHeight = 60;
            this.GraphicLineX1 = this.LeftMargin;
            this.GraphicLineY1 = 0;
            this.GraphicLineX2 = this.Width - this.RightMargin;
            this.GraphicLineY2 = this.GraphicLineHeight;
            this.GraphicLineWidth = this.GraphicLineX2 - this.GraphicLineX1;



            //TODO: Too many magic numbers
            #region drawing black border rectangle
            color1 = Color.FromArgb(0, 0, 0);
            pen1 = new Pen(color1);
            pen1.Width = 1;
            e.Graphics.DrawRectangle(pen1, GraphicLineX1 + this.GraphicLineYTitlesWidth, 0, this.Width - (this.RightMargin + this.LeftMargin + this.GraphicLineYTitlesWidth), this.GraphicLineHeight);
            #endregion

            //TODO: Too many magic numbers
            #region drawing grid

            color1 = Color.FromArgb(0, 0, 0);
            pen1 = new Pen(color1);
            pen1.Width = 1;
            //horisontal
            for (int i = GraphicLineY2; i > 0; i -= 10)
            {
                e.Graphics.DrawLine(pen1, GraphicLineX1 + this.GraphicLineYTitlesWidth, i, GraphicLineX2, i);
            }
            //vertical
            for (int i = GraphicLineX1 + this.GraphicLineYTitlesWidth; i < GraphicLineX2; i += 80)
            {
                e.Graphics.DrawLine(pen1, i, GraphicLineY1, i, GraphicLineY2);
            }
            #endregion

            //TODO: Too many magic numbers
            #region Vertical Titles
            for (int i=10;i<60;i+=10)
                e.Graphics.DrawString(i.ToString(), font_004, brush_004,GraphicLineX1 , GraphicLineY2 - 2 -i);
            #endregion

            #region Setpoint Line
                e.Graphics.DrawLine(pen1, 0, GraphicLineY1, 100, GraphicLineY2);
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
