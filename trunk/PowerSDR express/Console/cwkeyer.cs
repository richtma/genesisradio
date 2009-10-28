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
 *  Copyright (C)2008,2009 YT7PWR Goran Radivojevic
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

        public Thread Keyer, CWTone;
        private bool comm_port_clossing = true;
        private int keyermode = 0;
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
            set { memoryptt = value; }
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
                    default:
                        comm_port_clossing = true;
                        if (sp.IsOpen) sp.Close();
                        sp.PortName = primary_conn_port;
                        try
                        {
                            sp.Open();
                            sp.DtrEnable = true;
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
                        break;
                    case "CAT":
                        if (!cat_enabled)
                            MessageBox.Show("CAT was selected for the Keyer Secondary Port, but CAT is not enabled.");

                        break;
                    default: // COMx
                        if (sp2.IsOpen) sp2.Close();
                        sp2.PortName = secondary_conn_port;
                        try
                        {
                            sp2.Open();
                            sp2.DtrEnable = true;
                            sp2.RtsEnable = true;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Comport for keyer program could not be opened\n");
                            secondary_conn_port = "None";
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

        public CWKeyer2(Console c)
        {
            console = c;
            siolisten = console.Siolisten;
            Thread.Sleep(50);
            DttSP.NewKeyer(600.0f, 1, 0.0f, 3.0f, 25.0f, (float)Audio.SampleRate1);
            DttSP.SetKeyerMode(0);
            Thread.Sleep(50);


            CWTone = new Thread(new ThreadStart(DttSP.KeyerSoundThread));
            CWTone.Name = "CW Sound Thread";
            CWTone.Priority = ThreadPriority.Highest;
            CWTone.IsBackground = true;
            CWTone.Start();

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
            CWTone.Suspend();
            Keyer.Suspend();
            Thread.Sleep(50);
            DttSP.DeleteKeyer();
        }

        #endregion

        #region Thread Functions

        public void KeyThread() // changes yt7pwr
        {
            byte extkey_dash = 0, extkey_dot = 0;
            byte keyprog = 0;

            do
            {
                DttSP.KeyerStartedWait();
                for (; DttSP.KeyerRunning() != 0; )
                {
                    timer.Start();
                    DttSP.PollTimerWait();

                    switch (primary_conn_port)
                    {
                        case "USB":
                            {
                                if (console.g59.Connected)
                                {
                                    switch (console.g59.KEYER)
                                    {
                                        case 0:                                                     // DASH ON command from USB
                                            extkey_dash = 1;
                                            break;

                                        case 1:                                                     // DOT ON command from USB
                                            extkey_dot = 1;
                                            break;

                                        case 2:                                                     // DASH OFF command from USB
                                            extkey_dash = 0;
                                            break;

                                        case 3:                                                     // DOT OFF command from USB
                                            extkey_dot = 0;
                                            break;

                                        default:
                                            extkey_dash = 0;
                                            extkey_dot = 0;
                                            break;
                                    }
                                }
                            }
                            break;
                        case "None":
                            break;
                        default:
                            if (sp.IsOpen)
                            {
                                extkey_dash = System.Convert.ToByte(sp.CtsHolding);
                                extkey_dot = System.Convert.ToByte(sp.DsrHolding);
                            }
                            break;
                    }

                    switch (secondary_conn_port)
                    {
                        case "None":
                            break;
                        default: // comm port
                            if ((extkey_dash == 0) && (extkey_dot == 0)) // don't override primary
                            {
                                if (sp2.IsOpen)
                                {
                                    switch (secondary_dot_line)
                                    {
                                        case KeyerLine.DSR:
                                            extkey_dot = System.Convert.ToByte(sp2.DsrHolding);
                                            break;
                                        case KeyerLine.CTS:
                                            extkey_dot = System.Convert.ToByte(sp2.CtsHolding);
                                            break;
                                    }
                                    switch (secondary_dash_line)
                                    {
                                        case KeyerLine.DSR:
                                            extkey_dash = System.Convert.ToByte(sp2.DsrHolding);
                                            break;
                                        case KeyerLine.CTS:
                                            extkey_dash = System.Convert.ToByte(sp2.CtsHolding);
                                            break;
                                    }
                                }
                            }
                            break;
                    }

                    if (memoryptt)
                    {
                        //console ptt on
                        keyprog = 1;
                        extkey_dot = extkey_dash = System.Convert.ToByte(memorykey);
                    }
                    else
                    {
                        keyprog = 0;
                        //console ptt off
                    }
                    timer.Stop();
                    msdel = (float)timer.DurationMsec;
                    DttSP.KeyValue(msdel, extkey_dash, extkey_dot, keyprog);
//                    console.Keyer_command = 0xff;
                }
            } while (true);
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
    }
}
