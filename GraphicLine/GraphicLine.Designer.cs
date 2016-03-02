using System;
using System.Collections.Generic;

namespace GraphicLine
{
    partial class GraphicLine 
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // GraphicLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.MinimumSize = new System.Drawing.Size(250, 0);
            this.Name = "GraphicLine";
            this.Size = new System.Drawing.Size(501, 68);
            this.Load += new System.EventHandler(this.GraphicLine_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GraphicLine_Paint);
            this.Resize += new System.EventHandler(this.GraphicLine_Resize);
            this.ResumeLayout(false);

        }

        #endregion
        

        public void SetEmpty()
        {
            this.LeftMargin = 0;
            this.RightMargin = 0;
            this.GraphicLineHeight = 0;
            this.GraphicLineX1 = 0;
            this.GraphicLineY1 = 0;
            this.GraphicLineX2 = 0;
            this.GraphicLineY2 = 0;
            this.GraphicLineWidth = 0;
            
            this.StartTime = DateTime.MinValue;
            this.EndTime = DateTime.MaxValue;

            this.Data.Clear();
            
        }

        /// <summary>
        /// Add point to Data
        /// </summary>
        /// <param name="in_value">value - any values of int. Component will fit it</param>
        public void AddPoint(int in_value, DateTime datetime)
        {
            GLPoint a1 = new GLPoint(in_value, datetime);
            Data.Add(a1);
        }


        public void AddBasePeriod(DateTime start_time, DateTime end_time)
        {
            this.StartTime = start_time;
            this.EndTime = end_time;
        }
        

        public int LeftMargin;
        public int RightMargin;
        public List<GLPoint> Data = new List<GLPoint> { };


        /// <summary>
        /// 0 - sec, 1 - minute, 2 - hour, 3 - day
        /// </summary>
        public int TimeDimension;
        public String XMLHistoryPath;

        private DateTime StartTime;
        private DateTime EndTime;
        private int GraphicLineHeight;
        private int GraphicLineX1;
        private int GraphicLineY1;
        private int GraphicLineX2;
        private int GraphicLineY2;
        private int GraphicLineWidth;
        private int GraphicLineYTitlesWidth;

        private HistoryClass History;


    }
}
