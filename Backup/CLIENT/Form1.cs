using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IPDTP.Client;

namespace CLIENT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AppClient.Initialize(Encoding.UTF8, EncryptionType.DPL128, ".", textBox1.Text);
            AppClient.PacketReceived += new PacketReceivedHandler(AppClient_PacketReceived);
            button1.Enabled = false;
        }
        
        void AppClient_PacketReceived(PacketReceivedArgs e)
        {
            Connection REQ = PacketBuilder.GetConnectionContentFormByte(e.Packet.Content, e.Packet.ContentEncoding);
            if (REQ.Method == "GIVE")
            {
                if (REQ.DataType == ConnectionDataType.IMAGE)
                {
                    if (REQ.Content == "kk8dc2UJYJ4")
                    {
                        Sender.SendTakeImage(CLIENT.Properties.Resources.kk8dc2UJYJ4, e.Packet.SourceAdress, ".", e.Packet.SessionID);
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }
            else if (REQ.Method == "READ")
            {
                MessageBox.Show(REQ.PROCESS + " SAID : " + REQ.Content, this.Text);
                Sender.SendAnswer( REQ.Content + " THANK YOU", e.Packet.SourceAdress, ".", e.Packet.SessionID);
            }
            else if (REQ.Method == "PROCESS")
            {
                string replace = REQ.Content.Replace("<shit>", " ");
                MessageBox.Show("Processed " + " : " + replace,this.Text);
                Sender.SendRProcess(replace, e.Packet.SourceAdress, ".", e.Packet.SessionID);
            }
            else
            {

            }
       
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppClient.StopServer();
        }


    }
}
