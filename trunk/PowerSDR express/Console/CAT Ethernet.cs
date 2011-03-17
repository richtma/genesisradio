//=================================================================
// Network CAT control 
//=================================================================
//
//  Copyright (C)2010,2011 YT7PWR Goran Radivojevic
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
    /// <summary>
    /// CAT Ethernet server class
    /// </summary>
    public class CAToverEthernetServer
    {
        #region Variable

        private Socket ServerSocket;
        private Socket WorkingSocket;
        private int ServerPort;
        private Console console;
        private CATParser parser;
        private string ServerIPAddress = "192.168.0.1";
        private delegate void CATConnectCallback(byte[] data);
        public delegate void CATReceiveCallback(byte[] data);
        public byte[] receive_buffer;
        private string CATpassword = "12345678";
        private bool poll_thread_run = false;
        private Thread server_thread;
        private bool run_server_thread = false;
        private AutoResetEvent server_event;
        private Thread Poll_thread;
        private System.Windows.Forms.Timer ConnectionWatchdog = null;
        public int WatchdogInterval = 1000;
        public bool IPv6_enabled = false;
        private bool run_watchdog = false;

        #endregion

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
            if (ServerSocket != null)
                ServerSocket.Close(1000);
            if (ConnectionWatchdog != null)
                ConnectionWatchdog.Dispose();
        }

        public bool StartEthernetCAT(string ipAddress, int port, string password)
        {
            try
            {
                CATpassword = password;
                ServerIPAddress = ipAddress;
                ServerPort = port;
                EndPoint ipep = null;

                switch (console.WinVer)
                {
                    case WindowsVersion.Windows2000:
                    case WindowsVersion.WindowsXP:
                        {
                            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            ipep = new IPEndPoint(IPAddress.Parse(ServerIPAddress), ServerPort);
                            ServerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                        }
                        break;
                    case WindowsVersion.WindowsVista:
                    case WindowsVersion.Windows7:
                        {
                            ServerSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                            ServerSocket.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, 0);  // allow IPV4 and IPV6
                            ipep = new IPEndPoint(IPAddress.IPv6Any, ServerPort);
                        }
                        break;
                }

                ServerSocket.Bind(ipep);
                ServerSocket.Listen(4);

                run_server_thread = true;
                server_event.Reset();
                server_thread = new Thread(new ThreadStart(ServerThread));
                server_thread.Name = "CAT server thread";
                server_thread.IsBackground = true;
                server_thread.Priority = ThreadPriority.Normal;
                server_thread.Start();

                ConnectionWatchdog.Enabled = true;
                ConnectionWatchdog.Interval = WatchdogInterval;
                ConnectionWatchdog.Start();

                return true;
            }
            catch (SocketException ex)
            {
                console.SetupForm.EnableCATOverEthernetServer = false;
                MessageBox.Show("Cannot start CAT network server!\nCheck your Setting!\n\n"
                    + ex.ToString());
                return false;
            }
        }

        private void ServerThread()
        {
            try
            {
                while (run_server_thread)
                {
                    if (ServerSocket != null)
                        ServerSocket.BeginAccept(new AsyncCallback(AsyncAcceptCallback), ServerSocket);
                    else
                        run_server_thread = false;
                    server_event.WaitOne();
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.ToString());
            }
        }

        public bool close()
        {
            try
            {
                run_watchdog = false;
                ConnectionWatchdog.Stop();
                ConnectionWatchdog.Enabled = false;
                poll_thread_run = false;
                run_server_thread = false;
                server_event.Set();

                if (ServerSocket != null)
                    ServerSocket.Close(1000);

                if (WorkingSocket != null && WorkingSocket.Connected)
                {
                    WorkingSocket.Shutdown(SocketShutdown.Both);
                    WorkingSocket.Close(1000);
                }
                else if (WorkingSocket != null)
                    WorkingSocket.Close(1000);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.Print(e.Message);
                return false;
            }
        }

        private void ProcessData(byte[] data)
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
                    CommBuffer += Regex.Replace(buffer.GetString(data, 13, 2035), @"[^\w\;.]", "");
                    Regex rex = new Regex(".*?;");

                    string answer;

                    for (Match m = rex.Match(CommBuffer); m.Success; m = m.NextMatch())
                    {
                        answer = parser.Get(m.Value);
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
                while (poll_thread_run)
                {
                    byte[] buffer = new byte[256];
                    ASCIIEncoding ascii_buffer = new ASCIIEncoding();
                    string header = "CAT01";
                    string text;

                    if (WorkingSocket != null && WorkingSocket.Connected)
                    {
                        text = header + CATpassword +
                            "ZZPS;ZZAG;ZZBI;ZZCL;ZZTX;ZZCP;ZZCS;ZZDA;ZZNR;" +
                            "ZZFI;ZZGT;ZZID;ZZIU;ZZCM;ZZMD;ZZME;ZZSP;" +
                            "ZZMT;ZZNB;ZZNL;ZZNM;ZZPA;ZZPL;ZZRF;" +
                            "ZZRM;ZZSF;ZZSM;ZZSO;ZZSQ;ZZST;ZZTH;ZZTL;ZZVN;" +
                            "ZZVS;ZZXC;ZZXF;ZZRS;ZZST;ZZSV;ZZCB;ZZVG;ZZAR;" +
                            "ZZFO;ZZFA;ZZFB;";
                        ascii_buffer.GetBytes(text, 0, text.Length, buffer, 0);
                        WorkingSocket.Send(buffer, SocketFlags.None);
                    }
                    else if (WorkingSocket != null)
                    {
                        poll_thread_run = false;
                        server_event.Set();
                    }

                    Thread.Sleep(10000);        // 10s refresh period
                }
            }
            catch (Exception ex)
            {
                poll_thread_run = false;
                Debug.Write("Error!\n" + ex.ToString());
                WorkingSocket.Close(1000);
                server_event.Set();
            }
        }

        private void AsyncAcceptCallback(IAsyncResult result)
        {
            try
            {
                if (WorkingSocket == null)
                {
                    if (IPv6_enabled)
                        WorkingSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                    else
                        WorkingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }

                if (ServerSocket != null)
                {
                    if (WorkingSocket != null && !WorkingSocket.Connected)
                    {
                        Socket listener = (Socket)result.AsyncState;
                        WorkingSocket = listener.EndAccept(result);

                        StateObject state = new StateObject();
                        state.workSocket = WorkingSocket;

                        WorkingSocket.BeginReceive(receive_buffer, 0, receive_buffer.Length, SocketFlags.None,
                            new AsyncCallback(ReceiveCallback), WorkingSocket);

                        run_watchdog = true;
                        Debug.Write("Client connected!");

                        poll_thread_run = true;
                        Poll_thread = new Thread(new ThreadStart(PollStatusThread));
                        Poll_thread.Name = "CAT Poll thread";
                        Poll_thread.IsBackground = true;
                        Poll_thread.Priority = ThreadPriority.Normal;
                        Poll_thread.Start();
                    }
                }
            }
            catch (ObjectDisposedException exception)
            {
                if (WorkingSocket != null)
                    WorkingSocket.Close();
                if (ServerSocket != null)
                    ServerSocket.Close();
                server_event.Set();
                Debug.Write(exception.Message);
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                if (WorkingSocket != null && WorkingSocket.Connected)
                {
                    int num_read = WorkingSocket.EndReceive(result);
                    if (0 != num_read)
                    {
                        if (WorkingSocket.Connected)
                            ProcessData(receive_buffer);

                        for (int i = 0; i < 2048; i++)
                            receive_buffer[i] = 0;

                        WorkingSocket.BeginReceive(receive_buffer, 0, receive_buffer.Length, SocketFlags.None,
                            new AsyncCallback(ReceiveCallback), null);
                    }
                    else
                    {
                        Debug.Write("Disconnected!\n");
                        WorkingSocket.Close();
                        WorkingSocket = null;
                        server_event.Set();
                    }
                }
                else
                {
                    Debug.Write("Disconnected!\n");
                    run_watchdog = false;
                    ConnectionWatchdog.Stop();
                    WorkingSocket.Close();
                    WorkingSocket = null;
                    server_event.Set();
                }
            }
            catch (SocketException socketException)
            {
                //WSAECONNRESET, the other side closed impolitely
                if (socketException.ErrorCode == 10054)
                {
                    WorkingSocket.Close(1000);
                    WorkingSocket = null;
                    run_watchdog = false;
                    ConnectionWatchdog.Stop();
                    server_event.Set();
                }
            }
            catch (ObjectDisposedException)
            {
                // The socket was closed out from under me
                WorkingSocket.Close(1000);
                WorkingSocket = null;
                server_event.Set();
            }
        }

        private void DisconnectCallback(IAsyncResult result)
        {
            try
            {
                ConnectionWatchdog.Stop();
                // Complete the disconnect request.
                Socket client = (Socket)result.AsyncState;
                client.EndDisconnect(result);
                Debug.Write("Client Disconnected!\n");
                server_event.Set();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                server_event.Set();
            }
        }

        private void ServerWatchDogTimerTick(object sender, System.EventArgs e)
        {
            try
            {
                if (run_watchdog)
                {                   
                    if (WorkingSocket != null && !WorkingSocket.Connected)
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
    }

    /// <summary>
    /// CAT Ethernet client class
    /// </summary>
    public class CAToverEthernetClient
    {
        #region Variable

        private Socket CATSocket;
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
        private bool run_CAT_thread = false;
        private Thread CAT_send_thread;
        private Thread CAT_thread;
        private AutoResetEvent connect_event;
        private Mutex send_mutex = null;
        private AutoResetEvent proces_event = null;
        private System.Windows.Forms.Timer timeout_timer = null;
        public bool IPv6_enabled = false;
        public int CollectionTime = 100;
        private int byte_to_send = 0;
        private int out_data_index = 0;
        private bool header_added = false;

        #endregion

        public CAToverEthernetClient(Console c)
        {
            console = c;
            parser = new CATParser(console);
            receive_buffer = new byte[2048];
            send_buffer = new byte[2048];
            send_event = new AutoResetEvent(false);
            connect_event = new AutoResetEvent(false);
            send_mutex = new Mutex();
            proces_event = new AutoResetEvent(false);
            timeout_timer = new System.Windows.Forms.Timer();
            timeout_timer.Tick += new System.EventHandler(SendEventTimerTick);
            timeout_timer.Enabled = true;
        }

        ~CAToverEthernetClient()
        {
            if (CATSocket != null)
                CATSocket.Close(1000);
            timeout_timer.Stop();
        }

        public bool StartEthernetCATThread(string ServerAddress, int serverPort,
            string LocalAddress, string password)
        {
            try
            {
                ServerIPAddress = ServerAddress;
                ServerPort = serverPort;
                LocalIPAddress = LocalAddress;
                CATpassword = password;

                run_CAT_thread = true;
                CAT_thread = new Thread(new ThreadStart(EthernetCATClientThread));
                CAT_thread.Name = "CAT client ethernet thread";
                CAT_thread.IsBackground = true;
                CAT_thread.Priority = ThreadPriority.Normal;
                CAT_thread.Start();

                return true;
            }
            catch (Exception ex)
            {
                connect_event.Set();
                run_CAT_thread = false;
                Debug.Write(ex.ToString());
                return false;
            }
        }

        private void EthernetCATClientThread()
        {
            try
            {
                while (run_CAT_thread)
                {
                    if (!SetupSocket())
                    {
                        console.btnNetwork.BackColor = Color.Red;
                        if (CATSocket != null)
                            CATSocket.Close();
                        run_CAT_thread = false;
                        connect_event.Set();
                    }
                    else
                    {
                        Thread.Sleep(5000);
                        connect_event.WaitOne();
                    }
                }
            }
            catch (SocketException ex)
            {
                Debug.Write("Error!\nCannot start CAT over ethernet!\nCheck your Setting!\n"
                    + ex.ToString());
            }
        }

        private bool SetupSocket()
        {
            try
            {
                console.btnNetwork.BackColor = Color.Red;

                IPHostEntry ipHostInfo = Dns.GetHostEntry(ServerIPAddress);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint ipepServer = new IPEndPoint(ipAddress, ServerPort);
                switch (console.WinVer)
                {
                    case WindowsVersion.Windows2000:
                    case WindowsVersion.WindowsXP:
                        {
                            CATSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        }
                        break;
                    case WindowsVersion.WindowsVista:
                    case WindowsVersion.Windows7:
                        {
                            if (IPv6_enabled)
                                CATSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                            else
                                CATSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        }
                        break;
                }

                CATSocket.BeginConnect(ipepServer, new AsyncCallback(ConnectCallback), CATSocket);

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error!Check your CAT client network data!\n\n" + e.ToString());
                return false;
            }
        }

        private void SendThread()
        {
            try
            {

                while (run_CAT_send_thread)
                {
                    send_event.WaitOne();
                    if (CATSocket.Connected)
                    {
                        send_mutex.WaitOne();
                        int sendBytes = CATSocket.Send(send_buffer, 0, byte_to_send, SocketFlags.None);
                        send_mutex.ReleaseMutex();
                        if (sendBytes != byte_to_send)
                        {
                            CATSocket.Close(1000);
                            console.btnNetwork.BackColor = Color.Red;
                            run_CAT_send_thread = false;
                        }

                        for (int i = 0; i < send_buffer.Length; i++)
                            send_buffer[i] = 0;
                        byte_to_send = 13;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Write("Error!\n" + ex.ToString());
                connect_event.Set();
            }

            if (run_CAT_thread)
                connect_event.Set();
        }

        public bool close()
        {
            try
            {
                console.btnNetwork.BackColor = Color.Red;
                run_CAT_thread = false;     // exit connection thread
                if (connect_event != null)
                    connect_event.Set();

                if (CATSocket != null && CATSocket.Connected)
                {
                    CATSocket.Shutdown(SocketShutdown.Both);
                    CATSocket.Close(1000);
                }
                else if (CATSocket != null)
                    CATSocket.Close(1000);
                return true;
            }
            catch (System.Exception e)
            {
                Debug.Print(e.Message);
                return false;
            }
        }

        private void DisconnectCallback(IAsyncResult result)
        {
            try
            {
                console.btnNetwork.BackColor = Color.Red;
                if (CATSocket != null)
                    CATSocket.EndDisconnect(result);
                connect_event.Set();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }

        private void ConnectCallback(IAsyncResult result)
        {
            try
            {
                CATSocket = (Socket)result.AsyncState;
                if (CATSocket != null)
                {
                    CATSocket.EndConnect(result);

                    if (CATSocket.Connected)
                    {
                        Debug.Write("Connected!\n");
                        StateObject state = new StateObject();
                        state.workSocket = CATSocket;

                        CATSocket.BeginReceive(receive_buffer, 0, receive_buffer.Length, SocketFlags.None,
                            new AsyncCallback(ReceiveCallback), state);

                        run_CAT_send_thread = true;
                        CAT_send_thread = new Thread(new ThreadStart(SendThread));
                        CAT_send_thread.Name = "CAT client send thread";
                        CAT_send_thread.IsBackground = true;
                        CAT_send_thread.Priority = ThreadPriority.Normal;
                        CAT_send_thread.Start();

                        console.btnNetwork.BackColor = Color.Green;
                    }
                    else
                    {
                        connect_event.Set();    // try again
                        console.btnNetwork.BackColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                connect_event.Set();
                Debug.Write(ex.Message);
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                if(CATSocket != null && CATSocket.Connected)
                {
                    int num_read = CATSocket.EndReceive(result);
                    if (num_read > 0)
                    {
                        if (CATSocket.Connected)
                            ProcessData(receive_buffer);

                        for (int i = 0; i < receive_buffer.Length; i++)
                            receive_buffer[i] = 0;

                        StateObject state = new StateObject();
                        state.workSocket = CATSocket;
                        CATSocket.BeginReceive(receive_buffer, 0, receive_buffer.Length, SocketFlags.None,
                            new AsyncCallback(ReceiveCallback), state);
                    }
                    else
                    {
                        Debug.Write("Disconnected!\n");
                        if (CATSocket != null)
                            CATSocket.Close();
                        connect_event.Set();
                    }
                }
                else
                {
                    Debug.Write("Disconnected!\n");
                    if (CATSocket != null)
                        CATSocket.Close();
                    connect_event.Set();
                }
            }
            catch (SocketException socketException)
            {
                if (socketException.ErrorCode == 10054)
                {
                    CATSocket.Close(1000);
                }
            }
            catch (ObjectDisposedException)
            {
                CATSocket.Close(1000);
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

                    send_mutex.WaitOne();
                    send_buffer = out_string;
                    byte_to_send = out_index;
                    send_mutex.ReleaseMutex();
                    send_event.Set();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
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
}