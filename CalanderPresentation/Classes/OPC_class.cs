using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace CalanderPresentation
{
    class OPC_class
    {
        #region Constructors
        public OPC_class()
        {
            this.previous_value_of_counter = 0;
            this.VariablesInitialized = false;
            InitializeOPC();
        }
        public OPC_class(String in_URL,  String in_CounterName, String in_SpeedName)
        {
            try
            {
                this.VariablesInitialized = false;

                // 1st: Create a server object and connect
                Opc.URL url = new Opc.URL(in_URL);
                Opc.Da.Server server = new Opc.Da.Server(fact, null);


                //2nd: Connect to the created server
                //server.Connect(url, new Opc.ConnectData(new System.Net.NetworkCredential()));
                try
                {
                    server.Connect(url, new Opc.ConnectData(new System.Net.NetworkCredential()));
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                    //return false;
                }

                //3rd Create a group if items            
                groupState = new Opc.Da.SubscriptionState();
                groupState.Name = "Group999";
                groupState.UpdateRate = 1000;// this isthe time between every reads from OPC server
                groupState.Active = true;//this must be true if you the group has to read value
                groupRead = (Opc.Da.Subscription)server.CreateSubscription(groupState);
                //groupRead.DataChanged += groupRead_DataChanged;

                items[0] = new Opc.Da.Item();
                items[0].ItemName = in_CounterName;
                items[1] = new Opc.Da.Item();
                items[1].ItemName = in_SpeedName;
                items = groupRead.AddItems(items);

                Opc.Da.ItemValueResult[] values = groupRead.Read(items);
                MessageBox.Show("Counter =  " + values[0].Value.ToString() + " Speed = " + values[1].Value.ToString());

                //if no exeption
                this.URL = in_URL;
                this.VariablesInitialized = true;
            }
            catch
            {
                this.VariablesInitialized = false;
            }
            
        }
        #endregion

        #region Properties
        public bool VariablesInitialized;
        public Int32 CurrentCounterOfMaterial;
        public Int32 CurrentSpeed;
        private String URL;
        private Label MaterialCounterLabel;
        private Label CurrentSpeedLabel;
        public bool lockCount;
        private int previous_value_of_counter = 0;

        #region Variables for OPC client
        private Opc.URL url;
        private Opc.Da.Server server;
        private OpcCom.Factory fact = new OpcCom.Factory();
        private Opc.Da.Subscription groupRead;
        private Opc.Da.SubscriptionState groupState;
        private Opc.Da.Item[] items = new Opc.Da.Item[2];
        #endregion

        #endregion

        #region Metods

        #region InitializeOPC()
        private void InitializeOPC()
        {
            try
            {
                if (File.Exists("settings.xml"))
                {
                    this.CurrentCounterOfMaterial = 0;

                    XmlSerializer XmlSerializer1 = new XmlSerializer(typeof(Settings));
                    TextReader reader1 = new StreamReader("settings.xml");
                    Settings Settings1 = (Settings)XmlSerializer1.Deserialize(reader1);
                    reader1.Dispose();

                    if (Settings1.OPCVariablesInitialized == true)
                    {
                        // 1st: Create a server object and connect to the RSLinx OPC Server
                        url = new Opc.URL(Settings1.OPCConnectionString);
                        server = new Opc.Da.Server(fact, null);
                        //2nd: Connect to the created server
                        server.Connect(url, new Opc.ConnectData(new System.Net.NetworkCredential()));
                        //3rd Create a group if items            
                        groupState = new Opc.Da.SubscriptionState();
                        groupState.Name = "Group999";
                        groupState.UpdateRate = 1000;// this isthe time between every reads from OPC server
                        groupState.Active = true;//this must be true if you the group has to read value
                        groupRead = (Opc.Da.Subscription)server.CreateSubscription(groupState);
                        //groupRead.DataChanged += groupRead_DataChanged;

                        items[0] = new Opc.Da.Item();
                        items[0].ItemName = Settings1.OPCCounterName;
                        items[1] = new Opc.Da.Item();
                        items[1].ItemName = Settings1.OPCSpeedName;
                        items = groupRead.AddItems(items);

                        Opc.Da.ItemValueResult[] values = groupRead.Read(items);

                        this.previous_value_of_counter = Convert.ToInt32(values[0].Value);

                        this.VariablesInitialized = true;
                    }
                }
                else
                {
                    MessageBox.Show("OPC settings is empty. See Settings - > Connection...");
                    this.VariablesInitialized = false;
                }
            }
            catch
            {
                MessageBox.Show("Bad OPC connection. Review connection string");
                this.VariablesInitialized = false;
            }
        }

        public void AskAllValues()
        {
            try
            {
                if (File.Exists("settings.xml"))
                {
                    Opc.Da.ItemValueResult[] values = groupRead.Read(items);
                    /*
                    if (this.lockCount != true)
                    {
                        //this.MaterialCounterLabel.Text = (this.CurrentCounterOfMaterial != Convert.ToInt32(values[0].Value)) ? (this.CurrentCounterOfMaterial + (Convert.ToInt32(values[0].Value) - this.CurrentCounterOfMaterial)).ToString() : this.CurrentCounterOfMaterial.ToString();
                        this.CurrentCounterOfMaterial += (Convert.ToInt32(values[0].Value) - this.CurrentCounterOfMaterial);
                        //this.CurrentCounterOfMaterial = Convert.ToInt32(values[0].Value);
                    }
                    */
                    this.CurrentSpeed = Convert.ToInt32(values[1].Value);
                }
                else
                {
                    MessageBox.Show("OPC settings is empty. See Settings - > Connection...");
                }
            }
            catch
            {
                MessageBox.Show("Bad OPC connection. Review connection string");
            }
        }


        //void groupRead_DataChanged(object subscriptionHandle, object requestHandle, Opc.Da.ItemValueResult[] values)
        //{
        //    if (this.lockCount != true)
        //    {
        //        this.MaterialCounterLabel.Text = (this.CurrentCounterOfMaterial != Convert.ToInt32(values[0].Value)) ? (this.CurrentCounterOfMaterial + (Convert.ToInt32(values[0].Value) - this.CurrentCounterOfMaterial)).ToString() : this.CurrentCounterOfMaterial.ToString();
        //        this.CurrentCounterOfMaterial += (Convert.ToInt32(values[0].Value)- this.CurrentCounterOfMaterial);
        //    }
        //this.CurrentSpeed = Convert.ToInt32(values[1].Value);
        //this.CurrentSpeedLabel.Text = this.CurrentSpeed.ToString();
        //}
        #endregion
        //#region void SetActiveLabel(Label in_control)
        //public void SetActiveLabel(Label in_MaterialCounterLabel, Label in_CurrentSpeedLabel)
        //{
        //    this.MaterialCounterLabel = in_MaterialCounterLabel;
        //    this.CurrentSpeedLabel= in_CurrentSpeedLabel;
        //}

        //#endregion

        #endregion
    }
}
