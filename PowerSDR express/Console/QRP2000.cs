//=================================================================
//              QRP2000 with USB Si570
//=================================================================
//
// Copyright (C)2010,2011 YT7PWR Goran Radivojevic
// contact via email at: yt7pwr@ptt.rs or yt7pwr2002@yahoo.com
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

#define Si570

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;


namespace PowerSDR
{
    unsafe public class QRP2000
    {
        public Console console;
        public bool connected = false;
        int tmp;

        public QRP2000(Console c)
        {
            console = c;
        }

        #region Dll Method Definitions
        // ======================================================
        // DLL Method Definitions
        // ======================================================

        [DllImport("SRDLL.dll", EntryPoint = "srOpen")]
        public static extern bool Open(int vid, int pid, char[] Manufacturer, char[] product, char[] SerialNumber);

        [DllImport("SRDLL.dll", EntryPoint = "srClose")]
        public static extern bool Close();

        [DllImport("SRDLL.dll", EntryPoint = "srSetFreq")]
        public static extern bool SetFreq(double LOfreq, int i2cAddr);

        [DllImport("SRDLL.dll", EntryPoint = "srGetXtalFreq")]
        public static extern bool GetXtalFreq(double[] freq);

        [DllImport("SRDLL.dll", EntryPoint = "srSetXtalFreq")]
        public static extern bool SetXtalFreq(double freq);

        [DllImport("SRDLL.dll", EntryPoint = "srSetPTTGetCWInp")]
        public static extern bool SetPTTGetCWInp(int ptt, int[] CWkey);

        [DllImport("SRDLL.dll", EntryPoint = "srIsOpen")]
        public static extern bool IsOpen();
       
        #endregion

        #region properties

        public int i2caddr = 0x55;
        public int vid = 0x16c0;
        public int pid = 0x05dc;

        #endregion

        #region QRP2000 routines
        // ======================================================
        // Misc Routines
        // ======================================================

        public bool QRP2000Status()
        {
            return (IsOpen());
        }

        public void SetPTTGetCWInput(int ptt, int[] CWkey)
        {
            try
            {
                if (console.qrp2000 != null && IsOpen())
                    SetPTTGetCWInp(ptt, CWkey);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        unsafe public bool QRP2000_Init()
        {
            char[] name = new char[64];
            char[] model = new char[64];
            int[] type = new int[1];
            connected = false;

            try
            {
                if (File.Exists("SRDLL.dll"))
                {
                    connected = Open(vid, pid, null, null, null);

                    if (connected)
                    {
                        if (!console.SkinsEnabled)
                            console.btnUSB.BackColor = Color.GreenYellow;
                        console.SR_USBSi570Enable = true;
                    }
                    else
                    {
                        console.SR_USBSi570Enable = false;
                        console.btnUSB.BackColor = Color.Red;
                    }

                    return connected;
                }
                else
                {
                    MessageBox.Show("Missing SRDLL!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.Write("Error while QRp2000 init!\n"
                    + ex.ToString());
                return false;
            }
        }

        public void Close_QRP2000()
        {
            try
            {
                if (IsOpen())
                {
                    connected = false;
                    Close();
                    console.btnUSB.BackColor = Color.Red;
                }
            }
            catch(Exception ex)
            {
                Debug.Write("Error while closing USB connection!\n"
                    + ex.ToString());
            }
        }

        public void Set_SI570_freq(double freq)
        {
            try
            {
                if (IsOpen())
                    SetFreq(freq, i2caddr);

                Thread.Sleep(10);
            }
            catch (Exception ex)
            {
                Debug.Write("Error setting new frequency!\nValue is wrong!\n"
                    + ex.ToString());
                Thread.Sleep(10);
            }
        }

        #endregion
    }
}