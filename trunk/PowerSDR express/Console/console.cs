//=================================================================
// console.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004, 2005, 2006  FlexRadio Systems
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
// You may contact us via email at: sales@flex-radio.com.
// Paper mail may be sent to: 
//    FlexRadio Systems
//    12100 Technology Blvd.
//    Austin, TX 78727
//    USA
//=================================================================

/*
 *  Changes for GenesisRadio
 *  Copyright (C)2008,2009 YT7PWR Goran Radivojevic
 *  contact via email at: yt7pwr@ptt.rs or yt7pwr2002@yahoo.com
*/


//#define INTERLEAVED
//#define SPLIT_INTERLEAVED


using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using SDRSerialSupportII;
using Splash_Screen;
using GenesisG59;


namespace PowerSDR
{
	#region Enums

    public enum ColorSheme
    {
        original = 0,
        enhanced,
        SPECTRAN,
        BLACKWHITE,
        off,
    }

	public enum MultiMeterDisplayMode
	{
		Original = 0,
		Edge,
		Analog,
	}

	public enum FilterWidthMode
	{
		Linear = 0,
		Log,
		Log10,
	}

	public enum DisplayEngine
	{
		GDI_PLUS = 0,
		DIRECT_X,
	}

	public enum Model
	{
		GENESIS_G59 = 1,
        GENESIS_G3020 = 2,
        GENESIS_G40 = 3,
        GENESIS_G80 = 4,
        GENESIS_G160 = 5,
	}

	public enum BPFBand
	{
		NONE = -1,
		B160,
		B60,
		B20,
		B40,		
		B10,
		B6,
	}

	public enum RFELPFBand
	{
		NONE = -1,
		AUX,
		B6,
		B60,
		B20,
		B30,
		B40,
		B1210,
		B80,
		B1715,
		B160,
	}

	public enum PAFBand
	{
		NONE = 0,
		B1210,
		B1715,
		B3020,
		B6040,
		B80,
		B160,
	}

	public enum SoundCard
	{
		FIRST = -1,
		UNSUPPORTED_CARD,
		DELTA_44,
		FIREBOX,
		EDIROL_FA_66,
		AUDIGY,
		AUDIGY_2,
		AUDIGY_2_ZS,
		EXTIGY,
		MP3_PLUS,
		SANTA_CRUZ,
        REALTEK_HD_AUDIO,
		LAST,
	}

	public enum DisplayMode
	{
		FIRST = -1,
		PANADAPTER,
		WATERFALL,
        PANAFALL,
        PHASE,
        PHASE2,
        SPECTRUM,
        SCOPE,
        HISTOGRAM,
        OFF,
		LAST,
	}

	public enum AGCMode
	{
		FIRST = -1,
		FIXD,
		LONG,
		SLOW,
		MED,
		FAST,
		CUSTOM,
		LAST,
	}

	public enum MeterRXMode
	{
		FIRST = -1,
		SIGNAL_STRENGTH,
		SIGNAL_AVERAGE,
		ADC_L,
		ADC_R,
		OFF,
		LAST,
	}

	public enum MeterTXMode
	{
		FIRST = -1,
		FORWARD_POWER,
		REVERSE_POWER,
		MIC,
		EQ,
		LEVELER,
		LVL_G,
		COMP,
		CPDR,
		ALC,
		ALC_G,
		SWR,
		OFF,
		LAST,
	}

	public enum CATKeyerLine
	{
		NONE = 0,
		DTR,
		RTS,
	}

    public enum KeyerLine
    {
        NONE = 0,
        DSR,
        CTS,
    }

	public enum DateTimeMode
	{
		OFF = 0,
		LOCAL,
		UTC,
		LAST,
	}

	public enum BandPlan
	{
		IARU1 = 1,
		IARU2 = 2,
		IARU3 = 3,
	}

	public enum PreampMode
	{
		FIRST = -1,
		OFF,
		LOW,
		MED,
		HIGH,
		LAST,
	}

	public enum DSPMode
	{
		FIRST = -1,
		LSB,
		USB,
		DSB,
		CWL,
		CWU,
		FMN,
		AM,
		DIGU,
		SPEC,
		DIGL,
		SAM,
		DRM,
		LAST,
	}

	public enum Band
	{
		FIRST = -1,
		GEN,
		B160M,
		B80M,
		B60M,
		B40M,
		B30M,
		B20M,
		B17M,
		B15M,
		B12M,
		B10M,
		B6M,
		B2M,
		WWV,
		VHF0,
		VHF1,
		VHF2,
		VHF3,
		VHF4,
		VHF5,
		VHF6,
		VHF7,
		VHF8,
		VHF9,
		VHF10,
		VHF11,
		VHF12,
		VHF13,
		LAST,
	}

	public enum Filter
	{
		FIRST = -1,
		F1,
		F2,
		F3,
		F4,
		F5,
		F6,
		F7,
		F8,
		F9,
		F10,
		VAR1,
		VAR2,
		NONE,
		LAST,
	}

	public enum PTTMode
	{
		FIRST = -1,
		NONE,
		MANUAL,
		MIC,
		CW,
		CAT,
		VOX,
		LAST,
	}

	public enum DisplayLabelAlignment
	{
		FIRST = -1,
		LEFT,
		CENTER,
		RIGHT,
		AUTO,
		OFF,
		LAST,
	}

	public enum ClickTuneMode
	{
		Off = 0,
		VFOA,
		VFOB,
	}

	#endregion

    unsafe public partial class Console : System.Windows.Forms.Form
    {
        #region Variable Declarations
        // ======================================================
        // Variable Declarations
        // ======================================================

        #region Genesis
        internal const Int32 WM_DEVICECHANGE = 0X219;
        public GenesisG59.G59 g59;
        public double rx_phase = 0.0;
        public double rx_gain = 0.0;
        public double tx_phase = 0.0;
        public double tx_gain = 0.0;
        public bool booting = false;
        private ArrayList RXphase_gain;
        private ArrayList TXphase_gain;
        #endregion

        private PA19.PaStreamCallback callback1;			// audio callback for regular RX/TX
        private PA19.PaStreamCallback callbackVAC;			// audio callback for Virtual Audio Cable
        private PA19.PaStreamCallback callback4port;		// audio callback for 4port cards

        private SIOListenerII siolisten = null; 

        private Thread audio_process_thread;				// fields calls to DttSP functions
        private Thread draw_display_thread;					// draws the main display 
        private Thread multimeter_thread;					// draws/updates the multimeter
        private Thread poll_ptt_thread;						// polls the PTT line on the parallel port
        private Thread sql_update_thread;					// polls the RX signal strength
        private Thread vox_update_thread;					// polls the mic input
        private Thread noise_gate_update_thread;			// polls the mic input during TX

        public About AboutForm;
        public Setup SetupForm;
        public CWX CWXForm;
        public EQForm EQForm;
        public FilterForm filterForm;

        private ExtIO_si570_usb SI570;

        public WaveControl WaveForm;

        private bool run_setup_wizard;						// Used to run the wizard the first time the software comes up

        private int band_160m_index;						// These band indexes are used to keep track of which
        private int band_80m_index;							// location in the bandstack was last saved/recalled
        private int band_60m_index;
        private int band_40m_index;
        private int band_30m_index;
        private int band_20m_index;
        private int band_17m_index;
        private int band_15m_index;
        private int band_12m_index;
        private int band_10m_index;
        private int band_6m_index;
        private int band_2m_index;
        private int band_wwv_index;
        private int band_gen_index;

        private int band_160m_register;						// These integers are the number of band stack registers
        private int band_80m_register;						// found in the database for each band
        private int band_60m_register;
        private int band_40m_register;
        private int band_30m_register;
        private int band_20m_register;
        private int band_17m_register;
        private int band_15m_register;
        private int band_12m_register;
        private int band_10m_register;
        private int band_6m_register;
        private int band_2m_register;
        private int band_wwv_register;
        private int band_gen_register;

        private double[] wheel_tune_list;					// A list of available tuning steps
        private int wheel_tune_index;						// An index into the above array

        private Button[] vhf_text;
        private bool was_panadapter = false;				// used to restore panadater when switching to spectrum DSP mode

        private float[] preamp_offset;						// offset values for each preamp mode in dB
        public float multimeter_cal_offset;					// multimeter calibration offset per volume setting in dB
        public float filter_size_cal_offset;				// filter offset based on DSP filter size
        public float xvtr_gain_offset;						// gain offset as entered on the xvtr form
        private int current_xvtr_index = -1;				// index of current xvtr in use

        private bool meter_data_ready;						// used to synchronize the new DSP data with the multimeter
        private float new_meter_data;						// new data for the multimeter from the DSP
        private float current_meter_data;					// current data for the multimeter
        private int meter_peak_count;						// Counter for peak hold on multimeter
        private int meter_peak_value;						// Value for peak hold on multimeter
        private float[] meter_text_history;					// Array used to output the peak power over a period of time
        private int meter_text_history_index;				// index used with above variable to do peak power

        private int pa_fwd_power;							// forward power as read by the ADC on the PA
        private int pa_rev_power;							// reverse power as read by the ADC on the PA
        private bool tuning;								// true when the TUN button is active
        private bool atu_tuning;							// true while the atu is tuning
        private Band tuned_band;							// last band that the atu was tuned on

        private bool shift_down;							// used to modify tuning rate
        private bool calibrating;							// true if running a calibration routine
        private bool manual_mox;							// True if the MOX button was clicked on (not PTT)		

        private DSPMode vfob_dsp_mode;						// Saves control pointer for last mode used on VFO B 
        private Filter vfob_filter;							// Saves control pointer for last filter used on VFO B
        private int vfo_char_width;							// Used to calibrate mousewheel tuning
        private int vfo_char_space;							// Used to calibrate mousewheel tuning
        private int vfo_small_char_width;					// Used to calibrate mousewheel tuning
        private int vfo_small_char_space;					// Used to calibrate mousewheel tuning
        private int vfo_decimal_width;						// Used to calibrate mousewheel tuning
        private int vfo_decimal_space;						// Used to calibrate mousewheel tuning		
        private int vfo_pixel_offset;						// Used to calibrate mousewheel tuning
        private int vfoa_hover_digit;						// Digit for hover display
        private int vfob_hover_digit;						// Digit for hover display
        private string last_band;							// Used in bandstacking algorithm

        private int losc_char_width;
        private int losc_char_space;
        private int losc_small_char_width;
        private int losc_small_char_space;
        private int losc_decimal_width;
        private int losc_decimal_space;
        private int losc_pixel_offset;
        private int losc_hover_digit;

        private string separator;							// contains the locations specific decimal separator

        private int last_filter_shift;						// VK6APH
        private int last_var1_shift;						// VK6APH 
        private int last_var2_shift;						// VK6APH

        public string[] CmdLineArgs;

        private double avg_last_ddsfreq = 0;				// Used to move the display average when tuning
        private double avg_last_dttsp_osc = 0;

        public CWKeyer2 Keyer;
        private HiPerfTimer break_in_timer;
        public double avg_vox_pwr = 0.0;

        #endregion


        #region Windows Form Generated Code

        private System.Windows.Forms.ButtonTS btnDisplayZoom1x;
        private System.Windows.Forms.ButtonTS btnDisplayZoom2x;
        private System.Windows.Forms.ButtonTS btnDisplayZoom4x;
        private System.Windows.Forms.ButtonTS btnDisplayZoom8x;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.TextBoxTS txtLOSCFreq;
        private System.Windows.Forms.TextBoxTS txtVFOAFreq;
        private System.Windows.Forms.TextBoxTS txtVFOABand;
        private System.Windows.Forms.TextBoxTS txtVFOBFreq;
        private System.Windows.Forms.PictureBox picDisplay;
        private System.Windows.Forms.GroupBoxTS grpLOSC;
        private System.Windows.Forms.GroupBoxTS grpVFOA;
        private System.Windows.Forms.GroupBoxTS grpVFOB;
        private System.Windows.Forms.TextBoxTS txtVFOBBand;
        private System.Windows.Forms.GroupBoxTS grpMode;
        private System.Windows.Forms.GroupBoxTS grpDisplay;
        private System.Windows.Forms.CheckBoxTS chkPower;
        private System.Windows.Forms.LabelTS lblCPUMeter;
        private System.Windows.Forms.ComboBoxTS comboDisplayMode;
        private System.Windows.Forms.NumericUpDownTS udFilterLow;
        private System.Windows.Forms.NumericUpDownTS udFilterHigh;
        private System.Windows.Forms.RadioButtonTS radFilterVar1;
        private System.Windows.Forms.RadioButtonTS radFilterVar2;
        private System.Windows.Forms.RadioButtonTS radModeSPEC;
        private System.Windows.Forms.RadioButtonTS radModeLSB;
        private System.Windows.Forms.RadioButtonTS radModeDIGL;
        private System.Windows.Forms.RadioButtonTS radModeCWU;
        private System.Windows.Forms.RadioButtonTS radModeDSB;
        private System.Windows.Forms.RadioButtonTS radModeSAM;
        private System.Windows.Forms.RadioButtonTS radModeAM;
        private System.Windows.Forms.RadioButtonTS radModeCWL;
        private System.Windows.Forms.RadioButtonTS radModeUSB;
        private System.Windows.Forms.RadioButtonTS radModeFMN;
        private System.Windows.Forms.RadioButtonTS radModeDRM;
        private System.Windows.Forms.GroupBoxTS grpDSP;
        private System.Windows.Forms.LabelTS lblAGC;
        private System.Windows.Forms.ComboBoxTS comboAGC;
        private System.Windows.Forms.CheckBoxTS chkNB;
        private System.Windows.Forms.CheckBoxTS chkANF;
        private System.Windows.Forms.CheckBoxTS chkNR;
        private System.Windows.Forms.CheckBoxTS chkMON;
        private System.Windows.Forms.CheckBoxTS chkTUN;
        private System.Windows.Forms.CheckBoxTS chkMOX;
        private System.Windows.Forms.NumericUpDownTS udXIT;
        private System.Windows.Forms.NumericUpDownTS udRIT;
        private System.Windows.Forms.CheckBoxTS chkMUT;
        private System.Windows.Forms.CheckBoxTS chkXIT;
        private System.Windows.Forms.CheckBoxTS chkRIT;
        private System.Windows.Forms.LabelTS lblPWR;
        private System.Windows.Forms.NumericUpDownTS udPWR;
        private System.Windows.Forms.LabelTS lblAF;
        private System.Windows.Forms.NumericUpDownTS udAF;
        private System.Windows.Forms.LabelTS lblMIC;
        private System.Windows.Forms.NumericUpDownTS udMIC;
        private System.Windows.Forms.TextBoxTS txtWheelTune;
        private System.Windows.Forms.CheckBoxTS chkBIN;
        private System.Windows.Forms.GroupBoxTS grpMultimeter;
        private System.Windows.Forms.ButtonTS btnVFOSwap;
        private System.Windows.Forms.ButtonTS btnVFOBtoA;
        private System.Windows.Forms.ButtonTS btnVFOAtoB;
        private System.Windows.Forms.GroupBoxTS grpVFO;
        private System.Windows.Forms.CheckBoxTS chkVFOSplit;
        private System.Windows.Forms.GroupBoxTS grpDisplay2;
        private System.Windows.Forms.CheckBoxTS chkDisplayAVG;
        private System.Windows.Forms.TextBoxTS txtMultiText;
        private System.Windows.Forms.Timer timer_cpu_meter;
        private System.Windows.Forms.LabelTS lblFilterHigh;
        private System.Windows.Forms.LabelTS lblFilterLow;
        private System.Windows.Forms.LabelTS lblMultiSMeter;
        private System.Windows.Forms.PictureBox picMultimeterAnalog;
        private System.Windows.Forms.PictureBox picMultiMeterDigital;
        private System.Windows.Forms.NumericUpDownTS udSquelch;
        private System.Windows.Forms.CheckBoxTS chkSquelch;
        private System.Windows.Forms.Timer timer_peak_text;
        private System.Windows.Forms.ButtonTS btnMemoryQuickSave;
        private System.Windows.Forms.ButtonTS btnMemoryQuickRecall;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TrackBarTS tbFilterShift;
        private System.Windows.Forms.LabelTS lblFilterShift;
        private System.Windows.Forms.ButtonTS btnFilterShiftReset;
        private System.Windows.Forms.Timer timer_clock;
        private System.Windows.Forms.Panel panelLOSCHover;
        private System.Windows.Forms.Panel panelVFOAHover;
        private System.Windows.Forms.Panel panelVFOBHover;
        private System.Windows.Forms.ComboBoxTS comboMeterRXMode;
        private System.Windows.Forms.ComboBoxTS comboMeterTXMode;
        private System.Windows.Forms.GroupBoxTS grpSoundControls;
        private System.Windows.Forms.GroupBoxTS grpOptions;
        private System.Windows.Forms.ButtonTS btnXITReset;
        private System.Windows.Forms.ButtonTS btnRITReset;
        private System.Windows.Forms.ComboBoxTS comboPreamp;
        private System.Windows.Forms.LabelTS lblPreamp;
        private System.Windows.Forms.CheckBoxTS chkDSPComp;
        private System.Windows.Forms.CheckBoxTS chkDSPNB2;
        private System.Windows.Forms.CheckBoxTS chkVFOLock;
        private System.Windows.Forms.TrackBarTS tbFilterWidth;
        private System.Windows.Forms.LabelTS lblFilterWidth;
        private System.Windows.Forms.Label lblCWSpeed;
        private System.Windows.Forms.NumericUpDownTS udCWSpeed;
        private System.Windows.Forms.ButtonTS btnIFtoVFO;
        private System.Windows.Forms.ButtonTS btnZeroBeat;
        private System.Windows.Forms.CheckBoxTS chkDSPCompander;
        private System.Windows.Forms.MenuItem mnuSetup;
        private System.Windows.Forms.MenuItem mnuWave;
        private System.Windows.Forms.MenuItem mnuAbout;
        private System.Windows.Forms.CheckBoxTS chkDisplayPeak;
        private System.Windows.Forms.MenuItem mnuEQ;
        private System.Windows.Forms.GroupBoxTS grpFilter;
        private System.Windows.Forms.RadioButtonTS radModeDIGU;
        private System.Windows.Forms.MenuItem mnuCWX;
        private System.Windows.Forms.RadioButtonTS radFilter1;
        private System.Windows.Forms.RadioButtonTS radFilter2;
        private System.Windows.Forms.RadioButtonTS radFilter3;
        private System.Windows.Forms.RadioButtonTS radFilter4;
        private System.Windows.Forms.RadioButtonTS radFilter5;
        private System.Windows.Forms.RadioButtonTS radFilter6;
        private System.Windows.Forms.RadioButtonTS radFilter7;
        private System.Windows.Forms.RadioButtonTS radFilter8;
        private System.Windows.Forms.RadioButtonTS radFilter9;
        private System.Windows.Forms.RadioButtonTS radFilter10;
        private System.Windows.Forms.ContextMenu contextMenuFilter;
        private System.Windows.Forms.MenuItem menuItemFilterConfigure;
        private System.Windows.Forms.LabelTS lblRF;
        private System.Windows.Forms.NumericUpDownTS udRF;
        private System.Windows.Forms.TrackBarTS tbAF;
        private System.Windows.Forms.TrackBarTS tbRF;
        private System.Windows.Forms.TrackBarTS tbPWR;
        private System.Windows.Forms.Label lblTuneStep;
        private System.Windows.Forms.GroupBoxTS grpVFOBetween;
        private System.Windows.Forms.TrackBarTS tbMIC;
        private System.Windows.Forms.GroupBoxTS grpModeSpecificPhone;
        private System.Windows.Forms.GroupBoxTS grpModeSpecificCW;
        private System.Windows.Forms.GroupBoxTS grpModeSpecificDigital;
        private System.Windows.Forms.CheckBoxTS chkVOX;
        private System.Windows.Forms.CheckBoxTS chkBreakIn;
        private System.Windows.Forms.TrackBarTS tbCWSpeed;
        private System.Windows.Forms.CheckBoxTS chkVACEnabled;
        public System.Windows.Forms.LabelTS lblTXGain;
        private System.Windows.Forms.NumericUpDownTS udVACTXGain;
        public System.Windows.Forms.LabelTS lblRXGain;
        private System.Windows.Forms.NumericUpDownTS udVACRXGain;
        private System.Windows.Forms.TrackBarTS tbVACRXGain;
        private System.Windows.Forms.TrackBarTS tbVACTXGain;
        private System.Windows.Forms.TrackBarTS tbSQL;
        private System.Windows.Forms.PictureBox picSQL;
        private System.Windows.Forms.PictureBox picVOX;
        private System.Windows.Forms.TrackBarTS tbVOX;
        private System.Windows.Forms.CheckBoxTS chkNoiseGate;
        private System.Windows.Forms.TrackBarTS tbNoiseGate;
        private System.Windows.Forms.PictureBox picNoiseGate;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBoxTS txtDisplayCursorOffset;
        private System.Windows.Forms.TextBoxTS txtDisplayCursorPower;
        private System.Windows.Forms.TextBoxTS txtDisplayCursorFreq;
        private System.Windows.Forms.TextBoxTS txtDisplayPeakOffset;
        private System.Windows.Forms.TextBoxTS txtDisplayPeakFreq;
        private System.Windows.Forms.TextBoxTS txtDisplayPeakPower;
        private System.Windows.Forms.Label lblVFOBLSD;
        private System.Windows.Forms.TextBoxTS txtLOSCMSD;
        private System.Windows.Forms.TextBoxTS txtVFOAMSD;
        private System.Windows.Forms.TextBoxTS txtVFOBMSD;
        private System.Windows.Forms.TextBoxTS txtLOSCLSD;
        private System.Windows.Forms.TextBoxTS txtVFOALSD;
        private System.Windows.Forms.TextBoxTS txtVFOBLSD;
        private System.Windows.Forms.NumericUpDownTS udCPDR;
        private System.Windows.Forms.TrackBarTS tbCPDR;
        private System.Windows.Forms.NumericUpDownTS udCOMP;
        private System.Windows.Forms.TrackBarTS tbCOMP;
        private System.Windows.Forms.CheckBoxTS chkSR;
        private System.Windows.Forms.ButtonTS btnTuneStepChangeSmaller;
        private System.Windows.Forms.ButtonTS btnChangeTuneStepLarger;
        private System.Windows.Forms.NumericUpDownTS udNoiseGate;
        private System.Windows.Forms.NumericUpDownTS udVOX;
        private System.Windows.Forms.ComboBoxTS comboTXProfile;
        private System.Windows.Forms.CheckBoxTS chkShowTXFilter;
        private System.Windows.Forms.MenuItem mnuFilterReset;
        private System.Windows.Forms.ComboBoxTS comboVACSampleRate;
        private System.Windows.Forms.GroupBoxTS grpDIGSampleRate;
        private System.Windows.Forms.GroupBoxTS grpVACStereo;
        private System.Windows.Forms.CheckBoxTS chkVACStereo;
        private System.Windows.Forms.CheckBoxTS chkCWDisableMonitor;
        private System.Windows.Forms.CheckBoxTS chkCWIambic;
        private System.Windows.Forms.GroupBoxTS grpCWPitch;
        private System.Windows.Forms.LabelTS lblCWPitchFreq;
        private System.Windows.Forms.NumericUpDownTS udCWPitch;
        private System.Windows.Forms.CheckBoxTS chkCWVAC;
        private System.Windows.Forms.LabelTS lblTransmitProfile;
        private System.Windows.Forms.CheckBoxTS checkBoxTS1;
        private System.Windows.Forms.CheckBoxTS chkShowTXCWFreq;
        private System.Windows.Forms.CheckBoxTS chkEnableSubRX;
        private System.Windows.Forms.TrackBarTS tbPanMainRX;
        private System.Windows.Forms.TrackBarTS tbPanSubRX;
        private System.Windows.Forms.GroupBoxTS grpSubRX;
        private System.Windows.Forms.CheckBoxTS chkPanSwap;
        private System.Windows.Forms.TrackBarTS tbRX1Gain;
        private System.Windows.Forms.TrackBarTS tbRX0Gain;
        private GroupBoxTS grpBandHF;
        private ButtonTS btnBandGEN;
        private ButtonTS btnBandWWV;
        private ButtonTS btnBandVHF;
        private ButtonTS btnBand2;
        private ButtonTS btnBand6;
        private ButtonTS btnBand10;
        private ButtonTS btnBand12;
        private ButtonTS btnBand15;
        private ButtonTS btnBand17;
        private ButtonTS btnBand20;
        private ButtonTS btnBand30;
        private ButtonTS btnBand40;
        private ButtonTS btnBand60;
        private ButtonTS btnBand80;
        private ButtonTS btnBand160;
        private Button btnHidden;
        private PictureBox picWaterfall;
        private GroupBox groupBox2;
        public CheckBox chkUSB;
        private TextBox textBox2;
        private ButtonTS btnDisplayZoom16x;
        private ButtonTS btnDisplayZoom48x;
        private TrackBar tbDisplayPan;
        private TrackBar tbDisplayZoom;
        private Label label1;
        private Label label2;
        private Button btnHIGH_AF;
        private Button btnHIGH_RF;
        private Button btnATT;
        private ButtonTS btnEraseMemory;
        private CheckBoxTS chkVFOsinc;
        public GroupBox grpG160;
        private Button btnG160_X2;
        private Button btnG160_X1;
        public GroupBox grpG59Option;
        public GroupBox grpG80;
        private Button btnG80_X4;
        private Button btnG80_X3;
        private Button btnG80_X2;
        private Button btnG80_X1;
        public GroupBox grp3020;
        public GroupBox grpG40;
        private Button btnG40_X1;
        private LabelTS lblMemoryNumber;
        private ContextMenuStrip contextMemoryMenu;
        private ToolStripMenuItem eraseAllMemoryToolStripMenuItem;
        private TextBoxTS txtMemory;
        private MenuItem mnuWizard;
        private ContextMenuStrip contextLOSCMenu;
        private ToolStripMenuItem xtal1ToolStripMenuItem;
        private Button btnG3020_X4;
        private Button btnG3020_X3;
        private Button btnG3020_X2;
        private Button btnG3020_X1;
        private ButtonTS btnVFOA;
        private System.ComponentModel.IContainer components;

        #endregion

        #region Constructor and Destructor
        // ======================================================
        // Constructor and Destructor
        // ======================================================

        public Console(string[] args)
        {
            //			HiPerfTimer t1 = new HiPerfTimer();
            //			Debug.WriteLine("timer_freq: "+t1.GetFreq());

            CmdLineArgs = args;

#if(INTERLEAVED)
#if(SPLIT_INTERLEAVED)
			callback1 = new PA19.PaStreamCallback(Audio.Callback1ILDI);	// Init callbacks to prevent GC
			callbackVAC = new PA19.PaStreamCallback(Audio.CallbackVACILDI);
			callback4port = new PA19.PaStreamCallback(Audio.Callback4PortILDI);
#else
			callback1 = new PA19.PaStreamCallback(Audio.Callback1IL);	// Init callbacks to prevent GC
			callbackVAC = new PA19.PaStreamCallback(Audio.CallbackVACIL);
			callback4port = new PA19.PaStreamCallback(Audio.Callback4PortIL);
#endif
#else
            callback1 = new PA19.PaStreamCallback(Audio.Callback1);	// Init callbacks to prevent GC
            callbackVAC = new PA19.PaStreamCallback(Audio.CallbackVAC);
            callback4port = new PA19.PaStreamCallback(Audio.Callback4Port);
#endif

            Splash.ShowSplashScreen();							// Start splash screen

            Splash.SetStatus("Initializing Components");		// Set progress point
            InitializeComponent();								// Windows Forms Generated Code

            Splash.SetStatus("Initializing Database");			// Set progress point
            DB.Init();											// Initialize the database

            Splash.SetStatus("Initializing DSP");				// Set progress point
            DttSP.Init();										// Initialize the DSP processor

            Refresh_RX_phase_gain();
            Refresh_TX_phase_gain();

            Splash.SetStatus("Initializing PortAudio");			// Set progress point
            PA19.PA_Initialize();								// Initialize the audio interface

            Splash.SetStatus("Loading Main Form");				// Set progress point
#if !DEBUG
            Splash.SplashForm.Owner = this;						// So that main form will show when splash disappears
#endif
            break_in_timer = new HiPerfTimer();

            Splash.SetStatus("Initializing USB communication");	// Set progress point

            InitConsole();										// Initialize all forms and main variables

            Splash.SetStatus("Finished");						// Set progress point
            // Activates double buffering
            SetStyle(ControlStyles.DoubleBuffer, true);

            Splash.CloseForm();									// End splash screen

            if (run_setup_wizard)
            {
                SetupWizard w = new SetupWizard(this, 0);
                w.ShowDialog();
            }

            if (multimeter_cal_offset == 0.0f)
            {
                switch (current_soundcard)
                {
                    case SoundCard.SANTA_CRUZ:
                        multimeter_cal_offset = -26.39952f;
                        break;
                    case SoundCard.AUDIGY_2_ZS:
                        multimeter_cal_offset = 1.024933f;
                        break;
                    case SoundCard.MP3_PLUS:
                        multimeter_cal_offset = -33.40224f;
                        break;
                    case SoundCard.EXTIGY:
                        multimeter_cal_offset = -29.30501f;
                        break;
                    case SoundCard.DELTA_44:
                        multimeter_cal_offset = -25.13887f;
                        break;
                    case SoundCard.FIREBOX:
                        multimeter_cal_offset = -20.94611f;
                        break;
                    case SoundCard.EDIROL_FA_66:
                        multimeter_cal_offset = -46.82864f;
                        break;
                    case SoundCard.UNSUPPORTED_CARD:
                        multimeter_cal_offset = -52.43533f;
                        break;
                }
            }

            if (display_cal_offset == 0.0f)
            {
                switch (current_soundcard)
                {
                    case SoundCard.SANTA_CRUZ:
                        DisplayCalOffset = -56.56675f;
                        break;
                    case SoundCard.AUDIGY_2_ZS:
                        DisplayCalOffset = -29.20928f;
                        break;
                    case SoundCard.MP3_PLUS:
                        DisplayCalOffset = -62.84578f;
                        break;
                    case SoundCard.EXTIGY:
                        DisplayCalOffset = -62.099f;
                        break;
                    case SoundCard.DELTA_44:
                        DisplayCalOffset = -57.467f;
                        break;
                    case SoundCard.FIREBOX:
                        DisplayCalOffset = -54.019f;
                        break;
                    case SoundCard.EDIROL_FA_66:
                        DisplayCalOffset = -80.429f;
                        break;
                    case SoundCard.UNSUPPORTED_CARD:
                        DisplayCalOffset = -82.62103f;
                        break;
                }
            }
            loscFreq = LOSCFreq;
            vfoAFreq = VFOAFreq;
            vfoBFreq = VFOBFreq;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
            ExitConsole();
        }

        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Console));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mnuSetup = new System.Windows.Forms.MenuItem();
            this.mnuWave = new System.Windows.Forms.MenuItem();
            this.mnuEQ = new System.Windows.Forms.MenuItem();
            this.mnuCWX = new System.Windows.Forms.MenuItem();
            this.mnuAbout = new System.Windows.Forms.MenuItem();
            this.mnuWizard = new System.Windows.Forms.MenuItem();
            this.contextMenuFilter = new System.Windows.Forms.ContextMenu();
            this.menuItemFilterConfigure = new System.Windows.Forms.MenuItem();
            this.mnuFilterReset = new System.Windows.Forms.MenuItem();
            this.timer_cpu_meter = new System.Windows.Forms.Timer(this.components);
            this.timer_peak_text = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tbDisplayPan = new System.Windows.Forms.TrackBar();
            this.tbDisplayZoom = new System.Windows.Forms.TrackBar();
            this.chkUSB = new System.Windows.Forms.CheckBox();
            this.btnG160_X1 = new System.Windows.Forms.Button();
            this.btnG160_X2 = new System.Windows.Forms.Button();
            this.btnHIGH_AF = new System.Windows.Forms.Button();
            this.btnHIGH_RF = new System.Windows.Forms.Button();
            this.btnATT = new System.Windows.Forms.Button();
            this.tbRX0Gain = new System.Windows.Forms.TrackBarTS();
            this.chkPanSwap = new System.Windows.Forms.CheckBoxTS();
            this.chkEnableSubRX = new System.Windows.Forms.CheckBoxTS();
            this.tbPanSubRX = new System.Windows.Forms.TrackBarTS();
            this.tbPanMainRX = new System.Windows.Forms.TrackBarTS();
            this.tbRX1Gain = new System.Windows.Forms.TrackBarTS();
            this.btnBandGEN = new System.Windows.Forms.ButtonTS();
            this.btnBandWWV = new System.Windows.Forms.ButtonTS();
            this.btnBandVHF = new System.Windows.Forms.ButtonTS();
            this.btnBand2 = new System.Windows.Forms.ButtonTS();
            this.btnBand6 = new System.Windows.Forms.ButtonTS();
            this.btnBand10 = new System.Windows.Forms.ButtonTS();
            this.btnBand12 = new System.Windows.Forms.ButtonTS();
            this.btnBand15 = new System.Windows.Forms.ButtonTS();
            this.btnBand17 = new System.Windows.Forms.ButtonTS();
            this.btnBand20 = new System.Windows.Forms.ButtonTS();
            this.btnBand30 = new System.Windows.Forms.ButtonTS();
            this.btnBand40 = new System.Windows.Forms.ButtonTS();
            this.btnBand60 = new System.Windows.Forms.ButtonTS();
            this.btnBand80 = new System.Windows.Forms.ButtonTS();
            this.btnBand160 = new System.Windows.Forms.ButtonTS();
            this.chkNoiseGate = new System.Windows.Forms.CheckBoxTS();
            this.chkVOX = new System.Windows.Forms.CheckBoxTS();
            this.chkDSPComp = new System.Windows.Forms.CheckBoxTS();
            this.chkDSPCompander = new System.Windows.Forms.CheckBoxTS();
            this.comboTXProfile = new System.Windows.Forms.ComboBoxTS();
            this.chkShowTXFilter = new System.Windows.Forms.CheckBoxTS();
            this.chkVACStereo = new System.Windows.Forms.CheckBoxTS();
            this.comboVACSampleRate = new System.Windows.Forms.ComboBoxTS();
            this.btnChangeTuneStepLarger = new System.Windows.Forms.ButtonTS();
            this.btnTuneStepChangeSmaller = new System.Windows.Forms.ButtonTS();
            this.chkVFOLock = new System.Windows.Forms.CheckBoxTS();
            this.txtWheelTune = new System.Windows.Forms.TextBoxTS();
            this.btnMemoryQuickRecall = new System.Windows.Forms.ButtonTS();
            this.btnMemoryQuickSave = new System.Windows.Forms.ButtonTS();
            this.chkDisplayPeak = new System.Windows.Forms.CheckBoxTS();
            this.comboDisplayMode = new System.Windows.Forms.ComboBoxTS();
            this.chkDisplayAVG = new System.Windows.Forms.CheckBoxTS();
            this.chkShowTXCWFreq = new System.Windows.Forms.CheckBoxTS();
            this.chkCWVAC = new System.Windows.Forms.CheckBoxTS();
            this.udCWPitch = new System.Windows.Forms.NumericUpDownTS();
            this.chkCWDisableMonitor = new System.Windows.Forms.CheckBoxTS();
            this.chkCWIambic = new System.Windows.Forms.CheckBoxTS();
            this.chkBreakIn = new System.Windows.Forms.CheckBoxTS();
            this.chkMUT = new System.Windows.Forms.CheckBoxTS();
            this.chkMON = new System.Windows.Forms.CheckBoxTS();
            this.chkTUN = new System.Windows.Forms.CheckBoxTS();
            this.chkMOX = new System.Windows.Forms.CheckBoxTS();
            this.checkBoxTS1 = new System.Windows.Forms.CheckBoxTS();
            this.radModeAM = new System.Windows.Forms.RadioButtonTS();
            this.radModeSAM = new System.Windows.Forms.RadioButtonTS();
            this.radModeDSB = new System.Windows.Forms.RadioButtonTS();
            this.radModeCWU = new System.Windows.Forms.RadioButtonTS();
            this.radModeDIGU = new System.Windows.Forms.RadioButtonTS();
            this.radModeDIGL = new System.Windows.Forms.RadioButtonTS();
            this.radModeLSB = new System.Windows.Forms.RadioButtonTS();
            this.radModeSPEC = new System.Windows.Forms.RadioButtonTS();
            this.radModeDRM = new System.Windows.Forms.RadioButtonTS();
            this.radModeFMN = new System.Windows.Forms.RadioButtonTS();
            this.radModeUSB = new System.Windows.Forms.RadioButtonTS();
            this.radModeCWL = new System.Windows.Forms.RadioButtonTS();
            this.udFilterHigh = new System.Windows.Forms.NumericUpDownTS();
            this.udFilterLow = new System.Windows.Forms.NumericUpDownTS();
            this.tbFilterWidth = new System.Windows.Forms.TrackBarTS();
            this.btnFilterShiftReset = new System.Windows.Forms.ButtonTS();
            this.tbFilterShift = new System.Windows.Forms.TrackBarTS();
            this.chkPower = new System.Windows.Forms.CheckBoxTS();
            this.chkSR = new System.Windows.Forms.CheckBoxTS();
            this.chkDSPNB2 = new System.Windows.Forms.CheckBoxTS();
            this.chkNB = new System.Windows.Forms.CheckBoxTS();
            this.chkANF = new System.Windows.Forms.CheckBoxTS();
            this.chkNR = new System.Windows.Forms.CheckBoxTS();
            this.chkBIN = new System.Windows.Forms.CheckBoxTS();
            this.comboAGC = new System.Windows.Forms.ComboBoxTS();
            this.comboPreamp = new System.Windows.Forms.ComboBoxTS();
            this.udXIT = new System.Windows.Forms.NumericUpDownTS();
            this.udRIT = new System.Windows.Forms.NumericUpDownTS();
            this.chkXIT = new System.Windows.Forms.CheckBoxTS();
            this.chkRIT = new System.Windows.Forms.CheckBoxTS();
            this.udPWR = new System.Windows.Forms.NumericUpDownTS();
            this.udAF = new System.Windows.Forms.NumericUpDownTS();
            this.comboMeterTXMode = new System.Windows.Forms.ComboBoxTS();
            this.comboMeterRXMode = new System.Windows.Forms.ComboBoxTS();
            this.btnZeroBeat = new System.Windows.Forms.ButtonTS();
            this.btnRITReset = new System.Windows.Forms.ButtonTS();
            this.btnXITReset = new System.Windows.Forms.ButtonTS();
            this.btnIFtoVFO = new System.Windows.Forms.ButtonTS();
            this.btnVFOSwap = new System.Windows.Forms.ButtonTS();
            this.btnVFOBtoA = new System.Windows.Forms.ButtonTS();
            this.btnVFOAtoB = new System.Windows.Forms.ButtonTS();
            this.chkVFOSplit = new System.Windows.Forms.CheckBoxTS();
            this.tbPWR = new System.Windows.Forms.TrackBarTS();
            this.tbRF = new System.Windows.Forms.TrackBarTS();
            this.tbAF = new System.Windows.Forms.TrackBarTS();
            this.udRF = new System.Windows.Forms.NumericUpDownTS();
            this.chkSquelch = new System.Windows.Forms.CheckBoxTS();
            this.udCWSpeed = new System.Windows.Forms.NumericUpDownTS();
            this.udVACTXGain = new System.Windows.Forms.NumericUpDownTS();
            this.udVACRXGain = new System.Windows.Forms.NumericUpDownTS();
            this.chkVACEnabled = new System.Windows.Forms.CheckBoxTS();
            this.chkVFOsinc = new System.Windows.Forms.CheckBoxTS();
            this.btnEraseMemory = new System.Windows.Forms.ButtonTS();
            this.btnVFOA = new System.Windows.Forms.ButtonTS();
            this.picSQL = new System.Windows.Forms.PictureBox();
            this.timer_clock = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDisplayZoom48x = new System.Windows.Forms.ButtonTS();
            this.btnDisplayZoom1x = new System.Windows.Forms.ButtonTS();
            this.btnDisplayZoom2x = new System.Windows.Forms.ButtonTS();
            this.btnDisplayZoom16x = new System.Windows.Forms.ButtonTS();
            this.btnDisplayZoom4x = new System.Windows.Forms.ButtonTS();
            this.btnDisplayZoom8x = new System.Windows.Forms.ButtonTS();
            this.grpG160 = new System.Windows.Forms.GroupBox();
            this.grpG59Option = new System.Windows.Forms.GroupBox();
            this.grpG80 = new System.Windows.Forms.GroupBox();
            this.btnG80_X4 = new System.Windows.Forms.Button();
            this.btnG80_X3 = new System.Windows.Forms.Button();
            this.btnG80_X2 = new System.Windows.Forms.Button();
            this.btnG80_X1 = new System.Windows.Forms.Button();
            this.grp3020 = new System.Windows.Forms.GroupBox();
            this.btnG3020_X4 = new System.Windows.Forms.Button();
            this.btnG3020_X3 = new System.Windows.Forms.Button();
            this.btnG3020_X2 = new System.Windows.Forms.Button();
            this.btnG3020_X1 = new System.Windows.Forms.Button();
            this.contextMemoryMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.eraseAllMemoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextLOSCMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.xtal1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grpLOSC = new System.Windows.Forms.GroupBoxTS();
            this.panelLOSCHover = new System.Windows.Forms.Panel();
            this.txtLOSCLSD = new System.Windows.Forms.TextBoxTS();
            this.txtLOSCMSD = new System.Windows.Forms.TextBoxTS();
            this.txtLOSCFreq = new System.Windows.Forms.TextBoxTS();
            this.grpSubRX = new System.Windows.Forms.GroupBoxTS();
            this.grpBandHF = new System.Windows.Forms.GroupBoxTS();
            this.grpModeSpecificPhone = new System.Windows.Forms.GroupBoxTS();
            this.udNoiseGate = new System.Windows.Forms.NumericUpDownTS();
            this.udVOX = new System.Windows.Forms.NumericUpDownTS();
            this.tbMIC = new System.Windows.Forms.TrackBarTS();
            this.udCPDR = new System.Windows.Forms.NumericUpDownTS();
            this.tbCPDR = new System.Windows.Forms.TrackBarTS();
            this.udCOMP = new System.Windows.Forms.NumericUpDownTS();
            this.tbCOMP = new System.Windows.Forms.TrackBarTS();
            this.picNoiseGate = new System.Windows.Forms.PictureBox();
            this.tbNoiseGate = new System.Windows.Forms.TrackBarTS();
            this.picVOX = new System.Windows.Forms.PictureBox();
            this.tbVOX = new System.Windows.Forms.TrackBarTS();
            this.udMIC = new System.Windows.Forms.NumericUpDownTS();
            this.lblMIC = new System.Windows.Forms.LabelTS();
            this.lblTransmitProfile = new System.Windows.Forms.LabelTS();
            this.tbSQL = new System.Windows.Forms.TrackBarTS();
            this.grpModeSpecificDigital = new System.Windows.Forms.GroupBoxTS();
            this.grpVACStereo = new System.Windows.Forms.GroupBoxTS();
            this.grpDIGSampleRate = new System.Windows.Forms.GroupBoxTS();
            this.tbVACTXGain = new System.Windows.Forms.TrackBarTS();
            this.tbVACRXGain = new System.Windows.Forms.TrackBarTS();
            this.lblTXGain = new System.Windows.Forms.LabelTS();
            this.lblRXGain = new System.Windows.Forms.LabelTS();
            this.grpG40 = new System.Windows.Forms.GroupBox();
            this.btnG40_X1 = new System.Windows.Forms.Button();
            this.lblMemoryNumber = new System.Windows.Forms.LabelTS();
            this.grpVFOBetween = new System.Windows.Forms.GroupBoxTS();
            this.txtMemory = new System.Windows.Forms.TextBoxTS();
            this.lblTuneStep = new System.Windows.Forms.Label();
            this.grpDisplay2 = new System.Windows.Forms.GroupBoxTS();
            this.grpModeSpecificCW = new System.Windows.Forms.GroupBoxTS();
            this.grpCWPitch = new System.Windows.Forms.GroupBoxTS();
            this.lblCWPitchFreq = new System.Windows.Forms.LabelTS();
            this.tbCWSpeed = new System.Windows.Forms.TrackBarTS();
            this.lblCWSpeed = new System.Windows.Forms.Label();
            this.grpOptions = new System.Windows.Forms.GroupBoxTS();
            this.txtVFOAFreq = new System.Windows.Forms.TextBoxTS();
            this.grpVFOA = new System.Windows.Forms.GroupBoxTS();
            this.btnHidden = new System.Windows.Forms.Button();
            this.panelVFOAHover = new System.Windows.Forms.Panel();
            this.txtVFOALSD = new System.Windows.Forms.TextBoxTS();
            this.txtVFOAMSD = new System.Windows.Forms.TextBoxTS();
            this.txtVFOABand = new System.Windows.Forms.TextBoxTS();
            this.grpVFOB = new System.Windows.Forms.GroupBoxTS();
            this.txtVFOBLSD = new System.Windows.Forms.TextBoxTS();
            this.panelVFOBHover = new System.Windows.Forms.Panel();
            this.txtVFOBMSD = new System.Windows.Forms.TextBoxTS();
            this.lblVFOBLSD = new System.Windows.Forms.Label();
            this.txtVFOBBand = new System.Windows.Forms.TextBoxTS();
            this.txtVFOBFreq = new System.Windows.Forms.TextBoxTS();
            this.grpDisplay = new System.Windows.Forms.GroupBoxTS();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.picWaterfall = new System.Windows.Forms.PictureBox();
            this.txtDisplayPeakFreq = new System.Windows.Forms.TextBoxTS();
            this.txtDisplayCursorFreq = new System.Windows.Forms.TextBoxTS();
            this.txtDisplayCursorPower = new System.Windows.Forms.TextBoxTS();
            this.txtDisplayPeakPower = new System.Windows.Forms.TextBoxTS();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtDisplayCursorOffset = new System.Windows.Forms.TextBoxTS();
            this.txtDisplayPeakOffset = new System.Windows.Forms.TextBoxTS();
            this.picDisplay = new System.Windows.Forms.PictureBox();
            this.grpMode = new System.Windows.Forms.GroupBoxTS();
            this.grpFilter = new System.Windows.Forms.GroupBoxTS();
            this.lblFilterWidth = new System.Windows.Forms.LabelTS();
            this.lblFilterShift = new System.Windows.Forms.LabelTS();
            this.radFilter1 = new System.Windows.Forms.RadioButtonTS();
            this.radFilter2 = new System.Windows.Forms.RadioButtonTS();
            this.radFilter3 = new System.Windows.Forms.RadioButtonTS();
            this.radFilter4 = new System.Windows.Forms.RadioButtonTS();
            this.radFilter5 = new System.Windows.Forms.RadioButtonTS();
            this.radFilter6 = new System.Windows.Forms.RadioButtonTS();
            this.radFilter7 = new System.Windows.Forms.RadioButtonTS();
            this.radFilter8 = new System.Windows.Forms.RadioButtonTS();
            this.radFilter9 = new System.Windows.Forms.RadioButtonTS();
            this.radFilter10 = new System.Windows.Forms.RadioButtonTS();
            this.radFilterVar1 = new System.Windows.Forms.RadioButtonTS();
            this.radFilterVar2 = new System.Windows.Forms.RadioButtonTS();
            this.lblFilterLow = new System.Windows.Forms.LabelTS();
            this.lblFilterHigh = new System.Windows.Forms.LabelTS();
            this.lblCPUMeter = new System.Windows.Forms.LabelTS();
            this.grpDSP = new System.Windows.Forms.GroupBoxTS();
            this.lblAGC = new System.Windows.Forms.LabelTS();
            this.lblPreamp = new System.Windows.Forms.LabelTS();
            this.lblPWR = new System.Windows.Forms.LabelTS();
            this.lblAF = new System.Windows.Forms.LabelTS();
            this.grpMultimeter = new System.Windows.Forms.GroupBoxTS();
            this.picMultiMeterDigital = new System.Windows.Forms.PictureBox();
            this.lblMultiSMeter = new System.Windows.Forms.LabelTS();
            this.txtMultiText = new System.Windows.Forms.TextBoxTS();
            this.picMultimeterAnalog = new System.Windows.Forms.PictureBox();
            this.grpVFO = new System.Windows.Forms.GroupBoxTS();
            this.grpSoundControls = new System.Windows.Forms.GroupBoxTS();
            this.lblRF = new System.Windows.Forms.LabelTS();
            this.udSquelch = new System.Windows.Forms.NumericUpDownTS();
            ((System.ComponentModel.ISupportInitialize)(this.tbDisplayPan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDisplayZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRX0Gain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPanSubRX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPanMainRX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRX1Gain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udCWPitch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFilterHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFilterLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbFilterWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbFilterShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udXIT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udRIT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPWR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udAF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPWR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbAF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udRF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udCWSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udVACTXGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udVACRXGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSQL)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.grpG160.SuspendLayout();
            this.grpG59Option.SuspendLayout();
            this.grpG80.SuspendLayout();
            this.grp3020.SuspendLayout();
            this.contextMemoryMenu.SuspendLayout();
            this.contextLOSCMenu.SuspendLayout();
            this.grpLOSC.SuspendLayout();
            this.grpSubRX.SuspendLayout();
            this.grpBandHF.SuspendLayout();
            this.grpModeSpecificPhone.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udNoiseGate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udVOX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMIC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udCPDR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbCPDR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udCOMP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbCOMP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNoiseGate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbNoiseGate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVOX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbVOX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMIC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSQL)).BeginInit();
            this.grpModeSpecificDigital.SuspendLayout();
            this.grpVACStereo.SuspendLayout();
            this.grpDIGSampleRate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbVACTXGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbVACRXGain)).BeginInit();
            this.grpG40.SuspendLayout();
            this.grpVFOBetween.SuspendLayout();
            this.grpDisplay2.SuspendLayout();
            this.grpModeSpecificCW.SuspendLayout();
            this.grpCWPitch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbCWSpeed)).BeginInit();
            this.grpOptions.SuspendLayout();
            this.grpVFOA.SuspendLayout();
            this.grpVFOB.SuspendLayout();
            this.grpDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWaterfall)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).BeginInit();
            this.grpMode.SuspendLayout();
            this.grpFilter.SuspendLayout();
            this.grpDSP.SuspendLayout();
            this.grpMultimeter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMultiMeterDigital)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMultimeterAnalog)).BeginInit();
            this.grpVFO.SuspendLayout();
            this.grpSoundControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udSquelch)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuSetup,
            this.mnuWave,
            this.mnuEQ,
            this.mnuCWX,
            this.mnuAbout,
            this.mnuWizard});
            resources.ApplyResources(this.mainMenu1, "mainMenu1");
            // 
            // mnuSetup
            // 
            this.mnuSetup.Index = 0;
            resources.ApplyResources(this.mnuSetup, "mnuSetup");
            this.mnuSetup.Click += new System.EventHandler(this.menu_setup_Click);
            // 
            // mnuWave
            // 
            this.mnuWave.Index = 1;
            resources.ApplyResources(this.mnuWave, "mnuWave");
            this.mnuWave.Click += new System.EventHandler(this.menu_wave_Click);
            // 
            // mnuEQ
            // 
            this.mnuEQ.Index = 2;
            resources.ApplyResources(this.mnuEQ, "mnuEQ");
            this.mnuEQ.Click += new System.EventHandler(this.mnuEQ_Click);
            // 
            // mnuCWX
            // 
            this.mnuCWX.Index = 3;
            resources.ApplyResources(this.mnuCWX, "mnuCWX");
            this.mnuCWX.Click += new System.EventHandler(this.mnuCWX_Click);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Index = 4;
            resources.ApplyResources(this.mnuAbout, "mnuAbout");
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // mnuWizard
            // 
            this.mnuWizard.Index = 5;
            resources.ApplyResources(this.mnuWizard, "mnuWizard");
            this.mnuWizard.Click += new System.EventHandler(this.mnuWizard_Click);
            // 
            // contextMenuFilter
            // 
            this.contextMenuFilter.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFilterConfigure,
            this.mnuFilterReset});
            // 
            // menuItemFilterConfigure
            // 
            this.menuItemFilterConfigure.Index = 0;
            resources.ApplyResources(this.menuItemFilterConfigure, "menuItemFilterConfigure");
            this.menuItemFilterConfigure.Click += new System.EventHandler(this.menuItemFilterConfigure_Click);
            // 
            // mnuFilterReset
            // 
            this.mnuFilterReset.Index = 1;
            resources.ApplyResources(this.mnuFilterReset, "mnuFilterReset");
            this.mnuFilterReset.Click += new System.EventHandler(this.mnuFilterReset_Click);
            // 
            // timer_cpu_meter
            // 
            this.timer_cpu_meter.Enabled = true;
            this.timer_cpu_meter.Interval = 1000;
            this.timer_cpu_meter.Tick += new System.EventHandler(this.timer_cpu_meter_Tick);
            // 
            // timer_peak_text
            // 
            this.timer_peak_text.Interval = 500;
            this.timer_peak_text.Tick += new System.EventHandler(this.timer_peak_text_Tick);
            // 
            // tbDisplayPan
            // 
            resources.ApplyResources(this.tbDisplayPan, "tbDisplayPan");
            this.tbDisplayPan.Maximum = 1000;
            this.tbDisplayPan.Minimum = -1000;
            this.tbDisplayPan.Name = "tbDisplayPan";
            this.tbDisplayPan.TickStyle = System.Windows.Forms.TickStyle.None;
            this.toolTip1.SetToolTip(this.tbDisplayPan, resources.GetString("tbDisplayPan.ToolTip"));
            this.tbDisplayPan.Scroll += new System.EventHandler(this.tbDisplayPan_Scroll);
            this.tbDisplayPan.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tbDisplayPan_MouseMove);
            // 
            // tbDisplayZoom
            // 
            resources.ApplyResources(this.tbDisplayZoom, "tbDisplayZoom");
            this.tbDisplayZoom.Maximum = 128;
            this.tbDisplayZoom.Minimum = 4;
            this.tbDisplayZoom.Name = "tbDisplayZoom";
            this.tbDisplayZoom.TickStyle = System.Windows.Forms.TickStyle.None;
            this.toolTip1.SetToolTip(this.tbDisplayZoom, resources.GetString("tbDisplayZoom.ToolTip"));
            this.tbDisplayZoom.Value = 4;
            this.tbDisplayZoom.Scroll += new System.EventHandler(this.tbDisplayZoom_Scroll);
            this.tbDisplayZoom.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tbDisplayZoom_MouseMove);
            // 
            // chkUSB
            // 
            resources.ApplyResources(this.chkUSB, "chkUSB");
            this.chkUSB.Name = "chkUSB";
            this.toolTip1.SetToolTip(this.chkUSB, resources.GetString("chkUSB.ToolTip"));
            this.chkUSB.UseVisualStyleBackColor = true;
            this.chkUSB.Click += new System.EventHandler(this.chkUSB_Click);
            // 
            // btnG160_X1
            // 
            resources.ApplyResources(this.btnG160_X1, "btnG160_X1");
            this.btnG160_X1.Name = "btnG160_X1";
            this.toolTip1.SetToolTip(this.btnG160_X1, resources.GetString("btnG160_X1.ToolTip"));
            this.btnG160_X1.UseVisualStyleBackColor = true;
            this.btnG160_X1.Click += new System.EventHandler(this.btnG160_X1_Click);
            // 
            // btnG160_X2
            // 
            resources.ApplyResources(this.btnG160_X2, "btnG160_X2");
            this.btnG160_X2.Name = "btnG160_X2";
            this.toolTip1.SetToolTip(this.btnG160_X2, resources.GetString("btnG160_X2.ToolTip"));
            this.btnG160_X2.UseVisualStyleBackColor = true;
            this.btnG160_X2.Click += new System.EventHandler(this.btnG160_X2_Click);
            // 
            // btnHIGH_AF
            // 
            resources.ApplyResources(this.btnHIGH_AF, "btnHIGH_AF");
            this.btnHIGH_AF.Name = "btnHIGH_AF";
            this.toolTip1.SetToolTip(this.btnHIGH_AF, resources.GetString("btnHIGH_AF.ToolTip"));
            this.btnHIGH_AF.UseVisualStyleBackColor = true;
            this.btnHIGH_AF.Click += new System.EventHandler(this.btnHIGH_AF_Click);
            // 
            // btnHIGH_RF
            // 
            resources.ApplyResources(this.btnHIGH_RF, "btnHIGH_RF");
            this.btnHIGH_RF.Name = "btnHIGH_RF";
            this.toolTip1.SetToolTip(this.btnHIGH_RF, resources.GetString("btnHIGH_RF.ToolTip"));
            this.btnHIGH_RF.UseVisualStyleBackColor = true;
            this.btnHIGH_RF.Click += new System.EventHandler(this.btnHIGH_RF_Click);
            // 
            // btnATT
            // 
            resources.ApplyResources(this.btnATT, "btnATT");
            this.btnATT.Name = "btnATT";
            this.toolTip1.SetToolTip(this.btnATT, resources.GetString("btnATT.ToolTip"));
            this.btnATT.UseVisualStyleBackColor = true;
            this.btnATT.Click += new System.EventHandler(this.btnATT_Click);
            // 
            // tbRX0Gain
            // 
            resources.ApplyResources(this.tbRX0Gain, "tbRX0Gain");
            this.tbRX0Gain.Maximum = 100;
            this.tbRX0Gain.Name = "tbRX0Gain";
            this.tbRX0Gain.TickStyle = System.Windows.Forms.TickStyle.None;
            this.toolTip1.SetToolTip(this.tbRX0Gain, resources.GetString("tbRX0Gain.ToolTip"));
            this.tbRX0Gain.Value = 100;
            this.tbRX0Gain.Scroll += new System.EventHandler(this.tbRX0Gain_Scroll);
            // 
            // chkPanSwap
            // 
            resources.ApplyResources(this.chkPanSwap, "chkPanSwap");
            this.chkPanSwap.Image = null;
            this.chkPanSwap.Name = "chkPanSwap";
            this.toolTip1.SetToolTip(this.chkPanSwap, resources.GetString("chkPanSwap.ToolTip"));
            this.chkPanSwap.CheckedChanged += new System.EventHandler(this.chkPanSwap_CheckedChanged);
            // 
            // chkEnableSubRX
            // 
            resources.ApplyResources(this.chkEnableSubRX, "chkEnableSubRX");
            this.chkEnableSubRX.Image = null;
            this.chkEnableSubRX.Name = "chkEnableSubRX";
            this.toolTip1.SetToolTip(this.chkEnableSubRX, resources.GetString("chkEnableSubRX.ToolTip"));
            this.chkEnableSubRX.CheckedChanged += new System.EventHandler(this.chkEnableSubRX_CheckedChanged);
            // 
            // tbPanSubRX
            // 
            resources.ApplyResources(this.tbPanSubRX, "tbPanSubRX");
            this.tbPanSubRX.Maximum = 100;
            this.tbPanSubRX.Name = "tbPanSubRX";
            this.tbPanSubRX.TickFrequency = 25;
            this.toolTip1.SetToolTip(this.tbPanSubRX, resources.GetString("tbPanSubRX.ToolTip"));
            this.tbPanSubRX.Value = 100;
            this.tbPanSubRX.Scroll += new System.EventHandler(this.tbPanSubRX_Scroll);
            // 
            // tbPanMainRX
            // 
            resources.ApplyResources(this.tbPanMainRX, "tbPanMainRX");
            this.tbPanMainRX.Maximum = 100;
            this.tbPanMainRX.Name = "tbPanMainRX";
            this.tbPanMainRX.TickFrequency = 25;
            this.toolTip1.SetToolTip(this.tbPanMainRX, resources.GetString("tbPanMainRX.ToolTip"));
            this.tbPanMainRX.Scroll += new System.EventHandler(this.tbPanMainRX_Scroll);
            // 
            // tbRX1Gain
            // 
            resources.ApplyResources(this.tbRX1Gain, "tbRX1Gain");
            this.tbRX1Gain.Maximum = 100;
            this.tbRX1Gain.Name = "tbRX1Gain";
            this.tbRX1Gain.TickStyle = System.Windows.Forms.TickStyle.None;
            this.toolTip1.SetToolTip(this.tbRX1Gain, resources.GetString("tbRX1Gain.ToolTip"));
            this.tbRX1Gain.Value = 100;
            this.tbRX1Gain.Scroll += new System.EventHandler(this.tbRX1Gain_Scroll);
            // 
            // btnBandGEN
            // 
            resources.ApplyResources(this.btnBandGEN, "btnBandGEN");
            this.btnBandGEN.Image = null;
            this.btnBandGEN.Name = "btnBandGEN";
            this.toolTip1.SetToolTip(this.btnBandGEN, resources.GetString("btnBandGEN.ToolTip"));
            this.btnBandGEN.Click += new System.EventHandler(this.btnBandGEN_Click);
            // 
            // btnBandWWV
            // 
            resources.ApplyResources(this.btnBandWWV, "btnBandWWV");
            this.btnBandWWV.Image = null;
            this.btnBandWWV.Name = "btnBandWWV";
            this.toolTip1.SetToolTip(this.btnBandWWV, resources.GetString("btnBandWWV.ToolTip"));
            this.btnBandWWV.Click += new System.EventHandler(this.btnBandWWV_Click);
            // 
            // btnBandVHF
            // 
            resources.ApplyResources(this.btnBandVHF, "btnBandVHF");
            this.btnBandVHF.Image = null;
            this.btnBandVHF.Name = "btnBandVHF";
            this.toolTip1.SetToolTip(this.btnBandVHF, resources.GetString("btnBandVHF.ToolTip"));
            this.btnBandVHF.Click += new System.EventHandler(this.btnBandVHF_Click);
            // 
            // btnBand2
            // 
            resources.ApplyResources(this.btnBand2, "btnBand2");
            this.btnBand2.Image = null;
            this.btnBand2.Name = "btnBand2";
            this.toolTip1.SetToolTip(this.btnBand2, resources.GetString("btnBand2.ToolTip"));
            this.btnBand2.Click += new System.EventHandler(this.btnBand2_Click);
            // 
            // btnBand6
            // 
            resources.ApplyResources(this.btnBand6, "btnBand6");
            this.btnBand6.Image = null;
            this.btnBand6.Name = "btnBand6";
            this.toolTip1.SetToolTip(this.btnBand6, resources.GetString("btnBand6.ToolTip"));
            this.btnBand6.Click += new System.EventHandler(this.btnBand6_Click);
            // 
            // btnBand10
            // 
            resources.ApplyResources(this.btnBand10, "btnBand10");
            this.btnBand10.Image = null;
            this.btnBand10.Name = "btnBand10";
            this.toolTip1.SetToolTip(this.btnBand10, resources.GetString("btnBand10.ToolTip"));
            this.btnBand10.Click += new System.EventHandler(this.btnBand10_Click);
            // 
            // btnBand12
            // 
            resources.ApplyResources(this.btnBand12, "btnBand12");
            this.btnBand12.Image = null;
            this.btnBand12.Name = "btnBand12";
            this.toolTip1.SetToolTip(this.btnBand12, resources.GetString("btnBand12.ToolTip"));
            this.btnBand12.Click += new System.EventHandler(this.btnBand12_Click);
            // 
            // btnBand15
            // 
            resources.ApplyResources(this.btnBand15, "btnBand15");
            this.btnBand15.Image = null;
            this.btnBand15.Name = "btnBand15";
            this.toolTip1.SetToolTip(this.btnBand15, resources.GetString("btnBand15.ToolTip"));
            this.btnBand15.Click += new System.EventHandler(this.btnBand15_Click);
            // 
            // btnBand17
            // 
            resources.ApplyResources(this.btnBand17, "btnBand17");
            this.btnBand17.Image = null;
            this.btnBand17.Name = "btnBand17";
            this.toolTip1.SetToolTip(this.btnBand17, resources.GetString("btnBand17.ToolTip"));
            this.btnBand17.Click += new System.EventHandler(this.btnBand17_Click);
            // 
            // btnBand20
            // 
            resources.ApplyResources(this.btnBand20, "btnBand20");
            this.btnBand20.Image = null;
            this.btnBand20.Name = "btnBand20";
            this.toolTip1.SetToolTip(this.btnBand20, resources.GetString("btnBand20.ToolTip"));
            this.btnBand20.Click += new System.EventHandler(this.btnBand20_Click);
            // 
            // btnBand30
            // 
            resources.ApplyResources(this.btnBand30, "btnBand30");
            this.btnBand30.Image = null;
            this.btnBand30.Name = "btnBand30";
            this.toolTip1.SetToolTip(this.btnBand30, resources.GetString("btnBand30.ToolTip"));
            this.btnBand30.Click += new System.EventHandler(this.btnBand30_Click);
            // 
            // btnBand40
            // 
            resources.ApplyResources(this.btnBand40, "btnBand40");
            this.btnBand40.Image = null;
            this.btnBand40.Name = "btnBand40";
            this.toolTip1.SetToolTip(this.btnBand40, resources.GetString("btnBand40.ToolTip"));
            this.btnBand40.Click += new System.EventHandler(this.btnBand40_Click);
            // 
            // btnBand60
            // 
            resources.ApplyResources(this.btnBand60, "btnBand60");
            this.btnBand60.Image = null;
            this.btnBand60.Name = "btnBand60";
            this.toolTip1.SetToolTip(this.btnBand60, resources.GetString("btnBand60.ToolTip"));
            this.btnBand60.Click += new System.EventHandler(this.btnBand60_Click);
            // 
            // btnBand80
            // 
            resources.ApplyResources(this.btnBand80, "btnBand80");
            this.btnBand80.Image = null;
            this.btnBand80.Name = "btnBand80";
            this.toolTip1.SetToolTip(this.btnBand80, resources.GetString("btnBand80.ToolTip"));
            this.btnBand80.Click += new System.EventHandler(this.btnBand80_Click);
            // 
            // btnBand160
            // 
            resources.ApplyResources(this.btnBand160, "btnBand160");
            this.btnBand160.Image = null;
            this.btnBand160.Name = "btnBand160";
            this.toolTip1.SetToolTip(this.btnBand160, resources.GetString("btnBand160.ToolTip"));
            this.btnBand160.Click += new System.EventHandler(this.btnBand160_Click);
            // 
            // chkNoiseGate
            // 
            resources.ApplyResources(this.chkNoiseGate, "chkNoiseGate");
            this.chkNoiseGate.Image = null;
            this.chkNoiseGate.Name = "chkNoiseGate";
            this.toolTip1.SetToolTip(this.chkNoiseGate, resources.GetString("chkNoiseGate.ToolTip"));
            this.chkNoiseGate.CheckedChanged += new System.EventHandler(this.chkNoiseGate_CheckedChanged);
            // 
            // chkVOX
            // 
            resources.ApplyResources(this.chkVOX, "chkVOX");
            this.chkVOX.Image = null;
            this.chkVOX.Name = "chkVOX";
            this.toolTip1.SetToolTip(this.chkVOX, resources.GetString("chkVOX.ToolTip"));
            this.chkVOX.CheckedChanged += new System.EventHandler(this.chkVOX_CheckedChanged);
            // 
            // chkDSPComp
            // 
            resources.ApplyResources(this.chkDSPComp, "chkDSPComp");
            this.chkDSPComp.Image = null;
            this.chkDSPComp.Name = "chkDSPComp";
            this.toolTip1.SetToolTip(this.chkDSPComp, resources.GetString("chkDSPComp.ToolTip"));
            this.chkDSPComp.CheckedChanged += new System.EventHandler(this.chkDSPComp_CheckedChanged);
            // 
            // chkDSPCompander
            // 
            resources.ApplyResources(this.chkDSPCompander, "chkDSPCompander");
            this.chkDSPCompander.Image = null;
            this.chkDSPCompander.Name = "chkDSPCompander";
            this.toolTip1.SetToolTip(this.chkDSPCompander, resources.GetString("chkDSPCompander.ToolTip"));
            this.chkDSPCompander.CheckedChanged += new System.EventHandler(this.chkDSPCompander_CheckedChanged);
            // 
            // comboTXProfile
            // 
            this.comboTXProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTXProfile.DropDownWidth = 96;
            resources.ApplyResources(this.comboTXProfile, "comboTXProfile");
            this.comboTXProfile.Name = "comboTXProfile";
            this.toolTip1.SetToolTip(this.comboTXProfile, resources.GetString("comboTXProfile.ToolTip"));
            this.comboTXProfile.SelectedIndexChanged += new System.EventHandler(this.comboTXProfile_SelectedIndexChanged);
            // 
            // chkShowTXFilter
            // 
            this.chkShowTXFilter.Image = null;
            resources.ApplyResources(this.chkShowTXFilter, "chkShowTXFilter");
            this.chkShowTXFilter.Name = "chkShowTXFilter";
            this.toolTip1.SetToolTip(this.chkShowTXFilter, resources.GetString("chkShowTXFilter.ToolTip"));
            this.chkShowTXFilter.CheckedChanged += new System.EventHandler(this.chkShowTXFilter_CheckedChanged);
            // 
            // chkVACStereo
            // 
            this.chkVACStereo.Image = null;
            resources.ApplyResources(this.chkVACStereo, "chkVACStereo");
            this.chkVACStereo.Name = "chkVACStereo";
            this.toolTip1.SetToolTip(this.chkVACStereo, resources.GetString("chkVACStereo.ToolTip"));
            this.chkVACStereo.CheckedChanged += new System.EventHandler(this.chkVACStereo_CheckedChanged);
            // 
            // comboVACSampleRate
            // 
            this.comboVACSampleRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboVACSampleRate.DropDownWidth = 64;
            resources.ApplyResources(this.comboVACSampleRate, "comboVACSampleRate");
            this.comboVACSampleRate.Items.AddRange(new object[] {
            resources.GetString("comboVACSampleRate.Items"),
            resources.GetString("comboVACSampleRate.Items1"),
            resources.GetString("comboVACSampleRate.Items2"),
            resources.GetString("comboVACSampleRate.Items3"),
            resources.GetString("comboVACSampleRate.Items4"),
            resources.GetString("comboVACSampleRate.Items5"),
            resources.GetString("comboVACSampleRate.Items6"),
            resources.GetString("comboVACSampleRate.Items7")});
            this.comboVACSampleRate.Name = "comboVACSampleRate";
            this.toolTip1.SetToolTip(this.comboVACSampleRate, resources.GetString("comboVACSampleRate.ToolTip"));
            this.comboVACSampleRate.SelectedIndexChanged += new System.EventHandler(this.comboVACSampleRate_SelectedIndexChanged);
            // 
            // btnChangeTuneStepLarger
            // 
            resources.ApplyResources(this.btnChangeTuneStepLarger, "btnChangeTuneStepLarger");
            this.btnChangeTuneStepLarger.Image = null;
            this.btnChangeTuneStepLarger.Name = "btnChangeTuneStepLarger";
            this.toolTip1.SetToolTip(this.btnChangeTuneStepLarger, resources.GetString("btnChangeTuneStepLarger.ToolTip"));
            this.btnChangeTuneStepLarger.Click += new System.EventHandler(this.btnChangeTuneStepLarger_Click);
            // 
            // btnTuneStepChangeSmaller
            // 
            resources.ApplyResources(this.btnTuneStepChangeSmaller, "btnTuneStepChangeSmaller");
            this.btnTuneStepChangeSmaller.Image = null;
            this.btnTuneStepChangeSmaller.Name = "btnTuneStepChangeSmaller";
            this.toolTip1.SetToolTip(this.btnTuneStepChangeSmaller, resources.GetString("btnTuneStepChangeSmaller.ToolTip"));
            this.btnTuneStepChangeSmaller.Click += new System.EventHandler(this.btnChangeTuneStepSmaller_Click);
            // 
            // chkVFOLock
            // 
            resources.ApplyResources(this.chkVFOLock, "chkVFOLock");
            this.chkVFOLock.Image = null;
            this.chkVFOLock.Name = "chkVFOLock";
            this.toolTip1.SetToolTip(this.chkVFOLock, resources.GetString("chkVFOLock.ToolTip"));
            this.chkVFOLock.CheckedChanged += new System.EventHandler(this.chkVFOLock_CheckedChanged);
            // 
            // txtWheelTune
            // 
            this.txtWheelTune.BackColor = System.Drawing.SystemColors.Window;
            this.txtWheelTune.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtWheelTune, "txtWheelTune");
            this.txtWheelTune.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtWheelTune.Name = "txtWheelTune";
            this.txtWheelTune.ReadOnly = true;
            this.toolTip1.SetToolTip(this.txtWheelTune, resources.GetString("txtWheelTune.ToolTip"));
            this.txtWheelTune.GotFocus += new System.EventHandler(this.HideFocus);
            this.txtWheelTune.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WheelTune_MouseDown);
            // 
            // btnMemoryQuickRecall
            // 
            this.btnMemoryQuickRecall.Image = null;
            resources.ApplyResources(this.btnMemoryQuickRecall, "btnMemoryQuickRecall");
            this.btnMemoryQuickRecall.Name = "btnMemoryQuickRecall";
            this.toolTip1.SetToolTip(this.btnMemoryQuickRecall, resources.GetString("btnMemoryQuickRecall.ToolTip"));
            this.btnMemoryQuickRecall.Click += new System.EventHandler(this.btnMemoryQuickRestore_Click);
            // 
            // btnMemoryQuickSave
            // 
            this.btnMemoryQuickSave.Image = null;
            resources.ApplyResources(this.btnMemoryQuickSave, "btnMemoryQuickSave");
            this.btnMemoryQuickSave.Name = "btnMemoryQuickSave";
            this.toolTip1.SetToolTip(this.btnMemoryQuickSave, resources.GetString("btnMemoryQuickSave.ToolTip"));
            this.btnMemoryQuickSave.Click += new System.EventHandler(this.btnMemoryQuickSave_Click);
            // 
            // chkDisplayPeak
            // 
            resources.ApplyResources(this.chkDisplayPeak, "chkDisplayPeak");
            this.chkDisplayPeak.Image = null;
            this.chkDisplayPeak.Name = "chkDisplayPeak";
            this.toolTip1.SetToolTip(this.chkDisplayPeak, resources.GetString("chkDisplayPeak.ToolTip"));
            this.chkDisplayPeak.CheckedChanged += new System.EventHandler(this.chkDisplayPeak_CheckedChanged);
            // 
            // comboDisplayMode
            // 
            this.comboDisplayMode.BackColor = System.Drawing.SystemColors.Window;
            this.comboDisplayMode.DisplayMember = "0";
            this.comboDisplayMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDisplayMode.DropDownWidth = 88;
            this.comboDisplayMode.ForeColor = System.Drawing.SystemColors.WindowText;
            resources.ApplyResources(this.comboDisplayMode, "comboDisplayMode");
            this.comboDisplayMode.Name = "comboDisplayMode";
            this.toolTip1.SetToolTip(this.comboDisplayMode, resources.GetString("comboDisplayMode.ToolTip"));
            this.comboDisplayMode.SelectedIndexChanged += new System.EventHandler(this.comboDisplayMode_SelectedIndexChanged);
            // 
            // chkDisplayAVG
            // 
            resources.ApplyResources(this.chkDisplayAVG, "chkDisplayAVG");
            this.chkDisplayAVG.Image = null;
            this.chkDisplayAVG.Name = "chkDisplayAVG";
            this.toolTip1.SetToolTip(this.chkDisplayAVG, resources.GetString("chkDisplayAVG.ToolTip"));
            this.chkDisplayAVG.CheckedChanged += new System.EventHandler(this.chkDisplayAVG_CheckedChanged);
            // 
            // chkShowTXCWFreq
            // 
            this.chkShowTXCWFreq.Image = null;
            resources.ApplyResources(this.chkShowTXCWFreq, "chkShowTXCWFreq");
            this.chkShowTXCWFreq.Name = "chkShowTXCWFreq";
            this.toolTip1.SetToolTip(this.chkShowTXCWFreq, resources.GetString("chkShowTXCWFreq.ToolTip"));
            this.chkShowTXCWFreq.CheckedChanged += new System.EventHandler(this.chkShowTXCWFreq_CheckedChanged);
            // 
            // chkCWVAC
            // 
            resources.ApplyResources(this.chkCWVAC, "chkCWVAC");
            this.chkCWVAC.Image = null;
            this.chkCWVAC.Name = "chkCWVAC";
            this.toolTip1.SetToolTip(this.chkCWVAC, resources.GetString("chkCWVAC.ToolTip"));
            this.chkCWVAC.CheckedChanged += new System.EventHandler(this.chkCWVAC_CheckedChanged);
            // 
            // udCWPitch
            // 
            this.udCWPitch.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            resources.ApplyResources(this.udCWPitch, "udCWPitch");
            this.udCWPitch.Maximum = new decimal(new int[] {
            2250,
            0,
            0,
            0});
            this.udCWPitch.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.udCWPitch.Name = "udCWPitch";
            this.toolTip1.SetToolTip(this.udCWPitch, resources.GetString("udCWPitch.ToolTip"));
            this.udCWPitch.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.udCWPitch.ValueChanged += new System.EventHandler(this.udCWPitch_ValueChanged);
            // 
            // chkCWDisableMonitor
            // 
            resources.ApplyResources(this.chkCWDisableMonitor, "chkCWDisableMonitor");
            this.chkCWDisableMonitor.Image = null;
            this.chkCWDisableMonitor.Name = "chkCWDisableMonitor";
            this.toolTip1.SetToolTip(this.chkCWDisableMonitor, resources.GetString("chkCWDisableMonitor.ToolTip"));
            this.chkCWDisableMonitor.CheckedChanged += new System.EventHandler(this.chkCWDisableMonitor_CheckedChanged);
            // 
            // chkCWIambic
            // 
            this.chkCWIambic.Checked = true;
            this.chkCWIambic.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.chkCWIambic, "chkCWIambic");
            this.chkCWIambic.Image = null;
            this.chkCWIambic.Name = "chkCWIambic";
            this.toolTip1.SetToolTip(this.chkCWIambic, resources.GetString("chkCWIambic.ToolTip"));
            this.chkCWIambic.CheckedChanged += new System.EventHandler(this.chkCWIambic_CheckedChanged);
            // 
            // chkBreakIn
            // 
            resources.ApplyResources(this.chkBreakIn, "chkBreakIn");
            this.chkBreakIn.BackColor = System.Drawing.Color.Yellow;
            this.chkBreakIn.Checked = true;
            this.chkBreakIn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBreakIn.Image = null;
            this.chkBreakIn.Name = "chkBreakIn";
            this.toolTip1.SetToolTip(this.chkBreakIn, resources.GetString("chkBreakIn.ToolTip"));
            this.chkBreakIn.UseVisualStyleBackColor = false;
            this.chkBreakIn.CheckedChanged += new System.EventHandler(this.chkBreakIn_CheckedChanged);
            // 
            // chkMUT
            // 
            resources.ApplyResources(this.chkMUT, "chkMUT");
            this.chkMUT.Image = null;
            this.chkMUT.Name = "chkMUT";
            this.toolTip1.SetToolTip(this.chkMUT, resources.GetString("chkMUT.ToolTip"));
            this.chkMUT.CheckedChanged += new System.EventHandler(this.chkMUT_CheckedChanged);
            // 
            // chkMON
            // 
            resources.ApplyResources(this.chkMON, "chkMON");
            this.chkMON.Image = null;
            this.chkMON.Name = "chkMON";
            this.toolTip1.SetToolTip(this.chkMON, resources.GetString("chkMON.ToolTip"));
            this.chkMON.CheckedChanged += new System.EventHandler(this.chkMON_CheckedChanged);
            // 
            // chkTUN
            // 
            resources.ApplyResources(this.chkTUN, "chkTUN");
            this.chkTUN.Image = null;
            this.chkTUN.Name = "chkTUN";
            this.toolTip1.SetToolTip(this.chkTUN, resources.GetString("chkTUN.ToolTip"));
            this.chkTUN.CheckedChanged += new System.EventHandler(this.chkTUN_CheckedChanged);
            // 
            // chkMOX
            // 
            resources.ApplyResources(this.chkMOX, "chkMOX");
            this.chkMOX.Image = null;
            this.chkMOX.Name = "chkMOX";
            this.toolTip1.SetToolTip(this.chkMOX, resources.GetString("chkMOX.ToolTip"));
            this.chkMOX.Click += new System.EventHandler(this.chkMOX_Click);
            this.chkMOX.CheckedChanged += new System.EventHandler(this.chkMOX_CheckedChanged2);
            // 
            // checkBoxTS1
            // 
            resources.ApplyResources(this.checkBoxTS1, "checkBoxTS1");
            this.checkBoxTS1.Image = null;
            this.checkBoxTS1.Name = "checkBoxTS1";
            this.toolTip1.SetToolTip(this.checkBoxTS1, resources.GetString("checkBoxTS1.ToolTip"));
            // 
            // radModeAM
            // 
            resources.ApplyResources(this.radModeAM, "radModeAM");
            this.radModeAM.Image = null;
            this.radModeAM.Name = "radModeAM";
            this.toolTip1.SetToolTip(this.radModeAM, resources.GetString("radModeAM.ToolTip"));
            this.radModeAM.CheckedChanged += new System.EventHandler(this.radModeAM_CheckedChanged);
            // 
            // radModeSAM
            // 
            resources.ApplyResources(this.radModeSAM, "radModeSAM");
            this.radModeSAM.Image = null;
            this.radModeSAM.Name = "radModeSAM";
            this.toolTip1.SetToolTip(this.radModeSAM, resources.GetString("radModeSAM.ToolTip"));
            this.radModeSAM.CheckedChanged += new System.EventHandler(this.radModeSAM_CheckedChanged);
            // 
            // radModeDSB
            // 
            resources.ApplyResources(this.radModeDSB, "radModeDSB");
            this.radModeDSB.Image = null;
            this.radModeDSB.Name = "radModeDSB";
            this.toolTip1.SetToolTip(this.radModeDSB, resources.GetString("radModeDSB.ToolTip"));
            this.radModeDSB.CheckedChanged += new System.EventHandler(this.radModeDSB_CheckedChanged);
            // 
            // radModeCWU
            // 
            resources.ApplyResources(this.radModeCWU, "radModeCWU");
            this.radModeCWU.Image = null;
            this.radModeCWU.Name = "radModeCWU";
            this.toolTip1.SetToolTip(this.radModeCWU, resources.GetString("radModeCWU.ToolTip"));
            this.radModeCWU.CheckedChanged += new System.EventHandler(this.radModeCWU_CheckedChanged);
            // 
            // radModeDIGU
            // 
            resources.ApplyResources(this.radModeDIGU, "radModeDIGU");
            this.radModeDIGU.Image = null;
            this.radModeDIGU.Name = "radModeDIGU";
            this.toolTip1.SetToolTip(this.radModeDIGU, resources.GetString("radModeDIGU.ToolTip"));
            this.radModeDIGU.CheckedChanged += new System.EventHandler(this.radModeDIGU_CheckedChanged);
            // 
            // radModeDIGL
            // 
            resources.ApplyResources(this.radModeDIGL, "radModeDIGL");
            this.radModeDIGL.Image = null;
            this.radModeDIGL.Name = "radModeDIGL";
            this.toolTip1.SetToolTip(this.radModeDIGL, resources.GetString("radModeDIGL.ToolTip"));
            this.radModeDIGL.CheckedChanged += new System.EventHandler(this.radModeDIGL_CheckedChanged);
            // 
            // radModeLSB
            // 
            resources.ApplyResources(this.radModeLSB, "radModeLSB");
            this.radModeLSB.Image = null;
            this.radModeLSB.Name = "radModeLSB";
            this.toolTip1.SetToolTip(this.radModeLSB, resources.GetString("radModeLSB.ToolTip"));
            this.radModeLSB.CheckedChanged += new System.EventHandler(this.radModeLSB_CheckedChanged);
            // 
            // radModeSPEC
            // 
            resources.ApplyResources(this.radModeSPEC, "radModeSPEC");
            this.radModeSPEC.Image = null;
            this.radModeSPEC.Name = "radModeSPEC";
            this.toolTip1.SetToolTip(this.radModeSPEC, resources.GetString("radModeSPEC.ToolTip"));
            this.radModeSPEC.CheckedChanged += new System.EventHandler(this.radModeSPEC_CheckedChanged);
            // 
            // radModeDRM
            // 
            resources.ApplyResources(this.radModeDRM, "radModeDRM");
            this.radModeDRM.Image = null;
            this.radModeDRM.Name = "radModeDRM";
            this.toolTip1.SetToolTip(this.radModeDRM, resources.GetString("radModeDRM.ToolTip"));
            this.radModeDRM.CheckedChanged += new System.EventHandler(this.radModeDRM_CheckedChanged);
            // 
            // radModeFMN
            // 
            resources.ApplyResources(this.radModeFMN, "radModeFMN");
            this.radModeFMN.Image = null;
            this.radModeFMN.Name = "radModeFMN";
            this.toolTip1.SetToolTip(this.radModeFMN, resources.GetString("radModeFMN.ToolTip"));
            this.radModeFMN.CheckedChanged += new System.EventHandler(this.radModeFMN_CheckedChanged);
            // 
            // radModeUSB
            // 
            resources.ApplyResources(this.radModeUSB, "radModeUSB");
            this.radModeUSB.BackColor = System.Drawing.SystemColors.Control;
            this.radModeUSB.Image = null;
            this.radModeUSB.Name = "radModeUSB";
            this.toolTip1.SetToolTip(this.radModeUSB, resources.GetString("radModeUSB.ToolTip"));
            this.radModeUSB.UseVisualStyleBackColor = false;
            this.radModeUSB.CheckedChanged += new System.EventHandler(this.radModeUSB_CheckedChanged);
            // 
            // radModeCWL
            // 
            resources.ApplyResources(this.radModeCWL, "radModeCWL");
            this.radModeCWL.Image = null;
            this.radModeCWL.Name = "radModeCWL";
            this.toolTip1.SetToolTip(this.radModeCWL, resources.GetString("radModeCWL.ToolTip"));
            this.radModeCWL.CheckedChanged += new System.EventHandler(this.radModeCWL_CheckedChanged);
            // 
            // udFilterHigh
            // 
            this.udFilterHigh.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.udFilterHigh, "udFilterHigh");
            this.udFilterHigh.ForeColor = System.Drawing.SystemColors.ControlText;
            this.udFilterHigh.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udFilterHigh.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.udFilterHigh.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.udFilterHigh.Name = "udFilterHigh";
            this.toolTip1.SetToolTip(this.udFilterHigh, resources.GetString("udFilterHigh.ToolTip"));
            this.udFilterHigh.Value = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.udFilterHigh.ValueChanged += new System.EventHandler(this.udFilterHigh_ValueChanged);
            this.udFilterHigh.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
            this.udFilterHigh.LostFocus += new System.EventHandler(this.udFilterHigh_LostFocus);
            // 
            // udFilterLow
            // 
            this.udFilterLow.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.udFilterLow, "udFilterLow");
            this.udFilterLow.ForeColor = System.Drawing.SystemColors.ControlText;
            this.udFilterLow.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udFilterLow.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.udFilterLow.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.udFilterLow.Name = "udFilterLow";
            this.toolTip1.SetToolTip(this.udFilterLow, resources.GetString("udFilterLow.ToolTip"));
            this.udFilterLow.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udFilterLow.ValueChanged += new System.EventHandler(this.udFilterLow_ValueChanged);
            this.udFilterLow.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
            this.udFilterLow.LostFocus += new System.EventHandler(this.udFilterLow_LostFocus);
            // 
            // tbFilterWidth
            // 
            resources.ApplyResources(this.tbFilterWidth, "tbFilterWidth");
            this.tbFilterWidth.Maximum = 10000;
            this.tbFilterWidth.Minimum = 1;
            this.tbFilterWidth.Name = "tbFilterWidth";
            this.tbFilterWidth.TickStyle = System.Windows.Forms.TickStyle.None;
            this.toolTip1.SetToolTip(this.tbFilterWidth, resources.GetString("tbFilterWidth.ToolTip"));
            this.tbFilterWidth.Value = 1;
            this.tbFilterWidth.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Console_MouseWheel);
            this.tbFilterWidth.Scroll += new System.EventHandler(this.tbFilterWidth_Scroll);
            this.tbFilterWidth.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tbFilterWidth_MouseMove);
            // 
            // btnFilterShiftReset
            // 
            resources.ApplyResources(this.btnFilterShiftReset, "btnFilterShiftReset");
            this.btnFilterShiftReset.Image = null;
            this.btnFilterShiftReset.Name = "btnFilterShiftReset";
            this.btnFilterShiftReset.Tag = "Reset Filter Shift";
            this.toolTip1.SetToolTip(this.btnFilterShiftReset, resources.GetString("btnFilterShiftReset.ToolTip"));
            this.btnFilterShiftReset.Click += new System.EventHandler(this.btnFilterShiftReset_Click);
            // 
            // tbFilterShift
            // 
            resources.ApplyResources(this.tbFilterShift, "tbFilterShift");
            this.tbFilterShift.Maximum = 1000;
            this.tbFilterShift.Minimum = -1000;
            this.tbFilterShift.Name = "tbFilterShift";
            this.tbFilterShift.TickStyle = System.Windows.Forms.TickStyle.None;
            this.toolTip1.SetToolTip(this.tbFilterShift, resources.GetString("tbFilterShift.ToolTip"));
            this.tbFilterShift.Scroll += new System.EventHandler(this.tbFilterShift_Scroll);
            this.tbFilterShift.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tbFilterShift_MouseMove);
            // 
            // chkPower
            // 
            resources.ApplyResources(this.chkPower, "chkPower");
            this.chkPower.BackColor = System.Drawing.SystemColors.Control;
            this.chkPower.Image = null;
            this.chkPower.Name = "chkPower";
            this.toolTip1.SetToolTip(this.chkPower, resources.GetString("chkPower.ToolTip"));
            this.chkPower.UseVisualStyleBackColor = false;
            this.chkPower.CheckedChanged += new System.EventHandler(this.chkPower_CheckedChanged);
            // 
            // chkSR
            // 
            resources.ApplyResources(this.chkSR, "chkSR");
            this.chkSR.BackColor = System.Drawing.Color.Yellow;
            this.chkSR.Checked = true;
            this.chkSR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSR.Image = null;
            this.chkSR.Name = "chkSR";
            this.toolTip1.SetToolTip(this.chkSR, resources.GetString("chkSR.ToolTip"));
            this.chkSR.UseVisualStyleBackColor = false;
            this.chkSR.CheckedChanged += new System.EventHandler(this.chkSR_CheckedChanged);
            // 
            // chkDSPNB2
            // 
            resources.ApplyResources(this.chkDSPNB2, "chkDSPNB2");
            this.chkDSPNB2.Image = null;
            this.chkDSPNB2.Name = "chkDSPNB2";
            this.toolTip1.SetToolTip(this.chkDSPNB2, resources.GetString("chkDSPNB2.ToolTip"));
            this.chkDSPNB2.CheckedChanged += new System.EventHandler(this.chkDSPNB2_CheckedChanged);
            // 
            // chkNB
            // 
            resources.ApplyResources(this.chkNB, "chkNB");
            this.chkNB.Image = null;
            this.chkNB.Name = "chkNB";
            this.toolTip1.SetToolTip(this.chkNB, resources.GetString("chkNB.ToolTip"));
            this.chkNB.CheckedChanged += new System.EventHandler(this.chkNB_CheckedChanged);
            // 
            // chkANF
            // 
            resources.ApplyResources(this.chkANF, "chkANF");
            this.chkANF.Image = null;
            this.chkANF.Name = "chkANF";
            this.toolTip1.SetToolTip(this.chkANF, resources.GetString("chkANF.ToolTip"));
            this.chkANF.CheckedChanged += new System.EventHandler(this.chkANF_CheckedChanged);
            // 
            // chkNR
            // 
            resources.ApplyResources(this.chkNR, "chkNR");
            this.chkNR.Image = null;
            this.chkNR.Name = "chkNR";
            this.toolTip1.SetToolTip(this.chkNR, resources.GetString("chkNR.ToolTip"));
            this.chkNR.CheckedChanged += new System.EventHandler(this.chkNR_CheckedChanged);
            // 
            // chkBIN
            // 
            resources.ApplyResources(this.chkBIN, "chkBIN");
            this.chkBIN.Image = null;
            this.chkBIN.Name = "chkBIN";
            this.toolTip1.SetToolTip(this.chkBIN, resources.GetString("chkBIN.ToolTip"));
            this.chkBIN.CheckedChanged += new System.EventHandler(this.chkBIN_CheckedChanged);
            // 
            // comboAGC
            // 
            this.comboAGC.BackColor = System.Drawing.SystemColors.Window;
            this.comboAGC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAGC.DropDownWidth = 48;
            this.comboAGC.ForeColor = System.Drawing.SystemColors.WindowText;
            resources.ApplyResources(this.comboAGC, "comboAGC");
            this.comboAGC.Name = "comboAGC";
            this.toolTip1.SetToolTip(this.comboAGC, resources.GetString("comboAGC.ToolTip"));
            this.comboAGC.SelectedIndexChanged += new System.EventHandler(this.comboAGC_SelectedIndexChanged);
            // 
            // comboPreamp
            // 
            this.comboPreamp.BackColor = System.Drawing.SystemColors.Window;
            this.comboPreamp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPreamp.DropDownWidth = 48;
            this.comboPreamp.ForeColor = System.Drawing.SystemColors.WindowText;
            resources.ApplyResources(this.comboPreamp, "comboPreamp");
            this.comboPreamp.Items.AddRange(new object[] {
            resources.GetString("comboPreamp.Items"),
            resources.GetString("comboPreamp.Items1")});
            this.comboPreamp.Name = "comboPreamp";
            this.toolTip1.SetToolTip(this.comboPreamp, resources.GetString("comboPreamp.ToolTip"));
            this.comboPreamp.SelectedIndexChanged += new System.EventHandler(this.comboPreamp_SelectedIndexChanged);
            // 
            // udXIT
            // 
            this.udXIT.BackColor = System.Drawing.SystemColors.Window;
            this.udXIT.ForeColor = System.Drawing.SystemColors.ControlText;
            this.udXIT.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udXIT, "udXIT");
            this.udXIT.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.udXIT.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.udXIT.Name = "udXIT";
            this.toolTip1.SetToolTip(this.udXIT, resources.GetString("udXIT.ToolTip"));
            this.udXIT.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udXIT.ValueChanged += new System.EventHandler(this.udXIT_ValueChanged);
            this.udXIT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
            this.udXIT.LostFocus += new System.EventHandler(this.udXIT_LostFocus);
            // 
            // udRIT
            // 
            this.udRIT.BackColor = System.Drawing.SystemColors.Window;
            this.udRIT.ForeColor = System.Drawing.SystemColors.ControlText;
            this.udRIT.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udRIT, "udRIT");
            this.udRIT.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.udRIT.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.udRIT.Name = "udRIT";
            this.toolTip1.SetToolTip(this.udRIT, resources.GetString("udRIT.ToolTip"));
            this.udRIT.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udRIT.ValueChanged += new System.EventHandler(this.udRIT_ValueChanged);
            this.udRIT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
            this.udRIT.LostFocus += new System.EventHandler(this.udRIT_LostFocus);
            // 
            // chkXIT
            // 
            resources.ApplyResources(this.chkXIT, "chkXIT");
            this.chkXIT.Image = null;
            this.chkXIT.Name = "chkXIT";
            this.toolTip1.SetToolTip(this.chkXIT, resources.GetString("chkXIT.ToolTip"));
            this.chkXIT.CheckedChanged += new System.EventHandler(this.chkXIT_CheckedChanged);
            // 
            // chkRIT
            // 
            resources.ApplyResources(this.chkRIT, "chkRIT");
            this.chkRIT.Image = null;
            this.chkRIT.Name = "chkRIT";
            this.toolTip1.SetToolTip(this.chkRIT, resources.GetString("chkRIT.ToolTip"));
            this.chkRIT.CheckedChanged += new System.EventHandler(this.chkRIT_CheckedChanged);
            // 
            // udPWR
            // 
            this.udPWR.BackColor = System.Drawing.SystemColors.Window;
            this.udPWR.ForeColor = System.Drawing.SystemColors.ControlText;
            this.udPWR.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udPWR, "udPWR");
            this.udPWR.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udPWR.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udPWR.Name = "udPWR";
            this.toolTip1.SetToolTip(this.udPWR, resources.GetString("udPWR.ToolTip"));
            this.udPWR.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.udPWR.ValueChanged += new System.EventHandler(this.udPWR_ValueChanged);
            this.udPWR.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
            this.udPWR.LostFocus += new System.EventHandler(this.udPWR_LostFocus);
            // 
            // udAF
            // 
            this.udAF.BackColor = System.Drawing.SystemColors.Window;
            this.udAF.ForeColor = System.Drawing.SystemColors.ControlText;
            this.udAF.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udAF, "udAF");
            this.udAF.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udAF.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udAF.Name = "udAF";
            this.toolTip1.SetToolTip(this.udAF, resources.GetString("udAF.ToolTip"));
            this.udAF.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.udAF.ValueChanged += new System.EventHandler(this.udAF_ValueChanged);
            this.udAF.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
            this.udAF.LostFocus += new System.EventHandler(this.udAF_LostFocus);
            // 
            // comboMeterTXMode
            // 
            this.comboMeterTXMode.BackColor = System.Drawing.SystemColors.Window;
            this.comboMeterTXMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMeterTXMode.DropDownWidth = 72;
            this.comboMeterTXMode.ForeColor = System.Drawing.SystemColors.WindowText;
            resources.ApplyResources(this.comboMeterTXMode, "comboMeterTXMode");
            this.comboMeterTXMode.Name = "comboMeterTXMode";
            this.toolTip1.SetToolTip(this.comboMeterTXMode, resources.GetString("comboMeterTXMode.ToolTip"));
            this.comboMeterTXMode.SelectedIndexChanged += new System.EventHandler(this.comboMeterTXMode_SelectedIndexChanged);
            // 
            // comboMeterRXMode
            // 
            this.comboMeterRXMode.BackColor = System.Drawing.SystemColors.Window;
            this.comboMeterRXMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMeterRXMode.DropDownWidth = 72;
            this.comboMeterRXMode.ForeColor = System.Drawing.SystemColors.WindowText;
            resources.ApplyResources(this.comboMeterRXMode, "comboMeterRXMode");
            this.comboMeterRXMode.Name = "comboMeterRXMode";
            this.toolTip1.SetToolTip(this.comboMeterRXMode, resources.GetString("comboMeterRXMode.ToolTip"));
            this.comboMeterRXMode.SelectedIndexChanged += new System.EventHandler(this.comboMeterRXMode_SelectedIndexChanged);
            // 
            // btnZeroBeat
            // 
            resources.ApplyResources(this.btnZeroBeat, "btnZeroBeat");
            this.btnZeroBeat.Image = null;
            this.btnZeroBeat.Name = "btnZeroBeat";
            this.toolTip1.SetToolTip(this.btnZeroBeat, resources.GetString("btnZeroBeat.ToolTip"));
            this.btnZeroBeat.Click += new System.EventHandler(this.btnZeroBeat_Click);
            // 
            // btnRITReset
            // 
            this.btnRITReset.Image = null;
            resources.ApplyResources(this.btnRITReset, "btnRITReset");
            this.btnRITReset.Name = "btnRITReset";
            this.toolTip1.SetToolTip(this.btnRITReset, resources.GetString("btnRITReset.ToolTip"));
            this.btnRITReset.Click += new System.EventHandler(this.btnRITReset_Click);
            // 
            // btnXITReset
            // 
            this.btnXITReset.Image = null;
            resources.ApplyResources(this.btnXITReset, "btnXITReset");
            this.btnXITReset.Name = "btnXITReset";
            this.toolTip1.SetToolTip(this.btnXITReset, resources.GetString("btnXITReset.ToolTip"));
            this.btnXITReset.Click += new System.EventHandler(this.btnXITReset_Click);
            // 
            // btnIFtoVFO
            // 
            this.btnIFtoVFO.Image = null;
            resources.ApplyResources(this.btnIFtoVFO, "btnIFtoVFO");
            this.btnIFtoVFO.Name = "btnIFtoVFO";
            this.toolTip1.SetToolTip(this.btnIFtoVFO, resources.GetString("btnIFtoVFO.ToolTip"));
            this.btnIFtoVFO.Click += new System.EventHandler(this.btnIFtoVFO_Click);
            // 
            // btnVFOSwap
            // 
            this.btnVFOSwap.Image = null;
            resources.ApplyResources(this.btnVFOSwap, "btnVFOSwap");
            this.btnVFOSwap.Name = "btnVFOSwap";
            this.toolTip1.SetToolTip(this.btnVFOSwap, resources.GetString("btnVFOSwap.ToolTip"));
            this.btnVFOSwap.Click += new System.EventHandler(this.btnVFOSwap_Click);
            // 
            // btnVFOBtoA
            // 
            this.btnVFOBtoA.Image = null;
            resources.ApplyResources(this.btnVFOBtoA, "btnVFOBtoA");
            this.btnVFOBtoA.Name = "btnVFOBtoA";
            this.toolTip1.SetToolTip(this.btnVFOBtoA, resources.GetString("btnVFOBtoA.ToolTip"));
            this.btnVFOBtoA.Click += new System.EventHandler(this.btnVFOBtoA_Click);
            // 
            // btnVFOAtoB
            // 
            this.btnVFOAtoB.Image = null;
            resources.ApplyResources(this.btnVFOAtoB, "btnVFOAtoB");
            this.btnVFOAtoB.Name = "btnVFOAtoB";
            this.toolTip1.SetToolTip(this.btnVFOAtoB, resources.GetString("btnVFOAtoB.ToolTip"));
            this.btnVFOAtoB.Click += new System.EventHandler(this.btnVFOAtoB_Click);
            // 
            // chkVFOSplit
            // 
            resources.ApplyResources(this.chkVFOSplit, "chkVFOSplit");
            this.chkVFOSplit.Image = null;
            this.chkVFOSplit.Name = "chkVFOSplit";
            this.toolTip1.SetToolTip(this.chkVFOSplit, resources.GetString("chkVFOSplit.ToolTip"));
            this.chkVFOSplit.CheckedChanged += new System.EventHandler(this.chkVFOSplit_CheckedChanged);
            // 
            // tbPWR
            // 
            resources.ApplyResources(this.tbPWR, "tbPWR");
            this.tbPWR.Maximum = 100;
            this.tbPWR.Name = "tbPWR";
            this.tbPWR.TickFrequency = 10;
            this.tbPWR.TickStyle = System.Windows.Forms.TickStyle.None;
            this.toolTip1.SetToolTip(this.tbPWR, resources.GetString("tbPWR.ToolTip"));
            this.tbPWR.Value = 50;
            this.tbPWR.Scroll += new System.EventHandler(this.tbPWR_Scroll);
            this.tbPWR.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tbPWR_MouseMove);
            // 
            // tbRF
            // 
            resources.ApplyResources(this.tbRF, "tbRF");
            this.tbRF.Maximum = 120;
            this.tbRF.Minimum = -20;
            this.tbRF.Name = "tbRF";
            this.tbRF.TickStyle = System.Windows.Forms.TickStyle.None;
            this.toolTip1.SetToolTip(this.tbRF, resources.GetString("tbRF.ToolTip"));
            this.tbRF.Value = 90;
            this.tbRF.Scroll += new System.EventHandler(this.tbRF_Scroll);
            this.tbRF.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tbRF_MouseMove);
            // 
            // tbAF
            // 
            resources.ApplyResources(this.tbAF, "tbAF");
            this.tbAF.Maximum = 100;
            this.tbAF.Name = "tbAF";
            this.tbAF.TickStyle = System.Windows.Forms.TickStyle.None;
            this.toolTip1.SetToolTip(this.tbAF, resources.GetString("tbAF.ToolTip"));
            this.tbAF.Value = 50;
            this.tbAF.Scroll += new System.EventHandler(this.tbAF_Scroll);
            this.tbAF.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tbAF_MouseMove);
            // 
            // udRF
            // 
            this.udRF.BackColor = System.Drawing.SystemColors.Window;
            this.udRF.ForeColor = System.Drawing.SystemColors.ControlText;
            this.udRF.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udRF, "udRF");
            this.udRF.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.udRF.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            this.udRF.Name = "udRF";
            this.toolTip1.SetToolTip(this.udRF, resources.GetString("udRF.ToolTip"));
            this.udRF.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.udRF.ValueChanged += new System.EventHandler(this.udRF_ValueChanged);
            // 
            // chkSquelch
            // 
            resources.ApplyResources(this.chkSquelch, "chkSquelch");
            this.chkSquelch.Image = null;
            this.chkSquelch.Name = "chkSquelch";
            this.toolTip1.SetToolTip(this.chkSquelch, resources.GetString("chkSquelch.ToolTip"));
            this.chkSquelch.CheckedChanged += new System.EventHandler(this.chkSquelch_CheckedChanged);
            // 
            // udCWSpeed
            // 
            this.udCWSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udCWSpeed, "udCWSpeed");
            this.udCWSpeed.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.udCWSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udCWSpeed.Name = "udCWSpeed";
            this.toolTip1.SetToolTip(this.udCWSpeed, resources.GetString("udCWSpeed.ToolTip"));
            this.udCWSpeed.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.udCWSpeed.ValueChanged += new System.EventHandler(this.udCWSpeed_ValueChanged);
            this.udCWSpeed.LostFocus += new System.EventHandler(this.udCWSpeed_LostFocus);
            // 
            // udVACTXGain
            // 
            this.udVACTXGain.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udVACTXGain, "udVACTXGain");
            this.udVACTXGain.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.udVACTXGain.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            -2147483648});
            this.udVACTXGain.Name = "udVACTXGain";
            this.toolTip1.SetToolTip(this.udVACTXGain, resources.GetString("udVACTXGain.ToolTip"));
            this.udVACTXGain.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udVACTXGain.ValueChanged += new System.EventHandler(this.udVACTXGain_ValueChanged);
            // 
            // udVACRXGain
            // 
            this.udVACRXGain.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udVACRXGain, "udVACRXGain");
            this.udVACRXGain.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.udVACRXGain.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            -2147483648});
            this.udVACRXGain.Name = "udVACRXGain";
            this.toolTip1.SetToolTip(this.udVACRXGain, resources.GetString("udVACRXGain.ToolTip"));
            this.udVACRXGain.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udVACRXGain.ValueChanged += new System.EventHandler(this.udVACRXGain_ValueChanged);
            // 
            // chkVACEnabled
            // 
            resources.ApplyResources(this.chkVACEnabled, "chkVACEnabled");
            this.chkVACEnabled.Image = null;
            this.chkVACEnabled.Name = "chkVACEnabled";
            this.toolTip1.SetToolTip(this.chkVACEnabled, resources.GetString("chkVACEnabled.ToolTip"));
            this.chkVACEnabled.CheckedChanged += new System.EventHandler(this.chkVACEnabled_CheckedChanged);
            // 
            // chkVFOsinc
            // 
            resources.ApplyResources(this.chkVFOsinc, "chkVFOsinc");
            this.chkVFOsinc.Image = null;
            this.chkVFOsinc.Name = "chkVFOsinc";
            this.toolTip1.SetToolTip(this.chkVFOsinc, resources.GetString("chkVFOsinc.ToolTip"));
            this.chkVFOsinc.Click += new System.EventHandler(this.chkVFOsinc_Click);
            // 
            // btnEraseMemory
            // 
            this.btnEraseMemory.Image = null;
            resources.ApplyResources(this.btnEraseMemory, "btnEraseMemory");
            this.btnEraseMemory.Name = "btnEraseMemory";
            this.toolTip1.SetToolTip(this.btnEraseMemory, resources.GetString("btnEraseMemory.ToolTip"));
            this.btnEraseMemory.Click += new System.EventHandler(this.btnEraseMemory_Click);
            // 
            // btnVFOA
            // 
            this.btnVFOA.Image = null;
            resources.ApplyResources(this.btnVFOA, "btnVFOA");
            this.btnVFOA.Name = "btnVFOA";
            this.toolTip1.SetToolTip(this.btnVFOA, resources.GetString("btnVFOA.ToolTip"));
            this.btnVFOA.Click += new System.EventHandler(this.btnVFOA_Click);
            // 
            // picSQL
            // 
            this.picSQL.BackColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.picSQL, "picSQL");
            this.picSQL.Name = "picSQL";
            this.picSQL.TabStop = false;
            this.picSQL.Paint += new System.Windows.Forms.PaintEventHandler(this.picSQL_Paint);
            // 
            // timer_clock
            // 
            this.timer_clock.Enabled = true;
            this.timer_clock.Interval = 200;
            this.timer_clock.Tick += new System.EventHandler(this.timer_clock_Tick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tbDisplayZoom);
            this.groupBox2.Controls.Add(this.tbDisplayPan);
            this.groupBox2.Controls.Add(this.btnDisplayZoom48x);
            this.groupBox2.Controls.Add(this.btnDisplayZoom1x);
            this.groupBox2.Controls.Add(this.btnDisplayZoom2x);
            this.groupBox2.Controls.Add(this.btnDisplayZoom16x);
            this.groupBox2.Controls.Add(this.btnDisplayZoom4x);
            this.groupBox2.Controls.Add(this.btnDisplayZoom8x);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnDisplayZoom48x
            // 
            this.btnDisplayZoom48x.Image = null;
            resources.ApplyResources(this.btnDisplayZoom48x, "btnDisplayZoom48x");
            this.btnDisplayZoom48x.Name = "btnDisplayZoom48x";
            this.btnDisplayZoom48x.UseVisualStyleBackColor = true;
            this.btnDisplayZoom48x.Click += new System.EventHandler(this.btnDisplayZoom48x_Click);
            // 
            // btnDisplayZoom1x
            // 
            this.btnDisplayZoom1x.Image = null;
            resources.ApplyResources(this.btnDisplayZoom1x, "btnDisplayZoom1x");
            this.btnDisplayZoom1x.Name = "btnDisplayZoom1x";
            this.btnDisplayZoom1x.UseVisualStyleBackColor = true;
            this.btnDisplayZoom1x.Click += new System.EventHandler(this.btnDisplayZoom1x_Click);
            // 
            // btnDisplayZoom2x
            // 
            this.btnDisplayZoom2x.Image = null;
            resources.ApplyResources(this.btnDisplayZoom2x, "btnDisplayZoom2x");
            this.btnDisplayZoom2x.Name = "btnDisplayZoom2x";
            this.btnDisplayZoom2x.UseVisualStyleBackColor = true;
            this.btnDisplayZoom2x.Click += new System.EventHandler(this.btnDisplayZoom2x_Click);
            // 
            // btnDisplayZoom16x
            // 
            this.btnDisplayZoom16x.Image = null;
            resources.ApplyResources(this.btnDisplayZoom16x, "btnDisplayZoom16x");
            this.btnDisplayZoom16x.Name = "btnDisplayZoom16x";
            this.btnDisplayZoom16x.UseVisualStyleBackColor = true;
            this.btnDisplayZoom16x.Click += new System.EventHandler(this.btnDisplayZoom16x_Click);
            // 
            // btnDisplayZoom4x
            // 
            this.btnDisplayZoom4x.Image = null;
            resources.ApplyResources(this.btnDisplayZoom4x, "btnDisplayZoom4x");
            this.btnDisplayZoom4x.Name = "btnDisplayZoom4x";
            this.btnDisplayZoom4x.UseVisualStyleBackColor = true;
            this.btnDisplayZoom4x.Click += new System.EventHandler(this.btnDisplayZoom4x_Click);
            // 
            // btnDisplayZoom8x
            // 
            this.btnDisplayZoom8x.Image = null;
            resources.ApplyResources(this.btnDisplayZoom8x, "btnDisplayZoom8x");
            this.btnDisplayZoom8x.Name = "btnDisplayZoom8x";
            this.btnDisplayZoom8x.UseVisualStyleBackColor = true;
            this.btnDisplayZoom8x.Click += new System.EventHandler(this.btnDisplayZoom8x_Click);
            // 
            // grpG160
            // 
            this.grpG160.Controls.Add(this.btnG160_X2);
            this.grpG160.Controls.Add(this.btnG160_X1);
            resources.ApplyResources(this.grpG160, "grpG160");
            this.grpG160.Name = "grpG160";
            this.grpG160.TabStop = false;
            // 
            // grpG59Option
            // 
            this.grpG59Option.Controls.Add(this.btnATT);
            this.grpG59Option.Controls.Add(this.btnHIGH_AF);
            this.grpG59Option.Controls.Add(this.btnHIGH_RF);
            resources.ApplyResources(this.grpG59Option, "grpG59Option");
            this.grpG59Option.Name = "grpG59Option";
            this.grpG59Option.TabStop = false;
            // 
            // grpG80
            // 
            this.grpG80.Controls.Add(this.btnG80_X4);
            this.grpG80.Controls.Add(this.btnG80_X3);
            this.grpG80.Controls.Add(this.btnG80_X2);
            this.grpG80.Controls.Add(this.btnG80_X1);
            resources.ApplyResources(this.grpG80, "grpG80");
            this.grpG80.Name = "grpG80";
            this.grpG80.TabStop = false;
            // 
            // btnG80_X4
            // 
            resources.ApplyResources(this.btnG80_X4, "btnG80_X4");
            this.btnG80_X4.Name = "btnG80_X4";
            this.btnG80_X4.UseVisualStyleBackColor = true;
            this.btnG80_X4.Click += new System.EventHandler(this.btnG80_X4_Click);
            // 
            // btnG80_X3
            // 
            resources.ApplyResources(this.btnG80_X3, "btnG80_X3");
            this.btnG80_X3.Name = "btnG80_X3";
            this.btnG80_X3.UseVisualStyleBackColor = true;
            this.btnG80_X3.Click += new System.EventHandler(this.btnG80_X3_Click);
            // 
            // btnG80_X2
            // 
            resources.ApplyResources(this.btnG80_X2, "btnG80_X2");
            this.btnG80_X2.Name = "btnG80_X2";
            this.btnG80_X2.UseVisualStyleBackColor = true;
            this.btnG80_X2.Click += new System.EventHandler(this.btnG80_X2_Click);
            // 
            // btnG80_X1
            // 
            resources.ApplyResources(this.btnG80_X1, "btnG80_X1");
            this.btnG80_X1.Name = "btnG80_X1";
            this.btnG80_X1.UseVisualStyleBackColor = true;
            this.btnG80_X1.Click += new System.EventHandler(this.btnG80_X1_Click);
            // 
            // grp3020
            // 
            this.grp3020.Controls.Add(this.btnG3020_X4);
            this.grp3020.Controls.Add(this.btnG3020_X3);
            this.grp3020.Controls.Add(this.btnG3020_X2);
            this.grp3020.Controls.Add(this.btnG3020_X1);
            resources.ApplyResources(this.grp3020, "grp3020");
            this.grp3020.Name = "grp3020";
            this.grp3020.TabStop = false;
            // 
            // btnG3020_X4
            // 
            resources.ApplyResources(this.btnG3020_X4, "btnG3020_X4");
            this.btnG3020_X4.Name = "btnG3020_X4";
            this.btnG3020_X4.UseVisualStyleBackColor = true;
            this.btnG3020_X4.Click += new System.EventHandler(this.btnG3020_X4_Click);
            // 
            // btnG3020_X3
            // 
            resources.ApplyResources(this.btnG3020_X3, "btnG3020_X3");
            this.btnG3020_X3.Name = "btnG3020_X3";
            this.btnG3020_X3.UseVisualStyleBackColor = true;
            this.btnG3020_X3.Click += new System.EventHandler(this.btnG3020_X3_Click);
            // 
            // btnG3020_X2
            // 
            resources.ApplyResources(this.btnG3020_X2, "btnG3020_X2");
            this.btnG3020_X2.Name = "btnG3020_X2";
            this.btnG3020_X2.UseVisualStyleBackColor = true;
            this.btnG3020_X2.Click += new System.EventHandler(this.btnG3020_X2_Click);
            // 
            // btnG3020_X1
            // 
            resources.ApplyResources(this.btnG3020_X1, "btnG3020_X1");
            this.btnG3020_X1.Name = "btnG3020_X1";
            this.btnG3020_X1.UseVisualStyleBackColor = true;
            this.btnG3020_X1.Click += new System.EventHandler(this.btnG3020_X1_Click);
            // 
            // contextMemoryMenu
            // 
            this.contextMemoryMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eraseAllMemoryToolStripMenuItem});
            this.contextMemoryMenu.Name = "contextMemoryMenu";
            resources.ApplyResources(this.contextMemoryMenu, "contextMemoryMenu");
            // 
            // eraseAllMemoryToolStripMenuItem
            // 
            this.eraseAllMemoryToolStripMenuItem.Name = "eraseAllMemoryToolStripMenuItem";
            resources.ApplyResources(this.eraseAllMemoryToolStripMenuItem, "eraseAllMemoryToolStripMenuItem");
            this.eraseAllMemoryToolStripMenuItem.Click += new System.EventHandler(this.eraseAllMemoryToolStripMenuItem_Click);
            // 
            // contextLOSCMenu
            // 
            this.contextLOSCMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xtal1ToolStripMenuItem});
            this.contextLOSCMenu.Name = "contextLOSCMenu";
            resources.ApplyResources(this.contextLOSCMenu, "contextLOSCMenu");
            // 
            // xtal1ToolStripMenuItem
            // 
            this.xtal1ToolStripMenuItem.Name = "xtal1ToolStripMenuItem";
            resources.ApplyResources(this.xtal1ToolStripMenuItem, "xtal1ToolStripMenuItem");
            this.xtal1ToolStripMenuItem.Click += new System.EventHandler(this.xtalToolStripMenuItem);
            // 
            // grpLOSC
            // 
            this.grpLOSC.BackColor = System.Drawing.SystemColors.Control;
            this.grpLOSC.Controls.Add(this.panelLOSCHover);
            this.grpLOSC.Controls.Add(this.txtLOSCLSD);
            this.grpLOSC.Controls.Add(this.txtLOSCMSD);
            this.grpLOSC.Controls.Add(this.txtLOSCFreq);
            resources.ApplyResources(this.grpLOSC, "grpLOSC");
            this.grpLOSC.ForeColor = System.Drawing.Color.Black;
            this.grpLOSC.Name = "grpLOSC";
            this.grpLOSC.TabStop = false;
            // 
            // panelLOSCHover
            // 
            this.panelLOSCHover.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.panelLOSCHover, "panelLOSCHover");
            this.panelLOSCHover.Name = "panelLOSCHover";
            this.panelLOSCHover.Paint += new System.Windows.Forms.PaintEventHandler(this.panelLOSCHover_Paint);
            this.panelLOSCHover.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelLOSCHover_MouseMove);
            // 
            // txtLOSCLSD
            // 
            this.txtLOSCLSD.BackColor = System.Drawing.Color.Black;
            this.txtLOSCLSD.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLOSCLSD.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtLOSCLSD, "txtLOSCLSD");
            this.txtLOSCLSD.ForeColor = System.Drawing.Color.Olive;
            this.txtLOSCLSD.Name = "txtLOSCLSD";
            this.txtLOSCLSD.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtLOSCLSD_MouseMove);
            this.txtLOSCLSD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtLOSCLSD_MouseDown);
            // 
            // txtLOSCMSD
            // 
            this.txtLOSCMSD.BackColor = System.Drawing.Color.Black;
            this.txtLOSCMSD.ContextMenuStrip = this.contextLOSCMenu;
            this.txtLOSCMSD.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtLOSCMSD, "txtLOSCMSD");
            this.txtLOSCMSD.ForeColor = System.Drawing.Color.Olive;
            this.txtLOSCMSD.Name = "txtLOSCMSD";
            this.txtLOSCMSD.MouseLeave += new System.EventHandler(this.txtLOSCMSD_MouseLeave);
            this.txtLOSCMSD.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtLOSCMSD_MouseMove);
            this.txtLOSCMSD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtLOSCMSD_MouseDown);
            // 
            // txtLOSCFreq
            // 
            this.txtLOSCFreq.BackColor = System.Drawing.Color.Black;
            this.txtLOSCFreq.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtLOSCFreq, "txtLOSCFreq");
            this.txtLOSCFreq.ForeColor = System.Drawing.Color.Green;
            this.txtLOSCFreq.Name = "txtLOSCFreq";
            this.txtLOSCFreq.MouseLeave += new System.EventHandler(this.txtLOSCFreq_MouseLeave);
            this.txtLOSCFreq.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtLOSCFreq_MouseMove);
            this.txtLOSCFreq.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLOSCFreq_KeyPress);
            this.txtLOSCFreq.LostFocus += new System.EventHandler(this.txtLOSCFreq_LostFocus);
            // 
            // grpSubRX
            // 
            this.grpSubRX.Controls.Add(this.tbRX0Gain);
            this.grpSubRX.Controls.Add(this.chkPanSwap);
            this.grpSubRX.Controls.Add(this.chkEnableSubRX);
            this.grpSubRX.Controls.Add(this.tbPanSubRX);
            this.grpSubRX.Controls.Add(this.tbPanMainRX);
            this.grpSubRX.Controls.Add(this.tbRX1Gain);
            resources.ApplyResources(this.grpSubRX, "grpSubRX");
            this.grpSubRX.Name = "grpSubRX";
            this.grpSubRX.TabStop = false;
            // 
            // grpBandHF
            // 
            this.grpBandHF.Controls.Add(this.btnBandGEN);
            this.grpBandHF.Controls.Add(this.btnBandWWV);
            this.grpBandHF.Controls.Add(this.btnBandVHF);
            this.grpBandHF.Controls.Add(this.btnBand2);
            this.grpBandHF.Controls.Add(this.btnBand6);
            this.grpBandHF.Controls.Add(this.btnBand10);
            this.grpBandHF.Controls.Add(this.btnBand12);
            this.grpBandHF.Controls.Add(this.btnBand15);
            this.grpBandHF.Controls.Add(this.btnBand17);
            this.grpBandHF.Controls.Add(this.btnBand20);
            this.grpBandHF.Controls.Add(this.btnBand30);
            this.grpBandHF.Controls.Add(this.btnBand40);
            this.grpBandHF.Controls.Add(this.btnBand60);
            this.grpBandHF.Controls.Add(this.btnBand80);
            this.grpBandHF.Controls.Add(this.btnBand160);
            resources.ApplyResources(this.grpBandHF, "grpBandHF");
            this.grpBandHF.Name = "grpBandHF";
            this.grpBandHF.TabStop = false;
            // 
            // grpModeSpecificPhone
            // 
            this.grpModeSpecificPhone.Controls.Add(this.udNoiseGate);
            this.grpModeSpecificPhone.Controls.Add(this.udVOX);
            this.grpModeSpecificPhone.Controls.Add(this.tbMIC);
            this.grpModeSpecificPhone.Controls.Add(this.udCPDR);
            this.grpModeSpecificPhone.Controls.Add(this.tbCPDR);
            this.grpModeSpecificPhone.Controls.Add(this.udCOMP);
            this.grpModeSpecificPhone.Controls.Add(this.tbCOMP);
            this.grpModeSpecificPhone.Controls.Add(this.picNoiseGate);
            this.grpModeSpecificPhone.Controls.Add(this.tbNoiseGate);
            this.grpModeSpecificPhone.Controls.Add(this.picVOX);
            this.grpModeSpecificPhone.Controls.Add(this.tbVOX);
            this.grpModeSpecificPhone.Controls.Add(this.udMIC);
            this.grpModeSpecificPhone.Controls.Add(this.chkNoiseGate);
            this.grpModeSpecificPhone.Controls.Add(this.chkVOX);
            this.grpModeSpecificPhone.Controls.Add(this.lblMIC);
            this.grpModeSpecificPhone.Controls.Add(this.chkDSPComp);
            this.grpModeSpecificPhone.Controls.Add(this.chkDSPCompander);
            this.grpModeSpecificPhone.Controls.Add(this.comboTXProfile);
            this.grpModeSpecificPhone.Controls.Add(this.lblTransmitProfile);
            this.grpModeSpecificPhone.Controls.Add(this.chkShowTXFilter);
            resources.ApplyResources(this.grpModeSpecificPhone, "grpModeSpecificPhone");
            this.grpModeSpecificPhone.Name = "grpModeSpecificPhone";
            this.grpModeSpecificPhone.TabStop = false;
            // 
            // udNoiseGate
            // 
            this.udNoiseGate.BackColor = System.Drawing.SystemColors.Window;
            this.udNoiseGate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.udNoiseGate.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udNoiseGate, "udNoiseGate");
            this.udNoiseGate.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udNoiseGate.Minimum = new decimal(new int[] {
            160,
            0,
            0,
            -2147483648});
            this.udNoiseGate.Name = "udNoiseGate";
            this.udNoiseGate.Value = new decimal(new int[] {
            40,
            0,
            0,
            -2147483648});
            this.udNoiseGate.ValueChanged += new System.EventHandler(this.udNoiseGate_ValueChanged);
            // 
            // udVOX
            // 
            this.udVOX.BackColor = System.Drawing.SystemColors.Window;
            this.udVOX.ForeColor = System.Drawing.SystemColors.ControlText;
            this.udVOX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udVOX, "udVOX");
            this.udVOX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.udVOX.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udVOX.Name = "udVOX";
            this.udVOX.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.udVOX.ValueChanged += new System.EventHandler(this.udVOX_ValueChanged);
            // 
            // tbMIC
            // 
            resources.ApplyResources(this.tbMIC, "tbMIC");
            this.tbMIC.Maximum = 70;
            this.tbMIC.Name = "tbMIC";
            this.tbMIC.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbMIC.Value = 35;
            this.tbMIC.Scroll += new System.EventHandler(this.tbMIC_Scroll);
            // 
            // udCPDR
            // 
            this.udCPDR.BackColor = System.Drawing.SystemColors.Window;
            this.udCPDR.ForeColor = System.Drawing.SystemColors.ControlText;
            this.udCPDR.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udCPDR, "udCPDR");
            this.udCPDR.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udCPDR.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udCPDR.Name = "udCPDR";
            this.udCPDR.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.udCPDR.ValueChanged += new System.EventHandler(this.udCPDR_ValueChanged);
            // 
            // tbCPDR
            // 
            resources.ApplyResources(this.tbCPDR, "tbCPDR");
            this.tbCPDR.Name = "tbCPDR";
            this.tbCPDR.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbCPDR.Value = 3;
            this.tbCPDR.Scroll += new System.EventHandler(this.tbCPDR_Scroll);
            // 
            // udCOMP
            // 
            this.udCOMP.BackColor = System.Drawing.SystemColors.Window;
            this.udCOMP.ForeColor = System.Drawing.SystemColors.ControlText;
            this.udCOMP.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udCOMP, "udCOMP");
            this.udCOMP.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.udCOMP.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udCOMP.Name = "udCOMP";
            this.udCOMP.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.udCOMP.ValueChanged += new System.EventHandler(this.udCOMP_ValueChanged);
            // 
            // tbCOMP
            // 
            resources.ApplyResources(this.tbCOMP, "tbCOMP");
            this.tbCOMP.Maximum = 20;
            this.tbCOMP.Name = "tbCOMP";
            this.tbCOMP.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbCOMP.Value = 3;
            this.tbCOMP.Scroll += new System.EventHandler(this.tbCOMP_Scroll);
            // 
            // picNoiseGate
            // 
            this.picNoiseGate.BackColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.picNoiseGate, "picNoiseGate");
            this.picNoiseGate.Name = "picNoiseGate";
            this.picNoiseGate.TabStop = false;
            this.picNoiseGate.Paint += new System.Windows.Forms.PaintEventHandler(this.picNoiseGate_Paint);
            // 
            // tbNoiseGate
            // 
            resources.ApplyResources(this.tbNoiseGate, "tbNoiseGate");
            this.tbNoiseGate.Maximum = 0;
            this.tbNoiseGate.Minimum = -160;
            this.tbNoiseGate.Name = "tbNoiseGate";
            this.tbNoiseGate.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbNoiseGate.Value = -40;
            this.tbNoiseGate.Scroll += new System.EventHandler(this.tbNoiseGate_Scroll);
            // 
            // picVOX
            // 
            this.picVOX.BackColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.picVOX, "picVOX");
            this.picVOX.Name = "picVOX";
            this.picVOX.TabStop = false;
            this.picVOX.Paint += new System.Windows.Forms.PaintEventHandler(this.picVOX_Paint);
            // 
            // tbVOX
            // 
            resources.ApplyResources(this.tbVOX, "tbVOX");
            this.tbVOX.Maximum = 1000;
            this.tbVOX.Name = "tbVOX";
            this.tbVOX.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbVOX.Value = 200;
            this.tbVOX.Scroll += new System.EventHandler(this.tbVOX_Scroll);
            // 
            // udMIC
            // 
            this.udMIC.BackColor = System.Drawing.SystemColors.Window;
            this.udMIC.ForeColor = System.Drawing.SystemColors.ControlText;
            this.udMIC.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udMIC, "udMIC");
            this.udMIC.Maximum = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.udMIC.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udMIC.Name = "udMIC";
            this.udMIC.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
            this.udMIC.ValueChanged += new System.EventHandler(this.udMIC_ValueChanged);
            this.udMIC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
            this.udMIC.LostFocus += new System.EventHandler(this.udMIC_LostFocus);
            // 
            // lblMIC
            // 
            this.lblMIC.Image = null;
            resources.ApplyResources(this.lblMIC, "lblMIC");
            this.lblMIC.Name = "lblMIC";
            // 
            // lblTransmitProfile
            // 
            this.lblTransmitProfile.Image = null;
            resources.ApplyResources(this.lblTransmitProfile, "lblTransmitProfile");
            this.lblTransmitProfile.Name = "lblTransmitProfile";
            // 
            // tbSQL
            // 
            resources.ApplyResources(this.tbSQL, "tbSQL");
            this.tbSQL.Maximum = 0;
            this.tbSQL.Minimum = -160;
            this.tbSQL.Name = "tbSQL";
            this.tbSQL.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbSQL.Value = -150;
            this.tbSQL.Scroll += new System.EventHandler(this.tbSQL_Scroll);
            this.tbSQL.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tbSQL_MouseMove);
            // 
            // grpModeSpecificDigital
            // 
            this.grpModeSpecificDigital.Controls.Add(this.grpVACStereo);
            this.grpModeSpecificDigital.Controls.Add(this.grpDIGSampleRate);
            this.grpModeSpecificDigital.Controls.Add(this.tbVACTXGain);
            this.grpModeSpecificDigital.Controls.Add(this.tbVACRXGain);
            this.grpModeSpecificDigital.Controls.Add(this.udVACTXGain);
            this.grpModeSpecificDigital.Controls.Add(this.udVACRXGain);
            this.grpModeSpecificDigital.Controls.Add(this.lblTXGain);
            this.grpModeSpecificDigital.Controls.Add(this.lblRXGain);
            this.grpModeSpecificDigital.Controls.Add(this.chkVACEnabled);
            resources.ApplyResources(this.grpModeSpecificDigital, "grpModeSpecificDigital");
            this.grpModeSpecificDigital.Name = "grpModeSpecificDigital";
            this.grpModeSpecificDigital.TabStop = false;
            // 
            // grpVACStereo
            // 
            this.grpVACStereo.Controls.Add(this.chkVACStereo);
            resources.ApplyResources(this.grpVACStereo, "grpVACStereo");
            this.grpVACStereo.Name = "grpVACStereo";
            this.grpVACStereo.TabStop = false;
            // 
            // grpDIGSampleRate
            // 
            this.grpDIGSampleRate.Controls.Add(this.comboVACSampleRate);
            resources.ApplyResources(this.grpDIGSampleRate, "grpDIGSampleRate");
            this.grpDIGSampleRate.Name = "grpDIGSampleRate";
            this.grpDIGSampleRate.TabStop = false;
            // 
            // tbVACTXGain
            // 
            resources.ApplyResources(this.tbVACTXGain, "tbVACTXGain");
            this.tbVACTXGain.Maximum = 20;
            this.tbVACTXGain.Minimum = -40;
            this.tbVACTXGain.Name = "tbVACTXGain";
            this.tbVACTXGain.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbVACTXGain.Scroll += new System.EventHandler(this.tbVACTXGain_Scroll);
            // 
            // tbVACRXGain
            // 
            resources.ApplyResources(this.tbVACRXGain, "tbVACRXGain");
            this.tbVACRXGain.Maximum = 20;
            this.tbVACRXGain.Minimum = -40;
            this.tbVACRXGain.Name = "tbVACRXGain";
            this.tbVACRXGain.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbVACRXGain.Scroll += new System.EventHandler(this.tbVACRXGain_Scroll);
            // 
            // lblTXGain
            // 
            this.lblTXGain.Image = null;
            resources.ApplyResources(this.lblTXGain, "lblTXGain");
            this.lblTXGain.Name = "lblTXGain";
            // 
            // lblRXGain
            // 
            this.lblRXGain.Image = null;
            resources.ApplyResources(this.lblRXGain, "lblRXGain");
            this.lblRXGain.Name = "lblRXGain";
            // 
            // grpG40
            // 
            this.grpG40.Controls.Add(this.btnG40_X1);
            resources.ApplyResources(this.grpG40, "grpG40");
            this.grpG40.Name = "grpG40";
            this.grpG40.TabStop = false;
            // 
            // btnG40_X1
            // 
            resources.ApplyResources(this.btnG40_X1, "btnG40_X1");
            this.btnG40_X1.Name = "btnG40_X1";
            this.btnG40_X1.UseVisualStyleBackColor = true;
            this.btnG40_X1.Click += new System.EventHandler(this.btnG40_X1_Click);
            // 
            // lblMemoryNumber
            // 
            resources.ApplyResources(this.lblMemoryNumber, "lblMemoryNumber");
            this.lblMemoryNumber.BackColor = System.Drawing.Color.Blue;
            this.lblMemoryNumber.ContextMenuStrip = this.contextMemoryMenu;
            this.lblMemoryNumber.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblMemoryNumber.Image = null;
            this.lblMemoryNumber.MinimumSize = new System.Drawing.Size(27, 20);
            this.lblMemoryNumber.Name = "lblMemoryNumber";
            this.lblMemoryNumber.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblMemoryNumber_MouseMove);
            this.lblMemoryNumber.Click += new System.EventHandler(this.lblMemoryNumber_Click);
            // 
            // grpVFOBetween
            // 
            this.grpVFOBetween.Controls.Add(this.txtMemory);
            this.grpVFOBetween.Controls.Add(this.lblMemoryNumber);
            this.grpVFOBetween.Controls.Add(this.chkVFOsinc);
            this.grpVFOBetween.Controls.Add(this.btnEraseMemory);
            this.grpVFOBetween.Controls.Add(this.btnChangeTuneStepLarger);
            this.grpVFOBetween.Controls.Add(this.btnTuneStepChangeSmaller);
            this.grpVFOBetween.Controls.Add(this.chkVFOLock);
            this.grpVFOBetween.Controls.Add(this.txtWheelTune);
            this.grpVFOBetween.Controls.Add(this.lblTuneStep);
            this.grpVFOBetween.Controls.Add(this.btnMemoryQuickRecall);
            this.grpVFOBetween.Controls.Add(this.btnMemoryQuickSave);
            resources.ApplyResources(this.grpVFOBetween, "grpVFOBetween");
            this.grpVFOBetween.Name = "grpVFOBetween";
            this.grpVFOBetween.TabStop = false;
            // 
            // txtMemory
            // 
            resources.ApplyResources(this.txtMemory, "txtMemory");
            this.txtMemory.Name = "txtMemory";
            // 
            // lblTuneStep
            // 
            resources.ApplyResources(this.lblTuneStep, "lblTuneStep");
            this.lblTuneStep.Name = "lblTuneStep";
            // 
            // grpDisplay2
            // 
            this.grpDisplay2.Controls.Add(this.chkDisplayPeak);
            this.grpDisplay2.Controls.Add(this.comboDisplayMode);
            this.grpDisplay2.Controls.Add(this.chkDisplayAVG);
            resources.ApplyResources(this.grpDisplay2, "grpDisplay2");
            this.grpDisplay2.Name = "grpDisplay2";
            this.grpDisplay2.TabStop = false;
            // 
            // grpModeSpecificCW
            // 
            this.grpModeSpecificCW.Controls.Add(this.chkShowTXCWFreq);
            this.grpModeSpecificCW.Controls.Add(this.chkCWVAC);
            this.grpModeSpecificCW.Controls.Add(this.grpCWPitch);
            this.grpModeSpecificCW.Controls.Add(this.chkCWDisableMonitor);
            this.grpModeSpecificCW.Controls.Add(this.chkCWIambic);
            this.grpModeSpecificCW.Controls.Add(this.tbCWSpeed);
            this.grpModeSpecificCW.Controls.Add(this.chkBreakIn);
            this.grpModeSpecificCW.Controls.Add(this.lblCWSpeed);
            this.grpModeSpecificCW.Controls.Add(this.udCWSpeed);
            resources.ApplyResources(this.grpModeSpecificCW, "grpModeSpecificCW");
            this.grpModeSpecificCW.Name = "grpModeSpecificCW";
            this.grpModeSpecificCW.TabStop = false;
            // 
            // grpCWPitch
            // 
            this.grpCWPitch.Controls.Add(this.lblCWPitchFreq);
            this.grpCWPitch.Controls.Add(this.udCWPitch);
            resources.ApplyResources(this.grpCWPitch, "grpCWPitch");
            this.grpCWPitch.Name = "grpCWPitch";
            this.grpCWPitch.TabStop = false;
            // 
            // lblCWPitchFreq
            // 
            this.lblCWPitchFreq.Image = null;
            resources.ApplyResources(this.lblCWPitchFreq, "lblCWPitchFreq");
            this.lblCWPitchFreq.Name = "lblCWPitchFreq";
            // 
            // tbCWSpeed
            // 
            resources.ApplyResources(this.tbCWSpeed, "tbCWSpeed");
            this.tbCWSpeed.Maximum = 60;
            this.tbCWSpeed.Minimum = 1;
            this.tbCWSpeed.Name = "tbCWSpeed";
            this.tbCWSpeed.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbCWSpeed.Value = 25;
            this.tbCWSpeed.Scroll += new System.EventHandler(this.tbCWSpeed_Scroll);
            // 
            // lblCWSpeed
            // 
            resources.ApplyResources(this.lblCWSpeed, "lblCWSpeed");
            this.lblCWSpeed.Name = "lblCWSpeed";
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.chkMUT);
            this.grpOptions.Controls.Add(this.chkMON);
            this.grpOptions.Controls.Add(this.chkTUN);
            this.grpOptions.Controls.Add(this.chkMOX);
            this.grpOptions.Controls.Add(this.checkBoxTS1);
            resources.ApplyResources(this.grpOptions, "grpOptions");
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.TabStop = false;
            // 
            // txtVFOAFreq
            // 
            this.txtVFOAFreq.BackColor = System.Drawing.Color.Black;
            this.txtVFOAFreq.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtVFOAFreq, "txtVFOAFreq");
            this.txtVFOAFreq.ForeColor = System.Drawing.Color.Olive;
            this.txtVFOAFreq.Name = "txtVFOAFreq";
            this.txtVFOAFreq.MouseLeave += new System.EventHandler(this.txtVFOAFreq_MouseLeave);
            this.txtVFOAFreq.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtVFOAFreq_MouseMove);
            this.txtVFOAFreq.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVFOAFreq_KeyPress);
            this.txtVFOAFreq.LostFocus += new System.EventHandler(this.txtVFOAFreq_LostFocus);
            // 
            // grpVFOA
            // 
            this.grpVFOA.BackColor = System.Drawing.SystemColors.Control;
            this.grpVFOA.Controls.Add(this.btnHidden);
            this.grpVFOA.Controls.Add(this.panelVFOAHover);
            this.grpVFOA.Controls.Add(this.txtVFOALSD);
            this.grpVFOA.Controls.Add(this.txtVFOAMSD);
            this.grpVFOA.Controls.Add(this.txtVFOABand);
            this.grpVFOA.Controls.Add(this.txtVFOAFreq);
            resources.ApplyResources(this.grpVFOA, "grpVFOA");
            this.grpVFOA.ForeColor = System.Drawing.Color.Black;
            this.grpVFOA.Name = "grpVFOA";
            this.grpVFOA.TabStop = false;
            // 
            // btnHidden
            // 
            resources.ApplyResources(this.btnHidden, "btnHidden");
            this.btnHidden.Name = "btnHidden";
            this.btnHidden.UseVisualStyleBackColor = true;
            // 
            // panelVFOAHover
            // 
            this.panelVFOAHover.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.panelVFOAHover, "panelVFOAHover");
            this.panelVFOAHover.Name = "panelVFOAHover";
            this.panelVFOAHover.Paint += new System.Windows.Forms.PaintEventHandler(this.panelVFOAHover_Paint);
            this.panelVFOAHover.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelVFOAHover_MouseMove);
            // 
            // txtVFOALSD
            // 
            this.txtVFOALSD.BackColor = System.Drawing.Color.Black;
            this.txtVFOALSD.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtVFOALSD.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtVFOALSD, "txtVFOALSD");
            this.txtVFOALSD.ForeColor = System.Drawing.Color.Olive;
            this.txtVFOALSD.Name = "txtVFOALSD";
            this.txtVFOALSD.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtVFOALSD_MouseMove);
            this.txtVFOALSD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtVFOALSD_MouseDown);
            // 
            // txtVFOAMSD
            // 
            this.txtVFOAMSD.BackColor = System.Drawing.Color.Black;
            this.txtVFOAMSD.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtVFOAMSD, "txtVFOAMSD");
            this.txtVFOAMSD.ForeColor = System.Drawing.Color.Olive;
            this.txtVFOAMSD.Name = "txtVFOAMSD";
            this.txtVFOAMSD.MouseLeave += new System.EventHandler(this.txtVFOAMSD_MouseLeave);
            this.txtVFOAMSD.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtVFOAMSD_MouseMove);
            this.txtVFOAMSD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtVFOAMSD_MouseDown);
            // 
            // txtVFOABand
            // 
            this.txtVFOABand.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.txtVFOABand, "txtVFOABand");
            this.txtVFOABand.ForeColor = System.Drawing.Color.Green;
            this.txtVFOABand.Name = "txtVFOABand";
            this.txtVFOABand.ReadOnly = true;
            this.txtVFOABand.GotFocus += new System.EventHandler(this.HideFocus);
            // 
            // grpVFOB
            // 
            this.grpVFOB.BackColor = System.Drawing.SystemColors.Control;
            this.grpVFOB.Controls.Add(this.txtVFOBLSD);
            this.grpVFOB.Controls.Add(this.panelVFOBHover);
            this.grpVFOB.Controls.Add(this.txtVFOBMSD);
            this.grpVFOB.Controls.Add(this.lblVFOBLSD);
            this.grpVFOB.Controls.Add(this.txtVFOBBand);
            this.grpVFOB.Controls.Add(this.txtVFOBFreq);
            resources.ApplyResources(this.grpVFOB, "grpVFOB");
            this.grpVFOB.Name = "grpVFOB";
            this.grpVFOB.TabStop = false;
            // 
            // txtVFOBLSD
            // 
            this.txtVFOBLSD.BackColor = System.Drawing.Color.Black;
            this.txtVFOBLSD.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtVFOBLSD.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtVFOBLSD, "txtVFOBLSD");
            this.txtVFOBLSD.ForeColor = System.Drawing.Color.Olive;
            this.txtVFOBLSD.Name = "txtVFOBLSD";
            this.txtVFOBLSD.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtVFOBLSD_MouseMove);
            this.txtVFOBLSD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtVFOBLSD_MouseDown);
            // 
            // panelVFOBHover
            // 
            this.panelVFOBHover.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.panelVFOBHover, "panelVFOBHover");
            this.panelVFOBHover.Name = "panelVFOBHover";
            this.panelVFOBHover.Paint += new System.Windows.Forms.PaintEventHandler(this.panelVFOBHover_Paint);
            this.panelVFOBHover.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelVFOBHover_MouseMove);
            // 
            // txtVFOBMSD
            // 
            this.txtVFOBMSD.BackColor = System.Drawing.Color.Black;
            this.txtVFOBMSD.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtVFOBMSD, "txtVFOBMSD");
            this.txtVFOBMSD.ForeColor = System.Drawing.Color.Olive;
            this.txtVFOBMSD.Name = "txtVFOBMSD";
            this.txtVFOBMSD.MouseLeave += new System.EventHandler(this.txtVFOBMSD_MouseLeave);
            this.txtVFOBMSD.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtVFOBMSD_MouseMove);
            this.txtVFOBMSD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtVFOBMSD_MouseDown);
            // 
            // lblVFOBLSD
            // 
            this.lblVFOBLSD.BackColor = System.Drawing.Color.Cyan;
            resources.ApplyResources(this.lblVFOBLSD, "lblVFOBLSD");
            this.lblVFOBLSD.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblVFOBLSD.Name = "lblVFOBLSD";
            // 
            // txtVFOBBand
            // 
            this.txtVFOBBand.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.txtVFOBBand, "txtVFOBBand");
            this.txtVFOBBand.ForeColor = System.Drawing.Color.Green;
            this.txtVFOBBand.Name = "txtVFOBBand";
            this.txtVFOBBand.ReadOnly = true;
            this.txtVFOBBand.GotFocus += new System.EventHandler(this.HideFocus);
            // 
            // txtVFOBFreq
            // 
            this.txtVFOBFreq.BackColor = System.Drawing.Color.Black;
            this.txtVFOBFreq.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtVFOBFreq, "txtVFOBFreq");
            this.txtVFOBFreq.ForeColor = System.Drawing.Color.Olive;
            this.txtVFOBFreq.Name = "txtVFOBFreq";
            this.txtVFOBFreq.MouseLeave += new System.EventHandler(this.txtVFOBFreq_MouseLeave);
            this.txtVFOBFreq.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtVFOBFreq_MouseMove);
            this.txtVFOBFreq.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVFOBFreq_KeyPress);
            this.txtVFOBFreq.LostFocus += new System.EventHandler(this.txtVFOBFreq_LostFocus);
            // 
            // grpDisplay
            // 
            this.grpDisplay.Controls.Add(this.textBox2);
            this.grpDisplay.Controls.Add(this.picWaterfall);
            this.grpDisplay.Controls.Add(this.txtDisplayPeakFreq);
            this.grpDisplay.Controls.Add(this.txtDisplayCursorFreq);
            this.grpDisplay.Controls.Add(this.txtDisplayCursorPower);
            this.grpDisplay.Controls.Add(this.txtDisplayPeakPower);
            this.grpDisplay.Controls.Add(this.textBox1);
            this.grpDisplay.Controls.Add(this.txtDisplayCursorOffset);
            this.grpDisplay.Controls.Add(this.txtDisplayPeakOffset);
            this.grpDisplay.Controls.Add(this.picDisplay);
            resources.ApplyResources(this.grpDisplay, "grpDisplay");
            this.grpDisplay.Name = "grpDisplay";
            this.grpDisplay.TabStop = false;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.Black;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            // 
            // picWaterfall
            // 
            this.picWaterfall.BackColor = System.Drawing.Color.Black;
            this.picWaterfall.Cursor = System.Windows.Forms.Cursors.Cross;
            resources.ApplyResources(this.picWaterfall, "picWaterfall");
            this.picWaterfall.Name = "picWaterfall";
            this.picWaterfall.TabStop = false;
            this.picWaterfall.MouseLeave += new System.EventHandler(this.picWaterfall_MouseLeave);
            this.picWaterfall.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picWaterfall_MouseMove);
            this.picWaterfall.Resize += new System.EventHandler(this.picWaterfall_Resize);
            this.picWaterfall.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picWaterfall_MouseDown);
            this.picWaterfall.Paint += new System.Windows.Forms.PaintEventHandler(this.picWaterfall_Paint);
            this.picWaterfall.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picWaterfall_MouseUp);
            // 
            // txtDisplayPeakFreq
            // 
            this.txtDisplayPeakFreq.BackColor = System.Drawing.Color.Black;
            this.txtDisplayPeakFreq.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDisplayPeakFreq.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtDisplayPeakFreq, "txtDisplayPeakFreq");
            this.txtDisplayPeakFreq.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txtDisplayPeakFreq.Name = "txtDisplayPeakFreq";
            this.txtDisplayPeakFreq.ReadOnly = true;
            // 
            // txtDisplayCursorFreq
            // 
            this.txtDisplayCursorFreq.BackColor = System.Drawing.Color.Black;
            this.txtDisplayCursorFreq.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDisplayCursorFreq.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtDisplayCursorFreq, "txtDisplayCursorFreq");
            this.txtDisplayCursorFreq.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txtDisplayCursorFreq.Name = "txtDisplayCursorFreq";
            this.txtDisplayCursorFreq.ReadOnly = true;
            // 
            // txtDisplayCursorPower
            // 
            this.txtDisplayCursorPower.BackColor = System.Drawing.Color.Black;
            this.txtDisplayCursorPower.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDisplayCursorPower.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtDisplayCursorPower, "txtDisplayCursorPower");
            this.txtDisplayCursorPower.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txtDisplayCursorPower.Name = "txtDisplayCursorPower";
            this.txtDisplayCursorPower.ReadOnly = true;
            // 
            // txtDisplayPeakPower
            // 
            this.txtDisplayPeakPower.BackColor = System.Drawing.Color.Black;
            this.txtDisplayPeakPower.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDisplayPeakPower.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtDisplayPeakPower, "txtDisplayPeakPower");
            this.txtDisplayPeakPower.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txtDisplayPeakPower.Name = "txtDisplayPeakPower";
            this.txtDisplayPeakPower.ReadOnly = true;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Black;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            // 
            // txtDisplayCursorOffset
            // 
            this.txtDisplayCursorOffset.BackColor = System.Drawing.Color.Black;
            this.txtDisplayCursorOffset.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDisplayCursorOffset.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtDisplayCursorOffset, "txtDisplayCursorOffset");
            this.txtDisplayCursorOffset.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txtDisplayCursorOffset.Name = "txtDisplayCursorOffset";
            this.txtDisplayCursorOffset.ReadOnly = true;
            this.txtDisplayCursorOffset.GotFocus += new System.EventHandler(this.HideFocus);
            // 
            // txtDisplayPeakOffset
            // 
            this.txtDisplayPeakOffset.BackColor = System.Drawing.Color.Black;
            this.txtDisplayPeakOffset.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDisplayPeakOffset.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtDisplayPeakOffset, "txtDisplayPeakOffset");
            this.txtDisplayPeakOffset.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txtDisplayPeakOffset.Name = "txtDisplayPeakOffset";
            this.txtDisplayPeakOffset.ReadOnly = true;
            this.txtDisplayPeakOffset.GotFocus += new System.EventHandler(this.HideFocus);
            // 
            // picDisplay
            // 
            this.picDisplay.BackColor = System.Drawing.Color.Black;
            this.picDisplay.Cursor = System.Windows.Forms.Cursors.Cross;
            resources.ApplyResources(this.picDisplay, "picDisplay");
            this.picDisplay.Name = "picDisplay";
            this.picDisplay.TabStop = false;
            this.picDisplay.MouseLeave += new System.EventHandler(this.picDisplay_MouseLeave);
            this.picDisplay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picDisplay_MouseMove);
            this.picDisplay.Resize += new System.EventHandler(this.picDisplay_Resize);
            this.picDisplay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picDisplay_MouseDown);
            this.picDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.picDisplay_Paint);
            this.picDisplay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picDisplay_MouseUp);
            // 
            // grpMode
            // 
            this.grpMode.Controls.Add(this.radModeAM);
            this.grpMode.Controls.Add(this.radModeSAM);
            this.grpMode.Controls.Add(this.radModeDSB);
            this.grpMode.Controls.Add(this.radModeCWU);
            this.grpMode.Controls.Add(this.radModeDIGU);
            this.grpMode.Controls.Add(this.radModeDIGL);
            this.grpMode.Controls.Add(this.radModeLSB);
            this.grpMode.Controls.Add(this.radModeSPEC);
            this.grpMode.Controls.Add(this.radModeDRM);
            this.grpMode.Controls.Add(this.radModeFMN);
            this.grpMode.Controls.Add(this.radModeUSB);
            this.grpMode.Controls.Add(this.radModeCWL);
            resources.ApplyResources(this.grpMode, "grpMode");
            this.grpMode.Name = "grpMode";
            this.grpMode.TabStop = false;
            // 
            // grpFilter
            // 
            this.grpFilter.ContextMenu = this.contextMenuFilter;
            this.grpFilter.Controls.Add(this.udFilterHigh);
            this.grpFilter.Controls.Add(this.udFilterLow);
            this.grpFilter.Controls.Add(this.tbFilterWidth);
            this.grpFilter.Controls.Add(this.lblFilterWidth);
            this.grpFilter.Controls.Add(this.btnFilterShiftReset);
            this.grpFilter.Controls.Add(this.tbFilterShift);
            this.grpFilter.Controls.Add(this.lblFilterShift);
            this.grpFilter.Controls.Add(this.radFilter1);
            this.grpFilter.Controls.Add(this.radFilter2);
            this.grpFilter.Controls.Add(this.radFilter3);
            this.grpFilter.Controls.Add(this.radFilter4);
            this.grpFilter.Controls.Add(this.radFilter5);
            this.grpFilter.Controls.Add(this.radFilter6);
            this.grpFilter.Controls.Add(this.radFilter7);
            this.grpFilter.Controls.Add(this.radFilter8);
            this.grpFilter.Controls.Add(this.radFilter9);
            this.grpFilter.Controls.Add(this.radFilter10);
            this.grpFilter.Controls.Add(this.radFilterVar1);
            this.grpFilter.Controls.Add(this.radFilterVar2);
            this.grpFilter.Controls.Add(this.lblFilterLow);
            this.grpFilter.Controls.Add(this.lblFilterHigh);
            resources.ApplyResources(this.grpFilter, "grpFilter");
            this.grpFilter.Name = "grpFilter";
            this.grpFilter.TabStop = false;
            // 
            // lblFilterWidth
            // 
            this.lblFilterWidth.Image = null;
            resources.ApplyResources(this.lblFilterWidth, "lblFilterWidth");
            this.lblFilterWidth.Name = "lblFilterWidth";
            // 
            // lblFilterShift
            // 
            this.lblFilterShift.Image = null;
            resources.ApplyResources(this.lblFilterShift, "lblFilterShift");
            this.lblFilterShift.Name = "lblFilterShift";
            // 
            // radFilter1
            // 
            resources.ApplyResources(this.radFilter1, "radFilter1");
            this.radFilter1.Image = null;
            this.radFilter1.Name = "radFilter1";
            this.radFilter1.CheckedChanged += new System.EventHandler(this.radFilter1_CheckedChanged);
            // 
            // radFilter2
            // 
            resources.ApplyResources(this.radFilter2, "radFilter2");
            this.radFilter2.Image = null;
            this.radFilter2.Name = "radFilter2";
            this.radFilter2.CheckedChanged += new System.EventHandler(this.radFilter2_CheckedChanged);
            // 
            // radFilter3
            // 
            resources.ApplyResources(this.radFilter3, "radFilter3");
            this.radFilter3.Image = null;
            this.radFilter3.Name = "radFilter3";
            this.radFilter3.CheckedChanged += new System.EventHandler(this.radFilter3_CheckedChanged);
            // 
            // radFilter4
            // 
            resources.ApplyResources(this.radFilter4, "radFilter4");
            this.radFilter4.Image = null;
            this.radFilter4.Name = "radFilter4";
            this.radFilter4.CheckedChanged += new System.EventHandler(this.radFilter4_CheckedChanged);
            // 
            // radFilter5
            // 
            resources.ApplyResources(this.radFilter5, "radFilter5");
            this.radFilter5.Image = null;
            this.radFilter5.Name = "radFilter5";
            this.radFilter5.CheckedChanged += new System.EventHandler(this.radFilter5_CheckedChanged);
            // 
            // radFilter6
            // 
            resources.ApplyResources(this.radFilter6, "radFilter6");
            this.radFilter6.Image = null;
            this.radFilter6.Name = "radFilter6";
            this.radFilter6.CheckedChanged += new System.EventHandler(this.radFilter6_CheckedChanged);
            // 
            // radFilter7
            // 
            resources.ApplyResources(this.radFilter7, "radFilter7");
            this.radFilter7.Image = null;
            this.radFilter7.Name = "radFilter7";
            this.radFilter7.CheckedChanged += new System.EventHandler(this.radFilter7_CheckedChanged);
            // 
            // radFilter8
            // 
            resources.ApplyResources(this.radFilter8, "radFilter8");
            this.radFilter8.Image = null;
            this.radFilter8.Name = "radFilter8";
            this.radFilter8.CheckedChanged += new System.EventHandler(this.radFilter8_CheckedChanged);
            // 
            // radFilter9
            // 
            resources.ApplyResources(this.radFilter9, "radFilter9");
            this.radFilter9.Image = null;
            this.radFilter9.Name = "radFilter9";
            this.radFilter9.CheckedChanged += new System.EventHandler(this.radFilter9_CheckedChanged);
            // 
            // radFilter10
            // 
            resources.ApplyResources(this.radFilter10, "radFilter10");
            this.radFilter10.Image = null;
            this.radFilter10.Name = "radFilter10";
            this.radFilter10.CheckedChanged += new System.EventHandler(this.radFilter10_CheckedChanged);
            // 
            // radFilterVar1
            // 
            resources.ApplyResources(this.radFilterVar1, "radFilterVar1");
            this.radFilterVar1.Image = null;
            this.radFilterVar1.Name = "radFilterVar1";
            this.radFilterVar1.CheckedChanged += new System.EventHandler(this.radFilterVar1_CheckedChanged);
            // 
            // radFilterVar2
            // 
            resources.ApplyResources(this.radFilterVar2, "radFilterVar2");
            this.radFilterVar2.Image = null;
            this.radFilterVar2.Name = "radFilterVar2";
            this.radFilterVar2.CheckedChanged += new System.EventHandler(this.radFilterVar2_CheckedChanged);
            // 
            // lblFilterLow
            // 
            this.lblFilterLow.Image = null;
            resources.ApplyResources(this.lblFilterLow, "lblFilterLow");
            this.lblFilterLow.Name = "lblFilterLow";
            // 
            // lblFilterHigh
            // 
            this.lblFilterHigh.Image = null;
            resources.ApplyResources(this.lblFilterHigh, "lblFilterHigh");
            this.lblFilterHigh.Name = "lblFilterHigh";
            // 
            // lblCPUMeter
            // 
            this.lblCPUMeter.Image = null;
            resources.ApplyResources(this.lblCPUMeter, "lblCPUMeter");
            this.lblCPUMeter.Name = "lblCPUMeter";
            // 
            // grpDSP
            // 
            this.grpDSP.Controls.Add(this.chkSR);
            this.grpDSP.Controls.Add(this.chkDSPNB2);
            this.grpDSP.Controls.Add(this.chkNB);
            this.grpDSP.Controls.Add(this.chkANF);
            this.grpDSP.Controls.Add(this.chkNR);
            this.grpDSP.Controls.Add(this.chkBIN);
            resources.ApplyResources(this.grpDSP, "grpDSP");
            this.grpDSP.Name = "grpDSP";
            this.grpDSP.TabStop = false;
            // 
            // lblAGC
            // 
            this.lblAGC.Image = null;
            resources.ApplyResources(this.lblAGC, "lblAGC");
            this.lblAGC.Name = "lblAGC";
            // 
            // lblPreamp
            // 
            this.lblPreamp.Image = null;
            resources.ApplyResources(this.lblPreamp, "lblPreamp");
            this.lblPreamp.Name = "lblPreamp";
            // 
            // lblPWR
            // 
            this.lblPWR.Image = null;
            resources.ApplyResources(this.lblPWR, "lblPWR");
            this.lblPWR.Name = "lblPWR";
            // 
            // lblAF
            // 
            this.lblAF.Image = null;
            resources.ApplyResources(this.lblAF, "lblAF");
            this.lblAF.Name = "lblAF";
            // 
            // grpMultimeter
            // 
            this.grpMultimeter.Controls.Add(this.comboMeterTXMode);
            this.grpMultimeter.Controls.Add(this.picMultiMeterDigital);
            this.grpMultimeter.Controls.Add(this.lblMultiSMeter);
            this.grpMultimeter.Controls.Add(this.comboMeterRXMode);
            this.grpMultimeter.Controls.Add(this.txtMultiText);
            this.grpMultimeter.Controls.Add(this.picMultimeterAnalog);
            resources.ApplyResources(this.grpMultimeter, "grpMultimeter");
            this.grpMultimeter.Name = "grpMultimeter";
            this.grpMultimeter.TabStop = false;
            // 
            // picMultiMeterDigital
            // 
            this.picMultiMeterDigital.BackColor = System.Drawing.Color.Black;
            this.picMultiMeterDigital.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.picMultiMeterDigital, "picMultiMeterDigital");
            this.picMultiMeterDigital.Name = "picMultiMeterDigital";
            this.picMultiMeterDigital.TabStop = false;
            this.picMultiMeterDigital.Paint += new System.Windows.Forms.PaintEventHandler(this.picMultiMeterDigital_Paint);
            // 
            // lblMultiSMeter
            // 
            this.lblMultiSMeter.Image = null;
            resources.ApplyResources(this.lblMultiSMeter, "lblMultiSMeter");
            this.lblMultiSMeter.Name = "lblMultiSMeter";
            // 
            // txtMultiText
            // 
            this.txtMultiText.BackColor = System.Drawing.Color.Black;
            this.txtMultiText.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtMultiText, "txtMultiText");
            this.txtMultiText.ForeColor = System.Drawing.Color.Yellow;
            this.txtMultiText.Name = "txtMultiText";
            this.txtMultiText.ReadOnly = true;
            this.txtMultiText.GotFocus += new System.EventHandler(this.HideFocus);
            // 
            // picMultimeterAnalog
            // 
            this.picMultimeterAnalog.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.picMultimeterAnalog, "picMultimeterAnalog");
            this.picMultimeterAnalog.Name = "picMultimeterAnalog";
            this.picMultimeterAnalog.TabStop = false;
            // 
            // grpVFO
            // 
            this.grpVFO.Controls.Add(this.btnVFOA);
            this.grpVFO.Controls.Add(this.btnZeroBeat);
            this.grpVFO.Controls.Add(this.btnRITReset);
            this.grpVFO.Controls.Add(this.btnXITReset);
            this.grpVFO.Controls.Add(this.btnIFtoVFO);
            this.grpVFO.Controls.Add(this.btnVFOSwap);
            this.grpVFO.Controls.Add(this.btnVFOBtoA);
            this.grpVFO.Controls.Add(this.btnVFOAtoB);
            this.grpVFO.Controls.Add(this.udXIT);
            this.grpVFO.Controls.Add(this.chkXIT);
            this.grpVFO.Controls.Add(this.chkRIT);
            this.grpVFO.Controls.Add(this.udRIT);
            this.grpVFO.Controls.Add(this.chkVFOSplit);
            resources.ApplyResources(this.grpVFO, "grpVFO");
            this.grpVFO.Name = "grpVFO";
            this.grpVFO.TabStop = false;
            // 
            // grpSoundControls
            // 
            this.grpSoundControls.Controls.Add(this.tbPWR);
            this.grpSoundControls.Controls.Add(this.tbRF);
            this.grpSoundControls.Controls.Add(this.tbAF);
            this.grpSoundControls.Controls.Add(this.udRF);
            this.grpSoundControls.Controls.Add(this.lblRF);
            this.grpSoundControls.Controls.Add(this.lblPWR);
            this.grpSoundControls.Controls.Add(this.udPWR);
            this.grpSoundControls.Controls.Add(this.lblAF);
            this.grpSoundControls.Controls.Add(this.udAF);
            this.grpSoundControls.Controls.Add(this.comboPreamp);
            this.grpSoundControls.Controls.Add(this.lblPreamp);
            this.grpSoundControls.Controls.Add(this.lblAGC);
            this.grpSoundControls.Controls.Add(this.comboAGC);
            resources.ApplyResources(this.grpSoundControls, "grpSoundControls");
            this.grpSoundControls.Name = "grpSoundControls";
            this.grpSoundControls.TabStop = false;
            // 
            // lblRF
            // 
            this.lblRF.Image = null;
            resources.ApplyResources(this.lblRF, "lblRF");
            this.lblRF.Name = "lblRF";
            // 
            // udSquelch
            // 
            this.udSquelch.BackColor = System.Drawing.SystemColors.Window;
            this.udSquelch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.udSquelch.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.udSquelch, "udSquelch");
            this.udSquelch.Maximum = new decimal(new int[] {
            160,
            0,
            0,
            0});
            this.udSquelch.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udSquelch.Name = "udSquelch";
            this.udSquelch.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.udSquelch.ValueChanged += new System.EventHandler(this.udSquelch_ValueChanged);
            this.udSquelch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
            this.udSquelch.LostFocus += new System.EventHandler(this.udSquelch_LostFocus);
            // 
            // Console
            // 
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.grp3020);
            this.Controls.Add(this.grpG80);
            this.Controls.Add(this.grpG40);
            this.Controls.Add(this.grpG160);
            this.Controls.Add(this.grpG59Option);
            this.Controls.Add(this.chkUSB);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpLOSC);
            this.Controls.Add(this.grpSubRX);
            this.Controls.Add(this.grpBandHF);
            this.Controls.Add(this.grpModeSpecificPhone);
            this.Controls.Add(this.picSQL);
            this.Controls.Add(this.tbSQL);
            this.Controls.Add(this.grpModeSpecificDigital);
            this.Controls.Add(this.grpVFOBetween);
            this.Controls.Add(this.grpModeSpecificCW);
            this.Controls.Add(this.grpDisplay2);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.grpSoundControls);
            this.Controls.Add(this.grpVFO);
            this.Controls.Add(this.grpMultimeter);
            this.Controls.Add(this.grpDSP);
            this.Controls.Add(this.lblCPUMeter);
            this.Controls.Add(this.grpMode);
            this.Controls.Add(this.grpDisplay);
            this.Controls.Add(this.grpVFOA);
            this.Controls.Add(this.grpVFOB);
            this.Controls.Add(this.grpFilter);
            this.Controls.Add(this.chkPower);
            this.Controls.Add(this.chkSquelch);
            this.Controls.Add(this.udSquelch);
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.Name = "Console";
            this.ShowIcon = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Console_MouseWheel);
            this.SizeChanged += new System.EventHandler(this.Console_SizeChanged);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Console_Closing);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Console_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Console_KeyUp);
            this.Resize += new System.EventHandler(this.Console_Resize);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Console_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.tbDisplayPan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDisplayZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRX0Gain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPanSubRX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPanMainRX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRX1Gain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udCWPitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFilterHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFilterLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbFilterWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbFilterShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udXIT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udRIT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPWR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udAF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPWR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbAF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udRF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udCWSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udVACTXGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udVACRXGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSQL)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpG160.ResumeLayout(false);
            this.grpG59Option.ResumeLayout(false);
            this.grpG80.ResumeLayout(false);
            this.grp3020.ResumeLayout(false);
            this.contextMemoryMenu.ResumeLayout(false);
            this.contextLOSCMenu.ResumeLayout(false);
            this.grpLOSC.ResumeLayout(false);
            this.grpLOSC.PerformLayout();
            this.grpSubRX.ResumeLayout(false);
            this.grpBandHF.ResumeLayout(false);
            this.grpModeSpecificPhone.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udNoiseGate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udVOX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMIC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udCPDR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbCPDR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udCOMP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbCOMP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNoiseGate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbNoiseGate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVOX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbVOX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMIC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSQL)).EndInit();
            this.grpModeSpecificDigital.ResumeLayout(false);
            this.grpVACStereo.ResumeLayout(false);
            this.grpDIGSampleRate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbVACTXGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbVACRXGain)).EndInit();
            this.grpG40.ResumeLayout(false);
            this.grpVFOBetween.ResumeLayout(false);
            this.grpVFOBetween.PerformLayout();
            this.grpDisplay2.ResumeLayout(false);
            this.grpModeSpecificCW.ResumeLayout(false);
            this.grpCWPitch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbCWSpeed)).EndInit();
            this.grpOptions.ResumeLayout(false);
            this.grpVFOA.ResumeLayout(false);
            this.grpVFOA.PerformLayout();
            this.grpVFOB.ResumeLayout(false);
            this.grpVFOB.PerformLayout();
            this.grpDisplay.ResumeLayout(false);
            this.grpDisplay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWaterfall)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).EndInit();
            this.grpMode.ResumeLayout(false);
            this.grpFilter.ResumeLayout(false);
            this.grpDSP.ResumeLayout(false);
            this.grpMultimeter.ResumeLayout(false);
            this.grpMultimeter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMultiMeterDigital)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMultimeterAnalog)).EndInit();
            this.grpVFO.ResumeLayout(false);
            this.grpSoundControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udSquelch)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Main
        // ======================================================
        // Main
        // ======================================================

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                if (!File.Exists(Application.StartupPath + "\\wisdom"))
                {
                    Process p = Process.Start(Application.StartupPath + "\\fftw_wisdom.exe");
                    MessageBox.Show("Running one time optimization.  Please wait patiently for " +
                        "this process to finish.\nTypically the optimization takes no more than 3-5 minutes.",
                        "Optimizing...",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    p.WaitForExit();
                }

                try
                {
                    if (!CheckForOpenProcesses())
                        return;
                }
                catch (Exception)
                {

                }
                //				Application.EnableVisualStyles(); 
                //				Application.DoEvents(); 
                Application.Run(new Console(args));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace.ToString(), "Fatal Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void WndProc(ref Message m)
        {
            // Purpose    : Overrides WndProc to enable checking for and handling
            //            : WM_DEVICECHANGE(messages)
            // Accepts    : m - a Windows Message

            try
            {
                // The OnDeviceChange routine processes WM_DEVICECHANGE messages.
                if (m.Msg == WM_DEVICECHANGE)
                {
                    if (CurrentModel == Model.GENESIS_G59)
                    {
                        g59.OnDeviceChange(m);
                        if (g59.Connected)
                            chkUSB.BackColor = Color.GreenYellow;
                        else
                            chkUSB.BackColor = Color.Red;
                    }
                }

                // Let the base form process the message.
                base.WndProc(ref m);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "\n\n" + ex.StackTrace.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Console_Resize(object sender, EventArgs e)  // yt7pwr
        {
            if (this.Width >= 1024 && this.Height >= 768)
            {
                System.Drawing.Point grp_position = new System.Drawing.Point(this.Width - 178, 4);
                grpMultimeter.Location = grp_position;                  // multimeter grp
                grp_position.Y = (((grpDisplay.Location.Y + grpDisplay.Height) - 506) / 4)
                    + grpMultimeter.Location.Y + grpMultimeter.Height;
                grpBandHF.Location = grp_position;
                grp_position.Y = (((grpDisplay.Location.Y + grpDisplay.Height) - 506) / 4)
                    + grpBandHF.Location.Y + grpBandHF.Height;
                grpMode.Location = grp_position;
                grp_position.Y = (((grpDisplay.Location.Y + grpDisplay.Height) - 506) / 4)
                    + grpMode.Location.Y + grpMode.Height;
                grpFilter.Location = grp_position;
                grp_position = grpVFOBetween.Location;
                grp_position.X = ((this.Width - 1006) / 6) + 21;
                grp_position.Y = this.Height - 229;                     // VFO between
                grpVFOBetween.Location = grp_position;
                grp_position = grpVFO.Location;
                grp_position.X = ((this.Width - 1006) / 6) + grpVFOBetween.Location.X + grpVFOBetween.Width;
                grp_position.Y = this.Height - 229;                     // VFO grp
                grpVFO.Location = grp_position;
                grp_position = grpDSP.Location;
                grp_position.X = ((this.Width - 1006) / 6) + grpVFO.Location.X + grpVFO.Width;
                grp_position.Y = this.Height - 229;                     // DSP grp
                grpDSP.Location = grp_position;
                grp_position = grpSubRX.Location;
                grp_position.X = ((this.Width - 1006) / 6) + grpVFO.Location.X + grpVFO.Width;
                grp_position.Y = this.Height - 141;                     // subRX grp
                grpSubRX.Location = grp_position;
                grp_position = grpDisplay2.Location;
                grp_position.X = ((this.Width - 1006) / 6) + grpVFO.Location.X + grpVFO.Width + 128;
                grp_position.Y = this.Height - 229;                     // display type grp
                grpDisplay2.Location = grp_position;
                grp_position = grpModeSpecificPhone.Location;
                grp_position.X = ((this.Width - 1006) / 6) + grpDSP.Location.X + grpSubRX.Width;
                grp_position.Y = this.Height - 229;                     // Phone grp
                grpModeSpecificPhone.Location = grp_position;
                grpModeSpecificDigital.Location = grp_position;         // Digital grp
                grpModeSpecificCW.Location = grp_position;              // CW grp
                grp_position = groupBox2.Location;
                grp_position.X = ((this.Width - 1006) / 6) + grpModeSpecificPhone.Location.X + grpModeSpecificPhone.Width;
                grp_position.Y = this.Height - 229;                     // Zoom
                groupBox2.Location = grp_position;
                grp_position = textBox2.Location;
                grp_position.Y = this.Height - 360;
                textBox2.Location = grp_position;
                grp_position = textBox1.Location;
                grp_position.Y = this.Height - 360;
                textBox1.Location = grp_position;
                grp_position = txtDisplayCursorOffset.Location;
                grp_position.Y = this.Height - 360;
                txtDisplayCursorOffset.Location = grp_position;
                grp_position = txtDisplayCursorPower.Location;
                grp_position.Y = this.Height - 360;
                txtDisplayCursorPower.Location = grp_position;
                grp_position = txtDisplayCursorFreq.Location;
                grp_position.Y = this.Height - 360;
                txtDisplayCursorFreq.Location = grp_position;
                grp_position = txtDisplayPeakPower.Location;
                grp_position.Y = this.Height - 360;
                txtDisplayPeakPower.Location = grp_position;
                grp_position = txtDisplayPeakOffset.Location;
                grp_position.Y = this.Height - 360;
                txtDisplayPeakOffset.Location = grp_position;
                grp_position = txtDisplayPeakFreq.Location;
                grp_position.Y = this.Height - 360;

                switch (Display.CurrentDisplayMode)
                {
                    case DisplayMode.PANADAPTER:
                    case DisplayMode.PANAFALL:
                    case DisplayMode.OFF:
                    case DisplayMode.PHASE:
                    case DisplayMode.PHASE2:
                    case DisplayMode.SCOPE:
                    case DisplayMode.SPECTRUM:
                    case DisplayMode.HISTOGRAM:
                        txtDisplayPeakFreq.Width = picDisplay.Width - (textBox2.Width + textBox1.Width +
                            txtDisplayCursorOffset.Width + txtDisplayCursorPower.Width + txtDisplayCursorFreq.Width +
                            txtDisplayPeakPower.Width + txtDisplayPeakOffset.Width);
                        break;
                    case DisplayMode.WATERFALL:
                        txtDisplayPeakFreq.Width = picWaterfall.Width - (textBox2.Width + textBox1.Width +
                            txtDisplayCursorOffset.Width + txtDisplayCursorPower.Width + txtDisplayCursorFreq.Width +
                            txtDisplayPeakPower.Width + txtDisplayPeakOffset.Width);
                        break;
                }

                txtDisplayPeakFreq.Location = grp_position;
                grp_position = grpLOSC.Location;
                grp_position.X = grpMultimeter.Location.X - 211;
                grpLOSC.Location = grp_position;

                if (comboDisplayMode.Text == "Panadapter" || comboDisplayMode.Text == "Phase" || comboDisplayMode.Text == "Waterfall" ||
                    comboDisplayMode.Text == "Phase2" || comboDisplayMode.Text == "Scope" || comboDisplayMode.Text == "Spectrum"||
                    comboDisplayMode.Text == "Histogram")
                {
                    System.Drawing.Point picDisplay_position = new System.Drawing.Point(10, 15);
                    grpDisplay.Height = this.Height - grpVFO.Height - 180;    // picDisplay
                    grpDisplay.Width = this.Width - grpBandHF.Width - 140;
                    picDisplay.Location = picDisplay_position;
                    picDisplay.Height = (grpDisplay.Height - 40);
                    picDisplay.Width = grpDisplay.Width - 19;
                }
                if (comboDisplayMode.Text == "Waterfall")
                {
                    System.Drawing.Point picWaterfall_position = new System.Drawing.Point(10, 15);
                    grpDisplay.Height = this.Height - grpVFO.Height - 180;    // waterfall
                    grpDisplay.Width = this.Width - grpBandHF.Width - 140;
                    picWaterfall.Location = picWaterfall_position;
                    picWaterfall.Height = (grpDisplay.Height - 40);
                    picWaterfall.Width = grpDisplay.Width - 19;
                }

                if (comboDisplayMode.Text == "Panafall" || comboDisplayMode.Text == "Off")
                {
                    System.Drawing.Point picWaterfall_position = new System.Drawing.Point(10, 15);
                    grpDisplay.Height = this.Height - grpVFO.Height - 180;
                    grpDisplay.Width = this.Width - grpBandHF.Width - 140;
                    picWaterfall.Location = picWaterfall_position;

                    picWaterfall.Height = (grpDisplay.Height - 40) / 2;       // panafall           
                    picWaterfall.Width = grpDisplay.Width - 19;
                    grp_position = picWaterfall.Location;
                    grp_position.Y = picWaterfall.Height + 15;
                    picDisplay.Location = grp_position;
                    picDisplay.Height = (grpDisplay.Height - 40) / 2;
                    picDisplay.Width = grpDisplay.Width - 19;
                }

                grp_position = grpVFOA.Location;                         // VFOA
                grp_position.X = ((grpDisplay.Width - 615) / 4) + 116;
                grpVFOA.Location = grp_position;
                grp_position.X = ((grpDisplay.Width - 615) / 4) + (grpVFOA.Location.X + grpVFOA.Width);
                grpVFOB.Location = grp_position;                        // VFOB
                grp_position.X = ((grpDisplay.Width - 615) / 4) + (grpVFOB.Location.X + grpVFOB.Width);
                grpLOSC.Location = grp_position;                        // LOSC
            }
        }

        #endregion

        #region Misc Routines
        // ======================================================
        // Misc Routines
        // ======================================================

        public static double zoom_disp=1;

        public void TX_phase_gain() // yt7pwr
        {
            if (!booting)
            {
                string band = current_band.ToString();
                double vfo = Math.Round(VFOAFreq, 3);
                double losc = Math.Round(LOSCFreq, 3);
                double tmp_freq = Math.Round((losc - vfo) * 1000, 2);
                string vfoa = String.Format("{0:G}", tmp_freq);

                foreach (string s in TXphase_gain)				// string is in the format "freq,value"
                {
                    string[] vals = s.Split('/');

                    if (vals[0].Equals(band) && vals[1].Equals(vfoa))
                    {
                        string value = vals[2];
                        Double.TryParse(value, out tx_phase);
                        if (tx_phase > -400 && tx_phase < 400)
                            SetupForm.ImagePhaseTX = tx_phase;
                        value = vals[3];
                        Double.TryParse(value, out tx_gain);
                        if (tx_gain > -500 && tx_gain < 500)
                            SetupForm.ImageGainTX = tx_gain;
                    }
                }
            }
        }

        public void Refresh_TX_phase_gain() // yt7pwr
        {
            TXphase_gain = DB.GetTX_Phase_Gain();
            TXphase_gain.Sort();
        }

        public void Refresh_RX_phase_gain() // yt7pwr
        {
            RXphase_gain = DB.GetRX_Phase_Gain();
            RXphase_gain.Sort();
        }

        public void RX_phase_gain() // yt7pwr
        {
            if (!booting)
            {
                string band = current_band.ToString();
                double vfoA = Math.Round(VFOAFreq, 3);
                double losc = Math.Round(LOSCFreq, 3);
                double tmp_freq = Math.Round((losc - vfoA) * 1000, 2);
                string vfo = String.Format("{0:G}", tmp_freq);

                foreach (string s in RXphase_gain)				// string is in the format "freq,value"
                {
                    string[] vals = s.Split('/');

                    if (vals[0].Equals(band) && vals[1].Equals(vfo))
                    {
                        string value = vals[2];
                        Double.TryParse(value, out rx_phase);
                        if (rx_phase > -250 && rx_phase < 250)
                            SetupForm.ImagePhaseRX = rx_phase;
                        value = vals[3];
                        Double.TryParse(value, out rx_gain);
                        if (rx_gain > -250 && rx_gain < 250)
                            SetupForm.ImageGainRX = rx_gain;
                        return;
                    }
                }
            }
        }


        public static double zoom_disp_return
        {
            get {return zoom_disp;}
        }

        private void InitConsole()
        {
            g59 = new GenesisG59.G59(Handle);
            booting = true;
            g59.booting = true;

            UpdateBandStackRegisters();

            Audio.console = this;
            chkDSPNB2.Enabled = true;
            Display.console = this;

            losc_hover_digit = -1;
            vfoa_hover_digit = -1;
            vfob_hover_digit = -1;


            atu_tuning = false;
            tune_power = 10;
            calibrating = false;
            run_setup_wizard = true;

            // get culture specific decimal separator
            separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            //dsp_display_mutex = new Mutex();
            //dsp_meter_mutex = new Mutex();
            //pa_power_mutex = new Mutex();
            //high_swr_mutex = new Mutex();

            last_band = "";						// initialize bandstack

            wheel_tune_list = new double[13];		// initialize wheel tuning list array
            wheel_tune_list[0] = 0.000001;
            wheel_tune_list[1] = 0.000010;
            wheel_tune_list[2] = 0.000050;
            wheel_tune_list[3] = 0.000100;
            wheel_tune_list[4] = 0.000250;
            wheel_tune_list[5] = 0.000500;
            wheel_tune_list[6] = 0.001000;
            wheel_tune_list[7] = 0.005000;
            wheel_tune_list[8] = 0.009000;
            wheel_tune_list[9] = 0.010000;
            wheel_tune_list[10] = 0.100000;
            wheel_tune_list[11] = 1.000000;
            wheel_tune_list[12] = 10.000000;
            wheel_tune_index = 6;

            meter_text_history = new float[multimeter_text_peak_samples];

            current_meter_data = -200.0f;

            preamp_offset = new float[4];
            preamp_offset[(int)PreampMode.OFF] = 10.0f;
            preamp_offset[(int)PreampMode.LOW] = 0.0f;
            preamp_offset[(int)PreampMode.MED] = -16.0f;
            preamp_offset[(int)PreampMode.HIGH] = -26.0f;

            this.ActiveControl = chkPower;		// Power has focus initially

            Display.Target = picDisplay;
            Display.Init();						// Initialize Display variables
            InitDisplayModes();					// Initialize Display Modes
            InitAGCModes();						// Initialize AGC Modes
            InitMultiMeterModes();				// Initialize MultiMeter Modes

            audio_process_thread = new Thread(	// create audio process thread
                new ThreadStart(DttSP.ProcessSamplesThread));
            audio_process_thread.Name = "Audio Process Thread";
            audio_process_thread.Priority = ThreadPriority.Highest;
            audio_process_thread.IsBackground = true;
            audio_process_thread.Start();

            siolisten = new SIOListenerII(this);

            Keyer = new CWKeyer2(this);			// create new Keyer
            EQForm = new EQForm();

            InitFilterPresets();					// Initialize filter values

            SI570 = new ExtIO_si570_usb(this);

            SetupForm = new Setup(this);		// create Setup form
            SetupForm.StartPosition = FormStartPosition.Manual;

            SetupForm.GetTxProfiles();
            UpdateTXProfile();

            EQForm.RestoreSettings();

            WaveForm = new WaveControl(this);	// create Wave form
            WaveForm.StartPosition = FormStartPosition.Manual;

            CurrentAGCMode = AGCMode.MED;				// Initialize front panel controls
            comboPreamp.Text = "High";
            vfob_dsp_mode = DSPMode.LSB;
            vfob_filter = Filter.F3;
            comboDisplayMode.Text = "Panadapter";
            comboMeterRXMode.SelectedIndex = 0;
            udPWR.Value = 50;

            GetState();							// recall saved state

            if (current_dsp_mode == DSPMode.FIRST || current_dsp_mode == DSPMode.LAST)
                radModeLSB.Checked = true;
            if (current_filter == Filter.FIRST || current_filter == Filter.LAST ||
                (current_filter == Filter.NONE && current_dsp_mode != DSPMode.DRM && current_dsp_mode != DSPMode.SPEC))
                radFilter3.Checked = true;

            txtLOSCFreq_LostFocus(this, EventArgs.Empty);
            txtVFOAFreq_LostFocus(this, EventArgs.Empty);
            txtVFOBFreq_LostFocus(this, EventArgs.Empty);
            udPWR_ValueChanged(this, EventArgs.Empty);
            udAF_ValueChanged(this, EventArgs.Empty);
            udMIC_ValueChanged(this, EventArgs.Empty);
            CurrentPreampMode = current_preamp_mode;
            tbRX0Gain_Scroll(this, EventArgs.Empty);
            tbRX1Gain_Scroll(this, EventArgs.Empty);
            tbPanMainRX_Scroll(this, EventArgs.Empty);
            tbPanSubRX_Scroll(this, EventArgs.Empty);
            tbDisplayZoom.Value = 4;
            CalcDisplayFreq();

            wheel_tune_index--;					// Setup wheel tuning
            ChangeWheelTuneLeft();

            SetupForm.initCATandPTTprops();   // wjt added -- get console props setup for cat and ptt 
            if (CmdLineArgs != null)
            {
                for (int i = 0; i < CmdLineArgs.Length; i++)
                {
                    switch (CmdLineArgs[i])
                    {
                        case "--disable-swr-prot-at-my-risk":
                            DisableSWRProtection = true;
                            this.Text = this.Text + "  *** SWR Protection Disabled! ***";
                            break;
                        case "--high-pwr-am":
                            Audio.high_pwr_am = true;
                            MessageBox.Show("high power am");
                            break;
                    }
                }
            }

            if (comboMeterTXMode.Items.Count > 0 && comboMeterTXMode.SelectedIndex < 0)
                comboMeterTXMode.SelectedIndex = 0;
            chkMOX.Enabled = false;

            btnDisplayZoom1x.BackColor = button_selected_color;
            btnDisplayZoom2x.BackColor = SystemColors.Control;
            btnDisplayZoom4x.BackColor = SystemColors.Control;
            btnDisplayZoom8x.BackColor = SystemColors.Control;

            booting = false;
            g59.booting = false;
            if (CurrentModel == Model.GENESIS_G59 && !SetupForm.chkGeneralUSBPresent.Checked)
            {
                bool conn = false;
                conn = g59.Connect();
                if (conn)
                    chkUSB.BackColor = Color.GreenYellow;
                else
                    chkUSB.BackColor = Color.Red;
            }
            else if(SetupForm.chkGeneralUSBPresent.Checked)
            {
                SI570.Init_USB();
            }

            txtMemory_fill();

            #region Genesis
            booting = false;
            SetTXOscFreqs(false,false);

            btnG3020_X1.Text = G3020Xtal1.ToString();
            btnG3020_X2.Text = G3020Xtal2.ToString();
            btnG3020_X3.Text = G3020Xtal3.ToString();
            btnG3020_X4.Text = G3020Xtal4.ToString();
            btnG160_X1.Text = G160Xtal1.ToString();
            btnG160_X2.Text = G160Xtal2.ToString();
            btnG80_X1.Text = G80Xtal1.ToString();
            btnG80_X2.Text = G80Xtal2.ToString();
            btnG80_X3.Text = G80Xtal3.ToString();
            btnG80_X4.Text = G80Xtal4.ToString();
            btnG40_X1.Text = G40Xtal1.ToString();
            #endregion
        }

        public void ExitConsole()
        {
            g59.Disconnect();

            if (SetupForm != null)		// make sure Setup form is deallocated
                SetupForm.Dispose();
            if (CWXForm != null)			// make sure CWX form is deallocated
                CWXForm.Dispose();
            chkPower.Checked = false;	// make sure power is off		

            //			if(draw_display_thread != null)
            //				draw_display_thread.Abort();
            PA19.PA_Terminate();		// terminate audio interface
            DB.Exit();					// close and save database
            //Mixer.RestoreState();		// restore initial mixer state
            DttSP.Exit();				// deallocate DSP variables
        }

        public void SaveState()
        {
            // Automatically saves all control settings to the database in the tab
            // pages on this form of the following types: CheckBox, ComboBox,
            // NumericUpDown, RadioButton, TextBox, and TrackBar (slider)

            chkPower.Checked = false;		// turn off the power first

            ArrayList a = new ArrayList();

            foreach (Control c in this.Controls)			// For each control
            {
                if (c.GetType() == typeof(GroupBoxTS))		// if it is a groupbox, check for sub controls
                {
                    foreach (Control c2 in ((GroupBoxTS)c).Controls)	// for each sub-control
                    {	// check to see if it is a value type we need to save
                        if (c2.Enabled)
                        {
                            if (c2.GetType() == typeof(CheckBoxTS))
                                a.Add(c2.Name + "/" + ((CheckBoxTS)c2).Checked.ToString());
                            else if (c2.GetType() == typeof(ComboBoxTS))
                            {
                                if (((ComboBoxTS)c2).Items.Count > 0)
                                    a.Add(c2.Name + "/" + ((ComboBoxTS)c2).Text);
                            }
                            else if (c2.GetType() == typeof(NumericUpDownTS))
                                a.Add(c2.Name + "/" + ((NumericUpDownTS)c2).Value.ToString());
                            else if (c2.GetType() == typeof(RadioButtonTS))
                                a.Add(c2.Name + "/" + ((RadioButtonTS)c2).Checked.ToString());
                            else if (c2.GetType() == typeof(TextBoxTS))
                            {
                                if (((TextBoxTS)c2).ReadOnly == false)
                                    a.Add(c2.Name + "/" + ((TextBoxTS)c2).Text);
                            }
                            else if (c2.GetType() == typeof(TrackBarTS))
                                a.Add(c2.Name + "/" + ((TrackBarTS)c2).Value.ToString());
#if(DEBUG)
                            else if (c2.GetType() == typeof(GroupBox) ||
                                c2.GetType() == typeof(CheckBox) ||
                                c2.GetType() == typeof(ComboBox) ||
                                c2.GetType() == typeof(NumericUpDown) ||
                                c2.GetType() == typeof(RadioButton) ||
                                c2.GetType() == typeof(TextBox) ||
                                c2.GetType() == typeof(TrackBar))
                                Debug.WriteLine(c2.Name + " needs to be converted to a Thread Safe control.");
#endif
                        }
                    }
                }
                else // it is not a group box
                {	// check to see if it is a value type we need to save
                    if (c.Enabled)
                    {
                        if (c.GetType() == typeof(CheckBoxTS))
                            a.Add(c.Name + "/" + ((CheckBoxTS)c).Checked.ToString());
                        else if (c.GetType() == typeof(ComboBoxTS))
                        {
                            if (((ComboBoxTS)c).SelectedIndex >= 0)
                                a.Add(c.Name + "/" + ((ComboBoxTS)c).Text);
                        }
                        else if (c.GetType() == typeof(NumericUpDownTS))
                            a.Add(c.Name + "/" + ((NumericUpDownTS)c).Value.ToString());
                        else if (c.GetType() == typeof(RadioButtonTS))
                            a.Add(c.Name + "/" + ((RadioButtonTS)c).Checked.ToString());
                        else if (c.GetType() == typeof(TextBoxTS))
                        {
                            if (((TextBoxTS)c).ReadOnly == false)
                                a.Add(c.Name + "/" + ((TextBoxTS)c).Text);
                        }
                        else if (c.GetType() == typeof(TrackBarTS))
                            a.Add(c.Name + "/" + ((TrackBarTS)c).Value.ToString());
#if(DEBUG)
                        else if (c.GetType() == typeof(GroupBox) ||
                            c.GetType() == typeof(CheckBox) ||
                            c.GetType() == typeof(ComboBox) ||
                            c.GetType() == typeof(NumericUpDown) ||
                            c.GetType() == typeof(RadioButton) ||
                            c.GetType() == typeof(TextBox) ||
                            c.GetType() == typeof(TrackBar))
                            Debug.WriteLine(c.Name + " needs to be converted to a Thread Safe control.");
#endif
                    }
                }
            }

            a.Add("current_datetime_mode/" + (int)current_datetime_mode);
            a.Add("display_cal_offset/" + display_cal_offset.ToString("f3"));
            a.Add("multimeter_cal_offset/" + multimeter_cal_offset);

            for (int m = (int)DSPMode.FIRST + 1; m < (int)DSPMode.LAST; m++)
            {	// save filter settings per mode
                for (Filter f = Filter.FIRST + 1; f < Filter.LAST; f++)
                {
                    a.Add("filter_presets[" + m.ToString() + "][" + ((int)f).ToString() + "]/" + filter_presets[m].ToString(f));
                }
                a.Add("last_filter[" + m.ToString() + "]/" + filter_presets[m].LastFilter.ToString());
            }

            a.Add("band_160m_index/" + band_160m_index.ToString());
            a.Add("band_80m_index/" + band_80m_index.ToString());
            a.Add("band_60m_index/" + band_60m_index.ToString());
            a.Add("band_40m_index/" + band_40m_index.ToString());
            a.Add("band_30m_index/" + band_30m_index.ToString());
            a.Add("band_20m_index/" + band_20m_index.ToString());
            a.Add("band_17m_index/" + band_17m_index.ToString());
            a.Add("band_15m_index/" + band_15m_index.ToString());
            a.Add("band_12m_index/" + band_12m_index.ToString());
            a.Add("band_10m_index/" + band_10m_index.ToString());
            a.Add("band_6m_index/" + band_6m_index.ToString());
            a.Add("band_2m_index/" + band_2m_index.ToString());
            a.Add("band_wwv_index/" + band_wwv_index.ToString());
            a.Add("band_gen_index/" + band_gen_index.ToString());

            for (int i = (int)PreampMode.FIRST + 1; i < (int)PreampMode.LAST; i++)
                a.Add("preamp_offset[" + i.ToString() + "]/" + preamp_offset[i].ToString());

            a.Add("wheel_tune_index/" + wheel_tune_index.ToString());		// Save wheel tune value

            a.Add("vfob_dsp_mode/" + ((int)vfob_dsp_mode).ToString());			// Save VFO B values 
            a.Add("vfob_filter/" + ((int)vfob_filter).ToString());

            a.Add("console_top/" + this.Top.ToString());		// save form positions
            a.Add("console_left/" + this.Left.ToString());
            a.Add("setup_top/" + SetupForm.Top.ToString());
            a.Add("setup_left/" + SetupForm.Left.ToString());

            a.Add("Version/" + this.Text);		// save the current version

            DB.SaveVars("State", ref a);		// save the values to the DB
        }

        public void GetState()
        {
            // Automatically restores all controls from the database in the
            // tab pages on this form of the following types: CheckBox, ComboBox,
            // NumericUpDown, RadioButton, TextBox, and TrackBar (slider)

            ArrayList checkbox_list = new ArrayList();
            ArrayList combobox_list = new ArrayList();
            ArrayList numericupdown_list = new ArrayList();
            ArrayList radiobutton_list = new ArrayList();
            ArrayList textbox_list = new ArrayList();
            ArrayList trackbar_list = new ArrayList();

            //ArrayList controls = new ArrayList();	// list of controls to restore
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(GroupBoxTS))	// if control is a groupbox, retrieve all subcontrols
                {
                    foreach (Control c2 in ((GroupBoxTS)c).Controls)
                    {
                        if (c2.Enabled)
                        {
                            if (c2.GetType() == typeof(CheckBoxTS))			// the control is a CheckBox
                                checkbox_list.Add(c2);
                            else if (c2.GetType() == typeof(ComboBoxTS))		// the control is a ComboBox
                                combobox_list.Add(c2);
                            else if (c2.GetType() == typeof(NumericUpDownTS))	// the control is a NumericUpDown
                                numericupdown_list.Add(c2);
                            else if (c2.GetType() == typeof(RadioButtonTS))	// the control is a RadioButton
                                radiobutton_list.Add(c2);
                            else if (c2.GetType() == typeof(TextBoxTS))		// the control is a TextBox
                                textbox_list.Add(c2);
                            else if (c2.GetType() == typeof(TrackBarTS))		// the control is a TrackBar (slider)
                                trackbar_list.Add(c2);
                        }
                    }
                }
                else
                {
                    if (c.Enabled)
                    {
                        if (c.GetType() == typeof(CheckBoxTS))				// the control is a CheckBox
                            checkbox_list.Add(c);
                        else if (c.GetType() == typeof(ComboBoxTS))		// the control is a ComboBox
                            combobox_list.Add(c);
                        else if (c.GetType() == typeof(NumericUpDownTS))	// the control is a NumericUpDown
                            numericupdown_list.Add(c);
                        else if (c.GetType() == typeof(RadioButtonTS))		// the control is a RadioButton
                            radiobutton_list.Add(c);
                        else if (c.GetType() == typeof(TextBoxTS))			// the control is a TextBox
                            textbox_list.Add(c);
                        else if (c.GetType() == typeof(TrackBarTS))		// the control is a TrackBar (slider)
                            trackbar_list.Add(c);
                    }
                }
            }

            ArrayList a = DB.GetVars("State");							// Get the saved list of controls
            a.Sort();
            int num_controls = checkbox_list.Count + combobox_list.Count +
                numericupdown_list.Count + radiobutton_list.Count +
                textbox_list.Count + trackbar_list.Count;

            foreach (string s in a)				// string is in the format "name,value"
            {
                string[] vals = s.Split('/');
                if (vals.Length > 2)
                {
                    for (int i = 2; i < vals.Length; i++)
                        vals[1] += "/" + vals[i];
                }

                string name = vals[0];
                string val = vals[1];
                int num = 0;

                if (name.StartsWith("filter_presets["))
                {
                    int start = name.IndexOf("[") + 1;
                    int length = name.IndexOf("]") - start;
                    int mode_index = Int32.Parse(name.Substring(start, length));

                    start = name.LastIndexOf("[") + 1;
                    length = name.LastIndexOf("]") - start;
                    int filter_mode = Int32.Parse(name.Substring(start, length));

                    length = val.IndexOf(":");
                    string n = val.Substring(0, length);

                    start = val.IndexOf(":") + 2;
                    length = val.IndexOf(",") - start;
                    int low = Int32.Parse(val.Substring(start, length));

                    start = val.IndexOf(",") + 1;
                    int high = Int32.Parse(val.Substring(start));

                    filter_presets[mode_index].SetFilter((Filter)filter_mode, low, high, n);
                }
                else if (name.StartsWith("last_filter["))
                {
                    int start = name.IndexOf("[") + 1;
                    int length = name.IndexOf("]") - start;
                    int mode_index = Int32.Parse(name.Substring(start, length));

                    filter_presets[mode_index].LastFilter = (Filter)Enum.Parse(typeof(Filter), val);
                }
                else if (name.StartsWith("preamp_offset["))
                {
                    int start = name.IndexOf("[") + 1;
                    int length = name.IndexOf("]") - start;
                    int index = Int32.Parse(name.Substring(start, length));

                    preamp_offset[index] = float.Parse(val);
                }

                switch (name)
                {
                    case "band_160m_index":
                        band_160m_index = Int32.Parse(val);
                        break;
                    case "band_80m_index":
                        band_80m_index = Int32.Parse(val);
                        break;
                    case "band_60m_index":
                        band_60m_index = Int32.Parse(val);
                        break;
                    case "band_40m_index":
                        band_40m_index = Int32.Parse(val);
                        break;
                    case "band_30m_index":
                        band_30m_index = Int32.Parse(val);
                        break;
                    case "band_20m_index":
                        band_20m_index = Int32.Parse(val);
                        break;
                    case "band_17m_index":
                        band_17m_index = Int32.Parse(val);
                        break;
                    case "band_15m_index":
                        band_15m_index = Int32.Parse(val);
                        break;
                    case "band_12m_index":
                        band_12m_index = Int32.Parse(val);
                        break;
                    case "band_10m_index":
                        band_10m_index = Int32.Parse(val);
                        break;
                    case "band_6m_index":
                        band_6m_index = Int32.Parse(val);
                        break;
                    case "band_2m_index":
                        band_2m_index = Int32.Parse(val);
                        break;
                    case "band_wwv_index":
                        band_wwv_index = Int32.Parse(val);
                        break;
                    case "band_gen_index":
                        band_gen_index = Int32.Parse(val);
                        break;
                    case "current_datetime_mode":
                        CurrentDateTimeMode = (DateTimeMode)(Int32.Parse(val));
                        break;
                    case "wheel_tune_index":
                        wheel_tune_index = Int32.Parse(val);
                        break;
                    case "display_cal_offset":
                        DisplayCalOffset = float.Parse(val);
                        break;
                    case "multimeter_cal_offset":
                        multimeter_cal_offset = float.Parse(val);
                        break;
                    case "vfob_dsp_mode":
                        vfob_dsp_mode = (DSPMode)(Int32.Parse(val));
                        break;
                    case "vfob_filter":
                        vfob_filter = (Filter)(Int32.Parse(val));
                        break;
                    case "console_top":
                        num = Int32.Parse(val);
                        if ((num < 0) || (num > Screen.PrimaryScreen.Bounds.Height))
                            num = 0;
                        this.Top = num;
                        break;
                    case "console_left":
                        num = Int32.Parse(val);
                        if ((num < 0) || (num > Screen.PrimaryScreen.Bounds.Width))
                            num = 0;
                        this.Left = num;
                        break;
                    case "setup_top":
                        num = Int32.Parse(val);
                        if ((num < 0) || (num > Screen.PrimaryScreen.Bounds.Height))
                            num = 0;
                        SetupForm.Top = num;
                        break;
                    case "setup_left":
                        num = Int32.Parse(val);
                        if ((num < 0) || (num > Screen.PrimaryScreen.Bounds.Width))
                            num = 0;
                        SetupForm.Left = num;
                        break;
                    case "SetupWizard":
                        if (val == "1")
                            run_setup_wizard = false;
                        break;
                }
            }

            // restore saved values to the controls
            foreach (string s in a)				// string is in the format "name,value"
            {
                string[] vals = s.Split('/');
                string name = vals[0];
                string val = vals[1];

                if (s.StartsWith("chk"))			// control is a CheckBox
                {
                    for (int i = 0; i < checkbox_list.Count; i++)
                    {	// look through each control to find the matching name
                        CheckBoxTS c = (CheckBoxTS)checkbox_list[i];
                        if (c.Name.Equals(name))		// name found
                        {
                            c.Checked = bool.Parse(val);	// restore value
                            i = checkbox_list.Count + 1;
                        }
                        if (i == checkbox_list.Count)
                            MessageBox.Show("Control not found: " + name, "GetState Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (s.StartsWith("combo"))	// control is a ComboBox
                {
                    for (int i = 0; i < combobox_list.Count; i++)
                    {	// look through each control to find the matching name
                        ComboBoxTS c = (ComboBoxTS)combobox_list[i];
                        if (c.Name.Equals(name))		// name found
                        {
                            c.Text = val;	// restore value
                            i = combobox_list.Count + 1;
                        }
                        if (i == combobox_list.Count)
                            MessageBox.Show("Control not found: " + name, "GetState Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (s.StartsWith("ud"))
                {
                    for (int i = 0; i < numericupdown_list.Count; i++)
                    {	// look through each control to find the matching name
                        NumericUpDownTS c = (NumericUpDownTS)numericupdown_list[i];
                        if (c.Name.Equals(name))		// name found
                        {
                            decimal num = decimal.Parse(val);

                            if (num > c.Maximum) num = c.Maximum;		// check endpoints
                            else if (num < c.Minimum) num = c.Minimum;
                            c.Value = num;			// restore value
                            i = numericupdown_list.Count + 1;
                        }
                        if (i == numericupdown_list.Count)
                            MessageBox.Show("Control not found: " + name, "GetState Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (s.StartsWith("rad"))
                {	// look through each control to find the matching name
                    for (int i = 0; i < radiobutton_list.Count; i++)
                    {
                        RadioButtonTS c = (RadioButtonTS)radiobutton_list[i];
                        if (c.Name.Equals(name))		// name found
                        {
                            if (!val.ToLower().Equals("true") && !val.ToLower().Equals("false"))
                                val = "True";
                            c.Checked = bool.Parse(val);	// restore value
                            i = radiobutton_list.Count + 1;
                        }
                        if (i == radiobutton_list.Count)
                            MessageBox.Show("Control not found: " + name, "GetState Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (s.StartsWith("txt"))
                {	// look through each control to find the matching name
                    for (int i = 0; i < textbox_list.Count; i++)
                    {
                        TextBoxTS c = (TextBoxTS)textbox_list[i];
                        if (c.Name.Equals(name))		// name found
                        {
                            c.Text = val;	// restore value
                            i = textbox_list.Count + 1;
                        }
                        if (i == textbox_list.Count)
                            MessageBox.Show("Control not found: " + name, "GetState Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (s.StartsWith("tb"))
                {
                    // look through each control to find the matching name
                    for (int i = 0; i < trackbar_list.Count; i++)
                    {
                        TrackBarTS c = (TrackBarTS)trackbar_list[i];
                        if (c.Name.Equals(name))		// name found
                        {
                            c.Value = Int32.Parse(val);
                            i = trackbar_list.Count + 1;
                        }
                        if (i == trackbar_list.Count)
                            MessageBox.Show("Control not found: " + name, "GetState Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        public FilterPreset[] filter_presets = new FilterPreset[(int)DSPMode.LAST];
        private void InitFilterPresets()
        {
            // used to initialize all the filter variables
            for (int m = (int)DSPMode.FIRST + 1; m < (int)DSPMode.LAST; m++)
            {
                filter_presets[m] = new FilterPreset();
                for (Filter f = Filter.F1; f != Filter.LAST; f++)
                {
                    switch (m)
                    {
                        case (int)DSPMode.LSB:
                        case (int)DSPMode.DIGL:
                            switch (f)
                            {
                                case Filter.F1:
                                    filter_presets[m].SetFilter(f, -5150, -150, "5.0k");
                                    break;
                                case Filter.F2:
                                    filter_presets[m].SetFilter(f, -4550, -150, "4.4k");
                                    break;
                                case Filter.F3:
                                    filter_presets[m].SetFilter(f, -3950, -150, "3.8k");
                                    break;
                                case Filter.F4:
                                    filter_presets[m].SetFilter(f, -3450, -150, "3.3k");
                                    break;
                                case Filter.F5:
                                    filter_presets[m].SetFilter(f, -3050, -150, "2.9k");
                                    break;
                                case Filter.F6:
                                    filter_presets[m].SetFilter(f, -2850, -150, "2.7k");
                                    break;
                                case Filter.F7:
                                    filter_presets[m].SetFilter(f, -2550, -150, "2.4k");
                                    break;
                                case Filter.F8:
                                    filter_presets[m].SetFilter(f, -2250, -150, "2.1k");
                                    break;
                                case Filter.F9:
                                    filter_presets[m].SetFilter(f, -1950, -150, "1.8k");
                                    break;
                                case Filter.F10:
                                    filter_presets[m].SetFilter(f, -1150, -150, "1.0k");
                                    break;
                                case Filter.VAR1:
                                    filter_presets[m].SetFilter(f, -2850, -150, "Var 1");
                                    break;
                                case Filter.VAR2:
                                    filter_presets[m].SetFilter(f, -2850, -150, "Var 2");
                                    break;
                            }
                            filter_presets[m].LastFilter = Filter.F6;
                            break;
                        case (int)DSPMode.USB:
                        case (int)DSPMode.DIGU:
                            switch (f)
                            {
                                case Filter.F1:
                                    filter_presets[m].SetFilter(f, 150, 5150, "5.0k");
                                    break;
                                case Filter.F2:
                                    filter_presets[m].SetFilter(f, 150, 4550, "4.4k");
                                    break;
                                case Filter.F3:
                                    filter_presets[m].SetFilter(f, 150, 3950, "3.8k");
                                    break;
                                case Filter.F4:
                                    filter_presets[m].SetFilter(f, 150, 3450, "3.3k");
                                    break;
                                case Filter.F5:
                                    filter_presets[m].SetFilter(f, 150, 3050, "2.9k");
                                    break;
                                case Filter.F6:
                                    filter_presets[m].SetFilter(f, 150, 2850, "2.7k");
                                    break;
                                case Filter.F7:
                                    filter_presets[m].SetFilter(f, 150, 2550, "2.4k");
                                    break;
                                case Filter.F8:
                                    filter_presets[m].SetFilter(f, 150, 2250, "2.1k");
                                    break;
                                case Filter.F9:
                                    filter_presets[m].SetFilter(f, 150, 1950, "1.8k");
                                    break;
                                case Filter.F10:
                                    filter_presets[m].SetFilter(f, 150, 1150, "1.0k");
                                    break;
                                case Filter.VAR1:
                                    filter_presets[m].SetFilter(f, 150, 2850, "Var 1");
                                    break;
                                case Filter.VAR2:
                                    filter_presets[m].SetFilter(f, 150, 2850, "Var 2");
                                    break;
                            }
                            filter_presets[m].LastFilter = Filter.F6;
                            break;
                        case (int)DSPMode.CWL:
                            switch (f)
                            {
                                case Filter.F1:
                                    filter_presets[m].SetFilter(f, -cw_pitch - 500, -cw_pitch + 500, "1.0k");
                                    break;
                                case Filter.F2:
                                    filter_presets[m].SetFilter(f, -cw_pitch - 400, -cw_pitch + 400, "800");
                                    break;
                                case Filter.F3:
                                    filter_presets[m].SetFilter(f, -cw_pitch - 375, -cw_pitch + 375, "750");
                                    break;
                                case Filter.F4:
                                    filter_presets[m].SetFilter(f, -cw_pitch - 300, -cw_pitch + 300, "600");
                                    break;
                                case Filter.F5:
                                    filter_presets[m].SetFilter(f, -cw_pitch - 250, -cw_pitch + 250, "500");
                                    break;
                                case Filter.F6:
                                    filter_presets[m].SetFilter(f, -cw_pitch - 200, -cw_pitch + 200, "400");
                                    break;
                                case Filter.F7:
                                    filter_presets[m].SetFilter(f, -cw_pitch - 125, -cw_pitch + 125, "250");
                                    break;
                                case Filter.F8:
                                    filter_presets[m].SetFilter(f, -cw_pitch - 50, -cw_pitch + 50, "100");
                                    break;
                                case Filter.F9:
                                    filter_presets[m].SetFilter(f, -cw_pitch - 25, -cw_pitch + 25, "50");
                                    break;
                                case Filter.F10:
                                    filter_presets[m].SetFilter(f, -cw_pitch - 13, -cw_pitch + 13, "25");
                                    break;
                                case Filter.VAR1:
                                    filter_presets[m].SetFilter(f, -cw_pitch - 250, -cw_pitch + 250, "Var 1");
                                    break;
                                case Filter.VAR2:
                                    filter_presets[m].SetFilter(f, -cw_pitch - 250, -cw_pitch + 250, "Var 2");
                                    break;
                            }
                            filter_presets[m].LastFilter = Filter.F5;
                            break;
                        case (int)DSPMode.CWU:
                            switch (f)
                            {
                                case Filter.F1:
                                    filter_presets[m].SetFilter(f, cw_pitch - 500, cw_pitch + 500, "1.0k");
                                    break;
                                case Filter.F2:
                                    filter_presets[m].SetFilter(f, cw_pitch - 400, cw_pitch + 400, "800");
                                    break;
                                case Filter.F3:
                                    filter_presets[m].SetFilter(f, cw_pitch - 375, cw_pitch + 375, "750");
                                    break;
                                case Filter.F4:
                                    filter_presets[m].SetFilter(f, cw_pitch - 300, cw_pitch + 300, "600");
                                    break;
                                case Filter.F5:
                                    filter_presets[m].SetFilter(f, cw_pitch - 250, cw_pitch + 250, "500");
                                    break;
                                case Filter.F6:
                                    filter_presets[m].SetFilter(f, cw_pitch - 200, cw_pitch + 200, "400");
                                    break;
                                case Filter.F7:
                                    filter_presets[m].SetFilter(f, cw_pitch - 125, cw_pitch + 125, "250");
                                    break;
                                case Filter.F8:
                                    filter_presets[m].SetFilter(f, cw_pitch - 50, cw_pitch + 50, "100");
                                    break;
                                case Filter.F9:
                                    filter_presets[m].SetFilter(f, cw_pitch - 25, cw_pitch + 25, "50");
                                    break;
                                case Filter.F10:
                                    filter_presets[m].SetFilter(f, cw_pitch - 13, cw_pitch + 13, "25");
                                    break;
                                case Filter.VAR1:
                                    filter_presets[m].SetFilter(f, cw_pitch - 250, cw_pitch + 250, "Var 1");
                                    break;
                                case Filter.VAR2:
                                    filter_presets[m].SetFilter(f, cw_pitch - 250, cw_pitch + 250, "Var 2");
                                    break;
                            }
                            filter_presets[m].LastFilter = Filter.F5;
                            break;
                        case (int)DSPMode.AM:
                        case (int)DSPMode.SAM:
                        case (int)DSPMode.FMN:
                        case (int)DSPMode.DSB:
                            switch (f)
                            {
                                case Filter.F1:
                                    filter_presets[m].SetFilter(f, -8000, 8000, "16k");
                                    break;
                                case Filter.F2:
                                    filter_presets[m].SetFilter(f, -6000, 6000, "12k");
                                    break;
                                case Filter.F3:
                                    filter_presets[m].SetFilter(f, -5000, 5000, "10k");
                                    break;
                                case Filter.F4:
                                    filter_presets[m].SetFilter(f, -4000, 4000, "8.0k");
                                    break;
                                case Filter.F5:
                                    filter_presets[m].SetFilter(f, -3300, 3300, "6.6k");
                                    break;
                                case Filter.F6:
                                    filter_presets[m].SetFilter(f, -2600, 2600, "5.2k");
                                    break;
                                case Filter.F7:
                                    filter_presets[m].SetFilter(f, -2000, 2000, "4.0k");
                                    break;
                                case Filter.F8:
                                    filter_presets[m].SetFilter(f, -1550, 1550, "3.1k");
                                    break;
                                case Filter.F9:
                                    filter_presets[m].SetFilter(f, -1450, 1450, "2.9k");
                                    break;
                                case Filter.F10:
                                    filter_presets[m].SetFilter(f, -1200, 1200, "2.4k");
                                    break;
                                case Filter.VAR1:
                                    filter_presets[m].SetFilter(f, -3300, 3300, "Var 1");
                                    break;
                                case Filter.VAR2:
                                    filter_presets[m].SetFilter(f, -3300, 3300, "Var 2");
                                    break;
                            }
                            filter_presets[m].LastFilter = Filter.F5;
                            break;
                        default:
                            filter_presets[m].LastFilter = Filter.NONE;
                            break;
                    }
                }
            }
        }

        private void InitDisplayModes()
        {
            // populate the display mode list
            for (DisplayMode dm = DisplayMode.FIRST + 1; dm < DisplayMode.LAST; dm++)
            {
                string s = dm.ToString().ToLower();
                s = s.Substring(0, 1).ToUpper() + s.Substring(1, s.Length - 1);
                comboDisplayMode.Items.Add(s);
            }
        }

        private void InitAGCModes()
        {
            // populate the AGC mode list
            for (AGCMode agc = AGCMode.FIRST + 1; agc < AGCMode.LAST; agc++)
            {
                string s = agc.ToString().ToLower();
                s = s.Substring(0, 1).ToUpper() + s.Substring(1, s.Length - 1);
                comboAGC.Items.Add(s);
            }
        }

        private void InitMultiMeterModes()
        {
            comboMeterRXMode.Items.Add("Signal");
            comboMeterRXMode.Items.Add("Sig Avg");
            comboMeterRXMode.Items.Add("ADC L");
            comboMeterRXMode.Items.Add("ADC R");
            comboMeterRXMode.Items.Add("Off");

            comboMeterTXMode.Items.Add("Fwd Pwr");
            comboMeterTXMode.Items.Add("Ref Pwr");
            comboMeterTXMode.Items.Add("SWR");
            comboMeterTXMode.Items.Add("Mic");
            comboMeterTXMode.Items.Add("EQ");
            comboMeterTXMode.Items.Add("Leveler");
            comboMeterTXMode.Items.Add("Lev Gain");
            comboMeterTXMode.Items.Add("COMP");
            comboMeterTXMode.Items.Add("CPDR");
            comboMeterTXMode.Items.Add("ALC");
            comboMeterTXMode.Items.Add("ALC Comp");
            comboMeterTXMode.Items.Add("Off");
        }

        private void DisableAllFilters()
        {
            foreach (Control c in grpFilter.Controls)
            {
                if (c.GetType() == typeof(RadioButtonTS))
                {
                    c.Enabled = false;
                }
            }
        }

        private void EnableAllFilters()
        {
            foreach (Control c in grpFilter.Controls)
            {
                if (c.GetType() == typeof(RadioButtonTS))
                {
                    c.Enabled = true;

                    if (c.BackColor == vfo_text_dark_color)
                    {
                        c.BackColor = button_selected_color;
                    }
                }
            }
        }

        private void DisableAllBands()
        {
            foreach (ButtonTS b in grpBandHF.Controls)
            {
                b.Enabled = false;

                if (b.BackColor == button_selected_color)
                {
                    b.BackColor = vfo_text_dark_color;
                }
            }

        }

        private void EnableAllBands()
        {
            foreach (ButtonTS b in grpBandHF.Controls)
            {
                b.Enabled = true;

                if (b.BackColor == vfo_text_dark_color)
                    b.BackColor = button_selected_color;
            }
        }

        private void DisableAllModes()
        {
            foreach (RadioButtonTS r in grpMode.Controls)
            {
                r.Enabled = false;
                if (r.BackColor == button_selected_color)
                    r.BackColor = Color.Olive;
            }
        }

        private void EnableAllModes()
        {
            foreach (RadioButtonTS r in grpMode.Controls)
            {
                if (r.Text != "")
                    r.Enabled = true;
                if (r.BackColor == Color.Olive)
                    r.BackColor = button_selected_color;
            }
        }

        private void DisableFilters(int lowcutoff)
        {
            // Disables all filters below the number passed in. 
            // For example, DisableFilters(100) would cause the
            // 100Hz, 50Hz and 25Hz filters to be disabled.

            foreach (Control c in grpFilter.Controls)
            {
                if (c.GetType() == typeof(RadioButtonTS) && c.Name.IndexOf("Var") < 0)
                {
                    string name = c.Name;
                    int begin, len;
                    begin = name.IndexOf("Filter") + 6;
                    len = name.Length - begin;

                    int filter_width = Int32.Parse(name.Substring(begin, len));
                    if (filter_width < lowcutoff)
                    {
                        c.Enabled = false;
                        ((RadioButtonTS)c).Checked = false;
                    }
                }
            }
        }

        private void GetLOSCCharWidth()
        {
            Graphics g = txtLOSCFreq.CreateGraphics();

            SizeF size = g.MeasureString("0", txtLOSCFreq.Font, 1000, StringFormat.GenericTypographic);
            losc_char_width = (int)Math.Round(size.Width - 2.0f, 0);
            float float_char_width = size.Width - 2.0f;

            size = g.MeasureString("00", txtLOSCFreq.Font, 1000, StringFormat.GenericTypographic);
            losc_char_space = (int)Math.Round(size.Width - 2.0f - 2 * float_char_width, 0);

            size = g.MeasureString(separator, txtLOSCFreq.Font, 1000, StringFormat.GenericTypographic);
            losc_decimal_width = (int)(size.Width - 2.0f);

            size = g.MeasureString("0" + separator + "0", txtLOSCFreq.Font, 1000, StringFormat.GenericTypographic);
            losc_decimal_space = (int)Math.Round(size.Width - 2.0f - 2 * float_char_width, 0);

            size = g.MeasureString("1234.678901", txtLOSCFreq.Font, 1000, StringFormat.GenericTypographic);
            losc_pixel_offset = (int)Math.Round(size.Width - 2.0f, 0);

            size = g.MeasureString("0", txtLOSCLSD.Font, 1000, StringFormat.GenericTypographic);
            losc_small_char_width = (int)Math.Round(size.Width - 2.0f, 0);
            float_char_width = size.Width - 2.0f;

            size = g.MeasureString("00", txtLOSCLSD.Font, 1000, StringFormat.GenericTypographic);
            losc_small_char_space = (int)Math.Round(size.Width - 2.0f - 2 * float_char_width, 0);

            g.Dispose();
        }

        private void GetVFOCharWidth()
        {
            // This function calculates the pixel width of the VFO display.
            // This information is used for mouse wheel hover tuning.

            Graphics g = txtVFOAFreq.CreateGraphics();

            SizeF size = g.MeasureString("0", txtVFOAFreq.Font, 1000, StringFormat.GenericTypographic);
            vfo_char_width = (int)Math.Round(size.Width - 2.0f, 0);	// subtract 2 since measure string includes 1 pixel border on each side
            float float_char_width = size.Width - 2.0f;

            size = g.MeasureString("00", txtVFOAFreq.Font, 1000, StringFormat.GenericTypographic);
            vfo_char_space = (int)Math.Round(size.Width - 2.0f - 2 * float_char_width, 0);

            size = g.MeasureString(separator, txtVFOAFreq.Font, 1000, StringFormat.GenericTypographic);
            vfo_decimal_width = (int)(size.Width - 2.0f);

            size = g.MeasureString("0" + separator + "0", txtVFOAFreq.Font, 1000, StringFormat.GenericTypographic);
            vfo_decimal_space = (int)Math.Round(size.Width - 2.0f - 2 * float_char_width, 0);

            size = g.MeasureString("1234.678901", txtVFOAFreq.Font, 1000, StringFormat.GenericTypographic);
            vfo_pixel_offset = (int)Math.Round(size.Width - 2.0f, 0);

            size = g.MeasureString("0", txtVFOALSD.Font, 1000, StringFormat.GenericTypographic);
            vfo_small_char_width = (int)Math.Round(size.Width - 2.0f, 0);
            float_char_width = size.Width - 2.0f;

            size = g.MeasureString("00", txtVFOALSD.Font, 1000, StringFormat.GenericTypographic);
            vfo_small_char_space = (int)Math.Round(size.Width - 2.0f - 2 * float_char_width, 0);

            g.Dispose();
        }

        private void SaveBand() // changes yt7pwr
        {
            // Used in Bandstacking algorithm
            double freqA = Math.Round(VFOAFreq, 6);
            double freqB = Math.Round(VFOBFreq, 6);
            double losc_freq = Math.Round(LOSCFreq, 6);
            string filter = current_filter.ToString();
            string mode = current_dsp_mode.ToString();

            switch (current_band)
            {
                case Band.B160M:
                    if (freqA >= 1.8 && freqA < 2.0)
                        DB.SaveBandStack("160M", band_160m_index, mode, filter, freqA, freqB, losc_freq);
                    break;
                case Band.B80M:
                    if (freqA >= 3.5 && freqA < 4.0)
                        DB.SaveBandStack("80M", band_80m_index, mode, filter, freqA, freqB, losc_freq);
                    break;
                case Band.B60M:
                    if (extended)
                    {
                        if (freqA >= 5.0 && freqA < 6.0)
                            DB.SaveBandStack("60M", band_60m_index, "USB", filter, freqA, freqB, losc_freq);
                    }
                    else
                    {
                        if (freqA == 5.3305 || freqA == 5.3465 || freqA == 5.3665 || freqA == 5.3715 || freqA == 5.4035)
                            DB.SaveBandStack("60M", band_60m_index, "USB", filter, freqA, freqB, losc_freq);
                    }
                    break;
                case Band.B40M:
                    if (freqA >= 7.0 && freqA < 7.3)
                        DB.SaveBandStack("40M", band_40m_index, mode, filter, freqA, freqB, losc_freq);
                    break;
                case Band.B30M:
                    if (freqA >= 10.1 && freqA < 10.15)
                        DB.SaveBandStack("30M", band_30m_index, mode, filter, freqA, freqB, losc_freq);
                    break;
                case Band.B20M:
                    if (freqA >= 14.0 && freqA < 14.350)
                        DB.SaveBandStack("20M", band_20m_index, mode, filter, freqA, freqB, losc_freq);
                    break;
                case Band.B17M:
                    if (freqA >= 18.068 && freqA < 18.168)
                        DB.SaveBandStack("17M", band_17m_index, mode, filter, freqA, freqB, losc_freq);
                    break;
                case Band.B15M:
                    if (freqA >= 21.0 && freqA < 21.45)
                        DB.SaveBandStack("15M", band_15m_index, mode, filter, freqA, freqB, losc_freq);
                    break;
                case Band.B12M:
                    if (freqA >= 24.890 && freqA < 24.990)
                        DB.SaveBandStack("12M", band_12m_index, mode, filter, freqA, freqB, losc_freq);
                    break;
                case Band.B10M:
                    if (freqA >= 28.0 && freqA < 29.7)
                        DB.SaveBandStack("10M", band_10m_index, mode, filter, freqA, freqB, losc_freq);
                    break;
                case Band.B6M:
                    if (freqA >= 50.0 && freqA < 54.0)
                        DB.SaveBandStack("6M", band_6m_index, mode, filter, freqA, freqB, losc_freq);
                    break;
                case Band.B2M:
                    if (freqA >= 144.0 && freqA < 146.0)
                        DB.SaveBandStack("2M", band_2m_index, mode, filter, freqA, freqB, losc_freq);
                    break;
                case Band.WWV:
                    if (freqA == 2.5 || freqA == 5.0 || freqA == 10.0 || freqA == 15.0 || freqA == 20.0)
                        DB.SaveBandStack("WWV", band_wwv_index, mode, filter, freqA, freqB, losc_freq);
                    break;
                case Band.GEN:
                    DB.SaveBandStack("GEN", band_gen_index, mode, filter, freqA, freqB, losc_freq);
                    break;
            }
        }

        private void SetBand(string mode, string filter, double freqA, double freqB, double losc_freq) // changes yt7pwr
        {
            // Set mode, filter, and frequency according to passed parameters
            CurrentDSPMode = (DSPMode)Enum.Parse(typeof(DSPMode), mode, true);

            if (current_dsp_mode != DSPMode.DRM &&
                current_dsp_mode != DSPMode.SPEC)
            {
                CurrentFilter = (Filter)Enum.Parse(typeof(Filter), filter, true);
            }

            LOSCFreq = losc_freq;
            VFOAFreq = freqA;
            VFOBFreq = freqB;
        }

        public void MemoryRecall(int mode, int filter, double freq, double losc, int step, int agc, int squelch) // changes yt7pwr
        {
            // Set mode, filter, and frequency, mouse wheel tune step
            // and AGC according to passed parameters

            SaveBand();
            last_band = "";
            CurrentDSPMode = (DSPMode)mode;
            if (current_dsp_mode != DSPMode.DRM &&
                current_dsp_mode != DSPMode.SPEC)
                CurrentFilter = (Filter)filter;
            LOSCFreq = Math.Round(losc, 6);
            VFOAFreq = Math.Round(freq, 6);
            txtVFOAFreq_LostFocus(this, EventArgs.Empty);
            comboAGC.SelectedIndex = agc;
            udSquelch.Value = squelch;
            wheel_tune_index = step;
            switch (wheel_tune_index)
            {
                case 0:
                    txtWheelTune.Text = "1Hz";
                    break;
                case 1:
                    txtWheelTune.Text = "10Hz";
                    break;
                case 2:
                    txtWheelTune.Text = "50Hz";
                    break;
                case 3:
                    txtWheelTune.Text = "100Hz";
                    break;
                case 4:
                    txtWheelTune.Text = "250Hz";
                    break;
                case 5:
                    txtWheelTune.Text = "500Hz";
                    break;
                case 6:
                    txtWheelTune.Text = "1kHz";
                    break;
                case 7:
                    txtWheelTune.Text = "5kHz";
                    break;
                case 8:
                    txtWheelTune.Text = "10kHz";
                    break;
                case 9:
                    txtWheelTune.Text = "100kHz";
                    break;
                case 10:
                    txtWheelTune.Text = "1MHz";
                    break;
                case 11:
                    txtWheelTune.Text = "10MHz";
                    break;
            }
        }

        private void ChangeWheelTuneLeft()
        {
            // change mouse wheel tuning step one digit to the left
            wheel_tune_index = (wheel_tune_index + 1) % wheel_tune_list.Length;
            switch (wheel_tune_index)
            {
                case 0:
                    txtWheelTune.Text = "1Hz";
                    break;
                case 1:
                    txtWheelTune.Text = "10Hz";
                    break;
                case 2:
                    txtWheelTune.Text = "50Hz";
                    break;
                case 3:
                    txtWheelTune.Text = "100Hz";
                    break;
                case 4:
                    txtWheelTune.Text = "250Hz";
                    break;
                case 5:
                    txtWheelTune.Text = "500Hz";
                    break;
                case 6:
                    txtWheelTune.Text = "1kHz";
                    break;
                case 7:
                    txtWheelTune.Text = "5kHz";
                    break;
                case 8:
                    txtWheelTune.Text = "9kHz";
                    break;
                case 9:
                    txtWheelTune.Text = "10kHz";
                    break;
                case 10:
                    txtWheelTune.Text = "100kHz";
                    break;
                case 11:
                    txtWheelTune.Text = "1MHz";
                    break;
                case 12:
                    txtWheelTune.Text = "10MHz";
                    break;
            }
        }

        private void ChangeWheelTuneRight()
        {
            // change mouse wheel tuning step one digit to the right
            int length = wheel_tune_list.Length;
            wheel_tune_index = (wheel_tune_index - 1 + length) % length;
            switch (wheel_tune_index)
            {
                case 0:
                    txtWheelTune.Text = "1Hz";
                    break;
                case 1:
                    txtWheelTune.Text = "10Hz";
                    break;
                case 2:
                    txtWheelTune.Text = "50Hz";
                    break;
                case 3:
                    txtWheelTune.Text = "100Hz";
                    break;
                case 4:
                    txtWheelTune.Text = "250Hz";
                    break;
                case 5:
                    txtWheelTune.Text = "500Hz";
                    break;
                case 6:
                    txtWheelTune.Text = "1kHz";
                    break;
                case 7:
                    txtWheelTune.Text = "5kHz";
                    break;
                case 8:
                    txtWheelTune.Text = "9kHz";
                    break;
                case 9:
                    txtWheelTune.Text = "10kHz";
                    break;
                case 10:
                    txtWheelTune.Text = "100kHz";
                    break;
                case 11:
                    txtWheelTune.Text = "1MHz";
                    break;
                case 12:
                    txtWheelTune.Text = "10MHz";
                    break;
            }
        }

        private void SetBandButtonColor(Band b)
        {
            // Sets band button color based on passed band.

            Button btn = null;
            switch (b)
            {
                case Band.GEN:
                    btn = btnBandGEN;
                    break;
                case Band.B160M:
                    btn = btnBand160;
                    current_band_USB = 1;
                    break;
                case Band.B80M:
                    btn = btnBand80;
                    current_band_USB = 1;
                    break;
                case Band.B60M:
                    btn = btnBand60;
                    current_band_USB = 2;
                    break;
                case Band.B40M:
                    btn = btnBand40;
                    current_band_USB = 2;
                    break;
                case Band.B30M:
                    btn = btnBand30;
                    current_band_USB = 3;
                    break;
                case Band.B20M:
                    btn = btnBand20;
                    current_band_USB = 3;
                    break;
                case Band.B17M:
                    btn = btnBand17;
                    current_band_USB = 4;
                    break;
                case Band.B15M:
                    btn = btnBand15;
                    current_band_USB = 4;
                    break;
                case Band.B12M:
                    btn = btnBand12;
                    current_band_USB = 5;
                    break;
                case Band.B10M:
                    btn = btnBand10;
                    current_band_USB = 5;
                    break;
                case Band.B6M:
                    btn = btnBand6;
                    current_band_USB = 6;
                    break;
                case Band.B2M:
                    btn = btnBand2;
                    current_band_USB = 7;
                    break;
                case Band.WWV:
                    btn = btnBandWWV;
                    break;
            }

            if (b < Band.VHF0)
            {
                if (!grpBandHF.Visible)
                {
                }


                foreach (Button b2 in grpBandHF.Controls)
                {
                    Color c = SystemColors.Control;
                    if (b2 == btn)
                        c = button_selected_color;

                    b2.BackColor = c;
                }
            }
            else
            {

                foreach (Button b2 in grpBandHF.Controls)
                    b2.BackColor = SystemColors.Control;
            }
        }

        private Band BandByFreq(float freq)
        {
            Band b = Band.GEN;
            if (current_xvtr_index >= 0)
            {
                b = (Band)(Band.VHF0 + current_xvtr_index);
            }
            else if (!extended)
            {
                if (freq >= 1.8 && freq <= 2.0)
                    b = Band.B160M;
                else if (freq >= 3.5 && freq <= 4.0)
                    b = Band.B80M;
                else if (freq == 5.3305f || freq == 5.3465f ||
                    freq == 5.3665f || freq == 5.3715f ||
                    freq == 5.4035f)
                    b = Band.B60M;
                else if (freq >= 7.0f && freq <= 7.3f)
                    b = Band.B40M;
                else if (freq >= 10.1f && freq <= 10.15f)
                    b = Band.B30M;
                else if (freq >= 14.0f && freq <= 14.35f)
                    b = Band.B20M;
                else if (freq >= 18.068f && freq <= 18.168f)
                    b = Band.B17M;
                else if (freq >= 21.0f && freq <= 21.450f)
                    b = Band.B15M;
                else if (freq >= 24.89f && freq <= 24.99f)
                    b = Band.B12M;
                else if (freq >= 28.0f && freq <= 29.7f)
                    b = Band.B10M;
                else if (freq >= 50.0f && freq <= 54.0f)
                    b = Band.B6M;
                else if (freq >= 144.0f && freq <= 148.0f)
                    b = Band.B2M;
                else if (freq == 2.5f || freq == 5.0f ||
                    freq == 10.0f || freq == 15.0f ||
                    freq == 20.0f)
                    b = Band.WWV;
                else
                    b = Band.GEN;
            }
            else
            {
                if (freq >= 0.0 && freq <= 2.75)
                    b = Band.B160M;
                else if (freq > 2.75 && freq <= 5.3305)
                    b = Band.B80M;
                else if (freq > 5.3305 && freq < 7.0)
                    b = Band.B60M;
                else if (freq >= 7.0 && freq <= 8.7)
                    b = Band.B40M;
                else if (freq >= 8.7 && freq <= 12.075)
                    b = Band.B30M;
                else if (freq >= 12.075 && freq <= 16.209)
                    b = Band.B20M;
                else if (freq >= 16.209 && freq <= 19.584)
                    b = Band.B17M;
                else if (freq >= 19.584 && freq <= 23.17)
                    b = Band.B15M;
                else if (freq >= 23.17 && freq <= 26.495)
                    b = Band.B12M;
                else if (freq >= 26.495 && freq <= 29.7)
                    b = Band.B10M;
                else if (freq >= 50.0f && freq <= 54.0f)
                    b = Band.B6M;
                else if (freq >= 144.0f && freq <= 148.0f)
                    b = Band.B2M;
                else if (freq == 2.5f || freq == 5.0f ||
                    freq == 10.0f || freq == 15.0f ||
                    freq == 20.0f)
                    b = Band.WWV;
                else
                    b = Band.GEN;
            }
            return b;
        }

        private void SetCurrentBand(Band b)
        {
            if (CurrentBand != b)
            {
                if (chkVFOSplit.Checked)
                    chkVFOSplit.Checked = false;
            }

            CurrentBand = b;
            if (tuned_band != b &&
                tuned_band != Band.FIRST)
            {
                tuned_band = Band.FIRST;
                chkTUN.BackColor = SystemColors.Control;
            }
            if (!comboPreamp.Items.Contains("Off"))
                comboPreamp.Items.Insert(0, "Off");
            if (!comboPreamp.Items.Contains("Med"))
                comboPreamp.Items.Insert(2, "Med");
        }

        private float GainByBand(Band b)
        {
            float retval = 0;
            switch (b)
            {
                case Band.B160M:
                    retval = SetupForm.PAGain160;
                    break;
                case Band.B80M:
                    retval = SetupForm.PAGain80;
                    break;
                case Band.B60M:
                    retval = SetupForm.PAGain60;
                    break;
                case Band.B40M:
                    retval = SetupForm.PAGain40;
                    break;
                case Band.B30M:
                    retval = SetupForm.PAGain30;
                    break;
                case Band.B20M:
                    retval = SetupForm.PAGain20;
                    break;
                case Band.B17M:
                    retval = SetupForm.PAGain17;
                    break;
                case Band.B15M:
                    retval = SetupForm.PAGain15;
                    break;
                case Band.B12M:
                    retval = SetupForm.PAGain12;
                    break;
                case Band.B10M:
                    retval = SetupForm.PAGain10;
                    break;
                default:
                    retval = 1000;
                    break;
            }

            return retval;
        }

        public void CheckSelectedButtonColor()
        {
            // used when changing the background color of selected buttons
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(GroupBoxTS))
                {
                    foreach (Control c2 in ((GroupBoxTS)c).Controls)
                    {
                        if (c2.GetType() == typeof(RadioButtonTS))
                        {
                            RadioButtonTS r = (RadioButtonTS)c2;
                            if (r.Checked && r.BackColor != SystemColors.Control)
                            {
                                c2.BackColor = button_selected_color;
                            }
                        }
                        else if (c2.GetType() == typeof(CheckBoxTS))
                        {
                            CheckBoxTS chk = (CheckBoxTS)c2;
                            if (chk.Checked && chk.BackColor != SystemColors.Control)
                            {
                                c2.BackColor = button_selected_color;
                            }
                        }
                        else if (c2.GetType() == typeof(NumericUpDownTS))
                        {
                            NumericUpDownTS ud = (NumericUpDownTS)c2;
                            if (ud.BackColor != SystemColors.Window)
                            {
                                c2.BackColor = button_selected_color;
                            }
                        }
                        else if (c2.GetType() == typeof(ButtonTS))
                        {
                            ButtonTS b = (ButtonTS)c2;
                            if (b.BackColor != SystemColors.Control)
                            {
                                c2.BackColor = button_selected_color;
                            }
                        }
                    }
                }
                else if (c.GetType() == typeof(RadioButtonTS))
                {
                    RadioButtonTS r = (RadioButtonTS)c;
                    if (r.Checked && r.BackColor != SystemColors.Control)
                        c.BackColor = button_selected_color;
                }
                else if (c.GetType() == typeof(CheckBoxTS))
                {
                    CheckBoxTS chk = (CheckBoxTS)c;
                    if (chk.Checked && chk.BackColor != SystemColors.Control)
                        c.BackColor = button_selected_color;
                }
                else if (c.GetType() == typeof(NumericUpDownTS))
                {
                    NumericUpDownTS ud = (NumericUpDownTS)c;
                    if (ud.BackColor != SystemColors.Window)
                        c.BackColor = button_selected_color;
                }
                else if (c.GetType() == typeof(ButtonTS))
                {
                    ButtonTS b = (ButtonTS)c;
                    if (b.BackColor != SystemColors.Control)
                        c.BackColor = button_selected_color;
                }
            }
        }

        private double PABandOffset(Band b)
        {
            double num = 0;
            switch (b)
            {
                case Band.B160M:
                    num = SetupForm.PAADC160;
                    break;
                case Band.B80M:
                    num = SetupForm.PAADC80;
                    break;
                case Band.B60M:
                    num = SetupForm.PAADC60;
                    break;
                case Band.B40M:
                    num = SetupForm.PAADC40;
                    break;
                case Band.B30M:
                    num = SetupForm.PAADC30;
                    break;
                case Band.B20M:
                    num = SetupForm.PAADC20;
                    break;
                case Band.B17M:
                    num = SetupForm.PAADC17;
                    break;
                case Band.B15M:
                    num = SetupForm.PAADC15;
                    break;
                case Band.B12M:
                    num = SetupForm.PAADC12;
                    break;
                case Band.B10M:
                    num = SetupForm.PAADC10;
                    break;
            }

            if (num == 0) return 0;
            //return 100000 / Math.Pow(num, 2);
            return (double)108 / num;
        }

        private double SWR(int adc_fwd, int adc_rev)
        {
            if (adc_fwd == 0 && adc_rev == 0)
                return 1.0;
            else if (adc_rev > adc_fwd)
                return 50.0;

            double Ef = ScaledVoltage(adc_fwd);
            double Er = ScaledVoltage(adc_rev);

            double swr = (Ef + Er) / (Ef - Er);

            return swr;
        }

        private double ScaledVoltage(int adc)
        {
            double v_det = adc * 0.062963;			// scale factor in V/bit including pot ratio
            double v_out = v_det * 10.39853;		// scale factor in V/V for bridge output to detector voltage
            return v_out * PABandOffset(CurrentBand);
            //double v_det = adc * 0.0304;
            //			double v_out = 0;
            //			if(v_det >= 1.6)
            //				v_out = (-0.241259304*v_det+12.07915098)*v_det*PABandOffset(CurrentBand);
            //			else if(v_det > 0.35)
            //				v_out = (1/Math.Pow(v_det, 2)+11.3025111)*v_det*PABandOffset(CurrentBand);
            //return v_out;
        }

        private double ADCtodBm(int adc_data)
        {
            if (adc_data == 0)
                return 0;

            double mult = 100000 / Math.Pow(225 / PABandOffset(CurrentBand), 2);
            return 10 * Math.Log10(mult * Math.Pow(adc_data, 2));
        }

        private double PAPower(int adc)
        {
            double v_out = ScaledVoltage(adc);
            double pow = Math.Pow(v_out, 2) / 50;
            pow = Math.Max(pow, 0.0);
            return pow;
        }

        private double WattsTodBm(double watts)
        {
            return 10 * Math.Log10(watts / 0.001);
        }

        private double dBmToWatts(double dBm)
        {
            return Math.Pow(10, dBm / 10) * 0.001;
        }

        private static bool CheckForOpenProcesses()
        {
            // find all open PowerSDR processes
            Process[] p = Process.GetProcessesByName("PowerSDR");
            if (p.Length > 1)
            {
                DialogResult dr = MessageBox.Show("There are other PowerSDR instances running.\n" +
                    "Are you sure you want to continue?",
                    "Continue?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    return false;
                }
            }
            return true;
        }

        public int VersionTextToInt(string version)	// takes a version string like "1.0.6" 
        {											// and converts it to an int like 010006.
            string[] nums = version.Split('.');
            if (nums.Length < 3 || nums.Length > 4) return -1;

            int num1 = Int32.Parse(nums[0]);
            int num2 = Int32.Parse(nums[1]);
            int num3 = Int32.Parse(nums[2]);
            int num4 = 0;
            if (nums.Length == 4) num4 = Int32.Parse(nums[3]);

            return num1 * 1000000 + num2 * 10000 + num3 * 100 + num4;
        }

        public bool IsHamBand(BandPlan b, double f)
        {
            if (extended || current_xvtr_index > -1)
                return true;

            switch (b)
            {
                case BandPlan.IARU1:
                    if (f >= 1.8 && f <= 2.0) return true;
                    else if (f >= 3.5 && f <= 4.0) return true;
                    else if (f == 5.3305) return true;
                    else if (f == 5.3465) return true;
                    else if (f == 5.3665) return true;
                    else if (f == 5.3715) return true;
                    else if (f == 5.4035) return true;
                    else if (f >= 7.0 && f <= 7.3) return true;
                    else if (f >= 10.1 && f <= 10.15) return true;
                    else if (f >= 14.0 && f <= 14.35) return true;
                    else if (f >= 18.068 && f <= 18.168) return true;
                    else if (f >= 21.0 && f <= 21.45) return true;
                    else if (f >= 24.89 && f <= 24.99) return true;
                    else if (f >= 21.0 && f <= 21.45) return true;
                    else if (f >= 28.0 && f <= 29.7) return true;
                    else if (f >= 50.0 && f <= 54.0) return true;
                    else if (f >= 144.0 && f <= 146.0) return true;
                    else return false;
                default:
                    return false;
                // TODO: Implement other bandplans here
            }
        }

        // kb9yig sr40 mod 		
        // check and see if the band data includes alias data -- if so 
        // zero out (very negative) the portions of the data that are 
        // aliased 
        public void AdjustDisplayDataForBandEdge(ref float[] display_data) // changes yt7pwr
        {
            if ( current_model != Model.GENESIS_G59 && current_model != Model.GENESIS_G80 
                && current_model != Model.GENESIS_G40 && current_model != Model.GENESIS_G3020 
                && current_model != Model.GENESIS_G160)  // -- no aliasing going on 
                return;

            if (current_dsp_mode == DSPMode.DRM)  // for now don't worry about aliasing in DRM land 
            {
                return;
            }

            double losc_freq = double.Parse(txtLOSCFreq.Text);
            double hz_per_bin = DttSP.SampleRate / Display.BUFFER_SIZE;
            double data_center_freq = losc_freq;
            if (data_center_freq == 0)
            {
                return;
            }
            double data_low_edge_hz = (1e6 * data_center_freq) - DttSP.SampleRate / 2;
            double data_high_edge_hz = (1e6 * data_center_freq) + DttSP.SampleRate / 2;
            double alias_free_low_edge_hz = (1e6 * losc_freq) - DttSP.SampleRate / 2;
            double alias_free_high_edge_hz = (1e6 * losc_freq) + DttSP.SampleRate / 2;
            if (data_low_edge_hz < alias_free_low_edge_hz)   // data we have goes below alias free region -- zero it 
            {
                double hz_this_bin = data_low_edge_hz;
                int bin_num = 0;
                while (hz_this_bin < alias_free_low_edge_hz)
                {
                    display_data[bin_num] = -200.0f;
                    ++bin_num;
                    hz_this_bin += hz_per_bin;
                }
                // Debug.WriteLine("data_low: " + bin_num); 
            }
            else if (data_high_edge_hz > alias_free_high_edge_hz)
            {
                double hz_this_bin = data_high_edge_hz;
                int bin_num = Display.BUFFER_SIZE - 1;
                while (hz_this_bin > alias_free_high_edge_hz)
                {
                    display_data[bin_num] = -200.0f;
                    --bin_num;
                    hz_this_bin -= hz_per_bin;
                }
                // Debug.WriteLine("data_high: " + bin_num); 
            }
            return;
        }
        // end kb9yig sr40 mod 

        public void SelectVarFilter()
        {
            if (current_filter == Filter.VAR1) return;
            if (current_filter == Filter.VAR2) return;

            // save current filter bounds, reset to var, set filter bounds 
            int high = (int)udFilterHigh.Value;
            int low = (int)udFilterLow.Value;
            radFilterVar1.Checked = true;
            //SetFilter(Filter.VAR1); 
            UpdateFilters(low, high);
        }

        private void UpdateExtCtrl()
        {
        }

        // Added 06/24/05 BT for CAT commands
        public void CATMemoryQS()
        {
            btnMemoryQuickSave_Click(this.btnMemoryQuickSave, EventArgs.Empty);
        }

        // Added 06/25/05 BT for CAT commands
        public void CATMemoryQR()
        {
            btnMemoryQuickRestore_Click(this.btnMemoryQuickRecall, EventArgs.Empty);
        }

        // BT 06/30/05 Added for CAT commands
        public int CATBandGroup
        {
            get
            {
                if (grpBandHF.Visible)
                    return 0;
                else
                    return 1;
            }
            set
            {
            }
        }

        //BT 06/17/05 added for CAT commands
        public void SetCATBand(Band pBand)
        {
            Band b = pBand;
            switch (b)
            {
                case Band.B160M:
                    btnBand160_Click(this.btnBand160, EventArgs.Empty);
                    break;
                case Band.B80M:
                    btnBand80_Click(this.btnBand80, EventArgs.Empty);
                    break;
                case Band.B60M:
                    btnBand60_Click(this.btnBand60, EventArgs.Empty);
                    break;
                case Band.B40M:
                    btnBand40_Click(this.btnBand40, EventArgs.Empty);
                    break;
                case Band.B30M:
                    btnBand30_Click(this.btnBand30, EventArgs.Empty);
                    break;
                case Band.B20M:
                    btnBand20_Click(this.btnBand20, EventArgs.Empty);
                    break;
                case Band.B17M:
                    btnBand17_Click(this.btnBand17, EventArgs.Empty);
                    break;
                case Band.B15M:
                    btnBand15_Click(this.btnBand15, EventArgs.Empty);
                    break;
                case Band.B12M:
                    btnBand12_Click(this.btnBand12, EventArgs.Empty);
                    break;
                case Band.B10M:
                    btnBand10_Click(this.btnBand10, EventArgs.Empty);
                    break;
                case Band.B6M:
                    btnBand6_Click(this.btnBand6, EventArgs.Empty);
                    break;
                case Band.B2M:
                    btnBand2_Click(this.btnBand2, EventArgs.Empty);
                    break;
                case Band.GEN:
                    btnBandGEN_Click(this.btnBandGEN, EventArgs.Empty);
                    break;
                case Band.WWV:
                    btnBandWWV_Click(this.btnBandWWV, EventArgs.Empty);
                    break;
                default:
                    btnBandGEN_Click(this.btnBandGEN, EventArgs.Empty);
                    break;
            }
        }

        public void SetVHFText(int index, string text)
        {
            vhf_text[index].Text = text;
        }

        public void SetVHFEnabled(int index, bool b)
        {
            vhf_text[index].Enabled = b;
        }

        private void UpdateBandStackRegisters()
        {
            int[] band_stacks = DB.GetBandStackNum();
            band_160m_register = band_stacks[0];
            band_80m_register = band_stacks[1];
            band_60m_register = band_stacks[2];
            band_40m_register = band_stacks[3];
            band_30m_register = band_stacks[4];
            band_20m_register = band_stacks[5];
            band_17m_register = band_stacks[6];
            band_15m_register = band_stacks[7];
            band_12m_register = band_stacks[8];
            band_10m_register = band_stacks[9];
            band_6m_register = band_stacks[10];
            band_2m_register = band_stacks[11];
            band_wwv_register = band_stacks[12];
            band_gen_register = band_stacks[13];
        }

        public void UpdateFilters(int low, int high)
        {
            // System.Console.WriteLine("updf lo: " + low + " hi: " + high); 
            // qualify settings
            //if(low > high) return;
            switch (current_dsp_mode)
            {
                case DSPMode.LSB:
                case DSPMode.DIGL:
                case DSPMode.CWL:
                    if (low > high - 10) low = high - 10;
                    break;
                case DSPMode.USB:
                case DSPMode.DIGU:
                case DSPMode.CWU:
                    if (high < low + 10) high = low + 10;
                    break;
                case DSPMode.AM:
                case DSPMode.SAM:
                case DSPMode.FMN:
                case DSPMode.DSB:
                    if (high < low + 20)
                    {
                        if (Math.Abs(high) < Math.Abs(low))
                            high = low + 20;
                        else
                            low = high - 20;
                    }
                    break;
            }

            if (low < -9999)
                low = -9999;
            if (high > 9999)
                high = 9999;

            // send the settings to the DSP
            DttSP.SetRXFilters(low, high);

            // update var filter controls
            udFilterLow.Value = low;
            udFilterHigh.Value = high;

            // update Filter Shift
            tbFilterShift_Update(low, high);

            // update Filter Width
            tbFilterWidth_Update(low, high);

            // update display
//            Display.DrawBackground();

            // set XIT step rate
            if ((high - low) > 250)
            {
                udXIT.Increment = 10;
                udRIT.Increment = 10;
            }
            else
            {
                udXIT.Increment = 5;
                udRIT.Increment = 5;
            }

            if (filterForm != null && !filterForm.IsDisposed)
            {
                if (filterForm.CurrentDSPMode == current_dsp_mode)
                    filterForm.CurrentFilter = current_filter;
            }
        }

        public void UpdateFilterPresetNames(Filter f)
        {
            switch (f)
            {
                case Filter.F1:
                    radFilter1.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F1);
                    break;
                case Filter.F2:
                    radFilter2.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F2);
                    break;
                case Filter.F3:
                    radFilter3.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F3);
                    break;
                case Filter.F4:
                    radFilter4.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F4);
                    break;
                case Filter.F5:
                    radFilter5.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F5);
                    break;
                case Filter.F6:
                    radFilter6.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F6);
                    break;
                case Filter.F7:
                    radFilter7.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F7);
                    break;
                case Filter.F8:
                    radFilter8.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F8);
                    break;
                case Filter.F9:
                    radFilter9.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F9);
                    break;
                case Filter.F10:
                    radFilter10.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F10);
                    break;
                case Filter.VAR1:
                    radFilterVar1.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.VAR1);
                    break;
                case Filter.VAR2:
                    radFilterVar2.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.VAR2);
                    break;
            }

            if (f == current_filter)
                grpFilter.Text = "Filter - " + filter_presets[(int)current_dsp_mode].GetName(f);
        }

        public void UpdateFilterPresetLow(int val)
        {
            UpdateFilters(val, (int)udFilterHigh.Value);
        }

        public void UpdateFilterPresetHigh(int val)
        {
            UpdateFilters((int)udFilterLow.Value, val);
        }

        public void UpdateLOSCFreq(string freq)  // yt7pwr
        {	// only do this routine if there are six digits after the decimal point.
            txtLOSCFreq.Text = freq;
            txtLOSCMSD.Text = freq;

            string temp = freq;
            int index = temp.IndexOf(separator) + 4;
            txtLOSCLSD.Text = temp.Remove(0, index);
        }

        public void UpdateVFOAFreq(string freq)
        {	// only do this routine if there are six digits after the decimal point.
            txtVFOAFreq.Text = freq;
            txtVFOAMSD.Text = freq;

            string temp = freq;
            int index = temp.IndexOf(separator) + 4;
            txtVFOALSD.Text = temp.Remove(0, index);
        }

        public void UpdateVFOBFreq(string freq)
        {	// only do this routine if there are six digits after the decimal point.
            txtVFOBFreq.Text = freq;
            txtVFOBMSD.Text = freq;

            string temp = freq;
            int index = temp.IndexOf(separator) + 4;
            txtVFOBLSD.Text = temp.Remove(0, index);
        }

        public void CalcDisplayFreq() // changes yt7pwr
        {
            int low, high, low_tmp, high_tmp;

            double zoom_factor = tbDisplayZoom.Value / 4;
            double pan_factor = tbDisplayPan.Value;
            double vfo = -((LOSCFreq - VFOAFreq) * 1e6);

            int abs_low = (int)(-sample_rate1 / 2);
            int abs_high = -abs_low;

            double correction = 2 * (pan_factor * abs_high) / tbDisplayPan.Maximum;

            low = (int)(-sample_rate1 / (zoom_factor * 2));
            high = (int)(sample_rate1 / (zoom_factor * 2));

            if (tbDisplayZoom.Value == 4)
            {
                tbDisplayPan.Value = 0;
                low = (int)(-sample_rate1 / 2 );
                high = -low;
            }
            else
            {
                if (LOSCFreq < VFOAFreq)
                {
                    switch (CurrentDSPMode)
                    {
                        case (DSPMode.LSB):
                        case (DSPMode.DIGL):
                            vfo += DttSP.RXFilterLowCut / 2;
                            break;
                        case (DSPMode.USB):
                        case (DSPMode.DIGU):
                            vfo += DttSP.RXFilterHighCut / 2;
                            break;
                    }

                    high_tmp = ((int)vfo + high + (int)correction);
                    if (high_tmp > abs_high)
                    {
                        high = abs_high;
                        low = (abs_high + low * 2);
                    }
                    else
                    {
                        low = (high_tmp + low * 2);
                        if (low < abs_low)
                        {
                            low = abs_low;
                            high = low + high * 2;
                        }
                        else
                            high = high_tmp;
                    }
                }
                else
                {
                    if (LOSCFreq > VFOAFreq)
                    {
                        switch (CurrentDSPMode)
                        {
                            case (DSPMode.LSB):
                            case (DSPMode.DIGL):
                                vfo += DttSP.RXFilterLowCut / 2;
                                break;
                            case (DSPMode.USB):
                            case (DSPMode.DIGU):
                                vfo += DttSP.RXFilterHighCut / 2;
                                break;
                        }

                        low_tmp = ((int)vfo + low + (int)correction);
                        if (low_tmp < abs_low)
                        {
                            low = abs_low;
                            high = abs_low + high * 2;
                        }
                        else
                        {
                            high = (low_tmp + high * 2);
                            if (high > abs_high)
                            {
                                high = abs_high;
                                low = abs_high + low * 2;
                            }
                            else
                                low = low_tmp;
                        }
                    }
                }
            }

            DttSP.RXDisplayLow = DttSP.TXDisplayLow = low;
            DttSP.RXDisplayHigh = DttSP.TXDisplayHigh = high;
        }

        public void UpdateTXProfile()
        {
            if (SetupForm == null) return;

            string old = comboTXProfile.Text;
            comboTXProfile.Items.Clear();
            string[] s = SetupForm.GetTXProfileStrings();
            for (int i = 0; i < s.Length; i++)
                comboTXProfile.Items.Add(s[i]);
            if (old != "") comboTXProfile.Text = old;
        }

        #endregion

        #region Test and Calibration Routines

        public bool CalibrateFreq(float freq) // changes yt7pwr
        {
            bool retval = false;
            double losc_freq;
            bool rx_only = SetupForm.RXOnly;
            SetupForm.RXOnly = true;

            if (!chkPower.Checked)
            {
                MessageBox.Show("Power must be on in order to calibrate.", "Power Is Off",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (chkEnableSubRX.Checked)
                chkEnableSubRX.Checked = false;

            string vfo_freq_text = txtVFOAFreq.Text;		// save current frequency
            string losc_freq_text = txtLOSCFreq.Text;       // save LOSC frequency

            bool polyphase = SetupForm.Polyphase;			// save current polyphase setting
            SetupForm.Polyphase = false;					// disable polyphase

            int dsp_buf_size = SetupForm.DSPBufferSize;		// save current DSP buffer size
            SetupForm.DSPBufferSize = 2048;					// set DSP Buffer Size to 2048

            Filter filter = CurrentFilter;					// save current filter

            DSPMode dsp_mode = current_dsp_mode;			// save current demod mode
            CurrentDSPMode = DSPMode.AM;					// set DSP to AM

            bool rit_on = chkRIT.Checked;					// save current RIT state
            chkRIT.Checked = false;							// set RIT to Off

            int rit_value = (int)udRIT.Value;				// save current RIT value
            udRIT.Value = 0;								// set RIT Value to 0

            Filter am_filter = CurrentFilter;				// save am filter
            CurrentFilter = Filter.F5;						// set filter to 6600Hz

            LOSCFreq = freq + 0.004;
            losc_freq = double.Parse(txtLOSCFreq.Text);
            losc_freq *= 1000000;
            if (usb_si570_enable)
                SI570.Set_SI570_osc((long)losc_freq);
            else
                g59.Set_frequency((long)losc_freq);
            VFOAFreq = freq;               					// set frequency to passed value

            Thread.Sleep(200);
            //int ret = 0;

            fixed (float* ptr = &Display.new_display_data[0])
                DttSP.GetSpectrum(ptr);		// get the spectrum values

            float max = float.MinValue;
            //float avg = 0;

            int max_index = 0;
            int low = Display.BUFFER_SIZE >> 1;
            int high = low;
            low += (int)(((int)udFilterLow.Value * Display.BUFFER_SIZE) / DttSP.SampleRate);
            high += (int)(((int)udFilterHigh.Value * Display.BUFFER_SIZE) / DttSP.SampleRate);

            for (int i = low; i < high; i++)						// find the maximum signal
            {
                //avg += Display.new_display_data[i];
                if (Display.new_display_data[i] > max)
                {
                    max = Display.new_display_data[i];
                    max_index = i;
                }
            }

            // Calculate the difference between the known signal and the measured signal
            float diff = (float)((double)sample_rate1 / Display.BUFFER_SIZE * (Display.BUFFER_SIZE / 2 - max_index));

            // Calculate the DDS offset
            int offset = (int)(200.0 / freq * diff);

//            SetupForm.ClockOffset += offset;				// Offset the clock based on the difference
            retval = true;

        end:
            SetupForm.RXOnly = rx_only;						// restore RX Only setting
            CurrentFilter = am_filter;						// restore AM filter
            CurrentDSPMode = dsp_mode;						// restore DSP mode
            CurrentFilter = filter;							// restore filter
            chkRIT.Checked = rit_on;						// restore RIT state
            RITValue = rit_value;							// restore RIT value
            LOSCFreq = float.Parse(losc_freq_text);         // restore LOSC
            losc_freq = double.Parse(txtLOSCFreq.Text);
            losc_freq *= 1000000;
            if (usb_si570_enable)
                SI570.Set_SI570_osc((long)losc_freq);
            else
                g59.Set_frequency((long)losc_freq);
            VFOAFreq = float.Parse(vfo_freq_text);			// restore frequency VFOA
            SetupForm.DSPBufferSize = dsp_buf_size;			// restore DSP buffer size
            SetupForm.Polyphase = polyphase;				// restore polyphase

            return retval;
        }

        public bool CalibrateLevel(float level, float freq, Progress progress) // changes yt7pwr
        {
            // Calibration routine called by Setup Form.
            bool ret_val = false;
            if (!chkPower.Checked)
            {
                MessageBox.Show("Power must be on in order to calibrate.", "Power Is Off",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (chkEnableSubRX.Checked)
                chkEnableSubRX.Checked = false;

            bool rx_only = SetupForm.RXOnly;					// Save RX Only Setting
            SetupForm.RXOnly = true;

            double vfoa = VFOAFreq;								// save current VFOA
            double losc_freq = LOSCFreq;
            string losc_freq_text = txtLOSCFreq.Text;

            string display_mode = comboDisplayMode.Text;		// save current display mode
            comboDisplayMode.Text = "Spectrum";					// set display mode to spectrum

            bool polyphase = SetupForm.Polyphase;				// save current polyphase setting
            SetupForm.Polyphase = false;						// disable polyphase

            int dsp_buf_size = SetupForm.DSPBufferSize;			// save current DSP buffer size
            SetupForm.DSPBufferSize = 2048;						// set DSP Buffer Size to 2048

            Filter filter = CurrentFilter;						// save current filter

            DSPMode dsp_mode = current_dsp_mode;				// save current DSP demod mode
            CurrentDSPMode = DSPMode.AM;						// set mode to CWU

            LOSCFreq = freq + 0.004;
            losc_freq = double.Parse(txtLOSCFreq.Text);
            losc_freq *= 1000000;
            if (usb_si570_enable)
                SI570.Set_SI570_osc((long)losc_freq);
            else
                g59.Set_frequency((long)losc_freq);
            VFOAFreq = freq;									// set VFOA frequency

            Filter am_filter = CurrentFilter;					// save current AM filter
            CurrentFilter = Filter.F1;							// set filter to 500Hz

            PreampMode preamp = CurrentPreampMode;				// save current preamp mode
            CurrentPreampMode = PreampMode.HIGH;				// set to medium

            MeterRXMode rx_meter = CurrentMeterRXMode;			// save current RX Meter mode
            CurrentMeterRXMode = MeterRXMode.OFF;				// turn RX Meter off

            bool display_avg = chkDisplayAVG.Checked;			// save current average state
            chkDisplayAVG.Checked = false;
            chkDisplayAVG.Checked = true;						// set average state to off

            float old_multimeter_cal = multimeter_cal_offset;
            float old_display_cal = display_cal_offset;

            comboPreamp.Enabled = false;
            comboDisplayMode.Enabled = false;
            comboMeterRXMode.Enabled = false;

            progress.SetPercent(0.0f);
            int counter = 0;

            Thread.Sleep(2000);
            btnZeroBeat_Click(this, EventArgs.Empty);
            UpdateFilters(-250, 250);
            chkDisplayAVG.Checked = false;
            Thread.Sleep(200);

            DisableAllFilters();
            DisableAllModes();
            VFOLock = true;

            fixed (float* ptr = &Display.new_display_data[0])
                DttSP.GetSpectrum(ptr);		// get the spectrum values

            float max = float.MinValue;
            float avg = 0;

            int max_index = 0;
            int low = Display.BUFFER_SIZE >> 1;
            int high = low;
            low += (int)((DttSP.RXDisplayLow * Display.BUFFER_SIZE) / DttSP.SampleRate);
            high += (int)((DttSP.RXDisplayHigh * Display.BUFFER_SIZE) / DttSP.SampleRate);

            for (int i = low; i < high; i++)						// find the maximum signal
            {
                avg += Display.new_display_data[i];
                if (Display.new_display_data[i] > max)
                {
                    max = Display.new_display_data[i];
                    max_index = i;
                }
            }
            avg -= max;
            avg /= (high - low - 1);

            if (max < (avg + 30))
            {
                MessageBox.Show("Peak is less than 30dB from the noise floor.  " +
                    "Please use a larger signal for frequency calibration.",
                    "Calibration Error - Weak Signal",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                ret_val = false;
                goto end;
            }

            multimeter_cal_offset = 0.0f;
            display_cal_offset = 0.0f;
            float num = 0.0f, num2 = 0.0f, avg2 = 0.0f;
            avg = 0.0f;
            // get the value of the signal strength meter
            for (int i = 0; i < 50; i++)
            {
                num += DttSP.CalculateMeter(DttSP.MeterType.SIGNAL_STRENGTH);
                Thread.Sleep(50);
                if (!progress.Visible)
                    goto end;
                else progress.SetPercent((float)((float)++counter / 170));
            }
            avg = num / 50.0f;

                CurrentPreampMode = PreampMode.MED;
                Thread.Sleep(100);

                // get the value of the signal strength meter
                num2 = 0.0f;
                for (int i = 0; i < 50; i++)
                {
                    num2 += DttSP.CalculateMeter(DttSP.MeterType.SIGNAL_STRENGTH);
                    Thread.Sleep(50);
                    if (!progress.Visible)
                        goto end;
                    else progress.SetPercent((float)((float)++counter / 170));
                    counter++;
                }
                avg2 = num2 / 50.0f;

                float gain_offset = avg2 - avg;

                preamp_offset[(int)PreampMode.MED] = -gain_offset;
                preamp_offset[(int)PreampMode.HIGH] = 0.0f;

            CurrentPreampMode = PreampMode.HIGH;
            Thread.Sleep(100);

            num2 = 0.0f;
            for (int i = 0; i < 20; i++)
            {
                fixed (float* ptr = &Display.new_display_data[0])
                    DttSP.GetSpectrum(ptr);		// read again to clear out changed DSP

                max = float.MinValue;						// find the max spectrum value
                for (int j = 0; j < Display.BUFFER_SIZE; j++)
                    if (Display.new_display_data[j] > max) max = Display.new_display_data[j];

                num2 += max;

                Thread.Sleep(100);

                if (!progress.Visible)
                    goto end;
                else progress.SetPercent((float)((float)++counter / 170));
            }
            avg2 = num2 / 20.0f;

            // calculate the difference between the current value and the correct multimeter value
            float diff = level - (avg + multimeter_cal_offset + preamp_offset[(int)current_preamp_mode]);
            multimeter_cal_offset += diff;

            // calculate the difference between the current value and the correct spectrum value
            diff = level - (avg2 + display_cal_offset + preamp_offset[(int)current_preamp_mode]);
            DisplayCalOffset += diff;

            ret_val = true;

        end:
            progress.Hide();
            EnableAllFilters();
            EnableAllModes();
            VFOLock = false;
            comboPreamp.Enabled = true;
            comboDisplayMode.Enabled = true;
            comboMeterRXMode.Enabled = true;

            if (ret_val == false)
            {
                multimeter_cal_offset = old_multimeter_cal;
                display_cal_offset = old_display_cal;
            }

            SetupForm.RXOnly = rx_only;							// restore RX Only			
            DisplayAVG = display_avg;							// restore AVG value
            CurrentPreampMode = preamp;							// restore preamp value
            CurrentFilter = am_filter;							// restore AM filter
            CurrentDSPMode = dsp_mode;							// restore DSP mode
            CurrentFilter = filter;								// restore filter
            if (dsp_buf_size != 2048)
                chkPower.Checked = false;						// go to standby
            SetupForm.DSPBufferSize = dsp_buf_size;				// restore DSP Buffer Size
            LOSCFreq = float.Parse(losc_freq_text);             // restore LOSC
            losc_freq = double.Parse(txtLOSCFreq.Text);
            losc_freq *= 1000000;
            if (usb_si570_enable)
                SI570.Set_SI570_osc((long)losc_freq);
            else
                g59.Set_frequency((long)losc_freq);
            VFOAFreq = vfoa;									// restore vfo frequency
            if (dsp_buf_size != 2048)
            {
                Thread.Sleep(100);
                chkPower.Checked = true;
            }
            CurrentMeterRXMode = rx_meter;						// restore RX Meter mode
            SetupForm.Polyphase = polyphase;					// restore polyphase
            comboDisplayMode.Text = display_mode;				// restore display mode

            //			Debug.WriteLine("multimeter_cal_offset: "+multimeter_cal_offset);
            //			Debug.WriteLine("display_cal_offset: "+display_cal_offset);
            //			MessageBox.Show("multimeter_cal_offset: "+multimeter_cal_offset.ToString()+"\n"+
            //				"display_cal_offset: "+display_cal_offset.ToString());
            return ret_val;
        }

        public bool CalibrateImage(float freq, Progress progress) // changes yt7pwr
        {
            //			HiPerfTimer t1 = new HiPerfTimer();
            //			t1.Start();

            // Setup Rig for Image Null Cal
            bool ret_val = false;
            string losc_freq_text = txtLOSCFreq.Text;

            if (!chkPower.Checked)
            {
                MessageBox.Show("Power must be on in order to calibrate.", "Power Is Off",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (chkEnableSubRX.Checked)
                chkEnableSubRX.Checked = false;

            bool rx_only = SetupForm.RXOnly;				// save RX Only Setting
            SetupForm.RXOnly = true;

            bool polyphase = SetupForm.Polyphase;			// save current polyphase setting
            SetupForm.Polyphase = false;					// disable polyphase

            int dsp_buf_size = SetupForm.DSPBufferSize;		// save current DSP buffer size
            SetupForm.DSPBufferSize = 2048;					// set DSP Buffer Size to 2048

            DSPMode dsp_mode = current_dsp_mode;			// save current dsp mode
            CurrentDSPMode = DSPMode.AM;					// set dsp mode to AM

            Filter filter = current_filter;					// save current filter
            CurrentFilter = Filter.F1;			    		// set filter to 6kHz

            double losc_freq;
            LOSCFreq = freq;
            losc_freq = double.Parse(txtLOSCFreq.Text);
            losc_freq *= 1000000;
            if (usb_si570_enable)
                SI570.Set_SI570_osc((long)losc_freq);
            else
                g59.Set_frequency((long)losc_freq);

            double vfo_freq = VFOAFreq;						// save current frequency
            VFOAFreq = freq + 0.022050f;	    			// set frequency to passed value + 22kHz

            bool avg = chkDisplayAVG.Checked;		// save current average state
            chkDisplayAVG.Checked = false;

            string display_mode = comboDisplayMode.Text;
            comboDisplayMode.Text = "Off";

            DisableAllFilters();
            DisableAllModes();
            VFOLock = true;
            comboPreamp.Enabled = false;
            comboDisplayMode.Enabled = false;

            SetupForm.ImageGainRX = -250.0;
            SetupForm.ImagePhaseRX = -200.0;

            float[] a = new float[Display.BUFFER_SIZE];
            float[] init_max = new float[4];

            //int retval = 0;
            progress.SetPercent(0.0f);
            int counter = 0;

            Thread.Sleep(200);

            fixed (float* ptr = &a[0])
                DttSP.GetSpectrum(ptr);// get the spectrum values

            float max_signal = float.MinValue;				// find the signal value
            int peak_bin = -1;

            // find peak bin
            for (int j = 0; j < Display.BUFFER_SIZE; j++)
            {
                if (a[j] > max_signal)
                {
                    peak_bin = j;
                    max_signal = a[j];
                }
            }

            SetupForm.ImageGainRX = 0.0;
            SetupForm.ImagePhaseRX = 0.0;

            bool progressing = true;
            double phase_step = 20;
            double gain_step = 20;
            double phase_range = 400;
            double gain_range = 500;
            double low_phase, high_phase, low_gain, high_gain;
            double phase_index = 0;
            double gain_index = 0;
            double global_min_phase = 0;
            double global_min_gain = 0;
            double global_min_value = float.MaxValue;

            while (progressing)
            {
                // find minimum of the peak signal over 
                // the range of Phase settings

                low_phase = global_min_phase - phase_range / 2;
                if (low_phase < -400.0) low_phase = -400.0;
                high_phase = global_min_phase + phase_range / 2;
                if (high_phase > 400.0) high_phase = 400.0;
                float min_signal = float.MaxValue;
                for (double i = (int)Math.Max(low_phase, -400.0); i < high_phase && i <= 400.0; i += phase_step)
                {
                    SetupForm.ImagePhaseRX = i;				// phase slider
                    Thread.Sleep(100);

                    fixed (float* ptr = &a[0])
                        DttSP.GetSpectrum(ptr);// get the spectrum values

                    if (a[peak_bin] < min_signal)			// if image is less than minimum
                    {
                        //Debug.WriteLine(i.ToString()+": "+a[peak_bin].ToString("f6"));
                        min_signal = a[peak_bin];			// save new minimum
                        phase_index = i;					// save phase index
                    }

                    if (!progress.Visible)
                        goto end;
                    else progress.SetPercent((float)((float)counter++ / 1500));
                }

                //if(min_signal < global_min_value)
                {
                    global_min_value = min_signal;
                    global_min_phase = phase_index;
                    global_min_gain = gain_index;
                }

                SetupForm.ImagePhaseRX = global_min_phase;			//set phase slider to min found

                // find minimum of the peak signal over 
                // the range of Gain settings

                low_gain = global_min_gain - gain_range / 2;
                if (low_gain < -250.0) low_gain = -250.0;
                high_gain = global_min_gain + gain_range / 2;
                if (high_gain > 250.0) high_gain = 250.0;

                min_signal = float.MaxValue;
                for (double i = (int)Math.Max(low_gain, -250.0); i < high_gain && i <= 250.0; i += gain_step)
                {
                    SetupForm.ImageGainRX = i;				//set gain slider
                    Thread.Sleep(100);

                    fixed (float* ptr = &a[0])
                        DttSP.GetSpectrum(ptr);// get the spectrum values

                    if (a[peak_bin] < min_signal)			// if image is less than minimum
                    {
                        min_signal = a[peak_bin];			// save new minimum
                        gain_index = i;						// save phase index
                    }

                    if (!progress.Visible)
                        goto end;
                    else progress.SetPercent((float)((float)counter++ / 1500));
                }

                //if(min_signal < global_min_value)
                {
                    global_min_value = min_signal;
                    global_min_phase = phase_index;
                    global_min_gain = gain_index;
                }

                SetupForm.ImageGainRX = global_min_gain;			//set gain slider to min found

                // narrow search range and use more steps
                phase_step /= 2.0; if (phase_step < 0.01) phase_step = 0.01;
                phase_range /= 2.0; if (phase_range < phase_step * 10.0) phase_range = phase_step * 10.0;
                gain_step /= 2.0; if (gain_step < 0.01) gain_step = 0.01;
                gain_range /= 2.0; if (gain_range < gain_step * 10.0) gain_range = gain_step * 10.0;

                // stop when range and step are 1 for gain and phase
                if (phase_range <= 0.10 && phase_step <= 0.01 &&
                    gain_step <= 0.01 && gain_range == 0.10)
                    progressing = false;
            }

            // Finish the algorithm and reset the values
            ret_val = true;

        end:
            progress.Hide();

            EnableAllFilters();
            EnableAllModes();
            VFOLock = false;
            comboPreamp.Enabled = true;
            comboDisplayMode.Enabled = true;

            comboDisplayMode.Text = display_mode;				// restore display mode
            SetupForm.RXOnly = rx_only;							// restore RX Only setting
            CurrentDSPMode = dsp_mode;							// restore dsp mode
            CurrentFilter = filter;								// restore filter
            LOSCFreq = float.Parse(losc_freq_text);         // restore LOSC
            losc_freq = double.Parse(txtLOSCFreq.Text);
            losc_freq *= 1000000;
            if (usb_si570_enable)
                SI570.Set_SI570_osc((long)losc_freq);
            else
                g59.Set_frequency((long)freq);
            VFOAFreq = vfo_freq;								// restore frequency
            txtVFOAFreq_LostFocus(this, EventArgs.Empty);
            chkDisplayAVG.Checked = avg;						// restore average state
            SetupForm.DSPBufferSize = dsp_buf_size;				// restore DSP Buffer Size
            SetupForm.Polyphase = polyphase;					// restore polyphase

            //			t1.Stop();
            //			MessageBox.Show(t1.Duration.ToString());
            return ret_val;
        }

        // changes yt7pwr
        public bool CalibratePAGain(Progress progress, bool[] run, int target_watts) // calibrate PA Gain values
        {
            //			HiPerfTimer t1 = new HiPerfTimer();
            //			t1.Start();

            bool ret_val = false;

            if (!chkPower.Checked)
            {
                MessageBox.Show("Power must be on in order to calibrate.", "Power Is Off",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (chkEnableSubRX.Checked)
                chkEnableSubRX.Checked = false;

            calibrating = true;

            DSPMode dsp_mode = current_dsp_mode;			// save current dsp mode
            CurrentDSPMode = DSPMode.USB;					// set dsp mode to CWL

            string losc_freq_text = txtLOSCFreq.Text;
            double losc_freq = LOSCFreq;
            double vfo_freq = VFOAFreq;						// save current frequency

            int pwr = (int)udPWR.Value;						// save current pwr level

            DisableAllFilters();
            DisableAllModes();
            VFOLock = true;
            comboPreamp.Enabled = false;
            comboDisplayMode.Enabled = false;

            int on_time = 2500;
            int off_time = 2500;

            switch (current_soundcard)
            {
                case SoundCard.AUDIGY_2_ZS:
                    on_time = 3000;
                    off_time = 4000;
                    break;
                case SoundCard.DELTA_44:
                    on_time = 2000;
                    off_time = 2000;
                    break;
            }

            progress.SetPercent(0.0f);

            float[] band_freqs = { 1.9f, 3.75f, 5.3665f, 7.15f, 10.125f, 14.175f, 18.1f, 21.225f, 24.9f, 28.85f };
            int[] max_pwr = { 100, 100, 100, 100, 100, 100, 100, 100, 75, 75 };

            if (run[0]) SetupForm.PAGain160 = 49.0f;
            if (run[1]) SetupForm.PAGain80 = 49.0f;
            if (run[2]) SetupForm.PAGain60 = 49.0f;
            if (run[3]) SetupForm.PAGain40 = 49.0f;
            if (run[4]) SetupForm.PAGain30 = 49.0f;
            if (run[5]) SetupForm.PAGain20 = 49.0f;
            if (run[6]) SetupForm.PAGain17 = 49.0f;
            if (run[7]) SetupForm.PAGain15 = 49.0f;
            if (run[8]) SetupForm.PAGain12 = 49.0f;
            if (run[9]) SetupForm.PAGain10 = 49.0f;

            for (int i = 0; i < band_freqs.Length; i++)
            {
                if (run[i])
                {
                    int error_count = 0;
                    VFOLock = false;
                    LOSCFreq = band_freqs[i];
                    losc_freq = double.Parse(txtLOSCFreq.Text);
                    losc_freq *= 1000000;
                    if (usb_si570_enable)
                        SI570.Set_SI570_osc((long)losc_freq);
                    else
                        g59.Set_frequency((long)losc_freq);
                    VFOAFreq = band_freqs[i];				// set frequency
                    VFOLock = true;
                    udPWR.Value = Math.Min(target_watts, max_pwr[i]);
                    int target = (int)udPWR.Value;

                    bool good_result = false;
                    while (good_result == false)
                    {
                        Audio.SwitchCount = 4;
                        Audio.RampDown = true;
                        Audio.NextMox = true;
                        Audio.NextAudioState1 = Audio.AudioState.SINL_COSR;
                        Audio.CurrentAudioState1 = Audio.AudioState.SWITCH;
                        tuning = true;
                        chkMOX.Checked = true;

                        for (int j = 0; j < on_time / 100; j++)
                        {
                            Thread.Sleep(100);
                            if (!progress.Visible)
                                goto end;
                        }

                        double watts = 0;
                        //pa_power_mutex.WaitOne();
                        watts = PAPower(pa_fwd_power);
                        //pa_power_mutex.ReleaseMutex();

                        chkMOX.Checked = false;
                        tuning = false;
                        Audio.RampDown = true;
                        Audio.NextMox = false;
                        Audio.SwitchCount = 4;
                        Audio.NextAudioState1 = Audio.AudioState.DTTSP;
                        Audio.CurrentAudioState1 = Audio.AudioState.SWITCH;

                        //Debug.WriteLine("watts: "+watts.ToString());

                        if (!progress.Visible)
                            goto end;

                        if (Math.Abs(watts - target) > 4)
                        {
                            // convert to dBm
                            float diff_dBm = (float)Math.Round((WattsTodBm(watts) - WattsTodBm((double)target)), 1);

                            switch (i)										// fix gain value
                            {
                                case 0:
                                    if (SetupForm.PAGain160 + diff_dBm < 38.0)
                                    {
                                        if (++error_count > 6)
                                            goto error;

                                        SetupForm.PAGain160 = (float)Math.Max(38.0, SetupForm.PAGain160 - 2.0);
                                    }
                                    else SetupForm.PAGain160 += diff_dBm;
                                    break;
                                case 1:
                                    if (SetupForm.PAGain80 + diff_dBm < 38.0)
                                    {
                                        if (++error_count > 6)
                                            goto error;

                                        SetupForm.PAGain80 = (float)Math.Max(38.0, SetupForm.PAGain80 - 2.0);
                                    }
                                    else SetupForm.PAGain80 += diff_dBm;
                                    break;
                                case 2:
                                    if (SetupForm.PAGain60 + diff_dBm < 38.0)
                                    {
                                        if (++error_count > 6)
                                            goto error;

                                        SetupForm.PAGain60 = (float)Math.Max(38.0, SetupForm.PAGain60 - 2.0);
                                    }
                                    else SetupForm.PAGain60 += diff_dBm;
                                    break;
                                case 3:
                                    if (SetupForm.PAGain40 + diff_dBm < 38.0)
                                    {
                                        if (++error_count > 6)
                                            goto error;

                                        SetupForm.PAGain40 = (float)Math.Max(38.0, SetupForm.PAGain40 - 2.0);
                                    }
                                    else SetupForm.PAGain40 += diff_dBm;
                                    break;
                                case 4:
                                    if (SetupForm.PAGain30 + diff_dBm < 38.0)
                                    {
                                        if (++error_count > 6)
                                            goto error;

                                        SetupForm.PAGain30 = (float)Math.Max(38.0, SetupForm.PAGain30 - 2.0);
                                    }
                                    else SetupForm.PAGain30 += diff_dBm;
                                    break;
                                case 5:
                                    if (SetupForm.PAGain20 + diff_dBm < 38.0)
                                    {
                                        if (++error_count > 6)
                                            goto error;

                                        SetupForm.PAGain20 = (float)Math.Max(38.0, SetupForm.PAGain20 - 2.0);
                                    }
                                    else SetupForm.PAGain20 += diff_dBm;
                                    break;
                                case 6:
                                    if (SetupForm.PAGain17 + diff_dBm < 38.0)
                                    {
                                        if (++error_count > 6)
                                            goto error;

                                        SetupForm.PAGain17 = (float)Math.Max(38.0, SetupForm.PAGain17 - 2.0);
                                    }
                                    else SetupForm.PAGain17 += diff_dBm;
                                    break;
                                case 7:
                                    if (SetupForm.PAGain15 + diff_dBm < 38.0)
                                    {
                                        if (++error_count > 6)
                                            goto error;

                                        SetupForm.PAGain15 = (float)Math.Max(38.0, SetupForm.PAGain15 - 2.0);
                                    }
                                    else SetupForm.PAGain15 += diff_dBm;
                                    break;
                                case 8:
                                    if (SetupForm.PAGain12 + diff_dBm < 38.0)
                                    {
                                        if (++error_count > 6)
                                            goto error;

                                        SetupForm.PAGain12 = (float)Math.Max(38.0, SetupForm.PAGain12 - 2.0);
                                    }
                                    else SetupForm.PAGain12 += diff_dBm;
                                    break;
                                case 9:
                                    if (SetupForm.PAGain10 + diff_dBm < 38.0)
                                    {
                                        if (++error_count > 6)
                                            goto error;

                                        SetupForm.PAGain10 = (float)Math.Max(38.0, SetupForm.PAGain10 - 2.0);
                                    }
                                    else SetupForm.PAGain10 += diff_dBm;
                                    break;
                            }
                        }
                        else good_result = true;
                        for (int j = 0; j < off_time / 100; j++)
                        {
                            Thread.Sleep(100);
                            if (!progress.Visible)
                                goto end;
                        }
                    }
                }
                progress.SetPercent((float)((float)(i + 1) / 10));
            }

            ret_val = true;

        end:
            progress.Hide();

            EnableAllFilters();
            EnableAllModes();
            VFOLock = false;
            comboPreamp.Enabled = true;
            comboDisplayMode.Enabled = true;

            chkMOX.Checked = false;
            tuning = false;
            Audio.SwitchCount = 4;
            Audio.NextAudioState1 = Audio.AudioState.DTTSP;
            Audio.CurrentAudioState1 = Audio.AudioState.SWITCH;
            CurrentDSPMode = dsp_mode;							// restore dsp mode
            LOSCFreq = float.Parse(losc_freq_text);         // restore LOSC
            losc_freq = double.Parse(txtLOSCFreq.Text);
            losc_freq *= 1000000;
            if (usb_si570_enable)
                SI570.Set_SI570_osc((long)losc_freq);
            else
                g59.Set_frequency((long)losc_freq);
            VFOAFreq = vfo_freq;								// restore frequency
            txtVFOAFreq_LostFocus(this, EventArgs.Empty);
            udPWR.Value = pwr;									// restore pwr level

            calibrating = false;

            //t1.Stop();
            //MessageBox.Show(t1.Duration.ToString());
            return ret_val;

        error:
            MessageBox.Show("Calculated gain is invalid.  Please double check connections and try again.\n" +
                "If this problem persists, contact support@flex-radio.com for support.",
                "Invalid Gain Found",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            goto end;
        }

        // changes yt7pwr
        public bool LowPowerPASweep(Progress progress, int power) // calibrate PA Gain values
        {
            //			HiPerfTimer t1 = new HiPerfTimer();
            //			t1.Start();

            bool ret_val = false;

            if (!chkPower.Checked)
            {
                MessageBox.Show("Power must be on in order to calibrate.", "Power Is Off",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (chkEnableSubRX.Checked)
                chkEnableSubRX.Checked = false;

            string losc_freq_text = txtLOSCFreq.Text;
            double losc_freq = 0.0;

            double vfo_freq = VFOAFreq;						// save current frequency

            calibrating = true;

            DSPMode dsp_mode = CurrentDSPMode;				// save current DSP Mode
            CurrentDSPMode = DSPMode.USB;					// set DSP Mode to USB

            int pwr = (int)udPWR.Value;						// save current pwr level
            udPWR.Value = power;							// set pwr level to 100W

            progress.SetPercent(0.0f);

            float[] band_freqs = { 1.9f, 3.75f, 5.3665f, 7.15f, 10.125f, 14.175f, 18.1f, 21.225f, 24.9f, 28.85f };

            for (int i = 0; i < band_freqs.Length; i++)
            {
                LOSCFreq = band_freqs[i];
                losc_freq = double.Parse(txtLOSCFreq.Text);
                losc_freq *= 1000000;
                if (usb_si570_enable)
                    SI570.Set_SI570_osc((long)losc_freq);
                else
                    g59.Set_frequency((long)losc_freq);
                VFOAFreq = band_freqs[i];				// set frequency
                Audio.CurrentAudioState1 = Audio.AudioState.SINL_COSR;
                chkMOX.Checked = true;
                for (int j = 0; j < 30; j++)
                {
                    Thread.Sleep(100);
                    if (!progress.Visible)
                        goto end;
                }
                chkMOX.Checked = false;
                Audio.CurrentAudioState1 = Audio.AudioState.DTTSP;

                if (!progress.Visible)
                    goto end;

                for (int j = 0; j < 40; j++)
                {
                    Thread.Sleep(100);
                    if (!progress.Visible)
                        goto end;
                }
                if (!progress.Visible)
                    goto end;

                progress.SetPercent((float)((float)(i + 1) / 10));
            }

            ret_val = true;

        end:
            progress.Hide();
            chkMOX.Checked = false;
            Audio.CurrentAudioState1 = Audio.AudioState.DTTSP;
            CurrentDSPMode = dsp_mode;							// restore dsp mode
            LOSCFreq = float.Parse(losc_freq_text);             // restore LOSC
            losc_freq = double.Parse(txtLOSCFreq.Text);
            losc_freq *= 1000000;
            if (usb_si570_enable)
                SI570.Set_SI570_osc((long)losc_freq);
            else
                g59.Set_frequency((long)losc_freq);
            VFOAFreq = vfo_freq;								// restore frequency
            vfo_freq /= 1000000;
            txtVFOAFreq_LostFocus(this, EventArgs.Empty);
            udPWR.Value = pwr;									// restore pwr level

            calibrating = false;

            //t1.Stop();
            //MessageBox.Show(t1.Duration.ToString());
            return ret_val;
        }

        public bool CalibrateSoundCard(Progress progress, int card)
        {
            if (!chkPower.Checked)
            {
                MessageBox.Show("Power must be on in order to calibrate.", "Power Is Off",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            Audio.testing = true;
            progress.SetPercent(0.0f);

            double tx_volume = Audio.RadioVolume;	// save current TX volume
            double rx_volume = Audio.MonitorVolume;	// save current RX volume
            bool twotone = Audio.two_tone;			// save current two tone setting
            Audio.two_tone = false;

            if (num_channels == 4 || num_channels == 6)
            {
                chkMOX.Checked = true;
                Thread.Sleep(200);
                Audio.RadioVolume = 1.0;				// set volume to max
                Audio.MonitorVolume = 0.0;
            }
            else
            {
                Mixer.SetMainVolume(mixer_id1, 100);
                Mixer.SetWaveOutVolume(mixer_id1, 100);
                Audio.MonitorVolume = 1.0;
            }

            Audio.CurrentAudioState1 = Audio.AudioState.SINL_COSR;	// Start sending tone

            progress.Focus();

            while (progress.Visible == true)			// keep sending tone until abort is pressed
                Thread.Sleep(100);

            Audio.CurrentAudioState1 = Audio.AudioState.DTTSP;		// stop sending tone

            if (num_channels > 2)
            {
                Thread.Sleep(200);
                chkMOX.Checked = false;
            }

            Audio.RadioVolume = tx_volume;			// restore TX volume
            Audio.MonitorVolume = rx_volume;		// restore RX volume
            Audio.two_tone = twotone;				// restore two tone setting
            Audio.testing = false;

            return true;
        }

        #endregion

        #region Properties
        // ======================================================
        // Properties
        // ======================================================

        #region Genesis
        private bool TUNE = true;
        private bool tx_rx = false;
        public double G3020Xtal1 = 10.125;
        public double G3020Xtal2 = 14.045;
        public double G3020Xtal3 = 14.138;
        public double G3020Xtal4 = 14.232;
        public double G40Xtal1 = 7.047;
        public double G80Xtal1 = 3.545;
        public double G80Xtal2 = 3.638;
        public double G80Xtal3 = 3.732;
        public double G80Xtal4 = 3.835;
        public double G160Xtal1 = 1.838;
        public double G160Xtal2 = 1.845;

        private double vfoa_restore = 10.0;
        private double vfob_restore = 10.0;
        private double losc_restore = 10.0;
        private Filter filter_restore = Filter.F1;
        private int zoom_restore = 4;
        private int pan_restore = 0;
        private DSPMode mode_restore = DSPMode.USB;

        private bool vfo_sinc = false;
        private bool VFO_SINC
        {
            get { return vfo_sinc; }
            set { vfo_sinc = value; }
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

        private double si570_xtal = 114285000000.000;
        public double si570_fxtal
        {
            get { return si570_xtal; }

            set { si570_xtal = value; }
        }

        private int memory_number = 1;
        public int MemoryNumber
        {
            get { return memory_number; }

            set
            {
                memory_number = value;
                lblMemoryNumber.Text = memory_number.ToString();
            }
        }

        private bool usb_si570_enable = false;
        public bool usb_si570_dll
        {
            get { return usb_si570_enable; }

            set { usb_si570_enable = value; }
        }
        #endregion

        private ColorSheme color_palette = ColorSheme.original;
        public ColorSheme color_sheme
        {
            get { return color_palette; }

            set
            {
                color_palette = value;
            }
        }

        public SIOListenerII Siolisten
        {
            get { return siolisten; }
            set
            {
                siolisten = value;
                Keyer.Siolisten = value;
            }
        }

        public string TXProfile
        {
            get
            {
                if (comboTXProfile != null) return comboTXProfile.Text;
                else return "";
            }
            set { if (comboTXProfile != null) comboTXProfile.Text = value; }
        }



        public string VACSampleRate
        {
            get
            {
                if (comboVACSampleRate != null) return comboVACSampleRate.Text;
                else return "";
            }
            set
            {
                if (comboVACSampleRate != null) comboVACSampleRate.Text = value;
            }
        }

        public bool VACStereo
        {
            get
            {
                if (chkVACStereo != null) return chkVACStereo.Checked;
                else return false;
            }
            set
            {
                if (chkVACStereo != null) chkVACStereo.Checked = value;
            }
        }

        public bool CWIambic
        {
            get
            {
                if (chkCWIambic != null) return chkCWIambic.Checked;
                else return false;
            }

            set
            {
                if (chkCWIambic != null) chkCWIambic.Checked = value;
            }
        }

        private MultiMeterDisplayMode current_meter_display_mode = MultiMeterDisplayMode.Edge;
        public MultiMeterDisplayMode CurrentMeterDisplayMode
        {
            get { return current_meter_display_mode; }
            set
            {
                switch (current_meter_display_mode)
                {
                    case MultiMeterDisplayMode.Edge:
                        switch (value)
                        {
                            case MultiMeterDisplayMode.Edge:
                                break;
                            default:
                                picMultiMeterDigital.Height -= lblMultiSMeter.ClientSize.Height;
                                picMultiMeterDigital.BackColor = meter_background_color;
                                break;
                        }
                        break;
                    default:
                        switch (value)
                        {
                            case MultiMeterDisplayMode.Edge:
                                picMultiMeterDigital.Height += lblMultiSMeter.ClientSize.Height;
                                picMultiMeterDigital.BackColor = edge_meter_background_color;
                                break;
                        }
                        break;
                }
                current_meter_display_mode = value;
                picMultiMeterDigital.Invalidate();
            }
        }

        private Color vfo_background_color = Color.Black;
        public Color VFOBackgroundColor
        {
            get { return vfo_background_color; }
            set
            {
                vfo_background_color = value;
                txtVFOAFreq.BackColor = value;
                txtVFOAMSD.BackColor = value;
                txtVFOALSD.BackColor = value;
                txtVFOBFreq.BackColor = value;
                txtVFOBMSD.BackColor = value;
                txtVFOBLSD.BackColor = value;
                panelVFOAHover.BackColor = value;
                panelVFOBHover.BackColor = value;
            }
        }

        private Color meter_digital_text_color = Color.Yellow;
        public Color MeterDigitalTextColor
        {
            get { return meter_digital_text_color; }
            set
            {
                meter_digital_text_color = value;
                txtMultiText.ForeColor = value;
            }
        }

        private Color meter_digital_background_color = Color.Black;
        public Color MeterDigitalBackgroundColor
        {
            get { return meter_digital_background_color; }
            set
            {
                meter_digital_background_color = value;
                txtMultiText.BackColor = value;
            }
        }

        private Color band_background_color = Color.Black;
        public Color BandBackgroundColor
        {
            get { return band_background_color; }
            set
            {
                band_background_color = value;
                txtVFOABand.BackColor = value;
                txtVFOBBand.BackColor = value;
            }
        }

        private Color edge_meter_background_color = Color.Black;
        public Color EdgeMeterBackgroundColor
        {
            get { return edge_meter_background_color; }
            set
            {
                edge_meter_background_color = value;
                if (current_meter_display_mode == MultiMeterDisplayMode.Edge)
                {
                    picMultiMeterDigital.BackColor = value;
                    picMultiMeterDigital.Invalidate();
                }
            }
        }

        private Color edge_low_color = Color.White;
        public Color EdgeLowColor
        {
            get { return edge_low_color; }
            set
            {
                edge_low_color = value;
                if (current_meter_display_mode == MultiMeterDisplayMode.Edge)
                    picMultiMeterDigital.Invalidate();
            }
        }

        private Color edge_high_color = Color.Red;
        public Color EdgeHighColor
        {
            get { return edge_high_color; }
            set
            {
                edge_high_color = value;
                if (current_meter_display_mode == MultiMeterDisplayMode.Edge)
                    picMultiMeterDigital.Invalidate();
            }
        }

        private Color edge_avg_color = Color.Yellow;
        public Color EdgeAVGColor
        {
            get { return edge_avg_color; }
            set
            {
                edge_avg_color = value;
                if (current_meter_display_mode == MultiMeterDisplayMode.Edge)
                    picMultiMeterDigital.Invalidate();
            }
        }

        private Color meter_background_color = Color.Black;
        public Color MeterBackgroundColor
        {
            get { return meter_background_color; }
            set
            {
                meter_background_color = value;
                if (current_meter_display_mode == MultiMeterDisplayMode.Original)
                {
                    picMultiMeterDigital.BackColor = value;
                    picMultiMeterDigital.Invalidate();
                }
            }
        }

        private Color peak_background_color = Color.Black;
        public Color PeakBackgroundColor
        {
            get { return peak_background_color; }
            set
            {
                peak_background_color = value;
                txtDisplayCursorOffset.BackColor = value;
                txtDisplayCursorPower.BackColor = value;
                txtDisplayCursorFreq.BackColor = value;
                txtDisplayPeakOffset.BackColor = value;
                txtDisplayPeakPower.BackColor = value;
                txtDisplayPeakFreq.BackColor = value;
                textBox1.BackColor = value;
            }
        }

        private bool small_lsd = true;
        public bool SmallLSD
        {
            get { return small_lsd; }
            set
            {
                small_lsd = value;
                txtVFOALSD.Visible = value;
                txtVFOAMSD.Visible = value;
                txtVFOBLSD.Visible = value;
                txtVFOBMSD.Visible = value;
            }
        }

        private Color small_vfo_color = Color.OrangeRed;
        public Color SmallVFOColor
        {
            get { return small_vfo_color; }
            set
            {
                small_vfo_color = value;
                if (small_lsd && chkPower.Checked)
                {
                    txtVFOALSD.ForeColor = small_vfo_color;
                    if (chkVFOSplit.Checked)
                        txtVFOBLSD.ForeColor = small_vfo_color;
                }
            }
        }

        private int default_low_cut = 150;
        public int DefaultLowCut
        {
            get { return default_low_cut; }
            set
            {
                for (DSPMode m = DSPMode.FIRST + 1; m < DSPMode.LAST; m++)
                {
                    for (Filter f = Filter.FIRST + 1; f < Filter.LAST; f++)
                    {
                        int low = filter_presets[(int)m].GetLow(f);
                        int high = filter_presets[(int)m].GetHigh(f);

                        switch (m)
                        {
                            case DSPMode.USB:
                            case DSPMode.DIGU:
                                if (low == default_low_cut)
                                    filter_presets[(int)m].SetLow(f, value);
                                break;
                            case DSPMode.LSB:
                            case DSPMode.DIGL:
                                if (high == -default_low_cut)
                                    filter_presets[(int)m].SetHigh(f, -value);
                                break;
                        }
                    }
                }
                default_low_cut = value;
                CurrentFilter = current_filter;
            }
        }

        public int COMPVal
        {
            get
            {
                if (udCOMP != null) return (int)udCOMP.Value;
                else return -1;
            }
            set
            {
                if (udCOMP != null) udCOMP.Value = value;
            }
        }

        public int CPDRVal
        {
            get
            {
                if (udCPDR != null) return (int)udCPDR.Value;
                else return -1;
            }
            set
            {
                if (udCPDR != null) udCPDR.Value = value;
            }
        }

        public int NoiseGate
        {
            get
            {
                if (tbNoiseGate != null) return tbNoiseGate.Value;
                else return -1;
            }
            set
            {
                if (tbNoiseGate != null)
                {
                    if (value > tbNoiseGate.Maximum) value = tbNoiseGate.Maximum;
                    tbNoiseGate.Value = value;
                }
            }
        }

        public int VOXSens
        {
            get
            {
                if (tbVOX != null) return tbVOX.Value;
                else return -1;
            }
            set
            {
                if (tbVOX != null) tbVOX.Value = value;
            }
        }

        public bool NoiseGateEnabled
        {
            get
            {
                if (chkNoiseGate != null) return chkNoiseGate.Checked;
                else return false;
            }
            set
            {
                if (chkNoiseGate != null) chkNoiseGate.Checked = value;
            }
        }

        public int VACRXGain
        {
            get
            {
                if (udVACRXGain != null) return (int)udVACRXGain.Value;
                else return -99;
            }
            set
            {
                if (udVACRXGain != null) udVACRXGain.Value = value;
            }
        }

        public int VACTXGain
        {
            get
            {
                if (udVACTXGain != null) return (int)udVACTXGain.Value;
                else return -99;
            }
            set
            {
                if (udVACTXGain != null) udVACTXGain.Value = value;
            }
        }

        public bool BreakInEnabled
        {
            get
            {
                if (chkBreakIn != null) return chkBreakIn.Checked;
                else return false;
            }
            set
            {
                if (chkBreakIn != null) chkBreakIn.Checked = value;
            }
        }

        public bool VOXEnable
        {
            get
            {
                if (chkVOX != null) return chkVOX.Checked;
                else return false;
            }
            set
            {
                if (chkVOX != null) chkVOX.Checked = value;
            }
        }

        public int RF
        {
            get
            {
                if (udRF != null) return (int)udRF.Value;
                else return -1;
            }
            set
            {
                if (udRF != null) udRF.Value = value;
            }
        }

        private bool enable_kb_shortcuts = true;
        public bool EnableKBShortcuts
        {
            get { return enable_kb_shortcuts; }
            set { enable_kb_shortcuts = value; }
        }

        private bool save_filter_changes = false;
        public bool SaveFilterChanges
        {
            get { return save_filter_changes; }
            set { save_filter_changes = value; }
        }

        private int max_filter_shift = 9999;
        public int MaxFilterShift
        {
            get { return max_filter_shift; }
            set
            {
                max_filter_shift = value;
                UpdateFilters(DttSP.RXFilterLowCut, DttSP.RXFilterHighCut);
            }
        }

        private int max_filter_width = 9999;
        public int MaxFilterWidth
        {
            get { return max_filter_width; }
            set
            {
                max_filter_width = value;
                UpdateFilters(DttSP.RXFilterLowCut, DttSP.RXFilterHighCut);
            }
        }

        private bool mic_boost = false;
        public bool MicBoost
        {
            get { return mic_boost; }
            set
            {
                mic_boost = value;
                udMIC_ValueChanged(this, EventArgs.Empty);
            }
        }

        private bool always_on_top = false; // yt7pwr
        public bool AlwaysOnTop
        {
            get { return always_on_top; }
            set
            {
                always_on_top = value;
                if (value)
                {
                    Win32.SetWindowPos(this.Handle.ToInt32(),
                        -1, this.Left, this.Top, this.Width, this.Height, 0);
                }
                else
                {
                    Win32.SetWindowPos(this.Handle.ToInt32(),
                        -2, this.Left, this.Top, this.Width, this.Height, 0);
                }
            }
        }

        private bool quick_qsy = true;
        public bool QuickQSY
        {
            get { return quick_qsy; }
            set { quick_qsy = value; }
        }

        public bool HideTuneStep
        {
            get { return txtWheelTune.Visible; }
            set
            {
                if (SetupForm != null)
                    txtWheelTune.Visible = value;
            }
        }

        public string DisplayModeText
        {
            get { return comboDisplayMode.Text; }
            set { comboDisplayMode.Text = value; }
        }

        private bool auto_mute = false;
        public bool AutoMute
        {
            get { return auto_mute; }
            set { auto_mute = value; }
        }

        private float multimeter_avg_mult_old = 1 - (float)1 / 10;
        private float multimeter_avg_mult_new = (float)1 / 10;
        private int multimeter_avg_num_blocks = 10;
        public int MultiMeterAvgBlocks
        {
            get { return multimeter_avg_num_blocks; }
            set
            {
                multimeter_avg_num_blocks = value;
                multimeter_avg_mult_old = 1 - (float)1 / multimeter_avg_num_blocks;
                multimeter_avg_mult_new = (float)1 / multimeter_avg_num_blocks;
            }
        }

        private bool vac_auto_enable = false;
        public bool VACAutoEnable
        {
            get { return vac_auto_enable; }
            set
            {
                vac_auto_enable = value;
                if (SetupForm == null) return;
                if (vac_auto_enable)
                {
                    switch (current_dsp_mode)
                    {
                        case DSPMode.DIGL:
                        case DSPMode.DIGU:
                            SetupForm.VACEnable = true;
                            break;
                        default:
                            SetupForm.VACEnable = false;
                            break;
                    }
                }
                else SetupForm.VACEnable = false;
            }
        }

        private float display_cal_offset;					// display calibration offset per volume setting in dB
        public float DisplayCalOffset
        {
            get { return display_cal_offset; }
            set
            {
                display_cal_offset = value;
                Display.DisplayCalOffset = value;
            }
        }

        private int display_cursor_x;						// x-coord of the cursor when over the display
        public int DisplayCursorX
        {
            get { return display_cursor_x; }
            set
            {
                display_cursor_x = value;
                Display.DisplayCursorX = value;
            }
        }

        private int display_cursor_y;						// y-coord of the cursor when over the display
        public int DisplayCursorY
        {
            get { return display_cursor_y; }
            set
            {
                display_cursor_y = value;
                Display.DisplayCursorY = value;
            }
        }

        private ClickTuneMode current_click_tune_mode = ClickTuneMode.Off;
        public ClickTuneMode CurrentClickTuneMode
        {
            get { return current_click_tune_mode; }
            set
            {
                current_click_tune_mode = value;
                Display.CurrentClickTuneMode = value;
            }
        }

        private DisplayEngine current_display_engine = DisplayEngine.GDI_PLUS;
        public DisplayEngine CurrentDisplayEngine
        {
            get { return current_display_engine; }
            set
            {
                switch(value)
                {
                    case DisplayEngine.GDI_PLUS:
                        current_display_engine = value;
//                        Display.DirectXRelease();
                        break;
                    case DisplayEngine.DIRECT_X:
//                        Display.DirectXInit();
                        current_display_engine = value;
//                        Display.PrepareDisplayVars(Display.CurrentDisplayMode);
                        Display.DrawBackground();
                        break;
                }
                Display.CurrentDisplayEngine = value;
            }
        }

        private int digu_click_tune_offset = 1200;
        public int DIGUClickTuneOffset
        {
            get { return digu_click_tune_offset; }
            set { digu_click_tune_offset = value; }
        }

        private int digl_click_tune_offset = 2210;
        public int DIGLClickTuneOffset
        {
            get { return digl_click_tune_offset; }
            set { digl_click_tune_offset = value; }
        }

        private double vox_hang_time = 1500.0;
        public double VOXHangTime
        {
            get { return vox_hang_time; }
            set { vox_hang_time = value; }
        }

        private bool vox_active = false;
        public bool VOXActive
        {
            get { return vox_active; }
            set { vox_active = value; }
        }

        private SoundCard current_soundcard = SoundCard.UNSUPPORTED_CARD;
        public SoundCard CurrentSoundCard
        {
            get { return current_soundcard; }
            set
            {
                current_soundcard = value;
                Audio.CurSoundCard = value;
                if (SetupForm != null && SetupForm.CurrentSoundCard != current_soundcard)
                    SetupForm.CurrentSoundCard = current_soundcard;
            }
        }

        private Model current_model = Model.GENESIS_G59; // changes yt7pwr
        public Model CurrentModel
        {
            get { return current_model; }
            set
            {
                double losc_freq = double.Parse(txtLOSCFreq.Text);
                Model saved_model = current_model;
                current_model = value;
                Display.CurrentModel = value;
                switch (current_model)
                {
                    case Model.GENESIS_G59:
                        MinFreq = 0.011025;
                        MaxFreq = 52.0;
                        break;
                    case Model.GENESIS_G3020:
                        MinFreq = 10.100;
                        MaxFreq = 10.150;
                        break;
                    case Model.GENESIS_G40:
                        MinFreq = 7.0;
                        MaxFreq = 7.200;
                        break;
                    case Model.GENESIS_G80:
                        MinFreq = 3.5;
                        MaxFreq = 3.85;
                        break;
                    case Model.GENESIS_G160:
                        MinFreq = 1.8;
                        MaxFreq = 2.0;
                        break;
                }
                if (SetupForm != null && saved_model != current_model)
                {
                    txtVFOAFreq_LostFocus(this, EventArgs.Empty);
                    txtVFOBFreq_LostFocus(this, EventArgs.Empty);
                    txtLOSCFreq_LostFocus(this, EventArgs.Empty);
                }
            }
        }

        private DateTimeMode current_datetime_mode = DateTimeMode.LOCAL; // changes yt7pwr
        public DateTimeMode CurrentDateTimeMode
        {
            get { return current_datetime_mode; }
            set
            {
                current_datetime_mode = value;
                if (current_datetime_mode == DateTimeMode.OFF)
                {
                    timer_clock.Enabled = false;
                }
                else
                {
                    if (!timer_clock.Enabled)
                        timer_clock.Enabled = true;
                }
            }
        }

        public static bool first = false; // yt7pwr
        private double loscFreq = 10.0;
        public double LOSCFreq
        {
            get
            {
                try
                {
                    return double.Parse(txtLOSCFreq.Text);
                }

                catch (Exception)
                {
                    return 0;
                }
            }
                    set
            {
                if (current_model == Model.GENESIS_G59 || current_model == Model.GENESIS_G160 
                    || current_model == Model.GENESIS_G3020 || current_model == Model.GENESIS_G40
                    || current_model == Model.GENESIS_G80)
                {
                    txtLOSCFreq.Text = value.ToString("f6");
                    double freq = double.Parse(txtLOSCFreq.Text);
                    MinFreq = freq - DttSP.SampleRate / 2 * 1e-6;
                    MaxFreq = freq + DttSP.SampleRate / 2 * 1e-6;
                    if (SetupForm != null && first == false)
                        txtLOSCFreq_LostFocus(this, EventArgs.Empty);
                    first = false;

                    double losc_freq = value;
                    losc_freq *= 1000000;
                    if (usb_si570_enable)
                        SI570.Set_SI570_osc((long)losc_freq);
                    else
                        g59.Set_frequency((long)losc_freq);
                }
                loscFreq = value;
            }
        }

        private float saved_vfoa_freq = 7.0f;
        private float saved_losc_freq = 7.0f;
        private float saved_vfob_freq = 7.0f;
        private bool ext_ctrl_enabled = false;
        public bool ExtCtrlEnabled
        {
            get { return ext_ctrl_enabled; }
            set { ext_ctrl_enabled = value; }
        }

        private bool cw_semi_break_in_enabled = true;
        public bool CWSemiBreakInEnabled
        {
            get { return cw_semi_break_in_enabled; }
            set { cw_semi_break_in_enabled = value; }
        }

        private bool cw_disable_monitor = false;
        public bool CWDisableMonitor
        {
            get { return cw_disable_monitor; }
            set
            {
                cw_disable_monitor = value;
                if (chkCWDisableMonitor != null) chkCWDisableMonitor.Checked = value;
            }
        }

        public float FilterSizeCalOffset
        {
            get { return filter_size_cal_offset; }
            set { filter_size_cal_offset = value; }
        }

        private PTTMode current_ptt_mode = PTTMode.NONE;
        public PTTMode CurrentPTTMode
        {
            get { return current_ptt_mode; }
            set { current_ptt_mode = value; }
        }

        private bool vfo_lock = false;
        public bool VFOLock
        {
            get { return vfo_lock; }
            set
            {
                vfo_lock = value;
                bool enabled = !value;
                txtVFOAFreq.Enabled = enabled;
                btnBand160.Enabled = enabled;
                btnBand80.Enabled = enabled;
                btnBand60.Enabled = enabled;
                btnBand40.Enabled = enabled;
                btnBand30.Enabled = enabled;
                btnBand20.Enabled = enabled;
                btnBand17.Enabled = enabled;
                btnBand15.Enabled = enabled;
                btnBand12.Enabled = enabled;
                btnBand10.Enabled = enabled;
                btnBand6.Enabled = enabled;
                btnBand2.Enabled = enabled;
                btnBandWWV.Enabled = enabled;
                btnBandGEN.Enabled = enabled;

                radModeLSB.Enabled = enabled;
                radModeUSB.Enabled = enabled;
                radModeDSB.Enabled = enabled;
                radModeCWL.Enabled = enabled;
                radModeCWU.Enabled = enabled;
                radModeFMN.Enabled = enabled;
                radModeAM.Enabled = enabled;
                radModeSAM.Enabled = enabled;
                radModeSPEC.Enabled = enabled;
                radModeDIGL.Enabled = enabled;
                radModeDIGU.Enabled = enabled;
                radModeDRM.Enabled = enabled;

                btnVFOBtoA.Enabled = enabled;
                btnVFOSwap.Enabled = enabled;

                btnMemoryQuickRecall.Enabled = enabled;
            }
        }

        private double wave_freq = 0.0;
        private bool wave_playback = false;
        public bool WavePlayback  // changes yt7pwr
        {
            get { return wave_playback; }
            set
            {
                wave_playback = value;
                if (wave_playback)
                {
                    wave_freq = dds_freq * 1000000 % DttSP.SampleRate;
                    DttSP.SetOsc(0.0);
                }
                else
                {
                    txtVFOAFreq_LostFocus(this, EventArgs.Empty);
                    txtLOSCFreq_LostFocus(this, EventArgs.Empty);
                }
            }
        }

        private bool rx_only = false;
        public bool RXOnly
        {
            get { return rx_only; }
            set
            {
                rx_only = value;
                if (current_dsp_mode != DSPMode.SPEC &&
                    current_dsp_mode != DSPMode.DRM)
                    chkMOX.Enabled = !rx_only;
                chkTUN.Enabled = !rx_only;
            }
        }

        private double dds_step_size = 0.0;
        private double corrected_dds_clock = 200.0;
        private double dds_clock_correction = 0.0;
        public double DDSClockCorrection
        {
            get { return dds_clock_correction; }
            set
            {
                dds_clock_correction = value;
                corrected_dds_clock = 200.0 + dds_clock_correction;
                dds_step_size = corrected_dds_clock / Math.Pow(2, 48);
                DDSFreq = dds_freq;
            }
        }

        private BandPlan current_band_plan = BandPlan.IARU1;
        public BandPlan CurrentBandPlan
        {
            get { return current_band_plan; }
            set { current_band_plan = value; }
        }

        private bool spur_reduction = false;
        public bool SpurReduction
        {
            get { return spur_reduction; }
            set
            {
                spur_reduction = value;
                DDSFreq = dds_freq;
                chkSR.Checked = value;
            }
        }

        private bool tx_shift_if = false; // yt7pwr
        public bool tx_IF
        {
            get { return tx_shift_if; }
            set
            { tx_shift_if = value; }
        }

        private double tx_shift = 0.1125; // yt7pwr
        public double TX_IF_shift
        {
            get { return tx_shift; }

            set { tx_shift = value; }
        }

        private double dds_freq = 7.0;
        public double DDSFreq
        {
            get { return dds_freq; }
            set
            {
                dds_freq = value;
                double vfoFreq = value;
                double dsp_osc_freq = LOSCFreq - value;

                if (if_shift)
                    dsp_osc_freq = -11025.0;
            }
        }

        private double TuningWordToFreq(long word)
        {
            return word * corrected_dds_clock / Math.Pow(2, 48);
        }

        private double min_freq = 0.011025; // changes yt7pwr
        public double MinFreq
        {
            get { return min_freq; }
            set
            {
                min_freq = value;
                if (SetupForm == null) return;

                if (VFOAFreq < min_freq)
                {
                    switch (current_dsp_mode)
                    {
                        case DSPMode.LSB:
                            VFOAFreq = min_freq + 0.003;
                            break;
                        case DSPMode.USB:
                            VFOAFreq = min_freq + 0.0002;
                            break;
                        default:
                            VFOAFreq = min_freq;
                            break;
                    }
                }

                if (VFOBFreq < min_freq)
                {
                    switch (current_dsp_mode)
                    {
                        case DSPMode.LSB:
                            VFOBFreq = min_freq + 0.003;
                            break;
                        case DSPMode.USB:
                            VFOBFreq = min_freq + 0.0002;
                            break;
                        default:
                            VFOBFreq = min_freq;
                            break;
                    }
                }
            }
        }

        private double max_freq = 146.0; // changes yt7pwr
        public double MaxFreq
        {
            get { return max_freq; }
            set
            {
                max_freq = value;
                if (SetupForm == null) return;

                if (VFOAFreq > max_freq)
                {
                    switch (current_dsp_mode)
                    {
                        case DSPMode.LSB:
                            VFOAFreq = max_freq - 0.0002;
                            break;
                        case DSPMode.USB:
                            VFOAFreq = max_freq - 0.003;
                            break;
                        default:
                            VFOAFreq = max_freq;
                            break;
                    }
                }

                if (VFOBFreq > max_freq)
                {
                    switch (current_dsp_mode)
                    {
                        case DSPMode.LSB:
                            VFOBFreq = max_freq - 0.0002;
                            break;
                        case DSPMode.USB:
                            VFOBFreq = max_freq - 0.003;
                            break;
                        default:
                            VFOBFreq = max_freq;
                            break;
                    }
                }
            }
        }

        private double vfo_offset = 0.0;
        public double VFOOffset
        {
            get { return vfo_offset; }
            set { vfo_offset = value; }
        }

        private bool if_shift = true;
        public bool IFShift
        {
            get { return if_shift; }
            set { if_shift = value; }
        }

        private bool extended = false;
        public bool Extended
        {
            get { return extended; }
            set { extended = value; }
        }

        private bool enable_LPF0 = false;
        public bool EnableLPF0
        {
            get { return enable_LPF0; }
            set { enable_LPF0 = value; }
        }

        private int latch_delay = 0;
        public int LatchDelay
        {
            get { return latch_delay; }
            set { latch_delay = value; }
        }

        public bool COMP
        {
            get { return chkDSPComp.Checked; }
            set
            {
                if (SetupForm == null)
                    return;

                chkDSPComp.Checked = value;
            }
        }

        public bool CPDR
        {
            get { return chkDSPCompander.Checked; }
            set
            {
                if (SetupForm == null)
                    return;

                chkDSPCompander.Checked = value;
            }
        }

        public int Mic
        {
            get { return (int)udMIC.Value; }
            set
            {
                if (SetupForm == null)
                    return;

                udMIC.Value = value;
            }
        }

        private int tune_power;								// power setting to use when TUN button is pressed
        public int TunePower
        {
            get { return tune_power; }
            set
            {
                tune_power = value;
                if (SetupForm != null)
                    SetupForm.TunePower = tune_power;

                if (chkTUN.Checked)
                {
                    udPWR.Value = tune_power;
                }
            }
        }

        private bool disable_swr_protection = false;
        public bool DisableSWRProtection
        {
            get { return disable_swr_protection; }
            set { disable_swr_protection = value; }
        }

        private int previous_pwr = 50;
        public int PreviousPWR
        {
            get { return previous_pwr; }
            set { previous_pwr = value; }
        }

        private bool no_hardware_offset = false;
        public bool NoHardwareOffset
        {
            get { return no_hardware_offset; }
            set
            {
                no_hardware_offset = value;
                comboPreamp_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        #region CAT Properties

        private Band current_band;
        //BT 06/15/05 Made public for CAT commands
        public Band CurrentBand
        {
            get { return current_band; }
            set
            {
                Band old_band = current_band;
                current_band = value;
                if (current_band != old_band)
                    udPWR_ValueChanged(this, EventArgs.Empty);
            }
        }

        // Added 06/24/05 BT for CAT commands
        public bool CATVFOLock
        {
            get { return chkVFOLock.Checked; }
            set { chkVFOLock.Checked = value; }
        }

        public string CATGetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            //			int current_version = VersionTextToInt(fvi.FileVersion);
            return fvi.FileVersion;
        }

        // Added 07/30/05 BT for cat commands next 8 functions

        public string CATReadSigStrength()
        {
            float num = 0f;
            num = DttSP.CalculateMeter(DttSP.MeterType.SIGNAL_STRENGTH);
            num = num +
                multimeter_cal_offset +
                preamp_offset[(int)current_preamp_mode] +
                filter_size_cal_offset;
            return num.ToString("f1") + " dBm";
        }

        public string CATReadAvgStrength()
        {
            float num = 0f;
            num = DttSP.CalculateMeter(DttSP.MeterType.AVG_SIGNAL_STRENGTH);
            num = num +
                multimeter_cal_offset +
                preamp_offset[(int)current_preamp_mode] +
                filter_size_cal_offset;
            return num.ToString("f1") + " dBm";
        }

        public string CATReadADC_L()
        {
            float num = 0f;
            num = DttSP.CalculateMeter(DttSP.MeterType.ADC_REAL);
            return num.ToString("f1") + " dBFS";
        }

        public string CATReadADC_R()
        {
            float num = 0f;
            num = DttSP.CalculateMeter(DttSP.MeterType.ADC_IMAG);
            return num.ToString("f1") + " dBFS";
        }

        public string CATReadALC()
        {
            float num = 0f;

            if (Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
            {
                num = (float)Math.Max(-20.0, -DttSP.CalculateMeter(DttSP.MeterType.ALC));
                return num.ToString("f1") + " dB";
            }
            else return "0" + separator + "0 dB";
        }

        public string CATReadFwdPwr()
        {
            double power = 0.0;
            float num = 0f;

            if (VFOAFreq < MaxFreq)
            {
                //pa_power_mutex.WaitOne();
                power = PAPower(pa_fwd_power);
                //pa_power_mutex.ReleaseMutex();

                return power.ToString("f0") + " W";
            }
            else
            {
                if (Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
                {
                    num = (float)Math.Max(0.0, DttSP.CalculateMeter(DttSP.MeterType.PWR));
                    num *= (float)((double)udPWR.Value * 0.01);
                    return num.ToString("f2") + " W";
                }
                else return "0" + separator + "00 W";
            }
        }

        public string CATReadPeakPwr()
        {
            float num = 0f;
            if (VFOAFreq < MaxFreq)
            {
                if (Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
                {
                    num = (float)Math.Max(0.0, DttSP.CalculateMeter(DttSP.MeterType.ALC));
                    num *= (float)udPWR.Value;

                    meter_text_history[meter_text_history_index] = num;
                    meter_text_history_index = (meter_text_history_index + 1) % multimeter_text_peak_samples;
                    float max = float.MinValue;
                    for (int i = 0; i < multimeter_text_peak_samples; i++)
                    {
                        if (meter_text_history[i] > max)
                            max = meter_text_history[i];
                    }
                    num = max;

                    return num.ToString("f0") + " W";
                }
                else return "0 W";
            }
            else
            {
                if (Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
                {
                    num = (float)Math.Max(0.0, DttSP.CalculateMeter(DttSP.MeterType.ALC));
                    num *= (float)((float)udPWR.Value * 0.01);
                    return num.ToString("f2") + " W";
                }
                else return "0" + separator + "00 W";
            }
        }

        public string CATReadRevPwr()
        {
            double power = 0.0;
            //pa_power_mutex.WaitOne();
            power = PAPower(pa_rev_power);
            //pa_power_mutex.ReleaseMutex();
            return power.ToString("f0") + " W";
        }

        public string CATReadSWR()
        {
            double swr = 0.0;
            //pa_power_mutex.WaitOne();
            swr = SWR(pa_fwd_power, pa_rev_power);
            //pa_power_mutex.ReleaseMutex();
            return swr.ToString("f1") + " : 1";
        }

        //*************end of 8 functions.

        #endregion

        private DSPMode current_dsp_mode = DSPMode.FIRST;
        public DSPMode CurrentDSPMode
        {
            get { return current_dsp_mode; }
            set
            {
                RadioButtonTS r = null;
                switch (value)
                {
                    case DSPMode.LSB:
                        r = radModeLSB;
                        break;
                    case DSPMode.USB:
                        r = radModeUSB;
                        break;
                    case DSPMode.DSB:
                        r = radModeDSB;
                        break;
                    case DSPMode.CWL:
                        r = radModeCWL;
                        break;
                    case DSPMode.CWU:
                        r = radModeCWU;
                        break;
                    case DSPMode.FMN:
                        r = radModeFMN;
                        break;
                    case DSPMode.AM:
                        r = radModeAM;
                        break;
                    case DSPMode.SAM:
                        r = radModeSAM;
                        break;
                    case DSPMode.SPEC:
                        r = radModeSPEC;
                        break;
                    case DSPMode.DIGL:
                        r = radModeDIGL;
                        break;
                    case DSPMode.DIGU:
                        r = radModeDIGU;
                        break;
                    case DSPMode.DRM:
                        r = radModeDRM;
                        break;
                }

                r.Checked = true;
            }
        }

        private Filter current_filter = Filter.FIRST;
        public Filter CurrentFilter
        {
            get { return current_filter; }
            set
            {
                RadioButtonTS r = null;
                switch (value)
                {
                    case Filter.F1:
                        r = radFilter1;
                        break;
                    case Filter.F2:
                        r = radFilter2;
                        break;
                    case Filter.F3:
                        r = radFilter3;
                        break;
                    case Filter.F4:
                        r = radFilter4;
                        break;
                    case Filter.F5:
                        r = radFilter5;
                        break;
                    case Filter.F6:
                        r = radFilter6;
                        break;
                    case Filter.F7:
                        r = radFilter7;
                        break;
                    case Filter.F8:
                        r = radFilter8;
                        break;
                    case Filter.F9:
                        r = radFilter9;
                        break;
                    case Filter.F10:
                        r = radFilter10;
                        break;
                    case Filter.VAR1:
                        r = radFilterVar1;
                        break;
                    case Filter.VAR2:
                        r = radFilterVar2;
                        break;
                    case Filter.NONE:
                        SetFilter(Filter.NONE);
                        break;
                }

                if (r != null)
                {
                    if (r.Checked)
                    {
                        r.Checked = false;
                    }

                    r.Checked = true;
                }

                //SetFilter(value);  // kd5tfd added for cat zzsf support 
                // commented as changed order in CATCommands.cs should no longer require this
            }
        }

        private MeterRXMode current_meter_rx_mode = MeterRXMode.SIGNAL_STRENGTH;
        public MeterRXMode CurrentMeterRXMode
        {
            get { return current_meter_rx_mode; }
            set
            {
                string text = "";
                switch (value)
                {
                    case MeterRXMode.SIGNAL_STRENGTH:
                        text = "Signal";
                        break;
                    case MeterRXMode.SIGNAL_AVERAGE:
                        text = "Sig Avg";
                        break;
                    case MeterRXMode.ADC_L:
                        text = "ADC L";
                        break;
                    case MeterRXMode.ADC_R:
                        text = "ADC R";
                        break;
                    case MeterRXMode.OFF:	// BT Added 7/24/05 for CAT commands
                        text = "Off";
                        break;
                }

                if (text == "") return;

                comboMeterRXMode.Text = text;
            }
        }

        private MeterTXMode current_meter_tx_mode = MeterTXMode.FIRST;
        public MeterTXMode CurrentMeterTXMode
        {
            get { return current_meter_tx_mode; }
            set
            {
                string text = "";
                switch (value)
                {
                    case MeterTXMode.FORWARD_POWER:
                        text = "Fwd Pwr";
                        break;
                    case MeterTXMode.REVERSE_POWER:
                        text = "Ref Pwr";
                        break;
                    case MeterTXMode.MIC:
                        text = "Mic";
                        break;
                    case MeterTXMode.EQ:
                        text = "EQ";
                        break;
                    case MeterTXMode.LEVELER:
                        text = "Leveler";
                        break;
                    case MeterTXMode.LVL_G:
                        text = "Lvl Gain";
                        break;
                    case MeterTXMode.COMP:
                        text = "COMP";
                        break;
                    case MeterTXMode.CPDR:
                        text = "CPDR";
                        break;
                    case MeterTXMode.ALC:
                        text = "ALC";
                        break;
                    case MeterTXMode.ALC_G:
                        text = "ALC Comp";
                        break;
                    case MeterTXMode.SWR:
                        text = "SWR";
                        break;
                    case MeterTXMode.OFF:		// BT Added 07/24/05 for CAT commands
                        text = "Off";
                        break;
                }
                if (text == "") return;

                comboMeterTXMode.Text = text;
            }
        }

        private int cw_pitch = 600;
        public int CWPitch
        {
            get { return cw_pitch; }
            set
            {
                int diff = value - cw_pitch;
                cw_pitch = value;
                Audio.SineFreq1 = value;
                udCWPitch.Value = value;
                Display.CWPitch = value;

                for (Filter f = Filter.F1; f < Filter.LAST; f++)
                {
                    int low = filter_presets[(int)DSPMode.CWL].GetLow(f);
                    int high = filter_presets[(int)DSPMode.CWL].GetHigh(f);
                    string name = filter_presets[(int)DSPMode.CWL].GetName(f);

                    int bw = high - low;
                    low = -cw_pitch - bw / 2;
                    high = -cw_pitch + bw / 2;
                    filter_presets[(int)DSPMode.CWL].SetFilter(f, low, high, name);

                    low = filter_presets[(int)DSPMode.CWU].GetLow(f);
                    high = filter_presets[(int)DSPMode.CWU].GetHigh(f);
                    name = filter_presets[(int)DSPMode.CWU].GetName(f);

                    bw = high - low;
                    low = cw_pitch - bw / 2;
                    high = cw_pitch + bw / 2;
                    filter_presets[(int)DSPMode.CWU].SetFilter(f, low, high, name);
                }

                if (current_dsp_mode == DSPMode.CWL ||			// if in CW Mode
                    current_dsp_mode == DSPMode.CWU)
                {												// recalculate filter
                    if (current_dsp_mode == DSPMode.CWL)
                    {
                        DttSP.SetKeyerFreq(11250 + cw_pitch);
                    }
                    else // if(current_dsp_mode == DSPMode.CWU)
                    {
                        DttSP.SetKeyerFreq(11250 - cw_pitch);
                    }

                    if (chkMOX.Checked)
                    {
                        if (current_dsp_mode == DSPMode.CWL)
                            diff = -diff;
                        VFOAFreq += (double)diff / 1e6;
                    }
                    else
                    {
                        txtVFOAFreq_LostFocus(this, EventArgs.Empty);
                        txtLOSCFreq_LostFocus(this, EventArgs.Empty);
                    }
                    CurrentFilter = current_filter;
                }
            }
        }

        private int histogram_hang_time = 100;
        public int HistogramHangTime
        {
            get { return histogram_hang_time; }
            set { histogram_hang_time = value; }
        }

        private long band_filter = 0;
        public long current_band_USB
        {
            get
            {
                return band_filter;
            }
            set
            {
                if (band_filter != value)
                    g59.WriteToDevice(3, value);
                band_filter = value;
            }
        }

        private double vfoAFreq = 10.0;
        public double VFOAFreq   // changes yt7pwr
        {
            get
            {
                return double.Parse(txtVFOAFreq.Text);
            }
            set
            {
                if (vfo_lock || SetupForm == null) return;
                txtVFOAFreq.Text = value.ToString("f6");
                if (first == false)
                    txtVFOAFreq_LostFocus(this, EventArgs.Empty);
                else
                    first = false;

                if (!vfoA_drag)
                {
                    RX_phase_gain();
                    TX_phase_gain();
                }
                vfoAFreq = value;
            }
        }

        private double vfoBFreq = 10.0;
        public double VFOBFreq // changes yt7pwr
        {
            get
            {
                return double.Parse(txtVFOBFreq.Text);
            }
            set
            {
                value = Math.Max(0, value);
                txtVFOBFreq.Text = value.ToString("f6");
                if (first == false)
                    txtVFOBFreq_LostFocus(this, EventArgs.Empty);
                else
                    first = false;

                double tmpfreq = Math.Round(VFOBFreq, 3, MidpointRounding.ToEven);

                if (chkVFOSplit.Checked)     // update TX
                    SetTXOscFreqs(true,false);
                vfoBFreq = value;
            }
        }

        public int PWR
        {
            get { return (int)udPWR.Value; }
            set
            {
                value = Math.Max(0, value);			// lower bound
                value = Math.Min(100, value);		// upper bound

                if (current_dsp_mode == DSPMode.FMN)
                    value /= 4;

                udPWR.Value = value;
                udPWR_ValueChanged(this, EventArgs.Empty);
            }
        }

        public int AF
        {
            get { return (int)udAF.Value; }
            set
            {
                value = Math.Max(0, value);			// lower bound
                value = Math.Min(100, value);		// upper bound

                udAF.Value = value;
                udAF_ValueChanged(this, EventArgs.Empty);
            }
        }

        private int rxaf = 50;
        public int RXAF
        {
            get { return rxaf; }
            set { rxaf = value; }
        }

        private int txaf = 50;
        public int TXAF
        {
            get { return txaf; }
            set
            {
                txaf = value;
                if (SetupForm != null)
                {
                    SetupForm.TXAF = txaf;
                    if (MOX) udAF.Value = txaf;
                }
            }
        }

        public bool DisplayAVG
        {
            get { return chkDisplayAVG.Checked; }
            set { chkDisplayAVG.Checked = value; }
        }

        private double break_in_delay = 400;
        public double BreakInDelay
        {
            get { return break_in_delay; }
            set { break_in_delay = value; }
        }

        public bool MOX
        {
            get { return chkMOX.Checked; }
            set { chkMOX.Checked = value; }
        }

        public bool MOXEnabled
        {
            get { return chkMOX.Enabled; }
            set { chkMOX.Enabled = value; }
        }

        public bool MON
        {
            get { return chkMON.Checked; }
            set { chkMON.Checked = value; }
        }

        public bool MUT
        {
            get { return chkMUT.Checked; }
            set { chkMUT.Checked = value; }
        }

        public bool TUN
        {
            get { return chkTUN.Checked; }
            set { chkTUN.Checked = value; }
        }

        public bool TUNEnabled
        {
            get { return chkTUN.Enabled; }
            set { chkTUN.Enabled = value; }
        }

        public int FilterLowValue
        {
            get { return (int)udFilterLow.Value; }
            set { udFilterLow.Value = value; }
        }

        public int FilterHighValue
        {
            get { return (int)udFilterHigh.Value; }
            set { udFilterHigh.Value = value; }
        }

        public int FilterShiftValue
        {
            get { return tbFilterShift.Value; }
            set { tbFilterShift.Value = value; }
        }

        private PreampMode current_preamp_mode = PreampMode.MED;
        public PreampMode CurrentPreampMode
        {
            get { return current_preamp_mode; }
            set
            {
                current_preamp_mode = value;
                switch (current_preamp_mode)
                {
                    case PreampMode.OFF:
                        if (!usb_si570_enable)
                            g59.WriteToDevice(4, 0x00);
                        break;
                    case PreampMode.LOW:
                        if (!usb_si570_enable)
                            g59.WriteToDevice(4, 0x01);
                        break;
                    case PreampMode.MED:
                        if (!usb_si570_enable)
                            g59.WriteToDevice(4, 0x02);
                        break;
                    case PreampMode.HIGH:
                        if (!usb_si570_enable)
                            g59.WriteToDevice(4, 0x03);
                        break;
                }

                switch (current_preamp_mode)
                {
                    case PreampMode.OFF:
                        comboPreamp.Text = "Off";
                        break;
                    case PreampMode.LOW:
                        comboPreamp.Text = "Low";
                        break;
                    case PreampMode.MED:
                        comboPreamp.Text = "Med";
                        break;
                    case PreampMode.HIGH:
                        comboPreamp.Text = "High";
                        break;
                }

                Display.PreampOffset = preamp_offset[(int)current_preamp_mode];
            }
        }

        public int Squelch
        {
            get { return (int)udSquelch.Value; }
            set { udSquelch.Value = value; }
        }

        public int StepSize
        {
            get { return wheel_tune_index; }
        }

        public AGCMode CurrentAGCMode
        {
            get { return (AGCMode)comboAGC.SelectedIndex; }
            set { comboAGC.SelectedIndex = (int)value; }
        }

        public bool VFOSplit
        {
            get { return chkVFOSplit.Checked; }
            set { chkVFOSplit.Checked = value; }
        }

        public bool RIT
        {
            get { return chkRIT.Checked; }
            set { chkRIT.Checked = value; }
        }

        public bool RITOn
        {
            get { return chkRIT.Checked; }
            set { chkRIT.Checked = value; }
        }

        public int RITValue
        {
            get { return (int)udRIT.Value; }
            set { udRIT.Value = value; }
        }

        public bool XITOn
        {
            get { return chkXIT.Checked; }
            set { chkXIT.Checked = value; }
        }

        public int XITValue
        {
            get { return (int)udXIT.Value; }
            set { udXIT.Value = value; }
        }

        private int tx_filter_high = 3000;
        public int TXFilterHigh
        {
            get { return tx_filter_high; }
            set
            {
                tx_filter_high = value;
                DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
                if (chkMOX.Checked)
                  Display.DrawBackground();
            }
        }

        private int tx_filter_low = 300;
        public int TXFilterLow
        {
            get { return tx_filter_low; }
            set
            {
                tx_filter_low = value;
                DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
                if (chkMOX.Checked)
                  Display.DrawBackground();
            }
        }

        private delegate void SetTimerDel(System.Windows.Forms.Timer t, bool enable);
        private void SetTimer(System.Windows.Forms.Timer t, bool enable)
        {
            t.Enabled = enable;
        }

        private bool high_swr = false;
        public bool HighSWR
        {
            get { return high_swr; }
            set
            {
                high_swr = value;
                Display.HighSWR = value;
                Display.DrawBackground();
            }
        }

        private bool disable_ptt = false;
        public bool DisablePTT
        {
            get { return disable_ptt; }
            set { disable_ptt = value; }
        }

        public bool PowerOn
        {
            get { return chkPower.Checked; }
            set { chkPower.Checked = value; }
        }

        public bool PowerEnabled
        {
            get { return chkPower.Enabled; }
            set { chkPower.Enabled = value; }
        }

        private bool second_sound_card_stereo = false;
        public bool SecondSoundCardStereo
        {
            get { return second_sound_card_stereo; }
            set
            {
                second_sound_card_stereo = value;
                Audio.VACStereo = value;
            }
        }

        private bool vac_enabled = false;
        public bool VACEnabled
        {
            get { return vac_enabled; }
            set
            {
                vac_enabled = value;
                Audio.VACEnabled = value;
                if (chkVACEnabled != null) chkVACEnabled.Checked = value;
                if (chkCWVAC != null) chkCWVAC.Checked = value;
            }
        }

        private int audio_driver_index1 = 0;
        public int AudioDriverIndex1
        {
            get { return audio_driver_index1; }
            set { audio_driver_index1 = value; }
        }

        private int audio_driver_index2 = 0;
        public int AudioDriverIndex2
        {
            get { return audio_driver_index2; }
            set { audio_driver_index2 = value; }
        }

        private int audio_input_index1 = 0;
        public int AudioInputIndex1
        {
            get { return audio_input_index1; }
            set { audio_input_index1 = value; }
        }

        private int audio_input_index2 = 0;
        public int AudioInputIndex2
        {
            get { return audio_input_index2; }
            set { audio_input_index2 = value; }
        }

        private int audio_output_index1 = 0;
        public int AudioOutputIndex1
        {
            get { return audio_output_index1; }
            set { audio_output_index1 = value; }
        }

        private int audio_output_index2 = 0;
        public int AudioOutputIndex2
        {
            get { return audio_output_index2; }
            set { audio_output_index2 = value; }
        }

        private int audio_latency1 = 120;
        public int AudioLatency1
        {
            get { return audio_latency1; }
            set { audio_latency1 = value; }
        }

        private int audio_latency2 = 120;
        public int AudioLatency2
        {
            get { return audio_latency2; }
            set { audio_latency2 = value; }
        }

        private double audio_volts1 = 2.23;

        public double AudioVolts1
        {
            get { return audio_volts1; }
            set
            {
                audio_volts1 = value;
                Audio.AudioVolts1 = audio_volts1;
                udPWR_ValueChanged(this, EventArgs.Empty);
            }
        }

        private int mixer_id1 = 0;
        public int MixerID1
        {
            get { return mixer_id1; }
            set { mixer_id1 = value; }
        }

        private int mixer_id2 = 0;
        public int MixerID2
        {
            get { return mixer_id2; }
            set { mixer_id2 = value; }
        }

        private int mixer_rx_mux_id1 = 0;
        public int MixerRXMuxID1
        {
            get { return mixer_rx_mux_id1; }
            set { mixer_rx_mux_id1 = value; }
        }

        private int mixer_tx_mux_id1 = 0;
        public int MixerTXMuxID1
        {
            get { return mixer_tx_mux_id1; }
            set { mixer_tx_mux_id1 = value; }
        }

        private int mixer_rx_mux_id2 = 0;
        public int MixerRXMuxID2
        {
            get { return mixer_rx_mux_id2; }
            set { mixer_rx_mux_id2 = value; }
        }

        private int mixer_tx_mux_id2 = 0;
        public int MixerTXMuxID2
        {
            get { return mixer_tx_mux_id2; }
            set { mixer_tx_mux_id2 = value; }
        }

        private int sample_rate1 = 48000;
        public int SampleRate1
        {
            get { return sample_rate1; }
            set
            {
                sample_rate1 = value;
                Audio.SampleRate1 = value;
                Display.SampleRate = value;

                if (current_dsp_mode == DSPMode.SPEC)
                    SetMode(DSPMode.SPEC);
                else if (Display.CurrentDisplayMode == DisplayMode.PANADAPTER ||
                    Display.CurrentDisplayMode == DisplayMode.PANAFALL || Display.CurrentDisplayMode == DisplayMode.OFF ||
                    Display.CurrentDisplayMode == DisplayMode.PHASE || Display.CurrentDisplayMode == DisplayMode.PHASE2 ||
                    Display.CurrentDisplayMode == DisplayMode.SCOPE || Display.CurrentDisplayMode == DisplayMode.SPECTRUM ||
                    Display.CurrentDisplayMode == DisplayMode.HISTOGRAM)
                    CalcDisplayFreq();
            }
        }

        private int sample_rate2 = 48000;
        public int SampleRate2
        {
            get { return sample_rate2; }
            set
            {
                sample_rate2 = value;
                Audio.SampleRate2 = value;
            }
        }

        private int num_channels = 2;
        public int NumChannels
        {
            get { return num_channels; }
            set { num_channels = value; }
        }

        private int block_size1;
        public int BlockSize1
        {
            get { return block_size1; }
            set
            {
                block_size1 = value;
                Audio.BlockSize = value;
            }
        }

        private int block_size2;
        public int BlockSize2
        {
            get { return block_size2; }
            set
            {
                block_size2 = value;
                Audio.BlockSizeVAC = value;
            }
        }

        private bool cw_key_mode = false;
        public bool CWKeyMode
        {
            get { return cw_key_mode; }
            set { cw_key_mode = value; }
        }

        private int peak_text_delay = 500;
        public int PeakTextDelay
        {
            get { return peak_text_delay; }
            set
            {
                peak_text_delay = value;
                timer_peak_text.Interval = value;
            }
        }

        private int meter_delay = 100;
        public int MeterDelay
        {
            get { return meter_delay; }
            set
            {
                meter_delay = value;
                MultimeterPeakHoldTime = MultimeterPeakHoldTime;
            }
        }

        private int cpu_meter_delay = 1000;
        public int CPUMeterDelay
        {
            get { return cpu_meter_delay; }
            set
            {
                cpu_meter_delay = value;
                timer_cpu_meter.Interval = value;
            }
        }

        private int display_fps = 15;
        private int display_delay = 1000 / 15;
        public int DisplayFPS
        {
            get { return display_fps; }
            set
            {
                display_fps = value;
                display_delay = 1000 / display_fps;
            }
        }

        private int multimeter_peak_hold_time = 1000;
        private int multimeter_peak_hold_samples = 10;
        public int MultimeterPeakHoldTime
        {
            get { return multimeter_peak_hold_time; }
            set
            {
                multimeter_peak_hold_time = value;
                multimeter_peak_hold_samples = value / meter_delay;
            }
        }

        private int multimeter_text_peak_time = 500;
        private int multimeter_text_peak_samples = 5;
        public int MultimeterTextPeakTime
        {
            get { return multimeter_text_peak_time; }
            set
            {
                multimeter_text_peak_time = value;
                multimeter_text_peak_samples = value / meter_delay;
                if (multimeter_text_peak_samples > meter_text_history.Length)
                {
                    float[] temp = new float[multimeter_text_peak_samples];
                    for (int i = 0; i < meter_text_history.Length; i++)
                        temp[i] = meter_text_history[i];
                    meter_text_history = temp;
                }
            }
        }

        private Color losc_text_light_color = Color.Yellow; // yt7pwr
        public Color LOSCTextLightColor
        {
            get { return losc_text_light_color; }
            set
            {
                if (chkPower.Checked)
                {
                    txtLOSCFreq.ForeColor = value;
                    txtLOSCMSD.ForeColor = value;
                }

                losc_text_light_color = value;
            }
        }

        private Color vfo_text_light_color = Color.Yellow;
        public Color VFOTextLightColor
        {
            get { return vfo_text_light_color; }
            set
            {
                if (chkPower.Checked)
                {
                    txtVFOAFreq.ForeColor = value;
                    txtVFOAMSD.ForeColor = value;
                }

                vfo_text_light_color = value;
            }
        }

        private Color losc_text_dark_color = Color.Olive;
        public Color LoscTextDarkColor
        {
            get { return losc_text_dark_color; }
            set
            {
                txtLOSCFreq.ForeColor = value;
                txtLOSCMSD.ForeColor = value;
                txtLOSCLSD.ForeColor = value;

                losc_text_dark_color = value;
            }
        }

        private Color vfo_text_dark_color = Color.Olive;
        public Color VFOTextDarkColor
        {
            get { return vfo_text_dark_color; }
            set
            {
                if (!chkPower.Checked)
                {
                    txtVFOAFreq.ForeColor = value;
                    txtVFOAMSD.ForeColor = value;
                    txtVFOALSD.ForeColor = value;
                }
                if (!chkVFOSplit.Checked)
                {
                    txtVFOBFreq.ForeColor = value;
                    txtVFOBMSD.ForeColor = value;
                    txtVFOBLSD.ForeColor = value;
                }

                vfo_text_dark_color = value;
            }
        }

        private Color band_text_light_color = Color.Lime;
        public Color BandTextLightColor
        {
            get { return band_text_light_color; }
            set
            {
                if (chkPower.Checked)
                    txtVFOABand.ForeColor = value;
                if (chkVFOSplit.Checked)
                    txtVFOBBand.ForeColor = value;

                band_text_light_color = value;
            }
        }

        private Color band_text_dark_color = Color.Green;
        public Color BandTextDarkColor
        {
            get { return band_text_dark_color; }
            set
            {
                if (!chkPower.Checked)
                    txtVFOABand.ForeColor = value;
                if (!chkVFOSplit.Checked)
                    txtVFOBBand.ForeColor = value;

                band_text_dark_color = value;
            }
        }

        private Color peak_text_color = Color.DodgerBlue;
        public Color PeakTextColor
        {
            get { return peak_text_color; }
            set
            {
                peak_text_color = value;
                txtDisplayCursorOffset.ForeColor = value;
                txtDisplayCursorPower.ForeColor = value;
                txtDisplayCursorFreq.ForeColor = value;
                txtDisplayPeakOffset.ForeColor = value;
                txtDisplayPeakPower.ForeColor = value;
                txtDisplayPeakFreq.ForeColor = value;
            }
        }

        private Color out_of_band_color = Color.DimGray;
        public Color OutOfBandColor
        {
            get { return out_of_band_color; }
            set
            {
                out_of_band_color = value;
                if (SetupForm != null)
                    txtVFOAFreq_LostFocus(this, EventArgs.Empty);
            }
        }

        private Color button_selected_color = Color.Yellow;
        public Color ButtonSelectedColor
        {
            get { return button_selected_color; }
            set
            {
                button_selected_color = value;
                CheckSelectedButtonColor();
            }
        }

        private Color meter_left_color = Color.Green;
        public Color MeterLeftColor
        {
            get { return meter_left_color; }
            set
            {
                meter_left_color = value;
                picMultiMeterDigital.Invalidate();
            }
        }

        private Color meter_right_color = Color.Lime;
        public Color MeterRightColor
        {
            get { return meter_right_color; }
            set
            {
                meter_right_color = value;
                picMultiMeterDigital.Invalidate();
            }
        }

        private Keys key_show_hide_gui = Keys.F1; // yt7pwr
        public Keys KeyShowHideGUI                // GUI for Si570 external osillator
        {
            get { return key_show_hide_gui; }
            set { key_show_hide_gui = value; }
        }

        private Keys key_tune_up_1 = Keys.Q;
        public Keys KeyTuneUp1
        {
            get { return key_tune_up_1; }
            set { key_tune_up_1 = value; }
        }

        private Keys key_tune_down_1 = Keys.A;
        public Keys KeyTuneDown1
        {
            get { return key_tune_down_1; }
            set { key_tune_down_1 = value; }
        }

        private Keys key_tune_up_2 = Keys.W;
        public Keys KeyTuneUp2
        {
            get { return key_tune_up_2; }
            set { key_tune_up_2 = value; }
        }

        private Keys key_tune_down_2 = Keys.S;
        public Keys KeyTuneDown2
        {
            get { return key_tune_down_2; }
            set { key_tune_down_2 = value; }
        }

        private Keys key_tune_up_3 = Keys.E;
        public Keys KeyTuneUp3
        {
            get { return key_tune_up_3; }
            set { key_tune_up_3 = value; }
        }

        private Keys key_tune_down_3 = Keys.D;
        public Keys KeyTuneDown3
        {
            get { return key_tune_down_3; }
            set { key_tune_down_3 = value; }
        }

        private Keys key_tune_up_4 = Keys.R;
        public Keys KeyTuneUp4
        {
            get { return key_tune_up_4; }
            set { key_tune_up_4 = value; }
        }

        private Keys key_tune_down_4 = Keys.F;
        public Keys KeyTuneDown4
        {
            get { return key_tune_down_4; }
            set { key_tune_down_4 = value; }
        }

        private Keys key_tune_up_5 = Keys.T;
        public Keys KeyTuneUp5
        {
            get { return key_tune_up_5; }
            set { key_tune_up_5 = value; }
        }

        private Keys key_tune_down_5 = Keys.G;
        public Keys KeyTuneDown5
        {
            get { return key_tune_down_5; }
            set { key_tune_down_5 = value; }
        }

        private Keys key_tune_up_6 = Keys.Y;
        public Keys KeyTuneUp6
        {
            get { return key_tune_up_6; }
            set { key_tune_up_6 = value; }
        }

        private Keys key_tune_down_6 = Keys.H;
        public Keys KeyTuneDown6
        {
            get { return key_tune_down_6; }
            set { key_tune_down_6 = value; }
        }

        private Keys key_tune_up_7 = Keys.U;
        public Keys KeyTuneUp7
        {
            get { return key_tune_up_7; }
            set { key_tune_up_7 = value; }
        }

        private Keys key_tune_down_7 = Keys.J;
        public Keys KeyTuneDown7
        {
            get { return key_tune_down_7; }
            set { key_tune_down_7 = value; }
        }

        private Keys key_rit_up = Keys.O;
        public Keys KeyRITUp
        {
            get { return key_rit_up; }
            set { key_rit_up = value; }
        }

        private Keys key_rit_down = Keys.I;
        public Keys KeyRITDown
        {
            get { return key_rit_down; }
            set { key_rit_down = value; }
        }

        private int rit_step_rate = 50;
        public int RITStepRate
        {
            get { return rit_step_rate; }
            set { rit_step_rate = value; }
        }

        private Keys key_xit_up = Keys.OemOpenBrackets;
        public Keys KeyXITUp
        {
            get { return key_xit_up; }
            set { key_xit_up = value; }
        }

        private Keys key_xit_down = Keys.P;
        public Keys KeyXITDown
        {
            get { return key_xit_down; }
            set { key_xit_down = value; }
        }

        private int xit_step_rate = 50;
        public int XITStepRate
        {
            get { return xit_step_rate; }
            set { xit_step_rate = value; }
        }

        private Keys key_band_up = Keys.M;
        public Keys KeyBandUp
        {
            get { return key_band_up; }
            set { key_band_up = value; }
        }

        private Keys key_band_down = Keys.N;
        public Keys KeyBandDown
        {
            get { return key_band_down; }
            set { key_band_down = value; }
        }

        private Keys key_filter_up = Keys.B;
        public Keys KeyFilterUp
        {
            get { return key_filter_up; }
            set { key_filter_up = value; }
        }

        private Keys key_filter_down = Keys.V;
        public Keys KeyFilterDown
        {
            get { return key_filter_down; }
            set { key_filter_down = value; }
        }

        private Keys key_mode_up = Keys.X;
        public Keys KeyModeUp
        {
            get { return key_mode_up; }
            set { key_mode_up = value; }
        }

        private Keys key_mode_down = Keys.Z;
        public Keys KeyModeDown
        {
            get { return key_mode_down; }
            set { key_mode_down = value; }
        }

        private Keys key_cw_dot = Keys.OemPeriod;
        public Keys KeyCWDot
        {
            get { return key_cw_dot; }
            set { key_cw_dot = value; }
        }

        private Keys key_cw_dash = Keys.OemQuestion;
        public Keys KeyCWDash
        {
            get { return key_cw_dash; }
            set { key_cw_dash = value; }
        }

        private PerformanceCounter cpu_usage;
        public float CpuUsage
        {
            get
            {
                try
                {
                    if (cpu_usage == null)
                    {
                        cpu_usage = new PerformanceCounter(
                            "Processor", "% Processor Time", "_Total", true);
                    }
                    return cpu_usage.NextValue();
                }
                catch (Exception)
                {
                    timer_cpu_meter.Enabled = false;
                    lblCPUMeter.Visible = false;
                    return 0.0f;
                }
            }
        }

        private int scope_time = 50;
        public int ScopeTime
        {
            get { return scope_time; }
            set
            {
                scope_time = value;
                Display.ScopeTime = value;
            }
        }

        // Added 6/11/05 BT to support CAT
        public float MultiMeterCalOffset
        {
            get { return multimeter_cal_offset; }
        }

        public float PreampOffset
        {
            get { return preamp_offset[(int)current_preamp_mode]; }
        }


        #endregion

        #region Display Routines

        public int count = 0;

        private void UpdateDisplay()
        {
            switch (current_display_engine)
            {
                case DisplayEngine.GDI_PLUS:
                    count++;
                    switch (Display.CurrentDisplayMode)
                    {
                        case(DisplayMode.PANADAPTER):
                            picDisplay.Invalidate();
                            break;
                        case(DisplayMode.WATERFALL):
                            picWaterfall.Invalidate();
                            break;
                        case(DisplayMode.PANAFALL):
                            picWaterfall.Invalidate();
                            picDisplay.Invalidate();
                            break;
                        default:
                            picDisplay.Invalidate();
                            break;
                    }
                    break;
                case DisplayEngine.DIRECT_X:
//                    Display.RenderDirectX();
                    break;
            }
        }

        private void UpdatePeakText()
        {
            if (txtVFOAFreq.Text == "" ||
                txtVFOAFreq.Text == "." ||
                txtVFOAFreq.Text == ",")
                return;

            // update peak value
            float x = PixelToHz(Display.MaxX);
            float y = PixelToDb(Display.MaxY);
            y = Display.MaxY;

            double freq = double.Parse(txtVFOAFreq.Text) + (double)x * 0.0000010;

            if (current_dsp_mode == DSPMode.CWL)
                freq += (double)cw_pitch * 0.0000010;
            else if (current_dsp_mode == DSPMode.CWU)
                freq -= (double)cw_pitch * 0.0000010;

            switch (Display.CurrentDisplayMode)
            {
                case (DisplayMode.PANAFALL):
                    Display.MaxY = picWaterfall.Height/2;
                    break;
                case (DisplayMode.PANADAPTER):
                    Display.MaxY = picDisplay.Height;
                    break;
                case (DisplayMode.WATERFALL):
                    Display.MaxY = picWaterfall.Height;
                    break;
                default:
                    Display.MaxY = picDisplay.Height;
                    break;
            }

            txtDisplayPeakOffset.Text = x.ToString("f1") + "Hz";
            txtDisplayPeakPower.Text = y.ToString("f1") + "dBm";

            string temp_text = freq.ToString("f6") + " MHz";
            int jper = temp_text.IndexOf(separator) + 4;
            txtDisplayPeakFreq.Text = String.Copy(temp_text.Insert(jper, " "));
        }

        private float PixelToHz(float x)
        {
            int low, high;
            if (!chkMOX.Checked)
            {
                low = DttSP.RXDisplayLow;
                high = DttSP.RXDisplayHigh;
            }
            else
            {
                low = DttSP.TXDisplayLow;
                high = DttSP.TXDisplayHigh;
            }

            int width = high - low;
            return (float)(low + (double)x / (double)picDisplay.Width * (double)width);
            //return (float)(low + ((double)x*(high - low)/picDisplay.Width));
        }

        private int HzToPixel(float freq)
        {
            int low, high;
            if (!chkMOX.Checked)
            {
                low = DttSP.RXDisplayLow;
                high = DttSP.RXDisplayHigh;
            }
            else
            {
                low = DttSP.TXDisplayLow;
                high = DttSP.TXDisplayHigh;
            }

            int width = high - low;
            return (int)((double)(freq - low) / (double)width * (double)picDisplay.Width);
            //return picDisplay.Width/2+(int)(freq/(high-low)*picDisplay.Width);
        }

        private float PixelToDb(float y)
        {
            return (float)(Display.SpectrumGridMax - y * (double)(Display.SpectrumGridMax - Display.SpectrumGridMin) / picDisplay.Height);
        }
        #endregion

        #region Paint Event Handlers
        // ======================================================
        // Paint Event Handlers
        // ======================================================

        private void picDisplay_Paint(object sender, PaintEventArgs e)
        {
            switch (current_display_engine)
            {
                case DisplayEngine.GDI_PLUS:
                    {
                        switch (Display.CurrentDisplayMode)
                        {
                            case (DisplayMode.PANAFALL):
                            case (DisplayMode.PANADAPTER):
                            case (DisplayMode.WATERFALL):
                                Display.RenderGDIPlusPanadapter(ref e);
                                break;
                            default:
                                Display.RenderGDIPlus(ref e);
                                break;
                        }
                    }
                    break;
                case DisplayEngine.DIRECT_X:
/*                    Thread t = new Thread(new ThreadStart(Display.RenderDirectX));
                        t.Name = "DirectX Background Update";
                        t.IsBackground = true;
                        t.Priority = ThreadPriority.Normal;
                        t.Start();*/
                    break;
            }
        }

        private double avg_num = -200.0;
        private void picMultiMeterDigital_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int H = picMultiMeterDigital.ClientSize.Height;
            int W = picMultiMeterDigital.ClientSize.Width;
            Graphics g = e.Graphics;
            double num;
            int pixel_x = 0;

            switch (current_meter_display_mode)
            {
                case MultiMeterDisplayMode.Original:
                    #region Original
                    if (!chkMOX.Checked)
                    {
                        if (meter_data_ready)
                        {
                            current_meter_data = new_meter_data;
                            //meter_data_ready = false;  We do NOT want to do this before we have consumed it!!!!
                        }

                        num = current_meter_data;

                        switch (current_meter_rx_mode)
                        {
                            case MeterRXMode.SIGNAL_STRENGTH:
                            case MeterRXMode.SIGNAL_AVERAGE:
                                switch ((int)g.DpiX)
                                {
                                    case 96:
                                        double s = (num + 127) / 6;
                                        if (s <= 9.0F)
                                            pixel_x = (int)((s * 7.5) + 2);
                                        else
                                        {
                                            double over_s9 = num + 73;
                                            pixel_x = 69 + (int)(over_s9 * 1.05);
                                        }
                                        break;
                                    case 120:
                                        if (num <= -97.0f)
                                            pixel_x = (int)(0 + (num + 100.0) / 3.0 * 10);
                                        else if (num <= -91.0f)
                                            pixel_x = (int)(10 + (num + 97.0) / 6.0 * 17);
                                        else if (num <= -85.0f)
                                            pixel_x = (int)(27 + (num + 91.0) / 6.0 * 16);
                                        else if (num <= -79.0f)
                                            pixel_x = (int)(43 + (num + 85.0) / 6.0 * 17);
                                        else if (num <= -73.0f)
                                            pixel_x = (int)(60 + (num + 79.0) / 6.0 * 16);
                                        else if (num <= -53.0f)
                                            pixel_x = (int)(76 + (num + 73.0) / 20.0 * 24);
                                        else if (num <= -33.0f)
                                            pixel_x = (int)(100 + (num + 53.0) / 20.0 * 24);
                                        else if (num <= -13.0f)
                                            pixel_x = (int)(124 + (num + 33.0) / 20.0 * 24);
                                        else
                                            pixel_x = (int)(148 + (num + 13.0) / 20.0 * 19);
                                        break;
                                }
                                break;
                            case MeterRXMode.ADC_L:
                            case MeterRXMode.ADC_R:
                                switch ((int)g.DpiX)
                                {
                                    case 96:
                                        pixel_x = (int)(((num + 100) * 1.2) + 12);
                                        break;
                                    case 120:
                                        if (num <= -100.0f)
                                            pixel_x = (int)(0 + (num + 110.0) / 10.0 * 14);
                                        else if (num <= -80.0f)
                                            pixel_x = (int)(14 + (num + 100.0) / 20.0 * 27);
                                        else if (num <= -60.0f)
                                            pixel_x = (int)(41 + (num + 80.0) / 20.0 * 28);
                                        else if (num <= -40.0f)
                                            pixel_x = (int)(69 + (num + 60.0) / 20.0 * 28);
                                        else if (num <= -20.0f)
                                            pixel_x = (int)(97 + (num + 40.0) / 20.0 * 27);
                                        else if (num <= 0.0f)
                                            pixel_x = (int)(124 + (num + 20.0) / 20.0 * 24);
                                        else
                                            pixel_x = (int)(148 + (num - 0.0) / 10.0 * 19);
                                        break;
                                }
                                break;
                            case MeterRXMode.OFF:
                                break;
                        }
                    }
                    else
                    {
                        if (meter_data_ready)
                        {
                            current_meter_data = new_meter_data;
                            //  meter_data_ready = false;  NOT HERE, wait until consumed
                        }

                        num = current_meter_data;

                        switch (current_meter_tx_mode)
                        {
                            case MeterTXMode.MIC:
                            case MeterTXMode.EQ:
                            case MeterTXMode.LEVELER:
                            case MeterTXMode.COMP:
                            case MeterTXMode.CPDR:
                            case MeterTXMode.ALC:
                                //num += 3.0;  // number no longer has fudge factor added in the dsp, must be remove
                                switch ((int)g.DpiX)
                                {
                                    case 96:
                                        if (num <= -20.0f)
                                            pixel_x = (int)(0 + (num + 25.0) / 5.0 * 9);
                                        else if (num <= -10.0f)
                                            pixel_x = (int)(9 + (num + 20.0) / 10.0 * 27);
                                        else if (num <= -5.0f)
                                            pixel_x = (int)(36 + (num + 10.0) / 5.0 * 27);
                                        else if (num <= 0.0f)
                                            pixel_x = (int)(63 + (num + 5.0) / 5.0 * 24);
                                        else if (num <= 1.0f)
                                            pixel_x = (int)(87 + (num - 0.0) / 1.0 * 15);
                                        else if (num <= 2.0f)
                                            pixel_x = (int)(102 + (num - 1.0) / 1.0 * 15);
                                        else if (num <= 3.0f)
                                            pixel_x = (int)(117 + (num - 2.0) / 1.0 * 15);
                                        else
                                            pixel_x = (int)(132 + (num - 3.0) / 0.5 * 8);
                                        break;
                                    case 120:
                                        if (num <= -20.0f)
                                            pixel_x = (int)(0 + (num + 25.0) / 5.0 * 10);
                                        else if (num <= -10.0f)
                                            pixel_x = (int)(10 + (num + 20.0) / 10.0 * 30);
                                        else if (num <= -5.0f)
                                            pixel_x = (int)(40 + (num + 10.0) / 5.0 * 30);
                                        else if (num <= 0.0f)
                                            pixel_x = (int)(70 + (num + 5.0) / 5.0 * 27);
                                        else if (num <= 1.0f)
                                            pixel_x = (int)(97 + (num - 0.0) / 1.0 * 17);
                                        else if (num <= 2.0f)
                                            pixel_x = (int)(114 + (num - 1.0) / 1.0 * 17);
                                        else if (num <= 3.0f)
                                            pixel_x = (int)(131 + (num - 2.0) / 1.0 * 17);
                                        else
                                            pixel_x = (int)(148 + (num - 3.0) / 0.5 * 23);
                                        break;
                                }
                                break;
                            case MeterTXMode.FORWARD_POWER:
                            case MeterTXMode.REVERSE_POWER:
                                switch ((int)g.DpiX)
                                {
                                    case 96:
                                        if (num <= 1.0f)
                                            pixel_x = (int)(0 + num * 2);
                                        else if (num <= 5.0f)
                                            pixel_x = (int)(2 + (num - 1) / 4 * 24);
                                        else if (num <= 10.0f)
                                            pixel_x = (int)(26 + (num - 5) / 5 * 24);
                                        else if (num <= 50.0f)
                                            pixel_x = (int)(50 + (num - 10) / 40 * 24);
                                        else if (num <= 100.0f)
                                            pixel_x = (int)(74 + (num - 50) / 50 * 24);
                                        else if (num <= 120.0f)
                                            pixel_x = (int)(98 + (num - 100) / 20 * 24);
                                        else
                                            pixel_x = (int)(122 + (num - 120) / 20 * 16);
                                        break;
                                    case 120:
                                        if (num <= 1.0f)
                                            pixel_x = (int)(0 + num * 3);
                                        else if (num <= 5.0f)
                                            pixel_x = (int)(3 + (num - 1) / 4 * 26);
                                        else if (num <= 10.0f)
                                            pixel_x = (int)(29 + (num - 5) / 5 * 26);
                                        else if (num <= 50.0f)
                                            pixel_x = (int)(55 + (num - 10) / 40 * 27);
                                        else if (num <= 100.0f)
                                            pixel_x = (int)(82 + (num - 50) / 50 * 28);
                                        else if (num <= 120.0f)
                                            pixel_x = (int)(110 + (num - 100) / 20 * 27);
                                        else
                                            pixel_x = (int)(137 + (num - 120) / 20 * 30);
                                        break;
                                }
                                break;
                            case MeterTXMode.SWR:
                                switch ((int)g.DpiX)
                                {
                                    case 96:
                                        if (double.IsInfinity(num))
                                            pixel_x = 200;
                                        else if (num <= 1.0f)
                                            pixel_x = (int)(0 + num * 3);
                                        else if (num <= 1.5f)
                                            pixel_x = (int)(3 + (num - 1.0) / 0.5 * 27);
                                        else if (num <= 2.0f)
                                            pixel_x = (int)(30 + (num - 1.5) / 0.5 * 20);
                                        else if (num <= 3.0f)
                                            pixel_x = (int)(50 + (num - 2.0) / 1.0 * 21);
                                        else if (num <= 5.0f)
                                            pixel_x = (int)(71 + (num - 3.0) / 2.0 * 21);
                                        else if (num <= 10.0f)
                                            pixel_x = (int)(92 + (num - 5.0) / 5.0 * 21);
                                        else
                                            pixel_x = (int)(113 + (num - 10.0) / 15.0 * 26);
                                        break;
                                    case 120:
                                        if (double.IsInfinity(num))
                                            pixel_x = 200;
                                        else if (num <= 1.0f)
                                            pixel_x = (int)(0 + num * 3);
                                        else if (num <= 1.5f)
                                            pixel_x = (int)(3 + (num - 1.0) / 0.5 * 31);
                                        else if (num <= 2.0f)
                                            pixel_x = (int)(34 + (num - 1.5) / 0.5 * 22);
                                        else if (num <= 3.0f)
                                            pixel_x = (int)(56 + (num - 2.0) / 1.0 * 22);
                                        else if (num <= 5.0f)
                                            pixel_x = (int)(78 + (num - 3.0) / 2.0 * 23);
                                        else if (num <= 10.0f)
                                            pixel_x = (int)(101 + (num - 5.0) / 5.0 * 23);
                                        else
                                            pixel_x = (int)(124 + (num - 10.0) / 15.0 * 43);
                                        break;
                                }
                                break;
                            case MeterTXMode.ALC_G:
                            case MeterTXMode.LVL_G:
                                switch ((int)g.DpiX)
                                {
                                    case 96:
                                        if (num <= 0.0f)
                                            pixel_x = 3;
                                        else if (num <= 5.0f)
                                            pixel_x = (int)(3 + (num - 0.0) / 5.0 * 28);
                                        else if (num <= 10.0f)
                                            pixel_x = (int)(31 + (num - 5.0) / 5.0 * 29);
                                        else if (num <= 15.0f)
                                            pixel_x = (int)(60 + (num - 10.0) / 5.0 * 30);
                                        else if (num <= 20.0f)
                                            pixel_x = (int)(90 + (num - 15.0) / 5.0 * 31);
                                        else
                                            pixel_x = (int)(121 + (num - 20.0) / 5.0 * 29);
                                        break;
                                    case 120:
                                        if (num <= 0.0f)
                                            pixel_x = 3;
                                        else if (num <= 5.0f)
                                            pixel_x = (int)(3 + (num - 0.0) / 5.0 * 31);
                                        else if (num <= 10.0f)
                                            pixel_x = (int)(34 + (num - 5.0) / 5.0 * 33);
                                        else if (num <= 15.0f)
                                            pixel_x = (int)(77 + (num - 10.0) / 5.0 * 33);
                                        else if (num <= 20.0f)
                                            pixel_x = (int)(110 + (num - 15.0) / 5.0 * 35);
                                        else
                                            pixel_x = (int)(145 + (num - 20.0) / 5.0 * 32);
                                        break;
                                }
                                break;
                            case MeterTXMode.OFF:
                                break;
                        }
                    }
                    if (meter_data_ready)
                    {
                        meter_data_ready = false;  //We do NOT want to do this before we have consumed it!!!! so do it here.
                    }

                    switch ((int)g.DpiX)
                    {
                        case 96:
                            if (pixel_x > 139) pixel_x = 139;
                            break;
                        case 120:
                            if (pixel_x > 167) pixel_x = 167;
                            break;
                    }

                    if ((!chkMOX.Checked && current_meter_rx_mode != MeterRXMode.OFF) ||
                        (chkMOX.Checked && current_meter_tx_mode != MeterTXMode.OFF))
                    {
                        if (pixel_x <= 0) pixel_x = 1;

                        LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, pixel_x, H),
                            meter_left_color, meter_right_color, LinearGradientMode.Horizontal);

                        g.FillRectangle(brush, 0, 0, pixel_x, H);

                        for (int i = 0; i < 21; i++)
                            g.DrawLine(new Pen(meter_background_color), 6 + i * 8, 0, 6 + i * 8, H);

                        g.DrawLine(new Pen(Color.Red), pixel_x, 0, pixel_x, H);
                        g.FillRectangle(new SolidBrush(meter_background_color), pixel_x + 1, 0, W - pixel_x, H);

                        if (pixel_x >= meter_peak_value)
                        {
                            meter_peak_count = 0;
                            meter_peak_value = pixel_x;
                        }
                        else
                        {
                            if (meter_peak_count++ >= multimeter_peak_hold_samples)
                            {
                                meter_peak_count = 0;
                                meter_peak_value = pixel_x;
                            }
                            else
                            {
                                g.DrawLine(new Pen(Color.Red), meter_peak_value, 0, meter_peak_value, H);
                                g.DrawLine(new Pen(Color.Red), meter_peak_value - 1, 0, meter_peak_value - 1, H);
                            }
                        }
                    }
                    break;
                    #endregion
                case MultiMeterDisplayMode.Edge:
                    #region Edge
                    if (meter_data_ready)
                    {
                        current_meter_data = new_meter_data;
                    }

                    if (current_meter_data > avg_num)
                        num = avg_num = current_meter_data * 0.8 + avg_num * 0.2; // fast rise
                    else
                        num = avg_num = current_meter_data * 0.2 + avg_num * 0.8; // slow decay

                    g.DrawRectangle(new Pen(edge_meter_background_color), 0, 0, W, H);

                    SolidBrush low_brush = new SolidBrush(edge_low_color);
                    SolidBrush high_brush = new SolidBrush(edge_high_color);

                    if (!chkMOX.Checked)
                    {
                        switch (current_meter_rx_mode)
                        {
                            case MeterRXMode.SIGNAL_STRENGTH:
                            case MeterRXMode.SIGNAL_AVERAGE:
                                g.FillRectangle(low_brush, 0, H - 4, (int)(W * 0.5), 2);
                                g.FillRectangle(high_brush, (int)(W * 0.5), H - 4, (int)(W * 0.5) - 4, 2);
                                double spacing = (W * 0.5 - 2.0) / 5.0;
                                double string_height = 0;
                                for (int i = 1; i < 6; i++)
                                {
                                    g.FillRectangle(low_brush, (int)(i * spacing - spacing * 0.5), H - 4 - 3, 1, 3);
                                    g.FillRectangle(low_brush, (int)(i * spacing), H - 4 - 6, 2, 6);

                                    Font f = new Font("Arial", 7.0f, FontStyle.Bold);
                                    SizeF size = g.MeasureString((-1 + i * 2).ToString(), f, 1, StringFormat.GenericTypographic);
                                    double string_width = size.Width - 2.0;
                                    string_height = size.Height - 2.0;

                                    //g.TextRenderingHint = TextRenderingHint.AntiAlias;
                                    //g.SmoothingMode = SmoothingMode.AntiAlias;
                                    g.DrawString((-1 + i * 2).ToString(), f, low_brush, (int)(i * spacing - string_width + (int)(i / 5)), (int)(H - 4 - 8 - string_height));
                                    //g.SmoothingMode = SmoothingMode.None;
                                }
                                spacing = ((double)W * 0.5 - 2.0 - 4.0) / 3.0;
                                for (int i = 1; i < 4; i++)
                                {
                                    g.FillRectangle(high_brush, (int)((double)W * 0.5 + i * spacing - spacing * 0.5), H - 4 - 3, 1, 3);
                                    g.FillRectangle(high_brush, (int)((double)W * 0.5 + i * spacing), H - 4 - 6, 2, 6);

                                    Font f = new Font("Arial", 7.0f, FontStyle.Bold);
                                    SizeF size = g.MeasureString("+" + (i * 20).ToString(), f, 3, StringFormat.GenericTypographic);
                                    double string_width = size.Width - 2.0;

                                    //g.TextRenderingHint = TextRenderingHint.SystemDefault;
                                    g.DrawString("+" + (i * 20).ToString(), f, high_brush, (int)(W * 0.5 + i * spacing - (int)string_width * 3 - i / 3 * 2), (int)(H - 4 - 8 - string_height));
                                }

                                if (num > -73.0) // high area
                                {
                                    pixel_x = (int)(W * 0.5 + 1 + (73.0 + num) / 63.0 * (W * 0.5 - 3));
                                }
                                else
                                {
                                    pixel_x = (int)((num + 127.0) / 54.0 * (W * 0.5));
                                }
                                break;
                            case MeterRXMode.ADC_L:
                            case MeterRXMode.ADC_R:
                                spacing = ((double)W - 5.0) / 6.0;
                                g.FillRectangle(low_brush, 0, H - 4, (int)(W - 3.0 - spacing), 2);
                                g.FillRectangle(high_brush, (int)(W - 3.0 - spacing), H - 4, (int)spacing, 2);
                                for (int i = 1; i < 7; i++)
                                {
                                    SolidBrush b = low_brush;
                                    if (i == 6) b = high_brush;
                                    g.FillRectangle(b, (int)(i * spacing - spacing / 2), H - 4 - 3, 1, 5);
                                    g.FillRectangle(b, (int)(i * spacing), H - 4 - 6, 2, 8);

                                    Font f = new Font("Arial", 7.0f, FontStyle.Bold);
                                    string s = (-120 + i * 20).ToString();
                                    SizeF size = g.MeasureString(s, f, 1, StringFormat.GenericTypographic);
                                    double string_width = size.Width - 2.0;
                                    size = g.MeasureString("0", f, 1, StringFormat.GenericTypographic);
                                    string_height = size.Height - 2.0;

                                    g.DrawString(s, f, b, (int)(i * spacing - (int)string_width * (s.Length)), (int)(H - 4 - 8 - string_height));
                                }

                                pixel_x = (int)((num + 120.0) / 120.0 * (W - 5.0));
                                break;
                            case MeterRXMode.OFF:
                                break;
                        }
                    }
                    else
                    {
                        switch (current_meter_tx_mode)
                        {
                            case MeterTXMode.MIC:
                            case MeterTXMode.EQ:
                            case MeterTXMode.LEVELER:
                            case MeterTXMode.COMP:
                            case MeterTXMode.CPDR:
                            case MeterTXMode.ALC:
                                g.FillRectangle(low_brush, 0, H - 4, (int)(W * 0.665), 2);
                                g.FillRectangle(high_brush, (int)(W * 0.665), H - 4, (int)(W * 0.335) - 2, 2);
                                double spacing = (W * 0.665 - 2.0) / 3.0;
                                double string_height = 0;
                                for (int i = 1; i < 4; i++)
                                {
                                    g.FillRectangle(low_brush, (int)(i * spacing - spacing * 0.5), H - 4 - 3, 1, 3);
                                    g.FillRectangle(low_brush, (int)(i * spacing), H - 4 - 6, 2, 6);

                                    string s = (-30 + i * 10).ToString();
                                    Font f = new Font("Arial", 7.0f, FontStyle.Bold);
                                    SizeF size = g.MeasureString("0", f, 1, StringFormat.GenericTypographic);
                                    double string_width = size.Width - 2.0;
                                    string_height = size.Height - 2.0;

                                    //g.TextRenderingHint = TextRenderingHint.AntiAlias;
                                    //g.SmoothingMode = SmoothingMode.AntiAlias;
                                    g.DrawString(s, f, low_brush, (int)(i * spacing - string_width * s.Length + 1.0 - (int)(i / 2) + (int)(i / 3)), (int)(H - 4 - 8 - string_height));
                                    //g.SmoothingMode = SmoothingMode.None;
                                }
                                spacing = (W * 0.335 - 2.0 - 3.0) / 3.0;
                                for (int i = 1; i < 4; i++)
                                {
                                    g.FillRectangle(high_brush, (int)((double)W * 0.665 + i * spacing - spacing * 0.5), H - 4 - 3, 1, 3);
                                    g.FillRectangle(high_brush, (int)((double)W * 0.665 + i * spacing), H - 4 - 6, 2, 6);

                                    Font f = new Font("Arial", 7.0f, FontStyle.Bold);
                                    SizeF size = g.MeasureString(i.ToString(), f, 3, StringFormat.GenericTypographic);
                                    double string_width = size.Width - 2.0;

                                    g.TextRenderingHint = TextRenderingHint.SystemDefault;
                                    g.DrawString(i.ToString(), f, high_brush, (int)(W * 0.665 + i * spacing - (int)string_width), (int)(H - 4 - 8 - string_height));
                                }

                                if (num > 0.0) // high area
                                {
                                    pixel_x = (int)(W * 0.665 + num / 3.0 * (W * 0.335 - 4));
                                }
                                else
                                {
                                    pixel_x = (int)((num + 30.0) / 30.0 * (W * 0.665 - 1.0));
                                }
                                break;
                            case MeterTXMode.FORWARD_POWER:
                            case MeterTXMode.REVERSE_POWER:
/*                                if (pa_present)
                                {
                                    g.FillRectangle(low_brush, 0, H - 4, (int)(W * 0.75), 2);
                                    g.FillRectangle(high_brush, (int)(W * 0.75), H - 4, (int)(W * 0.25) - 10, 2);
                                    spacing = (W * 0.75 - 2.0) / 4.0;
                                    string_height = 0;
                                    string[] list = { "5", "10", "50", "100" };
                                    for (int i = 1; i < 5; i++)
                                    {
                                        g.FillRectangle(low_brush, (int)(i * spacing - spacing * 0.5), H - 4 - 3, 1, 3);
                                        g.FillRectangle(low_brush, (int)(i * spacing), H - 4 - 6, 2, 6);

                                        string s = list[i - 1];
                                        Font f = new Font("Arial", 7.0f, FontStyle.Bold);
                                        SizeF size = g.MeasureString("0", f, 1, StringFormat.GenericTypographic);
                                        double string_width = size.Width - 2.0;
                                        string_height = size.Height - 2.0;

                                        //g.TextRenderingHint = TextRenderingHint.AntiAlias;
                                        //g.SmoothingMode = SmoothingMode.AntiAlias;
                                        g.DrawString(s, f, low_brush, (int)(i * spacing - string_width * s.Length + (int)(i / 3) + (int)(i / 4)), (int)(H - 4 - 8 - string_height));
                                        //g.SmoothingMode = SmoothingMode.None;
                                    }
                                    spacing = (W * 0.25 - 2.0 - 10.0) / 1.0;
                                    for (int i = 1; i < 2; i++)
                                    {
                                        g.FillRectangle(high_brush, (int)((double)W * 0.75 + i * spacing - spacing * 0.5), H - 4 - 3, 1, 3);
                                        g.FillRectangle(high_brush, (int)((double)W * 0.75 + i * spacing), H - 4 - 6, 2, 6);

                                        Font f = new Font("Arial", 7.0f, FontStyle.Bold);
                                        SizeF size = g.MeasureString("0", f, 3, StringFormat.GenericTypographic);
                                        double string_width = size.Width - 2.0;

                                        g.TextRenderingHint = TextRenderingHint.SystemDefault;
                                        g.DrawString("120+", f, high_brush, (int)(W * 0.75 + i * spacing - (int)3.5 * string_width), (int)(H - 4 - 8 - string_height));
                                    }

                                    if (num <= 100.0) // low area
                                    {
                                        spacing = (W * 0.75 - 2.0) / 4.0;
                                        if (num <= 5.0)
                                            pixel_x = (int)(num / 5.0 * (int)spacing);
                                        else if (num <= 10.0)
                                            pixel_x = (int)(spacing + (num - 5.0) / 5.0 * spacing);
                                        else if (num <= 50.0)
                                            pixel_x = (int)(2 * spacing + (num - 10.0) / 40.0 * spacing);
                                        else
                                            pixel_x = (int)(3 * spacing + (num - 50.0) / 50.0 * spacing);
                                    }
                                    else
                                    {
                                        spacing = (W * 0.25 - 2.0 - 10.0) / 1.0;
                                        if (num <= 120.0)
                                            pixel_x = (int)(W * 0.75 + (num - 100.0) / 20.0 * spacing);
                                        else
                                            pixel_x = (int)(W * 0.75 + spacing + (num - 120.0) / 60.0 * spacing);
                                    }
                                }
                                else // 1W version
                                {*/
                                    g.FillRectangle(low_brush, 0, H - 4, (int)(W * 0.75), 2);
                                    g.FillRectangle(high_brush, (int)(W * 0.75), H - 4, (int)(W * 0.25) - 9, 2);
                                    spacing = (W * 0.75 - 2.0) / 4.0;
                                    string_height = 0;
                                    string[] list = { "100", "250", "500", "800", "1000" };
                                    for (int i = 1; i < 5; i++)
                                    {
                                        g.FillRectangle(low_brush, (int)(i * spacing - spacing * 0.5), H - 4 - 3, 1, 3);
                                        g.FillRectangle(low_brush, (int)(i * spacing), H - 4 - 6, 2, 6);

                                        string s = list[i - 1];
                                        Font f = new Font("Arial", 7.0f, FontStyle.Bold);
                                        SizeF size = g.MeasureString("0", f, 1, StringFormat.GenericTypographic);
                                        double string_width = size.Width - 2.0;
                                        string_height = size.Height - 2.0;

                                        //g.TextRenderingHint = TextRenderingHint.AntiAlias;
                                        //g.SmoothingMode = SmoothingMode.AntiAlias;
                                        g.DrawString(s, f, low_brush, (int)(i * spacing - string_width * s.Length + 1.0 + (int)(i / 2) - (int)(i / 4)), (int)(H - 4 - 8 - string_height));
                                        //g.SmoothingMode = SmoothingMode.None;
                                    }
                                    spacing = (W * 0.25 - 2.0 - 9.0) / 1.0;
                                    for (int i = 1; i < 2; i++)
                                    {
                                        g.FillRectangle(high_brush, (int)((double)W * 0.75 + i * spacing - spacing * 0.5), H - 4 - 3, 1, 3);
                                        g.FillRectangle(high_brush, (int)((double)W * 0.75 + i * spacing), H - 4 - 6, 2, 6);

                                        Font f = new Font("Arial", 7.0f, FontStyle.Bold);
                                        SizeF size = g.MeasureString("0", f, 3, StringFormat.GenericTypographic);
                                        double string_width = size.Width - 2.0;

                                        //g.TextRenderingHint = TextRenderingHint.SystemDefault;
                                        g.DrawString("1000", f, high_brush, (int)(W * 0.75 + 2 + i * spacing - (int)4.0 * string_width), (int)(H - 4 - 8 - string_height));
                                    }

                                    num *= 1000;
                                    if (num < 801.0) // low area
                                    {
                                        spacing = (W * 0.75 - 2.0) / 4.0;
                                        if (num <= 100.0)
                                            pixel_x = (int)(num / 100.0 * spacing);
                                        else if (num <= 250.0)
                                            pixel_x = (int)(spacing + (num - 100.0) / 150.0 * spacing);
                                        else if (num <= 500.0)
                                            pixel_x = (int)(2 * spacing + (num - 250.0) / 250.0 * spacing);
                                        else
                                            pixel_x = (int)(3 * spacing + (num - 500.0) / 300.0 * spacing);
                                    }
                                    else
                                    {
                                        spacing = (W * 0.25 - 2.0 - 9.0) / 1.0;
                                        pixel_x = (int)(W * 0.75 + (num - 800.0) / 200.0 * spacing);
                                    }
//                                }
                                break;
                            case MeterTXMode.SWR:
                                g.FillRectangle(low_brush, 0, H - 4, (int)(W * 0.75), 2);
                                g.FillRectangle(high_brush, (int)(W * 0.75), H - 4, (int)(W * 0.25) - 9, 2);
                                spacing = (W * 0.75 - 2.0) / 4.0;
                                string_height = 0;
                                string[] swr_list = { "1.5", "2", "5", "10", "20" };
                                for (int i = 1; i < 5; i++)
                                {
                                    g.FillRectangle(low_brush, (int)(i * spacing - spacing * 0.5), H - 4 - 3, 1, 3);
                                    g.FillRectangle(low_brush, (int)(i * spacing), H - 4 - 6, 2, 6);

                                    string s = swr_list[i - 1];
                                    Font f = new Font("Arial", 7.0f, FontStyle.Bold);
                                    SizeF size = g.MeasureString("0", f, 1, StringFormat.GenericTypographic);
                                    double string_width = size.Width - 2.0;
                                    string_height = size.Height - 2.0;

                                    //g.TextRenderingHint = TextRenderingHint.AntiAlias;
                                    //g.SmoothingMode = SmoothingMode.AntiAlias;
                                    g.DrawString(s, f, low_brush, (int)(i * spacing - string_width * s.Length + 2.0 - 2 * (int)(i / 2) + 3 * (int)(i / 4)), (int)(H - 4 - 8 - string_height));
                                    //g.SmoothingMode = SmoothingMode.None;
                                }
                                spacing = (W * 0.25 - 2.0 - 9.0) / 1.0;
                                for (int i = 1; i < 2; i++)
                                {
                                    g.FillRectangle(high_brush, (int)((double)W * 0.75 + i * spacing - spacing * 0.5), H - 4 - 3, 1, 3);
                                    g.FillRectangle(high_brush, (int)((double)W * 0.75 + i * spacing), H - 4 - 6, 2, 6);

                                    Font f = new Font("Arial", 7.0f, FontStyle.Bold);
                                    SizeF size = g.MeasureString("0", f, 3, StringFormat.GenericTypographic);
                                    double string_width = size.Width - 2.0;

                                    //g.TextRenderingHint = TextRenderingHint.SystemDefault;
                                    g.DrawString("20+", f, high_brush, (int)(W * 0.75 + i * spacing - (int)2.5 * string_width), (int)(H - 4 - 8 - string_height));
                                }

                                if (num < 10.0) // low area
                                {
                                    spacing = (W * 0.75 - 2.0) / 4.0;
                                    if (num <= 1.5)
                                        pixel_x = (int)((num - 1.0) / 0.5 * spacing);
                                    else if (num <= 2.0)
                                        pixel_x = (int)(spacing + (num - 1.5) / 0.5 * spacing);
                                    else if (num <= 5.0)
                                        pixel_x = (int)(2 * spacing + (num - 2.0) / 3.0 * spacing);
                                    else
                                        pixel_x = (int)(3 * spacing + (num - 5.0) / 5.0 * spacing);
                                }
                                else
                                {
                                    spacing = (W * 0.25 - 2.0 - 9.0) / 1.0;
                                    pixel_x = (int)(W * 0.75 + (num - 10.0) / 10.0 * spacing);
                                }
                                if (double.IsInfinity(num)) pixel_x = W - 2;
                                break;
                            case MeterTXMode.ALC_G:
                            case MeterTXMode.LVL_G:
                                g.FillRectangle(low_brush, 0, H - 4, (int)(W * 0.75), 2);
                                g.FillRectangle(high_brush, (int)(W * 0.75), H - 4, (int)(W * 0.25) - 9, 2);
                                spacing = (W * 0.75 - 2.0) / 4.0;
                                string_height = 0;
                                string[] gain_list = { "5", "10", "15", "20", "25" };
                                for (int i = 1; i < 5; i++)
                                {
                                    g.FillRectangle(low_brush, (int)(i * spacing - spacing * 0.5), H - 4 - 3, 1, 3);
                                    g.FillRectangle(low_brush, (int)(i * spacing), H - 4 - 6, 2, 6);

                                    string s = gain_list[i - 1];
                                    Font f = new Font("Arial", 7.0f, FontStyle.Bold);
                                    SizeF size = g.MeasureString("0", f, 1, StringFormat.GenericTypographic);
                                    double string_width = size.Width - 2.0;
                                    string_height = size.Height - 2.0;

                                    //g.TextRenderingHint = TextRenderingHint.AntiAlias;
                                    //g.SmoothingMode = SmoothingMode.AntiAlias;
                                    g.DrawString(s, f, low_brush, (int)(i * spacing - string_width * s.Length + (int)(i / 3)), (int)(H - 4 - 8 - string_height));
                                    //g.SmoothingMode = SmoothingMode.None;
                                }
                                spacing = (W * 0.25 - 2.0 - 9.0) / 1.0;
                                for (int i = 1; i < 2; i++)
                                {
                                    g.FillRectangle(high_brush, (int)((double)W * 0.75 + i * spacing - spacing * 0.5), H - 4 - 3, 1, 3);
                                    g.FillRectangle(high_brush, (int)((double)W * 0.75 + i * spacing), H - 4 - 6, 2, 6);

                                    Font f = new Font("Arial", 7.0f, FontStyle.Bold);
                                    SizeF size = g.MeasureString("0", f, 3, StringFormat.GenericTypographic);
                                    double string_width = size.Width - 2.0;

                                    //g.TextRenderingHint = TextRenderingHint.SystemDefault;
                                    g.DrawString("25+", f, high_brush, (int)(W * 0.75 + i * spacing - (int)2.5 * string_width), (int)(H - 4 - 8 - string_height));
                                }

                                spacing = (W * 0.75 - 2.0) / 4.0;
                                pixel_x = (int)(num / 5.0 * spacing);
                                break;
                            case MeterTXMode.OFF:
                                break;
                        }
                    }

                    if ((!chkMOX.Checked && current_meter_rx_mode != MeterRXMode.OFF) ||
                        (chkMOX.Checked && current_meter_tx_mode != MeterTXMode.OFF))
                    {
                        pixel_x = Math.Max(0, pixel_x);
                        pixel_x = Math.Min(W, pixel_x);

                        Pen line_pen = new Pen(edge_avg_color);
                        Pen line_dark_pen = new Pen(
                            Color.FromArgb((edge_avg_color.R + edge_meter_background_color.R) / 2,
                            (edge_avg_color.G + edge_meter_background_color.G) / 2,
                            (edge_avg_color.B + edge_meter_background_color.B) / 2));

                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.DrawLine(line_dark_pen, pixel_x - 1, 0, pixel_x - 1, H);
                        g.DrawLine(line_pen, pixel_x, 0, pixel_x, H);
                        g.DrawLine(line_dark_pen, pixel_x + 1, 0, pixel_x + 1, H);
                        g.InterpolationMode = InterpolationMode.Default;
                        g.SmoothingMode = SmoothingMode.Default;
                    }

                    if (meter_data_ready)
                    {
                        meter_data_ready = false;  //We do NOT want to do this before we have consumed it!!!! so do it here.
                    }
                    break;
                    #endregion
                case MultiMeterDisplayMode.Analog:
                    #region Analog

                    #endregion
                    break;
            }
        }

        private void ResetMultiMeterPeak()
        {
            meter_peak_count = multimeter_peak_hold_samples;
            avg_num = -200.0f;
        }

        private void panelLOSCHover_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (losc_hover_digit < 0)
                return;

            int x = 0;
            int width = 0;

            if (small_lsd && txtLOSCLSD.Visible)
            {
                x += (losc_char_width + losc_char_space) * losc_hover_digit;
                if (losc_hover_digit > 3)
                    x += (losc_decimal_space - losc_char_space);

                if (losc_hover_digit > 6)
                {
                    x += losc_small_char_width;
                    x += (losc_small_char_width + losc_small_char_space - losc_char_width - losc_char_space) * (losc_hover_digit - 6);
                    width = x + losc_small_char_width;
                }
                else width = x + losc_char_width;
            }
            else
            {
                x += (losc_char_width + losc_char_space) * losc_hover_digit;
                if (losc_hover_digit > 3)
                    x += (losc_decimal_space - losc_char_space);
                width = x + losc_char_width;
            }

            e.Graphics.DrawLine(new Pen(txtLOSCFreq.ForeColor, 2.0f), x, 1, width, 1);
        }

        private void panelVFOAHover_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (vfoa_hover_digit < 0)
                return;

            int x = 0;
            int width = 0;

            if (small_lsd && txtVFOALSD.Visible)
            {
                x += (vfo_char_width + vfo_char_space) * vfoa_hover_digit;
                if (vfoa_hover_digit > 3)
                    x += (vfo_decimal_space - vfo_char_space);

                if (vfoa_hover_digit > 6)
                {
                    x += vfo_small_char_width;
                    x += (vfo_small_char_width + vfo_small_char_space - vfo_char_width - vfo_char_space) * (vfoa_hover_digit - 6);
                    width = x + vfo_small_char_width;
                }
                else width = x + vfo_char_width;
            }
            else
            {
                x += (vfo_char_width + vfo_char_space) * vfoa_hover_digit;
                if (vfoa_hover_digit > 3)
                    x += (vfo_decimal_space - vfo_char_space);
                width = x + vfo_char_width;
            }

            e.Graphics.DrawLine(new Pen(txtVFOAFreq.ForeColor, 2.0f), x, 1, width, 1);
        }

        private void panelVFOBHover_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (vfob_hover_digit < 0)
                return;

            int x = 0;
            int width = 0;

            if (small_lsd && txtVFOBLSD.Visible)
            {
                x += (vfo_char_width + vfo_char_space) * vfob_hover_digit;
                if (vfob_hover_digit > 3)
                    x += (vfo_decimal_space - vfo_char_space);

                if (vfob_hover_digit > 6)
                {
                    x += vfo_small_char_width;
                    x += (vfo_small_char_width + vfo_small_char_space - vfo_char_width - vfo_char_space) * (vfob_hover_digit - 6);
                    width = x + vfo_small_char_width;
                }
                else width = x + vfo_char_width;
            }
            else
            {
                x += (vfo_char_width + vfo_char_space) * vfob_hover_digit;
                if (vfob_hover_digit > 3)
                    x += (vfo_decimal_space - vfo_char_space);
                width = x + vfo_char_width;
            }

            e.Graphics.DrawLine(new Pen(txtVFOBFreq.ForeColor, 2.0f), x, 1, width, 1);
        }

        public void UpdateDisplayWaterfallAverage()
        {
            double dttsp_osc = (LOSCFreq - vfoAFreq) * 1e6;
            // Debug.WriteLine("last vfo: " + avg_last_ddsfreq + " vfo: " + DDSFreq); 
            if (Display.average_waterfall_buffer[0] == Display.CLEAR_FLAG)
            {
                //Debug.WriteLine("Clearing average buf"); 
                for (int i = 0; i < Display.BUFFER_SIZE; i++)
                    Display.average_waterfall_buffer[i] = Display.current_waterfall_data[i];
            }
            else
            {
                // wjt added -- stop hosing the avg display when scrolling the vfo 
 /*               if ((avg_last_ddsfreq != 0 && avg_last_ddsfreq != LOSCFreq) ||
                    (avg_last_dttsp_osc != dttsp_osc))   // vfo has changed, need to shift things around 
                {
                    //Debug.WriteLine("dttsp_osc: " + dttsp_osc); 
                    double delta_vfo;
                    if (current_model != Model.DR3X)
                    {
                        delta_vfo = LOSCFreq - avg_last_ddsfreq;
                        delta_vfo *= 1e6; // vfo in mhz moron!
                    }
                    else
                    {
                        delta_vfo = dttsp_osc - avg_last_dttsp_osc;
                        delta_vfo = -delta_vfo;
                        //Debug.WriteLine("update from dttsp delta_vfo: " + delta_vfo); 
                    }
                    double hz_per_bin = DttSP.SampleRate / Display.BUFFER_SIZE;

                    int bucket_shift = (int)(delta_vfo / hz_per_bin);
                    double leftover = delta_vfo - ((double)bucket_shift * hz_per_bin);
                    leftover = leftover / hz_per_bin; // conver to fractions of bucket 
                    double total_leftover = leftover + last_bin_shift_leftover;
                    if (total_leftover < -0.5)
                    {
                        bucket_shift -= 1;
                        total_leftover += 1;
                        //Debug.WriteLine("bump down"); 
                    }
                    else if (total_leftover > 0.5)
                    {
                        bucket_shift += 1;
                        total_leftover -= 1;
                        //Debug.WriteLine("bump up"); 
                    }
                    last_bin_shift_leftover = total_leftover;
                    //Debug.WriteLine("leftover: " + leftover + " total_leftover: " + total_leftover); 

                    // bucket_shift = bucket_shift/2; 						
                    // indexed_value pre_max = findMax(average_buffer, display_buffer_size); 
                    // Debug.WriteLine("\nPre max: " + pre_max.val + " " + pre_max.idx); 
                    // Debug.WriteLine("bshift: " + bucket_shift + " delta_vfo: " + delta_vfo); 
                    if (bucket_shift > 0) // vfo increased, need to shift avgs to the left 
                    {
                        if (bucket_shift >= Display.BUFFER_SIZE)
                        {
                            Display.average_waterfall_buffer[0] = Display.CLEAR_FLAG;
                        }
                        else
                        {
                            for (int j = 0; j < Display.BUFFER_SIZE - bucket_shift; j++)
                                Display.average_waterfall_buffer[j] = Display.average_waterfall_buffer[j + bucket_shift];  // wjt fix use memmove 

                            // fill avg with last good data on the end
                            for (int j = Display.BUFFER_SIZE - bucket_shift; j < Display.BUFFER_SIZE; j++)
                                Display.average_waterfall_buffer[j] = Display.average_waterfall_buffer[Display.BUFFER_SIZE - bucket_shift - 1];
                        }
                    }
                    else if (bucket_shift < 0) // vfo decreased, move samples up 
                    {
                        if (-bucket_shift >= Display.BUFFER_SIZE)
                        {
                            Display.average_waterfall_buffer[0] = Display.CLEAR_FLAG;
                        }
                        else
                        {
                            for (int j = Display.BUFFER_SIZE - 1; j > -bucket_shift; j--)
                                Display.average_waterfall_buffer[j] = Display.average_waterfall_buffer[j + bucket_shift];

                            for (int j = 0; j < -bucket_shift; j++)
                                Display.average_waterfall_buffer[j] = Display.average_waterfall_buffer[-bucket_shift];
                        }
                    }
                    //					indexed_value post_max = findMax(average_buffer, display_buffer_size); 
                    //					Debug.WriteLine("Post max: " + post_max.val + " " + post_max.idx); 
                    //					indexed_value disp_max = findMax(display_data, display_buffer_size); 		
                    //					Debug.WriteLine("Disp max: " + disp_max.val + " " + disp_max.idx); 
                }
                else
                {
                    last_bin_shift_leftover = 0; // reset, this vfo = last vfo 
                }*/
            }

                float new_mult = 0.0f;
                float old_mult = 0.0f;

                new_mult = Display.waterfall_avg_mult_new;
                old_mult = Display.waterfall_avg_mult_old;

                for (int i = 0; i < Display.BUFFER_SIZE; i++)
                    Display.average_waterfall_buffer[i] = Display.current_waterfall_data[i] =
                        (float)(Display.current_waterfall_data[i] * new_mult +
                        Display.average_waterfall_buffer[i] * old_mult);

            if (Display.average_waterfall_buffer[0] == Display.CLEAR_FLAG)
            {
                avg_last_ddsfreq = 0;
                avg_last_dttsp_osc = 0;
            }
            else
            {
                avg_last_ddsfreq = loscFreq;
                avg_last_dttsp_osc = dttsp_osc;
            }
        }

        public void UpdateDisplayPanadapterAverage()
        {
            double dttsp_osc = (LOSCFreq - vfoAFreq) * 1e6;
            // Debug.WriteLine("last vfo: " + avg_last_ddsfreq + " vfo: " + DDSFreq); 
            if (Display.average_buffer[0] == Display.CLEAR_FLAG)
            {
                //Debug.WriteLine("Clearing average buf"); 
                for (int i = 0; i < Display.BUFFER_SIZE; i++)
                    Display.average_buffer[i] = Display.current_display_data[i];
            }
            else
            {
                // wjt added -- stop hosing the avg display when scrolling the vfo 
/*               if ((avg_last_ddsfreq != 0 && avg_last_ddsfreq != LOSCFreq) ||                   
                    (avg_last_dttsp_osc != dttsp_osc))   // vfo has changed, need to shift things around 
                {
                    //Debug.WriteLine("dttsp_osc: " + dttsp_osc); 
                    double delta_vfo;
                    if (current_model != Model.DR3X)
                    {
                        delta_vfo = LOSCFreq - avg_last_ddsfreq;
                        delta_vfo *= 1e6; // vfo in mhz moron!
                    }
                    else
                    {
                        delta_vfo = dttsp_osc - avg_last_dttsp_osc;
                        delta_vfo = -delta_vfo;
                        //Debug.WriteLine("update from dttsp delta_vfo: " + delta_vfo); 
                    }
                    double hz_per_bin = DttSP.SampleRate / Display.BUFFER_SIZE;

                    int bucket_shift = (int)(delta_vfo / hz_per_bin);
                    double leftover = delta_vfo - ((double)bucket_shift * hz_per_bin);
                    leftover = leftover / hz_per_bin; // conver to fractions of bucket 
                    double total_leftover = leftover + last_bin_shift_leftover;
                    if (total_leftover < -0.5)
                    {
                        bucket_shift -= 1;
                        total_leftover += 1;
                        //Debug.WriteLine("bump down"); 
                    }
                    else if (total_leftover > 0.5)
                    {
                        bucket_shift += 1;
                        total_leftover -= 1;
                        //Debug.WriteLine("bump up"); 
                    }
                    last_bin_shift_leftover = total_leftover;
                    //Debug.WriteLine("leftover: " + leftover + " total_leftover: " + total_leftover); 

                    // bucket_shift = bucket_shift/2; 						
                    // indexed_value pre_max = findMax(average_buffer, display_buffer_size); 
                    // Debug.WriteLine("\nPre max: " + pre_max.val + " " + pre_max.idx); 
                    // Debug.WriteLine("bshift: " + bucket_shift + " delta_vfo: " + delta_vfo); 
                    if (bucket_shift > 0) // vfo increased, need to shift avgs to the left 
                    {
                        if (bucket_shift >= Display.BUFFER_SIZE)
                        {
                            Display.average_buffer[0] = Display.CLEAR_FLAG;
                        }
                        else
                        {
                            for (int j = 0; j < Display.BUFFER_SIZE - bucket_shift; j++)
                                Display.average_buffer[j] = Display.average_buffer[j + bucket_shift];  // wjt fix use memmove 

                            // fill avg with last good data on the end
                            for (int j = Display.BUFFER_SIZE - bucket_shift; j < Display.BUFFER_SIZE; j++)
                                Display.average_buffer[j] = Display.average_buffer[Display.BUFFER_SIZE - bucket_shift - 1];
                        }
                    }
                    else if (bucket_shift < 0) // vfo decreased, move samples up 
                    {
                        if (-bucket_shift >= Display.BUFFER_SIZE)
                        {
                            Display.average_buffer[0] = Display.CLEAR_FLAG;
                        }
                        else
                        {
                            for (int j = Display.BUFFER_SIZE - 1; j > -bucket_shift; j--)
                                Display.average_buffer[j] = Display.average_buffer[j + bucket_shift];

                            for (int j = 0; j < -bucket_shift; j++)
                                Display.average_buffer[j] = Display.average_buffer[-bucket_shift];
                        }
                    }
                    //					indexed_value post_max = findMax(average_buffer, display_buffer_size); 
                    //					Debug.WriteLine("Post max: " + post_max.val + " " + post_max.idx); 
                    //					indexed_value disp_max = findMax(display_data, display_buffer_size); 		
                    //					Debug.WriteLine("Disp max: " + disp_max.val + " " + disp_max.idx); 
                }
/*                else
                {
                    last_bin_shift_leftover = 0; // reset, this vfo = last vfo 
                }
*/
                float new_mult = 0.0f;
                float old_mult = 0.0f;

                new_mult = Display.display_avg_mult_new;
                old_mult = Display.display_avg_mult_old;


                for (int i = 0; i < Display.BUFFER_SIZE; i++)
                    Display.average_buffer[i] = Display.current_display_data[i] =
                        (float)(Display.current_display_data[i] * new_mult +
                        Display.average_buffer[i] * old_mult);
            }

            if (Display.average_buffer[0] == Display.CLEAR_FLAG)
            {
                avg_last_ddsfreq = 0;
                avg_last_dttsp_osc = 0;
            }
            else
            {
                avg_last_ddsfreq = loscFreq;
                avg_last_dttsp_osc = dttsp_osc;
            }
        }

        #endregion

        #region Thread and Timer Routines
        // ======================================================
        // Thread Routines
        // ======================================================

        private void RunDisplay()
        {
            while (chkPower.Checked)
            {
//                if (tx_rx && (CurrentDSPMode == DSPMode.CWL || CurrentDSPMode == DSPMode.CWU))
                if(tx_rx)
                {
                    Thread.Sleep(300);
                }                        
                tx_rx = false;

                if (!Display.DataReady || !Display.WaterfallDataReady)
                {
                    switch (Display.CurrentDisplayMode)
                    {
                        case (DisplayMode.PANADAPTER):
                        case (DisplayMode.HISTOGRAM):
                            {
                                fixed (float* ptr = &Display.new_display_data[0])
                                    DttSP.GetPanadapter(ptr);
                            }
                            break;

                        case (DisplayMode.WATERFALL):
                        case(DisplayMode.PANAFALL):
                            {
                                fixed (float* ptr = &Display.new_display_data[0])
                                    DttSP.GetPanadapter(ptr);

                                fixed (float* ptr1 = &Display.new_waterfall_data[0])
                                    DttSP.GetPanadapter(ptr1);
                                Display.WaterfallDataReady = true;
                            }
                            break;
                        case (DisplayMode.PHASE2):
                            {
                                for (int i = 0; i < Display.PhaseNumPts; i++)
                                {
                                    Display.new_display_data[i * 2] = Audio.phase_buf_l[i];
                                    Display.new_display_data[i * 2 + 1] = Audio.phase_buf_r[i];
                                }
                            }
                            break;
                        case(DisplayMode.PHASE):
                            {
                                fixed (float* ptr = &Display.new_display_data[0])
                                    DttSP.GetPhase(ptr, Display.PhaseNumPts);
                            }
                            break;
                        case (DisplayMode.SCOPE):
                            {
                                fixed (float* ptr = &Display.new_display_data[0])
                                    DttSP.GetScope(ptr, (int)(scope_time * 48));
                            }
                            break;
                        case (DisplayMode.SPECTRUM):
                            {
                                fixed (float* ptr = &Display.new_display_data[0])
                                    DttSP.GetSpectrum(ptr);
                            }
                            break;
                        default:
                            fixed (float* ptr = &Display.new_display_data[0])
                                DttSP.GetPanadapter(ptr);
                            break;
                    }
                    Display.DataReady = true;
                }

                UpdateDisplay();

                if (chkPower.Checked)
                    Thread.Sleep(display_delay);
            }
        }

        private float multimeter_avg = Display.CLEAR_FLAG;
        private void UpdateMultimeter()
        {
            while (chkPower.Checked)
            {
                DttSP.SetRXListen(1);
                string output = "";
                if (!meter_data_ready)
                {
                    if (!chkMOX.Checked)
                    {
                        if (Audio.CurrentAudioState1 != Audio.AudioState.DTTSP)
                            goto end;

                        MeterRXMode mode = CurrentMeterRXMode;
                        float num = 0f;
                        switch (mode)
                        {
                            case MeterRXMode.SIGNAL_STRENGTH:
                                num = DttSP.CalculateMeter(DttSP.MeterType.SIGNAL_STRENGTH);
                                num = num +
                                    multimeter_cal_offset +
                                    preamp_offset[(int)current_preamp_mode] +
                                    filter_size_cal_offset;
                                output = num.ToString("f1") + " dBm";
                                new_meter_data = num;
                                break;
                            case MeterRXMode.SIGNAL_AVERAGE:
                                num = DttSP.CalculateMeter(DttSP.MeterType.SIGNAL_STRENGTH);
                                if (multimeter_avg == Display.CLEAR_FLAG) multimeter_avg = num;
                                num = multimeter_avg_mult_old * multimeter_avg + multimeter_avg_mult_new * num;
                                multimeter_avg = num;
                                num = num +
                                    multimeter_cal_offset +
                                    preamp_offset[(int)current_preamp_mode] +
                                    filter_size_cal_offset;
                                output = num.ToString("f1") + " dBm";
                                new_meter_data = num;
                                break;
                            case MeterRXMode.ADC_L:
                                num = DttSP.CalculateMeter(DttSP.MeterType.ADC_REAL);
                                output = num.ToString("f1") + " dBFS";
                                new_meter_data = num;
                                break;
                            case MeterRXMode.ADC_R:
                                num = DttSP.CalculateMeter(DttSP.MeterType.ADC_IMAG);
                                output = num.ToString("f1") + " dBFS";
                                new_meter_data = num;
                                break;
                            case MeterRXMode.OFF:
                                output = "";
                                new_meter_data = num;
                                break;
                        }
                    }
                    else
                    {
                        MeterTXMode mode = CurrentMeterTXMode;
                        float num = 0f;
                        double power = 0.0;

                        switch (mode)
                        {
                            case MeterTXMode.MIC:
                                if (Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
                                {
                                    num = (float)Math.Max(-30.0, -DttSP.CalculateMeter(DttSP.MeterType.MIC) + 3.0);
                                    output = num.ToString("f1") + " dB";
                                }
                                else output = "0" + separator + "0 dB";
                                new_meter_data = num;
                                break;
                            case MeterTXMode.EQ:
                                if (Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
                                {
                                    num = (float)Math.Max(-30.0, -DttSP.CalculateMeter(DttSP.MeterType.EQ) + 3.0);
                                    output = num.ToString("f1") + " dB";
                                }
                                else output = "0" + separator + "0 dB";
                                new_meter_data = num;
                                break;
                            case MeterTXMode.LEVELER:
                                if (Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
                                {
                                    num = (float)Math.Max(-30.0, -DttSP.CalculateMeter(DttSP.MeterType.LEVELER) + 3.0);
                                    output = num.ToString("f1") + " dB";
                                }
                                else output = "0" + separator + "0 dB";
                                new_meter_data = num;
                                break;
                            case MeterTXMode.LVL_G:
                                if (Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
                                {
                                    num = (float)Math.Max(0, DttSP.CalculateMeter(DttSP.MeterType.LVL_G));
                                    output = num.ToString("f1") + " dB";
                                }
                                else output = "0" + separator + "0 dB";
                                new_meter_data = num;
                                break;
                            case MeterTXMode.COMP:
                                if (Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
                                {
                                    num = (float)Math.Max(-30.0, -DttSP.CalculateMeter(DttSP.MeterType.COMP) + 3.0);
                                    output = num.ToString("f1") + " dB";
                                }
                                else output = "0" + separator + "0 dB";
                                new_meter_data = num;
                                break;
                            case MeterTXMode.CPDR:
                                if (Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
                                {
                                    num = (float)Math.Max(-30.0, -DttSP.CalculateMeter(DttSP.MeterType.CPDR) + 3.0);
                                    output = num.ToString("f1") + " dB";
                                }
                                else output = "0" + separator + "0 dB";
                                new_meter_data = num;
                                break;
                            case MeterTXMode.ALC:
                                if (Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
                                {
                                    num = (float)Math.Max(-30.0, -DttSP.CalculateMeter(DttSP.MeterType.ALC) + 3.0);
                                    output = num.ToString("f1") + " dB";
                                }
                                else output = "0" + separator + "0 dB";
                                new_meter_data = num;
                                break;
                            case MeterTXMode.ALC_G:
                                if (Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
                                {
                                    num = (float)Math.Max(0, -DttSP.CalculateMeter(DttSP.MeterType.ALC_G));
                                    output = num.ToString("f1") + " dB";
                                }
                                else output = "0" + separator + "0 dB";
                                new_meter_data = num;
                                break;
                            case MeterTXMode.FORWARD_POWER:
                                if (VFOAFreq < MaxFreq)
                                {
                                    power = PAPower(pa_fwd_power);
                                    output = power.ToString("f0") + " W";
                                    new_meter_data = (float)power;
                                }
                                else
                                {
                                    if (Audio.CurrentAudioState1 == Audio.AudioState.DTTSP)
                                    {
                                        num = (float)Math.Max(0.0, DttSP.CalculateMeter(DttSP.MeterType.PWR));
                                        num *= (float)((double)udPWR.Value * 0.01);
                                        output = (num * 1000).ToString("f0") + " mW";
                                    }
                                    else output = "0 mW";
                                    new_meter_data = num;
                                }
                                break;
                            case MeterTXMode.REVERSE_POWER:
                                power = PAPower(pa_rev_power);
                                output = power.ToString("f0") + " W";
                                new_meter_data = (float)power;
                                break;
                            case MeterTXMode.SWR:
                                double swr = 0.0;
                                if (chkTUN.Checked)
                                {
                                    swr = SWR(pa_fwd_power, pa_rev_power);
                                    output = swr.ToString("f1") + " : 1";
                                }
                                else
                                {
                                    output = "in TUN only ";
                                }
                                new_meter_data = (float)swr;
                                break;
                            case MeterTXMode.OFF:
                                output = "";
                                new_meter_data = num;
                                break;
                        }
                    }
                    meter_data_ready = true;
                    txtMultiText.Text = output;
                    picMultiMeterDigital.Invalidate();
                }
            end:
                if (chkPower.Checked)
                    Thread.Sleep(meter_delay);
            }
        }

        private float sql_data = -200.0f;
        private void UpdateSQL()
        {
            while (chkPower.Checked)
            {
                if (!chkMOX.Checked)
                {
                    float num = DttSP.CalculateMeter(DttSP.MeterType.SIGNAL_STRENGTH);
                    num = num +
                        multimeter_cal_offset +
                        preamp_offset[(int)current_preamp_mode] +
                        filter_size_cal_offset;

                    sql_data = num;
                    picSQL.Invalidate();
                }

                if (chkPower.Checked) Thread.Sleep(100);
            }
        }

        private float noise_gate_data = -200.0f;
        private void UpdateNoiseGate()
        {
            while (chkPower.Checked)
            {
                if (chkMOX.Checked)
                {
                    float num = -DttSP.CalculateMeter(DttSP.MeterType.MIC);

                    noise_gate_data = num + 3.0f;
                    picNoiseGate.Invalidate();
                }

                if (chkPower.Checked) Thread.Sleep(100);
            }
        }

        private void UpdateVOX()
        {
            while (chkPower.Checked)
            {
                switch (current_dsp_mode)
                {
                    case DSPMode.LSB:
                    case DSPMode.USB:
                    case DSPMode.DSB:
                    case DSPMode.AM:
                    case DSPMode.SAM:
                    case DSPMode.FMN:
                        picVOX.Invalidate();
                        break;
                }

                if (chkPower.Checked) Thread.Sleep(100);
            }
        }

        private bool mon_recall = false;
        private static HiPerfTimer vox_timer = new HiPerfTimer();
        private void PollPTT()
        {
            bool mic_ptt = false;
            bool cat_ptt = false;

            while (chkPower.Checked)
            {
                if (!manual_mox && !disable_ptt)
                {
                    switch (CurrentModel)
                    {
                        case (Model.GENESIS_G59):
                            {
                                mic_ptt = g59.MOX;
                            }
                            break;
                        default:
                            {
                                switch (CurrentDSPMode)
                                {

                                    case (DSPMode.CWL):
                                    case (DSPMode.CWU):
                                        {
                                            mic_ptt = false;
                                        }
                                        break;
                                    default:
                                        {
                                            mic_ptt = Keyer.isPTT();
                                        }
                                        break;
                                }
                            }
                            break;
                    }
                    bool cw_ptt = (CWSemiBreakInEnabled & (DttSP.KeyerPlaying() != 0)) | Keyer.KeyerPTT | Keyer.MemoryPTT;
                    bool vox_ptt = Audio.VOXActive;

                    if (PTTBitBangEnabled && serialPTT != null)
                        cat_ptt = serialPTT.isPTT();

                    if (cw_ptt) break_in_timer.Start();

                    if (!chkMOX.Checked)
                    {
                        if (cat_ptt)
                        {
                            current_ptt_mode = PTTMode.CAT;
                            if (current_dsp_mode == DSPMode.CWL ||
                                current_dsp_mode == DSPMode.CWU)
                            {
                                if (!cw_disable_monitor)
                                {
                                    mon_recall = chkMON.Checked;
                                    chkMON.Checked = true;
                                }
                                chkMOX.Checked = true;
                                if (!chkMOX.Checked)
                                {
                                    chkPower.Checked = false;
                                    return;
                                }
                            }
                            else
                            {
                                chkMOX.Checked = true;
                                if (!chkMOX.Checked)
                                {
                                    chkPower.Checked = false;
                                    return;
                                }
                            }
                        }

                        if ((current_dsp_mode == DSPMode.CWL ||
                            current_dsp_mode == DSPMode.CWU) &&
                            cw_ptt)
                        {
                            if (Keyer.PrimaryConnPort == "USB" &&
                                Keyer.SecondaryConnPort == "None" &&
                                !cw_semi_break_in_enabled)
                            {
                                // do nothing
                            }
                            else
                            {
                                current_ptt_mode = PTTMode.CW;
                                if (!cw_disable_monitor)
                                {
                                    mon_recall = chkMON.Checked;
                                    chkMON.Checked = true;
                                }
                                chkMOX.Checked = true;
                                if (!chkMOX.Checked)
                                {
                                    chkPower.Checked = false;
                                    return;
                                }
                            }
                        }

                        if ((current_dsp_mode == DSPMode.LSB ||
                            current_dsp_mode == DSPMode.USB ||
                            current_dsp_mode == DSPMode.DSB ||
                            current_dsp_mode == DSPMode.AM ||
                            current_dsp_mode == DSPMode.SAM ||
                            current_dsp_mode == DSPMode.DIGU ||
                            current_dsp_mode == DSPMode.DIGL ||
                            current_dsp_mode == DSPMode.FMN) && mic_ptt)
                        {
                            current_ptt_mode = PTTMode.MIC;
                            chkMOX.Checked = true;
                            if (!chkMOX.Checked)
                            {
                                chkPower.Checked = false;
                                return;
                            }
                        }

                        if ((current_dsp_mode == DSPMode.LSB ||
                            current_dsp_mode == DSPMode.USB ||
                            current_dsp_mode == DSPMode.DSB ||
                            current_dsp_mode == DSPMode.AM ||
                            current_dsp_mode == DSPMode.SAM ||
                            current_dsp_mode == DSPMode.DIGU ||
                            current_dsp_mode == DSPMode.DIGL ||
                            current_dsp_mode == DSPMode.FMN) &&
                            vox_ptt)
                        {
                            current_ptt_mode = PTTMode.VOX;
                            vox_timer.Start();
                            chkMOX.Checked = true;
                            if (!chkMOX.Checked)
                            {
                                chkPower.Checked = false;
                                return;
                            }
                        }
                    }
                    else // else if(chkMOX.Checked)
                    {
                        switch (current_ptt_mode)
                        {
                            case PTTMode.CAT:
                                if (!cat_ptt)
                                {
                                    chkMOX.Checked = false;
                                    if ((current_dsp_mode == DSPMode.CWL ||
                                        current_dsp_mode == DSPMode.CWU) &&
                                        !cw_disable_monitor)
                                        chkMON.Checked = mon_recall;
                                }
                                break;
                            case PTTMode.MIC:
                                if (!mic_ptt)
                                    chkMOX.Checked = false;
                                break;
                            case PTTMode.CW:
                                if (!cw_ptt)
                                {
                                    if (cw_semi_break_in_enabled)
                                    {
                                        break_in_timer.Stop();
                                        if (break_in_timer.DurationMsec > break_in_delay)
                                        {
                                            chkMOX.Checked = false;
                                            if ((current_dsp_mode == DSPMode.CWL ||
                                                current_dsp_mode == DSPMode.CWU) &&
                                                !cw_disable_monitor)
                                                chkMON.Checked = mon_recall;
                                        }
                                    }
                                    else
                                    {
                                        break_in_timer.Stop();
                                        if (break_in_timer.DurationMsec > 43)
                                        {
                                            chkMOX.Checked = false;
                                            if ((current_dsp_mode == DSPMode.CWL ||
                                                current_dsp_mode == DSPMode.CWU) &&
                                                !cw_disable_monitor)
                                                chkMON.Checked = mon_recall;
                                        }
                                    }
                                }
                                break;
                            case PTTMode.VOX:
                                if (!vox_ptt)
                                {
                                    vox_timer.Stop();
                                    if (vox_timer.DurationMsec > vox_hang_time)
                                        chkMOX.Checked = false;
                                }
                                else vox_timer.Start();
                                break;
                        }
                    }
                }

                Thread.Sleep(4);
            }
            //			poll_ptt_running = false;
        }

        private double SWRScale(double ref_pow)
        {
            if (ref_pow < 19) return 1.0;
            else return Math.Max((ref_pow * -0.01774) + 1.137097, 0.25); // mx+b found using 80% at 19, 25% at 50
        }

        private void timer_cpu_meter_Tick(object sender, System.EventArgs e)
        {
            lblCPUMeter.Text = "CPU %: " + CpuUsage.ToString("f1");
        }

        private void timer_peak_text_Tick(object sender, System.EventArgs e)
        {
            switch (Display.CurrentDisplayMode)
            {
                case DisplayMode.PANAFALL:
                case DisplayMode.PANADAPTER:
                case DisplayMode.WATERFALL:
                    UpdatePeakText();
                    break;
                default:
                    txtDisplayPeakOffset.Text = "";
                    txtDisplayPeakPower.Text = "";
                    txtDisplayPeakFreq.Text = "";
                    break;
            }
        }

        private int last_sec;		// for time of day clock
        private DateTime last_date;	// for date
        private void timer_clock_Tick(object sender, System.EventArgs e)
        {

/*            switch (current_datetime_mode)
            {
                case DateTimeMode.LOCAL:
                    DateTime date = DateTime.Now.Date;
                    if (date != last_date || txtDate.Text == "")
                    {
                        last_date = date;
                        txtDate.Text = DateTime.Now.ToShortDateString();
                    }

                    int sec = DateTime.Now.Second;
                    if (sec != last_sec)
                    {
                        last_sec = sec;
                        txtTime.Text = "LOC " + DateTime.Now.ToString("HH:mm:ss");
                    }
                    break;
                case DateTimeMode.UTC:
                    date = DateTime.UtcNow.Date;
                    if (date != last_date || txtDate.Text == "")
                    {
                        last_date = date;
                        txtDate.Text = DateTime.UtcNow.ToShortDateString();
                    }

                    sec = DateTime.UtcNow.Second;
                    if (sec != last_sec)
                    {
                        last_sec = sec;
                        txtTime.Text = "UTC " + DateTime.UtcNow.ToString("HH:mm:ss");
                    }
                    break;
                case DateTimeMode.OFF:
                    txtDate.Text = "";
                    txtTime.Text = "";
                    break;
            }*/
        }

        private void DelayedDisplayReset()
        {
            Thread.Sleep((int)((double)block_size1 / (double)sample_rate1 * 1000.0));
            Display.ResetDisplayAverage();
        }

        #endregion

        #region Event Handlers
        // ======================================================
        // Event Handlers
        // ======================================================

        // Console Events

        private void Console_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                btnHidden.Focus();
        }

        private void Console_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) // changes yt7pwr
        {
            if (e.Shift == false)
                shift_down = false;

            if (e.KeyCode == key_cw_dot)
            {
                g59.KEYER = 3;
            }
            else if (e.KeyCode == key_cw_dash)
            {
                g59.KEYER = 2;
            }

            if (e.KeyCode == Keys.Space)
                chkMOX.Checked = false;

        }

        private void Console_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) // changes yt7pwr
        {
            if (!chkPower.Checked)
                return;

            if (e.Shift == true)
                shift_down = true;

            if (e.Control == true && e.Shift == true && e.KeyCode == Keys.P)
            {
            }
            else if (!enable_kb_shortcuts)
            {
                return;
            }
            else if (e.Control && !e.Alt)		// control key is pressed
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        ChangeWheelTuneLeft();
                        e.Handled = true;
                        break;
                    case Keys.Right:
                        ChangeWheelTuneRight();
                        e.Handled = true;
                        break;
                    case Keys.Up:
                        Console_MouseWheel(this, new MouseEventArgs(MouseButtons.None, 0, 0, 0, 120));
                        e.Handled = true;
                        break;
                    case Keys.Down:
                        Console_MouseWheel(this, new MouseEventArgs(MouseButtons.None, 0, 0, 0, -120));
                        e.Handled = true;
                        break;
                    case Keys.A:
                        if (CurrentAGCMode == AGCMode.FAST)
                            CurrentAGCMode = AGCMode.FIXD;
                        else CurrentAGCMode++;
                        break;
                    case Keys.C:
                        btnMemoryQuickSave_Click(this, EventArgs.Empty);
                        break;
                    case Keys.D:
                        switch (Display.CurrentDisplayMode)
                        {
                            case DisplayMode.WATERFALL:
                                comboDisplayMode.Text = "Waterfall";
                                break;
                            case DisplayMode.PANAFALL:
                                comboDisplayMode.Text = "Panafall";
                                break;
                            case DisplayMode.PANADAPTER:
                                comboDisplayMode.Text = "Panadapter";
                                break;
                            case DisplayMode.PHASE:
                                comboDisplayMode.Text = "Phase";
                                break;
                            case DisplayMode.PHASE2:
                                comboDisplayMode.Text = "Phase2";
                                break;
                            case DisplayMode.SCOPE:
                                comboDisplayMode.Text = "Scope";
                                break;
                            case DisplayMode.SPECTRUM:
                                comboDisplayMode.Text = "Spectrum";
                                break;
                            case DisplayMode.HISTOGRAM:
                                comboDisplayMode.Text = "Histogram";
                                break;
                            default:
                                comboDisplayMode.Text = "Off";
                                break;
                        }
                        break;
                    case Keys.V:
                        btnMemoryQuickRestore_Click(this, EventArgs.Empty);
                        break;
                    case Keys.M:
                        if (chkMOX.Enabled)
                            chkMOX.Checked = !chkMOX.Checked;
                        break;
                    case Keys.P:
                        CurrentPreampMode = (PreampMode)(((int)current_preamp_mode + 1) % (int)PreampMode.LAST);
                        break;
                    case Keys.S:
                        if (chkVFOSplit.Enabled)
                            chkVFOSplit.Checked = !chkVFOSplit.Checked;
                        break;
                    case Keys.F:
                        int low = (int)udFilterLow.Value;
                        int high = (int)udFilterHigh.Value;
                        if (high - low > 10)
                        {
                            switch (current_dsp_mode)
                            {
                                case DSPMode.AM:
                                case DSPMode.SAM:
                                case DSPMode.DSB:
                                case DSPMode.FMN:
                                case DSPMode.CWU:
                                case DSPMode.CWL:
                                    UpdateFilters(low + 5, high - 5);
                                    break;
                                case DSPMode.USB:
                                case DSPMode.DIGU:
                                    UpdateFilters(low, high - 10);
                                    break;
                                case DSPMode.LSB:
                                case DSPMode.DIGL:
                                    UpdateFilters(low + 10, high);
                                    break;
                            }
                        }
                        break;
                    case Keys.L:
                        if (chkVFOLock.Enabled)
                            chkVFOLock.Checked = !chkVFOLock.Checked;
                        break;
                    case Keys.K:
                        if (chkVFOsinc.Enabled)
                            chkVFOsinc.Checked = !chkVFOsinc.Checked;
                        break;
                    case Keys.W:
                        if (udCWSpeed.Value != udCWSpeed.Minimum)
                            udCWSpeed.Value--;
                        break;
                }
            }
            else if (e.Alt && !e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.F:
                        int low = (int)udFilterLow.Value;
                        int high = (int)udFilterHigh.Value;
                        switch (current_dsp_mode)
                        {
                            case DSPMode.AM:
                            case DSPMode.SAM:
                            case DSPMode.DSB:
                            case DSPMode.FMN:
                            case DSPMode.CWU:
                            case DSPMode.CWL:
                                UpdateFilters(low - 5, high + 5);
                                break;
                            case DSPMode.USB:
                            case DSPMode.DIGU:
                                UpdateFilters(low, high + 10);
                                break;
                            case DSPMode.LSB:
                            case DSPMode.DIGL:
                                UpdateFilters(low - 10, high);
                                break;
                        }
                        break;
                    case Keys.G:
                        btnVFOAtoB_Click(this, EventArgs.Empty);
                        break;
                    case Keys.H:
                        btnVFOBtoA_Click(this, EventArgs.Empty);
                        break;
                    case Keys.I:
                        btnFilterShiftReset_Click(this, EventArgs.Empty);
                        break;
                    case Keys.Q:
                        if (udCWSpeed.Value != udCWSpeed.Maximum)
                            udCWSpeed.Value++;
                        break;
                    case Keys.R:
                        btnRITReset_Click(this, EventArgs.Empty);
                        break;
                    case Keys.T:
                        chkTUN.Checked = !chkTUN.Checked;
                        break;
                    case Keys.V:
                        btnVFOSwap_Click(this, EventArgs.Empty);
                        break;
                    case Keys.Y:
                        btnXITReset_Click(this, EventArgs.Empty);
                        break;
                    case Keys.Z:
                        if (btnZeroBeat.Enabled)
                            btnZeroBeat_Click(this, EventArgs.Empty);
                        break;
                }
            }
            else if (!e.Alt && !e.Control)
            {
                if (this.ActiveControl is TextBoxTS) return;
                if (this.ActiveControl is NumericUpDownTS) return;

                switch (e.KeyCode)
                {
                    case Keys.Multiply:
                        chkMUT.Checked = !chkMUT.Checked;
                        break;
                    case Keys.Add:
                        if (udAF.Value != udAF.Maximum)
                            udAF.Value++;
                        break;
                    case Keys.Subtract:
                        if (udAF.Value != udAF.Minimum)
                            udAF.Value--;
                        break;
                    case Keys.K:
                        int low = (int)udFilterLow.Value;
                        int high = (int)udFilterHigh.Value;
                        int increment = 0;
                        switch (current_dsp_mode)
                        {
                            case DSPMode.CWL:
                            case DSPMode.CWU:
                            case DSPMode.DIGL:
                            case DSPMode.DIGU:
                                increment = 10;
                                break;
                            default:
                                increment = 50;
                                break;
                        }
                        UpdateFilters(low - increment, high - increment);
                        /*if(tbFilterShift.Value != tbFilterShift.Minimum)
                            tbFilterShift.Value--;
                        tbFilterShift_Scroll(this, EventArgs.Empty);*/
                        break;
                    case Keys.L:
                        low = (int)udFilterLow.Value;
                        high = (int)udFilterHigh.Value;
                        increment = 0;
                        switch (current_dsp_mode)
                        {
                            case DSPMode.CWL:
                            case DSPMode.CWU:
                            case DSPMode.DIGL:
                            case DSPMode.DIGU:
                                increment = 10;
                                break;
                            default:
                                increment = 50;
                                break;
                        }
                        UpdateFilters(low + increment, high + increment);
                        /*if(tbFilterShift.Value != tbFilterShift.Maximum)
                            tbFilterShift.Value++;
                        tbFilterShift_Scroll(this, EventArgs.Empty);*/
                        break;
                }

                if (e.KeyCode == key_show_hide_gui)
                {
                    SI570.Show_Hide_SI570_GUI();
                }

                if (e.KeyCode == key_tune_up_1)
                {
                    double freq = Double.Parse(txtVFOAFreq.Text);
                    freq += 1.0;
                    VFOAFreq = freq;
                }
                else if (e.KeyCode == key_tune_down_1)
                {
                    double freq = Double.Parse(txtVFOAFreq.Text);
                    freq -= 1.0;
                    VFOAFreq = freq;
                }
                else if (e.KeyCode == key_tune_up_2)
                {
                    double freq = Double.Parse(txtVFOAFreq.Text);
                    freq += 0.1;
                    VFOAFreq = freq;
                }
                else if (e.KeyCode == key_tune_down_2)
                {
                    double freq = Double.Parse(txtVFOAFreq.Text);
                    freq -= 0.1;
                    VFOAFreq = freq;
                }
                else if (e.KeyCode == key_tune_up_3)
                {
                    double freq = Double.Parse(txtVFOAFreq.Text);
                    freq += 0.01;
                    VFOAFreq = freq;
                }
                else if (e.KeyCode == key_tune_down_3)
                {
                    double freq = Double.Parse(txtVFOAFreq.Text);
                    freq -= 0.01;
                    VFOAFreq = freq;
                }
                else if (e.KeyCode == key_tune_up_4)
                {
                    double freq = Double.Parse(txtVFOAFreq.Text);
                    freq += 0.001;
                    VFOAFreq = freq;
                }
                else if (e.KeyCode == key_tune_down_4)
                {
                    double freq = Double.Parse(txtVFOAFreq.Text);
                    freq -= 0.001;
                    VFOAFreq = freq;
                }
                else if (e.KeyCode == key_tune_up_5)
                {
                    double freq = Double.Parse(txtVFOAFreq.Text);
                    freq += 0.0001;
                    VFOAFreq = freq;
                }
                else if (e.KeyCode == key_tune_down_5)
                {
                    double freq = Double.Parse(txtVFOAFreq.Text);
                    freq -= 0.0001;
                    VFOAFreq = freq;
                }
                else if (e.KeyCode == key_tune_up_6)
                {
                    double freq = Double.Parse(txtVFOAFreq.Text);
                    freq += 0.00001;
                    VFOAFreq = freq;
                }
                else if (e.KeyCode == key_tune_down_6)
                {
                    double freq = Double.Parse(txtVFOAFreq.Text);
                    freq -= 0.00001;
                    VFOAFreq = freq;
                }
                else if (e.KeyCode == key_tune_up_7)
                {
                    double freq = Double.Parse(txtVFOAFreq.Text);
                    freq += 0.000001;
                    VFOAFreq = freq;
                }
                else if (e.KeyCode == key_tune_down_7)
                {
                    double freq = Double.Parse(txtVFOAFreq.Text);
                    freq -= 0.000001;
                    VFOAFreq = freq;
                }
                else if (e.KeyCode == key_rit_up)
                {
                    udRIT.Value += udRIT.Increment;
                }
                else if (e.KeyCode == key_rit_down)
                {
                    udRIT.Value -= udRIT.Increment;
                }
                else if (e.KeyCode == key_xit_up)
                {
                    udXIT.Value += udXIT.Increment;
                }
                else if (e.KeyCode == key_xit_down)
                {
                    udXIT.Value -= udXIT.Increment;
                }
                else if (e.KeyCode == key_filter_up)
                {
                    if (current_filter == Filter.NONE)
                        return;
                    if (current_filter == Filter.VAR2)
                        CurrentFilter = Filter.F1;
                    else
                        CurrentFilter++;
                }
                else if (e.KeyCode == key_filter_down)
                {
                    if (current_filter == Filter.NONE)
                        return;
                    if (current_filter == Filter.F1)
                        CurrentFilter = Filter.VAR2;
                    else
                        CurrentFilter--;
                }
                else if (e.KeyCode == key_mode_up)
                {
                    switch (current_dsp_mode)
                    {
                        case DSPMode.LSB:
                            CurrentDSPMode = DSPMode.USB;
                            break;
                        case DSPMode.USB:
                            CurrentDSPMode = DSPMode.DSB;
                            break;
                        case DSPMode.DSB:
                            CurrentDSPMode = DSPMode.CWL;
                            break;
                        case DSPMode.CWL:
                            CurrentDSPMode = DSPMode.CWU;
                            break;
                        case DSPMode.CWU:
                            CurrentDSPMode = DSPMode.FMN;
                            break;
                        case DSPMode.FMN:
                            CurrentDSPMode = DSPMode.AM;
                            break;
                        case DSPMode.AM:
                            CurrentDSPMode = DSPMode.SAM;
                            break;
                        case DSPMode.SAM:
                            CurrentDSPMode = DSPMode.DIGL;
                            break;
                        case DSPMode.DIGL:
                            CurrentDSPMode = DSPMode.DIGU;
                            break;
                        case DSPMode.DIGU:
                            CurrentDSPMode = DSPMode.SPEC;
                            break;
                        case DSPMode.SPEC:
                            CurrentDSPMode = DSPMode.DRM;
                            break;
                        case DSPMode.DRM:
                            CurrentDSPMode = DSPMode.LSB;
                            break;
                    }
                }
                else if (e.KeyCode == key_mode_down)
                {
                    switch (current_dsp_mode)
                    {
                        case DSPMode.LSB:
                            CurrentDSPMode = DSPMode.DRM;
                            break;
                        case DSPMode.USB:
                            CurrentDSPMode = DSPMode.LSB;
                            break;
                        case DSPMode.DSB:
                            CurrentDSPMode = DSPMode.USB;
                            break;
                        case DSPMode.CWL:
                            CurrentDSPMode = DSPMode.DSB;
                            break;
                        case DSPMode.CWU:
                            CurrentDSPMode = DSPMode.CWL;
                            break;
                        case DSPMode.FMN:
                            CurrentDSPMode = DSPMode.CWU;
                            break;
                        case DSPMode.AM:
                            CurrentDSPMode = DSPMode.FMN;
                            break;
                        case DSPMode.SAM:
                            CurrentDSPMode = DSPMode.AM;
                            break;
                        case DSPMode.DIGL:
                            CurrentDSPMode = DSPMode.SAM;
                            break;
                        case DSPMode.DIGU:
                            CurrentDSPMode = DSPMode.DIGL;
                            break;
                        case DSPMode.SPEC:
                            CurrentDSPMode = DSPMode.DIGU;
                            break;
                        case DSPMode.DRM:
                            CurrentDSPMode = DSPMode.SPEC;
                            break;
                    }
                }
                else if (e.KeyCode == key_band_up && !vfo_lock)
                {
                    switch (current_band)
                    {
                        case Band.B160M:
                            if (band_160m_index == 2)
                            {
                                band_80m_index = 0;
                                btnBand80_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                btnBand160_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B80M:
                            if (band_80m_index == 2)
                            {
                                band_60m_index = 0;
                                btnBand60_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                btnBand80_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B60M:
                            if (band_60m_index == 4)
                            {
                                band_40m_index = 0;
                                btnBand40_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                btnBand60_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B40M:
                            if (band_40m_index == 2)
                            {
                                band_30m_index = 0;
                                btnBand30_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                btnBand40_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B30M:
                            if (band_30m_index == 2)
                            {
                                band_20m_index = 0;
                                btnBand20_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                btnBand30_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B20M:
                            if (band_20m_index == 2)
                            {
                                band_17m_index = 0;
                                btnBand17_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                btnBand20_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B17M:
                            if (band_17m_index == 2)
                            {
                                band_15m_index = 0;
                                btnBand15_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                btnBand17_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B15M:
                            if (band_15m_index == 2)
                            {
                                band_12m_index = 0;
                                btnBand12_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                btnBand15_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B12M:
                            if (band_12m_index == 2)
                            {
                                band_10m_index = 0;
                                btnBand10_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                btnBand12_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B10M:
                            if (band_10m_index == 2)
                            {
                                band_6m_index = 0;
                                btnBand6_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                btnBand10_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B6M:
                            if (band_6m_index == 2)
                            {
                                    band_wwv_index = 0;
                                    btnBandWWV_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                btnBand6_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B2M:
                            if (band_2m_index == 2)
                            {
                                band_wwv_index = 0;
                                btnBandWWV_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                btnBand2_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.WWV:
                            if (band_wwv_index == 4)
                            {
                                band_gen_index = 0;
                                btnBandGEN_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                btnBandWWV_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.GEN:
                            if (band_gen_index == 4)
                            {
                                band_160m_index = 0;
                                btnBand160_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                btnBandGEN_Click(this, EventArgs.Empty);
                            }
                            break;
                    }
                }
                else if (e.KeyCode == key_band_down && !vfo_lock)
                {
                    switch (current_band)
                    {
                        case Band.B160M:
                            if (band_160m_index == 0)
                            {
                                band_gen_index = 4;
                                btnBandGEN_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                last_band = "160M";
                                band_160m_index = (band_160m_index + 1) % 3;
                                btnBand160_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B80M:
                            if (band_80m_index == 0)
                            {
                                band_160m_index = 2;
                                btnBand160_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                last_band = "80M";
                                band_80m_index = (band_80m_index + 1) % 3;
                                btnBand80_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B60M:
                            if (band_60m_index == 0)
                            {
                                band_80m_index = 2;
                                btnBand80_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                last_band = "60M";
                                band_60m_index = (band_60m_index + 3) % 5;
                                btnBand60_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B40M:
                            if (band_40m_index == 0)
                            {
                                band_60m_index = 4;
                                btnBand60_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                last_band = "40M";
                                band_40m_index = (band_40m_index + 1) % 3;
                                btnBand40_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B30M:
                            if (band_30m_index == 0)
                            {
                                band_40m_index = 2;
                                btnBand40_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                last_band = "30M";
                                band_30m_index = (band_30m_index + 1) % 3;
                                btnBand30_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B20M:
                            if (band_20m_index == 0)
                            {
                                band_30m_index = 2;
                                btnBand30_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                last_band = "20M";
                                band_20m_index = (band_20m_index + 1) % 3;
                                btnBand20_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B17M:
                            if (band_17m_index == 0)
                            {
                                band_20m_index = 2;
                                btnBand20_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                last_band = "17M";
                                band_17m_index = (band_17m_index + 1) % 3;
                                btnBand17_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B15M:
                            if (band_15m_index == 0)
                            {
                                band_17m_index = 2;
                                btnBand17_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                last_band = "15M";
                                band_15m_index = (band_15m_index + 1) % 3;
                                btnBand15_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B12M:
                            if (band_12m_index == 0)
                            {
                                band_15m_index = 2;
                                btnBand15_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                last_band = "12M";
                                band_12m_index = (band_12m_index + 1) % 3;
                                btnBand12_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B10M:
                            if (band_10m_index == 0)
                            {
                                band_12m_index = 2;
                                btnBand12_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                last_band = "10M";
                                band_10m_index = (band_10m_index + 1) % 3;
                                btnBand10_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B6M:
                            if (band_6m_index == 0)
                            {
                                band_10m_index = 2;
                                btnBand10_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                last_band = "6M";
                                band_6m_index = (band_6m_index + 1) % 3;
                                btnBand6_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.B2M:
                            if (band_2m_index == 0)
                            {
                                band_6m_index = 2;
                                btnBand6_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                last_band = "2M";
                                band_2m_index = (band_2m_index + 1) % 3;
                                btnBand6_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.WWV:
                            if (band_wwv_index == 0)
                            {
                                    band_6m_index = 2;
                                    btnBand6_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                last_band = "WWV";
                                band_wwv_index = (band_wwv_index + 3) % 5;
                                btnBandWWV_Click(this, EventArgs.Empty);
                            }
                            break;
                        case Band.GEN:
                            if (band_gen_index == 0)
                            {
                                band_wwv_index = 4;
                                btnBandWWV_Click(this, EventArgs.Empty);
                            }
                            else
                            {
                                last_band = "GEN";
                                band_gen_index = (band_gen_index + 3) % 5;
                                btnBandGEN_Click(this, EventArgs.Empty);
                            }
                            break;
                    }

                }
                else if (e.KeyCode == Keys.Space)
                {
                    if (chkPower.Checked)
                        chkMOX.Checked = true;
                }
                else if (e.KeyCode == key_cw_dot)
                {
                    g59.KEYER = 1;
                }
                else if (e.KeyCode == key_cw_dash)
                {
                    g59.KEYER = 0;
                }
                else if (vfo_lock || !quick_qsy)
                {
                    return;
                }
            }
        }

        public bool power = false;
        // chkPower
        private void chkPower_CheckedChanged(object sender, System.EventArgs e) // changes yt7pwr
        {
            double freq;
            DttSP.AudioReset();
            if (chkPower.Checked)
            {
                power = true;
                freq = double.Parse(txtLOSCFreq.Text);
                freq *= 1000000;
                if (usb_si570_enable)
                {
                    SI570.Start_SI570((long)freq);
                    SI570.Set_SI570_osc((long)freq);
                }
                else
                    g59.Set_frequency((long)freq);
                chkPower.Text = "On";
                chkPower.BackColor = button_selected_color;
                txtVFOAFreq.ForeColor = vfo_text_light_color;
                txtVFOAMSD.ForeColor = vfo_text_light_color;
                txtVFOALSD.ForeColor = small_vfo_color;
                txtVFOABand.ForeColor = band_text_light_color;
                txtLOSCFreq.ForeColor = vfo_text_light_color;
                txtLOSCMSD.ForeColor = vfo_text_light_color;
                txtLOSCLSD.ForeColor = small_vfo_color;

                if (chkVFOSplit.Checked)
                {
                    txtVFOBFreq.ForeColor = Color.Red;
                    txtVFOBMSD.ForeColor = Color.Red;
                    txtVFOBLSD.ForeColor = small_vfo_color;
                    txtVFOBBand.ForeColor = band_text_light_color;
                }
                else if (chkEnableSubRX.Checked)
                {
                    txtVFOBFreq.ForeColor = vfo_text_light_color;
                    txtVFOBMSD.ForeColor = vfo_text_light_color;
                    txtVFOBLSD.ForeColor = small_vfo_color;
                    txtVFOBBand.ForeColor = band_text_light_color;
                }

                txtVFOAFreq_LostFocus(this, EventArgs.Empty);
                txtVFOBFreq_LostFocus(this, EventArgs.Empty);
                txtLOSCFreq_LostFocus(this, EventArgs.Empty);

                // wjt added 
                if (PTTBitBangEnabled && serialPTT == null) // we are enabled but don't have port object 
                {
                    //Debug.WriteLine("Forcing property set on PTTBitBangEnabled"); 
                    PTTBitBangEnabled = true; // force creation of serial ptt 
                }
                // wjt added ends 

                SetupForm.AudioReceiveMux1 = SetupForm.AudioReceiveMux1;		// set receive mux

                Audio.CurrentAudioState1 = Audio.AudioState.DTTSP;
                Audio.callback_return = 0;

                if (!Audio.Start())
                {
                    chkPower.Checked = false;
                    return;
                }

                DttSP.SetRXListen(0);
                DttSP.SetOsc(0.0);

                double vfoA_freq = ((LOSCFreq - VFOAFreq) * 1e6);
                double vfoB_freq = ((LOSCFreq - VFOBFreq) * 1e6);

                if (chkEnableSubRX.Checked)
                {
                    DttSP.SetRXListen(2);
                    DttSP.SetRXOn();
                    DttSP.SetOscDll(vfoB_freq);
                    DttSP.SetRXListen(1);
                    DttSP.SetRXOn();
                    DttSP.SetOscDll(vfoA_freq);
                    DttSP.SetRXOutputGain(1, (double)tbRX0Gain.Value / tbRX0Gain.Maximum);
                    DttSP.SetRXOutputGain(2, (double)tbRX1Gain.Value / tbRX1Gain.Maximum);
                }
                else
                {
                    DttSP.SetRXListen(2);
                    DttSP.SetRXOff();
                    DttSP.SetRXListen(1);
                    DttSP.SetRXOn();
                    DttSP.SetOscDll(vfoA_freq);
                    DttSP.SetRXOutputGain(1, 1.0);
                    DttSP.SetRXOutputGain(2, 0.0);
                }

                DttSP.SetRXOutputGain(0, 0.0);

                if (draw_display_thread == null || !draw_display_thread.IsAlive)
                {
                    draw_display_thread = new Thread(new ThreadStart(RunDisplay));
                    draw_display_thread.Name = "Draw Display Thread";
                    draw_display_thread.Priority = ThreadPriority.BelowNormal;
                    draw_display_thread.IsBackground = true;
                    draw_display_thread.Start();
                }

                if (multimeter_thread == null || !multimeter_thread.IsAlive)
                {
                    multimeter_thread = new Thread(new ThreadStart(UpdateMultimeter));
                    multimeter_thread.Name = "Multimeter Thread";
                    multimeter_thread.Priority = ThreadPriority.Lowest;
                    multimeter_thread.IsBackground = true;
                    multimeter_thread.Start();
                }

                if (sql_update_thread == null || !sql_update_thread.IsAlive)
                {
                    sql_update_thread = new Thread(new ThreadStart(UpdateSQL));
                    sql_update_thread.Name = "Update SQL";
                    sql_update_thread.Priority = ThreadPriority.Normal;
                    sql_update_thread.IsBackground = true;
                    sql_update_thread.Start();
                }

                if (noise_gate_update_thread == null || !noise_gate_update_thread.IsAlive)
                {
                    noise_gate_update_thread = new Thread(new ThreadStart(UpdateNoiseGate));
                    noise_gate_update_thread.Name = "Update NoiseGate";
                    noise_gate_update_thread.Priority = ThreadPriority.Normal;
                    noise_gate_update_thread.IsBackground = true;
                    noise_gate_update_thread.Start();
                }

                if (vox_update_thread == null || !vox_update_thread.IsAlive)
                {
                    vox_update_thread = new Thread(new ThreadStart(UpdateVOX));
                    vox_update_thread.Name = "Update VOX";
                    vox_update_thread.Priority = ThreadPriority.Normal;
                    vox_update_thread.IsBackground = true;
                    vox_update_thread.Start();
                }

                if (poll_ptt_thread == null || !poll_ptt_thread.IsAlive)
                {
                    poll_ptt_thread = new Thread(new ThreadStart(PollPTT));
                    poll_ptt_thread.Name = "Poll PTT Thread";
                    poll_ptt_thread.Priority = ThreadPriority.Normal;
                    poll_ptt_thread.IsBackground = true;
                    poll_ptt_thread.Start();
                }

                if (!rx_only)
                {
                    chkMOX.Enabled = true;
                    chkTUN.Enabled = true;
                }
                chkVFOLock.Enabled = true;
                chkVFOsinc.Enabled = true;

                timer_peak_text.Enabled = true;

                if (DttSP.CurrentMode == DttSP.Mode.CWL ||
                    DttSP.CurrentMode == DttSP.Mode.CWU)
                {
                    DttSP.StartKeyer();
                }
            }
            else
            {
                power = false;
                DttSP.SetRXListen(2);
                DttSP.SetRXOff();
                DttSP.SetRXListen(1);
                DttSP.SetRXOff();
                Audio.callback_return = 2;
                chkPower.Text = "Standby";

                chkMOX.Checked = false;
                chkMOX.Enabled = false;
                chkTUN.Checked = false;
                chkTUN.Enabled = false;

                if (DttSP.CurrentMode == DttSP.Mode.CWL ||
                    DttSP.CurrentMode == DttSP.Mode.CWU)
                {
                    DttSP.StopKeyer();
                } 

                chkVFOLock.Enabled = false;
                chkVFOsinc.Enabled = false;

                chkPower.BackColor = SystemColors.Control;
                txtVFOAFreq.ForeColor = vfo_text_dark_color;
                txtVFOAMSD.ForeColor = vfo_text_dark_color;
                txtVFOALSD.ForeColor = vfo_text_dark_color;
                txtVFOABand.ForeColor = band_text_dark_color;

                txtVFOBFreq.ForeColor = vfo_text_dark_color;
                txtVFOBMSD.ForeColor = vfo_text_dark_color;
                txtVFOBLSD.ForeColor = vfo_text_dark_color;
                txtVFOBBand.ForeColor = band_text_dark_color;

                txtLOSCFreq.ForeColor = vfo_text_dark_color;
                txtLOSCMSD.ForeColor = vfo_text_dark_color;
                txtLOSCLSD.ForeColor = vfo_text_dark_color;

                timer_peak_text.Enabled = false;

                Display.ResetDisplayAverage();
                Display.ResetDisplayPeak();

                Audio.StopAudio1();
                if (vac_enabled) Audio.StopAudioVAC();
            }

            panelVFOAHover.Invalidate();
            panelVFOBHover.Invalidate();
            panelLOSCHover.Invalidate();
        }

        public void comboDisplayMode_SelectedIndexChanged(object sender, System.EventArgs e)  // changes yt7pwr
        {
            txtDisplayPeakFreq.Width = picDisplay.Width - (textBox2.Width + textBox1.Width +
                txtDisplayCursorOffset.Width + txtDisplayCursorPower.Width + txtDisplayCursorFreq.Width +
                txtDisplayPeakPower.Width + txtDisplayPeakOffset.Width);

            DisplayMode old_mode = Display.CurrentDisplayMode;
            if (draw_display_thread == null || !draw_display_thread.IsAlive)
            {
                draw_display_thread = new Thread(new ThreadStart(RunDisplay));
                draw_display_thread.Name = "Draw Display Thread";
                draw_display_thread.Priority = ThreadPriority.BelowNormal;
                draw_display_thread.Start();
            }

            switch (comboDisplayMode.Text)
            {
                case "Histogram":
                    {
                        picWaterfall.Hide();
                        System.Drawing.Point grp_position = new System.Drawing.Point(0, 0);
                        System.Drawing.Point picDisplay_position = new System.Drawing.Point(10, 15);
                        grpDisplay.Height = this.Height - grpVFO.Height - 180;    // picDisplay
                        grpDisplay.Width = this.Width - grpBandHF.Width - 140;
                        picDisplay.Location = picDisplay_position;
                        picDisplay.Height = (grpDisplay.Height - 40);
                        picDisplay.Width = grpDisplay.Width - 19;
                        picDisplay.Show();
                        Display.CurrentDisplayMode = DisplayMode.HISTOGRAM;
                    }
                    break;
                case "Scope":
                    {
                        picWaterfall.Hide();
                        System.Drawing.Point grp_position = new System.Drawing.Point(0, 0);
                        System.Drawing.Point picDisplay_position = new System.Drawing.Point(10, 15);
                        grpDisplay.Height = this.Height - grpVFO.Height - 180;    // picDisplay
                        grpDisplay.Width = this.Width - grpBandHF.Width - 140;
                        picDisplay.Location = picDisplay_position;
                        picDisplay.Height = (grpDisplay.Height - 40);
                        picDisplay.Width = grpDisplay.Width - 19;
                        picDisplay.Show();
                        Display.CurrentDisplayMode = DisplayMode.SCOPE;
                    }
                    break;
                case "Phase":
                    {
                        picWaterfall.Hide();
                        System.Drawing.Point grp_position = new System.Drawing.Point(0, 0);
                        System.Drawing.Point picDisplay_position = new System.Drawing.Point(10, 15);
                        grpDisplay.Height = this.Height - grpVFO.Height - 180;    // picDisplay
                        grpDisplay.Width = this.Width - grpBandHF.Width - 140;
                        picDisplay.Location = picDisplay_position;
                        picDisplay.Height = (grpDisplay.Height - 40);
                        picDisplay.Width = grpDisplay.Width - 19;
                        picDisplay.Show();
                        Display.CurrentDisplayMode = DisplayMode.PHASE;
                    }
                    break;
                case "Phase2":
                    {
                        picWaterfall.Hide();
                        System.Drawing.Point grp_position = new System.Drawing.Point(0, 0);
                        System.Drawing.Point picDisplay_position = new System.Drawing.Point(10, 15);
                        grpDisplay.Height = this.Height - grpVFO.Height - 180;    // picDisplay
                        grpDisplay.Width = this.Width - grpBandHF.Width - 140;
                        picDisplay.Location = picDisplay_position;
                        picDisplay.Height = (grpDisplay.Height - 40);
                        picDisplay.Width = grpDisplay.Width - 19;
                        picDisplay.Show();
                        Display.CurrentDisplayMode = DisplayMode.PHASE2;
                    }
                    break;
                case "Off":
                    {
                        System.Drawing.Point grp_position = new System.Drawing.Point(0, 0);
                        System.Drawing.Point picWaterfall_position = new System.Drawing.Point(10, 15);
                        grpDisplay.Height = this.Height - grpVFO.Height - 180;
                        grpDisplay.Width = this.Width - grpBandHF.Width - 140;
                        picWaterfall.Location = picWaterfall_position;
                        picWaterfall.Height = (grpDisplay.Height - 40) / 2;       // panafall           
                        picWaterfall.Width = grpDisplay.Width - 19;
                        grp_position = picWaterfall.Location;
                        grp_position.Y = picWaterfall.Height + 15;
                        picWaterfall.Show();
                        picDisplay.Location = grp_position;
                        picDisplay.Height = (grpDisplay.Height - 40) / 2;
                        picDisplay.Width = grpDisplay.Width - 19;
                        picDisplay.Show();
                        Display.CurrentDisplayMode = DisplayMode.OFF;
                        CalcDisplayFreq();
                    }
                    break;
                case "Panafall":
                    {
                        System.Drawing.Point grp_position = new System.Drawing.Point(0, 0);
                        System.Drawing.Point picWaterfall_position = new System.Drawing.Point(10, 15);
                        grpDisplay.Height = this.Height - grpVFO.Height - 180;
                        grpDisplay.Width = this.Width - grpBandHF.Width - 140;
                        picWaterfall.Location = picWaterfall_position;
                        picWaterfall.Height = (grpDisplay.Height - 40) / 2;       // panafall           
                        picWaterfall.Width = grpDisplay.Width - 19;
                        grp_position = picWaterfall.Location;
                        grp_position.Y = picWaterfall.Height + 15;
                        picWaterfall.Show();
                        picDisplay.Location = grp_position;
                        picDisplay.Height = (grpDisplay.Height - 40) / 2;
                        picDisplay.Width = grpDisplay.Width - 19;
                        picDisplay.Show();
                        Display.CurrentDisplayMode = DisplayMode.PANAFALL;
                        CalcDisplayFreq();
                    }
                    break;
                case "Spectrum":
                    {
                        System.Drawing.Point grp_position = new System.Drawing.Point(0, 0);
                        System.Drawing.Point picDisplay_position = new System.Drawing.Point(10, 15);
                        grpDisplay.Height = this.Height - grpVFO.Height - 180;    // picDisplay
                        grpDisplay.Width = this.Width - grpBandHF.Width - 140;
                        picDisplay.Location = picDisplay_position;
                        picDisplay.Height = (grpDisplay.Height - 40);
                        picDisplay.Width = grpDisplay.Width - 19;
                        picWaterfall.Hide();
                        picDisplay.Show();
                        Display.CurrentDisplayMode = DisplayMode.SPECTRUM;
                    }
                    break;
                case "Panadapter":
                    {
                        System.Drawing.Point grp_position = new System.Drawing.Point(0, 0);
                        System.Drawing.Point picDisplay_position = new System.Drawing.Point(10, 15);
                        grpDisplay.Height = this.Height - grpVFO.Height - 180;    // picDisplay
                        grpDisplay.Width = this.Width - grpBandHF.Width - 140;
                        picDisplay.Location = picDisplay_position;
                        picDisplay.Height = (grpDisplay.Height - 40);
                        picDisplay.Width = grpDisplay.Width - 19;
                        picWaterfall.Hide();
                        picDisplay.Show();
                        Display.CurrentDisplayMode = DisplayMode.PANADAPTER;
                        CalcDisplayFreq();
                    }
                    break;
                case "Waterfall":
                    {
                        System.Drawing.Point grp_position = new System.Drawing.Point(0, 0);
                        System.Drawing.Point picWaterfall_position = new System.Drawing.Point(10, 15);
                        picDisplay.Hide();
                        grpDisplay.Height = this.Height - grpVFO.Height - 180;    // waterfall
                        grpDisplay.Width = this.Width - grpBandHF.Width - 140;
                        picWaterfall.Location = picWaterfall_position;
                        picWaterfall.Height = (grpDisplay.Height - 40);
                        picWaterfall.Width = grpDisplay.Width - 19;
                        picWaterfall.Show();
                        Display.CurrentDisplayMode = DisplayMode.WATERFALL;
                        CalcDisplayFreq();
                    }
                    break;
            }

            if (old_mode == DisplayMode.PANADAPTER &&
                Display.CurrentDisplayMode != DisplayMode.PANADAPTER)
            {
                CurrentFilter = current_filter; // reset filter display limits
                DttSP.TXFilterLowCut = DttSP.TXFilterLowCut;
            }

            chkDisplayAVG.Enabled = true;
            chkDisplayPeak.Enabled = true;

            if (chkDisplayAVG.Checked)
            {
                switch (Display.CurrentDisplayMode)
                {
                    case DisplayMode.PANAFALL:
                    case DisplayMode.PANADAPTER:
                    case DisplayMode.WATERFALL:
                        btnZeroBeat.Enabled = true;
                        break;
                    default:
                        btnZeroBeat.Enabled = false;
                        break;
                }
            }

            was_panadapter = false;

            if (comboDisplayMode.Focused)
                btnHidden.Focus();
            Console_Resize(sender, e);
        }

        private void chkBIN_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkBIN.Checked)
            {
                chkBIN.BackColor = button_selected_color;
                DttSP.SetBIN(1);
            }
            else
            {
                chkBIN.BackColor = SystemColors.Control;
                DttSP.SetBIN(0);
            }
        }

        private void comboAGC_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboAGC.SelectedIndex < 0) return;
            DttSP.SetRXAGC((AGCMode)comboAGC.SelectedIndex);

            if ((AGCMode)comboAGC.SelectedIndex == AGCMode.CUSTOM)
                SetupForm.CustomRXAGCEnabled = true;
            else SetupForm.CustomRXAGCEnabled = false;

            if (comboAGC.Focused)
                btnHidden.Focus();
        }

        private void Console_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Audio.callback_return = 2;
            if (SetupForm != null) SetupForm.Hide();
            if (CWXForm != null) CWXForm.Hide();
            if (EQForm != null) EQForm.Hide();

            chkPower.Checked = false;
            Thread.Sleep(100);
            this.Hide();
            SaveState();

            if (CWXForm != null) CWXForm.SaveSettings();
            if (SetupForm != null) SetupForm.SaveOptions();
            if (EQForm != null) EQForm.SaveSettings();
        }

        private void comboPreamp_SelectedIndexChanged(object sender, System.EventArgs e)  // changes yt7pwr
        {
            switch (comboPreamp.Text)
            {
                case "Off":
                    CurrentPreampMode = PreampMode.OFF;
                    break; 
                case "High":
                    CurrentPreampMode = PreampMode.HIGH;
                    break;
            }

            if (comboPreamp.Focused)
                btnHidden.Focus();
        }

        private void chkMUT_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkMUT.Checked)
                chkMUT.BackColor = button_selected_color;
            else
                chkMUT.BackColor = SystemColors.Control;

            if (chkMUT.Checked)
                Audio.MonitorVolume = 0.0;
            else
                udAF_ValueChanged(this, EventArgs.Empty);
        }

        private void udPWR_ValueChanged(object sender, System.EventArgs e) // changes yt7pwr
        {
            if (SetupForm == null)
                return;

            float val = (float)udPWR.Value;

            if (udPWR.Value > udPWR.Maximum)
            {
                udPWR.Value = udPWR.Maximum;
                return;
            }

            if (udPWR.Value < udPWR.Minimum)
            {
                udPWR.Value = udPWR.Minimum;
                return;
            }

            tbPWR.Value = (int)udPWR.Value;

            if (VFOAFreq < 29.7f)
            {
                if (val == 0)
                {
                    Audio.RadioVolume = 0.0;
                    return;
                }

                double target_dbm = 10 * (double)Math.Log10((double)val * 10000);
                target_dbm -= GainByBand(CurrentBand);

                double target_volts = Math.Sqrt(Math.Pow(10, target_dbm * 0.1) * 0.05);		// E = Sqrt(P * R) 
                Audio.RadioVolume = target_volts / audio_volts1;
            }
            else
            {
                const double TARGET = 0.8;		// audio in volts needed to hit 1W 
                Audio.RadioVolume = (double)Math.Sqrt((double)udPWR.Value / 100.0) / audio_volts1 * TARGET;
            }
            if (udPWR.Focused)
                btnHidden.Focus();
        }

        private void tbPWR_Scroll(object sender, System.EventArgs e)
        {
            udPWR.Value = tbPWR.Value;
            if (tbPWR.Focused) btnHidden.Focus();
        }

        private void udAF_ValueChanged(object sender, System.EventArgs e)
        {
            if (SetupForm == null)
                return;

            if (udAF.Value > udAF.Maximum)
            {
                udAF.Value = udAF.Maximum;
                return;
            }

            if (udAF.Value < udAF.Minimum)
            {
                udAF.Value = udAF.Minimum;
                return;
            }

            tbAF.Value = (int)udAF.Value;

            if (chkMUT.Checked)
            {
                Audio.MonitorVolume = 0.0;
                goto end;
            }

            if ((num_channels > 2) && chkMOX.Checked && !chkMON.Checked)
            {
                // monitor is muted
                Audio.MonitorVolume = 0.0;
            }
            else
            {
                Audio.MonitorVolume = (double)udAF.Value / 100.0;
            }

        end:
            if (!MOX) RXAF = (int)udAF.Value;
            else TXAF = (int)udAF.Value;

            if (udAF.Focused)
                btnHidden.Focus();
        }

        private void tbAF_Scroll(object sender, System.EventArgs e)
        {
            udAF.Value = tbAF.Value;
            if (tbAF.Focused) btnHidden.Focus();
        }

        private void udMIC_ValueChanged(object sender, System.EventArgs e)
        {
            if (udMIC.Value > udMIC.Maximum)
            {
                udMIC.Value = udMIC.Maximum;
                return;
            }

            if (udMIC.Value < udMIC.Minimum)
            {
                udMIC.Value = udMIC.Minimum;
                return;
            }

            tbMIC.Value = (int)udMIC.Value;

            double gain_db = (double)udMIC.Value;
            if (mic_boost) gain_db += 20.0;
            Audio.MicPreamp = Math.Pow(10.0, gain_db / 20.0); // convert to scalar

            if (udMIC.Focused)
                btnHidden.Focus();
        }

        private void udRF_ValueChanged(object sender, System.EventArgs e)
        {
            if (udRF.Value > udRF.Maximum)
            {
                udRF.Value = udRF.Maximum;
                return;
            }

            if (udRF.Value < udRF.Minimum)
            {
                udRF.Value = udRF.Minimum;
                return;
            }

            tbRF.Value = (int)udRF.Value;

            switch (CurrentAGCMode)
            {
                case AGCMode.FIXD:
                    if (SetupForm != null) SetupForm.AGCFixedGain = (int)udRF.Value;
                    break;
                default:
                    if (SetupForm != null) SetupForm.AGCMaxGain = (int)udRF.Value;
                    break;
            }

            if (udRF.Focused) btnHidden.Focus();
        }

        private void tbRF_Scroll(object sender, System.EventArgs e)
        {
            udRF.Value = tbRF.Value;
            if (tbRF.Focused) btnHidden.Focus();
        }

        private void tbMIC_Scroll(object sender, System.EventArgs e)
        {
            udMIC.Value = tbMIC.Value;
            if (tbMIC.Focused) btnHidden.Focus();
        }

        private void udCWSpeed_ValueChanged(object sender, System.EventArgs e)
        {
            DttSP.SetKeyerSpeed((float)udCWSpeed.Value);
            tbCWSpeed.Value = (int)udCWSpeed.Value;
        }

        private void tbCWSpeed_Scroll(object sender, System.EventArgs e)
        {
            udCWSpeed.Value = tbCWSpeed.Value;
            if (tbCWSpeed.Focused) btnHidden.Focus();
        }

        private void chkBreakIn_CheckedChanged(object sender, System.EventArgs e)
        {
            if (SetupForm != null)
                SetupForm.BreakInEnabled = chkBreakIn.Checked;

            if (chkBreakIn.Checked) chkBreakIn.BackColor = button_selected_color;
            else chkBreakIn.BackColor = SystemColors.Control;
        }

        private void chkVOX_CheckedChanged(object sender, System.EventArgs e)
        {
            if (SetupForm != null) SetupForm.VOXEnable = chkVOX.Checked;

            if (chkVOX.Checked)
            {
                chkVOX.BackColor = button_selected_color;
            }
            else
            {
                Audio.VOXActive = false;
                chkVOX.BackColor = SystemColors.Control;
            }
        }

        private void udSquelch_ValueChanged(object sender, System.EventArgs e)
        {
            DttSP.SetSquelchVal(-(float)udSquelch.Value -
                preamp_offset[(int)current_preamp_mode] -
                multimeter_cal_offset -
                filter_size_cal_offset);
            tbSQL.Value = -(int)udSquelch.Value;
            if (udSquelch.Focused)
                btnHidden.Focus();
        }

        private void chkVACEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            if (SetupForm != null) SetupForm.VACEnable = chkVACEnabled.Checked;
            vac_enabled = chkVACEnabled.Checked;

            if (chkVACEnabled.Checked) chkVACEnabled.BackColor = button_selected_color;
            else chkVACEnabled.BackColor = SystemColors.Control;
        }

        private void udVACRXGain_ValueChanged(object sender, System.EventArgs e)
        {
            if (SetupForm != null) SetupForm.VACRXGain = (int)udVACRXGain.Value;
            tbVACRXGain.Value = (int)udVACRXGain.Value;
            if (udVACRXGain.Focused) btnHidden.Focus();
        }

        private void udVACTXGain_ValueChanged(object sender, System.EventArgs e)
        {
            if (SetupForm != null) SetupForm.VACTXGain = (int)udVACTXGain.Value;
            tbVACTXGain.Value = (int)udVACTXGain.Value;
            if (udVACTXGain.Focused) btnHidden.Focus();
        }

        private void chkNoiseGate_CheckedChanged(object sender, System.EventArgs e)
        {
            if (SetupForm != null) SetupForm.NoiseGateEnabled = chkNoiseGate.Checked;

            if (chkNoiseGate.Checked) chkNoiseGate.BackColor = button_selected_color;
            else chkNoiseGate.BackColor = SystemColors.Control;
        }

        private void tbVACRXGain_Scroll(object sender, System.EventArgs e)
        {
            udVACRXGain.Value = tbVACRXGain.Value;
            if (tbVACRXGain.Focused) btnHidden.Focus();
        }

        private void tbVACTXGain_Scroll(object sender, System.EventArgs e)
        {
            udVACTXGain.Value = tbVACTXGain.Value;
            if (tbVACTXGain.Focused) btnHidden.Focus();
        }

        private void picSQL_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int signal_x = (int)((sql_data + 160.0) * (picSQL.Width - 1) / 160.0);
            int sql_x = (int)((-(float)udSquelch.Value + 160.0) * (picSQL.Width - 1) / 160.0);

            if (chkMOX.Checked) signal_x = sql_x = 0;
            e.Graphics.FillRectangle(new SolidBrush(Color.LimeGreen), 0, 0, signal_x, picSQL.Height);
            if (sql_x < signal_x)
                e.Graphics.FillRectangle(new SolidBrush(Color.Red), sql_x + 1, 0, signal_x - sql_x - 1, picSQL.Height);
        }

        private void tbSQL_Scroll(object sender, System.EventArgs e)
        {
            udSquelch.Value = -tbSQL.Value;
            if (tbSQL.Focused) btnHidden.Focus();
        }

        private void tbVOX_Scroll(object sender, System.EventArgs e)
        {
            udVOX.Value = tbVOX.Value;
            if (tbVOX.Focused) btnHidden.Focus();
        }

        private void picVOX_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int peak_x = (int)(Audio.Peak * 10 * picVOX.Width);
            int vox_x = (int)(tbVOX.Value * (picVOX.Width - 1) / 1000.0);

            if (!chkVOX.Checked) peak_x = vox_x = 0;
            e.Graphics.FillRectangle(new SolidBrush(Color.LimeGreen), 0, 0, peak_x, picVOX.Height);
            if (vox_x < peak_x)
                e.Graphics.FillRectangle(new SolidBrush(Color.Red), vox_x + 1, 0, peak_x - vox_x - 1, picVOX.Height);
        }

        private void tbNoiseGate_Scroll(object sender, System.EventArgs e)
        {
            udNoiseGate.Value = tbNoiseGate.Value;
            if (tbNoiseGate.Focused) btnHidden.Focus();
        }

        private void picNoiseGate_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int signal_x = (int)((noise_gate_data + 160.0) * (picNoiseGate.Width - 1) / 160.0);
            int noise_x = (int)(((float)tbNoiseGate.Value + 160.0) * (picNoiseGate.Width - 1) / 160.0);

            if (!chkMOX.Checked) signal_x = noise_x = 0;
            e.Graphics.FillRectangle(new SolidBrush(Color.LimeGreen), 0, 0, signal_x, picNoiseGate.Height);
            if (noise_x < signal_x)
                e.Graphics.FillRectangle(new SolidBrush(Color.Red), noise_x + 1, 0, signal_x - noise_x - 1, picNoiseGate.Height);
        }

        private void WheelTune_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ChangeWheelTuneLeft();
        }

        private void chkMON_CheckedChanged(object sender, System.EventArgs e)
        {
            Audio.MON = chkMON.Checked;

            if (chkMON.Checked)
                chkMON.BackColor = button_selected_color;
            else
                chkMON.BackColor = SystemColors.Control;

            if (num_channels == 4 || num_channels == 6)
            {
                if (!(chkMON.Checked == false && chkMOX.Checked))
                    udAF_ValueChanged(this, EventArgs.Empty);
                else
                    Audio.MonitorVolume = 0.0;
            }
        }

        private void AudioMOXChanged(bool tx)  // changes yt7pwr
        {
            Audio.RadioVolume = 0;
            Audio.MonitorVolume = 0;
            DttSP.AudioReset();
            Audio.VACRBReset = true;

            if (tx)
            {
                if (num_channels == 2 && CurrentSoundCard != SoundCard.REALTEK_HD_AUDIO)
                {
                    Mixer.SetMux(mixer_id1, mixer_tx_mux_id1);
                }
                else if (num_channels == 2)         // for Realtek HD audio
                {

                    Mixer.SetMux_RealtekHDaudio(mixer_id1, mixer_tx_mux_id1, tx);
                }

                if (!tuning)
                {
                    if (!cw_key_mode)
                    {
                        if (Audio.CurrentAudioState1 == Audio.AudioState.DTTSP ||
                            Audio.CurrentAudioState1 == Audio.AudioState.SWITCH)
                        {
                            Audio.SwitchCount = Math.Max(DttSP.bufsize * 4 / Audio.BlockSize, 3);
                            Audio.RampDown = true;
                            Audio.RampUpNum = Math.Max(DttSP.bufsize / Audio.BlockSize, 1);
                            Audio.NextMox = true;
                            Audio.NextAudioState1 = Audio.AudioState.DTTSP;
                            Audio.CurrentAudioState1 = Audio.AudioState.SWITCH;
                            DttSP.SetTRX(DttSP.TransmitState.ON);
                            Audio.Spike = true;
                        }
                    }
                    else
                    {
                        int tmp = (int)(Audio.SampleRate1 * 0.020 / Audio.BlockSize);
                        int num_switch_buffers = ((tmp > 3) ? tmp : 3);
                        Audio.MOX = tx;
                        Audio.NextMox = tx;
                        Audio.SwitchCount = num_switch_buffers;
                        Audio.NextAudioState1 = Audio.AudioState.SWITCH;
                        Audio.CurrentAudioState1 = Audio.AudioState.CW;
                        DttSP.SetTRX(DttSP.TransmitState.ON);  // does this need to be here??	
                    }
                }

                if (current_soundcard == SoundCard.AUDIGY_2 ||
                    current_soundcard == SoundCard.AUDIGY_2_ZS)
                    Mixer.SetLineInMute(mixer_id1, true);

                if (num_channels == 2)
                    Mixer.SetMainMute(mixer_id1, false);

                udAF.Value = txaf;
            }
            else // rx
            {
                if (num_channels == 2 && CurrentSoundCard != SoundCard.REALTEK_HD_AUDIO)
                {
                    Mixer.SetMux(mixer_id1, mixer_rx_mux_id1);
                }
                else
                {
                    Mixer.SetMux_RealtekHDaudio(mixer_id1, mixer_tx_mux_id1, tx);
                }

                if (!tuning)
                {
                    if (!cw_key_mode)
                    {
                        if (Audio.CurrentAudioState1 == Audio.AudioState.DTTSP ||
                            Audio.CurrentAudioState1 == Audio.AudioState.SWITCH)
                        {
                            Audio.SwitchCount = Math.Max(DttSP.bufsize * 4 / Audio.BlockSize, 3);
                            Audio.RampDown = true;
                            Audio.RampUpNum = (int)Math.Max(DttSP.bufsize / Audio.BlockSize, 1);
                            Audio.NextMox = false;
                            Audio.NextAudioState1 = Audio.AudioState.DTTSP;
                            Audio.CurrentAudioState1 = Audio.AudioState.SWITCH;
                            DttSP.SetTRX(DttSP.TransmitState.OFF);
                            Audio.Spike = true;
                        }
                    }
                    else
                    {
                        int tmp = (int)(Audio.SampleRate1 * 0.020 / Audio.BlockSize);
                        int num_switch_buffers = ((tmp > 3) ? tmp : 3);
                        Audio.MOX = tx;
                        Audio.NextMox = tx;
                        Audio.SwitchCount = num_switch_buffers;
                        Audio.NextAudioState1 = Audio.AudioState.DTTSP;
                        Audio.CurrentAudioState1 = Audio.AudioState.SWITCH;
                        DttSP.SetTRX(DttSP.TransmitState.OFF);
                    }
                }

                if (!spur_reduction &&
                    (current_dsp_mode == DSPMode.AM ||
                    current_dsp_mode == DSPMode.SAM ||
                    current_dsp_mode == DSPMode.FMN))
                {
                    DttSP.SetRXListen(0);
                    DttSP.SetOsc(0.0);
                    DttSP.SetRXListen(1);
                }

                if (current_soundcard == SoundCard.AUDIGY_2 ||
                    current_soundcard == SoundCard.AUDIGY_2_ZS)
                    Mixer.SetLineInMute(mixer_id1, false);

                if (num_channels == 2)
                    Mixer.SetMainMute(mixer_id1, false);

                udAF.Value = rxaf;
            }

            udPWR_ValueChanged(this, EventArgs.Empty);
            udAF_ValueChanged(this, EventArgs.Empty);
        }

        private void UIMOXChangedTrue()
        {
            Display.MOX = true;
            meter_peak_count = multimeter_peak_hold_samples;		// reset multimeter peak

            switch (Display.CurrentDisplayMode)
            {
                case DisplayMode.PANAFALL:
                case DisplayMode.PANADAPTER:
                case DisplayMode.WATERFALL:
                case DisplayMode.PHASE:
                case DisplayMode.PHASE2:
                case DisplayMode.SCOPE:
                case DisplayMode.SPECTRUM:
                    Display.DrawBackground();
                    break;
                default:
                    break;
            }

            comboMeterRXMode.ForeColor = Color.Gray;
            comboMeterTXMode.ForeColor = Color.Black;
//            comboMeterTXMode_SelectedIndexChanged(this, EventArgs.Empty);

            SetupForm.SpurRedEnabled = false;
            DisableAllBands();
            DisableAllModes();
            chkVFOSplit.Enabled = false;
            btnVFOAtoB.Enabled = false;
            btnVFOBtoA.Enabled = false;
            btnVFOSwap.Enabled = false;
            chkPower.BackColor = Color.Red;

            comboPreamp.Enabled = !chkMOX.Checked;
            SetupForm.MOX = chkMOX.Checked;
            ResetMultiMeterPeak();
            chkMOX.BackColor = button_selected_color;

            picSQL.Invalidate();

            Thread t = new Thread(new ThreadStart(DelayedDisplayReset));
            t.Name = "Display Reset";
            t.Priority = ThreadPriority.BelowNormal;
            t.IsBackground = true;
            t.Start();
        }

        private void UIMOXChangedFalse()
        {
            Display.MOX = false;
            switch (Display.CurrentDisplayMode)
            {
                case DisplayMode.PANAFALL:
                case DisplayMode.PANADAPTER:
                case DisplayMode.WATERFALL:
                    Display.DrawBackground();
                    break;
                default:
                    break;
            }

            SetupForm.SpurRedEnabled = true;

            EnableAllBands();
            EnableAllModes();
            chkVFOSplit.Enabled = true;
            btnVFOAtoB.Enabled = true;
            btnVFOBtoA.Enabled = true;
            btnVFOSwap.Enabled = true;
            if (chkPower.Checked) chkPower.BackColor = button_selected_color;
            comboMeterTXMode.ForeColor = Color.Gray;
            comboMeterRXMode.ForeColor = Color.Black;
//            comboMeterRXMode_SelectedIndexChanged(this, EventArgs.Empty);

            pa_fwd_power = 0;
            pa_rev_power = 0;

            Audio.HighSWRScale = 1.0;
            HighSWR = false;

            for (int i = 0; i < meter_text_history.Length; i++)
                meter_text_history[i] = 0.0f;

            comboPreamp.Enabled = !chkMOX.Checked;
            SetupForm.MOX = chkMOX.Checked;
            ResetMultiMeterPeak();
            chkMOX.BackColor = SystemColors.Control;

            picNoiseGate.Invalidate();

            Thread t = new Thread(new ThreadStart(DelayedDisplayReset));
            t.Name = "Display Reset";
            t.Priority = ThreadPriority.BelowNormal;
            t.IsBackground = true;
            t.Start();
        }

        private void chkMOX_CheckedChanged2(object sender, System.EventArgs e)  // changes yt7pwr
        {
            if (rx_only)
            {
                chkMOX.Checked = false;
                return;
            }

            bool tx = chkMOX.Checked;
            double freq = 0.0;

            if (tx)
            {
                TUNE = false;
                tx_rx = false;

                if (chkVFOSplit.Checked)
                    freq = double.Parse(txtVFOBFreq.Text);
                else
                    freq = double.Parse(txtVFOAFreq.Text);

                if (chkXIT.Checked)
                    freq += (int)udXIT.Value * 0.000001;

                if (!calibrating)
                {
                    if (!IsHamBand(current_band_plan, freq))	// out of band
                    {
                        MessageBox.Show("The frequency " + freq.ToString("f6") + "MHz is not within the " +
                            "IARU Band specifications.",
                            "Transmit Error: Out Of Band",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        chkMOX.Checked = false;
                        return;
                    }

                    if (btnBand60.BackColor == button_selected_color &&
                        current_dsp_mode != DSPMode.USB && !extended)
                    {
                        MessageBox.Show(DttSP.CurrentMode.ToString() + " mode is not allowed on 60M band.",
                            "Transmit Error: Mode/Band",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        chkMOX.Checked = false;
                        return;
                    }
                }

                AudioMOXChanged(tx);

                if (tx_IF)
                    SetTXOscFreqs(tx, true);
                else
                    SetTXOscFreqs(tx, false);

                if(Keyer.sp.IsOpen)                                 // connection to Genesis G**
                    Keyer.enable_tx(true);

                if (!usb_si570_enable)
                    g59.WriteToDevice(13, 0);                           // transmiter ON for G59
                else
                    SI570.SetTX(true);
            }
            else
            {
                TUNE = true;
                tx_rx = true;

                if (!usb_si570_enable)
                    g59.WriteToDevice(14, 0);                           // transmiter OFF
                else
                    SI570.SetTX(false);
                AudioMOXChanged(tx);

                SetTXOscFreqs(tx, false);

                if(Keyer.sp.IsOpen)
                    Keyer.enable_tx(false);

                current_ptt_mode = PTTMode.NONE;
            }

            if (tx) mox_update_thread = new Thread(new ThreadStart(UIMOXChangedTrue));
            else mox_update_thread = new Thread(new ThreadStart(UIMOXChangedFalse));
            mox_update_thread.Name = "UIMOXChanged";
            mox_update_thread.IsBackground = true;
            mox_update_thread.Priority = ThreadPriority.Normal;
            mox_update_thread.Start();
        }

        private Thread mox_update_thread;
        /*
                private void chkMOX_CheckedChanged(object sender, System.EventArgs e)
                {
                    Audio.MOX = chkMOX.Checked;
                    DttSP.AudioReset();
                    Audio.VACRBReset = true;
                    const int num_switch_buffers = 1;
                    bool cw = (current_dsp_mode == DSPMode.CWL) ||
                              (current_dsp_mode == DSPMode.CWU);

                    if (cw)
                    {
                        cw_key_mode = true;
                    }

                    if(rx_only)
                    {
                        chkMOX.Checked = false;
                        return;
                    }

                    double tx_vol = Audio.RadioVolume;
                    double rx_vol = Audio.MonitorVolume;

                    meter_peak_count = multimeter_peak_hold_samples;		// reset multimeter peak

                    if(chkMOX.Checked)
                    {
                        double freq = 0.0;
                        if(chkVFOSplit.Checked)
                            freq = double.Parse(txtVFOBFreq.Text);
                        else
                            freq = double.Parse(txtVFOAFreq.Text);

                        if(current_xvtr_index >= 0)
                            freq = XVTRForm.TranslateFreq(freq);

                        switch(current_dsp_mode)
                        {
                            case DSPMode.CWL:
                                freq += (double)cw_pitch * 0.0000010;
                                break;
                            case DSPMode.CWU:
                                freq -= (double)cw_pitch * 0.0000010;
                                break;
                        }

                        if(chkXIT.Checked)
                            freq += (int)udXIT.Value * 0.000001;

                        if(!calibrating)
                        {
                            if(!IsHamBand(current_band_plan, freq))	// out of band
                            {
                                MessageBox.Show("The frequency "+freq.ToString("f6")+"MHz is not within the "+
                                    "IARU Band specifications.",
                                    "Transmit Error: Out Of Band",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                chkMOX.Checked = false;
                                return;
                            }

                            if(btnBand60.BackColor == button_selected_color &&
                                current_dsp_mode != DSPMode.USB)
                            {
                                MessageBox.Show(DttSP.CurrentMode.ToString()+" mode is not allowed on 60M band.",
                                    "Transmit Error: Mode/Band",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                chkMOX.Checked = false;
                                return;
                            }
                        }

                        Audio.RadioVolume = 0;
                        Audio.MonitorVolume = 0;

                        if(num_channels == 2)
                        {
                            Mixer.SetMainMute(mixer_id1, true);
                        }
                        Hdw.UpdateHardware = false;

                        if(!cw_key_mode)
                        {
                            if(Audio.CurrentAudioState1 == Audio.AudioState.DTTSP ||
                                Audio.CurrentAudioState1 == Audio.AudioState.SWITCH) 
                            {
                                Audio.SwitchCount = 2048/BlockSize1*num_switch_buffers;
                                Audio.NextAudioState1 = Audio.AudioState.DTTSP;
                                Audio.CurrentAudioState1 = Audio.AudioState.SWITCH;
                                Thread.Sleep(43);
                                DttSP.SetTRX(DttSP.TransmitState.ON);
                            }
					
                            switch(Display.CurrentDisplayMode)
                            {
                                case DisplayMode.PANADAPTER:
                                case DisplayMode.SPECTRUM:
                                case DisplayMode.HISTOGRAM:
                                case DisplayMode.WATERFALL:
                                    Display.DrawBackground();
                                    break;
                            }

                            Mixer.SetMux(mixer_id1, mixer_tx_mux_id1);
                        }

                        comboMeterRXMode.ForeColor = Color.Gray;
                        comboMeterTXMode.ForeColor = Color.Black;
                        comboMeterTXMode_SelectedIndexChanged(this, EventArgs.Empty);
				
                        if(current_soundcard == SoundCard.AUDIGY_2 ||
                            current_soundcard == SoundCard.AUDIGY_2_ZS)
                            Mixer.SetLineInMute(mixer_id1, true);

                        if(current_dsp_mode == DSPMode.AM ||
                            current_dsp_mode == DSPMode.SAM ||
                            current_dsp_mode == DSPMode.FMN)
                        {
                            freq -= 0.011025;
                        }

                        chkMOX.BackColor = button_selected_color;

                        SetupForm.SpurRedEnabled = false;
                        spur_reduction = false;
                        if_shift = false;

                        DDSFreq = freq;
                        if(num_channels == 2) Hdw.MuteRelay = !chkMON.Checked;

                        if(ext_ctrl_enabled)
                        {
                            Hdw.UpdateHardware = true;
                            UpdateExtCtrl();
                            Hdw.UpdateHardware = false;
                        }

                        if(x2_enabled)
                        {
                            Hdw.UpdateHardware = true;
                            Hdw.X2 = (byte)(Hdw.X2 | 0x40);						
                            Hdw.UpdateHardware = false;
                            Thread.Sleep(x2_delay);
                        }

                        if(rfe_present)
                        {					
                            Hdw.GainRelay = true;		// 0dB
                            Hdw.Attn = false;
					
                            if(xvtr_present && freq >= 144.0)
                            {
                                Hdw.XVTR_RF = true;
                                if(current_xvtr_tr_mode == XVTRTRMode.POSITIVE)
                                    Hdw.XVTR_TR = true;
                                else if(current_xvtr_tr_mode == XVTRTRMode.NEGATIVE)
                                    Hdw.XVTR_TR = false;
                            }
                            else
                            {
                                if(current_xvtr_index < 0 || !XVTRForm.GetXVTRRF(current_xvtr_index))
                                {
                                    Hdw.RFE_TR = true;
                                    if(pa_present)
                                    {
                                        Hdw.PA_TR_Relay = true;
                                        Hdw.PABias = true;
                                    }
                                }
                                else
                                {
                                    Hdw.XVTR_RF = true;
                                }
                            }
					
                        }
                        else
                            Hdw.GainRelay = false;		// 26dB
									
                        Hdw.TransmitRelay = true;
                        Hdw.UpdateHardware = true;				
						
                        DisableAllBands();
                        DisableAllModes();
                        chkVFOSplit.Enabled = false;
                        btnVFOAtoB.Enabled = false;
                        btnVFOBtoA.Enabled = false;
                        btnVFOSwap.Enabled = false;
                        chkPower.BackColor = Color.Red;
				
                        if(cw_key_mode)
                        {
                            if(!chkTUN.Checked)
                            {
                                Audio.SwitchCount  = 2048/BlockSize1*num_switch_buffers;
                                Audio.NextAudioState1 = Audio.AudioState.SWITCH;
                                Audio.CurrentAudioState1 = Audio.AudioState.CW;
                            }
                        }

                        if(num_channels == 2)
                        {
                            Mixer.SetMainMute(mixer_id1, false);
                        }

                        udPWR_ValueChanged(this, EventArgs.Empty);
				
                        bool af_changed = ((int)udAF.Value != txaf);
                        udAF.Value = txaf;
                        if(!af_changed) udAF_ValueChanged(this, EventArgs.Empty);								
                    }
                    else
                    {    // Going from TX to RX
                        Audio.RadioVolume = 0;
                        Audio.MonitorVolume = 0;
                        Hdw.UpdateHardware = false;
                        current_ptt_mode = PTTMode.NONE;

                        if(num_channels == 2)
                        {
                            Mixer.SetMainMute(mixer_id1, true);
                        }
				
                        if(rfe_present)
                        {
                            if(xvtr_present && Hdw.XVTR_RF)
                            {
                                Hdw.XVTR_RF = false;
                                if(current_xvtr_tr_mode == XVTRTRMode.POSITIVE)
                                    Hdw.XVTR_TR = false;
                                else if(current_xvtr_tr_mode == XVTRTRMode.NEGATIVE)
                                    Hdw.XVTR_TR = true;
                            }
                            else
                            {	
                                if(current_xvtr_index < 0 || !XVTRForm.GetXVTRRF(current_xvtr_index))
                                {
                                    Hdw.RFE_TR = false;
                                    if(pa_present)
                                    {
                                        Hdw.PABias = false;
                                        Hdw.PA_TR_Relay = false;
                                    }
                                }
                                else
                                {
                                    Hdw.XVTR_RF = false;
                                }
                            }
                        }
                        Hdw.TransmitRelay = false;

                        if(x2_enabled)
                        {
                            Thread.Sleep(x2_delay);
                            Hdw.X2 = (byte)(Hdw.X2 & 0xBF);
                        }

                        if(cw_key_mode)
                        {
                            Audio.SwitchCount = num_switch_buffers;
                            Audio.NextAudioState1 = Audio.AudioState.DTTSP;
                            Audio.CurrentAudioState1 = Audio.AudioState.SWITCH;
                            Thread.Sleep(43);
                            DttSP.SetTRX(DttSP.TransmitState.OFF);
                        }
                        else
                        {
                            if(Audio.CurrentAudioState1 == Audio.AudioState.DTTSP ||
                                Audio.CurrentAudioState1 == Audio.AudioState.SWITCH)
                            {
                                Audio.SwitchCount = num_switch_buffers;
                                Audio.NextAudioState1 = Audio.AudioState.DTTSP;
                                Audio.CurrentAudioState1 = Audio.AudioState.SWITCH;
                                Thread.Sleep(43);
                                DttSP.SetTRX(DttSP.TransmitState.OFF);
                            }
					
                            switch(Display.CurrentDisplayMode)
                            {
                                case DisplayMode.PANADAPTER:
                                case DisplayMode.SPECTRUM:
                                case DisplayMode.HISTOGRAM:
                                case DisplayMode.WATERFALL:
                                    Display.DrawBackground();
                                    break;
                            }
                        }

                        comboMeterTXMode.ForeColor = Color.Gray;
                        comboMeterRXMode.ForeColor = Color.Black;
                        comboMeterRXMode_SelectedIndexChanged(this, EventArgs.Empty);

                        chkMOX.BackColor = SystemColors.Control;
                        spur_reduction = SetupForm.chkGeneralSpurRed.Checked;
                        if(!spur_reduction &&
                            (current_dsp_mode == DSPMode.AM ||
                            current_dsp_mode == DSPMode.SAM ||
                            current_dsp_mode == DSPMode.FMN))
                            DttSP.SetOsc(-11025.0);
				
                        SetupForm.SpurRedEnabled = true;
                        if(current_dsp_mode != DSPMode.DRM &&
                            current_dsp_mode != DSPMode.SPEC)
                            if_shift = true;

                        txtVFOAFreq_LostFocus(this, EventArgs.Empty);
				
                        Mixer.SetMux(mixer_id1, mixer_rx_mux_id1);
				
                        EnableAllBands();
                        EnableAllModes();

                        chkVFOSplit.Enabled = true;
                        btnVFOAtoB.Enabled = true;
                        btnVFOBtoA.Enabled = true;
                        btnVFOSwap.Enabled = true;
                        if(chkPower.Checked) chkPower.BackColor = button_selected_color;
                        if(!chkMUT.Checked && num_channels == 2)
                            Hdw.MuteRelay = false;

                        Hdw.UpdateHardware = true;

                        if(current_soundcard == SoundCard.AUDIGY_2 ||
                            current_soundcard == SoundCard.AUDIGY_2_ZS)
                            Mixer.SetLineInMute(mixer_id1, false);

                        CurrentPreampMode = current_preamp_mode;

                        if(num_channels == 2)
                        {
                            Mixer.SetMainMute(mixer_id1, false);
                        }

                        udPWR_ValueChanged(this, EventArgs.Empty);

                        bool af_changed = ((int)udAF.Value != rxaf);
                        udAF.Value = rxaf;
                        if(!af_changed) udAF_ValueChanged(this, EventArgs.Empty);

                        pa_fwd_power = 0;
                        pa_rev_power = 0;

                        Audio.HighSWRScale = 1.0;
                        HighSWR = false;

                        for(int i=0; i<meter_text_history.Length; i++)
                            meter_text_history[i] = 0.0f;
                    }	

                    comboPreamp.Enabled = !chkMOX.Checked;
                    SetupForm.MOX = chkMOX.Checked;
                    ResetMultiMeterPeak();

                    Thread t = new Thread(new ThreadStart(DelayedDisplayReset));
                    t.Name = "Display Reset";
                    t.Priority = ThreadPriority.BelowNormal;
                    t.IsBackground = true;
                    t.Start();
                }
        */
        private void chkMOX_Click(object sender, System.EventArgs e)
        {
            if (chkMOX.Checked)			// because the CheckedChanged event fires first
                manual_mox = true;
            else
            {
                manual_mox = false;
                if (chkTUN.Checked)
                    chkTUN.Checked = false;
            }
        }

        private void comboMeterRXMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboMeterRXMode.Items.Count == 0 ||
                comboMeterRXMode.SelectedIndex < 0)
            {
                current_meter_rx_mode = MeterRXMode.FIRST;
            }
            else
            {
                MeterRXMode mode = MeterRXMode.FIRST;
                switch (comboMeterRXMode.Text)
                {
                    case "Signal":
                        mode = MeterRXMode.SIGNAL_STRENGTH;
                        break;
                    case "Sig Avg":
                        multimeter_avg = Display.CLEAR_FLAG;
                        mode = MeterRXMode.SIGNAL_AVERAGE;
                        break;
                    case "ADC L":
                        mode = MeterRXMode.ADC_L;
                        break;
                    case "ADC R":
                        mode = MeterRXMode.ADC_R;
                        break;
                    case "Off":
                        mode = MeterRXMode.OFF;
                        break;
                }
                current_meter_rx_mode = mode;

                if (!chkMOX.Checked)
                {
                    switch (mode)
                    {
                        case MeterRXMode.SIGNAL_STRENGTH:
                        case MeterRXMode.SIGNAL_AVERAGE:
                            lblMultiSMeter.Text = "  1   3   5   7   9  +20 +40 +60";
                            break;
                        case MeterRXMode.ADC_L:
                        case MeterRXMode.ADC_R:
                            lblMultiSMeter.Text = "-100  -80   -60   -40   -20    0";
                            break;
                        case MeterRXMode.OFF:
                            lblMultiSMeter.Text = "";
                            break;
                    }
                    ResetMultiMeterPeak();
                }
            }

            picMultiMeterDigital.Invalidate();

            if (comboMeterRXMode.Focused)
                btnHidden.Focus();
        }

        private void comboMeterTXMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (comboMeterTXMode.Items.Count == 0 ||
                comboMeterTXMode.SelectedIndex < 0)
            {
                current_meter_tx_mode = MeterTXMode.FIRST;
            }
            else
            {
                MeterTXMode mode = MeterTXMode.FIRST;
                switch (comboMeterTXMode.Text)
                {
                    case "Fwd Pwr":
                        mode = MeterTXMode.FORWARD_POWER;
                        break;
                    case "Ref Pwr":
                        mode = MeterTXMode.REVERSE_POWER;
                        break;
                    case "Mic":
                        mode = MeterTXMode.MIC;
                        break;
                    case "EQ":
                        mode = MeterTXMode.EQ;
                        break;
                    case "Leveler":
                        mode = MeterTXMode.LEVELER;
                        break;
                    case "Lev Gain":
                        mode = MeterTXMode.LVL_G;
                        break;
                    case "COMP":
                        mode = MeterTXMode.COMP;
                        break;
                    case "CPDR":
                        mode = MeterTXMode.CPDR;
                        break;
                    case "ALC":
                        mode = MeterTXMode.ALC;
                        break;
                    case "ALC Comp":
                        mode = MeterTXMode.ALC_G;
                        break;
                    case "SWR":
                        mode = MeterTXMode.SWR;
                        break;
                    case "Off":
                        mode = MeterTXMode.OFF;
                        break;
                }
                current_meter_tx_mode = mode;
            }

            if (chkMOX.Checked)
            {
                switch (current_meter_tx_mode)
                {
                    case MeterTXMode.FIRST:
                        lblMultiSMeter.Text = "";
                        break;
                    case MeterTXMode.MIC:
                    case MeterTXMode.EQ:
                    case MeterTXMode.LEVELER:
                    case MeterTXMode.COMP:
                    case MeterTXMode.CPDR:
                    case MeterTXMode.ALC:
                        lblMultiSMeter.Text = "-20    -10     -5      0   1   2   3";
                        break;
                    case MeterTXMode.FORWARD_POWER:
                    case MeterTXMode.REVERSE_POWER:
                            lblMultiSMeter.Text = "0      1     2     5        10";
                        break;
                    case MeterTXMode.SWR:
                        lblMultiSMeter.Text = "1      1.5   2     3     5    10";
                        lblMultiSMeter.Text = "0             10              20";
                        break;
                    case MeterTXMode.OFF:
                        lblMultiSMeter.Text = "";
                        break;
                    case MeterTXMode.LVL_G:
                    case MeterTXMode.ALC_G:
                        lblMultiSMeter.Text = "0       5       10      15      20";
                        break;
                }
                ResetMultiMeterPeak();
            }

            picMultiMeterDigital.Invalidate();

            if (comboMeterTXMode.Focused)
                btnHidden.Focus();
        }

        private void chkDisplayAVG_CheckedChanged(object sender, System.EventArgs e)
        {
            Display.AverageOn = chkDisplayAVG.Checked;
            if (chkDisplayAVG.Checked)
            {
                chkDisplayAVG.BackColor = button_selected_color;
            }
            else
            {
                chkDisplayAVG.BackColor = SystemColors.Control;
            }

            if (chkDisplayAVG.Checked)
            {
                switch (Display.CurrentDisplayMode)
                {
                    case DisplayMode.PANAFALL:
                    case DisplayMode.PANADAPTER:
                    case DisplayMode.WATERFALL:
                        btnZeroBeat.Enabled = true; // only allow zerobeat when avg is on 
                        break;
                    default:
                        btnZeroBeat.Enabled = false;
                        break;
                }
            }
            else btnZeroBeat.Enabled = false;
        }

        private void chkDisplayPeak_CheckedChanged(object sender, System.EventArgs e)
        {
            Display.PeakOn = chkDisplayPeak.Checked;
            if (chkDisplayPeak.Checked)
            {
                chkDisplayPeak.BackColor = button_selected_color;
            }
            else
            {
                chkDisplayPeak.BackColor = SystemColors.Control;
            }
        }

        private void chkSquelch_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkSquelch.Checked)
            {
                chkSquelch.BackColor = button_selected_color;
                DttSP.SetSquelchState(1);
            }
            else
            {
                chkSquelch.BackColor = SystemColors.Control;
                DttSP.SetSquelchState(0);
            }
        }

        private void chkTUN_CheckedChanged(object sender, System.EventArgs e) // changes yt7pwr
        {
            if (chkTUN.Checked && TUNE)
            {
                if (!chkPower.Checked)
                {
                    MessageBox.Show("Power must be on to turn on the Tune function.",
                        "Power is off",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Hand);
                    chkTUN.Checked = false;
                    return;
                }

                tuning = true;
                chkTUN.BackColor = button_selected_color;

                Audio.SwitchCount = Math.Max(DttSP.bufsize * 4 / Audio.BlockSize, 3);
                Audio.CurrentAudioState1 = Audio.AudioState.SWITCH;
                Audio.RampDown = true;
                Audio.NextMox = true;

                switch (current_dsp_mode)
                {
                    case DSPMode.USB:
                    case DSPMode.CWU:
                    case DSPMode.DIGU:
                        if (TX_IQ_channel_swap)
                            Audio.NextAudioState1 = Audio.AudioState.COSL_SINR;
                        else
                            Audio.NextAudioState1 = Audio.AudioState.SINL_COSR;
                        break;
                    case DSPMode.LSB:
                    case DSPMode.CWL:
                    case DSPMode.DIGL:
                        if (TX_IQ_channel_swap)
                            Audio.NextAudioState1 = Audio.AudioState.COSL_SINR;
                        else
                            Audio.NextAudioState1 = Audio.AudioState.SINL_COSR;
                        break;
                    case DSPMode.DSB:
                        if (TX_IQ_channel_swap)
                            Audio.NextAudioState1 = Audio.AudioState.SINL_COSR;
                        else
                            Audio.NextAudioState1 = Audio.AudioState.COSL_SINR;
                        break;
                    case DSPMode.AM:
                    case DSPMode.SAM:
                    case DSPMode.FMN:
                        if (TX_IQ_channel_swap)
                            Audio.NextAudioState1 = Audio.AudioState.SINL_COSR;
                        else
                            Audio.NextAudioState1 = Audio.AudioState.COSL_SINR;
                        break;
                }
                PreviousPWR = (int)udPWR.Value;
                if (current_dsp_mode == DSPMode.FMN)
                    udPWR.Value = tune_power / 4;
                else udPWR.Value = tune_power;

                chkMOX.Checked = true;
                if (!chkMOX.Checked)
                {
                    chkTUN.Checked = false;
                    return;
                }
                return;
            }
            else if (chkTUN.Checked && !TUNE)
                chkTUN.Checked = false;

            else if (!chkTUN.Checked && !TUNE)
            {
                Audio.NextAudioState1 = Audio.AudioState.DTTSP;
                Audio.SwitchCount = Math.Max(DttSP.bufsize * 4 / Audio.BlockSize, 3);
                Audio.CurrentAudioState1 = Audio.AudioState.SWITCH;
                Audio.NextMox = false;
                Audio.RampDown = true;

                chkMOX.Checked = false;
                chkTUN.BackColor = SystemColors.Control;
                tuning = false;

                if (!(current_band != Band.B2M))
                {
                    if (current_dsp_mode == DSPMode.FMN)
                        TunePower = (int)udPWR.Value * 4;
                    else TunePower = (int)udPWR.Value;
                }
                udPWR.Value = PreviousPWR;
            }
            TUNE = true;
        }

        public void SetTXOscFreqs(bool tx, bool losc) // yt7pwr
        {
            if (!vfoA_drag || !spectrum_drag)
            {
                double freq = 0.0;
                double tmpFreq = 0.0;

                if (chkVFOSplit.Checked)
                {
                    freq = double.Parse(txtVFOBFreq.Text);
                }
                else
                {
                    freq = double.Parse(txtVFOAFreq.Text);
                }

                if (tx)
                {
                    switch (DttSP.CurrentMode)
                    {
                        case DttSP.Mode.CWU:
                            {
                                if (tx_IF)
                                {
                                    if (chkXIT.Checked)
                                        TX_IF_shift += XITValue;
                                    if (TX_IQ_channel_swap)
                                        DttSP.SetTXOsc(TX_IF_shift * 1e5);
                                    else
                                        DttSP.SetTXOsc(-TX_IF_shift * 1e5);

                                    DttSP.SetKeyerFreq((float)(TX_IF_shift * 1e5));
                                    Audio.SineFreq1 = (float)(TX_IF_shift * 1e5);
                                }
                                else
                                {
                                    tmpFreq = (freq * 1e6 + cw_pitch);
                                    if (tmpFreq >= (LOSCFreq * 1e6))
                                    {
                                        tmpFreq = (freq - LOSCFreq) * 1e6;
                                        if (chkXIT.Checked)
                                            tmpFreq += XITValue;
                                        if (TX_IQ_channel_swap)
                                            DttSP.SetTXOsc(-tmpFreq);
                                        else
                                            DttSP.SetTXOsc(tmpFreq);
                                    }
                                    else
                                    {
                                        tmpFreq = (freq - LOSCFreq) * 1e6;
                                        if (chkXIT.Checked)
                                            tmpFreq += XITValue;
                                        if (TX_IQ_channel_swap)
                                            DttSP.SetTXOsc(tmpFreq);
                                        else
                                            DttSP.SetTXOsc(-tmpFreq);
                                    }

                                    DttSP.SetKeyerFreq((float)tmpFreq);
                                    Audio.SineFreq1 = tmpFreq;
                                }
                            }
                            break;

                        case DttSP.Mode.CWL:
                            {
                                if (tx_IF)
                                {
                                    if (chkXIT.Checked)
                                        TX_IF_shift += XITValue;
                                    if (TX_IQ_channel_swap)
                                        DttSP.SetTXOsc(TX_IF_shift * 1e5);
                                    else
                                        DttSP.SetTXOsc(-TX_IF_shift * 1e5);

                                    DttSP.SetKeyerFreq((float)(TX_IF_shift * 1e5));
                                    Audio.SineFreq1 = (float)(TX_IF_shift * 1e5);
                                }
                                else
                                {
                                    tmpFreq = (freq * 1e6 - cw_pitch);
                                    if (tmpFreq >= (LOSCFreq * 1e6))
                                    {
                                        tmpFreq = (freq - LOSCFreq) * 1e6;
                                        if (chkXIT.Checked)
                                            tmpFreq += XITValue;
                                        if (TX_IQ_channel_swap)
                                            DttSP.SetTXOsc(-tmpFreq);
                                        else
                                            DttSP.SetTXOsc(tmpFreq);
                                    }
                                    else
                                    {
                                        tmpFreq = (freq - LOSCFreq) * 1e6;
                                        if (chkXIT.Checked)
                                            tmpFreq += XITValue;
                                        if (TX_IQ_channel_swap)
                                            DttSP.SetTXOsc(tmpFreq);
                                        else
                                            DttSP.SetTXOsc(-tmpFreq);
                                    }

                                    DttSP.SetKeyerFreq((float)tmpFreq);
                                    Audio.SineFreq1 = tmpFreq;
                                }
                            }
                            break;

                        case DttSP.Mode.AM:
                        case DttSP.Mode.SAM:
                            {
                                if (tx_IF)
                                {
                                    if (chkXIT.Checked)
                                        TX_IF_shift += XITValue;
                                    if (TX_IQ_channel_swap)
                                        DttSP.SetTXOsc(-TX_IF_shift * 1e5);
                                    else
                                        DttSP.SetTXOsc(TX_IF_shift * 1e5);

                                    DttSP.SetKeyerFreq((float)(TX_IF_shift * 1e5));
                                    Audio.SineFreq1 = (float)(TX_IF_shift * 1e5);
                                }
                                else
                                {
                                    tmpFreq = (freq - LOSCFreq) * 1e6;
                                    if (chkXIT.Checked)
                                        tmpFreq += XITValue;
                                    if (TX_IQ_channel_swap)
                                        DttSP.SetTXOsc(-tmpFreq);
                                    else
                                        DttSP.SetTXOsc(tmpFreq);

                                    DttSP.SetKeyerFreq((float)tmpFreq);
                                    Audio.SineFreq1 = tmpFreq;
                                }
                            }
                            break;
                        default:
                            {
                                if (tx_IF)
                                {
                                    if (chkXIT.Checked)
                                        TX_IF_shift += XITValue;
                                    if (TX_IQ_channel_swap)
                                        DttSP.SetTXOsc(-TX_IF_shift * 1e5);
                                    else
                                        DttSP.SetTXOsc(TX_IF_shift * 1e5);

                                    DttSP.SetKeyerFreq((float)(TX_IF_shift * 1e5));
                                    Audio.SineFreq1 = (float)(TX_IF_shift * 1e5);
                                }
                                else
                                {
                                    tmpFreq = (freq - LOSCFreq) * 1e6;
                                    if (chkXIT.Checked)
                                        tmpFreq += XITValue;
                                    if (TX_IQ_channel_swap)
                                        DttSP.SetTXOsc(-tmpFreq);
                                    else
                                        DttSP.SetTXOsc(tmpFreq);

                                    DttSP.SetKeyerFreq((float)tmpFreq);
                                    Audio.SineFreq1 = tmpFreq;
                                }
                            }
                            break;
                    }

                    DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
                }
                else
                {
                    freq = double.Parse(txtLOSCFreq.Text);
                    freq *= 1000000;
                    if (usb_si570_enable)
                        SI570.Set_SI570_osc((long)freq);
                    else
                        g59.Set_frequency((long)freq);
                }

                if (losc)
                {
                    if (tx_IF)          // for fixed TX IF frequency
                    {
                        tmpFreq = freq - TX_IF_shift / 10;
                        tmpFreq *= 1000000;
                        if (usb_si570_enable)
                            SI570.Set_SI570_osc((long)tmpFreq);
                        else
                            g59.Set_frequency((long)tmpFreq);
                    }
                    else
                    {
                        freq = double.Parse(txtLOSCFreq.Text);
                        freq *= 1000000;
                        if (usb_si570_enable)
                            SI570.Set_SI570_osc((long)freq);
                        else
                            g59.Set_frequency((long)freq);
                    }
                }
            }
        }

        private void HideFocus(object sender, EventArgs e)
        {
            btnHidden.Focus();
        }

        private void chkVFOLock_CheckedChanged(object sender, System.EventArgs e)  // changes yt7pwr
        {
            VFOLock = chkVFOLock.Checked;
            if (chkVFOLock.Checked)
                chkVFOLock.BackColor = button_selected_color;
            else
                chkVFOLock.BackColor = SystemColors.Control;
        }

        private void chkVFOsinc_Click(object sender, EventArgs e)
        {
            if (chkVFOsinc.Checked && chkEnableSubRX.Checked)
            {
                chkVFOsinc.BackColor = button_selected_color;
                VFO_SINC = true;
            }
            else
            {
                chkVFOsinc.Checked = false;
                chkVFOsinc.BackColor = SystemColors.Control;
                VFO_SINC = false;
            }
        }

        private void btnBandVHF_Click(object sender, System.EventArgs e) // changes yt7pwr
        {
        }

        private void btnBandHF_Click(object sender, System.EventArgs e)
        {
            grpBandHF.Visible = true;
        }

        private void udPWR_LostFocus(object sender, EventArgs e)
        {
            udPWR_ValueChanged(sender, e);
        }

        private void udAF_LostFocus(object sender, EventArgs e)
        {
            udAF_ValueChanged(sender, e);
        }

        private void udMIC_LostFocus(object sender, EventArgs e)
        {
            udMIC_ValueChanged(sender, e);
        }

        private void udSquelch_LostFocus(object sender, EventArgs e)
        {
            udSquelch_ValueChanged(sender, e);
        }

        private void udFilterLow_LostFocus(object sender, EventArgs e)
        {
            udFilterLow_ValueChanged(sender, e);
        }

        private void udFilterHigh_LostFocus(object sender, EventArgs e)
        {
            udFilterHigh_ValueChanged(sender, e);
        }

        private void udRIT_LostFocus(object sender, EventArgs e)
        {
            udRIT_ValueChanged(sender, e);
        }

        private void udXIT_LostFocus(object sender, EventArgs e)
        {
            udXIT_ValueChanged(sender, e);
        }

        private void udCWSpeed_LostFocus(object sender, EventArgs e)
        {
            udCWSpeed_ValueChanged(sender, e);
        }

        private void tbCPDR_Scroll(object sender, System.EventArgs e)
        {
            udCPDR.Value = tbCPDR.Value;
            if (tbCPDR.Focused) btnHidden.Focus();
        }

        private void tbCOMP_Scroll(object sender, System.EventArgs e)
        {
            udCOMP.Value = tbCOMP.Value;
            if (tbCPDR.Focused) btnHidden.Focus();
        }

        private void udCOMP_ValueChanged(object sender, System.EventArgs e)
        {
            tbCOMP.Value = (int)udCOMP.Value;
            if (SetupForm != null)
                SetupForm.COMPVal = (int)udCOMP.Value;

        }

        private void udCPDR_ValueChanged(object sender, System.EventArgs e)
        {
            tbCPDR.Value = (int)udCPDR.Value;
            if (SetupForm != null)
                SetupForm.CPDRVal = (int)udCPDR.Value;
        }

        private void chkSR_CheckedChanged(object sender, System.EventArgs e)
        {
			if(SetupForm != null) SetupForm.SpurReduction = chkSR.Checked;
			if(chkEnableSubRX.Checked) txtVFOBFreq_LostFocus(this, EventArgs.Empty);
			if(chkSR.Checked) chkSR.BackColor = button_selected_color;
			else chkSR.BackColor = SystemColors.Control;
        }

        private void udNoiseGate_ValueChanged(object sender, System.EventArgs e)
        {
            tbNoiseGate.Value = (int)udNoiseGate.Value;
            if (SetupForm != null) SetupForm.NoiseGate = tbNoiseGate.Value;
        }

        private void btnChangeTuneStepSmaller_Click(object sender, System.EventArgs e)
        {
            ChangeWheelTuneRight();
        }

        private void btnChangeTuneStepLarger_Click(object sender, System.EventArgs e)
        {
            ChangeWheelTuneLeft();
        }

        private void udVOX_ValueChanged(object sender, System.EventArgs e)
        {
            tbVOX.Value = (int)udVOX.Value;
            if (SetupForm != null) SetupForm.VOXSens = tbVOX.Value;
        }

        private void comboTXProfile_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (SetupForm != null) SetupForm.TXProfile = comboTXProfile.Text;
            UpdateDisplay();

            if (comboTXProfile.Focused) btnHidden.Focus();
        }

        private void chkShowTXFilter_CheckedChanged(object sender, System.EventArgs e)
        {
            Display.DrawTXFilter = chkShowTXFilter.Checked;
        }

        private void mnuFilterReset_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "Are you sure you want to reset all custom filter settings to the default?",
                "Reset Filters?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr == DialogResult.No) return;

            InitFilterPresets();

            radFilter1.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F1);
            radFilter2.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F2);
            radFilter3.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F3);
            radFilter4.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F4);
            radFilter5.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F5);
            radFilter6.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F6);
            radFilter7.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F7);
            radFilter8.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F8);
            radFilter9.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F9);
            radFilter10.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.F10);
            radFilterVar1.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.VAR1);
            radFilterVar2.Text = filter_presets[(int)current_dsp_mode].GetName(Filter.VAR2);
            CurrentFilter = current_filter;

            if (filterForm != null && !filterForm.IsDisposed)
            {
                filterForm.CurrentDSPMode = current_dsp_mode;
            }
        }

        private void chkVACStereo_CheckedChanged(object sender, System.EventArgs e)
        {
            if (SetupForm != null) SetupForm.VACStereo = chkVACStereo.Checked;
        }

        private void chkCWVAC_CheckedChanged(object sender, System.EventArgs e)
        {
            if (SetupForm != null) SetupForm.VACEnable = chkCWVAC.Checked;
            if (chkCWVAC.Checked) chkCWVAC.BackColor = button_selected_color;
            else chkCWVAC.BackColor = SystemColors.Control;
        }

        private void chkCWIambic_CheckedChanged(object sender, System.EventArgs e)
        {
            if (SetupForm != null) SetupForm.CWIambic = chkCWIambic.Checked;
        }

        private void chkCWDisableMonitor_CheckedChanged(object sender, System.EventArgs e)
        {
            if (SetupForm != null) SetupForm.CWDisableMonitor = chkCWDisableMonitor.Checked;
        }

        private void udCWPitch_ValueChanged(object sender, System.EventArgs e)
        {
            if (SetupForm != null) SetupForm.CWPitch = (int)udCWPitch.Value;
            if (udCWPitch.Focused) btnHidden.Focus();
        }

        private void comboVACSampleRate_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (SetupForm != null) SetupForm.VACSampleRate = comboVACSampleRate.Text;
            if (comboVACSampleRate.Focused) btnHidden.Focus();
        }

        private bool RX_RL_swap = false; // yt7pwr
        public bool RX_IQ_channel_swap
        {
            get { return RX_RL_swap; }
            set { RX_RL_swap = value; }
        }

        private bool TX_RL_swap = false; // yt7pwr
        public bool TX_IQ_channel_swap
        {
            get { return TX_RL_swap; }
            set { TX_RL_swap = value; }
        }

        private void chkShowTXCWFreq_CheckedChanged(object sender, System.EventArgs e)
        {
            Display.DrawTXCWFreq = chkShowTXCWFreq.Checked;
        }

        #endregion

        #region VFO Events

        private void Console_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e) // changes yt7pwr
        {
            if (this.ActiveControl is TextBoxTS ||
                this.ActiveControl is NumericUpDownTS ||
                this.ActiveControl is TrackBarTS)
            {
                Console_KeyPress(this, new KeyPressEventArgs((char)Keys.Enter));
                return;
            }
            int numberToMove = e.Delta / 120;	// 1 per click

            if (vfo_char_width == 0)
                GetVFOCharWidth();

            if (numberToMove != 0)
            {
                int left, right, top, bottom;
                left = grpVFOA.Left + txtVFOAFreq.Left;
                right = left + txtVFOAFreq.Width;
                top = grpVFOA.Top + txtVFOAFreq.Top;
                bottom = top + txtVFOAFreq.Height;
                double losc_freq = double.Parse(txtLOSCFreq.Text);

                if (e.X > left && e.X < right &&			// Update VFOA
                    e.Y > top && e.Y < bottom)
                {
                    if (chkPower.Checked)
                    {
                        double freq = double.Parse(txtVFOAFreq.Text);
                        double mult = 1000.0;
                        if (vfoa_hover_digit < 0)
                        {
                            int x = right + 2 - (vfo_pixel_offset - 5);
                            while (x < e.X && mult > 0.0000011)
                            {
                                mult /= 10.0;
                                x += vfo_char_width;
                                if (mult == 1.0)
                                    x += vfo_decimal_space;
                                else x += vfo_char_space;
                            }
                        }
                        else
                        {
                            mult = Math.Pow(10, -vfoa_hover_digit) * 1000.0;
                        }

                        if (mult <= 1.0)
                        {
                            freq += mult * numberToMove;
                            MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
                            MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;
                            if (freq > max_freq)
                                freq = max_freq;
                            else if (freq < min_freq)
                                freq = min_freq;

                            if (!vfo_lock)
                            {
                                VFOAFreq = freq;
                                UpdateVFOAFreq(freq.ToString("f6"));
                                Display.VFOA = (long)(freq * 1e6);
                            }
                            if (vfo_sinc && chkEnableSubRX.Checked && 
                                (freq >= min_freq || freq <= max_freq))
                            {
                                freq = double.Parse(txtVFOBFreq.Text);
                                freq += mult * numberToMove;
                                if (freq > max_freq)
                                    freq = max_freq;
                                else if (freq < min_freq)
                                    freq = min_freq;

                                VFOBFreq = freq;
                                UpdateVFOBFreq(freq.ToString("f6"));
                                Display.VFOB = (long)(freq * 1e6);
                            }
                        }
                    }
                    return;
                }
                else
                {
                    left = grpVFOB.Left + txtVFOBFreq.Left;
                    right = left + txtVFOBFreq.Width;
                    top = grpVFOB.Top + txtVFOBFreq.Top;
                    bottom = top + txtVFOBFreq.Height;
                    if (e.X > left && e.X < right &&		// Update VFOB
                        e.Y > top && e.Y < bottom)
                    {
                        if (chkPower.Checked)
                        {
                            double freq = double.Parse(txtVFOBFreq.Text);
                            double mult = 1000.0;
                            if (vfob_hover_digit < 0)
                            {
                                int x = right + 2 - (vfo_pixel_offset - 5);
                                while (x < e.X && mult > 0.0000011)
                                {
                                    mult /= 10;
                                    x += vfo_char_width;
                                    if (mult == 1.0)
                                        x += vfo_decimal_space;
                                    else x += vfo_char_space;
                                }
                            }
                            else
                            {
                                mult = Math.Pow(10, -vfob_hover_digit) * 1000.0;
                            }

                            if (mult <= 1.0)
                            {
                                freq += mult * numberToMove;
                                MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
                                MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;
                                if (freq > max_freq)
                                    freq = max_freq;
                                else if (freq < min_freq)
                                    freq = min_freq;

                                if (!vfo_lock)
                                {
                                    VFOBFreq = freq;
                                    UpdateVFOBFreq(freq.ToString("f6"));
                                    Display.VFOB = (long)(freq * 1e6);
                                }
                            }
                        }
                        return;
                    }

                    else
                    {
                        left = grpVFOBetween.Left + lblMemoryNumber.Left;
                        right = left + lblMemoryNumber.Width;
                        top = grpVFOBetween.Top + lblMemoryNumber.Top;
                        bottom = top + lblMemoryNumber.Height;
                        if (e.X > left && e.X < right &&		// Update memory number
                            e.Y > top && e.Y < bottom)
                        {
                            if (numberToMove > 0)
                            {
                                if (MemoryNumber < 99)
                                    MemoryNumber++;
                                else if (MemoryNumber == 99)
                                    MemoryNumber = 1;
                            }
                            else if (numberToMove < 0)
                            {
                                if (MemoryNumber > 1)
                                    MemoryNumber--;
                                else if (MemoryNumber == 1)
                                    MemoryNumber = 99;
                            }
                            txtMemory_fill();
                            return;
                        }

                        left = grpLOSC.Left + txtLOSCFreq.Left;
                        right = left + txtLOSCFreq.Width;
                        top = grpLOSC.Top + txtLOSCFreq.Top;
                        bottom = top + txtLOSCFreq.Height;
                        if (e.X > left && e.X < right &&		// Update LOSC
                            e.Y > top && e.Y < bottom)
                        {
                            if (chkPower.Checked)
                            {
                                double freq = LOSCFreq;   // double.Parse(txtLOSCFreq.Text);
                                double freq_vfoB = VFOBFreq;
                                double mult = 1000.0;
                                if (losc_hover_digit < 0)
                                {
                                    int x = right + 2 - (losc_pixel_offset - 5);
                                    while (x < e.X && mult > 0.0000011)
                                    {
                                        mult /= 10;
                                        x += losc_char_width;
                                        if (mult == 1.0)
                                            x += losc_decimal_space;
                                        else x += losc_char_space;
                                    }
                                }
                                else
                                {
                                    mult = Math.Pow(10, -losc_hover_digit) * 1000.0;
                                }

                                if (mult <= 1.0)
                                {
                                    freq += mult * numberToMove;
                                    if (!(freq < 1.0))
                                    {
                                        if (!vfo_lock)
                                        {
                                            freq = Math.Round(freq, 6);
                                            if (!vfo_lock)
                                            {
                                                LOSCFreq = freq;
                                                UpdateLOSCFreq(freq.ToString("f6"));
                                                Display.LOSC = (long)(freq * 1e6);
                                                txtVFOAFreq_LostFocus(sender, e);
                                                if (chkEnableSubRX.Checked)
                                                    txtVFOBFreq_LostFocus(sender, e);
                                            }
                                        }
                                    }
                                    else
                                        MessageBox.Show("Error!", "1MHz is minimum frequency!");
                                }
                            }
                            return;
                        }

                        else
                        {
                            double freqA = 0.0f;
                            double freqB = 0.0f;

                                freqA = Double.Parse(txtVFOAFreq.Text);
                                freqB = Double.Parse(txtVFOBFreq.Text);

                                MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
                                MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;

                            double mult = wheel_tune_list[wheel_tune_index];
                            if (shift_down && mult >= 0.000009) mult /= 10;


                            if (mult == 0.009)
                            {
                                freqA = freqA + numberToMove * mult;
                                freqB = freqB + numberToMove * mult;
                            }
                            else
                            {
                                if (numberToMove > 0)
                                {
                                    double tempA = freqA;
                                    double tempB = freqB;
                                    numberToMove += 1;
                                    freqA = Math.Floor(freqA / mult);
                                    freqB = Math.Floor(freqB / mult);
                                    if ((double)Math.Round(freqA * mult, 6) == tempA) freqA -= 1.0;
                                    if ((double)Math.Round(freqB * mult, 6) == tempB) freqB -= 1.0;
                                    freqA = (freqA + numberToMove) * mult;
                                    freqB = (freqB + numberToMove) * mult;
                                }
                                else
                                {
                                    double tempA = freqA;
                                    double tempB = freqB;
                                    if (numberToMove < 0) numberToMove -= 1;
                                    freqA = Math.Floor((freqA / mult) + 1.0);
                                    freqB = Math.Floor((freqB / mult) + 1.0);
                                    if ((double)Math.Round(freqA * mult, 6) == tempA) freqA += 1.0;
                                    if ((double)Math.Round(freqB * mult, 6) == tempB) freqB += 1.0;
                                    freqA = (freqA + numberToMove) * mult;
                                    freqB = (freqB + numberToMove) * mult;
                                }
                            }
                            if (current_click_tune_mode == ClickTuneMode.VFOA)
                            {
                                if (freqA > MaxFreq)
                                    freqA = max_freq;
                                else if (freqA < MinFreq)
                                    freqA = min_freq;

                                if (!vfo_lock)
                                {
                                    VFOAFreq = freqA;
                                    UpdateVFOAFreq(freqA.ToString("f6"));
                                    Display.VFOA = (long)(freqA * 1e6);
                                }
                                if (vfo_sinc && chkEnableSubRX.Checked)
                                {
                                    if (freqB > MaxFreq)
                                        freqB = max_freq;
                                    else if (freqB < MinFreq)
                                        freqB = min_freq;

                                    VFOBFreq = freqB;
                                    UpdateVFOBFreq(freqB.ToString("f6"));
                                    Display.VFOB = (long)(freqB * 1e6);
                                }
                            }
                            else if (current_click_tune_mode == ClickTuneMode.VFOB)
                            {
                                if (freqB > MaxFreq)
                                    freqB = max_freq;
                                else if (freqB < MinFreq)
                                    freqB = min_freq;

                                if (!vfo_lock)
                                {
                                    VFOBFreq = freqB;
                                    UpdateVFOBFreq(freqB.ToString("f6"));
                                    Display.VFOB = (long)(freqB * 1e6);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void txtVFOAFreq_LostFocus(object sender, System.EventArgs e) // changes yt7pwr
        {
            if (txtVFOAFreq.Text == "." || txtVFOAFreq.Text == "")
            {
                VFOAFreq = saved_vfoa_freq;
                return;
            }

            double freq = double.Parse(txtVFOAFreq.Text);

            double rx_osc = ((LOSCFreq - VFOAFreq) * 1e6);

            if (rx_osc < -sample_rate1 / 2)
            {
                first = true;
                VFOAFreq = LOSCFreq + (sample_rate1 / 2 + DttSP.RXOsc - 1) * 1e-6;
                return;
            }
            else if (rx_osc > sample_rate1 / 2)
            {
                first = true;
                VFOAFreq = LOSCFreq + (-sample_rate1 / 2 + DttSP.RXOsc + 1) * 1e-6;
                return;
            }


            UpdateVFOAFreq(freq.ToString("f6"));
            Display.VFOA = (long)(freq * 1e6);

            // update Band Info
            string bandInfo;
            bool transmit_allowed = DB.BandText(freq, out bandInfo);
            if (!transmit_allowed)
            {
                txtVFOABand.BackColor = out_of_band_color;
                if (!chkVFOSplit.Checked && chkMOX.Checked)
                    chkMOX.Checked = false;
            }
            else txtVFOABand.BackColor = band_background_color;
            txtVFOABand.Text = bandInfo;

            if (!chkMOX.Checked || (chkMOX.Checked && !chkVFOSplit.Checked))
            {
                SetCurrentBand(BandByFreq((float)freq));
                SetBandButtonColor(current_band);
            }

            if (chkPower.Checked && ext_ctrl_enabled)
                UpdateExtCtrl();

            if (CurrentBand == Band.B60M)
            {
                chkXIT.Enabled = false;
                chkXIT.Checked = false;
            }
            else
                chkXIT.Enabled = true;

            saved_vfoa_freq = (float)freq;
            if (freq < min_freq)
            {
                UpdateVFOAFreq(freq.ToString("f6"));
            }
            else if (freq > max_freq)
            {
                //freq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
                UpdateVFOAFreq(freq.ToString("f6"));
            }

            if (chkMOX.Checked &&
                (CurrentDSPMode == DSPMode.AM ||
                CurrentDSPMode == DSPMode.SAM ||
                CurrentDSPMode == DSPMode.FMN))
                freq -= 0.011025;

            if (current_dsp_mode == DSPMode.CWL)
                freq += (double)cw_pitch * 0.0000010;
            else if (current_dsp_mode == DSPMode.CWU)
                freq -= (double)cw_pitch * 0.0000010;

            if (freq < min_freq) freq = min_freq;
            else if (freq > max_freq) freq = max_freq;

            if (chkRIT.Checked && !chkMOX.Checked && !chkVFOSplit.Checked)
                freq += (int)udRIT.Value * 0.000001;
            else if (chkXIT.Checked && chkMOX.Checked && !chkVFOSplit.Checked)
                freq += (int)udXIT.Value * 0.000001;

            if (freq < min_freq) freq = min_freq;
            else if (freq > max_freq) freq = max_freq;

            if (chkPower.Checked)
            {
                double osc_freq = ((LOSCFreq - VFOAFreq) * 1e6);
                if (current_dsp_mode == DSPMode.CWL)
                    osc_freq -= (double)cw_pitch;
                else if (current_dsp_mode == DSPMode.CWU)
                    osc_freq += (double)cw_pitch;
                if (chkRIT.Checked)
                    osc_freq += RITValue;

                switch (current_model)
                {
                    case Model.GENESIS_G59:
                    case Model.GENESIS_G160:
                    case Model.GENESIS_G3020:
                    case Model.GENESIS_G40:
                    case Model.GENESIS_G80:
                        //!!!!drm patch
                        if (current_dsp_mode == DSPMode.DRM) // if we're in DRM mode we need to be offset 12khz
                        {
                            osc_freq = osc_freq + 12000;
                        }
                        tuned_freq = freq;

                        if(!chkMOX.Checked)
                        {
                            DttSP.SetRXListen(1);
                            DttSP.SetOscDll(osc_freq);
                        }
                        if (chkMOX.Checked && !chkVFOSplit.Checked)
                        {
                            if (!IsHamBand(current_band_plan, freq))
                            {
                                MessageBox.Show("The frequency " + freq.ToString("f6") + "MHz is not within the " +
                                    "IARU Band specifications.",
                                    "Transmit Error: Out Of Band",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                chkMOX.Checked = false;
                                return;
                            }
                        }
                        break;
                }
            }
            
            if (small_lsd)
            {
                txtVFOAMSD.Visible = true;
                txtVFOALSD.Visible = true;
            }

            if (Display.PeakOn) Display.ResetDisplayPeak();

            if (MOX)
                SetTXOscFreqs(true,true);
            else
                SetTXOscFreqs(true,false);
        }

        private static double tuned_freq;
        private void txtVFOAFreq_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            int KeyCode = (int)e.KeyChar;
            if ((KeyCode < 48 || KeyCode > 57) &&			// numeric keys
                KeyCode != 8 &&								// backspace
                !e.KeyChar.ToString().Equals(separator) &&	// decimal
                KeyCode != 27)								// escape
            {
                e.Handled = true;
            }
            else
            {
                if (e.KeyChar.ToString().Equals(separator))
                {
                    e.Handled = (((TextBoxTS)sender).Text.IndexOf(separator) >= 0);
                }
                else if (KeyCode == 27)
                {
                    VFOAFreq = saved_vfoa_freq;
                    btnHidden.Focus();
                }
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtVFOAFreq_LostFocus(txtVFOAFreq, new System.EventArgs());
                btnHidden.Focus();
            }
        }

        private void txtVFOAFreq_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.ContainsFocus)
            {
                int old_digit = vfoa_hover_digit;
                int digit_index = 0;
                if (vfo_char_width == 0)
                    GetVFOCharWidth();

                int x = txtVFOAFreq.Width - (vfo_pixel_offset - 5);
                while (x < e.X)
                {
                    digit_index++;

                    if (small_lsd && txtVFOALSD.Visible)
                    {
                        if (digit_index < 6)
                            x += (vfo_char_width + vfo_char_space);
                        else
                            x += (vfo_small_char_width + vfo_small_char_space);

                        if (digit_index == 3)
                            x += (vfo_decimal_space - vfo_char_space);
                        if (digit_index == 6)
                            x += vfo_small_char_width;
                    }
                    else
                    {
                        x += vfo_char_width;
                        if (digit_index == 3)
                            x += vfo_decimal_space;
                        else
                            x += vfo_char_space;
                    }
                }

                if (digit_index < 3) digit_index = -1;
                if (digit_index > 9) digit_index = 9;
                vfoa_hover_digit = digit_index;
                if (vfoa_hover_digit != old_digit)
                    panelVFOAHover.Invalidate();
            }
        }

        private void txtVFOAFreq_MouseLeave(object sender, System.EventArgs e)
        {
            vfoa_hover_digit = -1;
            panelVFOAHover.Invalidate();
        }

        // txtVFOBFreq
        private void txtVFOBFreq_LostFocus(object sender, System.EventArgs e)  // changes yt7pwr
        {
            if (txtVFOBFreq.Text == "" || txtVFOBFreq.Text == ".")
            {
                VFOBFreq = saved_vfob_freq;
                return;
            }

            double freq = double.Parse(txtVFOBFreq.Text);

            double rx2_osc = ((LOSCFreq - VFOBFreq) * 1e6);

            if (chkEnableSubRX.Checked)
            {
                if (rx2_osc < -sample_rate1 / 2)
                {
                    first = true;
                    VFOBFreq = LOSCFreq +(sample_rate1 / 2 + DttSP.RXOsc - 1) * 1e-6;
                    return;
                }
                else if (rx2_osc > sample_rate1 / 2)
                {
                    first = true;
                    VFOBFreq = LOSCFreq +(-sample_rate1 / 2 + DttSP.RXOsc + 1) * 1e-6;
                    return;
                }
            }

            if (chkEnableSubRX.Checked &&
                (rx2_osc > -sample_rate1 / 2 && rx2_osc < sample_rate1 / 2))
            {
                if (current_dsp_mode == DSPMode.CWL)
                    rx2_osc -= (double)cw_pitch;
                else if (current_dsp_mode == DSPMode.CWU)
                    rx2_osc += (double)cw_pitch;
                DttSP.SetRXListen(2);
                DttSP.SetOscDll(rx2_osc);
                DttSP.SetRXListen(1);
            }

            UpdateVFOBFreq(freq.ToString("f6"));
            Display.VFOB = (long)(freq * 1e6);

            // update Band Info
            string bandInfo;
            bool transmit = DB.BandText(freq, out bandInfo);
            if (transmit == false)
            {
                txtVFOBBand.BackColor = Color.DimGray;
                if (chkVFOSplit.Checked && chkMOX.Checked)
                    chkMOX.Checked = false;
            }
            else txtVFOBBand.BackColor = band_background_color;
            txtVFOBBand.Text = bandInfo;

            saved_vfob_freq = (float)freq;

            if (chkPower.Checked && chkMOX.Checked && chkVFOSplit.Checked)
            {
                SetCurrentBand(BandByFreq((float)freq));
                SetBandButtonColor(current_band);

                if (ext_ctrl_enabled)
                    UpdateExtCtrl();

                if (CurrentBand == Band.B60M)
                {
                    chkXIT.Enabled = false;
                    chkXIT.Checked = false;
                }
                else
                    chkXIT.Enabled = true;

                if (chkMOX.Checked &&
                    (CurrentDSPMode == DSPMode.AM ||
                    CurrentDSPMode == DSPMode.SAM ||
                    CurrentDSPMode == DSPMode.FMN))
                    freq -= 0.011025;

                if (current_dsp_mode == DSPMode.CWL)
                    freq += (double)cw_pitch * 0.0000010;
                else if (current_dsp_mode == DSPMode.CWU)
                    freq -= (double)cw_pitch * 0.0000010;

                if (chkXIT.Checked)
                    freq += (int)udXIT.Value * 0.000001;

                if (freq < min_freq) freq = min_freq;
                else if (freq > max_freq) freq = max_freq;

                if (!IsHamBand(current_band_plan, freq))	// out of band
                {
                    MessageBox.Show("The frequency " + freq.ToString("f6") + "MHz is not within the " +
                        "IARU Band specifications.",
                        "Transmit Error: Out Of Band",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    chkMOX.Checked = false;
                    return;
                }
            }

            if (small_lsd)
            {
                txtVFOBMSD.Visible = true;
                txtVFOBLSD.Visible = true;
            }
        }

        private void txtVFOBFreq_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            int KeyCode = (int)e.KeyChar;
            if ((KeyCode < 48 || KeyCode > 57) &&			// numeric keys
                KeyCode != 8 &&								// backspace
                !e.KeyChar.ToString().Equals(separator) &&	// decimal
                KeyCode != 27)								// escape
            {
                e.Handled = true;
            }
            else
            {
                if (e.KeyChar.ToString().Equals(separator))
                {
                    e.Handled = (((TextBoxTS)sender).Text.IndexOf(separator) >= 0);
                }
                else if (KeyCode == 27)
                {
                    VFOBFreq = saved_vfob_freq;
                    btnHidden.Focus();
                }
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtVFOAFreq_LostFocus(txtVFOAFreq, new System.EventArgs());
                btnHidden.Focus();
            }
        }

        private void txtVFOBFreq_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.ContainsFocus)
            {
                int old_digit = vfob_hover_digit;
                int digit_index = 0;
                if (vfo_char_width == 0)
                    GetVFOCharWidth();

                int x = txtVFOBFreq.Width - (vfo_pixel_offset - 5);
                while (x < e.X)
                {
                    digit_index++;

                    if (small_lsd && txtVFOBLSD.Visible)
                    {
                        if (digit_index < 6)
                            x += (vfo_char_width + vfo_char_space);
                        else
                            x += (vfo_small_char_width + vfo_small_char_space);

                        if (digit_index == 3)
                            x += (vfo_decimal_space - vfo_char_space);
                        if (digit_index == 6)
                            x += vfo_small_char_width;
                    }
                    else
                    {
                        x += vfo_char_width;
                        if (digit_index == 3)
                            x += vfo_decimal_space;
                        else
                            x += vfo_char_space;
                    }
                }

                if (digit_index < 3) digit_index = -1;
                if (digit_index > 9) digit_index = 9;
                vfob_hover_digit = digit_index;
                if (vfob_hover_digit != old_digit)
                    panelVFOBHover.Invalidate();
            }
        }

        private void txtVFOBFreq_MouseLeave(object sender, System.EventArgs e)
        {
            vfob_hover_digit = -1;
            panelVFOBHover.Invalidate();
        }

        private void panelVFOAHover_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Control c1 = (Control)sender;
            Control c2 = txtVFOAFreq;
            int client_width = (c1.Size.Width - c1.ClientSize.Width) + (c2.Size.Width - c2.ClientSize.Width);
            int client_height = (c1.Size.Height - c1.ClientSize.Height) + (c2.Size.Height - c2.ClientSize.Height);
            int x_offset = c1.Left - c2.Left - client_width / 2;
            int y_offset = c1.Top - c2.Top - client_height / 2;
            txtVFOAFreq_MouseMove(sender, new MouseEventArgs(e.Button, e.Clicks, e.X + x_offset, e.Y + y_offset, e.Delta));

            /*txtVFOAFreq_MouseMove(sender, new MouseEventArgs(MouseButtons.None, 0,
                e.X+panelVFOAHover.Left-10, e.Y+panelVFOAHover.Top, 0));*/
        }

        private void panelVFOBHover_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Control c1 = (Control)sender;
            Control c2 = txtVFOBFreq;
            int client_width = (c1.Size.Width - c1.ClientSize.Width) + (c2.Size.Width - c2.ClientSize.Width);
            int client_height = (c1.Size.Height - c1.ClientSize.Height) + (c2.Size.Height - c2.ClientSize.Height);
            int x_offset = c1.Left - c2.Left - client_width / 2;
            int y_offset = c1.Top - c2.Top - client_height / 2;
            txtVFOBFreq_MouseMove(sender, new MouseEventArgs(e.Button, e.Clicks, e.X + x_offset, e.Y + y_offset, e.Delta));

            /*txtVFOBFreq_MouseMove(sender, new MouseEventArgs(MouseButtons.None, 0,
                e.X+panelVFOBHover.Left-10, e.Y+panelVFOBHover.Top, 0));*/
        }

        private void txtVFOALSD_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            txtVFOAMSD.Visible = false;
            txtVFOALSD.Visible = false;
            txtVFOAFreq.Focus();
            txtVFOAFreq.SelectAll();
        }

        private void txtVFOALSD_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Control c1 = (Control)sender;
            Control c2 = txtVFOAFreq;
            int client_width = (c1.Size.Width - c1.ClientSize.Width) + (c2.Size.Width - c2.ClientSize.Width);
            int client_height = (c1.Size.Height - c1.ClientSize.Height) + (c2.Size.Height - c2.ClientSize.Height);
            int x_offset = c1.Left - c2.Left - client_width / 2;
            int y_offset = c1.Top - c2.Top - client_height / 2;
            txtVFOAFreq_MouseMove(sender, new MouseEventArgs(e.Button, e.Clicks, e.X + x_offset, e.Y + y_offset, e.Delta));

            /*txtVFOAFreq_MouseMove(txtVFOALSD,
                new MouseEventArgs(e.Button, e.Clicks, e.X+165, e.Y+25, e.Delta));*/
        }

        private void txtVFOAMSD_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            txtVFOAMSD.Visible = false;
            txtVFOALSD.Visible = false;
            txtVFOAFreq.Focus();
            txtVFOAFreq.SelectAll();
        }

        private void txtVFOAMSD_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            txtVFOAFreq_MouseMove(txtVFOAMSD,
                new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
        }


        private void txtVFOAMSD_MouseLeave(object sender, System.EventArgs e)
        {
            txtVFOAFreq_MouseLeave(txtVFOAMSD, e);
        }

        private void txtVFOBMSD_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            txtVFOBMSD.Visible = false;
            txtVFOBLSD.Visible = false;
            txtVFOBFreq.Focus();
            txtVFOBFreq.SelectAll();
        }

        private void txtVFOBMSD_MouseLeave(object sender, System.EventArgs e)
        {
            txtVFOBFreq_MouseLeave(txtVFOBMSD, e);
        }

        private void txtVFOBMSD_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            txtVFOBFreq_MouseMove(txtVFOBMSD,
                new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
        }

        private void txtVFOBLSD_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            txtVFOBMSD.Visible = false;
            txtVFOBLSD.Visible = false;
            txtVFOBFreq.Focus();
            txtVFOBFreq.SelectAll();
        }

        private void txtVFOBLSD_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Control c1 = (Control)sender;
            Control c2 = txtVFOBFreq;
            int client_width = (c1.Size.Width - c1.ClientSize.Width) + (c2.Size.Width - c2.ClientSize.Width);
            int client_height = (c1.Size.Height - c1.ClientSize.Height) + (c2.Size.Height - c2.ClientSize.Height);
            int x_offset = c1.Left - c2.Left - client_width / 2;
            int y_offset = c1.Top - c2.Top - client_height / 2;
            txtVFOBFreq_MouseMove(sender, new MouseEventArgs(e.Button, e.Clicks, e.X + x_offset, e.Y + y_offset, e.Delta));

            /*txtVFOBFreq_MouseMove(txtVFOBLSD,
                new MouseEventArgs(e.Button, e.Clicks, e.X+165, e.Y+25, e.Delta));*/
        }

        #endregion

        #region Display Events

        private bool low_filter_drag = false;
        private bool high_filter_drag = false;
        private bool whole_filter_drag = false;
        private int whole_filter_start_x = 0;
        private int whole_filter_start_low = 0;
        private int whole_filter_start_high = 0;
        private bool spectrum_drag = false;
        private int spectrum_drag_last_x = 0;
        private int vfob_drag_last_x = 0;
        private int vfoA_drag_last_x = 0;
        private double vfob_drag_start_freq = 0.0;
        private double vfoA_drag_start_freq = 0.0;
        private bool vfob_drag = false;
        private bool vfoA_drag = false;
        public bool allow_vfoA_drag = false;

        private void picDisplay_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) // changes yt7pwr
        {
            try
            {
                if (SetupForm.AlwaysOnTop)
                    picDisplay.Focus();
                int vfoa_x = 0;
                int low_x = 0;
                int high_x = 0;
                if (!chkMOX.Checked)
                {
                    switch (CurrentDSPMode)
                    {
                        case (DSPMode.CWU):
                        case (DSPMode.CWL):
                            {
                                vfoa_x = HzToPixel((float)((vfoAFreq - loscFreq) * 1e6));
                                low_x = vfoa_x - (HzToPixel((DttSP.RXFilterHighCut - DttSP.RXFilterLowCut) / 2) - HzToPixel(0.0f));
                                high_x = vfoa_x + (HzToPixel((DttSP.RXFilterHighCut - DttSP.RXFilterLowCut) / 2) - HzToPixel(0.0f));
                            }
                            break;
                        default:
                            {
                                vfoa_x = HzToPixel((float)((vfoAFreq - loscFreq) * 1e6));
                                low_x = vfoa_x + (HzToPixel(DttSP.RXFilterLowCut) - HzToPixel(0.0f));
                                high_x = vfoa_x + (HzToPixel(DttSP.RXFilterHighCut) - HzToPixel(0.0f));
                            }
                            break;
                    }
                }
                else
                {
                    low_x = HzToPixel(DttSP.TXFilterLowCut);
                    high_x = HzToPixel(DttSP.TXFilterHighCut);
                }

                int vfob_x = 0;
                int vfob_low_x = 0;
                int vfob_high_x = 0;
                if (chkEnableSubRX.Checked && !chkMOX.Checked)
                {
                    vfob_x = HzToPixel((float)((vfoBFreq - loscFreq) * 1e6));
                    vfob_low_x = vfob_x + (HzToPixel(DttSP.RXFilterLowCut) - HzToPixel(0.0f));
                    vfob_high_x = vfob_x + (HzToPixel(DttSP.RXFilterHighCut) - HzToPixel(0.0f));
                }

                switch (Display.CurrentDisplayMode)
                {
                    case DisplayMode.WATERFALL:
                        DisplayCursorX = e.X;
                        DisplayCursorY = e.Y;
                        float x = PixelToHz(e.X);
                        float y = PixelToDb(e.Y);
                        double freq = loscFreq + (double)x * 0.0000010;
                        txtDisplayCursorOffset.Text = x.ToString("f1") + "Hz";
                        txtDisplayCursorPower.Text = y.ToString("f1") + "dBm";

                        string temp_text = freq.ToString("f6") + " MHz";
                        int jper = temp_text.IndexOf(separator) + 4;
                        txtDisplayCursorFreq.Text = String.Copy(temp_text.Insert(jper, " "));
                        break;
                    case DisplayMode.PANAFALL:
                    case DisplayMode.PANADAPTER:
                        DisplayCursorX = e.X;
                        DisplayCursorY = e.Y;
                        x = PixelToHz(e.X);
                        y = PixelToDb(e.Y);
                        freq = loscFreq + (double)x * 0.0000010;
                        txtDisplayCursorOffset.Text = x.ToString("f1") + "Hz";
                        txtDisplayCursorPower.Text = y.ToString("f1") + "dBm";

                        temp_text = freq.ToString("f6") + " MHz";
                        jper = temp_text.IndexOf(separator) + 4;
                        txtDisplayCursorFreq.Text = String.Copy(temp_text.Insert(jper, " "));

                        if (current_click_tune_mode == ClickTuneMode.Off && current_dsp_mode != DSPMode.DRM)
                        {
                            if (Cursor != Cursors.Hand)
                            {
                                if (Math.Abs(e.X - low_x) < 3 || Math.Abs(e.X - high_x) < 3 ||
                                    high_filter_drag || low_filter_drag ||
                                    (chkEnableSubRX.Checked && (e.X > vfob_low_x - 3 && e.X < vfob_high_x + 3)))
                                {
                                    Cursor = Cursors.SizeWE;
                                }
                                else if (e.X > low_x && e.X < high_x)
                                {
                                    Cursor = Cursors.NoMoveHoriz;
                                }
                                else
                                {
                                    Cursor = Cursors.Cross;
                                }
                            }

                            if (high_filter_drag)
                            {
                                if (!chkMOX.Checked)
                                {
                                    SelectVarFilter();
                                    float zero = 0.0F;
                                    int new_high = 0;
                                    switch (CurrentDSPMode)
                                    {
                                        case (DSPMode.CWU):
                                        case (DSPMode.CWL):
                                            {
                                                if (tbDisplayZoom.Value == 4)
                                                {
                                                    new_high = (int)(PixelToHz((e.X - vfoa_x) + picDisplay.Width / 2));
                                                    UpdateFilters(DttSP.RXFilterLowCut, new_high);
                                                }
                                                else
                                                {
                                                    x = PixelToHz((e.X - vfoa_x));
                                                    x -= PixelToHz(0.0F);
                                                    x = x * 2;
                                                    UpdateFilters(DttSP.RXFilterLowCut, (int)x);
                                                }
                                            }
                                            break;
                                        default:
                                            {
                                                if (tbDisplayZoom.Value == 4)
                                                {
                                                    new_high = (int)(PixelToHz((e.X - vfoa_x) + picDisplay.Width / 2));
                                                    UpdateFilters(DttSP.RXFilterLowCut, new_high);
                                                }
                                                else
                                                {
                                                    x = PixelToHz((e.X - vfoa_x));
                                                    x -= PixelToHz(0.0F);
                                                    UpdateFilters(DttSP.RXFilterLowCut, (int)x);
                                                }
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    int new_high = (int)Math.Max(PixelToHz(e.X), DttSP.TXFilterLowCut + 10);
                                    switch (current_dsp_mode)
                                    {
                                        case DSPMode.LSB:
                                        case DSPMode.CWL:
                                        case DSPMode.DIGL:
                                            int new_low = -new_high;
                                            SetupForm.TXFilterLow = new_low;
                                            break;
                                        case DSPMode.USB:
                                        case DSPMode.CWU:
                                        case DSPMode.DIGU:
                                        case DSPMode.AM:
                                        case DSPMode.SAM:
                                        case DSPMode.FMN:
                                        case DSPMode.DSB:
                                            SetupForm.TXFilterHigh = new_high;
                                            break;
                                    }
                                }
                            }
                            else if (low_filter_drag)
                            {
                                if (!chkMOX.Checked)
                                {
                                    SelectVarFilter();
                                    float zero = 0.0F;
                                    int new_low = 0;
                                    switch (CurrentDSPMode)
                                    {
                                        case (DSPMode.CWU):
                                        case (DSPMode.CWL):
                                            {
                                                if (tbDisplayZoom.Value == 4)
                                                {
                                                    new_low = (int)(PixelToHz((e.X - vfoa_x) + picDisplay.Width / 2));
                                                    UpdateFilters(new_low, DttSP.RXFilterHighCut);
                                                }
                                                else
                                                {
                                                    x = PixelToHz((e.X - vfoa_x));
                                                    x -= PixelToHz(0.0F);
                                                    UpdateFilters((int)x, DttSP.RXFilterHighCut);
                                                }
                                            }
                                            break;
                                        default:
                                            {
                                                if (tbDisplayZoom.Value == 4)
                                                {
                                                    new_low = (int)(PixelToHz((e.X - vfoa_x) + picDisplay.Width / 2));
                                                    UpdateFilters(new_low, DttSP.RXFilterHighCut);
                                                }
                                                else
                                                {
                                                    new_low = (int)(PixelToHz(e.X - vfoa_x) + DttSP.RXDisplayHigh / 2);
                                                    x = PixelToHz((e.X - vfoa_x));
                                                    x -= PixelToHz(zero);
                                                    UpdateFilters((int)x, DttSP.RXFilterHighCut);
                                                }
                                            }
                                            break;
                                    }

                                }
                                else
                                {
                                    int new_low = (int)(Math.Min(PixelToHz(e.X), DttSP.TXFilterHighCut - 10));
                                    switch (current_dsp_mode)
                                    {
                                        case DSPMode.LSB:
                                        case DSPMode.CWL:
                                        case DSPMode.DIGL:
                                        case DSPMode.AM:
                                        case DSPMode.SAM:
                                        case DSPMode.FMN:
                                        case DSPMode.DSB:
                                            int new_high = -new_low;
                                            SetupForm.TXFilterHigh = new_high;
                                            break;
                                        case DSPMode.USB:
                                        case DSPMode.CWU:
                                        case DSPMode.DIGU:
                                            SetupForm.TXFilterLow = new_low;
                                            break;
                                    }
                                }
                            }
                            else if (whole_filter_drag)
                            {
                                int diff = (int)(PixelToHz(e.X) - PixelToHz(whole_filter_start_x));

                                if (!chkMOX.Checked)
                                {
                                    UpdateFilters(whole_filter_start_low + diff, whole_filter_start_high + diff);
                                }
                                else
                                {
                                    switch (current_dsp_mode)
                                    {
                                        case DSPMode.LSB:
                                        case DSPMode.DIGL:
                                            SetupForm.TXFilterLow = whole_filter_start_low - diff;
                                            SetupForm.TXFilterHigh = whole_filter_start_high - diff;
                                            break;
                                        case DSPMode.USB:
                                        case DSPMode.DIGU:
                                            SetupForm.TXFilterLow = whole_filter_start_low + diff;
                                            SetupForm.TXFilterHigh = whole_filter_start_high + diff;
                                            break;
                                        case DSPMode.AM:
                                        case DSPMode.SAM:
                                        case DSPMode.FMN:
                                        case DSPMode.DSB:
                                            SetupForm.TXFilterHigh = whole_filter_start_high + diff;
                                            break;
                                    }
                                }
                            }
                            else if (vfoA_drag)
                            {
                                int diff = (int)(PixelToHz(e.X) - PixelToHz(vfoA_drag_last_x));
                                VFOAFreq = vfoA_drag_start_freq + diff * 1e-6;
                            }
                            else if (vfob_drag)
                            {
                                int diff = (int)(PixelToHz(e.X) - PixelToHz(vfob_drag_last_x));
                                VFOBFreq = vfob_drag_start_freq + diff * 1e-6;
                            }
                        }
                        break;
                    default:
                        txtDisplayCursorOffset.Text = "";
                        txtDisplayCursorPower.Text = "";
                        txtDisplayCursorFreq.Text = "";
                        break;
                }

                double zoom_factor = tbDisplayZoom.Value / 16;
                if (spectrum_drag && zoom_factor > 1)
                {
                    if (!chkMOX.Checked)
                    {
                        float start_freq = PixelToHz(spectrum_drag_last_x);
                        float end_freq = PixelToHz(e.X);
                        spectrum_drag_last_x = e.X;
                        float delta = end_freq - start_freq;
                        VFOAFreq -= delta * 0.0000010;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void picDisplay_MouseLeave(object sender, System.EventArgs e)
        {
            txtDisplayCursorOffset.Text = "";
            txtDisplayCursorPower.Text = "";
            txtDisplayCursorFreq.Text = "";
            DisplayCursorX = -1;
            DisplayCursorY = -1;
            Cursor = Cursors.Default;
        }

        private void picDisplay_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) // changes yt7pwr
        {

            int filter_low, filter_high;

            if (Display.CurrentDisplayMode == DisplayMode.PANADAPTER || Display.CurrentDisplayMode == DisplayMode.PANAFALL)
                picDisplay.Focus();
            filter_low = DttSP.TXFilterLowCut;
            filter_high = DttSP.TXFilterHighCut;

            if (e.Button == MouseButtons.Left && chkPower.Checked)
            {
                tbDisplayPan.Value = 0;
                if (current_click_tune_mode != ClickTuneMode.Off)
                {
                    switch (Display.CurrentDisplayMode)
                    {
                        case DisplayMode.PANAFALL:
                        case DisplayMode.WATERFALL:
                        case DisplayMode.PANADAPTER:
                            float x = PixelToHz(e.X);
                            double freq = loscFreq + (double)x * 0.0000010;
                            MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
                            MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;
                            if (freq > MaxFreq)
                                freq = MaxFreq;
                            if (freq < MinFreq)
                                freq = MinFreq;
                            if (current_click_tune_mode == ClickTuneMode.VFOA)
                            {
                                VFOAFreq = Math.Round(freq, 6);
                            }
                            else
                                VFOBFreq = Math.Round(freq, 6);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (Display.CurrentDisplayMode == DisplayMode.PANADAPTER || Display.CurrentDisplayMode == DisplayMode.PANAFALL
                        && current_dsp_mode != DSPMode.DRM)
                    {
                        int vfoa_x = 0;
                        int low_x = 0;
                        int high_x = 0;
                        if (!chkMOX.Checked)
                        {
                            switch (CurrentDSPMode)
                            {
                                case (DSPMode.CWL):
                                case (DSPMode.CWU):
                                    {
                                        vfoa_x = HzToPixel((float)((vfoAFreq - loscFreq) * 1e6));
                                        low_x = vfoa_x - (HzToPixel((DttSP.RXFilterHighCut - DttSP.RXFilterLowCut) / 2) - HzToPixel(0.0f));
                                        high_x = vfoa_x + (HzToPixel((DttSP.RXFilterHighCut - DttSP.RXFilterLowCut) / 2) - HzToPixel(0.0f));
                                    }
                                    break;
                                default:
                                    {
                                        vfoa_x = HzToPixel((float)((VFOAFreq - LOSCFreq) * 1e6));
                                        low_x = vfoa_x + (HzToPixel(DttSP.RXFilterLowCut) - HzToPixel(0.0f));
                                        high_x = vfoa_x + (HzToPixel(DttSP.RXFilterHighCut) - HzToPixel(0.0f));
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            low_x = HzToPixel(DttSP.TXFilterLowCut);
                            high_x = HzToPixel(DttSP.TXFilterHighCut);
                        }

                        int vfob_x = 0;
                        int vfob_low_x = 0;
                        int vfob_high_x = 0;
                        if (chkEnableSubRX.Checked && !chkMOX.Checked)
                        {
                            switch (CurrentDSPMode)
                            {
                                case (DSPMode.CWL):
                                case (DSPMode.CWU):
                                    {
                                        vfob_x = HzToPixel((float)((vfoBFreq - loscFreq) * 1e6));
                                        vfob_low_x = vfob_x - (HzToPixel((DttSP.RXFilterHighCut - DttSP.RXFilterLowCut) / 2) - HzToPixel(0.0f));
                                        vfob_high_x = vfob_x + (HzToPixel((DttSP.RXFilterHighCut - DttSP.RXFilterLowCut) / 2) - HzToPixel(0.0f));
                                    }
                                    break;
                                default:
                                    {
                                        vfob_x = HzToPixel((float)((VFOBFreq - LOSCFreq) * 1e6));
                                        vfob_low_x = vfob_x + (HzToPixel(DttSP.RXFilterLowCut) - HzToPixel(0.0f));
                                        vfob_high_x = vfob_x + (HzToPixel(DttSP.RXFilterHighCut) - HzToPixel(0.0f));
                                    }
                                    break;
                            }
                        }

                        if (Math.Abs(e.X - low_x) < 3 && e.X < high_x)
                        {
                            low_filter_drag = true;
                            Cursor = Cursors.SizeWE;
                        }
                        else if (Math.Abs(e.X - high_x) < 3)
                        {
                            high_filter_drag = true;
                            Cursor = Cursors.SizeWE;
                        }
                        else if (e.X > low_x && e.X < high_x)
                        {
                            if (!allow_vfoA_drag)
                            {
                                whole_filter_drag = true;
                                whole_filter_start_x = e.X;
                                if (!chkMOX.Checked)
                                {
                                    whole_filter_start_low = (int)udFilterLow.Value;
                                    whole_filter_start_high = (int)udFilterHigh.Value;
                                }
                                else
                                {
                                    whole_filter_start_low = SetupForm.TXFilterLow;
                                    whole_filter_start_high = SetupForm.TXFilterHigh;
                                }
                                Cursor = Cursors.NoMoveHoriz;
                            }
                            else
                            {
                                vfoA_drag_last_x = e.X;
                                vfoA_drag = true;
                                vfoA_drag_start_freq = VFOAFreq;
                                Cursor = Cursors.SizeWE;
                            }
                        }
                        else if (chkEnableSubRX.Checked && !chkMOX.Checked &&
                            (e.X > vfob_low_x - 3 && e.X < vfob_high_x + 3))
                        {
                            vfob_drag_last_x = e.X;
                            vfob_drag_start_freq = VFOBFreq;
                            vfob_drag = true;
                            Cursor = Cursors.SizeWE;
                        }
                        else
                        {
                            spectrum_drag_last_x = e.X;
                            spectrum_drag = true;
                            Cursor = Cursors.Hand;
                        }

                    }
                    else
                    {
                        spectrum_drag_last_x = e.X;
                        spectrum_drag = true;
                        Cursor = Cursors.Hand;
                    }
                }
            }
            else if (e.Button == MouseButtons.Right && chkPower.Checked)
            {
                switch (current_click_tune_mode)
                {
                    case ClickTuneMode.Off:
                        CurrentClickTuneMode = ClickTuneMode.VFOA;
                        break;
                    case ClickTuneMode.VFOA:
                        if (chkVFOSplit.Checked || chkEnableSubRX.Checked)
                            CurrentClickTuneMode = ClickTuneMode.VFOB;
                        else
                            CurrentClickTuneMode = ClickTuneMode.Off;
                        break;
                    case ClickTuneMode.VFOB:
                        CurrentClickTuneMode = ClickTuneMode.Off;
                        break;
                }
            }
            else if (e.Button == MouseButtons.Middle)
                ChangeWheelTuneLeft();
        }

        private void picDisplay_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Display.CurrentDisplayMode == DisplayMode.PANADAPTER || Display.CurrentDisplayMode == DisplayMode.PANAFALL)
                {
                    low_filter_drag = false;
                    high_filter_drag = false;
                    whole_filter_drag = false;
                    vfob_drag = false;
                }
                spectrum_drag = false;

                int low_x = 0;
                int high_x = 0;
                if (!chkMOX.Checked)
                {
                    low_x = HzToPixel((int)udFilterLow.Value);
                    high_x = HzToPixel((int)udFilterHigh.Value);
                }
                else
                {
                    low_x = HzToPixel(DttSP.TXFilterLowCut);
                    high_x = HzToPixel(DttSP.TXFilterHighCut);
                }

                if (Math.Abs(e.X - low_x) < 3 || Math.Abs(e.X - high_x) < 3 ||
                    high_filter_drag || low_filter_drag)
                {
                    Cursor = Cursors.SizeWE;
                }
                else if (e.X > low_x && e.X < high_x)
                {
                    Cursor = Cursors.NoMoveHoriz;
                }
                else
                {
                    Cursor = Cursors.Cross;
                }
                if (vfoA_drag == true)
                {
                    vfoA_drag = false;
                    VFOAFreq = Double.Parse(txtVFOAFreq.Text);
                }
            }
        }

        private void picDisplay_Resize(object sender, System.EventArgs e) // yt7pwr
        {
            Display.Target = picDisplay;
            Display.Init();
            Display.DrawBackground();
            UpdateDisplay();
        }

        #endregion

        # region Display zoom // yt7pwr

        private void btnDisplayZoom1x_Click(object sender, System.EventArgs e)
        {
            btnDisplayZoom1x.BackColor = button_selected_color;
            tbDisplayZoom.Value = 4;
            tbDisplayPan.Value = 0;
            CalcDisplayFreq();
            btnDisplayZoom2x.BackColor = SystemColors.Control;
            btnDisplayZoom4x.BackColor = SystemColors.Control;
            btnDisplayZoom8x.BackColor = SystemColors.Control;
            btnDisplayZoom16x.BackColor = SystemColors.Control;
            btnDisplayZoom48x.BackColor = SystemColors.Control;
        }

        private void btnDisplayZoom2x_Click(object sender, System.EventArgs e)
        {
            btnDisplayZoom2x.BackColor = button_selected_color;
            tbDisplayZoom.Value = 8;
            CalcDisplayFreq();
            btnDisplayZoom1x.BackColor = SystemColors.Control;
            btnDisplayZoom4x.BackColor = SystemColors.Control;
            btnDisplayZoom8x.BackColor = SystemColors.Control;
            btnDisplayZoom16x.BackColor = SystemColors.Control;
            btnDisplayZoom48x.BackColor = SystemColors.Control;
        }

        private void btnDisplayZoom4x_Click(object sender, System.EventArgs e)
        {
            btnDisplayZoom4x.BackColor = button_selected_color;
            tbDisplayZoom.Value = 16;
            CalcDisplayFreq();
            btnDisplayZoom1x.BackColor = SystemColors.Control;
            btnDisplayZoom2x.BackColor = SystemColors.Control;
            btnDisplayZoom8x.BackColor = SystemColors.Control;
            btnDisplayZoom16x.BackColor = SystemColors.Control;
            btnDisplayZoom48x.BackColor = SystemColors.Control;
        }

        private void btnDisplayZoom8x_Click(object sender, System.EventArgs e)
        {
            btnDisplayZoom8x.BackColor = button_selected_color;
            tbDisplayZoom.Value = 32;
            CalcDisplayFreq();
            btnDisplayZoom1x.BackColor = SystemColors.Control;
            btnDisplayZoom2x.BackColor = SystemColors.Control;
            btnDisplayZoom4x.BackColor = SystemColors.Control;
            btnDisplayZoom16x.BackColor = SystemColors.Control;
            btnDisplayZoom48x.BackColor = SystemColors.Control;
        }

        private void btnDisplayZoom16x_Click(object sender, System.EventArgs e)
        {
            btnDisplayZoom16x.BackColor = button_selected_color;
            tbDisplayZoom.Value = 64;
            CalcDisplayFreq();
            btnDisplayZoom1x.BackColor = SystemColors.Control;
            btnDisplayZoom2x.BackColor = SystemColors.Control;
            btnDisplayZoom4x.BackColor = SystemColors.Control;
            btnDisplayZoom8x.BackColor = SystemColors.Control;
            btnDisplayZoom48x.BackColor = SystemColors.Control;
        }

        private void btnDisplayZoom48x_Click(object sender, System.EventArgs e)
        {
            btnDisplayZoom48x.BackColor = button_selected_color;
            tbDisplayZoom.Value = 128;
            CalcDisplayFreq();
            btnDisplayZoom1x.BackColor = SystemColors.Control;
            btnDisplayZoom2x.BackColor = SystemColors.Control;
            btnDisplayZoom4x.BackColor = SystemColors.Control;
            btnDisplayZoom8x.BackColor = SystemColors.Control;
            btnDisplayZoom16x.BackColor = SystemColors.Control;
        }

        private void tbDisplayZoom_Scroll(object sender, EventArgs e)
        {
            double zoom_factor = tbDisplayZoom.Value / 4;

            if (zoom_factor == 1.0)
            {
                btnDisplayZoom1x.BackColor = button_selected_color;
                tbDisplayPan.Value = 0;
            }
            else
                btnDisplayZoom1x.BackColor = SystemColors.Control;

            if (zoom_factor == 2.0)
                btnDisplayZoom2x.BackColor = button_selected_color;
            else
                btnDisplayZoom2x.BackColor = SystemColors.Control;

            if (zoom_factor == 4.0)
                btnDisplayZoom4x.BackColor = button_selected_color;
            else
                btnDisplayZoom4x.BackColor = SystemColors.Control;

            if (zoom_factor == 8.0)
                btnDisplayZoom8x.BackColor = button_selected_color;
            else
                btnDisplayZoom8x.BackColor = SystemColors.Control;

            if (zoom_factor == 16.0)
                btnDisplayZoom16x.BackColor = button_selected_color;
            else
                btnDisplayZoom16x.BackColor = SystemColors.Control;

            if (zoom_factor == 32.0)
                btnDisplayZoom48x.BackColor = button_selected_color;
            else 
                btnDisplayZoom48x.BackColor = SystemColors.Control;

            CalcDisplayFreq();

            if (tbDisplayZoom.Focused) btnHidden.Focus();
        }

        private void tbDisplayPan_Scroll(object sender, EventArgs e)
        {
            CalcDisplayFreq();
            if (tbDisplayPan.Focused) btnHidden.Focus();
        }

        private void tbDisplayPan_MouseMove(object sender, MouseEventArgs e)
        {
            tbDisplayPan.Focus();
        }

        private void tbDisplayZoom_MouseMove(object sender, MouseEventArgs e)
        {
            tbDisplayZoom.Focus();
        }
        #endregion

        #region Band Button Events
        // ======================================================
        // Band Button Events
        // ======================================================

        private void btnBand160_Click(object sender, System.EventArgs e) // changes yt7pwr
        {
            SaveBand();
            if (last_band.Equals("160M"))
            {
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    band_160m_index = (band_160m_index - 1 + band_160m_register) % band_160m_register;
                else
                    band_160m_index = (band_160m_index + 1) % band_160m_register;
            }
            last_band = "160M";

            string filter, mode;
            double freqA;
            double freqB;
            double losc_freq;
            if (DB.GetBandStack(last_band, band_160m_index, out mode, out filter, out freqA, out freqB, out losc_freq))
            {
                SetBand(mode, filter, freqA, freqB, losc_freq);
            }
            MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
            MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;
            tuned_freq = freqA;

            if (!usb_si570_enable)
                g59.WriteToDevice(3, 0x00000001);
        }

        private void btnBand80_Click(object sender, System.EventArgs e) // changes yt7pwr
        {
            SaveBand();
            if (last_band.Equals("80M"))
            {
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    band_80m_index = (band_80m_index - 1 + band_80m_register) % band_80m_register;
                else
                    band_80m_index = (band_80m_index + 1) % band_80m_register;
            }
            last_band = "80M";

            string filter, mode;
            double freqA;
            double freqB;
            double losc_freq;
            if (DB.GetBandStack(last_band, band_80m_index, out mode, out filter, out freqA, out freqB, out losc_freq))
            {
                SetBand(mode, filter, freqA, freqB, losc_freq);
            }
            MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
            MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;
            tuned_freq = freqA;

            if (!usb_si570_enable)
                g59.WriteToDevice(3, 0x00000002);
        }

        private void btnBand60_Click(object sender, System.EventArgs e) // changes yt7pwr
        {
            SaveBand();
            if (last_band.Equals("60M"))
            {
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    band_60m_index = (band_60m_index - 1 + band_60m_register) % band_60m_register;
                else
                    band_60m_index = (band_60m_index + 1) % band_60m_register;
            }
            last_band = "60M";

            string filter, mode;
            double freqA;
            double freqB;
            double losc_freq;
            if (DB.GetBandStack(last_band, band_60m_index, out mode, out filter, out freqA, out freqB, out losc_freq))
            {
                SetBand(mode, filter, freqA, freqB, losc_freq);
            }
            MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
            MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;
            tuned_freq = freqA;

            if (!usb_si570_enable)
                g59.WriteToDevice(3, 0x00000003);
        }

        private void btnBand40_Click(object sender, System.EventArgs e) // changes yt7pwr
        {
            SaveBand();
            if (last_band.Equals("40M"))
            {
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    band_40m_index = (band_40m_index - 1 + band_40m_register) % band_40m_register;
                else
                    band_40m_index = (band_40m_index + 1) % band_40m_register;
            }
            last_band = "40M";

            string filter, mode;
            double freqA;
            double freqB;
            double losc_freq;
            if (DB.GetBandStack(last_band, band_40m_index, out mode, out filter, out freqA, out freqB, out losc_freq))
            {
                SetBand(mode, filter, freqA,  freqB, losc_freq);
            }
            MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
            MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;
            tuned_freq = freqA;

            if (!usb_si570_enable)
                g59.WriteToDevice(3, 0x00000003);
        }

        private void btnBand30_Click(object sender, System.EventArgs e) // changes yt7pwr
        {
            SaveBand();
            if (last_band.Equals("30M"))
            {
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    band_30m_index = (band_30m_index - 1 + band_30m_register) % band_30m_register;
                else
                    band_30m_index = (band_30m_index + 1) % band_30m_register;
            }
            last_band = "30M";

            string filter, mode;
            double freqA;
            double freqB;
            double losc_freq;
            if (DB.GetBandStack(last_band, band_30m_index, out mode, out filter, out freqA, out freqB, out losc_freq))
            {
                SetBand(mode, filter, freqA, freqB, losc_freq);
            }
            MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
            MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;
            tuned_freq = freqA;

            if (!usb_si570_enable)
                g59.WriteToDevice(3, 0x00000004);
        }

        private void btnBand20_Click(object sender, System.EventArgs e) // changes yt7pwr
        {
            SaveBand();
            if (last_band.Equals("20M"))
            {
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    band_20m_index = (band_20m_index - 1 + band_20m_register) % band_20m_register;
                else
                    band_20m_index = (band_20m_index + 1) % band_20m_register;
            }
            last_band = "20M";

            string filter, mode;
            double freqA;
            double freqB;
            double losc_freq;
            if (DB.GetBandStack(last_band, band_20m_index, out mode, out filter, out freqA, out freqB, out losc_freq))
            {
                SetBand(mode, filter, freqA, freqB, losc_freq);
            }
            MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
            MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;
            tuned_freq = freqA;

            if (!usb_si570_enable)
                g59.WriteToDevice(3, 0x00000004);
        }

        private void btnBand17_Click(object sender, System.EventArgs e) // changes yt7pwr
        {
            SaveBand();
            if (last_band.Equals("17M"))
            {
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    band_17m_index = (band_17m_index - 1 + band_17m_register) % band_17m_register;
                else
                    band_17m_index = (band_17m_index + 1) % band_17m_register;
            }
            last_band = "17M";

            string filter, mode;
            double freqA;
            double freqB;
            double losc_freq;
            if (DB.GetBandStack(last_band, band_17m_index, out mode, out filter, out freqA, out freqB, out losc_freq))
            {
                SetBand(mode, filter, freqA, freqB, losc_freq);
            }
            MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
            MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;
            tuned_freq = freqA;

            if (!usb_si570_enable)
                g59.WriteToDevice(3, 0x00000005);
        }

        private void btnBand15_Click(object sender, System.EventArgs e) // changes yt7pwr
        {
            SaveBand();
            if (last_band.Equals("15M"))
            {
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    band_15m_index = (band_15m_index - 1 + band_15m_register) % band_15m_register;
                else
                    band_15m_index = (band_15m_index + 1) % band_15m_register;
            }
            last_band = "15M";

            string filter, mode;
            double freqA;
            double freqB;
            double losc_freq;
            if (DB.GetBandStack(last_band, band_15m_index, out mode, out filter, out freqA, out freqB, out losc_freq))
            {
                SetBand(mode, filter, freqA, freqB, losc_freq);
            }
            MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
            MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;
            tuned_freq = freqA;

            if (!usb_si570_enable)
                g59.WriteToDevice(3, 0x00000005);
        }

        private void btnBand12_Click(object sender, System.EventArgs e) // changes yt7pwr
        {
            SaveBand();
            if (last_band.Equals("12M"))
            {
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    band_12m_index = (band_12m_index - 1 + band_12m_register) % band_12m_register;
                else
                    band_12m_index = (band_12m_index + 1) % band_12m_register;
            }
            last_band = "12M";

            string filter, mode;
            double freqA;
            double freqB;
            double losc_freq;
            if (DB.GetBandStack(last_band, band_12m_index, out mode, out filter, out freqA, out freqB, out losc_freq))
            {
                SetBand(mode, filter, freqA, freqB, losc_freq);
            }
            MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
            MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;
            tuned_freq = freqA;

            if (!usb_si570_enable)
                g59.WriteToDevice(3, 0x00000006);
        }

        private void btnBand10_Click(object sender, System.EventArgs e) // changes yt7pwr
        {
            SaveBand();
            if (last_band.Equals("10M"))
            {
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    band_10m_index = (band_10m_index - 1 + band_10m_register) % band_10m_register;
                else
                    band_10m_index = (band_10m_index + 1) % band_10m_register;
            }
            last_band = "10M";

            string filter, mode;
            double freqA;
            double freqB;
            double losc_freq;
            if (DB.GetBandStack(last_band, band_10m_index, out mode, out filter, out freqA, out freqB, out losc_freq))
            {
                SetBand(mode, filter, freqA, freqB, losc_freq);
            }
            MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
            MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;
            tuned_freq = freqA;

            if (!usb_si570_enable)
                g59.WriteToDevice(3, 0x00000006);
        }

        private void btnBand6_Click(object sender, System.EventArgs e) // changes yt7pwr
        {
            SaveBand();
            if (last_band.Equals("6M"))
            {
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    band_6m_index = (band_6m_index - 1 + band_6m_register) % band_6m_register;
                else
                    band_6m_index = (band_6m_index + 1) % band_6m_register;
            }
            last_band = "6M";

            string filter, mode;
            double freqA;
            double freqB;
            double losc_freq;
            if (DB.GetBandStack(last_band, band_6m_index, out mode, out filter, out freqA, out freqB, out losc_freq))
            {
                SetBand(mode, filter, freqA, freqB, losc_freq);
            }
            MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
            MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;
            tuned_freq = freqA;

            if (!usb_si570_enable)
                g59.WriteToDevice(3, 0x00000007);
        }

        private void btnBand2_Click(object sender, System.EventArgs e) // changes yt7pwr
        {
            SaveBand();
            if (last_band.Equals("2M"))
            {
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    band_2m_index = (band_2m_index - 1 + band_2m_register) % band_2m_register;
                else
                    band_2m_index = (band_2m_index + 1) % band_2m_register;
            }
            last_band = "2M";

            string filter, mode;
            double freqA;
            double freqB;
            double losc_freq;
            if (DB.GetBandStack(last_band, band_2m_index, out mode, out filter, out freqA, out freqB, out losc_freq))
            {
                SetBand(mode, filter, freqA, freqB, losc_freq);
            }
            MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
            MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;
            tuned_freq = freqA;

            if (!usb_si570_enable)
                g59.WriteToDevice(3, 0x00000004);       // for 14MHz IF!
        }

        private void btnBandWWV_Click(object sender, System.EventArgs e) // changes yt7pwr
        {
            SaveBand();
            if (last_band.Equals("WWV"))
            {
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    band_wwv_index = (band_wwv_index - 1 + band_wwv_register) % band_wwv_register;
                else
                    band_wwv_index = (band_wwv_index + 1) % band_wwv_register;
            }
            last_band = "WWV";

            string filter, mode;
            double freqA;
            double freqB;
            double losc_freq;
            if (DB.GetBandStack(last_band, band_wwv_index, out mode, out filter, out freqA, out freqB, out losc_freq))
            {
                SetBand(mode, filter, freqA, freqB, losc_freq);
            }
            MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
            MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;
            tuned_freq = freqA;

            if (!usb_si570_enable)
                g59.WriteToDevice(3, 0x00000000);       // general
        }

        private void btnBandGEN_Click(object sender, System.EventArgs e) // changes yt7pwr
        {
            SaveBand();
            if (last_band == "GEN")
            {
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    band_gen_index = (band_gen_index - 1 + band_gen_register) % band_gen_register;
                else
                    band_gen_index = (band_gen_index + 1) % band_gen_register;
            }
            last_band = "GEN";

            string filter, mode;
            double freqA;
            double freqB;
            double losc_freq;
            if (DB.GetBandStack(last_band, band_gen_index, out mode, out filter, out freqA, out freqB, out losc_freq))
            {
                SetBand(mode, filter, freqA, freqB, losc_freq);
            }
            MaxFreq = LOSCFreq + DttSP.SampleRate / 2 * 1e-6;
            MinFreq = LOSCFreq - DttSP.SampleRate / 2 * 1e-6;
            tuned_freq = freqA;

            if (!usb_si570_enable)
                g59.WriteToDevice(3, 0x00000000);       // general
        }

        #endregion

        #region Mode Button Events
        // ======================================================
        // Mode Button Events
        // ======================================================

        private void SetMode(DSPMode new_mode) // changes yt7pwr
        {
            if (new_mode == DSPMode.FIRST || new_mode == DSPMode.LAST) return;

            DttSP.CurrentMode = (DttSP.Mode)new_mode;				// set new DSP mode
            Audio.CurDSPMode = new_mode;
            Display.CurrentDSPMode = new_mode;

            double freq = double.Parse(txtVFOAFreq.Text);

            tbFilterShift.Value = 0;
            btnFilterShiftReset.BackColor = SystemColors.Control;

            switch (current_dsp_mode)
            {
                case DSPMode.LSB:
                    radModeLSB.BackColor = SystemColors.Control;
                    radModeLSB.Checked = false;
                    break;
                case DSPMode.USB:
                    radModeUSB.BackColor = SystemColors.Control;
                    radModeUSB.Checked = false;
                    break;
                case DSPMode.DSB:
                    radModeDSB.BackColor = SystemColors.Control;
                    radModeDSB.Checked = false;
                    break;
                case DSPMode.CWL:
                    radModeCWL.BackColor = SystemColors.Control;
                    radModeCWL.Checked = false;
                    DttSP.StopKeyer();
                    cw_key_mode = false;
                    break;
                case DSPMode.CWU:
                    radModeCWU.BackColor = SystemColors.Control;
                    radModeCWU.Checked = false;
                    DttSP.StopKeyer();
                    cw_key_mode = false;
                    break;
                case DSPMode.FMN:
                    radModeFMN.BackColor = SystemColors.Control;
                    radModeFMN.Checked = false;
                    if (new_mode != DSPMode.AM &&
                        new_mode != DSPMode.SAM &&
                        new_mode != DSPMode.FMN)
                    {
                        chkMON.Enabled = true;
                        chkBIN.Enabled = true;
                    }
                    int pwr = (int)udPWR.Value;
                    udPWR.Maximum = 100;
                    tbPWR.Maximum = 100;
                    tbPWR.TickFrequency = 10;
                    udPWR.Value = pwr * 4;
                    break;
                case DSPMode.AM:
                    radModeAM.BackColor = SystemColors.Control;
                    radModeAM.Checked = false;
                    if (new_mode != DSPMode.AM &&
                        new_mode != DSPMode.SAM &&
                        new_mode != DSPMode.FMN)
                    {
                        chkMON.Enabled = true;
                        chkBIN.Enabled = true;
                    }
                    break;
                case DSPMode.SAM:
                    radModeSAM.BackColor = SystemColors.Control;
                    radModeSAM.Checked = false;
                    if (new_mode != DSPMode.AM &&
                        new_mode != DSPMode.SAM &&
                        new_mode != DSPMode.FMN)
                    {
                        chkMON.Enabled = true;
                        chkBIN.Enabled = true;
                    }
                    break;
                case DSPMode.SPEC:
                    radModeSPEC.BackColor = SystemColors.Control;
                    radModeSPEC.Checked = false;
                    comboDisplayMode.Items.Insert(1, "Panadapter");
                    tbFilterShift.Enabled = true;
                    btnFilterShiftReset.Enabled = true;
                    if (new_mode != DSPMode.DRM)
                        EnableAllFilters();
                    if_shift = true;
                    if (!spur_reduction)
                    {
                        DttSP.SetRXListen(0);
                        DttSP.SetOsc(0.0);
                        DttSP.SetRXListen(1);
                    }
                    if (was_panadapter) comboDisplayMode.Text = "Panadapter";
                    break;
                case DSPMode.DIGL:
                    radModeDIGL.BackColor = SystemColors.Control;
                    radModeDIGL.Checked = false;
                    if (vac_auto_enable &&
                        new_mode != DSPMode.DIGU &&
                        new_mode != DSPMode.DRM)
                    {
                        SetupForm.VACEnable = false;
                    }
                    break;
                case DSPMode.DIGU:
                    radModeDIGU.BackColor = SystemColors.Control;
                    radModeDIGU.Checked = false;
                    if (vac_auto_enable &&
                        new_mode != DSPMode.DIGL &&
                        new_mode != DSPMode.DRM)
                    {
                        SetupForm.VACEnable = false;
                    }
                    break;
                case DSPMode.DRM:
                    radModeDRM.BackColor = SystemColors.Control;
                    radModeDRM.Checked = false;
                    if (vac_auto_enable &&
                        new_mode != DSPMode.DIGL &&
                        new_mode != DSPMode.DIGU)
                        SetupForm.VACEnable = false;
                    tbFilterShift.Enabled = true;
                    btnFilterShiftReset.Enabled = true;
                    if (new_mode != DSPMode.SPEC)
                        EnableAllFilters();
                    vfo_offset = 0.0;
                    if_shift = true;
                    break;
            }

            switch (new_mode)
            {
                case DSPMode.LSB:
                    radModeLSB.BackColor = button_selected_color;
                    grpMode.Text = "Mode - LSB";
                    if (!rx_only && chkPower.Checked)
                        chkMOX.Enabled = true;
                    DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
                    grpModeSpecificPhone.BringToFront();
                    break;
                case DSPMode.USB:
                    radModeUSB.BackColor = button_selected_color;
                    grpMode.Text = "Mode - USB";
                    if (!rx_only && chkPower.Checked)
                        chkMOX.Enabled = true;
                    DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
                    grpModeSpecificPhone.BringToFront();
                    break;
                case DSPMode.DSB:
                    radModeDSB.BackColor = button_selected_color;
                    grpMode.Text = "Mode - DSB";
                    if (!rx_only && chkPower.Checked)
                        chkMOX.Enabled = true;
                    DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
                    grpModeSpecificPhone.BringToFront();
                    break;
                case DSPMode.CWL:
                    radModeCWL.BackColor = button_selected_color;
                    grpMode.Text = "Mode - CWL";
                    if (!rx_only && chkPower.Checked)
                    {
                        chkMOX.Enabled = true;
                        DttSP.StopKeyer();
                        DttSP.CWRingRestart();
                        DttSP.StartKeyer();
                    }
                    cw_key_mode = true;
                    DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
                    grpModeSpecificCW.BringToFront();
                    break;
                case DSPMode.CWU:
                    radModeCWU.BackColor = button_selected_color;
                    grpMode.Text = "Mode - CWU";
                    if (!rx_only && chkPower.Checked)
                    {
                        chkMOX.Enabled = true;
                        DttSP.StopKeyer();
                        DttSP.CWRingRestart();
                        DttSP.StartKeyer();
                    }
                    cw_key_mode = true;
                    DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
                    grpModeSpecificCW.BringToFront();
                    break;
                case DSPMode.FMN:
                    radModeFMN.BackColor = button_selected_color;
                    grpMode.Text = "Mode - FMN";
                    int pwr = (int)udPWR.Value;
                    udPWR.Value = pwr / 4;
                    udPWR.Maximum = 25;
                    tbPWR.Maximum = 25;
                    tbPWR.TickFrequency = 5;
                    if (!rx_only && chkPower.Checked)
                        chkMOX.Enabled = true;
                    chkMON.Checked = false;
                    chkMON.Enabled = false;
                    chkBIN.Checked = false;
                    chkBIN.Enabled = false;
                    DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
                    grpModeSpecificPhone.BringToFront();
                    break;
                case DSPMode.AM:
                    radModeAM.BackColor = button_selected_color;
                    grpMode.Text = "Mode - AM";
                    if (!rx_only && chkPower.Checked)
                        chkMOX.Enabled = true;
                    chkMON.Checked = false;
                    chkMON.Enabled = false;
                    chkBIN.Checked = false;
                    chkBIN.Enabled = false;
                    DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
                    grpModeSpecificPhone.BringToFront();
                    break;
                case DSPMode.SAM:
                    radModeSAM.BackColor = button_selected_color;
                    grpMode.Text = "Mode - SAM";
                    if (!rx_only && chkPower.Checked)
                        chkMOX.Enabled = true;
                    chkMON.Checked = false;
                    chkMON.Enabled = false;
                    chkBIN.Checked = false;
                    chkBIN.Enabled = false;
                    DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
                    grpModeSpecificPhone.BringToFront();
                    break;
                case DSPMode.SPEC:
                    radModeSPEC.BackColor = button_selected_color;
                    grpMode.Text = "Mode - SPEC";
                    if_shift = false;
                    DttSP.SetRXListen(0);
                    DttSP.SetOsc(0.0);
                    DttSP.SetRXListen(1);
                    DisableAllFilters();
                    grpFilter.Text = "Filter - " + (DttSP.SampleRate / 1000).ToString("f0") + "kHz";
                    tbFilterShift.Enabled = false;
                    btnFilterShiftReset.Enabled = false;
                    bool save_pan;
                    if (save_pan = (Display.CurrentDisplayMode == DisplayMode.PANADAPTER))
                    {
                        comboDisplayMode.Text = "Spectrum";
                    }
                    comboDisplayMode.Items.Remove("Panadapter");
                    was_panadapter = save_pan;
                    DttSP.RXDisplayLow = -(int)DttSP.SampleRate / 2;
                    DttSP.RXDisplayHigh = (int)DttSP.SampleRate / 2;
                    break;
                case DSPMode.DIGL:
                    radModeDIGL.BackColor = button_selected_color;
                    grpMode.Text = "Mode - DIGL";
                    DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
                    if (vac_auto_enable)
                        SetupForm.VACEnable = true;
                    grpModeSpecificDigital.BringToFront();
                    break;
                case DSPMode.DIGU:
                    radModeDIGU.BackColor = button_selected_color;
                    grpMode.Text = "Mode - DIGU";
                    DttSP.SetTXFilters(tx_filter_low, tx_filter_high);
                    if (vac_auto_enable)
                        SetupForm.VACEnable = true;
                    grpModeSpecificDigital.BringToFront();
                    break;
                case DSPMode.DRM:
                    if_shift = false;
                    vfo_offset = -0.012;
                    radModeDRM.BackColor = button_selected_color;
                    grpMode.Text = "Mode - DRM";
                    if (vac_auto_enable)
                        SetupForm.VACEnable = true;
                    chkMOX.Enabled = false;
                    DisableAllFilters();
                    tbFilterShift.Enabled = false;
                    btnFilterShiftReset.Enabled = false;
                    grpFilter.Text = "Filter - DRM";
                    DttSP.SetRXFilters(6700, 17200);
                    DttSP.RXDisplayLow = -8000;
                    DttSP.RXDisplayHigh = 8000;
                    grpModeSpecificDigital.BringToFront();
                    break;
            }

            radFilter1.Text = filter_presets[(int)new_mode].GetName(Filter.F1);
            radFilter2.Text = filter_presets[(int)new_mode].GetName(Filter.F2);
            radFilter3.Text = filter_presets[(int)new_mode].GetName(Filter.F3);
            radFilter4.Text = filter_presets[(int)new_mode].GetName(Filter.F4);
            radFilter5.Text = filter_presets[(int)new_mode].GetName(Filter.F5);
            radFilter6.Text = filter_presets[(int)new_mode].GetName(Filter.F6);
            radFilter7.Text = filter_presets[(int)new_mode].GetName(Filter.F7);
            radFilter8.Text = filter_presets[(int)new_mode].GetName(Filter.F8);
            radFilter9.Text = filter_presets[(int)new_mode].GetName(Filter.F9);
            radFilter10.Text = filter_presets[(int)new_mode].GetName(Filter.F10);
            radFilterVar1.Text = filter_presets[(int)new_mode].GetName(Filter.VAR1);
            radFilterVar2.Text = filter_presets[(int)new_mode].GetName(Filter.VAR2);

            current_dsp_mode = new_mode;
            if (current_dsp_mode != DSPMode.SPEC)
                CurrentFilter = filter_presets[(int)new_mode].LastFilter;

            tbFilterWidthScroll_newMode(); // wjt 

            Display.DrawBackground();
            txtLOSCFreq_LostFocus(this, EventArgs.Empty);
            txtVFOAFreq_LostFocus(this, EventArgs.Empty);
            txtVFOBFreq_LostFocus(this, EventArgs.Empty);
        }

        private void radModeLSB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radModeLSB.Checked)
            {
                SetMode(DSPMode.LSB);
            }
        }

        private void radModeUSB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radModeUSB.Checked)
            {
                SetMode(DSPMode.USB);
            }
        }

        private void radModeDSB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radModeDSB.Checked)
            {
                SetMode(DSPMode.DSB);
            }
        }

        private void radModeCWL_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radModeCWL.Checked)
            {
                SetMode(DSPMode.CWL);
            }

        }

        private void radModeCWU_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radModeCWU.Checked)
            {
                SetMode(DSPMode.CWU);
            }
        }

        private void radModeFMN_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radModeFMN.Checked)
            {
                SetMode(DSPMode.FMN);
            }
        }

        private void radModeAM_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radModeAM.Checked)
            {
                SetMode(DSPMode.AM);
            }
        }

        private void radModeSAM_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radModeSAM.Checked)
            {
                SetMode(DSPMode.SAM);
            }
        }

        private void radModeDIGU_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radModeDIGU.Checked)
            {
                SetMode(DSPMode.DIGU);
            }
        }

        private void radModeSPEC_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radModeSPEC.Checked)
            {
                SetMode(DSPMode.SPEC);
            }
        }

        private void radModeDIGL_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radModeDIGL.Checked)
            {
                SetMode(DSPMode.DIGL);
            }
        }

        private void radModeDRM_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radModeDRM.Checked)
            {
                SetMode(DSPMode.DRM);
            }
        }

        #endregion

        #region Filter Button Events
        // ======================================================
        // Filter Button Events
        // ======================================================

        public void SetFilter(Filter new_filter)
        {
            if (current_dsp_mode == DSPMode.FIRST || current_dsp_mode == DSPMode.LAST) return;

            int low = 0, high = 0;

            switch (current_filter)
            {
                case Filter.F1:
                    radFilter1.BackColor = SystemColors.Control;
                    break;
                case Filter.F2:
                    radFilter2.BackColor = SystemColors.Control;
                    break;
                case Filter.F3:
                    radFilter3.BackColor = SystemColors.Control;
                    break;
                case Filter.F4:
                    radFilter4.BackColor = SystemColors.Control;
                    break;
                case Filter.F5:
                    radFilter5.BackColor = SystemColors.Control;
                    break;
                case Filter.F6:
                    radFilter6.BackColor = SystemColors.Control;
                    break;
                case Filter.F7:
                    radFilter7.BackColor = SystemColors.Control;
                    break;
                case Filter.F8:
                    radFilter8.BackColor = SystemColors.Control;
                    break;
                case Filter.F9:
                    radFilter9.BackColor = SystemColors.Control;
                    break;
                case Filter.F10:
                    radFilter10.BackColor = SystemColors.Control;
                    break;
                case Filter.VAR1:
                    radFilterVar1.BackColor = SystemColors.Control;
                    udFilterLow.BackColor = SystemColors.Window;
                    udFilterHigh.BackColor = SystemColors.Window;
                    udFilterLow.Enabled = false;
                    udFilterHigh.Enabled = false;
                    break;
                case Filter.VAR2:
                    radFilterVar2.BackColor = SystemColors.Control;
                    udFilterLow.BackColor = SystemColors.Window;
                    udFilterHigh.BackColor = SystemColors.Window;
                    udFilterLow.Enabled = false;
                    udFilterHigh.Enabled = false;
                    break;
            }

            current_filter = new_filter;
            // 
            // kd5tfd added if clause below to get the zzsf cat command working again 
            // which apparently was broken sometime after 1.6.1 
            //
            //			if ( new_filter != Filter.VAR1 && new_filter != Filter.VAR2 ) 
            //			{
            low = filter_presets[(int)current_dsp_mode].GetLow(new_filter);
            high = filter_presets[(int)current_dsp_mode].GetHigh(new_filter);
            //			}
            //			else 
            //			{ 
            //				low = (int)udFilterLow.Value; 
            //				high = (int)udFilterHigh.Value; 
            //			} 
            filter_presets[(int)current_dsp_mode].LastFilter = new_filter;

            grpFilter.Text = "Filter - " + filter_presets[(int)current_dsp_mode].GetName(new_filter);

            switch (new_filter)
            {
                case Filter.F1:
                    radFilter1.BackColor = button_selected_color;
                    break;
                case Filter.F2:
                    radFilter2.BackColor = button_selected_color;
                    break;
                case Filter.F3:
                    radFilter3.BackColor = button_selected_color;
                    break;
                case Filter.F4:
                    radFilter4.BackColor = button_selected_color;
                    break;
                case Filter.F5:
                    radFilter5.BackColor = button_selected_color;
                    break;
                case Filter.F6:
                    radFilter6.BackColor = button_selected_color;
                    break;
                case Filter.F7:
                    radFilter7.BackColor = button_selected_color;
                    break;
                case Filter.F8:
                    radFilter8.BackColor = button_selected_color;
                    break;
                case Filter.F9:
                    radFilter9.BackColor = button_selected_color;
                    break;
                case Filter.F10:
                    radFilter10.BackColor = button_selected_color;
                    break;
                case Filter.VAR1:
                    radFilterVar1.BackColor = button_selected_color;
                    udFilterLow.BackColor = button_selected_color;
                    udFilterHigh.BackColor = button_selected_color;
                    udFilterLow.Enabled = true;
                    udFilterHigh.Enabled = true;
                    break;
                case Filter.VAR2:
                    radFilterVar2.BackColor = button_selected_color;
                    udFilterLow.BackColor = button_selected_color;
                    udFilterHigh.BackColor = button_selected_color;
                    udFilterLow.Enabled = true;
                    udFilterHigh.Enabled = true;
                    break;
                case Filter.NONE:
                    foreach (Control c in grpFilter.Controls)
                    {
                        if (c.GetType() == typeof(RadioButtonTS))
                        {
                            ((RadioButtonTS)c).Checked = false;

                            if (c.BackColor != SystemColors.Control)
                                ((RadioButtonTS)c).BackColor = SystemColors.Control;
                        }
                    }
                    return;
            }

            UpdateFilters(low, high);
        }

        private void radFilter1_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radFilter1.Checked)
            {
                SetFilter(Filter.F1);
            }
        }

        private void radFilter2_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radFilter2.Checked)
                SetFilter(Filter.F2);
        }

        private void radFilter3_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radFilter3.Checked)
                SetFilter(Filter.F3);
        }

        private void radFilter4_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radFilter4.Checked)
                SetFilter(Filter.F4);
        }

        private void radFilter5_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radFilter5.Checked)
                SetFilter(Filter.F5);
        }

        private void radFilter6_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radFilter6.Checked)
                SetFilter(Filter.F6);
        }

        private void radFilter7_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radFilter7.Checked)
                SetFilter(Filter.F7);
        }

        private void radFilter8_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radFilter8.Checked)
                SetFilter(Filter.F8);
        }

        private void radFilter9_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radFilter9.Checked)
                SetFilter(Filter.F9);
        }

        private void radFilter10_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radFilter10.Checked)
                SetFilter(Filter.F10);
        }

        private void radFilterVar1_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radFilterVar1.Checked)
                SetFilter(Filter.VAR1);
        }

        private void radFilterVar2_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radFilterVar2.Checked)
                SetFilter(Filter.VAR2);
        }

        private void udFilterLow_ValueChanged(object sender, System.EventArgs e)
        {
            if (udFilterLow.Focused)
            {
                if (udFilterLow.Value >= udFilterHigh.Value - 10)
                {
                    udFilterLow.Value = udFilterHigh.Value - 10;
                    return;
                }

                UpdateFilters((int)udFilterLow.Value, (int)udFilterHigh.Value);

                if (!save_filter_changes)
                    filter_presets[(int)current_dsp_mode].SetLow(current_filter, (int)udFilterLow.Value);
            }

            if (save_filter_changes)
                filter_presets[(int)current_dsp_mode].SetLow(current_filter, (int)udFilterLow.Value);

            /*if(udFilterLow.Focused)
                btnHidden.Focus();*/
        }

        private void udFilterHigh_ValueChanged(object sender, System.EventArgs e)
        {
            if (udFilterHigh.Focused)
            {
                if (udFilterHigh.Value <= udFilterLow.Value + 10)
                {
                    udFilterHigh.Value = udFilterLow.Value + 10;
                    return;
                }

                UpdateFilters((int)udFilterLow.Value, (int)udFilterHigh.Value);

                if (!save_filter_changes)
                    filter_presets[(int)current_dsp_mode].SetHigh(current_filter, (int)udFilterHigh.Value);
            }

            if (save_filter_changes)
                filter_presets[(int)current_dsp_mode].SetHigh(current_filter, (int)udFilterHigh.Value);

            /*if(udFilterHigh.Focused)
                btnHidden.Focus();*/
        }
     
        private void DoFilterShift(int shift, bool redraw)
        {
            // VK6APH: Does the Filter Shift function, alters the filter low and high frequency values 
            // as the Filter Shift slider is moved. We need to keep the last Filter Shift values
            // that the variable filters use since, unlike the other filters, there are 
            // no pre-set bandwidths that they can default to when the Filter Shift is 
            // turned off. These values are stored in the public variables last_var1_shift and
            // last_var2_shift. 
            int IFShift;
            int low;
            int high;
            int bandwidth;
            int max_shift = 9999;		// needed when using variable filters so we can't exceed +/- 10kHz DSP limits

            if (current_dsp_mode == DSPMode.SPEC ||
                current_dsp_mode == DSPMode.DRM)
                return;

            bandwidth = (int)Math.Abs(udFilterHigh.Value - udFilterLow.Value); // calculate current filter bandwidth 

            // set the maximum IF Shift depending on filter bandwidth in use 
            if (bandwidth > 800)
            {
                tbFilterShift.Maximum = 1000;  // max IF Shift +/- 1kHz for filters > 800Hz wide
                tbFilterShift.Minimum = -1000;
            }
            else
            {
                tbFilterShift.Maximum = 500;	// max IF Shift +/- 500Hz for filters < 800Hz wide
                tbFilterShift.Minimum = -500;
            }
            // calculate how far the IF Shift slider has moved
            // if we are using variable bandwidth filters need to use their last shift value
            if (current_filter == Filter.VAR1)
                IFShift = shift - last_var1_shift;
            else if (current_filter == Filter.VAR2)
                IFShift = shift - last_var2_shift;
            else
                IFShift = shift - last_filter_shift;

            high = (int)Math.Min(udFilterHigh.Value + IFShift, max_shift);	// limit high shift to maximum value
            low = (int)Math.Max(udFilterLow.Value + IFShift, -max_shift);	// limit low shift to maximum value

            DttSP.SetRXFilters(low, high);					// select new filters
            udFilterLow.Value = low;						// display new low value 
            udFilterHigh.Value = high;						// display new high value
            if (redraw) Display.DrawBackground();			// draw new background for updated filter values

            // store the last IF Shift applied for use next time
            if (current_filter == Filter.VAR1)
                last_var1_shift = last_var1_shift + IFShift;
            else if (current_filter == Filter.VAR2)
                last_var2_shift = last_var2_shift + IFShift;
            else
                last_filter_shift = last_filter_shift + IFShift;
            // show the IF Shift is active by setting the zero button colour
            if (shift != 0)
                btnFilterShiftReset.BackColor = button_selected_color;
        }

        private void tbFilterShift_Scroll(object sender, System.EventArgs e)
        {
            SelectVarFilter();

            int bw = DttSP.RXFilterHighCut - DttSP.RXFilterLowCut;
            int default_center = 0;

            switch (current_dsp_mode)
            {
                case DSPMode.USB:
                    default_center = default_low_cut + bw / 2;
                    break;
                case DSPMode.LSB:
                    default_center = -default_low_cut - bw / 2;
                    break;
                case DSPMode.CWU:
                    default_center = cw_pitch;
                    break;
                case DSPMode.CWL:
                    default_center = -cw_pitch;
                    break;
                case DSPMode.DIGU:
                    default_center = digu_click_tune_offset;
                    break;
                case DSPMode.DIGL:
                    default_center = -digl_click_tune_offset;
                    break;
            }

            int adjusted_max = max_filter_shift;
            if (default_center > 0)
            {
                if (tbFilterShift.Value > 0)
                {
                    adjusted_max = Math.Min(max_filter_shift, 9999 - (Math.Abs(default_center) + bw / 2));
                }
            }
            else if (default_center < 0)
            {
                if (tbFilterShift.Value < 0)
                {
                    adjusted_max = Math.Min(max_filter_shift, 9999 - (Math.Abs(default_center) + bw / 2));
                }
            }
            else //default_center == 0
            {
                adjusted_max = Math.Min(max_filter_shift, 9999 - bw / 2);
            }

            int range = tbFilterShift.Maximum - tbFilterShift.Minimum;
            int new_center = default_center + (int)((float)tbFilterShift.Value / (range / 2) * adjusted_max);
            UpdateFilters(new_center - bw / 2, new_center + bw / 2);

            btnFilterShiftReset.BackColor = button_selected_color;

            if (tbFilterShift.Focused)
                btnHidden.Focus();
        }

        private void tbFilterShift_Update(int low, int high)
        {
            int bw = DttSP.RXFilterHighCut - DttSP.RXFilterLowCut;
            int default_center = 0;
            int current_center = (low + high) / 2;

            switch (current_dsp_mode)
            {
                case DSPMode.USB:
                    default_center = default_low_cut + bw / 2;
                    break;
                case DSPMode.LSB:
                    default_center = -default_low_cut - bw / 2;
                    break;
                case DSPMode.CWU:
                    default_center = cw_pitch;
                    break;
                case DSPMode.CWL:
                    default_center = -cw_pitch;
                    break;
                case DSPMode.DIGU:
                    default_center = digu_click_tune_offset;
                    break;
                case DSPMode.DIGL:
                    default_center = -digl_click_tune_offset;
                    break;
            }

            int adjusted_max = max_filter_shift;
            if (default_center > 0)
            {
                if (current_center > default_center)
                {
                    adjusted_max = Math.Min(max_filter_shift, 9999 - (Math.Abs(default_center) + bw / 2));
                }
            }
            else if (default_center < 0)
            {
                if (current_center < default_center)
                {
                    adjusted_max = Math.Min(max_filter_shift, 9999 - (Math.Abs(default_center) + bw / 2));
                }
            }
            else //default_center == 0
            {
                adjusted_max = Math.Min(max_filter_shift, 9999 - bw / 2);
            }

            int range = tbFilterShift.Maximum - tbFilterShift.Minimum;
            int delta = current_center - default_center;
            int new_val = (int)((float)delta / adjusted_max * (range / 2));
            if (new_val > tbFilterShift.Maximum) new_val = tbFilterShift.Maximum;
            if (new_val < tbFilterShift.Minimum) new_val = tbFilterShift.Minimum;
            tbFilterShift.Value = new_val;
        }

        private void btnFilterShiftReset_Click(object sender, System.EventArgs e)
        {
            int bw = DttSP.RXFilterHighCut - DttSP.RXFilterLowCut;
            int low, high;
            switch (current_dsp_mode)
            {
                case DSPMode.AM:
                case DSPMode.SAM:
                case DSPMode.FMN:
                case DSPMode.DSB:
                    tbFilterShift.Value = 0;
                    tbFilterShift_Scroll(this, EventArgs.Empty);
                    break;
                case DSPMode.USB:
                    low = default_low_cut;
                    high = low + bw;
                    UpdateFilters(low, high);
                    break;
                case DSPMode.CWU:
                    low = cw_pitch - bw / 2;
                    high = cw_pitch + bw / 2;
                    if (low < 0)
                    {
                        int delta = -low;
                        low += delta;
                        high += delta;
                    }
                    else if (high > 9999)
                    {
                        int delta = high - 9999;
                        high -= delta;
                        low -= delta;
                    }
                    UpdateFilters(low, high);
                    break;
                case DSPMode.DIGU:
                    low = digu_click_tune_offset - bw / 2;
                    high = digu_click_tune_offset + bw / 2;
                    if (low < 0)
                    {
                        int delta = -low;
                        low += delta;
                        high += delta;
                    }
                    else if (high > 9999)
                    {
                        int delta = high - 9999;
                        high -= delta;
                        low -= delta;
                    }
                    UpdateFilters(low, high);
                    break;
                case DSPMode.LSB:
                    high = -default_low_cut;
                    low = high - bw;
                    UpdateFilters(low, high);
                    break;
                case DSPMode.CWL:
                    high = -cw_pitch + bw / 2;
                    low = -cw_pitch - bw / 2;
                    if (high > 0)
                    {
                        int delta = -high;
                        low -= delta;
                        high -= delta;
                    }
                    else if (low < -9999)
                    {
                        int delta = low + 9999;
                        high += delta;
                        low += delta;
                    }
                    UpdateFilters(low, high);
                    break;
                case DSPMode.DIGL:
                    high = -digl_click_tune_offset + bw / 2;
                    low = -digl_click_tune_offset - bw / 2;
                    if (high > 0)
                    {
                        int delta = -high;
                        low -= delta;
                        high -= delta;
                    }
                    else if (low < -9999)
                    {
                        int delta = low + 9999;
                        high += delta;
                        low += delta;
                    }
                    UpdateFilters(low, high);
                    break;
            }
            btnFilterShiftReset.BackColor = SystemColors.Control;	// make button grey
        }

        private FilterWidthMode current_filter_width_mode = FilterWidthMode.Linear;
        public FilterWidthMode CurrentFilterWidthMode
        {
            get { return current_filter_width_mode; }
            set
            {
                current_filter_width_mode = value;
                UpdateFilters(DttSP.RXFilterLowCut, DttSP.RXFilterHighCut);
            }
        }

        private void tbFilterWidth_Update(int low, int high)
        {
            int bw = high - low;
            switch (current_dsp_mode)
            {
                case DSPMode.AM:
                case DSPMode.SAM:
                case DSPMode.FMN:
                case DSPMode.DSB:
                    bw /= 2;
                    break;
            }

            int range = tbFilterWidth.Maximum - tbFilterWidth.Minimum;
            int new_val = 0;

            switch (current_filter_width_mode)
            {
                case FilterWidthMode.Linear:
                    new_val = tbFilterWidth.Minimum + (int)((float)bw / max_filter_width * range);
                    break;
                case FilterWidthMode.Log:
                    double max_log = Math.Log(tbFilterWidth.Maximum);
                    double temp = max_log - (float)bw / max_filter_width * max_log;
                    new_val = tbFilterWidth.Maximum - (int)Math.Pow(Math.E, temp);
                    break;
                case FilterWidthMode.Log10:
                    max_log = Math.Log10(tbFilterWidth.Maximum);
                    temp = max_log - (float)bw / max_filter_width * max_log;
                    new_val = tbFilterWidth.Maximum - (int)Math.Pow(10, temp);
                    break;
            }

            if (new_val > tbFilterWidth.Maximum) new_val = tbFilterWidth.Maximum;
            if (new_val < tbFilterWidth.Minimum) new_val = tbFilterWidth.Minimum;
            tbFilterWidth.Value = new_val;
        }

        private void tbFilterWidth_Scroll(object sender, System.EventArgs e)
        {
            if (current_dsp_mode == DSPMode.DRM || current_dsp_mode == DSPMode.SPEC)
            {
                return;  // no good in this mode 
            }

            SelectVarFilter();

            int range = tbFilterWidth.Maximum - tbFilterWidth.Minimum;
            int new_bw = 0;

            switch (current_filter_width_mode)
            {
                case FilterWidthMode.Linear:
                    new_bw = (int)((float)(tbFilterWidth.Value - tbFilterWidth.Minimum) / range * max_filter_width);
                    break;
                case FilterWidthMode.Log:
                    double max_log = Math.Log(tbFilterWidth.Maximum);
                    double temp = Math.Log(Math.Max((tbFilterWidth.Maximum - tbFilterWidth.Value), 1.0));
                    temp = max_log - temp;
                    new_bw = (int)((float)(temp / max_log * max_filter_width));
                    break;
                case FilterWidthMode.Log10:
                    max_log = Math.Log10(tbFilterWidth.Maximum);
                    temp = Math.Log10(Math.Max((tbFilterWidth.Maximum - tbFilterWidth.Value), 1.0));
                    temp = max_log - temp;
                    new_bw = (int)((float)(temp / max_log * max_filter_width));
                    break;
            }

            new_bw = Math.Max(new_bw, 10);
            int current_center = (DttSP.RXFilterLowCut + DttSP.RXFilterHighCut) / 2;
            int low = 0, high = 0;
            switch (current_dsp_mode)
            {
                case DSPMode.AM:
                case DSPMode.SAM:
                case DSPMode.FMN:
                case DSPMode.DSB:
                    low = current_center - new_bw;
                    high = current_center + new_bw;
                    if (low < -max_filter_width)
                    {
                        low += (-max_filter_width - low);
                        high += (-max_filter_width - low);
                    }
                    else if (high > max_filter_width)
                    {
                        high -= (high - max_filter_width);
                        low -= (high - max_filter_width);
                    }
                    break;
                case DSPMode.LSB:
                    high = -default_low_cut;
                    low = high - new_bw;
                    break;
                case DSPMode.CWL:
                case DSPMode.DIGL:
                    low = current_center - new_bw / 2;
                    high = current_center + new_bw / 2;
                    if (high > -default_low_cut && DttSP.RXFilterHighCut <= -default_low_cut)
                    {
                        high = -default_low_cut;
                        low = high - new_bw;
                    }
                    else if (low < -9999)
                    {
                        low = -9999;
                        high = low + new_bw;
                    }
                    break;
                case DSPMode.USB:
                    low = default_low_cut;
                    high = low + new_bw;
                    break;
                case DSPMode.CWU:
                case DSPMode.DIGU:
                    low = current_center - new_bw / 2;
                    high = current_center + new_bw / 2;
                    if (low < default_low_cut && DttSP.RXFilterLowCut >= default_low_cut)
                    {
                        low = default_low_cut;
                        high = low + new_bw;
                    }
                    else if (high > 9999)
                    {
                        high = 9999;
                        low = high - new_bw;
                    }
                    break;
            }
            UpdateFilters(low, high);

            if (tbFilterWidth.Focused)
                btnHidden.Focus();
        }

        private void tbFilterWidthScroll_newMode()
        {
            //centerSave = 0;  // dump any save center with scroller is keeping 
            switch (current_dsp_mode)
            {
                case DSPMode.SPEC:
                case DSPMode.DRM:
                    tbFilterWidth.Enabled = false;
                    break;

                default:
                    tbFilterWidth.Enabled = true;
                    break;
            }
        }
        #endregion

        #region VFO Button Events
        // ======================================================
        // VFO Button Events
        // ======================================================

        // Added 6/20/05 BT for CAT commands
        public void CATVFOSwap(string pChangec)
        {
            string c = pChangec;
            if (c.Length > 0)
            {
                switch (c)
                {
                    case "0":
                        btnVFOAtoB_Click(btnVFOAtoB, EventArgs.Empty);
                        break;
                    case "1":
                        btnVFOBtoA_Click(btnVFOBtoA, EventArgs.Empty);
                        break;
                    case "2":
                        btnVFOSwap_Click(btnVFOSwap, EventArgs.Empty);
                        break;
                }
            }
        }


        private void btnVFOAtoB_Click(object sender, System.EventArgs e)
        {
            txtVFOBFreq.Text = txtVFOAFreq.Text;
            txtVFOBFreq_LostFocus(this, EventArgs.Empty);
            vfob_dsp_mode = current_dsp_mode;
            vfob_filter = current_filter;
        }

        private void btnVFOBtoA_Click(object sender, System.EventArgs e)
        {
            CurrentDSPMode = vfob_dsp_mode;
            CurrentFilter = vfob_filter;
            txtVFOAFreq.Text = txtVFOBFreq.Text;
            txtVFOAFreq_LostFocus(this, EventArgs.Empty);
        }

        private void btnVFOSwap_Click(object sender, System.EventArgs e)
        {
            string temp = txtVFOAFreq.Text;

            if (!chkEnableSubRX.Checked)
            {
                DSPMode mode = current_dsp_mode;
                Filter filter = current_filter;

                CurrentDSPMode = vfob_dsp_mode;
                CurrentFilter = vfob_filter;
                vfob_dsp_mode = mode;
                vfob_filter = filter;
            }

            txtVFOAFreq.Text = txtVFOBFreq.Text;
            txtVFOBFreq.Text = temp;
            txtVFOAFreq_LostFocus(this, EventArgs.Empty);
            txtVFOBFreq_LostFocus(this, EventArgs.Empty);
        }

        private void chkVFOSplit_CheckedChanged(object sender, System.EventArgs e) // changes yt7pwr
        {
            Display.SplitEnabled = chkVFOSplit.Checked;
            if (chkVFOSplit.Checked)
            {
                //chkXIT.Checked = false;
                chkVFOSplit.BackColor = button_selected_color;
                grpVFOB.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
                grpVFOB.ForeColor = Color.Red;
                if (chkPower.Checked)
                {
                    txtVFOBFreq.ForeColor = Color.Red;
                    txtVFOBMSD.ForeColor = Color.Red;
                    txtVFOBLSD.ForeColor = small_vfo_color;
                    txtVFOBBand.ForeColor = band_text_light_color;
                }
            }
            else
            {
                chkVFOSplit.BackColor = SystemColors.Control;
                grpVFOB.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular);
                grpVFOB.ForeColor = Color.Black;
                if (chkPower.Checked)
                {
                    if (chkEnableSubRX.Checked)
                    {
                        txtVFOBFreq.ForeColor = vfo_text_light_color;
                        txtVFOBMSD.ForeColor = vfo_text_light_color;
                        txtVFOBLSD.ForeColor = small_vfo_color;
                        txtVFOBBand.ForeColor = band_text_light_color;
                    }
                    else
                    {
                        txtVFOBFreq.ForeColor = vfo_text_dark_color;
                        txtVFOBMSD.ForeColor = vfo_text_dark_color;
                        txtVFOBLSD.ForeColor = vfo_text_dark_color;
                        txtVFOBBand.ForeColor = band_text_dark_color;
                    }
                }
            }
            if (MOX)
                SetTXOscFreqs(true,false);
        }

        private void chkXIT_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkXIT.Checked)
            {
                chkXIT.BackColor = button_selected_color;
                Display.XIT = (int)udXIT.Value;
            }
            else
            {
                chkXIT.BackColor = SystemColors.Control;
                Display.XIT = 0;
            }

            if (chkMOX.Checked)
            {
                if (chkVFOSplit.Checked)
                    txtVFOBFreq_LostFocus(this, EventArgs.Empty);
                else
                    txtVFOAFreq_LostFocus(this, EventArgs.Empty);
            }
        }

        private void chkRIT_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkRIT.Checked)
            {
                chkRIT.BackColor = button_selected_color;
                Display.RIT = (int)udRIT.Value;
            }
            else
            {
                chkRIT.BackColor = SystemColors.Control;
                Display.RIT = 0;
            }

            if (!chkMOX.Checked)
                txtVFOAFreq_LostFocus(this, EventArgs.Empty);
        }

        private void udRIT_ValueChanged(object sender, System.EventArgs e)
        {
            if (chkRIT.Checked && !chkMOX.Checked)
                txtVFOAFreq_LostFocus(this, EventArgs.Empty);
            if (chkRIT.Checked) Display.RIT = (int)udRIT.Value;

            if (udRIT.Focused)
                btnHidden.Focus();
        }

        private void udXIT_ValueChanged(object sender, System.EventArgs e)
        {
            if (chkXIT.Checked && chkMOX.Checked)
            {
                if (chkVFOSplit.Checked)
                    txtVFOBFreq_LostFocus(this, EventArgs.Empty);
                else
                    txtVFOAFreq_LostFocus(this, EventArgs.Empty);
            }

            if (chkXIT.Checked) Display.XIT = (int)udXIT.Value;

            if (udXIT.Focused)
                btnHidden.Focus();
        }

        private void btnXITReset_Click(object sender, System.EventArgs e) // changes yt7pwr
        {
            udXIT.Value = 0;
            if (MOX)
                SetTXOscFreqs(true,false);
        }

        private void btnRITReset_Click(object sender, System.EventArgs e)
        {
            udRIT.Value = 0;
        }

        private void btnZeroBeat_Click(object sender, System.EventArgs e)
        {
            int peak_hz = FindPeakFreqInPassband();
            if (peak_hz == -1)
            {
                return; // find peak croaked - bail
            }
            // Debug.WriteLine("peak: " + peak_hz);
            int delta_hz = 0;

            // if we're in CW mode, zero beat to CWPitch, provided it is in the passband
            switch (current_dsp_mode)
            {
                case DSPMode.CWL:
                case DSPMode.CWU:
                case DSPMode.USB:
                case DSPMode.LSB:
                    int local_pitch = CWPitch;
                    if (current_dsp_mode == DSPMode.CWL || current_dsp_mode == DSPMode.LSB)
                    {
                        local_pitch = -local_pitch;
                    }
                    // is cwoffset in passband?
                    if (local_pitch >= udFilterLow.Value &&
                        local_pitch <= udFilterHigh.Value)
                    {
                        delta_hz = peak_hz - local_pitch;
                        // Debug.WriteLine("delta(cw): " + delta_hz);
                    }
                    else
                    {
                        // if we get here and delta_hz is still 0, the current
                        // CW pitch is not within the passband.
                        // Put strongest peak @ center of passband
                        int center_hz = (DttSP.RXFilterHighCut + DttSP.RXFilterLowCut) / 2;
                        delta_hz = peak_hz - center_hz;
                    }
                    break;
                case DSPMode.DIGL:
                    local_pitch = -digl_click_tune_offset;
                    if (local_pitch >= udFilterLow.Value &&
                        local_pitch <= udFilterHigh.Value)
                    {
                        delta_hz = peak_hz - local_pitch;
                    }
                    else
                    {
                        // if we get here and delta_hz is still 0, the current
                        // pitch is not within the passband.
                        // Put strongest peak @ center of passband
                        int center_hz = (DttSP.RXFilterHighCut + DttSP.RXFilterLowCut) / 2;
                        delta_hz = peak_hz - center_hz;
                    }
                    break;
                case DSPMode.DIGU:
                    local_pitch = digu_click_tune_offset;
                    if (local_pitch >= udFilterLow.Value &&
                        local_pitch <= udFilterHigh.Value)
                    {
                        delta_hz = peak_hz - local_pitch;
                    }
                    else
                    {
                        // if we get here and delta_hz is still 0, the current
                        // pitch is not within the passband.
                        // Put strongest peak @ center of passband
                        int center_hz = (DttSP.RXFilterHighCut + DttSP.RXFilterLowCut) / 2;
                        delta_hz = peak_hz - center_hz;
                    }
                    break;
                case DSPMode.AM:
                case DSPMode.SAM:
                case DSPMode.FMN:
                    delta_hz = peak_hz;
                    break;
            }

            //          Debug.WriteLine("peak: " + peak_hz);
            //          Debug.WriteLine("center: " + center_hz);
            //          Debug.WriteLine("delta: " + delta_hz + "\n");

            VFOAFreq += (double)delta_hz / 1e6;
        }

        unsafe private int FindPeakFreqInPassband()
        {
            // convert hz to buckets in the averaging data
            int lo_cut_hz = (int)udFilterLow.Value;
            int hi_cut_hz = (int)udFilterHigh.Value;
            double hz_per_bucket = DttSP.SampleRate / Display.BUFFER_SIZE;
            int zero_hz_bucket = Display.BUFFER_SIZE / 2;
            int lo_bucket = (int)(((double)lo_cut_hz / hz_per_bucket)) + zero_hz_bucket;
            int hi_bucket = (int)(((double)hi_cut_hz / hz_per_bucket)) + zero_hz_bucket;

            //~~~~ 
            float max_val = float.MinValue;
            int max_bucket = 0;

            float[] spectrum_data;

            // reuse the existing display data if there is any
            switch (Display.CurrentDisplayMode)
            {
                case DisplayMode.PANADAPTER:
                    if (chkDisplayAVG.Checked)
                    {
                        spectrum_data = Display.average_buffer;
                    }
                    else
                    {
                        spectrum_data = Display.current_display_data;
                    }
                    break;
                case DisplayMode.PANAFALL:
                case DisplayMode.WATERFALL:
                    if (chkDisplayAVG.Checked)
                    {
                        spectrum_data = Display.average_waterfall_buffer;
                    }
                    else
                    {
                        spectrum_data = Display.current_waterfall_data;
                    }
                    break;

                // no specturm data - go get some 
                default:
                    spectrum_data = new float[Display.BUFFER_SIZE];
                    if (spectrum_data == null)
                    {
                        return -1; // bail out - not buffer 
                    }
                    fixed (float* ptr = &(spectrum_data[0]))
                        DttSP.GetSpectrum(ptr);
                    break;
            }


            for (int i = lo_bucket; i <= hi_bucket; i++)
            {
                if (spectrum_data[i] > max_val)
                {
                    max_bucket = i;
                    max_val = spectrum_data[i];
                }
            }
            int peak_hz = (int)(((double)(max_bucket - zero_hz_bucket)) * hz_per_bucket);
            return peak_hz;
        }

        private void btnIFtoVFO_Click(object sender, System.EventArgs e)
        {
            int current_if_shift;

            bool is_centered_mode = false;
            bool is_cw_mode = false;
            bool is_lower_sb_mode = false;

            current_if_shift = tbFilterShift.Value;

            //			Debug.WriteLine("current if shift: " + current_if_shift);

            if (current_if_shift == 0) return; // nothing to do

            switch (CurrentDSPMode)
            {
                case DSPMode.DRM:
                case DSPMode.SPEC:
                case DSPMode.DIGL:
                case DSPMode.DIGU:
                    return; // nothing to do for these modes

                case DSPMode.AM:
                case DSPMode.FMN:
                case DSPMode.DSB:
                case DSPMode.SAM:
                    is_centered_mode = true;
                    break;

                case DSPMode.CWL:
                    is_cw_mode = true;
                    is_lower_sb_mode = true;
                    break;
                case DSPMode.CWU:
                    is_cw_mode = true;
                    break;
                case DSPMode.LSB:
                    is_lower_sb_mode = true;
                    break;
                case DSPMode.USB:
                    break;
                default:
                    // no clue what the mode is -- bail out
                    return;
            }

            int current_width = (int)udFilterHigh.Value - (int)udFilterLow.Value;
            int current_center = (int)udFilterLow.Value + (current_width / 2);
            //			Debug.WriteLine("w: " + current_width + " center: " + current_center + " vfo: " +  VFOAFreq);

            double new_vfo = 0;
            int new_lo = 0;
            int new_hi = 0;

            if (is_centered_mode)
            {
                new_vfo = VFOAFreq + ((double)current_center) / (1e6);
                new_lo = -(current_width / 2);
                new_hi = current_width / 2;
            }
            else  // sideband style mode
            {
                int new_center;
                if (is_cw_mode)
                {
                    new_center = cw_pitch;
                }
                else  // sideband mode
                {
                    new_center = default_low_cut + (current_width / 2);
                }
                if (is_lower_sb_mode)
                {
                    new_center = -new_center;
                }
                new_vfo = VFOAFreq + ((double)(current_center - new_center)) / (1e6);
                // now figure out filter limits
                new_lo = new_center - (current_width / 2);
                new_hi = new_center + (current_width / 2);
            }
            //			Debug.WriteLine("new vfo: " + new_vfo + " lo: " + new_lo + " hi: " + new_hi );
            if (VFOAFreq > new_vfo)  // need to change this in the right order!
            {
                udFilterHigh.Value = new_hi;
                udFilterLow.Value = new_lo;
            }
            else
            {
                udFilterLow.Value = new_lo;
                udFilterHigh.Value = new_hi;
            }

            VFOAFreq = new_vfo;
            switch (CurrentFilter)
            {
                case Filter.VAR1:
                    last_var1_shift = 0;
                    break;
                case Filter.VAR2:
                    last_var2_shift = 0;
                    break;
                default:
                    last_filter_shift = 0;
                    break;
            }
            btnFilterShiftReset_Click(this, EventArgs.Empty);
        }

        #endregion

        #region DSP Button Events

        private void chkNR_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkNR.Checked)
            {
                chkNR.BackColor = button_selected_color;
                DttSP.SetNR(1);
            }
            else
            {
                chkNR.BackColor = SystemColors.Control;
                DttSP.SetNR(0);
            }
        }

        private void chkANF_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkANF.Checked)
            {
                chkANF.BackColor = button_selected_color;
                DttSP.SetANF(1);
            }
            else
            {
                chkANF.BackColor = SystemColors.Control;
                DttSP.SetANF(0);
            }
        }

        private void chkNB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkNB.Checked)
            {
                chkNB.BackColor = button_selected_color;
                DttSP.SetNB(1);
            }
            else
            {
                chkNB.BackColor = SystemColors.Control;
                DttSP.SetNB(0);
            }
        }

        private void chkDSPComp_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkDSPComp.Checked)
            {
                chkDSPComp.BackColor = button_selected_color;
                DttSP.SetTXAGCFF(1);
            }
            else
            {
                chkDSPComp.BackColor = SystemColors.Control;
                DttSP.SetTXAGCFF(0);
            }
        }

        private void chkDSPNB2_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkDSPNB2.Checked)
            {
                chkDSPNB2.BackColor = button_selected_color;
                DttSP.SetSDROM(1);
            }
            else
            {
                chkDSPNB2.BackColor = SystemColors.Control;
                DttSP.SetSDROM(0);
            }
        }

        private void chkDSPCompander_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkDSPCompander.Checked)
            {
                DttSP.SetTXCompandSt(1);
                chkDSPCompander.BackColor = button_selected_color;
            }
            else
            {
                DttSP.SetTXCompandSt(0);
                chkDSPCompander.BackColor = SystemColors.Control;
            }
        }

        #endregion

        #region Menu Events

        private void menu_setup_Click(object sender, System.EventArgs e)
        {
            if (SetupForm.IsDisposed)
                SetupForm = new Setup(this);
            SetupForm.Show();
            SetupForm.Focus();
        }

        private void menu_wave_Click(object sender, System.EventArgs e)
        {
            if (WaveForm.IsDisposed)
                WaveForm = new WaveControl(this);
            WaveForm.Show();
            WaveForm.Focus();
        }

        private void mnuEQ_Click(object sender, System.EventArgs e)
        {
            if (EQForm == null || EQForm.IsDisposed)
                EQForm = new EQForm();
            EQForm.Show();
            EQForm.Focus();
        }

        private void mnuCWX_Click(object sender, System.EventArgs e)
        {
            if (current_dsp_mode == DSPMode.LSB)
                CurrentDSPMode = DSPMode.CWL;
            else if (current_dsp_mode == DSPMode.USB)
                CurrentDSPMode = DSPMode.CWU;

            if (current_dsp_mode != DSPMode.CWL &&
                current_dsp_mode != DSPMode.CWU)
            {
                MessageBox.Show("The radio must be in CWL or CWU mode in order to open the " +
                    "CWX Control Form.",
                    "CWX Error: Wrong Mode",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            //	cw_key_mode = true;
            if (CWXForm == null || CWXForm.IsDisposed)
            {
                CWXForm = new CWX(this);
                CWXForm.StartPosition = FormStartPosition.Manual;
                CWXForm.RestoreSettings();
            }

            CWXForm.Show();
            CWXForm.Focus();
        }

        private void menuItemFilterConfigure_Click(object sender, System.EventArgs e)
        {
            if (current_dsp_mode == DSPMode.DRM || current_dsp_mode == DSPMode.SPEC) return;

            if (filterForm == null || filterForm.IsDisposed)
                filterForm = new FilterForm(this);

            filterForm.CurrentDSPMode = current_dsp_mode;
            filterForm.CurrentFilter = current_filter;
            filterForm.Show();
            filterForm.Focus();
        }

        #endregion

        #region SUB RX
        private void tbPanMainRX_Scroll(object sender, System.EventArgs e) // changes yt7pwr
        {
            if (chkEnableSubRX.Checked)
            {
                float val = (int)tbPanMainRX.Value / 100.0f;
                if (chkPanSwap.Checked) val = 1.0f - val;
                DttSP.SetRXListen(1);
                DttSP.SetRXPan(val);
            }
            if (tbPanMainRX.Focused)
                btnHidden.Focus();
        }

        private void tbPanSubRX_Scroll(object sender, System.EventArgs e) // changes yt7pwr
        {
            float val = (int)tbPanSubRX.Value / 100.0f;
            if (chkPanSwap.Checked) val = 1.0f - val;
            DttSP.SetRXListen(2);
            DttSP.SetRXPan(val);
            DttSP.SetRXListen(1);
            if (tbPanSubRX.Focused)
                btnHidden.Focus();
        }

        private void chkEnableSubRX_CheckedChanged(object sender, System.EventArgs e) // changes yt7pwr
        {
            if (chkEnableSubRX.Checked)
            {
                if (chkVFOsinc.Checked)
                {
                    chkVFOsinc.BackColor = button_selected_color;
                    vfo_sinc = true;
                }
                tbPanMainRX_Scroll(this, EventArgs.Empty);
                tbRX0Gain_Scroll(this, EventArgs.Empty);
                DttSP.SetRXListen(2);
                DttSP.SetRXOn();
                DttSP.SetRXListen(1);
                DttSP.SetRXOutputGain(2, (double)tbRX1Gain.Value / tbRX1Gain.Maximum);
                chkEnableSubRX.BackColor = button_selected_color;
                if (chkPower.Checked)
                {
                    txtVFOBFreq_LostFocus(this, EventArgs.Empty);
                    if (!chkVFOSplit.Checked)
                    {
                        txtVFOBFreq.ForeColor = vfo_text_light_color;
                        txtVFOBMSD.ForeColor = vfo_text_light_color;
                        txtVFOBLSD.ForeColor = small_vfo_color;
                        txtVFOBBand.ForeColor = band_text_light_color;
                    }
                }
            }
            else
            {
                chkVFOsinc.Checked = false;
                chkVFOsinc.BackColor = SystemColors.Control;
                vfo_sinc = false;
                DttSP.SetRXListen(2);
                DttSP.SetRXOff();
                DttSP.SetRXListen(1);
                chkEnableSubRX.BackColor = SystemColors.Control;
                if (chkPower.Checked)
                {
                    if (!chkVFOSplit.Checked)
                    {
                        txtVFOBFreq.ForeColor = vfo_text_dark_color;
                        txtVFOBMSD.ForeColor = vfo_text_dark_color;
                        txtVFOBLSD.ForeColor = vfo_text_dark_color;
                        txtVFOBBand.ForeColor = band_text_dark_color;
                    }
                }
            }
            Display.SubRXEnabled = chkEnableSubRX.Checked;
        }

        private void chkPanSwap_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkEnableSubRX.Checked)
            {
                tbPanMainRX_Scroll(this, EventArgs.Empty);
                tbPanSubRX_Scroll(this, EventArgs.Empty);
            }
        }

        private void tbRX0Gain_Scroll(object sender, System.EventArgs e)
        {
            if (chkEnableSubRX.Checked)
                DttSP.SetRXOutputGain(1, (double)tbRX0Gain.Value / tbRX0Gain.Maximum);
            if (tbRX0Gain.Focused)
                btnHidden.Focus();
        }

        private void tbRX1Gain_Scroll(object sender, System.EventArgs e)
        {
            DttSP.SetRXOutputGain(2, (double)tbRX1Gain.Value / tbRX1Gain.Maximum);
            if (tbRX1Gain.Focused)
                btnHidden.Focus();
        }
        #endregion


        // yt7pwr
        #region LOSC event

        private void panelLOSCHover_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Control c1 = (Control)sender;
            Control c2 = txtLOSCFreq;
            int client_width = (c1.Size.Width - c1.ClientSize.Width) + (c2.Size.Width - c2.ClientSize.Width);
            int client_height = (c1.Size.Height - c1.ClientSize.Height) + (c2.Size.Height - c2.ClientSize.Height);
            int x_offset = c1.Left - c2.Left - client_width / 2;
            int y_offset = c1.Top - c2.Top - client_height / 2;
            txtLOSCFreq_MouseMove(sender, new MouseEventArgs(e.Button, e.Clicks, e.X + x_offset, e.Y + y_offset, e.Delta));
        }

        private void txtLOSCLSD_MouseDown(object sender, MouseEventArgs e)
        {
            txtLOSCMSD.Visible = false;
            txtLOSCLSD.Visible = false;
            txtLOSCFreq.Focus();
            txtLOSCFreq.SelectAll();
        }

        private void txtLOSCMSD_MouseLeave(object sender, EventArgs e)
        {
            txtLOSCFreq_MouseLeave(txtLOSCMSD, e);
        }

        private void txtLOSCMSD_MouseMove(object sender, MouseEventArgs e)
        {
            txtLOSCFreq_MouseMove(txtLOSCMSD,
                new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
        }

        private void txtLOSCMSD_MouseDown(object sender, MouseEventArgs e)
        {
            txtLOSCMSD.Visible = false;
            txtLOSCLSD.Visible = false;
            txtLOSCFreq.Focus();
            txtLOSCFreq.SelectAll();
        }

        private void txtLOSCFreq_MouseLeave(object sender, EventArgs e)
        {
            losc_hover_digit = -1;
            panelVFOAHover.Invalidate();
        }

        private void txtLOSCLSD_MouseMove(object sender, MouseEventArgs e)
        {
            Control c1 = (Control)sender;
            Control c2 = txtLOSCFreq;
            int client_width = (c1.Size.Width - c1.ClientSize.Width) + (c2.Size.Width - c2.ClientSize.Width);
            int client_height = (c1.Size.Height - c1.ClientSize.Height) + (c2.Size.Height - c2.ClientSize.Height);
            int x_offset = c1.Left - c2.Left - client_width / 2;
            int y_offset = c1.Top - c2.Top - client_height / 2;
            txtLOSCFreq_MouseMove(sender, new MouseEventArgs(e.Button, e.Clicks, e.X + x_offset, e.Y + y_offset, e.Delta));
        }

        private void txtLOSCFreq_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.ContainsFocus)
            {
                int old_digit = losc_hover_digit;
                int digit_index = 0;
                if (losc_char_width == 0)
                    GetLOSCCharWidth();

                int x = txtLOSCFreq.Width - (losc_pixel_offset - 5);
                while (x < e.X)
                {
                    digit_index++;

                    if (small_lsd && txtLOSCLSD.Visible)
                    {
                        if (digit_index < 6)
                            x += (losc_char_width + losc_char_space);
                        else
                            x += (losc_small_char_width + losc_small_char_space);

                        if (digit_index == 3)
                            x += (losc_decimal_space - losc_char_space);
                        if (digit_index == 6)
                            x += losc_small_char_width;
                    }
                    else
                    {
                        x += losc_char_width;
                        if (digit_index == 3)
                            x += losc_decimal_space;
                        else
                            x += losc_char_space;
                    }
                }

                if (digit_index < 3) digit_index = -1;
                if (digit_index > 9) digit_index = 9;
                losc_hover_digit = digit_index;
                if (losc_hover_digit != old_digit)
                    panelLOSCHover.Invalidate();
            }
        }

        private void txtLOSCFreq_LostFocus(object sender, System.EventArgs e)
        {
            if (txtLOSCFreq.Text == "." || txtLOSCFreq.Text == "")
            {
                LOSCFreq = saved_losc_freq;
                return;
            }

            double loscfreq = double.Parse(txtLOSCFreq.Text);
            UpdateLOSCFreq(loscfreq.ToString("f6"));
            Display.LOSC = (long)(loscfreq * 1e6);

            if (small_lsd)
            {
                txtLOSCMSD.Visible = true;
                txtLOSCLSD.Visible = true;
            }
        }

        private void txtLOSCFreq_KeyPress(object sender, KeyPressEventArgs e)
        {
            string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            int KeyCode = (int)e.KeyChar;
            if ((KeyCode < 48 || KeyCode > 57) &&			// numeric keys
                KeyCode != 8 &&								// backspace
                !e.KeyChar.ToString().Equals(separator) &&	// decimal
                KeyCode != 27)								// escape
            {
                e.Handled = true;
            }
            else
            {
                if (e.KeyChar.ToString().Equals(separator))
                {
                    e.Handled = (((TextBoxTS)sender).Text.IndexOf(separator) >= 0);
                }
                else if (KeyCode == 27)
                {
                    LOSCFreq = saved_losc_freq;
                    btnHidden.Focus();
                }
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtLOSCFreq_LostFocus(txtLOSCFreq, new System.EventArgs());
                btnHidden.Focus();
            }
        }
        #endregion

        // yt7pwr
        #region Waterfall

        private void picWaterfall_MouseDown(object sender, MouseEventArgs e)
        {
            picDisplay_MouseDown(sender, e);
        }

        private void picWaterfall_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) // yt7pwr
        {
            try
            {
                if (SetupForm.AlwaysOnTop)
                    picDisplay.Focus();
                int vfoa_x = 0;
                int low_x = 0;
                int high_x = 0;
                if (!chkMOX.Checked)
                {
                    vfoa_x = HzToPixel((float)((vfoAFreq - loscFreq) * 1e6));
                    low_x = vfoa_x + (HzToPixel(DttSP.RXFilterLowCut) - HzToPixel(0.0f));
                    high_x = vfoa_x + (HzToPixel(DttSP.RXFilterHighCut) - HzToPixel(0.0f));
                }
                else
                {
                    low_x = HzToPixel(DttSP.TXFilterLowCut);
                    high_x = HzToPixel(DttSP.TXFilterHighCut);
                }

                int vfob_x = 0;
                int vfob_low_x = 0;
                int vfob_high_x = 0;
                if (chkEnableSubRX.Checked && !chkMOX.Checked)
                {
                    vfob_x = HzToPixel((float)((vfoBFreq - loscFreq) * 1e6));
                    vfob_low_x = vfob_x + (HzToPixel(DttSP.RXFilterLowCut) - HzToPixel(0.0f));
                    vfob_high_x = vfob_x + (HzToPixel(DttSP.RXFilterHighCut) - HzToPixel(0.0f));
                }

                switch (Display.CurrentDisplayMode)
                {
                    case DisplayMode.WATERFALL:
                        DisplayCursorX = e.X;
                        DisplayCursorY = e.Y;
                        float x = PixelToHz(e.X);
                        float y = PixelToDb(e.Y);
                        double freq = loscFreq + (double)x * 0.0000010;
                        txtDisplayCursorOffset.Text = x.ToString("f1") + "Hz";
//                        txtDisplayCursorPower.Text = y.ToString("f1") + "dBm";

                        string temp_text = freq.ToString("f6") + " MHz";
                        int jper = temp_text.IndexOf(separator) + 4;
                        txtDisplayCursorFreq.Text = String.Copy(temp_text.Insert(jper, " "));
                        break;
                    case DisplayMode.PANAFALL:
                    case DisplayMode.PANADAPTER:
                        DisplayCursorX = e.X;
                        DisplayCursorY = e.Y;
                        x = PixelToHz(e.X);
                        y = PixelToDb(e.Y);
                        freq = loscFreq + (double)x * 0.0000010;
                        txtDisplayCursorOffset.Text = x.ToString("f1") + "Hz";
//                        txtDisplayCursorPower.Text = y.ToString("f1") + "dBm";

                        temp_text = freq.ToString("f6") + " MHz";
                        jper = temp_text.IndexOf(separator) + 4;
                        txtDisplayCursorFreq.Text = String.Copy(temp_text.Insert(jper, " "));

                        if (current_click_tune_mode == ClickTuneMode.Off && current_dsp_mode != DSPMode.DRM)
                        {
                            if (Cursor != Cursors.Hand)
                            {
                                if (Math.Abs(e.X - low_x) < 3 || Math.Abs(e.X - high_x) < 3 ||
                                    high_filter_drag || low_filter_drag ||
                                    (chkEnableSubRX.Checked && (e.X > vfob_low_x - 3 && e.X < vfob_high_x + 3)))
                                {
                                    Cursor = Cursors.SizeWE;
                                }
                                else if (e.X > low_x && e.X < high_x)
                                {
                                    Cursor = Cursors.NoMoveHoriz;
                                }
                                else
                                {
                                    Cursor = Cursors.Cross;
                                }
                            }

                            if (high_filter_drag)
                            {
                                if (!chkMOX.Checked)
                                {
                                    SelectVarFilter();
                                    float zero = 0.0F;
                                    int new_high = 0;
                                    if (tbDisplayZoom.Value == 4)
                                    {
                                        new_high = (int)(PixelToHz((e.X - vfoa_x) + picDisplay.Width / 2));
                                        UpdateFilters(DttSP.RXFilterLowCut, new_high);
                                    }
                                    else
                                    {
                                        new_high = (int)(PixelToHz(e.X - vfoa_x) + DttSP.RXDisplayHigh / 2);
                                        x = PixelToHz((e.X - vfoa_x));
                                        x -= PixelToHz(zero);
                                        UpdateFilters(DttSP.RXFilterLowCut, (int)x);
                                    }
                                }
                                else
                                {
                                    int new_high = (int)Math.Max(PixelToHz(e.X), DttSP.TXFilterLowCut + 10);
                                    switch (current_dsp_mode)
                                    {
                                        case DSPMode.LSB:
                                        case DSPMode.CWL:
                                        case DSPMode.DIGL:
                                            int new_low = -new_high;
                                            SetupForm.TXFilterLow = new_low;
                                            break;
                                        case DSPMode.USB:
                                        case DSPMode.CWU:
                                        case DSPMode.DIGU:
                                        case DSPMode.AM:
                                        case DSPMode.SAM:
                                        case DSPMode.FMN:
                                        case DSPMode.DSB:
                                            SetupForm.TXFilterHigh = new_high;
                                            break;
                                    }
                                }
                            }
                            else if (low_filter_drag)
                            {
                                if (!chkMOX.Checked)
                                {
                                    SelectVarFilter();
                                    float zero = 0.0F;
                                    int new_low = 0;
                                    if (tbDisplayZoom.Value == 4)
                                    {
                                        new_low = (int)(PixelToHz((e.X - vfoa_x) + picDisplay.Width / 2));
                                        UpdateFilters(new_low, DttSP.RXFilterHighCut);
                                    }
                                    else
                                    {
                                        new_low = (int)(PixelToHz(e.X - vfoa_x) + DttSP.RXDisplayHigh / 2);
                                        x = PixelToHz((e.X - vfoa_x));
                                        x -= PixelToHz(zero);
                                        UpdateFilters((int)x, DttSP.RXFilterHighCut);
                                    }

                                }
                                else
                                {
                                    int new_low = (int)(Math.Min(PixelToHz(e.X), DttSP.TXFilterHighCut - 10));
                                    switch (current_dsp_mode)
                                    {
                                        case DSPMode.LSB:
                                        case DSPMode.CWL:
                                        case DSPMode.DIGL:
                                        case DSPMode.AM:
                                        case DSPMode.SAM:
                                        case DSPMode.FMN:
                                        case DSPMode.DSB:
                                            int new_high = -new_low;
                                            SetupForm.TXFilterHigh = new_high;
                                            break;
                                        case DSPMode.USB:
                                        case DSPMode.CWU:
                                        case DSPMode.DIGU:
                                            SetupForm.TXFilterLow = new_low;
                                            break;
                                    }
                                }
                            }
                            else if (whole_filter_drag)
                            {
                                int diff = (int)(PixelToHz(e.X) - PixelToHz(whole_filter_start_x));

                                if (!chkMOX.Checked)
                                {
                                    UpdateFilters(whole_filter_start_low + diff, whole_filter_start_high + diff);
                                }
                                else
                                {
                                    switch (current_dsp_mode)
                                    {
                                        case DSPMode.LSB:
                                        case DSPMode.DIGL:
                                            SetupForm.TXFilterLow = whole_filter_start_low - diff;
                                            SetupForm.TXFilterHigh = whole_filter_start_high - diff;
                                            break;
                                        case DSPMode.USB:
                                        case DSPMode.DIGU:
                                            SetupForm.TXFilterLow = whole_filter_start_low + diff;
                                            SetupForm.TXFilterHigh = whole_filter_start_high + diff;
                                            break;
                                        case DSPMode.AM:
                                        case DSPMode.SAM:
                                        case DSPMode.FMN:
                                        case DSPMode.DSB:
                                            SetupForm.TXFilterHigh = whole_filter_start_high + diff;
                                            break;
                                    }
                                }
                            }
                            else if (vfoA_drag)
                            {
                                int diff = (int)(PixelToHz(e.X) - PixelToHz(vfoA_drag_last_x));
                                VFOAFreq = vfoA_drag_start_freq + diff * 1e-6;
                            }
                            else if (vfob_drag)
                            {
                                int diff = (int)(PixelToHz(e.X) - PixelToHz(vfob_drag_last_x));
                                VFOBFreq = vfob_drag_start_freq + diff * 1e-6;
                            }
                        }
                        break;
                    default:
                        txtDisplayCursorOffset.Text = "";
                        txtDisplayCursorPower.Text = "";
                        txtDisplayCursorFreq.Text = "";
                        break;
                }

                double zoom_factor = tbDisplayZoom.Value / 16;
                if (spectrum_drag && zoom_factor > 1)
                {
                    if (!chkMOX.Checked)
                    {
                        float start_freq = PixelToHz(spectrum_drag_last_x);
                        float end_freq = PixelToHz(e.X);
                        spectrum_drag_last_x = e.X;
                        float delta = end_freq - start_freq;
                        VFOAFreq -= delta * 0.0000010;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void picWaterfall_MouseLeave(object sender, EventArgs e)
        {
            picDisplay_MouseLeave(sender, e);
        }

        private void picWaterfall_MouseUp(object sender, MouseEventArgs e)
        {
            picDisplay_MouseUp(sender, e);
        }

        private void picWaterfall_Paint(object sender, PaintEventArgs e)
        {
            switch (current_display_engine)
            {
                case DisplayEngine.GDI_PLUS:
                    {
                        switch (Display.CurrentDisplayMode)
                        {
                            case (DisplayMode.PANAFALL):
                            case (DisplayMode.WATERFALL):
                            case(DisplayMode.PANADAPTER):
                                Display.RenderGDIPlusWaterfall(ref e);
                                break;
                            default:
                                Display.RenderGDIPlus(ref e);
                                break;
                        }
                    }
                    break;

                case DisplayEngine.DIRECT_X:
      /*              Thread t = new Thread(new ThreadStart(Display.RenderDirectX));
                    t.Name = "DirectX Background Update";
                    t.IsBackground = true;
                    t.Priority = ThreadPriority.Normal;
                    t.Start();*/
                    break;
            }
        }

        private void picWaterfall_Resize(object sender, EventArgs e)
        {
            Display.Target = picWaterfall;
            Display.Init();
            Display.DrawBackground();
            UpdateDisplay();
        }

        #endregion

        #region CAT Properties

        // props for cat control 
        // Added 06/20/05 BT for CAT commands
        private int cat_nr_status = 0;
        public int CATNR
        {
            get { return cat_nr_status; }
            set
            {
                if (value == 0)
                    chkNR.Checked = false;
                else if (value == 1)
                    chkNR.Checked = true;
            }
        }

        // Added 06/20/05 BT for CAT commands
        private int cat_anf_status = 0;
        public int CATANF
        {
            get { return cat_anf_status; }
            set
            {
                if (value == 0)
                    chkANF.Checked = false;
                else if (value == 1)
                    chkANF.Checked = true;
            }
        }

        // Added 06/21/05 BT for CAT Commands
        private int cat_nb1_status = 0;
        public int CATNB1
        {
            get { return cat_nb1_status; }
            set
            {
                if (value == 0)
                    chkNB.Checked = false;
                else if (value == 1)
                    chkNB.Checked = true;
            }
        }

        // Added 06/21/05 BT for CAT commands
        private int cat_nb2_status = 0;
        public int CATNB2
        {
            get { return cat_nb2_status; }
            set
            {
                if (value == 0)
                    chkDSPNB2.Checked = false;
                else if (value == 1)
                    chkDSPNB2.Checked = true;
            }
        }

        // Added 06/22/05 BT for CAT commands
        private int cat_cmpd_status = 0;
        public int CATCmpd
        {
            get { return cat_cmpd_status; }
            set
            {
                if (value == 0)
                    chkDSPCompander.Checked = false;
                else if (value == 1)
                    chkDSPCompander.Checked = true;
            }
        }

        // Added 06/22/05 BT for CAT commands
        private int cat_mic_status = 0;
        public int CATMIC
        {
            get
            {
                cat_mic_status = (int)udMIC.Value;
                return cat_mic_status;
            }
            set
            {
                value = Math.Max(0, value);
                value = Math.Min(100, value);
                udMIC.Value = value;
            }
        }

        // Added 06/22/05 BT for CAT commands
        // modified 07/22/05 to fix display problem
        private int cat_filter_width = 0;
        public int CATFilterWidth
        {
            get
            {
                cat_filter_width = tbFilterWidth.Value;
                return cat_filter_width;
            }
            set
            {
                value = Math.Max(1, value);
                value = Math.Min(10000, value);
                tbFilterWidth.Value = value;
                tbFilterWidth_Scroll(this.tbFilterWidth, EventArgs.Empty);	// added
            }
        }

        // Added 07/22/05 for cat commands
        public int CATFilterShift
        {
            get
            {
                return tbFilterShift.Value;
            }
            set
            {
                value = Math.Max(-1000, value);
                value = Math.Min(1000, value);
                tbFilterShift.Value = value;
                tbFilterShift_Scroll(this.tbFilterShift, EventArgs.Empty);
            }
        }

        // Added 07/22/05 for CAT commands
        public int CATFilterShiftReset
        {
            set
            {
                if (value == 1)
                    btnFilterShiftReset.PerformClick();
            }
        }

        // Added 06/22/05 BT for CAT commands
        private int cat_bin_status = 0;
        public int CATBIN
        {
            get
            {
                if (chkBIN.Checked)
                    cat_bin_status = 1;
                else
                    cat_bin_status = 0;

                return cat_bin_status;
            }
            set
            {
                if (value == 1)
                    chkBIN.Checked = true;
                else if (value == 0)
                    chkBIN.Checked = false;
            }
        }

        // Added/repaired 7/10/05 BT for cat commands
        public int CATPreamp
        {
            set { comboPreamp.SelectedIndex = value; }
            get { return comboPreamp.SelectedIndex; }
        }

        // Added 06/30/05 BT for CAT commands
        public int CATCWSpeed
        {
            get
            {
                return (int)udCWSpeed.Value;
            }
            set
            {
                value = Math.Max(1, value);
                value = Math.Min(60, value);
                udCWSpeed.Value = value;
            }
        }

        // Added 06/30/05 BT for CAT commands
        private int cat_display_avg_status = 0;
        public int CATDisplayAvg
        {
            get
            {
                if (chkDisplayAVG.Checked)
                    cat_display_avg_status = 1;
                else
                    cat_display_avg_status = 0;

                return cat_display_avg_status;
            }
            set
            {
                if (value == 1)
                    chkDisplayAVG.Checked = true;
                else
                    chkDisplayAVG.Checked = false;
            }
        }

        // Added 06/30/05 BT for CAT commands
        private int cat_squelch_status = 0;
        public int CATSquelch
        {
            get
            {
                if (chkSquelch.Checked)
                    cat_squelch_status = 1;
                else
                    cat_squelch_status = 0;

                return cat_squelch_status;
            }
            set
            {
                if (value == 1)
                    chkSquelch.Checked = true;
                else
                    chkSquelch.Checked = false;
            }
        }

        // Added 7/9/05 BT for cat commands
        public string CATQMSValue
        {
            get { return this.txtMemory.Text; }
        }

        private SDRSerialSupportII.SDRSerialPort.Parity cat_parity;
        public SDRSerialSupportII.SDRSerialPort.Parity CATParity
        {
            set { cat_parity = value; }
            get { return cat_parity; }
        }


        private SDRSerialSupportII.SDRSerialPort.StopBits cat_stop_bits;
        public SDRSerialSupportII.SDRSerialPort.StopBits CATStopBits
        {
            set { cat_stop_bits = value; }
            get { return cat_stop_bits; }
        }
        private SDRSerialSupportII.SDRSerialPort.DataBits cat_data_bits;
        public SDRSerialSupportII.SDRSerialPort.DataBits CATDataBits
        {
            set { cat_data_bits = value; }
            get { return cat_data_bits; }
        }

        private int cat_baud_rate;
        public int CATBaudRate
        {
            set { cat_baud_rate = value; }
            get { return cat_baud_rate; }
        }

        private bool cat_enabled;
        public bool CATEnabled
        {
            set
            {
                cat_enabled = value;
                Keyer.CATEnabled = value;
                if (siolisten != null)  // if we've got a listener tell them about state change 
                {
                    if (cat_enabled)
                    {
                        Siolisten.enableCAT();
                    }
                    else
                    {
                        Siolisten.disableCAT();
                    }
                }
            }
            get { return cat_enabled; }
        }

        private int cat_rig_type;
        public int CATRigType
        {
            get { return cat_rig_type; }
            set { cat_rig_type = value; }
        }

        private int cat_port;
        public int CATPort
        {
            get { return cat_port; }
            set { cat_port = value; }
        }

        private bool cat_ptt_rts = false;
        public bool CATPTTRTS
        {
            get { return cat_ptt_rts; }
            set { cat_ptt_rts = value; }
        }

        private bool cat_ptt_dtr;
        public bool CATPTTDTR
        {
            get { return cat_ptt_dtr; }
            set { cat_ptt_dtr = value; }
        }

        public SerialPortPTT serialPTT = null;
        private bool ptt_bit_bang_enabled;
        public bool PTTBitBangEnabled  // changes yt7pwr
        {
            get { return ptt_bit_bang_enabled; }
            set
            {
                ptt_bit_bang_enabled = value;
                if (serialPTT != null)  // kill current serial PTT if we have one 
                {
                    serialPTT.Close();
                    serialPTT = null;
                }
                if (ptt_bit_bang_enabled)
                {
                    // wjt -- don't really like popping a msg box in here ...   nasty when we do a remoted 
                    // setup ... will let that wait for the great console refactoring 
                    try
                    {
                        string ptt = SetupForm.comboCATPTTPort.Text;
                        serialPTT = new SerialPortPTT(CATPTTBingBangPort_name, cat_ptt_dtr, cat_ptt_rts);
                        serialPTT.Init();
                    }
                    catch (Exception ex)
                    {
                        ptt_bit_bang_enabled = false;
                        if (SetupForm != null)
                        {
                            SetupForm.copyCATPropsToDialogVars(); // need to make sure the props on the setup page get reset 
                        }
                        MessageBox.Show("Could not initialize PTT Bit Bang control.  Exception was:\n\n " + ex.Message +
                            "\n\nPTT Bit Bang control has been disabled.", "Error Initializing PTT control",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
            }
        }

        private int cat_ptt_bit_bang_port;
        public int CATPTTBitBangPort
        {
            get { return cat_ptt_bit_bang_port; }
            set { cat_ptt_bit_bang_port = value; }
        }

        private string cat_ptt_name = "COM1";
        public string CATPTTBingBangPort_name
        {
            get { return cat_ptt_name; }

            set { cat_ptt_name = value; }
        }

        #endregion

        // yt7pwr
        #region USB events

        public void chkUSB_Click(object sender, EventArgs e)
        {
            ReInit_USB();
        }

        public bool ReInit_USB()
        {
            bool result = false;

            if (!booting)
            {
                if (usb_si570_enable == true && SetupForm.chkGeneralUSBPresent.Checked)
                {
                    chkUSB.BackColor = Color.Red;
                    result = SI570.Init_USB();
                }
                else if (current_model == Model.GENESIS_G59)
                {
                    if (!g59.Connected)
                    {
                        result = g59.Connect();
                        if (result)
                            chkUSB.BackColor = Color.GreenYellow;
                        else
                            chkUSB.BackColor = Color.Red;
                    }
                }
                else
                {
                    chkUSB.BackColor = Color.Red;
                    g59.Disconnect();
                    g59.Connected = false;
                }
            }

            return result;
        }

        #endregion

        #region About // yt7pwr
        private void mnuAbout_Click(object sender, EventArgs e) // yt7pwr
        {
            AboutForm = new About();
            AboutForm.Show();
            AboutForm.Focus();
        }
        #endregion

        #region Auto focus // yt7pwr
        private void Console_SizeChanged(object sender, EventArgs e)
        {
            Console_Resize(sender, e);
        }

        private void tbAF_MouseMove(object sender, MouseEventArgs e)
        {
            tbAF.Focus();
        }

        private void tbRF_MouseMove(object sender, MouseEventArgs e)
        {
            tbRF.Focus();
        }

        private void tbPWR_MouseMove(object sender, MouseEventArgs e)
        {
            tbPWR.Focus();
        }

        private void tbSQL_MouseMove(object sender, MouseEventArgs e)
        {
            tbSQL.Focus();
        }

        private void tbFilterWidth_MouseMove(object sender, MouseEventArgs e)
        {
            tbFilterWidth.Focus();
        }

        private void tbFilterShift_MouseMove(object sender, MouseEventArgs e)
        {
            tbFilterShift.Focus();
        }
        #endregion

        // yt7pwr
        #region Genesis button // yt7pwr

        private void btnHIGH_RF_Click(object sender, EventArgs e)
        {
            if (!usb_si570_enable)
            {
                if (btnHIGH_RF.BackColor == SystemColors.Control)
                {
                    btnHIGH_RF.BackColor = Color.GreenYellow;
                    g59.WriteToDevice(11, 0);               // PREAMP ON
                }
                else
                {
                    btnHIGH_RF.BackColor = SystemColors.Control;
                    g59.WriteToDevice(12, 0);               // PREAMP OFF
                }
            }
        }

        private void btnHIGH_AF_Click(object sender, EventArgs e)
        {
            if (!usb_si570_enable)
            {
                if (btnHIGH_AF.BackColor == SystemColors.Control)
                {
                    btnHIGH_AF.BackColor = Color.GreenYellow;
                    g59.WriteToDevice(5, 0);                // AF ON
                }
                else
                {
                    btnHIGH_AF.BackColor = SystemColors.Control;
                    g59.WriteToDevice(6, 0);                // AF OFF
                }
            }
        }

        private void btnATT_Click(object sender, EventArgs e)
        {
            if (!usb_si570_enable)
            {
                if (btnATT.BackColor == SystemColors.Control)
                {
                    btnATT.BackColor = Color.GreenYellow;
                    g59.WriteToDevice(4, 3);                // ATT ON
                }
                else
                {
                    btnATT.BackColor = SystemColors.Control;
                    g59.WriteToDevice(4, 0);                // ATT OFF
                }
            }
        }

        public void btnG80_X1_Click(object sender, EventArgs e)
        {
            LOSCFreq = G80Xtal1;
            btnG80_X1.Text = G80Xtal1.ToString();
            btnG80_X1.BackColor = button_selected_color;
            btnG80_X2.BackColor = SystemColors.Control;
            btnG80_X3.BackColor = SystemColors.Control;
            btnG80_X4.BackColor = SystemColors.Control;
        }

        public void btnG80_X2_Click(object sender, EventArgs e)
        {
            LOSCFreq = G80Xtal2;
            btnG80_X2.Text = G80Xtal2.ToString();
            btnG80_X1.BackColor = SystemColors.Control;
            btnG80_X2.BackColor = button_selected_color;
            btnG80_X3.BackColor = SystemColors.Control;
            btnG80_X4.BackColor = SystemColors.Control;
        }

        public void btnG80_X3_Click(object sender, EventArgs e)
        {
            LOSCFreq = G80Xtal3;
            btnG80_X3.Text = G80Xtal3.ToString();
            btnG80_X1.BackColor = SystemColors.Control;
            btnG80_X2.BackColor = SystemColors.Control;
            btnG80_X3.BackColor = button_selected_color;
            btnG80_X4.BackColor = SystemColors.Control;
        }

        public void btnG80_X4_Click(object sender, EventArgs e)
        {
            LOSCFreq = G80Xtal4;
            btnG80_X4.Text = G80Xtal4.ToString();
            btnG80_X1.BackColor = SystemColors.Control;
            btnG80_X2.BackColor = SystemColors.Control;
            btnG80_X3.BackColor = SystemColors.Control;
            btnG80_X4.BackColor = button_selected_color;
        }

        public void btnG160_X1_Click(object sender, EventArgs e)
        {
            LOSCFreq = G160Xtal1;
            btnG160_X1.Text = G160Xtal1.ToString();
            btnG160_X1.BackColor = button_selected_color;
            btnG160_X2.BackColor = SystemColors.Control;
        }

        public void btnG160_X2_Click(object sender, EventArgs e)
        {
            LOSCFreq = G160Xtal2;
            btnG160_X2.Text = G160Xtal2.ToString();
            btnG160_X1.BackColor = SystemColors.Control;
            btnG160_X2.BackColor = button_selected_color;
        }

        public void btnG3020_X1_Click(object sender, EventArgs e)
        {
            LOSCFreq = G3020Xtal1;
            btnG3020_X1.Text = G3020Xtal1.ToString();
            btnG3020_X1.BackColor = button_selected_color;
            btnG3020_X2.BackColor = SystemColors.Control;
            btnG3020_X3.BackColor = SystemColors.Control;
            btnG3020_X4.BackColor = SystemColors.Control;
        }

        public void btnG3020_X2_Click(object sender, EventArgs e)
        {
            LOSCFreq = G3020Xtal2;
            btnG3020_X2.Text = G3020Xtal2.ToString();
            btnG3020_X1.BackColor = SystemColors.Control;
            btnG3020_X2.BackColor = button_selected_color;
            btnG3020_X3.BackColor = SystemColors.Control;
            btnG3020_X4.BackColor = SystemColors.Control;
        }

        public void btnG3020_X3_Click(object sender, EventArgs e)
        {
            LOSCFreq = G3020Xtal3;
            btnG3020_X3.Text = G3020Xtal3.ToString();
            btnG3020_X1.BackColor = SystemColors.Control;
            btnG3020_X2.BackColor = SystemColors.Control;
            btnG3020_X3.BackColor = button_selected_color;
            btnG3020_X4.BackColor = SystemColors.Control;
        }

        public void btnG3020_X4_Click(object sender, EventArgs e)
        {
            LOSCFreq = G3020Xtal4;
            btnG3020_X4.Text = G3020Xtal4.ToString();
            btnG3020_X1.BackColor = SystemColors.Control;
            btnG3020_X2.BackColor = SystemColors.Control;
            btnG3020_X3.BackColor = SystemColors.Control;
            btnG3020_X4.BackColor = button_selected_color;
        }

        public void btnG40_X1_Click(object sender, EventArgs e)
        {
            LOSCFreq = G40Xtal1;
            btnG40_X1.Text = G40Xtal1.ToString();
            btnG40_X1.BackColor = button_selected_color;
        }

        private void xtalToolStripMenuItem(object sender, EventArgs e)
        {
            try
            {
                switch (current_model)
                {
                    case Model.GENESIS_G3020:
                        {
                            if (btnG3020_X1.BackColor == button_selected_color)
                                SetupForm.udG3020Xtal1.Value = (decimal)LOSCFreq;
                            else if (btnG3020_X2.BackColor == button_selected_color)
                                SetupForm.udG3020Xtal2.Value = (decimal)LOSCFreq;
                            else if (btnG3020_X3.BackColor == button_selected_color)
                                SetupForm.udG3020Xtal3.Value = (decimal)LOSCFreq;
                            else if (btnG3020_X4.BackColor == button_selected_color)
                                SetupForm.udG3020Xtal4.Value = (decimal)LOSCFreq;
                            else
                                SetupForm.udG3020Xtal1.Value = (decimal)LOSCFreq;
                        }
                        break;
                    case Model.GENESIS_G40:
                        SetupForm.udG40Xtal1.Value = (decimal)LOSCFreq;
                        break;
                    case Model.GENESIS_G80:
                        {
                            if (btnG80_X1.BackColor == button_selected_color)
                                SetupForm.udG80Xtal1.Value = (decimal)LOSCFreq;
                            else if (btnG80_X2.BackColor == button_selected_color)
                                SetupForm.udG80Xtal2.Value = (decimal)LOSCFreq;
                            else if (btnG80_X3.BackColor == button_selected_color)
                                SetupForm.udG80Xtal3.Value = (decimal)LOSCFreq;
                            else if (btnG80_X4.BackColor == button_selected_color)
                                SetupForm.udG80Xtal4.Value = (decimal)LOSCFreq;
                            else
                                SetupForm.udG80Xtal1.Value = (decimal)LOSCFreq;
                        }
                        break;
                    case Model.GENESIS_G160:
                        {
                            if (btnG160_X1.BackColor == button_selected_color)
                                SetupForm.udG160Xtal1.Value = (decimal)LOSCFreq;
                            else if (btnG160_X2.BackColor == button_selected_color)
                                SetupForm.udG160Xtal2.Value = (decimal)LOSCFreq;
                            else
                                SetupForm.udG160Xtal1.Value = (decimal)LOSCFreq;
                        }
                        break;
                }
            }
            catch
            {
                MessageBox.Show("Wrong value!");
            }
        }

        private void btnVFOA_Click(object sender, EventArgs e)
        {
            LOSCFreq = losc_restore;
            VFOAFreq = vfoa_restore;
            VFOBFreq = vfob_restore;
            tbDisplayZoom.Value = zoom_restore;
            tbDisplayZoom_Scroll(sender, e);
            tbDisplayPan.Value = pan_restore;
            SetFilter(filter_restore);
            SetMode(mode_restore);
            CalcDisplayFreq();
        }

        #endregion

        // yt7pwr
        #region Memory Events // yt7pwr
        // ======================================================
        // Memory Events
        // ======================================================

        private void btnMemoryQuickSave_Click(object sender, System.EventArgs e)
        {
            int number;
            Int32.TryParse(lblMemoryNumber.Text, out number);
            if (DB.SaveMemory(number, VFOAFreq, LOSCFreq, (int)CurrentDSPMode, (int)current_filter, StepSize,
                comboAGC.SelectedIndex, (int)udSquelch.Value, (int)tbDisplayZoom.Value, (int)tbDisplayPan.Value))
                txtMemory_fill();
        }

        private void btnMemoryQuickRestore_Click(object sender, System.EventArgs e)
        {
            int number;
            Int32.TryParse(lblMemoryNumber.Text, out number);
            double vfoa, losc_freq;
            int modeID, filterID,stepID,agcID,squelchID, zoomID, panID;
            bool in_use = false;
            ArrayList memory;

            // first save data for restoring

            vfoa_restore = vfoAFreq;
            vfob_restore = vfoBFreq;
            losc_restore = loscFreq;
            filter_restore = current_filter;
            zoom_restore = tbDisplayZoom.Value;
            pan_restore = tbDisplayPan.Value;
            mode_restore = (DSPMode)DttSP.CurrentMode;

            memory = DB.GetMemory(number);
            foreach (string s in memory)				// string is in the format "freq,losc,mode,filter,cleared"
            {
                string[] vals = s.Split('/');

                string freq = vals[1];
                Double.TryParse(freq, out vfoa);
                string losc = vals[2];
                Double.TryParse(losc, out losc_freq);
                string mode = vals[3];
                Int32.TryParse(mode, out modeID);
                string filter = vals[4];
                Int32.TryParse(filter, out filterID);
                string step = vals[5];
                Int32.TryParse(step, out stepID);
                string  agc = vals[6];
                Int32.TryParse(agc, out agcID);
                string squelch = vals[7];
                Int32.TryParse(squelch, out squelchID);
                string DisplayZoom = vals[8];
                Int32.TryParse(DisplayZoom, out zoomID);
                string DisplayPan = vals[9];
                Int32.TryParse(DisplayPan, out panID);
                string cleared = vals[10];
                bool.TryParse(cleared, out in_use);

                if (!in_use)
                {
                    MemoryRecall(modeID, filterID, vfoa, losc_freq, stepID, agcID, squelchID);
                    tbDisplayPan.Value = panID;
                    tbDisplayZoom.Value = zoomID;
                    CalcDisplayFreq();
                }
            }
        }

        private void btnEraseMemory_Click(object sender, EventArgs e)
        {
            int number;
            Int32.TryParse(lblMemoryNumber.Text, out number);
            if (DB.ClearMemory(number))
                txtMemory_fill();
        }

        private void txtMemory_fill()
        {
            int number;
            Int32.TryParse(lblMemoryNumber.Text, out number);
            double vfoa;
            DSPMode mem_mode;
            int modeID;
            ArrayList memory;

            memory = DB.GetMemory(number);

            foreach (string s in memory)				// string is in the format "freq,losc,mode,filter,cleared"
            {
                bool in_use = false;
                string[] vals = s.Split('/');

                string freq = vals[1];
                Double.TryParse(freq, out vfoa);
                string mode = vals[3];
                Int32.TryParse(mode, out modeID);
                mem_mode = (DSPMode)modeID;
                string cleared = vals[10];
                bool.TryParse(cleared, out in_use);

                if (!in_use)
                {
                    txtMemory.Text = freq + mem_mode.ToString();
                    lblMemoryNumber.BackColor = Color.Blue;
                }
                else
                {
                    txtMemory.Text = "empty";
                    lblMemoryNumber.BackColor = Color.Red;
                }
            }
        }

        private void eraseAllMemoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DB.ClearMemoryTable();
            lblMemoryNumber_Click(sender, e);
        }

        private void lblMemoryNumber_MouseMove(object sender, MouseEventArgs e)
        {
            lblMemoryNumber.Focus();
        }

        private void lblMemoryNumber_Click(object sender, EventArgs e)
        {
            btnMemoryQuickRestore_Click(sender, e);
        }

        #endregion

        #region Wizard  // yt7pwr
        private void mnuWizard_Click(object sender, EventArgs e)
        {
            SetupWizard w = new SetupWizard(this, 0);
            w.ShowDialog();
        }
        #endregion
    }
}