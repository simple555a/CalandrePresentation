using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using CalanderPresentation.Classes;

namespace CalanderPresentation
{
    public partial class ShiftStatistic : Form
    {
        public ShiftStatistic()
        {
            InitializeComponent();
        }

        static ShiftStatisticClass NowStatictic = new ShiftStatisticClass();
        

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(checkBox1.Checked || checkBox2.Checked || checkBox3.Checked || checkBox4.Checked))
            {
                checkBox1.BackColor = Color.Red;
                checkBox2.BackColor = Color.Red;
                checkBox3.BackColor = Color.Red;
                checkBox4.BackColor = Color.Red;
            }

            //Scrap
            try
            {
                NowStatictic.ScrapAmount = Convert.ToDouble(textBox1.Text);
                textBox1.BackColor = System.Drawing.SystemColors.ControlLight; 
            }
            catch
            {
                textBox1.BackColor = Color.Red;
            }
            //Additional jobs
            try
            {
                NowStatictic.AdditionalJobs = Convert.ToDouble(textBox2.Text);
                textBox2.BackColor = System.Drawing.SystemColors.ControlLight;
            }
            catch
            {
                textBox2.BackColor = Color.Red;
            }
            //A Rolls
            try
            {
                NowStatictic.A_Rolls_amount = Convert.ToDouble(textBox3.Text);
                textBox3.BackColor = System.Drawing.SystemColors.ControlLight;
            }
            catch
            {
                textBox3.BackColor = Color.Red;
            }
            //C Rolls
            try
            {
                NowStatictic.C_Rolls_amount = Convert.ToDouble(textBox4.Text);
                textBox4.BackColor = System.Drawing.SystemColors.ControlLight;
            }
            catch
            {
                textBox4.BackColor = Color.Red;
            }
            //People
            try
            {
                NowStatictic.PeopleAmount = Convert.ToDouble(textBox5.Text);
                textBox5.BackColor = System.Drawing.SystemColors.ControlLight;
            }
            catch
            {
                textBox5.BackColor = Color.Red;
            }
        }

        private void SetDefaultValuesForNowStatistic(ref ShiftStatisticClass a)
        {
            a.ScrapAmount = 0;
            a.AdditionalJobs = 0;
            a.A_Rolls_amount = 0;
            a.A_Rolls_amount = 0;
            a.PeopleAmount = 0;
            a.Prodused = 0;
            a.ShiftName = "-1";
            
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox1.BackColor = Color.YellowGreen;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;

                //checkBox1.BackColor = System.Drawing.SystemColors.ControlLight;
                checkBox2.BackColor = System.Drawing.SystemColors.ControlLight;
                checkBox3.BackColor = System.Drawing.SystemColors.ControlLight;
                checkBox4.BackColor = System.Drawing.SystemColors.ControlLight;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
                checkBox2.BackColor = Color.YellowGreen;
                checkBox3.Checked = false;
                checkBox4.Checked = false;

                checkBox1.BackColor = System.Drawing.SystemColors.ControlLight;
                //checkBox2.BackColor = System.Drawing.SystemColors.ControlLight;
                checkBox3.BackColor = System.Drawing.SystemColors.ControlLight;
                checkBox4.BackColor = System.Drawing.SystemColors.ControlLight;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.BackColor = Color.YellowGreen;
                checkBox4.Checked = false;

                checkBox1.BackColor = System.Drawing.SystemColors.ControlLight;
                checkBox2.BackColor = System.Drawing.SystemColors.ControlLight;
                //checkBox3.BackColor = System.Drawing.SystemColors.ControlLight;
                checkBox4.BackColor = System.Drawing.SystemColors.ControlLight;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.BackColor = Color.YellowGreen;

                checkBox1.BackColor = System.Drawing.SystemColors.ControlLight;
                checkBox2.BackColor = System.Drawing.SystemColors.ControlLight;
                checkBox3.BackColor = System.Drawing.SystemColors.ControlLight;
                //checkBox4.BackColor = System.Drawing.SystemColors.ControlLight;
            }
        }
    }
}
