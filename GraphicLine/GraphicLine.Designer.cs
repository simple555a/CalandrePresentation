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
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // GraphicLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Maroon;
            this.MinimumSize = new System.Drawing.Size(250, 0);
            this.Name = "GraphicLine";
            this.Size = new System.Drawing.Size(327, 42);
            this.Load += new System.EventHandler(this.GraphicLine_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GraphicLine_Paint);
            this.Resize += new System.EventHandler(this.GraphicLine_Resize);
            this.ResumeLayout(false);

        }

        #endregion


        public bool AddPeriod(byte in_colorRed, byte in_colorGreen, byte in_colorBlue, DateTime in_PeriodStartTime, DateTime in_PeriodEndTime, bool is_last)
        {
            Section temp = new Section(in_colorRed, in_colorGreen, in_colorBlue, in_PeriodStartTime, in_PeriodEndTime, is_last);
            this.Data.Add(temp);

            this.SetEmpty_property = false;
            return true;
        }

        public bool AddBasePeriod(DateTime in_PeriodStartTime, DateTime in_PeriodEndTime, bool forsed_SetEmptyProperty)
        {

            this.StartTime = in_PeriodStartTime;
            this.EndTime = in_PeriodEndTime;

            this.SetEmpty_property = forsed_SetEmptyProperty;

            return true;
        }

        public void SetEmpty()
        {
            this.StartTime = DateTime.MinValue;
            this.EndTime = DateTime.MaxValue;
            this.TimeDimension = 0;
            this.LeftMargin = 0;
            this.RightMargin = 0;
            this.GraphicLineHeight = 0;
            this.GraphicLineX1 = 0;
            this.GraphicLineY1 = 0;
            this.GraphicLineX2 = 0;
            this.GraphicLineY2 = 0;
            this.GraphicLineWidth = 0;

            this.Data.Clear();

            this.SetEmpty_property = true;


        }

        public List<Section> Data = new List<Section> { };
        /// <summary>
        /// 0 - sec, 1 - minute, 2 - hour, 3 - day
        /// </summary>
        public int TimeDimension;
        public int MaxYAxisValue;



        private DateTime StartTime;
        private DateTime EndTime;
        private System.Windows.Forms.ToolTip toolTip1;
        private int LeftMargin;
        private int RightMargin;
        private int GraphicLineHeight;
        private int GraphicLineX1;
        private int GraphicLineY1;
        private int GraphicLineX2;
        private int GraphicLineY2;
        private int GraphicLineWidth;
        private bool SetEmpty_property;
        
        
    }
}
