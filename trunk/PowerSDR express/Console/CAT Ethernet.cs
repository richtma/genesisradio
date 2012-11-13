//=================================================================
// Network CAT control 
//=================================================================
//
//  Copyright (C)2010,2011,2012 YT7PWR Goran Radivojevic
//  contact via email at: yt7pwr@ptt.rs or yt7pwr2002@yahoo.com
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 3
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
//
//=================================================================


using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data;
using System.Drawing;



namespace PowerSDR
{
    #region CAT Server class

    /// <summary>
    /// CAT Ethernet server class
    /// </summary>
    public class CAToverEthernetServer
    {
        #region Variable

        Socket Listener;
        Socket client;
        private int ServerPort;
        private Console console;
        private CATParser parser;
        private string ServerIPAddress = "127.0.0.1";
        private delegate void CATConnectCallback(byte[] data);
        public delegate void CATReceiveCallback(byte[] data);
        public byte[] receive_buffer;
        private string CATpassword = "12345678";
        private bool run_server = false;
        private AutoResetEvent server_event;
        private Thread Poll_thread;
        private System.Windows.Forms.Timer ConnectionWatchdog = null;
        public int WatchdogInterval = 1000;
        public bool IPv6_enabled = false;
        private bool run_watchdog = false;
        public bool debug = false;
        private delegate void DebugCallbackFunction(string name);
        bool client_connected = false;

        #endregion

        #region constructor/destructor

        public CAToverEthernetServer(Console c)
        {
            console = c;
            parser = new CATParser(console);
            receive_buffer = new byte[2048];
            server_event = new AutoResetEvent(false);
            ConnectionWatchdog = new System.Windows.Forms.Timer();
            ConnectionWatchdog.Tick += new System.EventHandler(ServerWatchDogTimerTick);
        }

        ~CAToverEthernetServer()
        {
            if (ConnectionWatchdog != null)
                ConnectionWatchdog.Dispose();
        }

        #endregion

        #region Start/Stop

        public bool Start(string ipAddress, int port, string password)
        {
            try
            {
                CATpassword = password;
                ServerIPAddress = ipAddress;
                ServerPort = port;
                console.SetupForm.txtCATLocalIPAddress.ForeColor = Color.Red;

                string strHostName = Dns.GetHostName();
                IPHostEntry ipEntry = Dns.GetHostByAddress(ServerIPAddress);
                IPAddress[] aryLocalAddr = ipEntry.AddressList;


                switch (console.WinVer)
                {
                    case WindowsVersion.Windows2000:
                    case WindowsVersion.WindowsXP:
                        {
                            // Create the listener socket in this machines IP address
                            Listener = new Socket(AddressFamily.InterNetwork,
                                              SocketType.Stream, ProtocolType.Tcp);
                        }
                        break;

                    case WindowsVersion.WindowsVista:
                    case WindowsVersion.Windows7:
                    case WindowsVersion.Windows8:
                        {
                            if (IPv6_enabled && aryLocalAddr[0].AddressFamily == AddressFamily.InterNetworkV6)
                            {
                                // Create the listener socket in this machines IPv6 address
                                Listener = new Socket(AddressFamily.InterNetworkV6,
                                                  SocketType.Stream, ProtocolType.Tcp);
                            }
                            else
                            {
                                // Create the listener socket in this machines IP address
                                Listener = new Socket(AddressFamily.InterNetwork,
                                                  SocketType.Stream, ProtocolType.Tcp);
                            }
                        }
                        break;
                }

                Listener.Bind(new IPEndPoint(aryLocalAddr[0], port));
                Listener.Listen(1);
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Setup a callback to be notified of connection requests
                Listener.BeginAccept(new AsyncCallback(OnConnectRequest), Listener);
                console.SetupForm.txtCATLocalIPAddress.ForeColor = Color.Green;

                run_server = true;
                Poll_thread = new Thread(new ThreadStart(PollStatusThread));
                Poll_thread.Name = "Poll server thread";
                Poll_thread.IsBackground = true;
                Poll_thread.Priority = ThreadPriority.Normal;
                Poll_thread.Start();

                if (debug && !console.ConsoleClosing)
                    console.Invoke(new DebugCallbackFunction(console.DebugCallback),
                        "Server started!");

                return true;
            }
            catch (SocketException ex)
            {
                console.SetupForm.txtCATLocalIPAddress.ForeColor = Color.Red;
                console.SetupForm.EnableCATOverEthernetServer = false;
                MessageBox.Show("Cannot start CAT network server!\nCheck your Setting!\n\n"
                    + ex.ToString());

                return false;
            }
        }

        public bool Stop()
        {
            try
            {
                client_connected = false;

                if (client.Connected)
                {
                    client.Disconnect(true);
                    client.Close();
                }

                Listener.Close();
                console.SetupForm.txtCATLocalIPAddress.ForeColor = Color.Red;
                run_watchdog = false;
                run_server = false;
                ConnectionWatchdog.Stop();
                ConnectionWatchdog.Enabled = false;

                if (debug && !console.ConsoleClosing)
                    console.Invoke(new DebugCallbackFunction(console.DebugCallback),
                        "Server stoped!");

                return true;
            }
            catch (System.Exception ex)
            {
                Debug.Print(ex.Message);
                return false;
            }
        }

        #endregion

        #region misc function

        public void OnConnectRequest(IAsyncResult ar)
        {
            try
            {
                Socket listener = (Socket)ar.AsyncState;
                client = listener.EndAccept(ar);

                if (client.Connected && !client_connected)
                {
                    client_connected = true;
                    ConnectionWatchdog.Enabled = true;
                    ConnectionWatchdog.Interval = WatchdogInterval;
                    ConnectionWatchdog.Start();

                    if (debug && !console.ConsoleClosing)
                        console.Invoke(new DebugCallbackFunction(console.DebugCallback),
                            "Client connected!");

                    client.BeginReceive(receive_buffer, 0, receive_buffer.Length, SocketFlags.None, OnRecievedData, client);
                    Debug.Write("Client joined " + client.RemoteEndPoint.ToString() + "\n");
                    listener.BeginAccept(new AsyncCallback(OnConnectRequest), listener);        // again
                }
                else
                {
                    listener.Disconnect(true);
                    Listener.BeginAccept(new AsyncCallback(OnConnectRequest), Listener);

                    if (debug && !console.ConsoleClosing)
                        console.Invoke(new DebugCallbackFunction(console.DebugCallback),
                            "Client already connected!");
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        public void OnRecievedData(IAsyncResult result)
        {
            try
            {
                Socket sock = (Socket)result.AsyncState;
                int ret = sock.EndReceive(result);  // sock.Receive(receive_buffer, receive_buffer.Length, 0);


                if (ret > 0)
                {
                    ProcessData(receive_buffer, ret);

                    sock.BeginReceive(receive_buffer, 0, receive_buffer.Length, SocketFlags.None,
                        OnRecievedData, sock);
                }
                else
                {
                    client_connected = false;
                    sock.Shutdown(SocketShutdown.Both);     // loost connection
                    sock.Close(1000);
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        private void ProcessData(byte[] data, int count)
        {
            ASCIIEncoding buffer = new ASCIIEncoding();
            string command_type;
            string version;
            string password;
            string CommBuffer = "";

            try
            {
                command_type = buffer.GetString(data, 0, 3);
                version = buffer.GetString(data, 3, 2);
                password = buffer.GetString(data, 5, 8);

                if (command_type == "CAT" && version == "01" && CATpassword == password)
                {
                    CommBuffer += Regex.Replace(buffer.GetString(data, 13, Math.Min(data.Length - 13, count - 13)),
                        @"[^\w\;.]", "");
                    Regex rex = new Regex(".*?;");

                    string answer;

                    for (Match m = rex.Match(CommBuffer); m.Success; m = m.NextMatch())
                    {
                        answer = parser.Get(m.Value);

                        if (debug && !console.ConsoleClosing)
                        {
                            console.Invoke(new DebugCallbackFunction(console.DebugCallback),
                                "CAT command: " + m.Value.ToString() + "\n" +
                                "CAT answer: " + answer);
                        }

                        Debug.WriteLine(m.Value);
                        Debug.WriteLine(answer);
                        CommBuffer = CommBuffer.Replace(m.Value, "");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void PollStatusThread()
        {
            try
            {
                byte[] buffer = new byte[256];
                ASCIIEncoding ascii_buffer = new ASCIIEncoding();
                string header = "CAT01";
                string text;
                bool once = false;

                while (run_server)
                {                   
                    if (client != null && client.Connected)
                    {
                        once = false;
                        text = header + CATpassword +
                            "ZZPS;ZZAG;ZZBI;ZZCL;ZZTX;ZZCP;ZZCS;ZZDA;ZZNR;" +
                            "ZZFI;ZZGT;ZZID;ZZCM;ZZMD;ZZME;ZZSP;" +
                            "ZZMT;ZZNB;ZZNL;ZZNM;ZZPA;ZZPL;ZZRF;" +
                            "ZZRM;ZZSF;ZZSM0;ZZSO;ZZSQ;ZZST;ZZTH;ZZTL;ZZVN;" +
                            "ZZSO;ZZXF;ZZRS;ZZST;ZZSV;ZZCB;ZZVG;ZZAR;" +
                            "ZZFO;ZZFA;ZZFB;";
                        ascii_buffer.GetBytes(text, 0, text.Length, buffer, 0);
                        client.Send(buffer, SocketFlags.None);
                    }
                    else
                    {
                        if (!once)
                        {
                            if (debug && !console.ConsoleClosing)
                                console.Invoke(new DebugCallbackFunction(console.DebugCallback),
                                    "Client disconnected!");

                            client.Close();
                            once = true;
                        }
                    }

                    Thread.Sleep(10000);        // 5s refresh period
                }
            }
            catch (Exception ex)
            {
                client.Close();
                Debug.Write("Error!\n" + ex.ToString());
            }
        }

        private void ServerWatchDogTimerTick(object sender, System.EventArgs e)
        {
            try
            {
                if (run_watchdog)
                {
                    if (client != null && !client.Connected)
                    {
                        run_watchdog = false;
                        server_event.Set();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        #endregion
    }

    #endregion

    #region CAT client class

    /// <summary>
    /// CAT Ethernet client class
    /// </summary>
    public class CAToverEthernetClient
    {
        #region Variable

        private Socket ClientSocket;
        private Console console;
        private string ServerIPAddress = "127.0.0.1";
        private int ServerPort = 5000;
        private string LocalIPAddress = "127.0.0.1";
        private delegate void CATCallback(byte[] data);
        private byte[] receive_buffer;
        private byte[] send_buffer;
        private string CATpassword = "12345678";
        private CATParser parser;
        private static AutoResetEvent send_event;
        private bool run_CAT_send_thread = false;
        private bool run_CAT_client = false;
        private Thread CAT_send_thread;
        private Thread CAT_thread;
        //private AutoResetEvent connect_event;
        private Mutex send_mutex = null;
        private System.Windows.Forms.Timer timeout_timer = null;
        public bool IPv6_enabled = false;
        public int CollectionTime = 100;
        private int byte_to_send = 0;
        private int out_data_index = 0;
        private bool header_added = false;
        public bool debug = false;
        private delegate void DebugCallbackFunction(string name);

        #endregion

        #region constructor/destructor

        public CAToverEthernetClient(Console c)
        {
            try
            {
                console = c;
                parser = new CATParser(console);
                receive_buffer = new byte[2048];
                send_buffer = new byte[2048];
                send_event = new AutoResetEvent(false);
                //connect_event = new AutoResetEvent(false);
                send_mutex = new Mutex();
                timeout_timer = new System.Windows.Forms.Timer();
                timeout_timer.Tick += new System.EventHandler(SendEventTimerTick);
                timeout_timer.Enabled = true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());

                if (debug && !console.ConsoleClosing)
                    console.Invoke(new DebugCallbackFunction(console.DebugCallback),
                        "Ethernet client constructor error!\n" + ex.ToString());
            }
        }

        ~CAToverEthernetClient()
        {
            if (ClientSocket != null)
                ClientSocket.Close(1000);

            timeout_timer.Stop();
        }

        #endregion

        #region Start/Stop

        public bool Start(string ServerAddress, int serverPort,
            string LocalAddress, string password)
        {
            try
            {
                ServerIPAddress = ServerAddress;
                ServerPort = serverPort;
                LocalIPAddress = LocalAddress;
                CATpassword = password;

                run_CAT_client = true;
                SetupSocket();
                /*CAT_thread = new Thread(new ThreadStart(EthernetCATClientThread));
                CAT_thread.Name = "CAT client ethernet thread";
                CAT_thread.IsBackground = true;
                CAT_thread.Priority = ThreadPriority.Normal;
                CAT_thread.Start();*/

                return true;
            }
            catch (Exception ex)
            {
                //connect_event.Set();
                run_CAT_client = false;
                Debug.Write(ex.ToString());
                return false;
            }
        }

        public bool Stop()
        {
            try
            {
                run_CAT_send_thread = false;
                send_event.Set();
                console.btnNetwork.BackColor = Color.Red;
                console.SetupForm.txtCATServerIPAddress.ForeColor = Color.Red;
                run_CAT_client = false;     // exit connection thread

                //if (connect_event != null)
                    //connect_event.Set();

                if (ClientSocket != null && ClientSocket.Connected)
                {
                    ClientSocket.Shutdown(SocketShutdown.Both);
                    ClientSocket.Close();
                }
                else if (ClientSocket != null)
                    ClientSocket.Close();

                return true;
            }
            catch (System.Exception ex)
            {
                Debug.Print(ex.Message);

                if (debug && !console.ConsoleClosing)
                    console.Invoke(new DebugCallbackFunction(console.DebugCallback),
                        "SClose client error!\n" + ex.ToString());

                return false;
            }
        }

        #endregion

        #region misc function

        private bool SetupSocket()
        {
            try
            {
                console.btnNetwork.BackColor = Color.Red;
                console.SetupForm.txtCATServerIPAddress.ForeColor = Color.Red;

                IPHostEntry ipHostInfo = Dns.GetHostEntry(ServerIPAddress);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint ipepServer = new IPEndPoint(ipAddress, ServerPort);

                switch (console.WinVer)
                {
                    case WindowsVersion.Windows2000:
                    case WindowsVersion.WindowsXP:
                        {
                            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            ClientSocket.Blocking = false;
                            ClientSocket.BeginConnect(ServerIPAddress, ServerPort, new AsyncCallback(ConnectCallback),
                                ClientSocket);
                        }
                        break;
                    case WindowsVersion.WindowsVista:
                    case WindowsVersion.Windows7:
                    case WindowsVersion.Windows8:
                        {
                            if (IPv6_enabled && ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
                            {
                                ClientSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                                ClientSocket.Blocking = false;
                                ClientSocket.BeginConnect(ipepServer, new AsyncCallback(ConnectCallback), ClientSocket);
                            }
                            else
                            {
                                ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                ClientSocket.Blocking = false;
                                ClientSocket.BeginConnect(ServerIPAddress, ServerPort, new AsyncCallback(ConnectCallback),
                                    ClientSocket);
                            }
                        }
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                if (debug && !console.ConsoleClosing)
                    console.Invoke(new DebugCallbackFunction(console.DebugCallback),
                        "SetupSocket client error!\n" + ex.ToString());

                MessageBox.Show("Error!Check your CAT client network data!\n\n" + ex.ToString());
                return false;
            }
        }

        private void ConnectCallback(IAsyncResult result)
        {
            try
            {
                Socket sock = (Socket)result.AsyncState;

                if (sock.Connected)
                {
                    Debug.Write("Connected!\n");

                    if (debug && !console.ConsoleClosing)
                        console.Invoke(new DebugCallbackFunction(console.DebugCallback), "Connected!");

                    sock.BeginReceive(receive_buffer, 0, receive_buffer.Length, SocketFlags.None,
                        new AsyncCallback(ReceiveCallback), sock);

                    run_CAT_send_thread = true;
                    CAT_send_thread = new Thread(new ThreadStart(SendThread));
                    CAT_send_thread.Name = "CAT client send thread";
                    CAT_send_thread.IsBackground = true;
                    CAT_send_thread.Priority = ThreadPriority.Normal;
                    CAT_send_thread.Start();

                    console.btnNetwork.BackColor = Color.Green;
                    console.SetupForm.txtCATServerIPAddress.ForeColor = Color.Green;
                }
                else
                {
                    console.SetupForm.txtCATServerIPAddress.ForeColor = Color.Red;
                    console.btnNetwork.BackColor = Color.Red;
                    SetupSocket();
                }
            }
            catch (Exception ex)
            {
                //connect_event.Set();
                Debug.Write(ex.Message);
                SetupSocket();
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                Socket sock = (Socket)result.AsyncState;
                int num_read = sock.EndReceive(result);

                if (num_read > 0)
                {
                    ProcessData(receive_buffer);

                    for (int i = 0; i < receive_buffer.Length; i++)
                        receive_buffer[i] = 0;

                    sock.BeginReceive(receive_buffer, 0, receive_buffer.Length, SocketFlags.None,
                        new AsyncCallback(ReceiveCallback), sock);
                }
                else
                {
                    send_event.Set();
                    Debug.Write("Disconnected!\n");

                    if (debug && !console.ConsoleClosing)
                        console.Invoke(new DebugCallbackFunction(console.DebugCallback), "Disconnected!");

                    if (ClientSocket != null)
                    {
                        ClientSocket.Shutdown(SocketShutdown.Both);
                        ClientSocket.Close();
                    }

                    //connect_event.Set();
                    SetupSocket();
                }               
            }
            catch (SocketException socketException)
            {
                send_event.Set();

                if (socketException.ErrorCode == 10054)
                {
                    ClientSocket.Close(1000);
                    SetupSocket();
                }
            }
            catch (ObjectDisposedException)
            {
                send_event.Set();
                ClientSocket.Close(1000);
                SetupSocket();
            }
        }

        public void ClientServerSync(string data)               // request for sending to server
        {
            ASCIIEncoding buffer = new ASCIIEncoding();
            string command_type = "CAT";
            string version = "01";
            string CommBuffer = "";

            try
            {
                CommBuffer += Regex.Replace(data, @"[^\w\;.]", "");
                Regex rex = new Regex(".*?;");

                string answer = "";
                byte[] out_string = new byte[1024];
                byte[] header = new byte[16];
                int out_string_index = 0;

                send_mutex.WaitOne();

                if (!header_added)
                {
                    buffer.GetBytes(command_type + version + CATpassword, 0, 13, header, 0);
                    string header_string = "";
                    header_string = buffer.GetString(header, 0, 13);
                    buffer.GetBytes(header_string, 0, 13, send_buffer, 0);
                    header_added = true;
                    byte_to_send = 13;
                    out_data_index = 13;
                }

                for (Match m = rex.Match(CommBuffer); m.Success; m = m.NextMatch())
                {
                    answer = parser.Get(m.Value);
                    Debug.WriteLine(m.Value);
                    Debug.WriteLine(answer);
                    buffer.GetBytes(answer, 0, answer.Length, out_string, out_string_index);
                    CommBuffer = CommBuffer.Replace(m.Value, "");
                    out_string_index += answer.Length;
                }

                string tmp_string = "";
                tmp_string = buffer.GetString(out_string, 0, answer.Length);
                buffer.GetBytes(tmp_string, 0, answer.Length, send_buffer, out_data_index);
                out_data_index += answer.Length;
                byte_to_send = out_data_index;

                timeout_timer.Interval = CollectionTime;
                timeout_timer.Start();

                send_mutex.ReleaseMutex();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                if (debug && !console.ConsoleClosing)
                    console.Invoke(new DebugCallbackFunction(console.DebugCallback),
                        "ClientServerSync client error!\n" + ex.ToString());
            }
        }

        private void ProcessData(byte[] data)                       // answer received from server
        {
            ASCIIEncoding buffer = new ASCIIEncoding();
            string command_type;
            string version;
            string password;
            string CommBuffer = "";

            try
            {
                command_type = buffer.GetString(data, 0, 3);
                version = buffer.GetString(data, 3, 2);
                password = buffer.GetString(data, 5, 8);

                if (command_type == "CAT" && version == "01" && CATpassword == password)
                {
                    CommBuffer += Regex.Replace(buffer.GetString(data, 13, data.Length - 13), @"[^\w\;.]", "");
                    Regex rex = new Regex(".*?;");

                    string answer;
                    byte[] out_string = new byte[2048];
                    int out_index = 13;

                    buffer.GetBytes(command_type + version + password, 0, 13, out_string, 0);

                    for (Match m = rex.Match(CommBuffer); m.Success; m = m.NextMatch())
                    {
                        answer = parser.Get(m.Value);

                        if (debug && !console.ConsoleClosing)
                        {
                            console.Invoke(new DebugCallbackFunction(console.DebugCallback),
                                "CAT command: " + m.Value.ToString() + "\n" +
                                "CAT answer: " + answer);
                        }

                        Debug.WriteLine(m.Value);
                        Debug.WriteLine(answer);
                        buffer.GetBytes(answer, 0, answer.Length, out_string, out_index);
                        CommBuffer = CommBuffer.Replace(m.Value, "");
                        out_index += answer.Length;

                        if (out_index > 1024)
                        {
                            out_index = 13;
                            send_buffer = out_string;
                            byte_to_send = out_data_index;
                            send_event.Set();
                            buffer.GetBytes(command_type + version + password, 0, 13, out_string, 0);
                        }
                    }

                    //send_mutex.WaitOne();
                    send_buffer = out_string;
                    byte_to_send = out_index;
                    //send_mutex.ReleaseMutex();
                    send_event.Set();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void SendThread()
        {
            try
            {
                while (run_CAT_send_thread)
                {
                    send_event.WaitOne();

                    if (ClientSocket.Connected && run_CAT_send_thread)
                    {
                        send_mutex.WaitOne();
                        int sendBytes = ClientSocket.Send(send_buffer, 0, byte_to_send, SocketFlags.None);
                        send_mutex.ReleaseMutex();

                        if (sendBytes != byte_to_send)
                        {
                            ClientSocket.Shutdown(SocketShutdown.Both);
                            ClientSocket.Close(1000);
                            console.btnNetwork.BackColor = Color.Red;
                            console.SetupForm.txtCATServerIPAddress.ForeColor = Color.Red;
                            run_CAT_send_thread = false;
                        }

                        for (int i = 0; i < send_buffer.Length; i++)
                            send_buffer[i] = 0;
                        byte_to_send = 13;
                    }
                    else
                    {
                        ClientSocket.Shutdown(SocketShutdown.Both);
                        ClientSocket.Close();

                        if (debug && !console.ConsoleClosing)
                            console.Invoke(new DebugCallbackFunction(console.DebugCallback),
                                "Client disconnected!");

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Write("Error!\n" + ex.ToString());
                //connect_event.Set();
                SetupSocket();
            }

            if (run_CAT_client)
                //connect_event.Set();
                SetupSocket();
        }

        private void SendEventTimerTick(object sender, System.EventArgs e)
        {
            send_mutex.WaitOne();
            send_event.Set();
            out_data_index = 0;                        // reset for new packet
            header_added = false;
            timeout_timer.Stop();
            send_mutex.ReleaseMutex();
        }

        #endregion
    }


    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////
    /// State object for receiving data from remote device.
    /// </summary>
    public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 128;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    #endregion
}