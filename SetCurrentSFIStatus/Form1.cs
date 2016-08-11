using System;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Data.SqlClient;


namespace SetCurrentSFIStatus
{
    public partial class Form1 : Form
    {

        static Settings Settings1 = new Settings();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String SQLQuery = @"SELECT [guid]
      ,[WCName]
      ,[MachineState]
      ,[StartTime]
      ,[EndTime]
      ,[LastModified]
  FROM [SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStateHistory]
WHERE [EndTime] IS NULL";
            
            using (SqlConnection con = new SqlConnection("Data Source=" + Settings1.SQLConnectionString + ";Initial Catalog=SFI_local_PC_SQL;Integrated Security=True"))
            {
                con.Open();

                using (SqlDataAdapter da = new SqlDataAdapter(SQLQuery, con))
                {
                    using (SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(da))
                    {
                        DataSet DataSet1 = new DataSet();
                        da.Fill(DataSet1, "tbl_slc_MachineStateHistory");
                        DataSet1.Tables["tbl_slc_MachineStateHistory"].Rows[0]["MachineState"] = 0;
                        da.Update(DataSet1, "tbl_slc_MachineStateHistory");
                    }
                        
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("SetCurrentSFIStatusSettings.xml"))
            {
                XmlSerializer XmlSerializer1 = new XmlSerializer(typeof(Settings));
                TextReader reader1 = new StreamReader("SetCurrentSFIStatusSettings.xml");
                Settings1 = (Settings)XmlSerializer1.Deserialize(reader1);
                reader1.Dispose();

                textBox2.Text = Settings1.SQLConnectionString;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Settings1.SQLConnectionString = textBox2.Text;
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            TextWriter writer = new StreamWriter("SetCurrentSFIStatusSettings.xml");
            serializer.Serialize(writer, Settings1);
            writer.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String SQLQuery = @"SELECT [guid]
      ,[WCName]
      ,[MachineState]
      ,[StartTime]
      ,[EndTime]
      ,[LastModified]
  FROM [SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStateHistory]
WHERE [EndTime] IS NULL";

            using (SqlConnection con = new SqlConnection("Data Source=" + Settings1.SQLConnectionString + ";Initial Catalog=SFI_local_PC_SQL;Integrated Security=True"))
            {
                con.Open();

                using (SqlDataAdapter da = new SqlDataAdapter(SQLQuery, con))
                {
                    using (SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(da))
                    {
                        DataSet DataSet1 = new DataSet();
                        da.Fill(DataSet1, "tbl_slc_MachineStateHistory");
                        DataSet1.Tables["tbl_slc_MachineStateHistory"].Rows[0]["MachineState"] = Convert.ToInt32(textBox1.Text);
                        da.Update(DataSet1, "tbl_slc_MachineStateHistory");
                    }

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                String SQLQuery = @"DECLARE @SFI_DB_Name nvarchar(64);
DECLARE @SQL_String nvarchar(256);
DECLARE @WCName nvarchar(64);

SELECT 
@WCName=[WCName]
FROM [SFI_local_PC_SQL].[dbo].[tbl_slc_Workcenter]


SELECT TOP 1 @SFI_DB_Name = name
FROM sys.databases 
WHERE name like 'SFI_logic%'

SET @SQL_String = '[' + @SFI_DB_Name + '].[dbo].[sp_slc_MachineStateChange]'

EXEC @SQL_String @WCName, '999', NULL, '1'";

                using (SqlConnection con = new SqlConnection("Data Source=" + Settings1.SQLConnectionString + ";Initial Catalog=SFI_local_PC_SQL;Integrated Security=True"))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = SQLQuery;
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                String SQLQuery = @"DECLARE @SFI_DB_Name nvarchar(64);
DECLARE @SQL_String nvarchar(256);
DECLARE @WCName nvarchar(64);

SELECT 
@WCName=[WCName]
FROM [SFI_local_PC_SQL].[dbo].[tbl_slc_Workcenter]


SELECT TOP 1 @SFI_DB_Name = name
FROM sys.databases 
WHERE name like 'SFI_logic%'

SET @SQL_String = '[' + @SFI_DB_Name + '].[dbo].[sp_slc_MachineStateChange]'

EXEC @SQL_String @WCName, '999', NULL, '1'";

                using (SqlConnection con = new SqlConnection("Data Source=" + Settings1.SQLConnectionString + ";Initial Catalog=SFI_local_PC_SQL;Integrated Security=True"))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = SQLQuery;
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {

            }
        }
    }
}
