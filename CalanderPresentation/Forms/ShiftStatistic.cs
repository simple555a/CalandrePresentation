using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
//using CalanderPresentation.Classes;

namespace CalanderPresentation
{
    public partial class ShiftStatistic : Form
    {
        
        static ShiftStatisticClass ref_NowStatistic = new ShiftStatisticClass();

        public ShiftStatistic(ShiftStatisticClass a)
        {
            ref_NowStatistic = a;
            InitializeComponent();
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            bool all_ok_flag = true;

            if (!(checkBox1.Checked || checkBox2.Checked || checkBox3.Checked || checkBox4.Checked))
            {
                checkBox1.BackColor = Color.Red;
                checkBox2.BackColor = Color.Red;
                checkBox3.BackColor = Color.Red;
                checkBox4.BackColor = Color.Red;

                all_ok_flag = false;
            }

            //Scrap
            try
            {
                //ref_NowStatistic.ScrapAmount = Convert.ToDouble(textBox1.Text);
                textBox1.BackColor = System.Drawing.SystemColors.ControlLight; 
            }
            catch
            {
                textBox1.BackColor = Color.Red;

                all_ok_flag = false;
            }
            //Additional jobs
            try
            {
                //ref_NowStatistic.AdditionalJobs = Convert.ToDouble(textBox2.Text);
                textBox2.BackColor = System.Drawing.SystemColors.ControlLight;
            }
            catch
            {
                textBox2.BackColor = Color.Red;

                all_ok_flag = false;
            }
            //A Rolls
            try
            {
                //ref_NowStatistic.A_Rolls_amount = Convert.ToDouble(textBox3.Text);
                textBox3.BackColor = System.Drawing.SystemColors.ControlLight;
            }
            catch
            {
                textBox3.BackColor = Color.Red;

                all_ok_flag = false;
            }
            //C Rolls
            try
            {
                //ref_NowStatistic.C_Rolls_amount = Convert.ToDouble(textBox4.Text);
                textBox4.BackColor = System.Drawing.SystemColors.ControlLight;
            }
            catch
            {
                textBox4.BackColor = Color.Red;

                all_ok_flag = false;
            }
            //People
            try
            {
                //ref_NowStatistic.PeopleAmount = Convert.ToInt32(textBox5.Text);
                textBox5.BackColor = System.Drawing.SystemColors.ControlLight;
            }
            catch
            {
                textBox5.BackColor = Color.Red;

                all_ok_flag = false;
            }

            if (all_ok_flag)
            {
                if (this.checkBox1.Checked)
                {
                    ref_NowStatistic.ShiftName = 1.ToString();
                }
                if (this.checkBox2.Checked)
                {
                    ref_NowStatistic.ShiftName = 2.ToString();
                }
                if (this.checkBox3.Checked)
                {
                    ref_NowStatistic.ShiftName = 3.ToString();
                }
                if (this.checkBox4.Checked)
                {
                    ref_NowStatistic.ShiftName = 4.ToString();
                }
                ref_NowStatistic.ScrapAmount            = Convert.ToDouble(textBox1.Text);
                ref_NowStatistic.AdditionalJobs         = Convert.ToDouble(textBox2.Text);
                ref_NowStatistic.A_Rolls_amount         = Convert.ToDouble(textBox3.Text);
                ref_NowStatistic.C_Rolls_amount         = Convert.ToDouble(textBox4.Text);
                ref_NowStatistic.PeopleAmount           = Convert.ToInt32(textBox5.Text);
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

        private void ShiftStatistic_Load(object sender, EventArgs e)
        {

        }

        private ShiftStatisticClass GetLastShiftData(int shift_number)
        {
            ShiftStatisticClass ret_value = new ShiftStatisticClass();

            StreamReader sr = new StreamReader(@"ShiftStatisticData.txt");
            //sr.

            return ret_value;
        }
    }
}
