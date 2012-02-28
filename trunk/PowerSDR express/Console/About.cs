//=================================================================
// About
//=================================================================
//  
//  Copyright (C)2009,2010,2011 YT7PWR Goran Radivojevic
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace PowerSDR
{
    public partial class About : Form
    {
        Console console;

        public About(Console c)
        {
            console = c;
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Stream GetResource(string name)
        {
            return this.GetType().Assembly.GetManifestResourceStream(name);
        }

        private void About_Load(object sender, EventArgs e)
        {
            switch (console.CurrentModel)
            {
                case Model.GENESIS_G59USB:
                    {
                        console.g59.WriteToDevice(19, 0);  // read software version
                        Thread.Sleep(100);
                        console.g59.WriteToDevice(29, 0);  // read serial number
                        Thread.Sleep(100);
                        lblFirm_version.Text = console.g59.FIRMWARE_VER;
                        lblSerialNumber.Text = console.g59.SERIAL_NO;
                        lblBoot_version.Text = console.g59.BOOT_VER;
                        console.g59.FIRMWARE_VER = "";
                        console.g59.BOOT_VER = "";
                        console.g59.SERIAL_NO = "";
                        lblRadioModel.Text = "Genesis G59";
                    }
                    break;

                case Model.GENESIS_G59NET:
                    {
                        console.net_device.WriteToDevice(19, 0);  // read software version
                        Thread.Sleep(100);
                        console.net_device.WriteToDevice(29, 0);  // read serial number
                        Thread.Sleep(100);
                        lblFirm_version.Text = console.net_device.FIRMWARE_VER;
                        lblSerialNumber.Text = console.net_device.SERIAL_NO;
                        lblBoot_version.Text = console.net_device.BOOT_VER;
                        console.net_device.FIRMWARE_VER = "";
                        console.net_device.BOOT_VER = "";
                        console.net_device.SERIAL_NO = "";
                        lblRadioModel.Text = "Genesis G59NET";
                    }
                    break;

                case Model.GENESIS_G11:
                    {
                        console.g11.WriteToDevice(19, 0);  // read software version
                        Thread.Sleep(100);
                        console.g11.WriteToDevice(29, 0);  // read serial number
                        Thread.Sleep(100);
                        lblFirm_version.Text = console.g11.FIRMWARE_VER;
                        lblSerialNumber.Text = console.g11.SERIAL_NO;
                        lblBoot_version.Text = console.g11.BOOT_VER;
                        console.g11.FIRMWARE_VER = "";
                        console.g11.BOOT_VER = "";
                        console.g11.SERIAL_NO = "";
                        lblRadioModel.Text = "Genesis G11";
                    }
                    break;

                case Model.GENESIS_G137:
                    lblRadioModel.Text = "Genesis G137";
                    break;

                case Model.GENESIS_G500:
                    lblRadioModel.Text = "Genesis G500";
                    break;

                case Model.GENESIS_G160:
                    lblRadioModel.Text = "Genesis G160";
                    break;

                case Model.GENESIS_G80:
                    lblRadioModel.Text = "Genesis G80";
                    break;

                case Model.GENESIS_G40:
                    lblRadioModel.Text = "Genesis G40";
                    break;

                case Model.GENESIS_G3020:
                    lblRadioModel.Text = "Genesis G3020";
                    break;

                case Model.QRP2000:
                    lblRadioModel.Text = "QRP2000";
                    break;

            }
        }
    }
}