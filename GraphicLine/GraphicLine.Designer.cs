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
            this.SuspendLayout();
            // 
            // GraphicLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.MinimumSize = new System.Drawing.Size(250, 0);
            this.Name = "GraphicLine";
            this.Size = new System.Drawing.Size(347, 58);
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
            
        }

        
        public int LeftMargin;
        public int RightMargin;
        
        private int GraphicLineHeight;
        private int GraphicLineX1;
        private int GraphicLineY1;
        private int GraphicLineX2;
        private int GraphicLineY2;
        private int GraphicLineWidth;
        private int GraphicLineYTitlesWidth;


    }
}
