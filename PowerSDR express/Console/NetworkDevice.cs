//=================================================================
// Network control for external 18F4620 + ENC28J60 device
//=================================================================
//
//  Copyright (C)2010 YT7PWR Goran Radivojevic
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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using NetworkController;
using System.Net;
using System.Net.Sockets;
using System.Drawing;

namespace PowerSDR
{
    public class NetworkControl
    {
        #region Variable declaration

        private Console console;
        private static NetworkController.NetworkControlDll net_control_device;
        private bool GUI_visible;
        Thread network_thread;

        public delegate void CallbackFunction(byte[] data);
        public static NetworkController.NetworkControlDll.NetworkCallback callback;
        private static string RemoteHost;
        private static int RemotePort;
        private static int HostPort;

        public long Si570_freq = 56000000;
        private double rfreq = 0.0;
        private double fdco = 0.0;
        private long n1 = 0;
        private char[] msg_hs_div_n1_rfreq = new char[6];
        private int[] hs_divtab = new int[6];
        private double freq_division = 4.0;
        public bool booting = true;
        private bool run_network_thread = false;
        private bool test_connection = false;
        private int test_counter = 0;
        private int tmp_hs_div = 4;

        #endregion

        #region properties

        private bool smooth_tuning = false;
        public bool SmoothTuning
        {
            get { return smooth_tuning; }
            set { smooth_tuning = value; }
        }

        private Keyer_mode keyer_mode = Keyer_mode.PHONE;
        public short KeyerMode
        {
            get { return (short)keyer_mode; }
            set { keyer_mode = (Keyer_mode)value; }
        }

        private string firmware = null;
        public string FIRMWARE_VER
        {
            get { return firmware; }
            set { firmware = value; }
        }

        private string boot = null;
        public string BOOT_VER
        {
            get { return boot; }
            set { boot = value; }
        }

        private string serial_no = null;
        public string SERIAL_NO
        {
            get { return serial_no; }
            set { serial_no = value; }
        }

        private int fwd_pwr = 0x0000;
        public int fwd_PWR
        {
            get { return fwd_pwr; }
            set { fwd_pwr = value; }
        }

        private int ref_pwr = 0x0000;
        public int Ref_PWR
        {
            get { return ref_pwr; }
            set { ref_pwr = value; }
        }

        private int Keyer_command = 0xff;
        public int KEYER
        {
            get { return Keyer_command; }
            set { Keyer_command = value; }
        }

        private bool mox = false;
        public bool MOX
        {
            get { return mox; }
            set { mox = value; }
        }

        private bool connected = false;
        public bool Connected
        {
            get { return connected; }
            set { connected = value; }
        }

        private int si570_address = 0x55;
        public int si570_i2c_address
        {
            get { return si570_address; }

            set { si570_address = value; }
        }

        private int si570_divider = 4;
        public int si570_div
        {
            get { return si570_divider; }

            set { si570_divider = value; }
        }

        private double si570_xtal = 114285000.000;
        public double si570_fxtal
        {
            get { return si570_xtal; }

            set { si570_xtal = value; }
        }

        private double FDCO_min = 4850e6;
        public double FDCOmin
        {
            set { FDCO_min = value; }
        }

        private double FDCO_max = 5670e6;
        public double FDCOmax
        {
            set { FDCO_max = value; }
        }

        private int hs_div = 11;
        public int HSDiv
        {
            set { hs_div = value; }
        }

        #endregion

        #region Constructor and Destructor

        public NetworkControl(Console c)
        {
            console = c;
            net_control_device = new NetworkControlDll();
            GUI_visible = false;
            connected = false;
            callback = new NetworkControlDll.NetworkCallback(GetData);
            net_control_device.SetCallback(callback);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        ~NetworkControl()
        {
        }

        #endregion

        #region General routines

        public void Connect(string host, int Host_Port, int Remote_Port)
        {
            RemoteHost = host;
            RemotePort = Remote_Port;
            HostPort = Host_Port;

            if (network_thread == null || !network_thread.IsAlive)
            {
                run_network_thread = false;
                network_thread = new Thread(new ThreadStart(NetworkThread));
                network_thread.Name = "Connection Network Thread";
                network_thread.Priority = ThreadPriority.Normal;
                network_thread.IsBackground = true;
                network_thread.Start();
            }
            run_network_thread = true;
        }

        private void NetworkThread()
        {
            byte[] OutputReportBuffer;
            OutputReportBuffer = new byte[64];
            test_counter = 0;
            net_control_device.StartConnection(RemoteHost, RemotePort);
            Thread.Sleep(100);
            while (run_network_thread)
            {
                if (connected)
                {
                    test_connection = false;
                    Thread.Sleep(100);
                    string text = "SET_NAME Genesis";
                    ASCIIEncoding buffer = new ASCIIEncoding();
                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                    OutputReportBuffer[22] = 1;        // for testing network connection
                    bool success = net_control_device.SendUDP(RemoteHost, RemotePort, OutputReportBuffer);
                    Thread.Sleep(3000);
                    if (!test_connection)
                    {
                        test_counter++;
                        if (test_counter >= 3)
                        {
                            console.network_event.Set();    // kick network thread
                            run_network_thread = false;     // exit
                            connected = false;
                        }
                    }
                    else if (!success)
                    {
                        console.network_event.Set();    // kick network thread
                        run_network_thread = false;     // exit
                        connected = false;
                    }
                }
                else
                {
                    string text = "SET_NAME Genesis";
                    ASCIIEncoding buffer = new ASCIIEncoding();
                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                    OutputReportBuffer[22] = 0;        // for connecting
                    bool success = net_control_device.SendUDP(RemoteHost, RemotePort, OutputReportBuffer);
                    if (!success)
                    {
                        console.network_event.Set();    // kick network thread
                        run_network_thread = false;     // exit
                        connected = false;
                    }
                    Thread.Sleep(1000);
                }
            }
        }

        public void Disconnect()
        {
            if (connected)
                net_control_device.Disconnect();
            connected = false;
            run_network_thread = false;
        }

        public void Show_Hide_GUI()
        {
            if (!GUI_visible)
            {
                GUI_visible = true;
            }
            else
            {
                GUI_visible = false;
            }
        }

        public void SetLOSC(long new_freq, bool forced)
        {
            try
            {
                if (smooth_tuning && !forced)
                {
                    double freq_drift = (((double)Si570_freq * 4) * 0.0035 / 2); // 3500ppm
                    if (Si570_freq == new_freq &&
                            connected && (new_freq > 1000000 && new_freq < 66000000))
                        WriteToDevice(25, new_freq);
                    else if (new_freq > Si570_freq && ((new_freq - Si570_freq) < freq_drift) &&
                            connected && (new_freq > 1000000 && new_freq < 66000000))
                        WriteToDevice(25, new_freq);
                    else if (Si570_freq > new_freq && ((Si570_freq - new_freq) < freq_drift) &&
                            connected && (new_freq > 1000000 && new_freq < 66000000))
                        WriteToDevice(25, new_freq);
                    else if (connected && (new_freq > 1000000 && new_freq < 66000000))
                    {
                        WriteToDevice(1, new_freq);
                        Si570_freq = new_freq;
                    }
                }
                else
                {
                    if (connected && (new_freq > 1000000 && new_freq < 66000000))
                    {
                        WriteToDevice(1, new_freq);
                        Si570_freq = new_freq;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace.ToString(), "Error in SetHWLO!",
                  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void WriteToDevice(int type, long data)
        {
            if (!booting && connected)
                WriteToNETdevice(type, data);
        }

        public bool WriteToNETdevice(int type, long data)
        {
            byte[] OutputReportBuffer;
            bool success = false;
            int i;

            try
            {
                if (console.CurrentModel == Model.GENESIS_G59NET)
                {
                    if (type == 1 || type == 25)
                        si570_freq(data);

                    if (connected)
                    {
                        success = false;
                        // Set the size of the Output report buffer.
                        OutputReportBuffer = new byte[64];

                        switch (type)
                        {
                            case (0):            // set name on LCD display
                                {
                                    string text = "SET_NAME Genesis";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                    OutputReportBuffer[21] = (byte)si570_address;
                                    OutputReportBuffer[22] = (byte)data;        // for testing network connection
                                }
                                break;
                            case (1):            // set FREQ
                                {
                                    String text = "";
                                    if (data < 10000000.0)
                                        text = String.Format("SET_FREQ0{0:G}", data);
                                    else if (data >= 144000000)
                                        text = String.Format("SET_FREQ{0:G}", data);
                                    else
                                        text = String.Format("SET_FREQ{0:G}", data);
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                    for (i = 0; i < 6; i++)
                                    {
                                        OutputReportBuffer[i + 21] = (byte)msg_hs_div_n1_rfreq[i];
                                    }
                                    OutputReportBuffer[18] = (byte)si570_address;
                                }
                                break;
                            case (2):            // exit application
                                {
                                    string text = "CLOSE_HW";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, 64, OutputReportBuffer, 0);
                                }
                                break;
                            case (3):            // send filter data
                                {
                                    string text = "SET_FILT";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                    OutputReportBuffer[21] = (byte)data;
                                }
                                break;
                            case (4):            // send CW Keyer Speed
                                {
                                    string text = "K_SPEED ";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                    OutputReportBuffer[21] = (byte)(1200 / (data * 6));
                                }
                                break;
                            case (5):            // AF ON/OFF
                                {
                                    string text = " AF_ON  ";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                    OutputReportBuffer[21] = (byte)data;
                                }
                                break;
                            case (7):            // MUTE ON
                                {
                                    string text = "MUTE_ON ";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                }
                                break;
                            case (8):            // MUTE OFF
                                {
                                    string text = "MUTE_OFF";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                }
                                break;
                            case (9):            // XVTR ON
                                {
                                    string text = "TRV_ON  ";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                    OutputReportBuffer[21] = (byte)data;
                                }
                                break;
                            case (11):            // preamplifier ON/OFF
                                {
                                    string text = "RF_ON   ";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                    OutputReportBuffer[21] = (byte)data;
                                }
                                break;
                            case (13):            // Transmiter ON
                                {
                                    string text = "TX_ON   ";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                    OutputReportBuffer[21] = (byte)data;

                                }
                                break;
                            case (14):            // Transmiter OFF
                                {
                                    string text = "TX_OFF  ";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                }
                                break;
                            case (15):            // Read configuration  (About)
                                {
                                    string text = "READ_CFG";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                }
                                break;
                            case (16):            // ATT ON/OFF
                                {
                                    string text = "ATT_ON  ";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                    OutputReportBuffer[21] = (byte)data;
                                }
                                break;
                            case (18):            // send CW Keyer Mode
                                {
                                    string text = "K_MODE  ";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                    OutputReportBuffer[21] = (byte)(data);
                                }
                                break;
                            case (19):            // read config
                                {
                                    string text = "READ_CFG";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                }
                                break;
                            case (20):            // DASH/DOT ratio
                                {
                                    string text = "K_RATIO ";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                    OutputReportBuffer[21] = (byte)(data);
                                }
                                break;
                            case (21):            // PA10 present in system
                                {
                                    string text = "PA10_ON ";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                    OutputReportBuffer[21] = (byte)(data);
                                }
                                break;
                            case (22):            // Keyer Automatic correction
                                {
                                    string text = "AUTO_COR";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                    OutputReportBuffer[21] = (byte)(data);
                                }
                                break;
                            case (23):            // Second RX antena
                                {
                                    string text = "SEC_RX2 ";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                    OutputReportBuffer[21] = (byte)(data);
                                }
                                break;
                            case (24):            // CW monitor
                                {
                                    string text = "MONITOR ";
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                    OutputReportBuffer[21] = (byte)(data);
                                }
                                break;
                            case (25):            // set FREQ
                                {
                                    String text = "";
                                    if (data < 10000000.0)
                                        text = String.Format("SMOOTH  0{0:G}", data);
                                    else if (data >= 144000000)
                                        text = String.Format("SMOOTH  {0:G}", data);
                                    else
                                        text = String.Format("SMOOTH  {0:G}", data);
                                    ASCIIEncoding buffer = new ASCIIEncoding();
                                    buffer.GetBytes(text, 0, text.Length, OutputReportBuffer, 0);
                                    for (i = 0; i < 6; i++)
                                    {
                                        OutputReportBuffer[i + 21] = (byte)msg_hs_div_n1_rfreq[i];
                                    }
                                    OutputReportBuffer[19] = (byte)si570_address;
                                }
                                break;
                        }

                        success = net_control_device.SendUDP(RemoteHost, RemotePort, OutputReportBuffer);
                        return success;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace.ToString(), "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region Process network data

        private void ProcessData(byte[] data)
        {
            string command_buffer;
            ASCIIEncoding buffer = new ASCIIEncoding();
            if (data.Length == 64)
            {
                command_buffer = buffer.GetString(data, 0, 8);
                fwd_PWR = data[40] << 8;
                fwd_PWR += data[41];
                Ref_PWR = data[42] << 8;
                Ref_PWR += data[43];
                fwd_PWR = (fwd_PWR & 0x3ff);
                Ref_PWR = (Ref_PWR & 0x3ff);
                switch (command_buffer)
                {
                    case "DASH_ON ":
                        KEYER = 0;
                        break;

                    case "DOT_ON  ":
                        if (mox == false && keyer_mode == Keyer_mode.PHONE)
                            mox = true;
                        KEYER = 1;
                        break;

                    case "DASH_OFF":
                        KEYER = 2;
                        break;

                    case "DOT_OFF ":
                        if (mox == true && keyer_mode == Keyer_mode.PHONE)
                            mox = false;
                        KEYER = 3;
                        break;

                    case "BOOT_VER":
                        {
                            ASCIIEncoding firmware_buffer = new ASCIIEncoding();
                            boot = buffer.GetString(data, 8, 5);
                        }
                        break;

                    case "FIRM_VER":
                        {
                            ASCIIEncoding firmware_buffer = new ASCIIEncoding();
                            firmware = buffer.GetString(data, 8, 5);
                            serial_no = buffer.GetString(data, 15, 3);
                        }
                        break;
                    case "CONNECT ":
                        {
                            connected = true;
                            console.btnNetwork.BackColor = Color.Green;

                            if (console.CurrentModel == Model.GENESIS_G59NET)
                            {
                                console.net_device.WriteToDevice(6, 0); // AF OFF
                                Thread.Sleep(20);
                                console.net_device.WriteToDevice(8, 0); // MUTE OFF
                                Thread.Sleep(20);
                                console.net_device.WriteToDevice(10, 0);  // TRV OFF
                                Thread.Sleep(20);
                                console.net_device.WriteToDevice(12, 0);  // RF OFF
                                Thread.Sleep(20);
                                console.net_device.WriteToDevice(14, 0);  // TX OFF
                                Thread.Sleep(20);
                                console.net_device.WriteToDevice(17, 0);  // ATT OFF
                                Thread.Sleep(20);
                                console.net_device.WriteToDevice(21, 0);  // PA10 OFF
                                Thread.Sleep(20);
                                console.net_device.WriteToDevice(24, 0);  // MONITOR OFF
                                Thread.Sleep(20);
                                console.net_device.WriteToDevice(4, (long)console.SetupForm.G59CWSpeed);    // write CW keyer speed
                                Thread.Sleep(20);
                                console.net_device.WriteToDevice(20, (long)(console.SetupForm.G59DASH_DOT_ratio * 10)); // write DASH/DOT ratio
                                Thread.Sleep(20);
                            }
                        }
                        break;
                    case "DISCONN ":
                        Connected = false;
                        break;
                    case "I'M LIVE":
                        {
                            test_counter = 0;
                            test_connection = true;
                        }
                        break;
                }
            }
        }

        #endregion

        #region Thread

        private void GetData(byte[] data)
        {
            ProcessData(data);
        }

        #endregion

        // yt7pwr
        #region Si570

        private void si570_program()
        {
            long rfreq_int = (long)rfreq;
            long rfreq_frac = (long)((rfreq - (double)rfreq_int) * (double)268435456);
            long rft0, rft1, rft2;
            long tmp;
            tmp_hs_div -= 4;
            n1 = n1 - 1;

            tmp = ((tmp_hs_div << 5) & 0xe0) | ((n1 >> 2) & 0x1f);
            msg_hs_div_n1_rfreq[0] = (char)tmp;

            //there are 38 bits, 10 bits int portion, 28 bits frac portion.
            rft0 = (rfreq_int >> 4) & 0x3f;		// cutoff 4 bits, store remaining 6 bits
            tmp = rft0 | ((n1 << 6) & 0xc0);
            msg_hs_div_n1_rfreq[1] = (char)tmp;

            rft1 = (rfreq_int & 0x0F) << 4;		// mask the lower 4 bit and shift to upper nibble
            rft2 = (rfreq_frac >> 24) & 0x0f;	// frac bits 24 to 28 into lower nibble
            tmp = (rft1 | rft2);
            msg_hs_div_n1_rfreq[2] = (char)tmp;

            tmp = (rfreq_frac >> 16) & 0xff;
            msg_hs_div_n1_rfreq[3] = (char)tmp;
            tmp = (rfreq_frac >> 8) & 0xff;
            msg_hs_div_n1_rfreq[4] = (char)tmp;
            tmp = rfreq_frac & 0xff;
            msg_hs_div_n1_rfreq[5] = (char)tmp;
        }

        double si570_match(double fo)
        {
            long _hs_div, _n1, _rfreq_int, _rfreq_frac;
            double _fdco, _rfreq, _fo;
            double nxhsdiv;
            int hsdivpt;

            switch (hs_div)
            {
                case 4:
                    hs_divtab[0] = 4;  //{11,9,7,6,5,4};
                    hs_divtab[1] = 5;
                    hs_divtab[2] = 6;
                    hs_divtab[3] = 7;
                    hs_divtab[4] = 9;
                    hs_divtab[5] = 11;
                    break;
                case 5:
                    hs_divtab[0] = 5;
                    hs_divtab[1] = 6;
                    hs_divtab[2] = 7;
                    hs_divtab[3] = 9;
                    hs_divtab[4] = 11;
                    hs_divtab[5] = 4;
                    break;
                case 6:
                    hs_divtab[0] = 6;
                    hs_divtab[1] = 7;
                    hs_divtab[2] = 9;
                    hs_divtab[3] = 11;
                    hs_divtab[4] = 4;
                    hs_divtab[5] = 5;
                    break;
                case 7:
                    hs_divtab[0] = 7;
                    hs_divtab[1] = 9;
                    hs_divtab[2] = 11;
                    hs_divtab[3] = 4;
                    hs_divtab[4] = 5;
                    hs_divtab[5] = 6;
                    break;
                case 9:
                    hs_divtab[0] = 9;
                    hs_divtab[1] = 11;
                    hs_divtab[2] = 4;
                    hs_divtab[3] = 5;
                    hs_divtab[4] = 6;
                    hs_divtab[5] = 7;
                    break;
                case 11:
                    hs_divtab[0] = 11;
                    hs_divtab[1] = 4;
                    hs_divtab[2] = 5;
                    hs_divtab[3] = 6;
                    hs_divtab[4] = 7;
                    hs_divtab[5] = 9;
                    break;
            }

            n1 = 0;

            for (hsdivpt = 0; hsdivpt < 6; hsdivpt++)
            {
                _hs_div = hs_divtab[hsdivpt];

                for (_n1 = 2; _n1 <= 128; _n1 += 2)
                {
                    nxhsdiv = (double)_n1 * (double)_hs_div;
                    if (!(nxhsdiv >= 6.0)) continue;
                    _fdco = nxhsdiv * fo;
                    if ((_fdco >= FDCO_min) && (_fdco <= FDCO_max))
                    {
                        // we have a valid combination
                        _rfreq = _fdco / si570_fxtal;
                        if (!(_rfreq >= (double)0 && _rfreq < (double)1025)) continue;
                        _rfreq_int = (long)_rfreq;
                        _rfreq_frac = (long)((_rfreq - (double)_rfreq_int) * (double)268435456);
                        _fo = ((double)_rfreq_int + (double)_rfreq_frac / (double)268435456) * si570_fxtal / ((double)_n1 * (double)_hs_div);
                        //				if(!(fabs(_fo - fo) < 1 )) continue;

                        if (nxhsdiv >= n1 * hs_div)
                        {
                            n1 = _n1; tmp_hs_div = (int)_hs_div; rfreq = _rfreq; fo = _fo;
                            fdco = _fdco;
                            return fo;
                        }
                    }
                }
            }
            return 0.0;
        }

        double si570_freq(long value)
        {
            freq_division = 4;
            double fo = freq_division * (double)value;
            double fres;
            n1 = 0;
            fres = si570_match(fo);
            if (fres == 0.0) return 0.0;
            si570_program();
            return fres;
        }
        #endregion

    }
}
