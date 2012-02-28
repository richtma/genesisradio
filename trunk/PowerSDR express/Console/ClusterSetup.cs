//=================================================================
// ClusterSetup.cs
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

namespace PowerSDR
{
    partial class ClusterSetup : Form
    {
        #region DLL imports

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr window, int message, int wparam, int lparam);

        #endregion

        const int WM_VSCROLL = 0x115;
        const int SB_BOTTOM = 7;
        DXClusterClient ParrentForm;

        public ClusterSetup(DXClusterClient form)
        {
            InitializeComponent();
            ParrentForm = form;
            GetOptions();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string[] vals = txtHosts.Text.Split('\n');
                ParrentForm.UpdateHostsList(vals);
                this.Close();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        public void GetOptions()
        {
            try
            {
                ArrayList a = DB.GetVars("DXClusterOptions");
                a.Sort();

                foreach (string s in a)
                {
                    string b = s.Replace('/', '\n');
                    txtHosts.AppendText(b);
                    SendMessage(txtHosts.Handle, WM_VSCROLL, SB_BOTTOM, 0);
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }
    }
}
