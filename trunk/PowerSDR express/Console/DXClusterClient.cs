//=================================================================
// DXClusterClient.cs
//=================================================================
// Copyright (C) 2011 YT7PWR
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//=================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace PowerSDR
{
    partial class DXClusterClient : Form
    {
        #region DLL imports

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr window, int message, int wparam, int lparam);

        #endregion

        #region variable

        const int WM_VSCROLL = 0x115;
        const int SB_BOTTOM = 7;
        TelnetClient client;
        public string CALL = "";
        public string NAME = "";
        public string QTH = "";
        ClusterSetup ClusterSetupForm;
        public bool closing = false;

        #endregion

        #region constructor

        public DXClusterClient(string call, string name, string qth)
        {
            InitializeComponent();
            client = new TelnetClient(this);
            CALL = call;
            NAME = name;
            QTH = qth;
            GetOptions();
            ClusterSetupForm = new ClusterSetup(this);

            if (comboDXCluster.Items.Count > 0)
                comboDXCluster.SelectedIndex = 0;
        }

        #endregion

        #region button events

        private void comboDXCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (client.ClusterClient != null && client.ClusterClient.Connected)
                client.SendMessage(1, "BYE");
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (client.ClusterClient == null)
            {
                client.Start(comboDXCluster.Text.ToString());
            }
            else if (client.ClusterClient != null && !client.ClusterClient.Connected)
                client.Start(comboDXCluster.Text.ToString());
        }

        private void btnBye_Click(object sender, EventArgs e)
        {
            if (client.ClusterClient != null && client.ClusterClient.Connected)
                client.SendMessage(1, "BYE");
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (ClusterSetupForm != null && !ClusterSetupForm.IsDisposed)
                ClusterSetupForm.Show();
            else
            {
                ClusterSetupForm = new ClusterSetup(this);
                ClusterSetupForm.Show();
            }
        }

        private void btnNoDX_Click(object sender, EventArgs e)
        {
            if (client.ClusterClient != null && client.ClusterClient.Connected)
                client.SendMessage(0, "SET/FILTER DX/REJECT");
        }

        private void btnShowDX_Click(object sender, EventArgs e)
        {
            if (client.ClusterClient != null && client.ClusterClient.Connected)
                client.SendMessage(0, "set/filter dx/off");
        }

        private void btnNoVHF_Click(object sender, EventArgs e)
        {
            if (client.ClusterClient != null && client.ClusterClient.Connected)
                client.SendMessage(0, "set/filter vhf/reject");
        }

        private void btnClearTxt_Click(object sender, EventArgs e)
        {
            rtbDXClusterText.Clear();
            rtbDXClusterText.Refresh();
        }

        #endregion

        #region CrossThread

        public void CrossThreadCommand(int command, byte[] data, int count)
        {
            try
            {
                switch (command)
                {
                    case 0:
                        {                                                   // regular text
                            ASCIIEncoding buffer = new ASCIIEncoding();
                            string text = buffer.GetString(data, 0, count);
                            text = text.Replace('\a', ' ');
                            /*string[] vals = text.Split(':');
                            text = vals[0] + "       ";
                            vals = vals[1].Split(' ');

                            foreach (string b in vals)
                            {
                                if(b != "")
                                {
                                    text += b + " ";
                                }
                            }*/

                            rtbDXClusterText.AppendText(text);
                            SendMessage(rtbDXClusterText.Handle, WM_VSCROLL, SB_BOTTOM, 0);

                            if (text.StartsWith("login:"))
                            {
                                client.SendMessage(0, CALL);
                                //Thread.Sleep(1000);

                                if (NAME != "")
                                    client.SendMessage(0, "set/name " + NAME);

                                if (QTH != "")
                                    client.SendMessage(0, "set/QTH " + QTH);
                            }
                        }
                        break;

                    case 1:
                        {                                                   // screen caption
                            ASCIIEncoding buffer = new ASCIIEncoding();
                            string text = buffer.GetString(data, 0, count);
                            this.Text = text;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        #endregion

        #region Save/Restore settings

        public void SaveOptions()
        {
            try
            {
                ArrayList a = new ArrayList();

                int items = comboDXCluster.Items.Count;

                for(int i=0; i<items; i++)
                {
                    comboDXCluster.SelectedIndex = i;
                    a.Add(comboDXCluster.Text + "/");
                }

                DB.SaveVars("DXClusterOptions", ref a);		    // save the values to the DB
                DB.Update();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in DXCluster SaveOptions function!\n" + ex.ToString());
            }
        }

        public void GetOptions()
        {
            try
            {
                // get list of live controls
                ArrayList temp = new ArrayList();		// list of all first level controls
                temp.Add(rtbDXClusterText);

                ArrayList combobox_list = new ArrayList();

                foreach (Control c in temp)
                {
                    if (c.GetType() == typeof(ComboBox))
                        combobox_list.Add(c);
                }

                ArrayList a = DB.GetVars("DXClusterOptions");
                a.Sort();

                if (a.Count > 0)
                    comboDXCluster.Items.Clear();

                foreach (string s in a)
                {
                    string b = s.Replace('/', ' ');
                    comboDXCluster.Items.Add(b);
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        public void UpdateHostsList(string[] vals)
        {
            try
            {
                comboDXCluster.Items.Clear();

                foreach (string b in vals)
                {
                    if (b != "")
                        comboDXCluster.Items.Add(b);
                }

                if (comboDXCluster.Items.Count > 0)
                    comboDXCluster.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        #endregion

        #region misc function

        private void rtbDXClusterText_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(e.LinkText);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        private void txtMessage_KeyUP(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (client.ClusterClient != null && client.ClusterClient.Connected)
                        client.SendMessage(0, txtMessage.Text.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        #endregion
    }

    #region Telnet Client

    class TelnetClient
    {
        #region Variable

        public TcpClient ClusterClient;
        Stream sr;
        Stream sw;
        delegate void CrossThreadCallback(int command, byte[] data, int count);
        DXClusterClient ClusterForm;
        string remote_addr = "";
        string remote_port = "";

        #endregion

        #region constructor/destructor

        public TelnetClient(DXClusterClient form)
        {
            try
            {
                ClusterForm = form;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        ~TelnetClient()
        {
        }

        #endregion

        #region misc function

        public bool SendMessage(int type, string message)
        {
            try
            {
                switch (type)
                {
                    case 0:
                        {                                               // regular message
                            if (ClusterClient.Connected)
                            {
                                ASCIIEncoding asen = new ASCIIEncoding();
                                byte[] ba = asen.GetBytes(message);
                                byte[] data = new byte[ba.Length + 2];

                                for (int i = 0; i < data.Length - 2; i++)
                                    data[i] = ba[i];

                                data[data.Length - 2] = 0x0d;
                                data[data.Length - 1] = 0x0a;
                                sw.Write(data, 0, data.Length);
                            }
                        }
                        break;

                    case 1:
                        {                                               // bye
                            if (ClusterClient.Connected)
                            {
                                ASCIIEncoding asen = new ASCIIEncoding();
                                byte[] ba = asen.GetBytes(message);
                                byte[] data = new byte[5];

                                for (int i = 0; i < message.Length; i++)
                                    data[i] = ba[i];

                                data[3] = 0x0d;
                                data[4] = 0x0a;
                                sw.Write(data, 0, data.Length);
                                Thread.Sleep(100);
                                ClusterClient.Client.Disconnect(true);
                            }
                        }
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return false;
            }
        }

        public void Start(string remote_address)
        {
            try
            {
                string[] address = remote_address.Split(':');
                remote_addr = address[0];
                remote_port = address[1];
                ClusterClient = new TcpClient();
                Thread t = new Thread(new ThreadStart(ClientServiceLoop));
                t.Start();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                MessageBox.Show("Error creating client!", "DX Cluster error");
            }
        }

        public bool Close()
        {
            try
            {
                sr.Close();
                sr.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return false;
            }
        }

        public void ClientServiceLoop()
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(remote_addr);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint ipepRemote = new IPEndPoint(ipAddress, int.Parse(remote_port));
                byte[] buffer = new byte[2048];
                int count = 0;
                string text = "";
                ASCIIEncoding buf = new ASCIIEncoding();

                if (!ClusterForm.closing)
                {
                    text = "DXClusterClient - Connecting to " + remote_addr.ToString();
                    buf.GetBytes(text, 0, text.Length, buffer, 0);
                    ClusterForm.Invoke(new CrossThreadCallback(ClusterForm.CrossThreadCommand), 1, buffer, text.Length);
                }

                ClusterClient.Connect(ipepRemote);

                if (ClusterClient.Connected && !ClusterForm.closing)
                {
                    sw = ClusterClient.GetStream();
                    sr = ClusterClient.GetStream();
                    text = "DXClusterClient - Connected to " + remote_addr.ToString();
                    buf.GetBytes(text, 0, text.Length, buffer, 0);
                    ClusterForm.Invoke(new CrossThreadCallback(ClusterForm.CrossThreadCommand), 1, buffer, text.Length);
                }
                else
                {
                    if (!ClusterForm.closing)
                    {
                        text = "DXClusterClient - Unable to connect to " + remote_addr.ToString();
                        buf.GetBytes(text, 0, text.Length, buffer, 0);
                        ClusterForm.Invoke(new CrossThreadCallback(ClusterForm.CrossThreadCommand), 1, buffer, text.Length);
                        ClusterClient.Close();
                    }
                    return;
                }

                while (ClusterClient.Connected)
                {
                    count = sr.Read(buffer, 0, 2048);
                    ClusterForm.Invoke(new CrossThreadCallback(ClusterForm.CrossThreadCommand), 0, buffer, count);
                }

                if (!ClusterForm.closing)
                {
                    text = "DXClusterClient - Disconnected";
                    buf = new ASCIIEncoding();
                    buf.GetBytes(text, 0, text.Length, buffer, 0);
                    ClusterForm.Invoke(new CrossThreadCallback(ClusterForm.CrossThreadCommand), 1, buffer, text.Length);
                }

                ClusterClient.Close();
            }
            catch (Exception ex)
            {
                if (!ClusterForm.closing)
                {
                    byte[] buffer = new byte[100];
                    string text = "";
                    ASCIIEncoding buf = new ASCIIEncoding();
                    text = "DXClusterClient - Error while connecting to " + remote_addr.ToString();
                    buf.GetBytes(text, 0, text.Length, buffer, 0);
                    ClusterForm.Invoke(new CrossThreadCallback(ClusterForm.CrossThreadCommand), 1, buffer, text.Length);
                    Debug.Write(ex.ToString());
                }
            }
        }

        #endregion
    }

    #endregion
}
