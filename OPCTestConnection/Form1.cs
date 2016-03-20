using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace OPCTestConnection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static Settings Settings1 = new Settings();

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("OPCTestConnectionSettings.xml"))
            {
                XmlSerializer XmlSerializer1 = new XmlSerializer(typeof(Settings));
                TextReader reader1 = new StreamReader("OPCTestConnectionSettings.xml");
                Settings1 = (Settings)XmlSerializer1.Deserialize(reader1);
                reader1.Dispose();

                textBox1.Text = Settings1.OPCConnectionString;
                textBox2.Text = Settings1.OPCIntVarName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Settings1.OPCConnectionString = textBox1.Text;
            Settings1.OPCIntVarName = textBox2.Text;
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            TextWriter writer = new StreamWriter("OPCTestConnectionSettings.xml");
            serializer.Serialize(writer, Settings1);
            writer.Dispose();
        }

        private Opc.URL url;
        private Opc.Da.Server server;
        private OpcCom.Factory fact = new OpcCom.Factory();
        private Opc.Da.Subscription groupRead;
        private Opc.Da.SubscriptionState groupState;
        private Opc.Da.Item[] items; 

        private void button1_Click(object sender, EventArgs e)
        {
            // 1st: Create a server object and connect to the RSLinx OPC Server
            url = new Opc.URL(Settings1.OPCConnectionString);
            server = new Opc.Da.Server(fact, null);
            listBox1.Items.Add("1st: Create a server object and connect to the RSLinx OPC Server");
            //2nd: Connect to the created server
            server.Connect(url, new Opc.ConnectData(new System.Net.NetworkCredential()));
            listBox1.Items.Add("2nd: Connect to the created server");
            //3rd Create a group if items            
            groupState = new Opc.Da.SubscriptionState();
            groupState.Name = "Group999";
            groupState.UpdateRate = 1000;// this isthe time between every reads from OPC server
            groupState.Active = true;//this must be true if you the group has to read value
            groupRead = (Opc.Da.Subscription)server.CreateSubscription(groupState);
            //groupRead.DataChanged += GroupRead_DataChanged; ;
            //listBox1.Items.Add("Add event");

            items = new Opc.Da.Item[1];
            items[0] = new Opc.Da.Item();
            items[0].ItemName = Settings1.OPCIntVarName;
            items = groupRead.AddItems(items);
            listBox1.Items.Add("Add Items");
            Opc.Da.ItemValueResult[] values = groupRead.Read(items);
            listBox1.Items.Add("Read(items)");
            listBox1.Items.Add("========================");
            listBox1.Items.Add(Convert.ToInt32(values[0].Value.ToString()));
            label3.Text = values[0].Value.ToString();

        }

        private void GroupRead_DataChanged(object subscriptionHandle, object requestHandle, Opc.Da.ItemValueResult[] values)
        {
            listBox1.Items.Add(Convert.ToInt32(values[0].Value.ToString()));
            label4.Text = values[0].Value.ToString();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 1st: Create a server object and connect to the RSLinx OPC Server
            url = new Opc.URL(Settings1.OPCConnectionString);
            server = new Opc.Da.Server(fact, null);
            listBox1.Items.Add("1st: Create a server object and connect to the RSLinx OPC Server");
            //2nd: Connect to the created server
            server.Connect(url, new Opc.ConnectData(new System.Net.NetworkCredential()));
            listBox1.Items.Add("2nd: Connect to the created server");
            //3rd Create a group if items            
            groupState = new Opc.Da.SubscriptionState();
            groupState.Name = "Group999";
            groupState.UpdateRate = 1000;// this isthe time between every reads from OPC server
            groupState.Active = true;//this must be true if you the group has to read value
            groupRead = (Opc.Da.Subscription)server.CreateSubscription(groupState);
            groupRead.DataChanged += GroupRead_DataChanged; ;
            //listBox1.Items.Add("Add event");

            items = new Opc.Da.Item[1];
            items[0] = new Opc.Da.Item();
            items[0].ItemName = Settings1.OPCIntVarName;
            items = groupRead.AddItems(items);
            listBox1.Items.Add("Add Items");
            Opc.Da.ItemValueResult[] values = groupRead.Read(items);
            listBox1.Items.Add("Read(items)");
            listBox1.Items.Add("========================");
            listBox1.Items.Add(Convert.ToInt32(values[0].Value.ToString()));
        }
    }
}
