//=================================================================
// Si570 external control
//=================================================================
//
//  USB communication with External Si570
//  Copyright (C)2008,2009 YT7PWR Goran Radivojevic
//  contact via email at: yt7pwr@ptt.rs or yt7pwr2002@yahoo.com
//
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
    unsafe class ExtIO_si570_usb
    {
        public Console console;

        public bool connected = false;
        int tmp;


        public ExtIO_si570_usb(Console c)
        {
            console = c;
        }

        #region Dll Method Definitions
        // ======================================================
        // DLL Method Definitions
        // ======================================================

        [DllImport("ExtIO_si570_usb.dll", EntryPoint = "InitHW")]
        public static extern bool InitHW(char* name, char* model, int* type);

        [DllImport("ExtIO_si570_usb.dll", EntryPoint = "OpenHW")]
        public static extern bool OpenHW();

        [DllImport("ExtIO_si570_usb.dll", EntryPoint = "StartHW")]
        public static extern int StartHW(long LOfreq);

        [DllImport("ExtIO_si570_usb.dll", EntryPoint = "GetStatus")]
        public static extern int GetStatus();

        [DllImport("ExtIO_si570_usb.dll", EntryPoint = "StopHW")]
        public static extern void StopHW();

        [DllImport("ExtIO_si570_usb.dll", EntryPoint = "CloseHW")]
        public static extern void CloseHW();

        [DllImport("ExtIO_si570_usb.dll", EntryPoint = "SetHWLO")]
        public static extern int SetHWLO(long freq);

        [DllImport("ExtIO_si570_usb.dll", EntryPoint = "SetCallback")]
        public static extern void SetCallback();

        [DllImport("ExtIO_si570_usb.dll", EntryPoint = "ShowGUI")]
        public static extern void ShowGUI();

        [DllImport("ExtIO_si570_usb.dll", EntryPoint = "HideGUI")]
        public static extern void HideGUI();

        [DllImport("ExtIO_si570_usb.dll", EntryPoint = "SetPTT")]
        public static extern void SetPTT(bool ptt);

        #endregion

        #region ExtIO_Si570 routines
        // ======================================================
        // Misc Routines
        // ======================================================

        public void Start_SI570(long freq)
        {
            StartHW(freq);
        }

        public void Show_Hide_SI570_GUI()
        {
            ShowGUI();
        }

        public void SetTX(bool ptt)
        {
            SetPTT(ptt);
        }

        unsafe public bool Init_USB()
        {
            char name;
            char model;
            int type;

            if (File.Exists("ExtIO_Si570_usb.dll"))
            {
                connected = InitHW(&name, &model, &type);

                if (connected)
                {
                    console.chkUSB.BackColor = Color.GreenYellow;
                    console.usb_si570_dll = true;
                }
                else
                {
                    console.SetupForm.chkGeneralUSBPresent.Checked = false;
                    console.chkUSB.BackColor = Color.Red;
                }

                return connected;
            }
            else
                return false;
        }

        public void CloseUSB()
        {
            if (connected)
                CloseHW();
        }

        public void Get_Block()
        {
        }

        public void Set_SI570_osc(long freq)
        {
            tmp = SetHWLO(freq);
        }

        public void Stop_HW()
        {
            StopHW();
        }

        #endregion
    }
}