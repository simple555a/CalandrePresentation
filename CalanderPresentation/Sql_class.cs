﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Drawing;
using TimeLine;
using CalanderPresentation.TYPES;

namespace CalanderPresentation
{
    class Sql_class//: IDisposable
    {
        #region 1. Constructor

        public  Sql_class()
        {
            this.Initialized = false;
            InitializeSQL();
        }
        public  Sql_class(String in_ConnectionString)
        {
            //MessageBox.Show("windows");
            this.Initialized = false;
            try
            {
                SqlConnection con = new SqlConnection("Data Source=" + in_ConnectionString +  ";Initial Catalog=SFI_local_PC_SQL;Integrated Security=True");
                con.Open();
                //if ok - fill connection string field
                this.ConnectionString = "Data Source=" + in_ConnectionString +  ";Initial Catalog=SFI_local_PC_SQL;Integrated Security=True";
                this.Initialized = true;
            }
            catch
            {
                this.Initialized = false;
            }
        }
        public Sql_class(String in_ConnectionString,String in_login, String in_password)
        {

            //MessageBox.Show("Data Source=" + in_ConnectionString + ";User ID=" + in_login + ";Password=" + in_password);
            this.Initialized = false;
            //Data Source=SERZH\SQLEXPRESS;User ID=qwe;Password=***********
            try
            {
                SqlConnection con = new SqlConnection("Data Source=" + in_ConnectionString + ";User ID=" + in_login + ";Password="+in_password);
                con.Open();
                //if ok - fill connection string field
                this.ConnectionString = "Data Source=" + in_ConnectionString + ";User ID=" + in_login + ";Password=" + in_password;
                this.Initialized = true;
            }
            catch
            {
                this.Initialized = false;
            }
        }

        #endregion

        #region 2. Dispose
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        #endregion

        #region 2. Properties

        private String ConnectionString;
        public bool Initialized;

        #region public static String rules_limited_times
        public static String rules_limited_times = @"
INSERT INTO 
@rules_limited_times
VALUES      
(210,40);

";
        #endregion
        #region public static String rules_unlimited_times
        public static String rules_unlimited_times = @"
INSERT INTO 
@rules_unlimited_times
VALUES      
(0);

INSERT INTO 
@rules_unlimited_times
VALUES      
(111);

INSERT INTO 
@rules_unlimited_times
VALUES      
(413);

INSERT INTO 
@rules_unlimited_times
VALUES      
(611);

INSERT INTO 
@rules_unlimited_times
VALUES      
(612);

INSERT INTO 
@rules_unlimited_times
VALUES      
(641);

INSERT INTO 
@rules_unlimited_times
VALUES      
(824);

INSERT INTO 
@rules_unlimited_times
VALUES      
(825);

INSERT INTO 
@rules_unlimited_times
VALUES      
(836);

INSERT INTO 
@rules_unlimited_times
VALUES      
(851);


";
        #endregion

        #endregion

        #region 3. Metods 

        #region private void InitializeSQL()
        private void InitializeSQL()
        {
                try
                {
                    if (File.Exists("settings.xml"))
                    {
                        XmlSerializer XmlSerializer1 = new XmlSerializer(typeof(Settings));
                        TextReader reader1 = new StreamReader("settings.xml");
                        Settings Settings1 = (Settings)XmlSerializer1.Deserialize(reader1);
                        reader1.Dispose();

                        String con_string="";
                        if (Settings1.SQLInitialized == true)
                        {
                            if (Settings1.SQLWindowsAuthorization == true)
                            {
                                con_string = "Data Source=" + Settings1.SQLConnectionString + ";Initial Catalog=SFI_local_PC_SQL;Integrated Security=True";
                                SqlConnection con = new SqlConnection(con_string);
                                con.Open();
                            }
                            if (Settings1.SQLWindowsAuthorization == false)
                            {
                                con_string = "Data Source=" + Settings1.SQLConnectionString + ";User ID=" + Settings1.SQLLogin + ";Password=" + Settings1.SQLPassword;
                                SqlConnection con = new SqlConnection(con_string);
                                con.Open();
                            }

                            //if ok - fill connection string field
                            this.ConnectionString = con_string;

                            this.Initialized = true;
                        }
                        if (Settings1.SQLInitialized != true)
                        {
                            MessageBox.Show("SQL connection is not tested. See Settings - > Connection...");
                            this.Initialized = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("SQL settings is empty. See Settings - > Connection...");
                        this.Initialized = false;
                    }


                }
                catch
                {
                    MessageBox.Show("Bad SQL connection. Review connection settings");
                    this.Initialized = false;
                }
            }
        #endregion
        #region public String GetWCName()
        public String GetWCName()
            {
                if (!this.Initialized) return "*****";

                String SQLQuery = @"SELECT
		                                [WCName]
                                    FROM [SFI_local_PC_SQL].[dbo].[tbl_slc_Workcenter]";

                using (SqlConnection con = new SqlConnection(this.ConnectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(SQLQuery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())

                            while (reader.Read())
                            {
                                return reader.GetString(0);
                            }
                        return "Nothing";
                    }
                }
            }
        #endregion
        #region public String GetOperatorName()
        public String GetOperatorName()
        {
            if (!this.Initialized) return "***************";

            String SQLQuery = @"SELECT  
[SFI_local_PC_SQL].[dbo].[tbl_slc_Station].[SLCName]
,[SFI_local_PC_SQL].[dbo].[tbl_slc_WCUser].[CurrentUserName]
,[SLC_rsActive].[dbo].[APP_USER].[first_name]
,[SLC_rsActive].[dbo].[APP_USER].[last_name]
FROM [SFI_local_PC_SQL].[dbo].[tbl_slc_Station]
INNER JOIN 
[SFI_local_PC_SQL].[dbo].[tbl_slc_WCUser]
ON [SFI_local_PC_SQL].[dbo].[tbl_slc_WCUser].[ClientPC]=[SFI_local_PC_SQL].[dbo].[tbl_slc_Station].[SLCName]
INNER JOIN
[SLC_rsActive].[dbo].[APP_USER]
ON [SLC_rsActive].[dbo].[APP_USER].[user_name]=[SFI_local_PC_SQL].[dbo].[tbl_slc_WCUser].[CurrentUserName]";

            String SQLQuery_alt = @"SELECT  
[SFI_local_PC_SQL].[dbo].[tbl_slc_Station].[SLCName]
,[SFI_local_PC_SQL].[dbo].[tbl_slc_WCUser].[CurrentUserName]
,[SLC_rsActive_alt].[dbo].[APP_USER].[first_name]
,[SLC_rsActive_alt].[dbo].[APP_USER].[last_name]
FROM [SFI_local_PC_SQL].[dbo].[tbl_slc_Station]
INNER JOIN 
[SFI_local_PC_SQL].[dbo].[tbl_slc_WCUser]
ON [SFI_local_PC_SQL].[dbo].[tbl_slc_WCUser].[ClientPC]=[SFI_local_PC_SQL].[dbo].[tbl_slc_Station].[SLCName]
INNER JOIN
[SLC_rsActive_alt].[dbo].[APP_USER]
ON [SLC_rsActive_alt].[dbo].[APP_USER].[user_name]=[SFI_local_PC_SQL].[dbo].[tbl_slc_WCUser].[CurrentUserName]";

                using (SqlConnection con = new SqlConnection(this.ConnectionString))
                {
                    con.Open();

                    bool _alt_active = false;
                    //try SQLQuery
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(SQLQuery, con))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())

                                while (reader.Read())
                                {
                                    return reader.GetString(2) + " " + reader.GetString(3);
                                }
                            return "Nobody";
                        }
                    }
                    catch 
                    {
                        _alt_active = true;
                    }

                    //try SQLQuery_alt
                    if (_alt_active)
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(SQLQuery_alt, con))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())

                                while (reader.Read())
                                {
                                    return reader.GetString(2) + " " + reader.GetString(3);
                                }
                            return "Nobody";
                        }
                    }
                    catch { }
                    
                }
                return "Nobody";
        }
        #endregion
        #region public Section[] GetTimeLineData(DateTime in_StartTime, DateTime in_EndTime, DateTime in_CURR)
        public Section[] GetTimeLineData(DateTime in_StartTime, DateTime in_EndTime, DateTime in_CURR)
        {
            Section[] NULL_return = new Section[0];
            if (!this.Initialized) return NULL_return;

            Section[] a1;

            String SQLQuery = @"SELECT DISTINCT
                                [MachineState]
                                ,COLORS.[ColorValue]
                                ,[StartTime]
                                ,[EndTime]
                                FROM [SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStateHistory]
                                INNER JOIN
                                [SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStates] AS COLORS
                                ON [MachineState]=COLORS.StatusCode
                                WHERE 
                                [StartTime]>=CONVERT(DATETIME,'" + in_StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "',120) AND [StartTime]<CONVERT(DATETIME,'" + in_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "',120)" +
                                "OR [EndTime]>=CONVERT(DATETIME,'" + in_StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "',120) AND [StartTime]<CONVERT(DATETIME,'" + in_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "',120)" +
                                "OR [StartTime]<CONVERT(DATETIME,'" + in_StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "',120) AND CONVERT(DATETIME,'" + in_StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "',120)<CONVERT(DATETIME,'" + in_CURR.ToString("yyyy-MM-dd HH:mm:ss") + "',120) AND [EndTime] IS NULL " +
                                "ORDER BY [StartTime] asc";


            String SQLQuery_getCount = @"SELECT DISTINCT
                                    [MachineState]
                                    ,COLORS.[ColorValue]
                                    ,[StartTime]
                                    FROM [SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStateHistory]
                                    INNER JOIN
                                    [SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStates] AS COLORS
                                    ON [MachineState]=COLORS.StatusCode
                                    WHERE 
                                    [StartTime] BETWEEN CONVERT(DATETIME,'" + in_StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "',120) AND CONVERT(DATETIME,'" + in_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "',120)" +
                                    "ORDER BY [StartTime] asc";

            //get count
            Int32 RecordsCount=0;
            using (SqlConnection con = new SqlConnection(this.ConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(SQLQuery, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RecordsCount++;
                        }
                    }
                }
            }
             
            using (SqlConnection con = new SqlConnection(this.ConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(SQLQuery, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        a1 = new Section[RecordsCount];
                        for (int i = 0; i < RecordsCount; i++)
                        {
                            reader.Read();
                            a1[i] = new Section();
                            a1[i].MachineState = reader.GetInt32(0);
                            a1[i].StartTime = reader.GetDateTime(2);

                            try
                            {
                                a1[i].EndTime = reader.GetDateTime(3);
                            }
                            catch
                            {
                                a1[i].EndTime = DateTime.MaxValue;
                            }
                            a1[i].colorBlue = Convert.ToByte(reader.GetInt64(1) >> 16);
                            a1[i].colorGreen = Convert.ToByte((reader.GetInt64(1) >> 8) & 255);
                            a1[i].colorRed = Convert.ToByte((reader.GetInt64(1)  & 255));
                            
                        }
                    }
                }
            }

            return a1;
        }
        #endregion
        #region public String GetCurrentStatusAsString()
        public String GetCurrentStatusAsString()
        {
            if (!this.Initialized) return "***************";

            string return_string="";

            String SQLQuery = @"SELECT DISTINCT
                                [StatusDescription],
                                [Language],
                                [EndTime] 
                                FROM [SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStateHistory]
                                INNER JOIN
                                [SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStates] AS COLORS
                                ON [MachineState]=COLORS.StatusCode
                                WHERE 
                                [EndTime] IS NULL 
                                AND ([Language]='ru-RU' OR [Language]='en-US')";

            using (SqlConnection con = new SqlConnection(this.ConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(SQLQuery, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                            reader.Read();
                            return_string += reader.GetString(0);
                            reader.Read();
                            return_string += " - "+reader.GetString(0);
                            

                    }
                }
            }
            

            return return_string;
        }
        #endregion
        #region public String GetCurrentStatusAsInt()
        public int GetCurrentStatusAsInt()
        {
            if (!this.Initialized) return -1;
            

            String SQLQuery = @"SELECT DISTINCT
                                [MachineState],
                                [StartTime],
                                [EndTime] 
                                FROM [SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStateHistory]
                                WHERE 
                                [EndTime] IS NULL";

            using (SqlConnection con = new SqlConnection(this.ConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(SQLQuery, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        return reader.GetInt32(0);


                    }
                }
            }
            
            return -1;
        }
        #endregion
        #region public Color GetCurrentStatusColor()
        public Color GetCurrentStatusColor()
        {
            if (!this.Initialized) return Color.White;

            Byte colorBlue,
                colorGreen,
                colorRed;

            String SQLQuery = @"SELECT DISTINCT
                                [ColorValue]
                                FROM [SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStateHistory]
                                INNER JOIN
                                [SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStates] AS COLORS
                                ON [MachineState]=COLORS.StatusCode
                                WHERE 
                                [EndTime] IS NULL 
                                AND ([Language]='ru-RU' OR [Language]='en-US')";

            using (SqlConnection con = new SqlConnection(this.ConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(SQLQuery, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        colorBlue = Convert.ToByte(reader.GetInt64(0) >> 16);
                        colorGreen = Convert.ToByte((reader.GetInt64(0) >> 8) & 255);
                        colorRed = Convert.ToByte((reader.GetInt64(0) & 255));


                    }
                }
            }


            return Color.FromArgb(colorRed,colorGreen,colorBlue);
        }
        #endregion
        #region public List<DataGridRow> GetTableStatistic(DateTime in_StartTime, DateTime in_EndTime, DateTime in_CURR)
        public List<DataGridRow> GetTableStatistic(DateTime in_StartTime, DateTime in_EndTime, DateTime in_CURR)
        {
                

            List<DataGridRow> return_value = new List<DataGridRow>();
            if (!this.Initialized) return return_value;

            String SQLQuery = @"DECLARE @gStartTime DATETIME;
DECLARE @gEndTime DATETIME;
DECLARE @gCURR DATETIME;

SET @gStartTime=CONVERT(DATETIME,'" + in_StartTime.ToString("yyyy-MM-dd HH:mm:ss") + @"',120)
SET @gEndTime=CONVERT(DATETIME,'" + in_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + @"',120)
SET @gCURR=CONVERT(DATETIME,'" + in_CURR.ToString("yyyy-MM-dd HH:mm:ss") + @"',120)

DECLARE @TB1 table(MachineState int, ColorValue int, StartTime datetime, EndTime Datetime);
INSERT INTO @TB1 
SELECT DISTINCT
[MachineState]
,COLORS.[ColorValue] 
,[StartTime]
,[EndTime]
FROM 
[SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStateHistory]
INNER JOIN
[SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStates] AS COLORS
ON 
[MachineState]=COLORS.StatusCode 
WHERE 
[StartTime]>=@gStartTime AND [StartTime]<@gEndTime
OR [EndTime]>=@gStartTime AND [StartTime]<@gEndTime
OR [StartTime]<@gStartTime AND @gStartTime<@gCURR AND ISNULL([EndTime],@gCURR)>@gStartTime
ORDER BY [StartTime] asc


DECLARE @TB2 table(MachineState int, DateDifference int);
INSERT INTO @TB2  
SELECT 
[MachineState]
,SUM(DATEDIFF(
            SECOND
            ,CAST(CASE
                        WHEN [StartTime]<@gStartTime
                        THEN @gStartTime
                        ELSE [StartTime]
                    END as DATETIME)
            ,CAST(CASE
                        WHEN ISNULL([EndTime],@gCURR)>@gEndTime
                        THEN @gEndTime
                        ELSE ISNULL([EndTime],@gCURR)
                    END as DATETIME)
            )) AS DateDifference 
FROM @TB1
GROUP BY  [MachineState]


DECLARE @TB3 table(MachineState int, StartTime datetime);
INSERT INTO @TB3
SELECT 
[MachineState]
,[StartTime]
FROM @TB1
 

DECLARE @TB4 table(MachineState int, StartTime datetime);
INSERT INTO @TB4
SELECT 
[MachineState]
,min([StartTime]) 
FROM @TB3
GROUP BY [MachineState]

DECLARE @rules_limited_times table (MachineState int, ApprovedTime int)"+rules_limited_times+
@"
DECLARE @rules_unlimited_times table (MachineState int)" + rules_unlimited_times +
@"

DECLARE @correct_tb1 table (MachineState int, StartTime datetime, EndTime Datetime)
INSERT INTO @correct_tb1
SELECT 
[@TB1].[MachineState]
,CAST(
	CASE
		WHEN [StartTime]<@gStartTime 
		THEN @gStartTime 
		ELSE [StartTime]
	END AS DATETIME
	)
,CAST(CASE
                        WHEN ISNULL([EndTime],@gCURR)>@gEndTime
                        THEN @gEndTime
                        ELSE ISNULL([EndTime],@gCURR)
                    END as DATETIME
        )											
FROM @TB1
									    
DECLARE @excessed_times table (MachineState int, ExceededTimeSumValue int)
INSERT INTO @excessed_times  
select 
[@correct_tb1].[MachineState]
,SUM(CAST(
	CASE
--if exist in LIMITED times	
		WHEN  ([@rules_limited_times].[ApprovedTime] IS NOT NULL) 
				AND ([@rules_unlimited_times].[MachineState] IS NULL)
				AND (DATEDIFF(SECOND,[StartTime],[EndTime])-[@rules_limited_times].[ApprovedTime]*60)>=0
		THEN (DATEDIFF(SECOND,[StartTime],[EndTime])-[@rules_limited_times].[ApprovedTime]*60)
		
		WHEN  ([@rules_limited_times].[ApprovedTime] IS NOT NULL) 
				AND ([@rules_unlimited_times].[MachineState] IS NULL)
				AND (DATEDIFF(SECOND,[StartTime],[EndTime])-[@rules_limited_times].[ApprovedTime]*60)<0
		THEN 0

--if exist in UNLIMITED times		
		WHEN  ([@rules_limited_times].[ApprovedTime] IS NULL) 
				AND ([@rules_unlimited_times].[MachineState] IS NOT NULL)
		THEN 0
		
--if doesnt exist in LIMITED and UNLIMITED
		WHEN  ([@rules_limited_times].[ApprovedTime] IS NULL) 
				AND ([@rules_unlimited_times].[MachineState] IS NULL)
		THEN (DATEDIFF(SECOND,[StartTime],[EndTime]))
		
	END AS INT))
from @correct_tb1
LEFT JOIN 
@rules_limited_times
ON [@correct_tb1].[MachineState]=[@rules_limited_times].[MachineState]
LEFT JOIN 
@rules_unlimited_times
ON [@correct_tb1].[MachineState]=[@rules_unlimited_times].[MachineState]
group by  [@correct_tb1].[MachineState]

DECLARE @status_count table(MachineState int, _count int)
INSERT INTO @status_count            
select 
[MachineState]
,COUNT([MachineState])
from @TB1
GROUP BY [MachineState]

select distinct
[@TB4].MachineState
,[SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStates].ColorValue
,[SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStates].StatusDescription
,[@TB2].DateDifference
,[StartTime]
,[@status_count].[_count]
,[@excessed_times].[ExceededTimeSumValue]
FROM @TB4
INNER JOIN
@TB2
ON [@TB4].[MachineState]=[@TB2].MachineState
INNER JOIN
[SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStates]
ON [@TB4].[MachineState]=[SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStates].StatusCode
INNER JOIN
@excessed_times
ON [@TB4].[MachineState]=[@excessed_times].[MachineState]
INNER JOIN
@status_count
ON [@TB4].[MachineState]=[@status_count].[MachineState]
WHERE [SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStates].[Language]='en-US'
ORDER BY [StartTime]";

            using (SqlConnection con = new SqlConnection(this.ConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(SQLQuery, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            using (DataGridRow a1 = new DataGridRow())
                            {
                                a1.MachineCode = reader.GetInt32(0).ToString();
                                a1.Color = Color.FromArgb(Convert.ToByte((reader.GetInt64(1) & 255)), Convert.ToByte((reader.GetInt64(1) >> 8) & 255), Convert.ToByte(reader.GetInt64(1) >> 16));
                                a1.Status = reader.GetString(2);
                                a1.SummaryTime = reader.GetInt32(3).ToString();
                                a1.Count = reader.GetInt32(5).ToString();
                                a1.ExceededTime = reader.GetInt32(6).ToString();
                                return_value.Add(a1);
                            }
                        }
                    }
                }
            }

            return return_value;
        }
        #endregion
        #region public Int32 GetProductionCounter()
        public Int32 GetProductionCounter()
        {
            if (!this.Initialized) return 0;

            String SQLQuery = @"DECLARE @ShiftID nvarchar(2),
@StartShiftDate datetime,
@Hour int;

--SET @StartShiftDate = CAST(FLOOR(CAST(CAST('2015-04-23T00:00:00.000' AS DATETIME) AS FLOAT)) AS DATETIME)
SET @StartShiftDate = GETDATE();
SET @Hour = DATEPART(hour, @StartShiftDate)
SET @StartShiftDate = CAST(FLOOR(CAST(GETDATE() AS FLOAT)) AS DATETIME)


--day shift
IF (@Hour >= 8) and (@Hour < 20)
BEGIN
    SET @ShiftID = '01'
END

--night shift. first part
IF (@Hour < 8)
BEGIN
	SET @ShiftID = '02'
	SET @StartShiftDate = DATEADD(day,-1,@StartShiftDate);
END

--night shift. last part
IF (@Hour >= 20)
BEGIN
	SET @ShiftID = '02'
END


SELECT 
ISNULL(SUM (CountsGood),0) as CommCounter
FROM [SFI_local_PC_SQL].[dbo].[tbl_slc_Counter]
WHERE [ShiftDate] = @StartShiftDate AND [ShiftID] = @ShiftID AND [ClientPC] is not NULL";

            using (SqlConnection con = new SqlConnection(this.ConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(SQLQuery, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())

                        while (reader.Read())
                        {
                            return Convert.ToInt32(reader.GetDecimal(0));
                        }
                    return 0;
                }
            }
        }
            
        #endregion
        #region public TimeSpan GetBalastedTimes(DateTime in_StartTime, DateTime in_EndTime, DateTime in_CURR)
        public TimeSpan GetBalastedTimes(DateTime in_StartTime, DateTime in_EndTime, DateTime in_CURR)
        {
            TimeSpan return_value = new TimeSpan();
            if (!this.Initialized) return return_value;

            String SQLQuery = @"DECLARE @gStartTime DATETIME;
DECLARE @gEndTime DATETIME;
DECLARE @gCURR DATETIME;

SET @gStartTime=CONVERT(DATETIME,'" + in_StartTime.ToString("yyyy-MM-dd HH:mm:ss") + @"',120)
SET @gEndTime=CONVERT(DATETIME,'" + in_EndTime.ToString("yyyy-MM-dd HH:mm:ss") + @"',120)
SET @gCURR=CONVERT(DATETIME,'" + in_CURR.ToString("yyyy-MM-dd HH:mm:ss") + @"',120)

DECLARE @TB1 table(MachineState int, ColorValue int, StartTime datetime, EndTime Datetime);
INSERT INTO @TB1 
SELECT DISTINCT
[MachineState]
,COLORS.[ColorValue] 
,[StartTime]
,[EndTime]
FROM 
[SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStateHistory]
INNER JOIN
[SFI_local_PC_SQL].[dbo].[tbl_slc_MachineStates] AS COLORS
ON 
[MachineState]=COLORS.StatusCode 
WHERE 
[StartTime]>=@gStartTime AND [StartTime]<@gEndTime
OR [EndTime]>=@gStartTime AND [StartTime]<@gEndTime
OR [StartTime]<@gStartTime AND @gStartTime<@gCURR AND ISNULL([EndTime],@gCURR)>@gStartTime
ORDER BY [StartTime] asc


DECLARE @TB2 table(MachineState int, DateDifference int);
INSERT INTO @TB2  
SELECT 
[MachineState]
,SUM(DATEDIFF(
            SECOND
            ,CAST(CASE
                        WHEN [StartTime]<@gStartTime
                        THEN @gStartTime
                        ELSE [StartTime]
                    END as DATETIME)
            ,CAST(CASE
                        WHEN ISNULL([EndTime],@gCURR)>@gEndTime
                        THEN @gEndTime
                        ELSE ISNULL([EndTime],@gCURR)
                    END as DATETIME)
            )) AS DateDifference 
FROM @TB1
GROUP BY  [MachineState]


DECLARE @TB3 table(MachineState int, StartTime datetime);
INSERT INTO @TB3
SELECT 
[MachineState]
,[StartTime]
FROM @TB1
 

DECLARE @TB4 table(MachineState int, StartTime datetime);
INSERT INTO @TB4
SELECT 
[MachineState]
,min([StartTime]) 
FROM @TB3
GROUP BY [MachineState]

DECLARE @rules_limited_times table (MachineState int, ApprovedTime int)"+rules_limited_times+
@"
DECLARE @rules_unlimited_times table (MachineState int)" + rules_unlimited_times +
@"

DECLARE @correct_tb1 table (MachineState int, StartTime datetime, EndTime Datetime)
INSERT INTO @correct_tb1
SELECT 
[@TB1].[MachineState]
,CAST(
	CASE
		WHEN [StartTime]<@gStartTime 
		THEN @gStartTime 
		ELSE [StartTime]
	END AS DATETIME
	)
,CAST(CASE
                        WHEN ISNULL([EndTime],@gCURR)>@gEndTime
                        THEN @gEndTime
                        ELSE ISNULL([EndTime],@gCURR)
                    END as DATETIME
        )											
FROM @TB1
									    
DECLARE @excessed_times table (MachineState int, ExceededTimeSumValue int)
INSERT INTO @excessed_times            
select 
[@correct_tb1].[MachineState]
,SUM(CAST(
	CASE
--if exist in LIMITED times	
		WHEN  ([@rules_limited_times].[ApprovedTime] IS NOT NULL) 
				AND ([@rules_unlimited_times].[MachineState] IS NULL)
				AND (DATEDIFF(SECOND,[StartTime],[EndTime])-[@rules_limited_times].[ApprovedTime]*60)>=0
		THEN (DATEDIFF(SECOND,[StartTime],[EndTime])-[@rules_limited_times].[ApprovedTime]*60)
		
		WHEN  ([@rules_limited_times].[ApprovedTime] IS NOT NULL) 
				AND ([@rules_unlimited_times].[MachineState] IS NULL)
				AND (DATEDIFF(SECOND,[StartTime],[EndTime])-[@rules_limited_times].[ApprovedTime]*60)<0
		THEN 0
--if exist in UNLIMITED times		
		WHEN  ([@rules_limited_times].[ApprovedTime] IS NULL) 
				AND ([@rules_unlimited_times].[MachineState] IS NOT NULL)
		THEN 0
		
--if doesnt exist in LIMITED and UNLIMITED
		WHEN  ([@rules_limited_times].[ApprovedTime] IS NULL) 
				AND ([@rules_unlimited_times].[MachineState] IS NULL)
		THEN (DATEDIFF(SECOND,[StartTime],[EndTime]))
		
	END AS INT))
from @correct_tb1
LEFT JOIN 
@rules_limited_times
ON [@correct_tb1].[MachineState]=[@rules_limited_times].[MachineState]
LEFT JOIN 
@rules_unlimited_times
ON [@correct_tb1].[MachineState]=[@rules_unlimited_times].[MachineState]
group by  [@correct_tb1].[MachineState]

DECLARE @status_count table(MachineState int, _count int)
INSERT INTO @status_count            
select 
[MachineState]
,COUNT([MachineState])
from @TB1
GROUP BY [MachineState]

select distinct
ISNULL(SUM([@excessed_times].[ExceededTimeSumValue]),0)
FROM @TB4
INNER JOIN
@excessed_times
ON [@TB4].[MachineState]=[@excessed_times].[MachineState]";

            using (SqlConnection con = new SqlConnection(this.ConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(SQLQuery, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return_value = TimeSpan.FromSeconds(reader.GetInt32(0));
                        }
                    }
                }
            }

            return return_value;
        }

        #endregion

        #endregion



    }   

}
