/* 	This file is part of a program that implements a Software-Defined Radio.
	The code in this file is derived from routines originally written by
	Pierre-Philippe Coupard for his CWirc X-chat program. That program
	is issued under the GPL and is
	Copyright (C) Pierre-Philippe Coupard - 18/06/2003
	This derived version is
	Copyright (C) 2004, 2005, 2006 by Frank Brickle, AB2KT and Bob McGwier, N4HY

	This program is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

	The authors can be reached by email at

	ab2kt@arrl.net
	or
	rwmcgwier@comcast.net

	or by paper mail at

	The DTTS Microwave Society
	6 Kathleen Place
	Bridgewater, NJ 08807
*/

/*
 *  Changes for GenesisRadio
 *  Copyright (C)2008,2009,2010 YT7,2011PWR Goran Radivojevic
 *  contact via email at: yt7pwr@ptt.rs or yt7pwr2002@yahoo.com
*/

using System;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

namespace PowerSDR
{
    
    public class CWKeyer2
    {
        #region Variables and Properties

        public Thread Keyer, CWTone, Monitor;
        private bool comm_port_clossing = true;
        private bool CW_monitor_off = false;
        private bool CW_monitor_on = false;
        private int keyermode = 0;
        public bool extkey_dash = false, extkey_dot = false;
        private bool keyprog = false;
        public bool CWMonitorState = false;


        private bool primary_keyer_mox = false;             // yt7pwr
        public bool PrimaryKeyerMox
        {
            set { primary_keyer_mox = value; }
        }

        private bool cw_monitor_enabled = false;            // yt7pwr
        public bool CWMonitorEnabled
        {
            set { cw_monitor_enabled = value; }
        }

        private bool enabled_primary_keyer = false;            // yt7pwr
        public bool PrimaryKeyer
        {
            set { enabled_primary_keyer = value; }
        }

        private bool enabled_secondary_keyer = false;            // yt7pwr
        public bool SecondaryKeyer
        {
            set { enabled_secondary_keyer = value; }
        }

        private bool tuneCW = false;            // yt7pwr
        public bool TuneCW
        {
            set { tuneCW = value; }
        }

        private bool dtr_cw_monitor = false;     // yt7pwr
        public bool DTRCWMonitor
        {
            set { dtr_cw_monitor = value; }
        }

        public int KeyerMode
        {
            get { return keyermode; }
            set
            {
                keyermode = value;
                DttSP.SetKeyerMode(keyermode);
            }
        }
        private HiPerfTimer timer;
        private float msdel;
        private Console console;

        public bool QRP2000CW1 = true;
        public bool QRP2000CW2 = false;

        private CATKeyerLine secondary_ptt_line = CATKeyerLine.NONE;
        public CATKeyerLine SecondaryPTTLine
        {
            get { return secondary_ptt_line; }
            set
            {
                secondary_ptt_line = value;
            }
        }

        private KeyerLine secondary_dot_line = KeyerLine.NONE;
        public KeyerLine SecondaryDOTLine
        {
            get { return secondary_dot_line; }
            set
            {
                secondary_dot_line = value;
            }
        }

        private KeyerLine secondary_dash_line = KeyerLine.NONE;
        public KeyerLine SecondaryDASHLine
        {
            get { return secondary_dash_line; }
            set
            {
                secondary_dash_line = value;
            }
        }

        private SIOListenerII siolisten;
        public SIOListenerII Siolisten
        {
            get { return siolisten; }
            set
            {
                siolisten = value;
            }
        }
        private bool cat_enabled = false;
        public bool CATEnabled
        {
            set { cat_enabled = value; }
        }

        private KeyerLine secondary_key_line = KeyerLine.NONE;
        public KeyerLine SecondaryKeyLine // changes yt7pwr
        {
            get { return secondary_key_line; }
            set
            {
                secondary_key_line = value;
                switch (secondary_key_line)
                {
                    case KeyerLine.NONE:
                        break;
                    case KeyerLine.DSR:
                        DttSP.SetWhichKey(1);
                        SP2DotKey = true;
                        break;
                    case KeyerLine.CTS:
                        SP2DotKey = false;
                        DttSP.SetWhichKey(0);
                        break;
                }
            }
        }

        private bool memorykey = false;
        public bool MemoryKey
        {
            get { return memorykey; }
            set { memorykey = value; }
        }

        private bool memoryptt = false;
        public bool MemoryPTT
        {
            get { return memoryptt; }
            set 
            {
                memoryptt = value;
            }
        }

        private string primary_conn_port = "None";
        public string PrimaryConnPort  // changes yt7pwr
        {
            get { return primary_conn_port; }
            set
            {
                primary_conn_port = value;
                switch (primary_conn_port)
                {
                    case "None":
                        primary_conn_port = "None";
                        break;
                    case "USB":
                        comm_port_clossing = true;
                        if (sp.IsOpen) sp.Close();
                        break;
                    case "NET":
                        comm_port_clossing = true;
                        if (sp.IsOpen) sp.Close();
                        break;
                    default:
                        comm_port_clossing = true;
                        if (sp.IsOpen) sp.Close();
                        sp.PortName = primary_conn_port;
                        try
                        {
                            sp.Open();
                            if (!console.DTRCWMonitor)
                                sp.DtrEnable = true;        // default
                            else
                                sp.DtrEnable = false;       // require the PCB alteration!
                            if (sp.IsOpen)
                                comm_port_clossing = false;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Primary Keyer Port [" + primary_conn_port + "] could not be opened.");
                            primary_conn_port = "USB";
                        }
                        break;
                }
            }
        }

        private string secondary_conn_port = "None";
        public string SecondaryConnPort // changes yt7pwr
        {
            get { return secondary_conn_port; }
            set
            {
                secondary_conn_port = value;
                switch (secondary_conn_port)
                {
                    case "None":
                        if (sp2.IsOpen) sp2.Close();
                        enabled_secondary_keyer = false;
                        break;
                    case "CAT":
                        if (!cat_enabled)
                            MessageBox.Show("CAT was selected for the Keyer Secondary Port, but CAT is not enabled.");
                        enabled_secondary_keyer = false;
                        break;
                    default: // COMx
                        if (sp2.IsOpen) sp2.Close();
                        sp2.PortName = secondary_conn_port;
                        try
                        {
                            sp2.Open();
                            sp2.DtrEnable = true;
                            sp2.RtsEnable = true;
                            enabled_secondary_keyer = true;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Comport for keyer program could not be opened\n");
                            secondary_conn_port = "None";
                            enabled_secondary_keyer = false;
                        }
                        break;
                }
            }
        }

        private bool sp2dotkey = false;
        public bool SP2DotKey
        {
            set { sp2dotkey = value; }
            get { return sp2dotkey; }
        }

        private bool keyerptt = false;
        public bool KeyerPTT
        {
            get { return keyerptt; }
        }

        public SerialPorts.SerialPort sp = new SerialPorts.SerialPort();
        public SerialPorts.SerialPort sp2 = new SerialPorts.SerialPort();

        #endregion

        #region Constructor and Destructor

        unsafe public CWKeyer2(Console c)
        {
            console = c;
            siolisten = console.Siolisten;
            Thread.Sleep(50);
            DttSP.NewKeyer(600.0f, true, 0.0f, 3.0f, 25.0f, (float)Audio.SampleRate1);
            DttSP.SetKeyerMode(0);
            Thread.Sleep(50);


            CWTone = new Thread(new ThreadStart(DttSP.KeyerSoundThread));
            CWTone.Name = "CW Sound Thread";
            CWTone.Priority = ThreadPriority.Highest;
            CWTone.IsBackground = true;
            CWTone.Start();

            Monitor = new Thread(new ThreadStart(DttSP.KeyerMonitorThread));
            Monitor.Name = "CW Monitor Thread";
            Monitor.Priority = ThreadPriority.Highest;
            Monitor.IsBackground = true;
            Monitor.Start();

            Thread.Sleep(100);

            Keyer = new Thread(new ThreadStart(KeyThread));
            Keyer.Name = "CW KeyThread";
            Keyer.Priority = ThreadPriority.Highest;
            Keyer.IsBackground = true;
            Keyer.Start();

            timer = new HiPerfTimer();
        }

        ~CWKeyer2()
        {
            // Destructor logic here, make sure threads cleaned up
            DttSP.StopKeyer();
            Thread.Sleep(10);
            if (CWTone != null)
                CWTone.Abort();
            if (Monitor != null)
                Monitor.Abort();
            if (Keyer.IsAlive)
                Keyer.Abort();
            Thread.Sleep(50);
            DttSP.DeleteKeyer();
        }

        #endregion

        #region Thread Functions

        unsafe private void KeyThread() // changes yt7pwr
        {
            try
            {
                int[] tmp = new int[1];

                do
                {
                    DttSP.KeyerStartedWait();
                    for (; DttSP.KeyerRunning(); )
                    {
                        timer.Start();
                        DttSP.PollTimerWait();
                        CWMonitorState = DttSP.KeyerPlaying();

                        if (tuneCW)
                        {
                            extkey_dash = true;
                            extkey_dot = false;
                        }
                        else if (memoryptt)
                        {
                            //console ptt on
                            keyprog = true;
                            extkey_dot = extkey_dash = memorykey;
                            primary_keyer_mox = false;

                            if (console.CWMonitorEnabled)
                            {
                                if (console.CurrentModel == Model.GENESIS_G59USB)
                                {
                                    if (memorykey)
                                    {
                                        if (!CW_monitor_on && cw_monitor_enabled)
                                        {
                                            console.g59.WriteToDevice(24, 1); // CW monitor on
                                            CW_monitor_off = false;
                                            CW_monitor_on = true;
                                            Debug.Write("CW monitor on\n");
                                        }
                                    }
                                    else
                                    {
                                        if (!CW_monitor_off && cw_monitor_enabled)
                                        {
                                            console.g59.WriteToDevice(24, 0);  // CW monitor off
                                            CW_monitor_on = false;
                                            CW_monitor_off = true;
                                            Debug.Write("CW monitor off\n");
                                        }
                                    }
                                }
                                else if (console.CurrentModel == Model.GENESIS_G59NET)
                                {
                                    if (memorykey)
                                    {
                                        if (!CW_monitor_on && cw_monitor_enabled)
                                        {
                                            console.net_device.WriteToDevice(24, 1); // CW monitor on
                                            CW_monitor_off = false;
                                            CW_monitor_on = true;
                                        }
                                    }
                                    else
                                    {
                                        if (!CW_monitor_off && cw_monitor_enabled)
                                        {
                                            console.net_device.WriteToDevice(24, 0);  // CW monitor off
                                            CW_monitor_on = false;
                                            CW_monitor_off = true;
                                        }
                                    }
                                }
                                else if (console.CurrentModel == Model.GENESIS_G160 ||
                                    console.CurrentModel == Model.GENESIS_G3020 ||
                                    console.CurrentModel == Model.GENESIS_G40 ||
                                    console.CurrentModel == Model.GENESIS_G80)
                                {
                                    if (memorykey)
                                    {
                                        if (!CW_monitor_on && cw_monitor_enabled)
                                        {
                                            CW_monitor(true);
                                            CW_monitor_off = false;
                                            CW_monitor_on = true;
                                        }
                                    }
                                    else
                                    {
                                        if (!CW_monitor_off && cw_monitor_enabled)
                                        {
                                            CW_monitor(false);
                                            CW_monitor_on = false;
                                            CW_monitor_off = true;
                                        }
                                    }
                                }
                            }
                        }

                        if (enabled_primary_keyer && console.g59.NewData)
                        {
                            console.g59.NewData = false;
                            extkey_dash = false;
                            extkey_dot = false;
                            keyprog = false;
                            primary_keyer_mox = true;

                            switch (primary_conn_port)
                            {
                                case "USB":
                                    {
                                        if (console.g59.Connected)
                                        {
                                            switch (console.g59.KEYER)
                                            {
                                                case 0:                                            // DASH ON command from USB
                                                    extkey_dash = true;
                                                    break;

                                                case 1:                                            // DOT ON command from USB
                                                    extkey_dot = true;
                                                    break;

                                                case 2:                                            // DASH OFF command from USB
                                                    extkey_dash = false;
                                                    break;

                                                case 3:                                            // DOT OFF command from USB
                                                    extkey_dot = false;
                                                    break;

                                                default:
                                                    extkey_dash = false;
                                                    extkey_dot = false;
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                case "NET":
                                    {
                                        if (console.net_device.Connected)
                                        {
                                            switch (console.net_device.KEYER)
                                            {
                                                case 0:                                            // DASH ON command from Network
                                                    extkey_dash = true;
                                                    break;

                                                case 1:                                            // DOT ON command from Network
                                                    extkey_dot = true;
                                                    break;

                                                case 2:                                            // DASH OFF command from Network
                                                    extkey_dash = false;
                                                    break;

                                                case 3:                                            // DOT OFF command from Network
                                                    extkey_dot = false;
                                                    break;

                                                default:
                                                    extkey_dash = false;
                                                    extkey_dot = false;
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                case "QRP2000":
                                    {
                                        if (console.MOX)
                                            console.qrp2000.SetPTTGetCWInput(1, tmp);
                                        else
                                            console.qrp2000.SetPTTGetCWInput(0, tmp);
                                        if (QRP2000CW1)
                                        {
                                            tmp[0] &= 0x20;
                                        }
                                        else if (QRP2000CW2)
                                        {
                                            tmp[0] &= 0x02;
                                        }

                                        if (tmp[0] == 0x00)
                                        {
                                            extkey_dash = extkey_dot = true;
                                        }
                                        else
                                            extkey_dash = extkey_dot = false;
                                    }
                                    break;
                                default:
                                    if (sp.IsOpen)
                                    {
                                        keyprog = false;
                                        extkey_dash = sp.CtsHolding;
                                        extkey_dot = sp.DsrHolding;

                                        if (dtr_cw_monitor && console.CWMonitorEnabled)
                                        {
                                            if (CWMonitorState)
                                                CW_monitor(true);
                                            else
                                                CW_monitor(false);
                                        }
                                    }
                                    break;
                            }
                        }
                        else if (enabled_secondary_keyer && !primary_keyer_mox)
                        {
                            keyprog = false;

                            switch (secondary_conn_port)
                            {
                                case "None":
                                    break;
                                default: // comm port
                                    if (sp2.IsOpen)
                                    {
                                        switch (secondary_dot_line)
                                        {
                                            case KeyerLine.DSR:
                                                extkey_dot = sp2.DsrHolding;
                                                break;
                                            case KeyerLine.CTS:
                                                extkey_dot = sp2.CtsHolding;
                                                break;
                                        }
                                        switch (secondary_dash_line)
                                        {
                                            case KeyerLine.DSR:
                                                extkey_dash = sp2.DsrHolding;
                                                break;
                                            case KeyerLine.CTS:
                                                extkey_dash = sp2.CtsHolding;
                                                break;
                                        }

                                        if (console.CWMonitorEnabled)
                                        {
                                            if (console.CurrentModel == Model.GENESIS_G59USB)
                                            {
                                                if (CWMonitorState)
                                                {
                                                    if (!CW_monitor_on && cw_monitor_enabled)
                                                    { 
                                                        console.g59.WriteToDevice(24, 1); // CW monitor on
                                                        CW_monitor_off = false;
                                                        CW_monitor_on = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (!CW_monitor_off && cw_monitor_enabled)
                                                    {
                                                        console.g59.WriteToDevice(24, 0);  // CW monitor off
                                                        CW_monitor_on = false;
                                                        CW_monitor_off = true;
                                                    }
                                                }
                                            }
                                            else if (console.CurrentModel == Model.GENESIS_G59NET)
                                            {
                                                if (CWMonitorState)
                                                {
                                                    if (!CW_monitor_on && cw_monitor_enabled)
                                                    {
                                                        console.net_device.WriteToDevice(24, 1); // CW monitor on
                                                        CW_monitor_off = false;
                                                        CW_monitor_on = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (!CW_monitor_off && cw_monitor_enabled)
                                                    {
                                                        console.net_device.WriteToDevice(24, 0);  // CW monitor off
                                                        CW_monitor_on = false;
                                                        CW_monitor_off = true;
                                                    }
                                                }
                                            }
                                            else if (console.CurrentModel == Model.GENESIS_G160 ||
                                                console.CurrentModel == Model.GENESIS_G3020 ||
                                                console.CurrentModel == Model.GENESIS_G40 ||
                                                console.CurrentModel == Model.GENESIS_G80)
                                            {

                                                if (dtr_cw_monitor && cw_monitor_enabled)
                                                {
                                                    if (CWMonitorState)
                                                        CW_monitor(true);
                                                    else
                                                        CW_monitor(false);
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                        }

                        timer.Stop();
                        msdel = (float)timer.DurationMsec;
                        DttSP.KeyValue(msdel, extkey_dash, extkey_dot, keyprog);

                    }
                } while (true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in KeyThread!\n" + ex.ToString());
            }
        }

        #endregion

        public void enable_tx(bool tx) // yt7pwr
        {
            if (sp.IsOpen)
                sp.RtsEnable = tx;
        }

        public bool isPTT()  // yt7pwr
        {
            bool dsr = false;
            if (!comm_port_clossing)
            {
                if (sp.IsOpen)
                {
                    dsr = System.Convert.ToBoolean(sp.DsrHolding);
                    if (dsr) return true;
                }
            }
                if (sp2.IsOpen)
                {
                    dsr = System.Convert.ToBoolean(sp2.DsrHolding);
                    if (dsr) return true;
                }
                else
                    return false;

            return false;
        }

        private void CW_monitor(bool state)      // yt7pwr
        {
            if (dtr_cw_monitor)
            {
                if (sp.IsOpen)
                    sp.DtrEnable = state;
            }
        }
    }
}
