using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IPDTP.Client;

namespace CLIENTA
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
            if (REQ.Method == "TAKE")
            {
                if (REQ.DataType == ConnectionDataType.IMAGE)
                {
                    pictureBox1.Image = Sender.byteArrayToImage(Security.BFromBase64(REQ.Content, e.Packet.ContentEncoding));
                    Sender.SetSID(e.Packet.SessionID, SessionState.OK);
                }
                else
                {

                }
            }
            else if (REQ.Method == "ANSWER")
            {
                MessageBox.Show(REQ.Content, this.Text);
                Sender.SetSID(e.Packet.SessionID, SessionState.OK);
            }
            else if (REQ.Method == "RPROCESS")
            {
                MessageBox.Show("DATA PROCESSED : " + REQ.Content, this.Text);
                Sender.SetSID(e.Packet.SessionID, SessionState.OK);
            }
            else
            {

            }

         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Sender.SendREAD(textBox2.Text,textBox3.Text, ".");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Sender.SendGIVEImage("kk8dc2UJYJ4", textBox3.Text, ".");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppClient.StopServer();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Sender.SendProcess(textBox5.Text, textBox3.Text, ".");
        }
    }
}
