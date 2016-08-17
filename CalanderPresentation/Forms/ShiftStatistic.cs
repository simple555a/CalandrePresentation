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
                ref_NowStatistic.ScrapAmount = Convert.ToDouble(textBox1.Text);
                ref_NowStatistic.AdditionalJobs = Convert.ToDouble(textBox2.Text);
                ref_NowStatistic.A_Rolls_amount = Convert.ToDouble(textBox3.Text);
                ref_NowStatistic.C_Rolls_amount = Convert.ToDouble(textBox4.Text);
                ref_NowStatistic.PeopleAmount = Convert.ToInt32(textBox5.Text);
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
            textBox1.Text = ref_NowStatistic.ScrapAmount.ToString();
            textBox2.Text = ref_NowStatistic.AdditionalJobs.ToString();
            textBox3.Text = ref_NowStatistic.A_Rolls_amount.ToString();
            textBox4.Text = ref_NowStatistic.C_Rolls_amount.ToString();
            textBox5.Text = ref_NowStatistic.PeopleAmount.ToString();


            ShiftStatisticClass Shiftstatistic_1 = new ShiftStatisticClass();
            ShiftStatisticClass Shiftstatistic_2 = new ShiftStatisticClass();
            ShiftStatisticClass Shiftstatistic_3 = new ShiftStatisticClass();
            ShiftStatisticClass Shiftstatistic_4 = new ShiftStatisticClass();

            Shiftstatistic_1.GetLastShiftData("1");
            Shiftstatistic_2.GetLastShiftData("2");
            Shiftstatistic_3.GetLastShiftData("3");
            Shiftstatistic_4.GetLastShiftData("4");

            label28.Text = Shiftstatistic_1.Prodused.ToString();
            label30.Text = Shiftstatistic_1.Efficiency.ToString();
            label32.Text = Shiftstatistic_1.ScrapAmount.ToString();
            label34.Text = Shiftstatistic_1.A_Rolls_amount.ToString();
            label36.Text = Shiftstatistic_1.C_Rolls_amount.ToString();
            label38.Text = Shiftstatistic_1.PeopleAmount.ToString();

            label40.Text = Shiftstatistic_2.Prodused.ToString();
            label42.Text = Shiftstatistic_2.Efficiency.ToString();
            label44.Text = Shiftstatistic_2.ScrapAmount.ToString();
            label46.Text = Shiftstatistic_2.A_Rolls_amount.ToString();
            label48.Text = Shiftstatistic_2.C_Rolls_amount.ToString();
            label50.Text = Shiftstatistic_2.PeopleAmount.ToString();

            label52.Text = Shiftstatistic_3.Prodused.ToString();
            label54.Text = Shiftstatistic_3.Efficiency.ToString();
            label56.Text = Shiftstatistic_3.ScrapAmount.ToString();
            label58.Text = Shiftstatistic_3.A_Rolls_amount.ToString();
            label60.Text = Shiftstatistic_3.C_Rolls_amount.ToString();
            label62.Text = Shiftstatistic_3.PeopleAmount.ToString();

            label64.Text = Shiftstatistic_4.Prodused.ToString();
            label66.Text = Shiftstatistic_4.Efficiency.ToString();
            label68.Text = Shiftstatistic_4.ScrapAmount.ToString();
            label70.Text = Shiftstatistic_4.A_Rolls_amount.ToString();
            label72.Text = Shiftstatistic_4.C_Rolls_amount.ToString();
            label74.Text = Shiftstatistic_4.PeopleAmount.ToString();

            ShiftStatisticClass ShiftstatisticPerMonth_1 = new ShiftStatisticClass();
            ShiftStatisticClass ShiftstatisticPerMonth_2 = new ShiftStatisticClass();
            ShiftStatisticClass ShiftstatisticPerMonth_3 = new ShiftStatisticClass();
            ShiftStatisticClass ShiftstatisticPerMonth_4 = new ShiftStatisticClass();

            ShiftstatisticPerMonth_1.GetLastShiftDataPerMonth("1");
            ShiftstatisticPerMonth_2.GetLastShiftDataPerMonth("2");
            ShiftstatisticPerMonth_3.GetLastShiftDataPerMonth("3");
            ShiftstatisticPerMonth_4.GetLastShiftDataPerMonth("4");

            label29.Text = ShiftstatisticPerMonth_1.Prodused.ToString();
            label31.Text = ShiftstatisticPerMonth_1.Efficiency.ToString();
            label33.Text = ShiftstatisticPerMonth_1.ScrapAmount.ToString();
            label35.Text = ShiftstatisticPerMonth_1.A_Rolls_amount.ToString();
            label37.Text = ShiftstatisticPerMonth_1.C_Rolls_amount.ToString();
            label39.Text = ShiftstatisticPerMonth_1.PeopleAmount.ToString();

            //MessageBox.Show()
            label41.Text = ShiftstatisticPerMonth_2.Prodused.ToString();
            label43.Text = ShiftstatisticPerMonth_2.Efficiency.ToString();
            label45.Text = ShiftstatisticPerMonth_2.ScrapAmount.ToString();
            label47.Text = ShiftstatisticPerMonth_2.A_Rolls_amount.ToString();
            label49.Text = ShiftstatisticPerMonth_2.C_Rolls_amount.ToString();
            label51.Text = ShiftstatisticPerMonth_2.PeopleAmount.ToString();

            label53.Text = ShiftstatisticPerMonth_3.Prodused.ToString();
            label55.Text = ShiftstatisticPerMonth_3.Efficiency.ToString();
            label57.Text = ShiftstatisticPerMonth_3.ScrapAmount.ToString();
            label59.Text = ShiftstatisticPerMonth_3.A_Rolls_amount.ToString();
            label61.Text = ShiftstatisticPerMonth_3.C_Rolls_amount.ToString();
            label63.Text = ShiftstatisticPerMonth_3.PeopleAmount.ToString();

            label65.Text = ShiftstatisticPerMonth_4.Prodused.ToString();
            label67.Text = ShiftstatisticPerMonth_4.Efficiency.ToString();
            label69.Text = ShiftstatisticPerMonth_4.ScrapAmount.ToString();
            label71.Text = ShiftstatisticPerMonth_4.A_Rolls_amount.ToString();
            label73.Text = ShiftstatisticPerMonth_4.C_Rolls_amount.ToString();
            label75.Text = ShiftstatisticPerMonth_4.PeopleAmount.ToString();


        }

        


        
    }
}
