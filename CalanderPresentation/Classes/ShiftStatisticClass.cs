using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CalanderPresentation
{
    public class ShiftStatisticClass
    {
        public String ShiftName;
        public DateTime ShiftStartDateTime;
        public double Prodused;
        public double Efficiency;
        public double ScrapAmount;
        public double AdditionalJobs;
        public double A_Rolls_amount;
        public double C_Rolls_amount;
        public double PeopleAmount;

        public void GetLastShiftDataPerMonth(String in_shift_name)
        {
            

            List<ShiftStatisticClass> ShiftData_list = new List<ShiftStatisticClass>();
            ShiftStatisticClass ShiftData_list_entry;
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
                                local_cnt++;

                                if (local_cnt == 1) { ShiftData_list_entry.ShiftStartDateTime = Convert.ToDateTime(part_str); }
                                if (local_cnt == 2) { ShiftData_list_entry.ShiftName = part_str; }
                                if (local_cnt == 3) { ShiftData_list_entry.Prodused = Convert.ToDouble(part_str); }
                                if (local_cnt == 4) { ShiftData_list_entry.Efficiency = Convert.ToDouble(part_str); }
                                if (local_cnt == 5) { ShiftData_list_entry.ScrapAmount = Convert.ToDouble(part_str); }
                                if (local_cnt == 6) { ShiftData_list_entry.AdditionalJobs = Convert.ToDouble(part_str); }
                                if (local_cnt == 7) { ShiftData_list_entry.A_Rolls_amount = Convert.ToDouble(part_str); }
                                if (local_cnt == 8) { ShiftData_list_entry.C_Rolls_amount = Convert.ToDouble(part_str); }
                                if (local_cnt == 9) { ShiftData_list_entry.PeopleAmount = Convert.ToInt32(part_str); }


                                part_str = "";
                            }

                        }

                        ShiftData_list.Add(ShiftData_list_entry);
                        //MessageBox.Show(ShiftData_list[ShiftData_list.Count - 1].ShiftName.ToString() + " " + ShiftData_list[0].ShiftName.ToString());
                    }

                }
            }
            catch { }

            //get required shift with earliest data
            int ShiftFrequency = 0;
            foreach (ShiftStatisticClass a in ShiftData_list)
            {
                //MessageBox.Show(a.ShiftName.ToString());
                if (a.ShiftName == in_shift_name && (a.ShiftStartDateTime.Month == DateTime.Now.Month && a.ShiftStartDateTime.Year == DateTime.Now.Year))
                {
                    this.Prodused += a.Prodused;
                    this.Efficiency += a.Efficiency;
                    this.ScrapAmount += a.ScrapAmount;
                    this.AdditionalJobs += a.AdditionalJobs;
                    this.A_Rolls_amount += a.A_Rolls_amount;
                    this.C_Rolls_amount += a.C_Rolls_amount;
                    this.PeopleAmount += a.PeopleAmount;

                    ShiftFrequency++;
                }
            }

            if (ShiftFrequency != 0)
            {
                //MessageBox.Show(ShiftFrequency.ToString());
                //this.Prodused /= ShiftFrequency;
                this.Efficiency /= ShiftFrequency;
                //this.ScrapAmount /= ShiftFrequency;
                //this.AdditionalJobs /= ShiftFrequency;
                //this.A_Rolls_amount /= ShiftFrequency;
                //this.C_Rolls_amount /= ShiftFrequency;
                this.PeopleAmount /= ShiftFrequency;
            }
            if (ShiftFrequency == 0)
            {
                this.Prodused = 0;
                this.Efficiency = 0;
                this.ScrapAmount = 0;
                this.AdditionalJobs = 0;
                this.A_Rolls_amount = 0;
                this.C_Rolls_amount = 0;
                this.PeopleAmount = 0;
            }
            
        }


        public void GetLastShiftData(String in_shift_name)
        {

            List<ShiftStatisticClass> ShiftData_list = new List<ShiftStatisticClass>();
            ShiftStatisticClass ShiftData_list_entry;
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
                                local_cnt++;

                                if (local_cnt == 1) { ShiftData_list_entry.ShiftStartDateTime = Convert.ToDateTime(part_str); }
                                if (local_cnt == 2) { ShiftData_list_entry.ShiftName = part_str; }
                                if (local_cnt == 3) { ShiftData_list_entry.Prodused = Convert.ToDouble(part_str); }
                                if (local_cnt == 4) { ShiftData_list_entry.Efficiency = Convert.ToDouble(part_str); }
                                if (local_cnt == 5) { ShiftData_list_entry.ScrapAmount = Convert.ToDouble(part_str); }
                                if (local_cnt == 6) { ShiftData_list_entry.AdditionalJobs = Convert.ToDouble(part_str); }
                                if (local_cnt == 7) { ShiftData_list_entry.A_Rolls_amount = Convert.ToDouble(part_str); }
                                if (local_cnt == 8) { ShiftData_list_entry.C_Rolls_amount = Convert.ToDouble(part_str); }
                                if (local_cnt == 9) { ShiftData_list_entry.PeopleAmount = Convert.ToInt32(part_str); }


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
                if (a.ShiftName == in_shift_name && a.ShiftStartDateTime > Convert.ToDateTime(temp_datetime))
                {
                    temp_datetime = a.ShiftStartDateTime;

                    this.Prodused = a.Prodused;
                    this.Efficiency = a.Efficiency;
                    this.ScrapAmount = a.ScrapAmount;
                    this.AdditionalJobs = a.AdditionalJobs;
                    this.A_Rolls_amount = a.A_Rolls_amount;
                    this.C_Rolls_amount = a.C_Rolls_amount;
                    this.PeopleAmount = a.PeopleAmount;
                }
            }

            
        }

        public int GetTotalScore()
        {
            return Convert.ToInt32((this.Prodused + this.Efficiency * this.Prodused - this.ScrapAmount * 10 + this.A_Rolls_amount * 50 - this.C_Rolls_amount * 50) * (8 / this.PeopleAmount));
        }
    }
}