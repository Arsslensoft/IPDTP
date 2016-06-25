using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IPDTPLib;
using System.Diagnostics;

namespace IPDTPApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {

                IPDTPApplication.Initialize();

            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    e.Cancel = true;
                    this.Hide();
                }
                else
                    IPDTPApplication.Stop();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {

            }
        }

        private void restartServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Application.ExecutablePath);
            Process.GetCurrentProcess().Kill();
        }

        private void stopServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void refreshListOfClientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (KeyValuePair<string, string> k in IPDTPApplication.SPNS)
            {
                listBox1.Items.Add("Server name : " + k.Value + " UPL : " + k.Key); 
            }
            this.Show();
        }
    }
}
