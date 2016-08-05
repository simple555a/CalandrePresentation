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
            ShiftStatisticClass Shiftstatistic_1 = GetLastShiftData("1");
            ShiftStatisticClass Shiftstatistic_2 = GetLastShiftData("2");
            ShiftStatisticClass Shiftstatistic_3 = GetLastShiftData("3");
            ShiftStatisticClass Shiftstatistic_4 = GetLastShiftData("4");

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

            
        }

        private ShiftStatisticClass GetLastShiftData(String in_shift_name)
        {
            ShiftStatisticClass ret_value = new ShiftStatisticClass();

            List<ShiftStatisticClass> ShiftData_list = new List<ShiftStatisticClass>();
            ShiftStatisticClass ShiftData_list_entry;
            //MessageBox.Show("666");
            //fill ShiftData_list
            try
            {
                using (StreamReader sr = new StreamReader(@"ShiftStatisticData.txt"))
                {
                    //MessageBox.Show(in_shift_name);
                    String entire_str, part_str;
                    int local_cnt = 0;  //pointer of data in string 
                    while (!sr.EndOfStream)
                    {
                        entire_str = sr.ReadLine();
                        //sr.
                        //MessageBox.Show(entire_str);
                        part_str = "";
                        local_cnt = 0;
                        ShiftData_list_entry = new ShiftStatisticClass();
                        for (int i = 0; i < entire_str.Length; i++)
                        {
                            if (entire_str[i] != ';')
                            {
                                part_str += entire_str[i];
                            }

                            if (entire_str[i] == ';')
                            {

                                if (local_cnt == 0) { ShiftData_list_entry.ShiftStartDateTime = Convert.ToDateTime(part_str); }
                                if (local_cnt == 1) { ShiftData_list_entry.ShiftName = part_str; }
                                if (local_cnt == 2) { ShiftData_list_entry.Prodused = Convert.ToDouble(part_str); }
                                if (local_cnt == 3) { ShiftData_list_entry.Efficiency = Convert.ToDouble(part_str); }
                                if (local_cnt == 4) { ShiftData_list_entry.ScrapAmount = Convert.ToDouble(part_str); }
                                if (local_cnt == 5) { ShiftData_list_entry.AdditionalJobs = Convert.ToDouble(part_str); }
                                if (local_cnt == 6) { ShiftData_list_entry.A_Rolls_amount = Convert.ToDouble(part_str); }
                                if (local_cnt == 7) { ShiftData_list_entry.C_Rolls_amount = Convert.ToDouble(part_str); }
                                if (local_cnt == 8) { ShiftData_list_entry.PeopleAmount = Convert.ToInt32(part_str); }

                                local_cnt++;

                                part_str = "";
                            }

                        }

                        ShiftData_list.Add(ShiftData_list_entry);
                        //MessageBox.Show(ShiftData_list[ShiftData_list.Count - 1].ShiftName.ToString() + " " + ShiftData_list[0].ShiftName.ToString());
                    }

                }
            }
            catch { }
            
            //get requaried shift with earliest data
            DateTime temp_datetime = DateTime.MinValue;
            foreach (ShiftStatisticClass a in ShiftData_list)
            {
                //MessageBox.Show(a.ShiftName.ToString());
                if (a.ShiftName==in_shift_name && a.ShiftStartDateTime>Convert.ToDateTime(temp_datetime))
                {
                    temp_datetime = a.ShiftStartDateTime;
                    ret_value = a;
                }
            }


            return ret_value;
        }
    }
}
