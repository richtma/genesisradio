//=================================================================
// display.cs
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

using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;

//using Microsoft.DirectX;
//using Microsoft.DirectX.Direct3D;


namespace PowerSDR
{
    public enum RenderType
    {
        HARDWARE = 0,
        SOFTWARE,
        NONE,
    }

    class Display
    {
        #region Variable Declaration

        public static int counter = 0;

        public static Console console;
        //private static Mutex background_image_mutex;			// used to lock the base display image
        //private static Bitmap background_bmp;					// saved background picture for display
        //private static Bitmap display_bmp;					// Bitmap for use when drawing
        private static int waterfall_counter;
        private static Bitmap waterfall_bmp;					// saved waterfall picture for display
        //private static Graphics display_graphics;				// GDI graphics object
        public const float CLEAR_FLAG = -999.999F;				// for resetting buffers
        public const int BUFFER_SIZE = 4096;
        public static float[] new_display_data;					// Buffer used to store the new data from the DSP for the display
        public static float[] current_display_data;				// Buffer used to store the current data for the display
        public static float[] waterfall_display_data;            // Buffer for waterfall

        public static float[] average_buffer;					// Averaged display data buffer
        public static float[] peak_buffer;						// Peak hold display data buffer


        #endregion

        #region Properties

        private static int waterfall_update_period = 100; // in ms
        public static int WaterfallUpdatePeriod
        {
            get { return waterfall_update_period; }
            set { waterfall_update_period = value; }
        }

        public static float waterfall_avg_mult_old = 1 - (float)1 / 18;
        public static float waterfall_avg_mult_new = (float)1 / 18;
        private static int waterfall_avg_num_blocks = 18;
        public static int WaterfallAvgBlocks
        {
            get { return waterfall_avg_num_blocks; }
            set
            {
                waterfall_avg_num_blocks = value;
                waterfall_avg_mult_old = 1 - (float)1 / waterfall_avg_num_blocks;
                waterfall_avg_mult_new = (float)1 / waterfall_avg_num_blocks;
            }
        }

        private static Color sub_rx_zero_line_color = Color.LightSkyBlue;
        public static Color SubRXZeroLine
        {
            get { return sub_rx_zero_line_color; }
            set
            {
                sub_rx_zero_line_color = value;
                if (current_display_mode == DisplayMode.PANADAPTER && sub_rx_enabled)
                    DrawBackground();
            }
        }

        private static Color sub_rx_filter_color = Color.Blue;
        public static Color SubRXFilterColor
        {
            get { return sub_rx_filter_color; }
            set
            {
                sub_rx_filter_color = value;
                if (current_display_mode == DisplayMode.PANADAPTER && sub_rx_enabled)
                    DrawBackground();
            }
        }

        private static bool sub_rx_enabled = false;
        public static bool SubRXEnabled
        {
            get { return sub_rx_enabled; }
            set
            {
                sub_rx_enabled = value;
                if (current_display_mode == DisplayMode.PANADAPTER)
                    DrawBackground();
            }
        }

        private static bool split_enabled = false;
        public static bool SplitEnabled
        {
            get { return split_enabled; }
            set
            {
                split_enabled = value;
                if (current_display_mode == DisplayMode.PANADAPTER && draw_tx_filter)
                    DrawBackground();
            }
        }

        private static bool show_freq_offset = false;
        public static bool ShowFreqOffset
        {
            get { return show_freq_offset; }
            set
            {
                show_freq_offset = value;
                if (current_display_mode == DisplayMode.PANADAPTER)
                    DrawBackground();
            }
        }

        private static Color band_edge_color = Color.Red;
        public static Color BandEdgeColor
        {
            get { return band_edge_color; }
            set
            {
                band_edge_color = value;
                if (current_display_mode == DisplayMode.PANADAPTER)
                    DrawBackground();
            }
        }

        private static long losc_hz;
        public static long LOSC
        {
            get { return losc_hz; }
            set
            {
                losc_hz = value;
                if (current_display_mode == DisplayMode.PANADAPTER || current_display_mode == DisplayMode.PANAFALL)
                    DrawBackground();
            }
        }

        private static long vfoa_hz;
        public static long VFOA
        {
            get { return vfoa_hz; }
            set
            {
                vfoa_hz = value;
                if (current_display_mode == DisplayMode.PANADAPTER || current_display_mode == DisplayMode.PANAFALL)
                    DrawBackground();
            }
        }

        private static long vfob_hz;
        public static long VFOB
        {
            get { return vfob_hz; }
            set
            {
                vfob_hz = value;
                if ((current_display_mode == DisplayMode.PANADAPTER || current_display_mode == DisplayMode.PANAFALL)
                    || (current_display_mode == DisplayMode.PANADAPTER && sub_rx_enabled))
                    DrawBackground();
            }
        }

        private static int rit_hz;
        public static int RIT
        {
            get { return rit_hz; }
            set
            {
                rit_hz = value;
                if (current_display_mode == DisplayMode.PANADAPTER)
                    DrawBackground();
            }
        }

        private static int xit_hz;
        public static int XIT
        {
            get { return xit_hz; }
            set
            {
                xit_hz = value;
                if (current_display_mode == DisplayMode.PANADAPTER && (draw_tx_filter || mox))
                    DrawBackground();
            }
        }

        private static int cw_pitch = 600;
        public static int CWPitch
        {
            get { return cw_pitch; }
            set { cw_pitch = value; }
        }

        private static int H = 0;	// target height
        private static int W = 0;	// target width
        private static Control target = null;
        public static Control Target
        {
            get { return target; }
            set
            {
                target = value;
                H = target.Height;
                W = target.Width;
            }
        }

        private static int rx_display_low = -4000;
        public static int RXDisplayLow
        {
            get { return rx_display_low; }
            set { rx_display_low = value; }
        }

        private static int rx_display_high = 4000;
        public static int RXDisplayHigh
        {
            get { return rx_display_high; }
            set { rx_display_high = value; }
        }

        private static int tx_display_low = -4000;
        public static int TXDisplayLow
        {
            get { return tx_display_low; }
            set { tx_display_low = value; }
        }

        private static int tx_display_high = 4000;
        public static int TXDisplayHigh
        {
            get { return tx_display_high; }
            set { tx_display_high = value; }
        }

        private static float preamp_offset = 0.0f;
        public static float PreampOffset
        {
            get { return preamp_offset; }
            set { preamp_offset = value; }
        }

        private static float display_cal_offset;					// display calibration offset per volume setting in dB
        public static float DisplayCalOffset
        {
            get { return display_cal_offset; }
            set { display_cal_offset = value; }
        }

        private static Model current_model = Model.DR3X;
        public static Model CurrentModel
        {
            get { return current_model; }
            set { current_model = value; }
        }

        private static int display_cursor_x;						// x-coord of the cursor when over the display
        public static int DisplayCursorX
        {
            get { return display_cursor_x; }
            set { display_cursor_x = value; }
        }

        private static int display_cursor_y;						// y-coord of the cursor when over the display
        public static int DisplayCursorY
        {
            get { return display_cursor_y; }
            set { display_cursor_y = value; }
        }

        private static ClickTuneMode current_click_tune_mode = ClickTuneMode.Off;
        public static ClickTuneMode CurrentClickTuneMode
        {
            get { return current_click_tune_mode; }
            set { current_click_tune_mode = value; }
        }

        private static int scope_time = 50;
        public static int ScopeTime
        {
            get { return scope_time; }
            set { scope_time = value; }
        }

        private static int sample_rate = 48000;
        public static int SampleRate
        {
            get { return sample_rate; }
            set { sample_rate = value; }
        }

        private static bool high_swr = false;
        public static bool HighSWR
        {
            get { return high_swr; }
            set { high_swr = value; }
        }

        private static DisplayEngine current_display_engine = DisplayEngine.GDI_PLUS;
        public static DisplayEngine CurrentDisplayEngine
        {
            get { return current_display_engine; }
            set { current_display_engine = value; }
        }

        private static bool mox = false;
        public static bool MOX
        {
            get { return mox; }
            set { mox = value; }
        }

        private static DSPMode current_dsp_mode = DSPMode.USB;
        public static DSPMode CurrentDSPMode
        {
            get { return current_dsp_mode; }
            set { current_dsp_mode = value; }
        }

        private static DisplayMode current_display_mode = DisplayMode.PANADAPTER;
        public static DisplayMode CurrentDisplayMode
        {
            get { return current_display_mode; }
            set
            {
                //PrepareDisplayVars(value);

                current_display_mode = value;

                switch (current_display_mode)
                {
                    case DisplayMode.PANAFALL:
                    case DisplayMode.WATERFALL:
                    case DisplayMode.PANADAPTER:
                        //						DttSP.SetPWSmode(1);
                        DttSP.NotPan = false;
                        break;
                    default:
                        //						DttSP.SetPWSmode(0);
                        DttSP.NotPan = true;
                        break;
                }

                switch (current_display_mode)
                {
                    default:
                        Audio.phase = false;
                        break;
                }

                if (average_on) ResetDisplayAverage();
                if (peak_on) ResetDisplayPeak();

                DrawBackground();
            }
        }

        private static float max_x;								// x-coord of maxmimum over one display pass
        public static float MaxX
        {
            get { return max_x; }
            set { max_x = value; }
        }

        private static float max_y;								// y-coord of maxmimum over one display pass
        public static float MaxY
        {
            get { return max_y; }
            set { max_y = value; }
        }

        private static bool average_on;							// True if the Average button is pressed
        public static bool AverageOn
        {
            get { return average_on; }
            set
            {
                average_on = value;
                if (!average_on) ResetDisplayAverage();
            }
        }

        private static bool peak_on;							// True if the Peak button is pressed
        public static bool PeakOn
        {
            get { return peak_on; }
            set
            {
                peak_on = value;
                if (!peak_on) ResetDisplayPeak();
            }
        }

        private static bool data_ready;					// True when there is new display data ready from the DSP
        public static bool DataReady
        {
            get { return data_ready; }
            set { data_ready = value; }
        }

        public static float display_avg_mult_old = 1 - (float)1 / 8;
        public static float display_avg_mult_new = (float)1 / 8;
        private static int display_avg_num_blocks = 8;
        public static int DisplayAvgBlocks
        {
            get { return display_avg_num_blocks; }
            set
            {
                display_avg_num_blocks = value;
                display_avg_mult_old = 1 - (float)1 / display_avg_num_blocks;
                display_avg_mult_new = (float)1 / display_avg_num_blocks;
            }
        }

        private static int spectrum_grid_max = 0;
        public static int SpectrumGridMax
        {
            get { return spectrum_grid_max; }
            set
            {
                spectrum_grid_max = value;
                DrawBackground();
            }
        }

        private static int spectrum_grid_min = -150;
        public static int SpectrumGridMin
        {
            get { return spectrum_grid_min; }
            set
            {
                spectrum_grid_min = value;
                DrawBackground();
            }
        }

        private static int spectrum_grid_step = 10;
        public static int SpectrumGridStep
        {
            get { return spectrum_grid_step; }
            set
            {
                spectrum_grid_step = value;
                DrawBackground();
            }
        }

        private static Color grid_text_color = Color.Yellow;
        public static Color GridTextColor
        {
            get { return grid_text_color; }
            set
            {
                grid_text_color = value;
                DrawBackground();
            }
        }

        private static Color grid_zero_color = Color.Red;
        public static Color GridZeroColor
        {
            get { return grid_zero_color; }
            set
            {
                grid_zero_color = value;
                DrawBackground();
            }
        }

        private static Color grid_color = Color.Purple;
        public static Color GridColor
        {
            get { return grid_color; }
            set
            {
                grid_color = value;
                DrawBackground();
            }
        }

        private static Pen data_line_pen = new Pen(new SolidBrush(Color.LightGreen), display_line_width);
        private static Color data_line_color = Color.LightGreen;
        public static Color DataLineColor
        {
            get { return data_line_color; }
            set
            {
                data_line_color = value;
                data_line_pen = new Pen(new SolidBrush(data_line_color), display_line_width);
                DrawBackground();
            }
        }

        private static Color display_filter_color = Color.Green;
        public static Color DisplayFilterColor
        {
            get { return display_filter_color; }
            set
            {
                display_filter_color = value;
                DrawBackground();
            }
        }

        private static Color display_filter_tx_color = Color.Yellow;
        public static Color DisplayFilterTXColor
        {
            get { return display_filter_tx_color; }
            set
            {
                display_filter_tx_color = value;
                DrawBackground();
            }
        }

        private static bool draw_tx_filter = false;
        public static bool DrawTXFilter
        {
            get { return draw_tx_filter; }
            set
            {
                draw_tx_filter = value;
                DrawBackground();
            }
        }

        private static bool draw_tx_cw_freq = false;
        public static bool DrawTXCWFreq
        {
            get { return draw_tx_cw_freq; }
            set
            {
                draw_tx_cw_freq = value;
                DrawBackground();
            }
        }

        private static Color display_background_color = Color.Black;
        public static Color DisplayBackgroundColor
        {
            get { return display_background_color; }
            set
            {
                display_background_color = value;
                DrawBackground();
            }
        }

        private static Color waterfall_low_color = Color.Black;
        public static Color WaterfallLowColor
        {
            get { return waterfall_low_color; }
            set { waterfall_low_color = value; }
        }

        private static Color waterfall_mid_color = Color.Red;
        public static Color WaterfallMidColor
        {
            get { return waterfall_mid_color; }
            set { waterfall_mid_color = value; }
        }

        private static Color waterfall_high_color = Color.Yellow;
        public static Color WaterfallHighColor
        {
            get { return waterfall_high_color; }
            set { waterfall_high_color = value; }
        }

        private static float waterfall_high_threshold = -80.0F;
        public static float WaterfallHighThreshold
        {
            get { return waterfall_high_threshold; }
            set { waterfall_high_threshold = value; }
        }

        private static float waterfall_low_threshold = -130.0F;
        public static float WaterfallLowThreshold
        {
            get { return waterfall_low_threshold; }
            set { waterfall_low_threshold = value; }
        }

        private static float display_line_width = 1.0F;
        public static float DisplayLineWidth
        {
            get { return display_line_width; }
            set
            {
                display_line_width = value;
                data_line_pen = new Pen(new SolidBrush(data_line_color), display_line_width);
            }
        }

        private static DisplayLabelAlignment display_label_align = DisplayLabelAlignment.LEFT;
        public static DisplayLabelAlignment DisplayLabelAlign
        {
            get { return display_label_align; }
            set
            {
                display_label_align = value;
                DrawBackground();
            }
        }

        #endregion

        #region General Routines

        public static void Init()
        {

            waterfall_bmp = new Bitmap(W, H, PixelFormat.Format24bppRgb);	// initialize waterfall display

            average_buffer = new float[BUFFER_SIZE];	// initialize averaging buffer array
            average_buffer[0] = CLEAR_FLAG;		// set the clear flag

            peak_buffer = new float[BUFFER_SIZE];
            peak_buffer[0] = CLEAR_FLAG;

            //background_image_mutex = new Mutex(false);

            new_display_data = new float[BUFFER_SIZE];
            current_display_data = new float[BUFFER_SIZE];
            waterfall_display_data = new float[BUFFER_SIZE];
            for (int i = 0; i < BUFFER_SIZE; i++)
            {
                new_display_data[i] = -200.0f;
                current_display_data[i] = -200.0f;
                waterfall_display_data[i] = -200.0f;
            }
        }

        public static void DrawBackground()
        {
            // draws the background image for the display based
            // on the current selected display mode.

            if (current_display_engine == DisplayEngine.GDI_PLUS)
            {
                target.Invalidate();
            }
        }

        #endregion

        #region GDI+

        unsafe public static void RenderGDIPlusWaterfall(ref PaintEventArgs e)
        {
            bool update = true;

            update = DrawWaterfall(e.Graphics, W, H);
            counter++;
        }

        unsafe public static void RenderGDIPlusPanadapter(ref PaintEventArgs e)
        {
            bool update = true;
            counter = 0;
            update = DrawPanadapter(e.Graphics, W, H);
        }

        private static void UpdateDisplayPeak()
        {
            if (peak_buffer[0] == CLEAR_FLAG)
            {
                for (int i = 0; i < BUFFER_SIZE; i++)
                    peak_buffer[i] = current_display_data[i];
            }
            else
            {
                for (int i = 0; i < BUFFER_SIZE; i++)
                {
                    if (current_display_data[i] > peak_buffer[i])
                        peak_buffer[i] = current_display_data[i];
                    current_display_data[i] = peak_buffer[i];
                }
            }
        }

        #region Drawing Routines
        // ======================================================
        // Drawing Routines
        // ======================================================

        public static int center_line_x = 415;
        public static int filter_left_x = 150;
        public static int filter_right_x = 2550;

        private static void DrawPanadapterGrid(ref Graphics g, int W, int H)
        {
            // draw background
            g.FillRectangle(new SolidBrush(display_background_color), 0, 0, W, H);

            int low = rx_display_low;					// initialize variables
            int high = rx_display_high;
            int mid_w = W / 2;
            int[] step_list = { 10, 20, 25, 50 };
            int step_power = 1;
            int step_index = 0;
            int freq_step_size = 50;

            System.Drawing.Font font = new System.Drawing.Font("Arial", 9);
            SolidBrush grid_text_brush = new SolidBrush(grid_text_color);
            Pen grid_pen = new Pen(grid_color);
            Pen tx_filter_pen = new Pen(display_filter_tx_color);
            int y_range = spectrum_grid_max - spectrum_grid_min;
            int filter_low, filter_high;

            //            double zoom = Console.zoom_disp;

            center_line_x = W / 2;

            if (mox) // get filter limits
            {
                filter_low = DttSP.TXFilterLowCut;
                filter_high = DttSP.TXFilterHighCut;
            }
            else
            {
                filter_low = DttSP.RXFilterLowCut;
                filter_high = DttSP.RXFilterHighCut;
            }

            if (current_dsp_mode == DSPMode.DRM)
            {
                filter_low = -5000;
                filter_high = 5000;
            }

            // Calculate horizontal step size
            int width = high - low;
            while (width / freq_step_size > 10)
            {
                freq_step_size = step_list[step_index] * (int)Math.Pow(10.0, step_power);
                step_index = (step_index + 1) % 4;
                if (step_index == 0) step_power++;
            }

            int w_steps = width / freq_step_size;

            // calculate vertical step size
            int h_steps = (spectrum_grid_max - spectrum_grid_min) / spectrum_grid_step;
            double h_pixel_step = (double)H / h_steps;
            int top = (int)((double)spectrum_grid_step * H / y_range);

            if (!mox && sub_rx_enabled)
            {
                // draw Sub RX filter
                // get filter screen coordinates
                int filter_left_x = (int)((float)(filter_low - low + vfob_hz - losc_hz) / (high - low) * W);
                int filter_right_x = (int)((float)(filter_high - low + vfob_hz - losc_hz) / (high - low) * W);

                // make the filter display at least one pixel wide.
                if (filter_left_x == filter_right_x) filter_right_x = filter_left_x + 1;

                // draw rx filter
                g.FillRectangle(new SolidBrush(sub_rx_filter_color),	// draw filter overlay
                    filter_left_x, top, filter_right_x - filter_left_x, H - top);

                // draw Sub RX 0Hz line
                int x = (int)((float)(vfob_hz - losc_hz - low) / (high - low) * W);
                g.DrawLine(new Pen(grid_zero_color), x, top, x, H);
                g.DrawLine(new Pen(grid_zero_color), x - 1, top, x - 1, H);
            }

            if (mox)
            {
                // get filter screen coordinates
                int filter_left_x = (int)((float)(filter_low - low) / (high - low) * W);
                int filter_right_x = (int)((float)(filter_high - low) / (high - low) * W);

                // make the filter display at least one pixel wide.
                if (filter_left_x == filter_right_x) filter_right_x = filter_left_x + 1;

                // draw rx filter
                g.FillRectangle(new SolidBrush(display_filter_color),	// draw filter overlay
                    filter_left_x, top, filter_right_x - filter_left_x, H - top);

                // draw Main RX 0Hz line
                int x = (int)((float)(-low) / (high - low) * W);
                g.DrawLine(new Pen(sub_rx_zero_line_color), x, top, x, H);
                g.DrawLine(new Pen(sub_rx_zero_line_color), x - 1, top, x - 1, H);
            }

            else
            {
                if (!(mox && (current_dsp_mode == DSPMode.CWL || current_dsp_mode == DSPMode.CWU)))
                {
                    // get filter screen coordinates
                    int filter_left_x = (int)((float)(filter_low - low + vfoa_hz - losc_hz) / (high - low) * W);
                    int filter_right_x = (int)((float)(filter_high - low + vfoa_hz - losc_hz) / (high - low) * W);

                    // make the filter display at least one pixel wide.
                    if (filter_left_x == filter_right_x) filter_right_x = filter_left_x + 1;

                    // draw rx filter
                    g.FillRectangle(new SolidBrush(display_filter_color),	// draw filter overlay
                        filter_left_x, top, filter_right_x - filter_left_x, H - top);

                    // draw Main RX 0Hz line
                    int x = (int)((float)(vfoa_hz - losc_hz - low) / (high - low) * W);
                    g.DrawLine(new Pen(sub_rx_zero_line_color), x, top, x, H);
                    g.DrawLine(new Pen(sub_rx_zero_line_color), x - 1, top, x - 1, H);
                }
            }

            if (!mox && draw_tx_filter &&
                (current_dsp_mode != DSPMode.CWL && current_dsp_mode != DSPMode.CWU))
            {
                // get tx filter limits
                int filter_left_x;
                int filter_right_x;

                if (!split_enabled)
                {
                    filter_left_x = (int)((float)(DttSP.TXFilterLowCut - low + xit_hz) / (high - low) * W);
                    filter_right_x = (int)((float)(DttSP.TXFilterHighCut - low + xit_hz) / (high - low) * W);
                }
                else
                {
                    filter_left_x = (int)((float)(DttSP.TXFilterLowCut - low + xit_hz + (losc_hz - vfoa_hz)) / (high - low) * W);
                    filter_right_x = (int)((float)(DttSP.TXFilterHighCut - low + xit_hz + (losc_hz - vfoa_hz)) / (high - low) * W);
                }

                g.DrawLine(tx_filter_pen, filter_left_x, top, filter_left_x, H);		// draw tx filter overlay
                g.DrawLine(tx_filter_pen, filter_left_x + 1, top, filter_left_x + 1, H);	// draw tx filter overlay
                g.DrawLine(tx_filter_pen, filter_right_x, top, filter_right_x, H);	// draw tx filter overlay
                g.DrawLine(tx_filter_pen, filter_right_x + 1, top, filter_right_x + 1, H);// draw tx filter overlay
            }

            if (!mox && draw_tx_cw_freq &&
                (current_dsp_mode == DSPMode.CWL || current_dsp_mode == DSPMode.CWU))
            {
                int pitch = cw_pitch;
                if (current_dsp_mode == DSPMode.CWL)
                    pitch = -cw_pitch;

                int cw_line_x;
                if (!split_enabled)
                    cw_line_x = (int)((float)(pitch - low + xit_hz) / (high - low) * W);
                else
                    cw_line_x = (int)((float)(pitch - low + xit_hz + (vfob_hz - vfoa_hz)) / (high - low) * W);

                g.DrawLine(tx_filter_pen, cw_line_x, top, cw_line_x, H);
                g.DrawLine(tx_filter_pen, cw_line_x + 1, top, cw_line_x + 1, H);
            }

            double vfo;

            if (mox)
            {
                if (split_enabled)
                    vfo = vfob_hz;
                else

                    vfo = vfoa_hz;
                vfo += xit_hz;
            }
            else
            {
                vfo = losc_hz + rit_hz;
            }

            switch (current_dsp_mode)
            {
                case DSPMode.CWL:
                    vfo += cw_pitch;
                    break;
                case DSPMode.CWU:
                    vfo -= cw_pitch;
                    break;
                default:
                    break;
            }

            long vfo_round = ((long)(vfo / freq_step_size)) * freq_step_size;
            long vfo_delta = (long)(vfo - vfo_round);

            // Draw vertical lines
            for (int i = 0; i <= h_steps + 1; i++)
            {
                string label;
                int offsetL;
                int offsetR;

                int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W);

                if (!show_freq_offset)
                {
                    if (actual_fgrid == 1.8 || actual_fgrid == 2.0 ||
                        actual_fgrid == 3.5 || actual_fgrid == 4.0 ||
                        actual_fgrid == 7.0 || actual_fgrid == 7.3 ||
                        actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                        actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                        actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                        actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                        actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                        actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                        actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                        actual_fgrid == 50.0 || actual_fgrid == 54.0 ||
                        actual_fgrid == 144.0 || actual_fgrid == 148.0)
                    {

                        g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                        label = actual_fgrid.ToString("f3");
                        if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                        else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                        else offsetL = (int)((label.Length + 1) * 4.1) - 8;

                        g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));
                    }
                    else
                    {

                        if (freq_step_size >= 2000)
                        {
                            double t100;
                            double t1000;
                            t100 = (actual_fgrid * 100);
                            t1000 = (actual_fgrid * 1000);

                            int it100 = (int)t100;
                            int it1000 = (int)t1000;

                            int it100x10 = it100 * 10;

                            if (it100x10 == it1000)
                            {
                            }
                            else
                            {
                                grid_pen.DashStyle = DashStyle.Dot;
                            }
                        }
                        else
                        {
                            if (freq_step_size == 1000)
                            {
                                double t200;
                                double t2000;
                                t200 = (actual_fgrid * 200);
                                t2000 = (actual_fgrid * 2000);

                                int it200 = (int)t200;
                                int it2000 = (int)t2000;

                                int it200x10 = it200 * 10;

                                if (it200x10 == it2000)
                                {
                                }
                                else
                                {
                                    grid_pen.DashStyle = DashStyle.Dot;
                                }
                            }
                            else
                            {
                                double t1000;
                                double t10000;
                                t1000 = (actual_fgrid * 1000);
                                t10000 = (actual_fgrid * 10000);

                                int it1000 = (int)t1000;
                                int it10000 = (int)t10000;

                                int it1000x10 = it1000 * 10;

                                if (it1000x10 == it10000)
                                {
                                }
                                else
                                {
                                    grid_pen.DashStyle = DashStyle.Dot;
                                }
                            }
                        }
                        //						g.DrawLine(grid_pen, vgrid, top, vgrid, H);			//wa6ahl
                        grid_pen.DashStyle = DashStyle.Solid;

                        if (((double)((int)(actual_fgrid * 1000))) == actual_fgrid * 1000)
                        {
                            label = actual_fgrid.ToString("f3"); //wa6ahl

                            if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                            else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                            else offsetL = (int)((label.Length + 1) * 4.1) - 8;
                        }
                        else
                        {
                            string temp_string;
                            int jper;
                            label = actual_fgrid.ToString("f4");
                            temp_string = label;
                            jper = label.IndexOf('.') + 4;
                            label = label.Insert(jper, " ");

                            if (actual_fgrid < 10) offsetL = (int)((label.Length) * 4.1) - 14;
                            else if (actual_fgrid < 100.0) offsetL = (int)((label.Length) * 4.1) - 11;
                            else offsetL = (int)((label.Length) * 4.1) - 8;
                        }

                        g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                    }
                }
                else
                {
                    vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W);	//wa6ahl
                    //					g.DrawLine(grid_pen, vgrid, top, vgrid, H);			//wa6ahl

                    double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                    label = fgrid.ToString();
                    offsetL = (int)((label.Length + 1) * 4.1);
                    offsetR = (int)(label.Length * 4.1);
                    if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                    {
                        //						g.DrawString(label, font, grid_text_brush, vgrid-offsetL, (float)Math.Floor(H*.01));
                    }
                }
            }

            int[] band_edge_list = { 18068000, 18168000, 1800000, 2000000, 3500000, 4000000,
				7000000, 7300000, 10100000, 10150000, 14000000, 14350000, 21000000, 21450000,
				24890000, 24990000, 28000000, 29700000, 50000000, 54000000, 144000000, 148000000 };

            for (int i = 0; i < band_edge_list.Length; i++)
            {
                double band_edge_offset = band_edge_list[i] - losc_hz;
                if (band_edge_offset >= low && band_edge_offset <= high)
                {
                    int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                    g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
                }
                if (i == 1 && !show_freq_offset) break;
            }

            // Draw horizontal lines
            for (int i = 1; i < h_steps; i++)
            {
                int xOffset = 0;
                int num = spectrum_grid_max - i * spectrum_grid_step;
                int y = (int)((double)(spectrum_grid_max - num) * H / y_range);
                g.DrawLine(grid_pen, 0, y, W, y);

                // Draw horizontal line labels
                if (i != 1) // avoid intersecting vertical and horizontal labels
                {
                    num = spectrum_grid_max - i * spectrum_grid_step;
                    string label = num.ToString();
                    if (label.Length == 3) xOffset = 7;
                    //int offset = (int)(label.Length*4.1);
                    if (display_label_align != DisplayLabelAlignment.LEFT &&
                        display_label_align != DisplayLabelAlignment.AUTO &&
                        (current_dsp_mode == DSPMode.USB ||
                        current_dsp_mode == DSPMode.CWU))
                        xOffset -= 32;
                    SizeF size = g.MeasureString(label, font);

                    int x = 0;
                    switch (display_label_align)
                    {
                        case DisplayLabelAlignment.LEFT:
                            x = xOffset + 3;
                            break;
                        case DisplayLabelAlignment.CENTER:
                            x = center_line_x + xOffset;
                            break;
                        case DisplayLabelAlignment.RIGHT:
                            x = (int)(W - size.Width);
                            break;
                        case DisplayLabelAlignment.AUTO:
                            x = xOffset + 3;
                            break;
                        case DisplayLabelAlignment.OFF:
                            x = W;
                            break;
                    }

                    y -= 8;
                    if (y + 9 < H)
                        g.DrawString(label, font, grid_text_brush, x, y);
                }
            }

            if (high_swr)
                g.DrawString("High SWR", new System.Drawing.Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Red), 245, 20);
        }

        private static void DrawOffBackground(ref Bitmap b, int W, int H)
        {
            Bitmap bmp = new Bitmap(W, H);				// create bitmap
            Graphics g = Graphics.FromImage(bmp);		// create graphics object for drawing

            // draw background
            g.FillRectangle(new SolidBrush(display_background_color), 0, 0, W, H);

            if (high_swr)
                g.DrawString("High SWR", new System.Drawing.Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Red), 245, 20);

            // save bitmap
            //background_image_mutex.WaitOne();
            if (b != null) b.Dispose();
            b = (Bitmap)bmp.Clone();
            //background_image_mutex.ReleaseMutex();
            bmp.Dispose();
            g.Dispose();
        }

        private static Point[] points;

        unsafe static private bool DrawPanadapter(Graphics g, int W, int H)
        {
            DrawPanadapterGrid(ref g, W, H);
            if (points == null || points.Length < W)
                points = new Point[W];			    // array of points to display
            float slope = 0.0F;						// samples to process per pixel
            int num_samples = 0;					// number of samples to process
            int start_sample_index = 0;				// index to begin looking at samples
            int Low = rx_display_low;
            int High = rx_display_high;
            int y_range = spectrum_grid_max - spectrum_grid_min;
            int yRange = spectrum_grid_max - spectrum_grid_min;
            int top = (int)((double)spectrum_grid_step * H / y_range);

            if (Display.console.power)
            {
                if (current_dsp_mode == DSPMode.DRM)
                {
                    Low += 12000;
                    High += 12000;
                }

                max_y = Int32.MinValue;

                if (data_ready)
                {
                    if (mox && (current_dsp_mode == DSPMode.CWL || current_dsp_mode == DSPMode.CWU))
                    {
                        for (int i = 0; i < current_display_data.Length; i++)
                            current_display_data[i] = -200.0f;
                    }
                    else
                    {
                        // get new data
                        fixed (void* rptr = &new_display_data[0])
                        fixed (void* wptr = &current_display_data[0])
                            Win32.memcpy(wptr, rptr, BUFFER_SIZE * sizeof(float));

                        // kb9yig sr mod starts 
                        console.AdjustDisplayDataForBandEdge(ref current_display_data);
                        // end kb9yig sr mods 
                    }
                    data_ready = false;
                }

                if (average_on)
                    console.UpdateDisplayAverage();
                if (peak_on)
                    UpdateDisplayPeak();

                num_samples = (High - Low);

                start_sample_index = (BUFFER_SIZE >> 1) + (int)((Low * BUFFER_SIZE) / DttSP.SampleRate);
                num_samples = (int)((High - Low) * BUFFER_SIZE / DttSP.SampleRate);
                if (start_sample_index < 0) start_sample_index += 4096;
                if ((num_samples - start_sample_index) > (BUFFER_SIZE + 1))
                    num_samples = BUFFER_SIZE - start_sample_index;

                //Debug.WriteLine("start_sample_index: "+start_sample_index);
                slope = (float)num_samples / (float)W;
                for (int i = 0; i < W; i++)
                {
                    float max = float.MinValue;
                    float dval = i * slope + start_sample_index;
                    int lindex = (int)Math.Floor(dval);
                    int rindex = (int)Math.Floor(dval + slope);

                    if (slope <= 1.0 || lindex == rindex)
                    {
                        max = current_display_data[lindex % 4096] * ((float)lindex - dval + 1) + current_display_data[(lindex + 1) % 4096] * (dval - (float)lindex);
                    }
                    else
                    {
                        for (int j = lindex; j < rindex; j++)
                            if (current_display_data[j % 4096] > max) max = current_display_data[j % 4096];
                    }

                    max += display_cal_offset;
                    if (!mox) max += preamp_offset;

                    if (max > max_y)
                    {
                        max_y = max;
                        max_x = i;
                    }

                    points[i].X = i;
                    points[i].Y = (int)(Math.Floor((spectrum_grid_max - max) * H / yRange));
                }

                g.DrawLines(data_line_pen, points);

                points = null;

                // draw long cursor
                if (current_click_tune_mode != ClickTuneMode.Off)
                {
                    Pen p;
                    if (current_click_tune_mode == ClickTuneMode.VFOA)
                        p = new Pen(grid_text_color);
                    else p = new Pen(Color.Red);
                    g.DrawLine(p, display_cursor_x, top, display_cursor_x, H);
                    g.DrawLine(p, display_cursor_x + 1, top, display_cursor_x + 1, H);
                }
            }

            return true;
        }

        private static HiPerfTimer timer_waterfall = new HiPerfTimer();
        private static float[] waterfall_data;
        unsafe static private bool DrawWaterfall(Graphics g, int W, int H)
        {
            DrawWaterfallGrid(ref g, W, H);
            if (waterfall_data == null || waterfall_data.Length < W)
                waterfall_data = new float[W];		// array of points to display
            float slope = 0.0F;						// samples to process per pixel
            int num_samples = 0;					// number of samples to process
            int start_sample_index = 0;				// index to begin looking at samples
            int Low = rx_display_low;
            int High = rx_display_high;
            int yRange = spectrum_grid_max - spectrum_grid_min;

            if (current_dsp_mode == DSPMode.DRM)
            {
                Low += 12000;
                High += 12000;
            }

            max_y = Int32.MinValue;

            if (data_ready)
            {
                if (mox && (current_dsp_mode == DSPMode.CWL || current_dsp_mode == DSPMode.CWU))
                {
                    for (int i = 0; i < current_display_data.Length; i++)
                        current_display_data[i] = -200.0f;
                }
                else
                {
                    // get new data
                    fixed (void* rptr = &new_display_data[0])
                    fixed (void* wptr = &current_display_data[0])
                        Win32.memcpy(wptr, rptr, BUFFER_SIZE * sizeof(float));

                    // kb9yig sr mod starts 
                    console.AdjustDisplayDataForBandEdge(ref current_display_data);
                    // end kb9yig sr mods 
                }

                data_ready = false;
            }

            console.UpdateDisplayAverage();
            if (peak_on)
                UpdateDisplayPeak();


            timer_waterfall.Stop();
            if (timer_waterfall.DurationMsec > waterfall_update_period)
            {
                timer_waterfall.Start();
                num_samples = (High - Low);

                start_sample_index = (BUFFER_SIZE >> 1) + (int)((Low * BUFFER_SIZE) / DttSP.SampleRate);
                num_samples = (int)((High - Low) * BUFFER_SIZE / DttSP.SampleRate);
                if (start_sample_index < 0) start_sample_index += 4096;
                if ((num_samples - start_sample_index) > (BUFFER_SIZE + 1))
                    num_samples = BUFFER_SIZE - start_sample_index;

                slope = (float)num_samples / (float)W;
                for (int i = 0; i < W; i++)
                {
                    float max = float.MinValue;
                    float dval = i * slope + start_sample_index;
                    int lindex = (int)Math.Floor(dval);
                    int rindex = (int)Math.Floor(dval + slope);

                    if (slope <= 1.0 || lindex == rindex)
                    {
                        max = current_display_data[lindex % 4096] * ((float)lindex - dval + 1) + current_display_data[(lindex + 1) % 4096] * (dval - (float)lindex);
                    }
                    else
                    {
                        for (int j = lindex; j < rindex; j++)
                            if (current_display_data[j % 4096] > max) max = current_display_data[j % 4096];
                    }

                    max += display_cal_offset;
                    if (!mox) max += preamp_offset;

                    if (max > max_y)
                    {
                        max_y = max;
                        max_x = i;
                    }

                    waterfall_data[i] = max;
                }

                BitmapData bitmapData = waterfall_bmp.LockBits(
                    new Rectangle(0, 0, waterfall_bmp.Width, waterfall_bmp.Height),
                    ImageLockMode.ReadWrite,
                    waterfall_bmp.PixelFormat);

                int pixel_size = 3;
                byte* row = null;

                // first scroll image
                int total_size = bitmapData.Stride * bitmapData.Height;		// find buffer size
                Win32.memcpy(new IntPtr((int)bitmapData.Scan0 + bitmapData.Stride).ToPointer(),
                    bitmapData.Scan0.ToPointer(),
                    total_size - bitmapData.Stride);

                row = (byte*)bitmapData.Scan0;

                // draw new data
                for (int i = 0; i < W; i++)	// for each pixel in the new line
                {
                    int R, G, B;		// variables to save Red, Green and Blue component values

                    /*if(waterfall_data[i] <= waterfall_low_threshold)		// if less than low threshold, just use low color
                    {
                        R = waterfall_low_color.R;
                        G = waterfall_low_color.G;
                        B = waterfall_low_color.B;
                    }
                    else if(waterfall_data[i] >= waterfall_high_threshold)// if more than high threshold, just use high color
                    {
                        R = waterfall_high_color.R;
                        G = waterfall_high_color.G;
                        B = waterfall_high_color.B;
                    }
                    else // use a color between high and low
                    {
                        float percent = (waterfall_data[i] - waterfall_low_threshold)/(waterfall_high_threshold - waterfall_low_threshold);
                        if(percent <= 0.5)	// use a gradient between low and mid colors
                        {
                            percent *= 2;

                            R = (int)((1-percent)*waterfall_low_color.R + percent*waterfall_mid_color.R);
                            G = (int)((1-percent)*waterfall_low_color.G + percent*waterfall_mid_color.G);
                            B = (int)((1-percent)*waterfall_low_color.B + percent*waterfall_mid_color.B);
                        }
                        else				// use a gradient between mid and high colors
                        {
                            percent = (float)(percent-0.5)*2;

                            R = (int)((1-percent)*waterfall_mid_color.R + percent*waterfall_high_color.R);
                            G = (int)((1-percent)*waterfall_mid_color.G + percent*waterfall_high_color.G);
                            B = (int)((1-percent)*waterfall_mid_color.B + percent*waterfall_high_color.B);
                        }
                    }*/

                    //waterfall_data[i] = (float)i/W*(waterfall_high_threshold - waterfall_low_threshold) + waterfall_low_threshold;

                    if (waterfall_data[i] <= waterfall_low_threshold)
                    {
                        R = waterfall_low_color.R;
                        G = waterfall_low_color.G;
                        B = waterfall_low_color.B;
                    }
                    else if (waterfall_data[i] >= waterfall_high_threshold)
                    {
                        R = 192;
                        G = 124;
                        B = 255;
                    }
                    else // value is between low and high
                    {
                        float range = waterfall_high_threshold - waterfall_low_threshold;
                        float offset = waterfall_data[i] - waterfall_low_threshold;
                        float overall_percent = offset / range; // value from 0.0 to 1.0 where 1.0 is high and 0.0 is low.

                        if (overall_percent < (float)2 / 9) // background to blue
                        {
                            float local_percent = overall_percent / ((float)2 / 9);
                            R = (int)((1.0 - local_percent) * waterfall_low_color.R);
                            G = (int)((1.0 - local_percent) * waterfall_low_color.G);
                            B = (int)(waterfall_low_color.B + local_percent * (255 - waterfall_low_color.B));
                        }
                        else if (overall_percent < (float)3 / 9) // blue to blue-green
                        {
                            float local_percent = (overall_percent - (float)2 / 9) / ((float)1 / 9);
                            R = 0;
                            G = (int)(local_percent * 255);
                            B = 255;
                        }
                        else if (overall_percent < (float)4 / 9) // blue-green to green
                        {
                            float local_percent = (overall_percent - (float)3 / 9) / ((float)1 / 9);
                            R = 0;
                            G = 255;
                            B = (int)((1.0 - local_percent) * 255);
                        }
                        else if (overall_percent < (float)5 / 9) // green to red-green
                        {
                            float local_percent = (overall_percent - (float)4 / 9) / ((float)1 / 9);
                            R = (int)(local_percent * 255);
                            G = 255;
                            B = 0;
                        }
                        else if (overall_percent < (float)7 / 9) // red-green to red
                        {
                            float local_percent = (overall_percent - (float)5 / 9) / ((float)2 / 9);
                            R = 255;
                            G = (int)((1.0 - local_percent) * 255);
                            B = 0;
                        }
                        else if (overall_percent < (float)8 / 9) // red to red-blue
                        {
                            float local_percent = (overall_percent - (float)7 / 9) / ((float)1 / 9);
                            R = 255;
                            G = 0;
                            B = (int)(local_percent * 255);
                        }
                        else // red-blue to purple end
                        {
                            float local_percent = (overall_percent - (float)8 / 9) / ((float)1 / 9);
                            R = (int)((0.75 + 0.25 * (1.0 - local_percent)) * 255);
                            G = (int)(local_percent * 255 * 0.5);
                            B = 255;
                        }
                    }

                    // set pixel color
                    row[i * pixel_size + 0] = (byte)B;	// set color in memory
                    row[i * pixel_size + 1] = (byte)G;
                    row[i * pixel_size + 2] = (byte)R;
                }
                waterfall_bmp.UnlockBits(bitmapData);
            }

            g.DrawImageUnscaled(waterfall_bmp, 0, 16);	// draw the image on the background	

            waterfall_counter++;

            // draw long cursor
            if (current_click_tune_mode != ClickTuneMode.Off)
            {
                Pen p;
                if (current_click_tune_mode == ClickTuneMode.VFOA)
                    p = new Pen(grid_text_color);
                else p = new Pen(Color.Red);
                g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H);
                g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
            }

            return true;
        }

        private static void DrawWaterfallGrid(ref Graphics g, int W, int H)
        {
            // draw background
            g.FillRectangle(new SolidBrush(display_background_color), 0, 0, W, H);

            int low = rx_display_low;					// initialize variables
            int high = rx_display_high;
            int mid_w = W / 2;
            int[] step_list = { 10, 20, 25, 50 };
            int step_power = 1;
            int step_index = 0;
            int freq_step_size = 50;

            System.Drawing.Font font = new System.Drawing.Font("Arial", 9);
            SolidBrush grid_text_brush = new SolidBrush(grid_text_color);
            Pen grid_pen = new Pen(grid_color);
            Pen tx_filter_pen = new Pen(display_filter_tx_color);
            int y_range = spectrum_grid_max - spectrum_grid_min;
            int filter_low, filter_high;

            int center_line_x = (int)(-(double)low / (high - low) * W);

            if (mox) // get filter limits
            {
                filter_low = DttSP.TXFilterLowCut;
                filter_high = DttSP.TXFilterHighCut;
            }
            else
            {
                filter_low = DttSP.RXFilterLowCut;
                filter_high = DttSP.RXFilterHighCut;
            }

            if (current_dsp_mode == DSPMode.DRM)
            {
                filter_low = -5000;
                filter_high = 5000;
            }

            // Calculate horizontal step size
            int width = high - low;
            while (width / freq_step_size > 10)
            {
                freq_step_size = step_list[step_index] * (int)Math.Pow(10.0, step_power);
                step_index = (step_index + 1) % 4;
                if (step_index == 0) step_power++;
            }
            double w_pixel_step = (double)W * freq_step_size / width;
            int w_steps = width / freq_step_size;

            // calculate vertical step size
            int h_steps = (spectrum_grid_max - spectrum_grid_min) / spectrum_grid_step;
            double h_pixel_step = (double)H / h_steps;
            int top = (int)((double)spectrum_grid_step * H / y_range);

            if (!mox && sub_rx_enabled)
            {
                // draw Sub RX 0Hz line
                int x = (int)((float)(vfob_hz - vfoa_hz - low) / (high - low) * W);
                g.DrawLine(new Pen(sub_rx_zero_line_color), x, 0, x, top);
                g.DrawLine(new Pen(sub_rx_zero_line_color), x - 1, 0, x - 1, top);

                // draw Sub RX filter
                // get filter screen coordinates
                int filter_left_x = (int)((float)(filter_low - low + vfob_hz - vfoa_hz) / (high - low) * W);
                int filter_right_x = (int)((float)(filter_high - low + vfob_hz - vfoa_hz) / (high - low) * W);

                // make the filter display at least one pixel wide.
                if (filter_left_x == filter_right_x) filter_right_x = filter_left_x + 1;

                // draw rx filter
                g.FillRectangle(new SolidBrush(sub_rx_filter_color),	// draw filter overlay
                    filter_left_x, 0, filter_right_x - filter_left_x, top);
            }

            if (!(mox && (current_dsp_mode == DSPMode.CWL || current_dsp_mode == DSPMode.CWU)))
            {
                // get filter screen coordinates
                int filter_left_x = (int)((float)(filter_low - low) / (high - low) * W);
                int filter_right_x = (int)((float)(filter_high - low) / (high - low) * W);

                // make the filter display at least one pixel wide.
                if (filter_left_x == filter_right_x) filter_right_x = filter_left_x + 1;

                // draw rx filter
                g.FillRectangle(new SolidBrush(display_filter_color),	// draw filter overlay
                    filter_left_x, 0, filter_right_x - filter_left_x, top);
            }

            if (!mox && draw_tx_filter &&
                (current_dsp_mode != DSPMode.CWL && current_dsp_mode != DSPMode.CWU))
            {
                // get tx filter limits
                int filter_left_x;
                int filter_right_x;

                if (!split_enabled)
                {
                    filter_left_x = (int)((float)(DttSP.TXFilterLowCut - low + xit_hz) / (high - low) * W);
                    filter_right_x = (int)((float)(DttSP.TXFilterHighCut - low + xit_hz) / (high - low) * W);
                }
                else
                {
                    filter_left_x = (int)((float)(DttSP.TXFilterLowCut - low + xit_hz + (vfob_hz - vfoa_hz)) / (high - low) * W);
                    filter_right_x = (int)((float)(DttSP.TXFilterHighCut - low + xit_hz + (vfob_hz - vfoa_hz)) / (high - low) * W);
                }

                g.DrawLine(tx_filter_pen, filter_left_x, 0, filter_left_x, top);		// draw tx filter overlay
                g.DrawLine(tx_filter_pen, filter_left_x + 1, 0, filter_left_x + 1, top);	// draw tx filter overlay
                g.DrawLine(tx_filter_pen, filter_right_x, 0, filter_right_x, top);		// draw tx filter overlay
                g.DrawLine(tx_filter_pen, filter_right_x + 1, 0, filter_right_x + 1, top);	// draw tx filter overlay
            }

            if (!mox && draw_tx_cw_freq &&
                (current_dsp_mode == DSPMode.CWL || current_dsp_mode == DSPMode.CWU))
            {
                int pitch = cw_pitch;
                if (current_dsp_mode == DSPMode.CWL)
                    pitch = -cw_pitch;

                int cw_line_x;
                if (!split_enabled)
                    cw_line_x = (int)((float)(pitch - low + xit_hz) / (high - low) * W);
                else
                    cw_line_x = (int)((float)(pitch - low + xit_hz + (vfob_hz - vfoa_hz)) / (high - low) * W);

                g.DrawLine(tx_filter_pen, cw_line_x, top, cw_line_x, H);
                g.DrawLine(tx_filter_pen, cw_line_x + 1, top, cw_line_x + 1, H);
            }

            double vfo;

            if (mox)
            {
                if (split_enabled)
                    vfo = vfob_hz;
                else
                    vfo = vfoa_hz;
                vfo += xit_hz;
            }
            else
            {
                vfo = vfoa_hz + rit_hz;
            }

            long vfo_round = ((long)(vfo / freq_step_size)) * freq_step_size;
            long vfo_delta = (long)(vfo - vfo_round);

            // Draw vertical lines
            for (int i = 0; i <= h_steps + 1; i++)
            {
                string label;
                int offsetL;
                int offsetR;

                int fgrid = i * freq_step_size + (low / freq_step_size) * freq_step_size;
                double actual_fgrid = ((double)(vfo_round + fgrid)) / 1000000;
                int vgrid = (int)((double)(fgrid - vfo_delta - low) / (high - low) * W);

                if (!show_freq_offset)
                {
                    if (actual_fgrid == 1.8 || actual_fgrid == 2.0 ||
                        actual_fgrid == 3.5 || actual_fgrid == 4.0 ||
                        actual_fgrid == 7.0 || actual_fgrid == 7.3 ||
                        actual_fgrid == 10.1 || actual_fgrid == 10.15 ||
                        actual_fgrid == 14.0 || actual_fgrid == 14.35 ||
                        actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
                        actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                        actual_fgrid == 24.89 || actual_fgrid == 24.99 ||
                        actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
                        actual_fgrid == 28.0 || actual_fgrid == 29.7 ||
                        actual_fgrid == 50.0 || actual_fgrid == 54.0 ||
                        actual_fgrid == 144.0 || actual_fgrid == 148.0)
                    {
                        g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

                        label = actual_fgrid.ToString("f3");
                        if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                        else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                        else offsetL = (int)((label.Length + 1) * 4.1) - 8;

                        g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid - offsetL, (float)Math.Floor(H * .01));
                    }
                    else
                    {

                        if (freq_step_size >= 2000)
                        {
                            double t100;
                            double t1000;
                            t100 = (actual_fgrid * 100);
                            t1000 = (actual_fgrid * 1000);

                            int it100 = (int)t100;
                            int it1000 = (int)t1000;

                            int it100x10 = it100 * 10;

                            if (it100x10 == it1000)
                            {
                            }
                            else
                            {
                                grid_pen.DashStyle = DashStyle.Dot;
                            }
                        }
                        else
                        {
                            if (freq_step_size == 1000)
                            {
                                double t200;
                                double t2000;
                                t200 = (actual_fgrid * 200);
                                t2000 = (actual_fgrid * 2000);

                                int it200 = (int)t200;
                                int it2000 = (int)t2000;

                                int it200x10 = it200 * 10;

                                if (it200x10 == it2000)
                                {
                                }
                                else
                                {
                                    grid_pen.DashStyle = DashStyle.Dot;
                                }
                            }
                            else
                            {
                                double t1000;
                                double t10000;
                                t1000 = (actual_fgrid * 1000);
                                t10000 = (actual_fgrid * 10000);

                                int it1000 = (int)t1000;
                                int it10000 = (int)t10000;

                                int it1000x10 = it1000 * 10;

                                if (it1000x10 == it10000)
                                {
                                }
                                else
                                {
                                    grid_pen.DashStyle = DashStyle.Dot;
                                }
                            }
                        }
                        g.DrawLine(grid_pen, vgrid, top, vgrid, H);			//wa6ahl
                        grid_pen.DashStyle = DashStyle.Solid;

                        if (((double)((int)(actual_fgrid * 1000))) == actual_fgrid * 1000)
                        {
                            label = actual_fgrid.ToString("f3"); //wa6ahl

                            //if(actual_fgrid > 1300.0)
                            //	label = label.Substring(label.Length-4);

                            if (actual_fgrid < 10) offsetL = (int)((label.Length + 1) * 4.1) - 14;
                            else if (actual_fgrid < 100.0) offsetL = (int)((label.Length + 1) * 4.1) - 11;
                            else offsetL = (int)((label.Length + 1) * 4.1) - 8;
                        }
                        else
                        {
                            string temp_string;
                            int jper;
                            label = actual_fgrid.ToString("f4");
                            temp_string = label;
                            jper = label.IndexOf('.') + 4;
                            label = label.Insert(jper, " ");

                            //if(actual_fgrid > 1300.0)
                            //	label = label.Substring(label.Length-4);

                            if (actual_fgrid < 10) offsetL = (int)((label.Length) * 4.1) - 14;
                            else if (actual_fgrid < 100.0) offsetL = (int)((label.Length) * 4.1) - 11;
                            else offsetL = (int)((label.Length) * 4.1) - 8;
                        }

                        g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                    }
                }
                else
                {
                    vgrid = Convert.ToInt32((double)-(fgrid - low) / (low - high) * W);	//wa6ahl
                    g.DrawLine(grid_pen, vgrid, top, vgrid, H);			//wa6ahl

                    double new_fgrid = (vfoa_hz + fgrid) / 1000000;

                    label = fgrid.ToString();
                    offsetL = (int)((label.Length + 1) * 4.1);
                    offsetR = (int)(label.Length * 4.1);
                    if ((vgrid - offsetL >= 0) && (vgrid + offsetR < W) && (fgrid != 0))
                    {
                        g.DrawString(label, font, grid_text_brush, vgrid - offsetL, (float)Math.Floor(H * .01));
                    }
                }
            }

            int[] band_edge_list = { 18068000, 18168000, 1800000, 2000000, 3500000, 4000000,
									   7000000, 7300000, 10100000, 10150000, 14000000, 14350000, 21000000, 21450000,
									   24890000, 24990000, 28000000, 29700000, 50000000, 54000000, 144000000, 148000000 };

            for (int i = 0; i < band_edge_list.Length; i++)
            {
                double band_edge_offset = band_edge_list[i] - vfo;
                if (band_edge_offset >= low && band_edge_offset <= high)
                {
                    int temp_vline = (int)((double)(band_edge_offset - low) / (high - low) * W);//wa6ahl
                    g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);//wa6ahl
                }
                if (i == 1 && !show_freq_offset) break;
            }

            /*// Draw horizontal lines
            for(int i=1; i<h_steps; i++)
            {
                int xOffset = 0;
                int num = spectrum_grid_max - i*spectrum_grid_step;
                int y = (int)((double)(spectrum_grid_max - num)*H/y_range);
                g.DrawLine(grid_pen, 0, y, W, y);

                // Draw horizontal line labels
                if(i != 1) // avoid intersecting vertical and horizontal labels
                {
                    num = spectrum_grid_max - i*spectrum_grid_step;
                    string label = num.ToString();
                    if(label.Length == 3) xOffset = 7;
                    //int offset = (int)(label.Length*4.1);
                    if(display_label_align != DisplayLabelAlignment.LEFT &&
                        display_label_align != DisplayLabelAlignment.AUTO &&
                        (current_dsp_mode == DSPMode.USB ||
                        current_dsp_mode == DSPMode.CWU))
                        xOffset -= 32;
                    SizeF size = g.MeasureString(label, font);

                    int x = 0;
                    switch(display_label_align)
                    {
                        case DisplayLabelAlignment.LEFT:
                            x = xOffset + 3;
                            break;
                        case DisplayLabelAlignment.CENTER:
                            x = center_line_x+xOffset;
                            break;
                        case DisplayLabelAlignment.RIGHT:
                            x = (int)(W-size.Width);
                            break;
                        case DisplayLabelAlignment.AUTO:
                            x = xOffset + 3;
                            break;
                        case DisplayLabelAlignment.OFF:
                            x = W;
                            break;
                    }

                    y -= 8;
                    if(y+9 < H)
                        g.DrawString(label, font, grid_text_brush, x, y);
                }
            }*/

            // Draw 0Hz vertical line if visible
            if (center_line_x >= 0 && center_line_x <= W)
            {
                g.DrawLine(new Pen(grid_zero_color), center_line_x, 0, center_line_x, top);
                g.DrawLine(new Pen(grid_zero_color), center_line_x - 1, 0, center_line_x - 1, top);
            }

            if (show_freq_offset)
                g.DrawString("0", font, new SolidBrush(grid_zero_color), center_line_x - 5, (float)Math.Floor(H * .01));

            if (high_swr)
                g.DrawString("High SWR", new System.Drawing.Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Red), 245, 20);
        }

        public static void ResetDisplayAverage()
        {
            average_buffer[0] = CLEAR_FLAG;	// set reset flag
        }

        public static void ResetDisplayPeak()
        {
            peak_buffer[0] = CLEAR_FLAG; // set reset flag
        }

        #endregion

        #endregion

        #region Background Class

        public class Background
        {
            public ArrayList strings;		// array of strings to be drawn on the background
            public ArrayList str_loc;		// array of top/left location of strings to be drawn
            public ArrayList lines;			// array of DXLines to be drawn
            public ArrayList overlay;		// array of points for overlay to be drawn

            public Background()
            {
                strings = new ArrayList();
                str_loc = new ArrayList();
                lines = new ArrayList();
                overlay = new ArrayList();
            }
        }

        #endregion
    }
}
