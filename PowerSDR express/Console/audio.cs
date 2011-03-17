//=================================================================
// audio.cs
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
 *  Copyright (C)2008,2009,2010,2011 YT7PWR Goran Radivojevic
 *  contact via email at: yt7pwr@ptt.rs or yt7pwr2002@yahoo.com
*/



//#define VAC_DEBUG
//#define MINMAX
//#define TIMER
//#define INTERLEAVED
//#define SPLIT_INTERLEAVED


using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;
using PaError = System.Int32;

namespace PowerSDR
{
    #region Enum

    public enum TestChannels
    {
        Left = 0,
        Right,
        Both,
        None,
    }

    #endregion

    public class Audio
	{
        #region Enums

        public enum AudioState
        {
            DTTSP = 0,
            CW,
            SINL_COSR,
            SINL_SINR,
            SINL_NOR,
            COSL_SINR,
            NOL_SINR,
            NOL_NOR,
            PIPE,
            SWITCH,
            CW_COSL_SINR,
        }

        public enum SignalSource
        {
            SOUNDCARD,
            SINE,
            NOISE,
            TRIANGLE,
            SAWTOOTH,
        }

        #endregion

		#region PowerSDR Specific Variables
		// ======================================================
		// PowerSDR Specific Variables
        // ======================================================

        unsafe private static PA19.PaStreamCallback callback1 = new PA19.PaStreamCallback(Callback1);
		unsafe private static PA19.PaStreamCallback callbackVAC = new PA19.PaStreamCallback(CallbackVAC);
		unsafe private static PA19.PaStreamCallback callback4port = new PA19.PaStreamCallback(Callback4Port);

        unsafe private static PA19.PaStreamCallback input_callbackVAC = new PA19.PaStreamCallback(input_CallbackVAC);
        unsafe private static PA19.PaStreamCallback output_callbackVAC = new PA19.PaStreamCallback(output_CallbackVAC);
        unsafe private static PA19.PaStreamCallback input_callback1 = new PA19.PaStreamCallback(input_Callback1);
        unsafe private static PA19.PaStreamCallback input_callback4port = new PA19.PaStreamCallback(input_Callback4Port);

        unsafe private static PA19.PaStreamCallback output_callback1 = new PA19.PaStreamCallback(output_Callback1);
        unsafe private static PA19.PaStreamCallback output_callback4port = new PA19.PaStreamCallback(output_Callback4Port);

        unsafe private static PA19.PaStreamCallback ClientCallback1 = new PA19.PaStreamCallback(NetworkClientCallback1);

        unsafe private static PA19.PaStreamCallback ServerCallbackFullSpectar = new PA19.PaStreamCallback(NetworkServerCallbackFullSpectar);
        unsafe private static PA19.PaStreamCallback ServerCallbackVACFullSpectar = new PA19.PaStreamCallback(NetworkServerCallbackVACFullSpectar);
        unsafe private static PA19.PaStreamCallback ServerCallback4portFullSpectar = new PA19.PaStreamCallback(NetworkServerCallback4PortFullSpectar);

        unsafe private static PA19.PaStreamCallback ServerCallbackAFSpectar = new PA19.PaStreamCallback(NetworkServerCallbackAFSpectar);
        unsafe private static PA19.PaStreamCallback ServerCallbackVACAFSpectar = new PA19.PaStreamCallback(NetworkServerCallbackVACAFSpectar);
        unsafe private static PA19.PaStreamCallback ServerCallback4portAFSpectar = new PA19.PaStreamCallback(NetworkServerCallback4PortAFSpectar);

        unsafe private static PA19.PaStreamCallback MultiPSKServerCallback = new PA19.PaStreamCallback(MultiPSKServerCallbackFunction);

        unsafe private static PA19.PaStreamCallback SA_callback = new PA19.PaStreamCallback(SA_Callback);
        unsafe private static PA19.PaStreamCallback SA_callback4port = new PA19.PaStreamCallback(SA_Callback4Port);

		public static int callback_return = 0;
        private static bool vac_callback = false;
        public static bool loopDLL_enabled = false;
        public static TestChannels ChannelTest = TestChannels.Left;

        private static bool server_rf_spectar = true;                // yt7pwr
        public static bool ServerRFSpectar
        {
            get { return server_rf_spectar; }
            set { server_rf_spectar = value; }
        }

        private static bool server_af_spectar = false;                // yt7pwr
        public static bool ServerAFSpectar
        {
            get { return server_af_spectar; }
            set { server_af_spectar = value; }
        }

        private static bool client_rf_spectar = true;                // yt7pwr
        public static bool ClientRFSpectar
        {
            get { return client_rf_spectar; }
            set { client_rf_spectar = value; }
        }

        private static bool client_af_spectar = false;                // yt7pwr
        public static bool ClientAFSpectar
        {
            get { return client_af_spectar; }
            set { client_af_spectar = value; }
        }

        private static bool enable_ethernet_server = false;      // yt7pwr
        public static bool EnableEthernetServerDevice
        {
            get { return enable_ethernet_server; }
            set { enable_ethernet_server = value; }
        }

        private static bool enable_ethernet_client = false;      // yt7pwr
        public static bool EnableEthernetClientDevice
        {
            get { return enable_ethernet_client; }
            set { enable_ethernet_client = value; }
        }

        private static bool enable_local_host = true;           // yt7pwr
        public static bool EnableLocalHost
        {
            get { return enable_local_host; }
            set { enable_local_host = value; }
        }

        private static bool enable_MultiPSK_server = false;      // yt7pwr
        public static bool EnableMultiPSKServer
        {
            get { return enable_MultiPSK_server; }
            set { enable_MultiPSK_server = value; }
        }

        private static bool enable_spectrum_analyzer = false;      // yt7pwr
        public static bool EnableSpectrumAnalyzer
        {
            get { return enable_spectrum_analyzer; }
            set { enable_spectrum_analyzer = value; }
        }

		private static bool spike = false;
		public static bool Spike
		{
			get { return spike; }
			set { spike = value; }
		}

		private static double input_source_scale = 1.0;
		public static double InputSourceScale
		{
			get { return input_source_scale; }
			set { input_source_scale = value; }
		}

		private static SignalSource current_input_signal = SignalSource.SOUNDCARD;
		public static SignalSource CurrentInputSignal
		{
			get { return current_input_signal; }
			set { current_input_signal = value; }
		}

		private static SignalSource current_output_signal = SignalSource.SOUNDCARD;
		public static SignalSource CurrentOutputSignal
		{
			get { return current_output_signal; }
			set { current_output_signal = value; }
		}

		private static bool record_rx_preprocessed = true;
		public static bool RecordRXPreProcessed
		{
			get { return record_rx_preprocessed; }
			set { record_rx_preprocessed = value; }
		}

		private static bool record_tx_preprocessed = false;
		public static bool RecordTXPreProcessed
		{
			get { return record_tx_preprocessed; }
			set { record_tx_preprocessed = value; }
		}

		private static float peak = 0.0f;
		public static float Peak
		{
			get { return peak; }
			set { peak = value; }
		}

		private static bool vox_enabled = false;
		public static bool VOXEnabled
		{
			get { return vox_enabled; }
			set { vox_enabled = value; }
		}

		private static float vox_threshold = 0.001f;
		public static float VOXThreshold
		{
			get { return vox_threshold; }
			set { vox_threshold = value; }
		}

		public static double TXScale
		{
			get { return high_swr_scale * radio_volume; }
		}

		private static double high_swr_scale = 1.0;
		public static double HighSWRScale
		{
			get { return high_swr_scale; }
			set	{ high_swr_scale = value; } 
		}

		private static double mic_preamp = 1.0;
		public static double MicPreamp
		{
			get { return mic_preamp; }
			set { mic_preamp = value; }
		}

        private static double voice_message_playback_preamp = 1.0;
        public static double VoiceMessagePlaybackPreamp             // yt7pwr
        {
            get { return voice_message_playback_preamp; }
            set { voice_message_playback_preamp = value; }
        }

		private static double wave_preamp = 1.0;
		public static double WavePreamp
		{
			get { return wave_preamp; }
			set { wave_preamp = value; }
		}

		private static double monitor_volume = 0.0;
		public static double MonitorVolume
		{
			get { return monitor_volume; }
			set
			{
				//Debug.WriteLine("monitor_volume: "+value.ToString("f3"));
				monitor_volume = value;
			}
		}

		private static double radio_volume = 0.0;
		public static double RadioVolume
		{
			get { return radio_volume; }
			set
			{
				//Debug.WriteLine("radio_volume: "+value.ToString("f3"));
				radio_volume = value; 				
			}
		}

		private static bool next_mox = false;
		public static bool NextMox
		{
			get { return next_mox; }
			set { next_mox = value; }
		}

		private static int ramp_samples = (int)(sample_rate1*0.005);
		private static double ramp_step = 1.0/ramp_samples;
		private static int ramp_count = 0;
		private static double ramp_val = 0.0;

		private static bool ramp_down = false;
		public static bool RampDown
		{
			get { return ramp_down; }
			set
			{
				ramp_down = value;
				ramp_samples = (int)(sample_rate1*0.005);
				ramp_step = 1.0/ramp_samples;
				ramp_count = 0;
                ramp_val = 1.0;
			}
		}

		private static bool ramp_up = false;
		public static bool RampUp
		{
			get { return ramp_up; }
			set
			{
				ramp_up = value;
				ramp_samples = (int)(sample_rate1*0.005);
				ramp_step = 1.0/ramp_samples;
				ramp_count = 0;
				ramp_val = 0.0;
			}
		}
	
		private static int ramp_up_num = 1;
		public static int RampUpNum
		{
			get { return ramp_up_num; }
			set	{ ramp_up_num = value; }
		}

		private static int switch_count = 1;
		public static int SwitchCount
		{
			get {return switch_count; }
			set {switch_count = value; }
		}

		private static AudioState current_audio_state1 = AudioState.DTTSP;
		public static AudioState CurrentAudioState1
		{
			get { return current_audio_state1; }
			set { current_audio_state1 = value; }
		}

		private static AudioState next_audio_state1 = AudioState.NOL_NOR;
		public static AudioState NextAudioState1
		{
			get { return next_audio_state1; }
			set { next_audio_state1 = value; }
		}

		private static AudioState save_audio_state1 = AudioState.NOL_NOR;
		public static AudioState SaveAudioState1
		{
			get { return save_audio_state1; }
			set { save_audio_state1 = value; }
		}

		private static double sine_freq1 = 1250.0;
		private static double phase_step1 = sine_freq1/sample_rate1*2*Math.PI;
		private static double phase_accumulator1 = 0.0;

		private static double sine_freq2 = 1900.0;
		private static double phase_step2 = sine_freq2/sample_rate1*2*Math.PI;
		private static double phase_accumulator2 = 0.0;

		public static double SineFreq1
		{
			get { return sine_freq1; }
			set
			{
				sine_freq1 = value;
				phase_step1 = sine_freq1/sample_rate1*2*Math.PI;
			}
		}

		public static double SineFreq2
		{
			get { return sine_freq2; }
			set
			{
				sine_freq2 = value;
				phase_step2 = sine_freq2/sample_rate1*2*Math.PI;
			}
		}

        public static byte[] network_input_bufer_l;
        public static byte[] network_input_bufer_r;
		public static Console console = null;
		unsafe private static void *stream1;    // input
		unsafe private static void *stream2;    // 
        unsafe private static void* stream3;    // output
        unsafe private static void* stream4;
        unsafe private static void* stream5;    // input VAC
        unsafe private static void* stream6;    // output VAC
		private static int block_size2 = 2048;
		public static float[] phase_buf_l;
		public static float[] phase_buf_r;
		public static bool phase = false;
		public static bool wave_record = false;
		public static bool wave_playback = false;
        public static bool voice_message_playback = false;
        public static bool voice_message_record = false;
		public static WaveFileWriter wave_file_writer;
		public static WaveFileReader wave_file_reader;
        public static VoiceMsgWaveFileWriter voice_msg_file_writer;
        public static VoiceMsgWaveFileReader voice_msg_file_reader;
		public static bool two_tone = false;
		//public static Mutex phase_mutex = new Mutex();
		public static bool high_pwr_am = false;
		public static bool testing = false;
        public static Mutex CATNetwork_mutex = new Mutex();
        public static Mutex DttSP_mutex = new Mutex();
        public static Mutex MultiPSK_mutex = new Mutex();
        public static bool TX_out_1_2 = true;             // 4 channel audio card

		#region VAC Variables

		private static RingBufferFloat rb_vacIN_l;
        private static RingBufferFloat rb_vacIN_r;
        private static RingBufferFloat rb_vacOUT_l;
        private static RingBufferFloat rb_vacOUT_r;

		private static float[] res_inl;
		private static float[] res_inr;
		private static float[] res_outl;
		private static float[] res_outr;
        private static float[] vac_outl;
        private static float[] vac_outr;
        private static float[] vac_inl;
        private static float[] vac_inr;

        unsafe private static double[] loopDLL_inl;
        unsafe private static double[] loopDLL_inr;
        unsafe private static double[] loopDLL_outl;
        unsafe private static double[] loopDLL_outr;

        unsafe private static void* resampServerPtrIn_l;
        unsafe private static void* resampServerPtrIn_r;
		unsafe private static void *resampPtrIn_l;
		unsafe private static void *resampPtrIn_r;
		unsafe private static void *resampPtrOut_l;
		unsafe private static void *resampPtrOut_r;

		private static bool vac_resample = false;

		#endregion

		#endregion

		#region Local Copies of External Properties

        private static bool VAC_audio_exclusive = false;       // yt7pwr
        public static bool VACAudioExclusive
        {
            get { return VAC_audio_exclusive; }
            set { VAC_audio_exclusive = value; }
        }

        private static bool audio_exclusive = false;       // yt7pwr
        public static bool AudioExclusive
        {
            get { return audio_exclusive; }
            set { audio_exclusive = value; }
        }

        private static int MultiPSK_RX_sample_rate = 11025;     // yt7pwr
        public static int MultiPSKRXSampleRate
        {
            set { MultiPSK_RX_sample_rate = value; }
        }

        private static int MultiPSK_TX_sample_rate = 11025;     // yt7pwr
        public static int MultiPSKTXSampleRate
        {
            set { MultiPSK_TX_sample_rate = value; }
        }

        private static int thread_no = 0;                       // yt7pwr
		private static bool mox = false;
		public static bool MOX
		{
			set 
            {
                mox = value;
/*                if (mox)
                    thread_no = 1;
                else
                    thread_no = 0;*/
            }
		}

		unsafe private static void *cs_vac;

		private static bool mon = false;
		public static bool MON
		{
            get { return mon; }
			set { mon = value; }
		}

        private static bool vac_mon = false;    // yt7pwr
        public static bool VAC_MON
        {
            get { return vac_mon; }
            set { vac_mon = value; }
        }

		private static bool vac_enabled = false;
		public static bool VACEnabled
		{
			set
			{
				vac_enabled = value;
				if(vac_enabled) InitVAC();
				else CleanUpVAC();
			}
			get { return vac_enabled; }
		}

		private static bool vac_rb_reset = false;
		public static bool VACRBReset
		{
			set
			{
				vac_rb_reset = value;
			}
			get { return vac_rb_reset; }
		}

		private static double vac_preamp = 1.0;
		public static double VACPreamp
		{
			get { return vac_preamp; }
			set
			{
				//Debug.WriteLine("vac_preamp: "+value.ToString("f3"));
				vac_preamp = value;
			}
		}

		private static double vac_rx_scale = 1.0;
		public static double VACRXScale
		{
			get { return vac_rx_scale; }
			set
			{
				//Debug.WriteLine("vac_rx_scale: "+value.ToString("f3"));
				vac_rx_scale = value;
			}
		}

        private static DSPMode dsp_mode_subRX = DSPMode.LSB;
        public static DSPMode CurDSPModeSubRX
        {
            set { dsp_mode_subRX = value; }
        }

		private static DSPMode dsp_mode = DSPMode.LSB;
		public static DSPMode CurDSPMode
		{
			set { dsp_mode = value; }
		}

		private static int sample_rate1 = 48000;
		public static int SampleRate1
		{
			get { return sample_rate1; }
			set	
			{
				sample_rate1 = value;
				SineFreq1 = sine_freq1;
				SineFreq2 = sine_freq2;
			}
		}

		private static int sample_rateVAC = 48000;
		public static int SampleRateVAC
		{
			get { return sample_rateVAC; }
			set 
			{
				sample_rateVAC = value; 
				if(vac_enabled) InitVAC();
			}			
		}

		private static int block_size1 = 1024;
		public static int BlockSize
		{
			get { return block_size1; }
			set { block_size1 = value; }			
		}

		private static int block_size_vac = 2048;
		public static int BlockSizeVAC
		{
			get { return block_size_vac; }
			set { block_size_vac = value; }			
		}

		private static double audio_volts1 = 2.23;
		public static double AudioVolts1
		{
			get { return audio_volts1; }
			set { audio_volts1 = value; }
		}

		private static bool vac_stereo = false;
		public static bool VACStereo
		{
			set { vac_stereo = value; }
		}

        //[patch_7
        private static bool vac_primary_audiodev = false;
        public static bool VACPrimaryAudiodevice
        {
            get { return vac_primary_audiodev; }
            set { vac_primary_audiodev = value; }
        }
        //patch_7]

		private static SoundCard soundcard = SoundCard.UNSUPPORTED_CARD;
		public static SoundCard CurSoundCard
		{
			set { soundcard = value; }
		}

		private static bool vox_active = false;
		public static bool VOXActive
		{
			get { return vox_active; }
			set { vox_active = value; }
		}

		private static int num_channels = 2;
		public static int NumChannels
		{
			set { num_channels = value; }
		}

		private static int host1 = 0;
		public static int Host1
		{
			set { host1 = value; }
		}

		private static int host2 = 0;
		public static int Host2
		{
			set { host2 = value; }
		}

		private static int input_dev1 = 0;
		public static int Input1
		{
			set { input_dev1 = value; }
		}

		private static int input_dev2 = 0;
		public static int Input2
		{
			set { input_dev2 = value; }
		}

        private static int input_dev3 = 0;      // yt7pwr
        public static int Input3
        {
            set { input_dev3 = value; }
        }

		private static int output_dev1 = 0;
		public static int Output1
		{
			set { output_dev1 = value; }
		}

		private static int output_dev2 = 0;
		public static int Output2
		{
			set { output_dev2 = value; }
		}

        private static int output_dev3 = 0;
        public static int Output3
        {
            set { output_dev3 = value; }
        }

		private static int latency1 = 0;
		public static int Latency1
		{
			set { latency1 = value; }
		}

		private static int latency2 = 0;
		public static int Latency2
		{
			set { latency2 = value; }
		}

		#endregion

		#region Callback Routines
		// ======================================================
		// Callback Routines
        // ======================================================

        #region classic callback

        unsafe public static int Callback1(void* input, void* output, int frameCount,           // changes yt7pwr
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            try
            {
#if(WIN64)
                Int64* array_ptr = (Int64*)input;
                float* in_l_ptr1 = (float*)array_ptr[0];
                float* in_r_ptr1 = (float*)array_ptr[1];
                double* VAC_in = (double*)input;
                array_ptr = (Int64*)output;
                float* out_l_ptr1 = (float*)array_ptr[1];
                float* out_r_ptr1 = (float*)array_ptr[0];
#endif

#if(WIN32)                
                int* array_ptr = (int*)input;
                float* in_l_ptr1 = (float*)array_ptr[0];
                float* in_r_ptr1 = (float*)array_ptr[1];
                double* VAC_in = (double*)input;
                array_ptr = (int*)output;
                float* out_l_ptr1 = (float*)array_ptr[1];
                float* out_r_ptr1 = (float*)array_ptr[0];
#endif

                if (wave_playback)
                    wave_file_reader.GetPlayBuffer(in_l_ptr1, in_r_ptr1);
                else if ((wave_record && !mox && record_rx_preprocessed) ||
                    (wave_record && mox && record_tx_preprocessed))
                    wave_file_writer.AddWriteBuffer(in_l_ptr1, in_r_ptr1);

                if (phase)
                {
                    //phase_mutex.WaitOne();
                    Marshal.Copy(new IntPtr(in_l_ptr1), phase_buf_l, 0, frameCount);
                    Marshal.Copy(new IntPtr(in_r_ptr1), phase_buf_r, 0, frameCount);
                    //phase_mutex.ReleaseMutex();
                }

                float* in_l = null, in_l_VAC = null, in_r = null, in_r_VAC = null, out_l = null, out_r = null;

                if (!mox && !voice_message_record)   // rx
                {
                    if (!console.RX_IQ_channel_swap)
                    {
                        in_l = in_l_ptr1;
                        in_r = in_r_ptr1;

                        out_l = out_l_ptr1;
                        out_r = out_r_ptr1;
                    }
                    else
                    {
                        in_l = in_r_ptr1;
                        in_r = in_l_ptr1;

                        out_l = out_r_ptr1;
                        out_r = out_l_ptr1;
                    }
                }
                else if (mox && !voice_message_record)
                {       // tx
                    if (voice_message_playback)
                    {
                        voice_msg_file_reader.GetPlayBuffer(in_l_ptr1, in_r_ptr1);
                    }

                    if (!console.TX_IQ_channel_swap)
                    {
                        in_r = in_l_ptr1;
                        in_l = in_r_ptr1;

                        out_r = out_l_ptr1;
                        out_l = out_r_ptr1;
                    }
                    else
                    {
                        in_r = in_l_ptr1;
                        in_l = in_r_ptr1;

                        out_l = out_l_ptr1;
                        out_r = out_r_ptr1;
                    }
                }
                else if (voice_message_record)
                {
                    in_l = in_l_ptr1;
                    in_r = in_r_ptr1;
                    out_l = out_l_ptr1;
                    out_r = out_r_ptr1;
                }

                if (voice_message_record)
                {
                    voice_msg_file_writer.AddWriteBuffer(in_l_ptr1, in_r_ptr1);
                }

                if (vac_enabled && loopDLL_enabled && mox)
                {
                    int i;

                    in_l_VAC = in_l;    // save pointer
                    in_r_VAC = in_r;    // save pointer

                    fixed (double* loopDLL_inl_ptr = &(loopDLL_inl[0]))
                    {
                        if (console.loopDLL.ReadTXBuffer(loopDLL_inl_ptr))
                        {
                            for (i = 0; i < frameCount; i++)
                            {
                                in_l[0] = (float)(loopDLL_inl[i] / 1e4);
                                in_r[0] = (float)(loopDLL_inl[i] / 1e4);
                                in_l++;
                                in_r++;
                            }
                        }
                        else
                        {

                        }
                    }

                    in_l = in_l_VAC;    // restore pointer
                    in_r = in_r_VAC;    // restore pointer
                }

                switch (current_audio_state1)
                {
                    case AudioState.DTTSP:
                        // scale input with mic preamp
                        if (mox && !vac_enabled &&
                            (dsp_mode == DSPMode.LSB ||
                            dsp_mode == DSPMode.USB ||
                            dsp_mode == DSPMode.DSB ||
                            dsp_mode == DSPMode.AM ||
                            dsp_mode == DSPMode.SAM ||
                            dsp_mode == DSPMode.FMN))
                        {
                            if (wave_playback)
                            {
                                ScaleBuffer(in_l, in_l, frameCount, (float)wave_preamp);
                                ScaleBuffer(in_r, in_r, frameCount, (float)wave_preamp);
                            }
                            else
                            {
                                ScaleBuffer(in_l, in_l, frameCount, (float)mic_preamp);
                                ScaleBuffer(in_r, in_r, frameCount, (float)mic_preamp);
                            }
                        }

                        #region Input Signal Source

                        switch (current_input_signal)
                        {
                            case SignalSource.SOUNDCARD:
                                break;
                            case SignalSource.SINE:
                                SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
                                phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
                                ScaleBuffer(in_l, in_l, frameCount, (float)input_source_scale);
                                ScaleBuffer(in_r, in_r, frameCount, (float)input_source_scale);
                                break;
                            case SignalSource.NOISE:
                                Noise(in_l, frameCount);
                                Noise(in_r, frameCount);
                                break;
                            case SignalSource.TRIANGLE:
                                Triangle(in_l, frameCount, sine_freq1);
                                CopyBuffer(in_l, in_r, frameCount);
                                break;
                            case SignalSource.SAWTOOTH:
                                Sawtooth(in_l, frameCount, sine_freq1);
                                CopyBuffer(in_l, in_r, frameCount);
                                break;
                        }

                        #endregion

                        if (!loopDLL_enabled && vac_enabled &&
                            rb_vacIN_l != null && rb_vacIN_r != null &&
                            rb_vacOUT_l != null && rb_vacOUT_r != null
                             && !voice_message_playback)
                        {
                            if (mox)
                            {
                                if (rb_vacIN_l.ReadSpace() >= frameCount)
                                {
                                    rb_vacIN_l.ReadPtr(in_l, frameCount);
                                }
                                else
                                {
                                    ClearBuffer(in_l, frameCount);
                                    VACDebug("rb_vacIN_l underflow");
                                }
                                if (rb_vacIN_r.ReadSpace() >= frameCount)
                                {
                                    rb_vacIN_r.ReadPtr(in_r, frameCount);
                                }
                                else
                                {
                                    ClearBuffer(in_r, frameCount);
                                    VACDebug("rb_vacIN_r underflow");
                                }

                                ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
                                ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);

                                DttSP.ExchangeSamples(thread_no, in_l, in_r, out_l, out_r, frameCount);
                            }
                            else
                            {
                                DttSP.ExchangeSamples(thread_no, in_l, in_r, out_l, out_r, frameCount);

                                if ((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
                                {
                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacIN_l.ReadPtr(in_l, frameCount);
                                    rb_vacIN_r.ReadPtr(in_r, frameCount);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                                else
                                {
                                    ClearBuffer(in_l, frameCount);
                                    ClearBuffer(in_r, frameCount);
                                    VACDebug("rb_vacIN_l underflow");
                                    VACDebug("rb_vacIN_r underflow");
                                }
                            }
                        }
                        else if (loopDLL_enabled && mox)
                        {
                            ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
                            ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);
                            DttSP.ExchangeSamples(thread_no, in_l, in_r, out_l, out_r, frameCount);
                        }
                        else
                            DttSP.ExchangeSamples(thread_no, in_l, in_r, out_l, out_r, frameCount);

                        #region Output Signal Source

                        switch (current_output_signal)
                        {
                            case SignalSource.SOUNDCARD:
                                break;
                            case SignalSource.SINE:
                                switch (ChannelTest)
                                {
                                    case TestChannels.Left:
                                        SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                        phase_accumulator1 = CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                        break;
                                    case TestChannels.Right:
                                        SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                        phase_accumulator1 = CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                        break;
                                    case TestChannels.Both:
                                        SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                        phase_accumulator1 = CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                        SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                        phase_accumulator2 = CosineWave(out_r_ptr1, frameCount, phase_accumulator2, sine_freq1);
                                        break;
                                }
                                break;
                            case SignalSource.NOISE:
                                switch (ChannelTest)
                                {
                                    case TestChannels.Both:
                                        Noise(out_l_ptr1, frameCount);
                                        Noise(out_r_ptr1, frameCount);
                                        break;
                                    case TestChannels.Left:
                                        Noise(out_l_ptr1, frameCount);
                                        break;
                                    case TestChannels.Right:
                                        Noise(out_r_ptr1, frameCount);
                                        break;
                                }
                                break;
                            case SignalSource.TRIANGLE:
                                switch (ChannelTest)
                                {
                                    case TestChannels.Both:
                                        Triangle(out_l_ptr1, frameCount, sine_freq1);
                                        CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                                        break;
                                    case TestChannels.Left:
                                        Triangle(out_l_ptr1, frameCount, sine_freq1);
                                        break;
                                    case TestChannels.Right:
                                        Triangle(out_r_ptr1, frameCount, sine_freq1);
                                        break;
                                }
                                break;
                            case SignalSource.SAWTOOTH:
                                switch (ChannelTest)
                                {
                                    case TestChannels.Both:
                                        Sawtooth(out_l_ptr1, frameCount, sine_freq1);
                                        CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                                        break;
                                    case TestChannels.Left:
                                        Sawtooth(out_l_ptr1, frameCount, sine_freq1);
                                        break;
                                    case TestChannels.Right:
                                        Sawtooth(out_r_ptr1, frameCount, sine_freq1);
                                        break;
                                }
                                break;
                        }

                        #endregion

                        break;
                    case AudioState.CW:
                        if (next_audio_state1 == AudioState.SWITCH)
                        {
                            Win32.memset(in_l_ptr1, 0, frameCount * sizeof(float));
                            Win32.memset(in_r_ptr1, 0, frameCount * sizeof(float));
                            if (vac_enabled)
                            {
                                if ((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
                                {
                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacIN_l.ReadPtr(in_l_ptr1, frameCount);
                                    rb_vacIN_r.ReadPtr(in_r_ptr1, frameCount);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                                else
                                {
                                    VACDebug("rb_vacIN_l underflow 4inTX");
                                }
                            }

                            DttSP.ExchangeSamples(thread_no, in_l_ptr1, in_r_ptr1, out_l_ptr1, out_r_ptr1, frameCount);

                            if (switch_count == 0) next_audio_state1 = AudioState.CW;
                            switch_count--;
                        }

                        DttSP.CWtoneExchange(out_r, out_l, frameCount);

                        break;
                    case AudioState.SINL_COSR:
                        if (two_tone)
                        {
                            double dump;

                            SineWave2Tone(out_l_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out dump, out dump);

                            CosineWave2Tone(out_r_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                        }
                        else
                        {
                            SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            phase_accumulator1 = CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                        }
                        break;
                    case AudioState.SINL_SINR:
                        if (two_tone)
                        {
                            SineWave2Tone(out_l_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);

                            CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                        }
                        else
                        {
                            phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                        }
                        break;
                    case AudioState.SINL_NOR:
                        if (two_tone)
                        {
                            SineWave2Tone(out_l_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                            ClearBuffer(out_r_ptr1, frameCount);
                        }
                        else
                        {
                            phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            ClearBuffer(out_r_ptr1, frameCount);
                        }
                        break;
                    case AudioState.CW_COSL_SINR:
                        if (two_tone)
                        {
                            double dump;

                            CosineWave2Tone(out_r_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out dump, out dump);

                            SineWave2Tone(out_l_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                        }
                        else
                        {
                            CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                        }
                        break;
                    case AudioState.COSL_SINR:
                        if (two_tone)
                        {
                            double dump;

                            CosineWave2Tone(out_l_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out dump, out dump);

                            SineWave2Tone(out_r_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                        }
                        else
                        {
                            CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            phase_accumulator1 = SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                        }
                        break;
                    case AudioState.NOL_SINR:
                        if (two_tone)
                        {
                            ClearBuffer(out_l_ptr1, frameCount);
                            SineWave2Tone(out_r_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                        }
                        else
                        {
                            ClearBuffer(out_l_ptr1, frameCount);
                            phase_accumulator1 = SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                        }
                        break;
                    case AudioState.NOL_NOR:
                        ClearBuffer(out_l_ptr1, frameCount);
                        ClearBuffer(out_r_ptr1, frameCount);
                        break;
                    case AudioState.PIPE:
                        CopyBuffer(in_l_ptr1, out_l_ptr1, frameCount);
                        CopyBuffer(in_r_ptr1, out_r_ptr1, frameCount);
                        break;
                    case AudioState.SWITCH:
                        if (!ramp_down && !ramp_up)
                        {
                            ClearBuffer(in_l_ptr1, frameCount);
                            ClearBuffer(in_r_ptr1, frameCount);
                            if (mox != next_mox) mox = next_mox;
                        }
                        if (vac_enabled)
                        {
                            if ((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
                            {
                                Win32.EnterCriticalSection(cs_vac);
                                rb_vacIN_l.ReadPtr(in_l_ptr1, frameCount);
                                rb_vacIN_r.ReadPtr(in_r_ptr1, frameCount);
                                Win32.LeaveCriticalSection(cs_vac);
                            }
                            else
                            {
                                VACDebug("rb_vacIN_l underflow 4inTX");
                            }
                        }

                        DttSP.ExchangeSamples(thread_no, in_l_ptr1, in_r_ptr1, out_l_ptr1, out_r_ptr1, frameCount);

                        if (ramp_down)
                        {
                            int i;
                            for (i = 0; i < frameCount; i++)
                            {
                                float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                out_l_ptr1[i] *= w;
                                out_r_ptr1[i] *= w;
                                ramp_val += ramp_step;
                                if (++ramp_count >= ramp_samples)
                                {
                                    ramp_down = false;
                                    break;
                                }
                            }

                            if (ramp_down)
                            {
                                for (; i < frameCount; i++)
                                {
                                    out_l_ptr1[i] = 0.0f;
                                    out_r_ptr1[i] = 0.0f;
                                }
                            }
                        }
                        else if (ramp_up)
                        {
                            for (int i = 0; i < frameCount; i++)
                            {
                                float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                out_l_ptr1[i] *= w;
                                out_r_ptr1[i] *= w;
                                ramp_val += ramp_step;
                                if (++ramp_count >= ramp_samples)
                                {
                                    ramp_up = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ClearBuffer(out_l_ptr1, frameCount);
                            ClearBuffer(out_r_ptr1, frameCount);
                        }

                        if (next_audio_state1 == AudioState.CW)
                        {
                            //cw_delay = 1;
                            DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
                        }
                        else if (switch_count == 1)
                            DttSP.CWRingRestart();

                        switch_count--;
                        if (switch_count == ramp_up_num) RampUp = true;
                        if (switch_count == 0)
                            current_audio_state1 = next_audio_state1;
                        break;
                }

                if (vac_enabled && !loopDLL_enabled &&
                    rb_vacIN_l != null && rb_vacIN_r != null &&
                    rb_vacOUT_l != null && rb_vacOUT_r != null)
                {
                    fixed (float* outl_ptr = &(vac_outl[0]))
                    fixed (float* outr_ptr = &(vac_outr[0]))
                    {
                        if (!mox)
                        {
                            ScaleBuffer(out_l, outl_ptr, frameCount, (float)vac_rx_scale);
                            ScaleBuffer(out_r, outr_ptr, frameCount, (float)vac_rx_scale);
                        }
                        else if (mox && vac_mon && (dsp_mode == DSPMode.CWU || dsp_mode == DSPMode.CWL))
                        {
                            ScaleBuffer(out_l, outl_ptr, frameCount, 0.0f);
                            ScaleBuffer(out_r, outr_ptr, frameCount, 0.0f);
                        }
                        else // zero samples going back to VAC since TX monitor is off
                        {
                            ScaleBuffer(out_l, outl_ptr, frameCount, 0.0f);
                            ScaleBuffer(out_r, outr_ptr, frameCount, 0.0f);
                        }

                        if((!vac_mon && mox) || !mox)
                        {
                            if (sample_rateVAC == sample_rate1)
                            {
                                if ((rb_vacOUT_l.WriteSpace() >= frameCount) && (rb_vacOUT_r.WriteSpace() >= frameCount))
                                {
                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacOUT_l.WritePtr(outl_ptr, frameCount);
                                    rb_vacOUT_r.WritePtr(outr_ptr, frameCount);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                                else
                                {
                                    VACDebug("rb_vacOUT_l overflow");
                                    VACDebug("rb_vacOUT_r overflow");
                                }
                            }
                            else
                            {
                                if (vac_stereo)
                                {
                                    fixed (float* res_outl_ptr = &(res_outl[0]))
                                    fixed (float* res_outr_ptr = &(res_outr[0]))
                                    {
                                        int outsamps;
                                        DttSP.DoResamplerF(outl_ptr, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
                                        DttSP.DoResamplerF(outr_ptr, res_outr_ptr, frameCount, &outsamps, resampPtrOut_r);
                                        if ((rb_vacOUT_l.WriteSpace() >= outsamps) && (rb_vacOUT_r.WriteSpace() >= outsamps))
                                        {
                                            Win32.EnterCriticalSection(cs_vac);
                                            rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
                                            rb_vacOUT_r.WritePtr(res_outr_ptr, outsamps);
                                            Win32.LeaveCriticalSection(cs_vac);
                                        }
                                        else
                                        {
                                            VACDebug("rb_vacOUT_l overflow");
                                            VACDebug("rb_vacOUT_r overflow");
                                        }
                                    }
                                }
                                else
                                {
                                    fixed (float* res_outl_ptr = &(res_outl[0]))
                                    {
                                        int outsamps;
                                        DttSP.DoResamplerF(outl_ptr, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
                                        if ((rb_vacOUT_l.WriteSpace() >= outsamps) && (rb_vacOUT_r.WriteSpace() >= outsamps))
                                        {
                                            Win32.EnterCriticalSection(cs_vac);
                                            rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
                                            rb_vacOUT_r.WritePtr(res_outl_ptr, outsamps);
                                            Win32.LeaveCriticalSection(cs_vac);
                                        }
                                        else
                                        {
                                            VACDebug("rb_vacOUT_l overflow");
                                            VACDebug("rb_vacOUT_r overflow");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                double vol = monitor_volume;
                if (mox)
                {
                    vol = TXScale;

                    if (high_pwr_am)
                    {
                        if (dsp_mode == DSPMode.AM ||
                            dsp_mode == DSPMode.SAM)
                            vol *= 1.414;
                    }
                }

                if ((wave_record && !mox && !record_rx_preprocessed) ||     // post process audio
                    (wave_record && mox && !record_tx_preprocessed))
                    wave_file_writer.AddWriteBuffer(out_r_ptr1, out_l_ptr1);

                //[patch_7
                if (VACPrimaryAudiodevice && !mox && current_audio_state1 == AudioState.DTTSP &&
                    !loopDLL_enabled)
                {
                    ClearBuffer(out_l_ptr1, frameCount);
                    ClearBuffer(out_r_ptr1, frameCount);
                }
                //patch_7]

                if (loopDLL_enabled && vac_enabled && !mox)
                {

                    int i;
                    double[] buffer = new double[frameCount];

                    fixed (double* buffer_ptr = &(buffer[0]))
                    fixed (float* res_outl_ptr = &(res_outl[0]))
                    {
                        ScaleBuffer(out_l, res_outl_ptr, frameCount, (float)vol);
                        ScaleBuffer(out_l, out_l, frameCount, 0.0f);
                        ScaleBuffer(out_r, out_r, frameCount, 0.0f);

                        {
                            for (i = 0; i < frameCount; i++)
                            {
                                buffer[i] = (double)1e5 * res_outl[i];
                            }

                            console.loopDLL.WriteRXBuffer(buffer_ptr);
                        }
                    }
                }
                else
                {
                    ScaleBuffer(out_l, out_l, frameCount, (float)vol);
                    ScaleBuffer(out_r, out_r, frameCount, (float)vol);
                }

                return callback_return;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return 0;
            }
        }

        unsafe public static int Callback4Port(void* input, void* output, int frameCount,       // changes yt7pwr
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            try
            {
                void* ex_input = (void*)input;
                void* ex_output = (void*)output;
                float* in_l = null, in_l_VAC = null, in_r = null, in_r_VAC = null;
                float* out_l1 = null, out_r1 = null, out_l2 = null, out_r2 = null;
                float* in_l_ptr1 = null;
                float* in_l_ptr2 = null;
                float* in_r_ptr1 = null;
                float* in_r_ptr2 = null;
                float* out_l_ptr1 = null;
                float* out_l_ptr2 = null;
                float* out_r_ptr1 = null;
                float* out_r_ptr2 = null;

                if (!mox)
                {
#if(WIN64)
                    Int64* array_ptr = (Int64*)input;
                    in_l_ptr1 = (float*)array_ptr[0];
                    in_r_ptr1 = (float*)array_ptr[1];
                    in_l_ptr2 = (float*)array_ptr[2];
                    in_r_ptr2 = (float*)array_ptr[3];
                    array_ptr = (Int64*)output;
                    out_l_ptr1 = (float*)array_ptr[0];
                    out_r_ptr1 = (float*)array_ptr[1];
                    out_l_ptr2 = (float*)array_ptr[2];
                    out_r_ptr2 = (float*)array_ptr[3];
#endif

#if(WIN32)
                    int* array_ptr = (int*)input;
                    in_l_ptr1 = (float*)array_ptr[0];
                    in_r_ptr1 = (float*)array_ptr[1];
                    in_l_ptr2 = (float*)array_ptr[2];
                    in_r_ptr2 = (float*)array_ptr[3];
                    array_ptr = (int*)output;
                    out_l_ptr1 = (float*)array_ptr[0];
                    out_r_ptr1 = (float*)array_ptr[1];
                    out_l_ptr2 = (float*)array_ptr[2];
                    out_r_ptr2 = (float*)array_ptr[3];
#endif
                }
                else
                {
#if(WIN64)

                    Int64* array_ptr = (Int64*)input;
                    in_l_ptr1 = (float*)array_ptr[0];
                    in_r_ptr1 = (float*)array_ptr[1];
                    in_l_ptr2 = (float*)array_ptr[2];
                    in_r_ptr2 = (float*)array_ptr[3];
                    array_ptr = (Int64*)output;
                    out_l_ptr1 = (float*)array_ptr[0];
                    out_r_ptr1 = (float*)array_ptr[1];
                    out_l_ptr2 = (float*)array_ptr[2];
                    out_r_ptr2 = (float*)array_ptr[3];
#endif

#if(WIN32)
                    int* array_ptr = (int*)input;
                    in_l_ptr1 = (float*)array_ptr[0];
                    in_r_ptr1 = (float*)array_ptr[1];
                    in_l_ptr2 = (float*)array_ptr[2];
                    in_r_ptr2 = (float*)array_ptr[3];
                    array_ptr = (int*)output;
                    out_l_ptr1 = (float*)array_ptr[0];
                    out_r_ptr1 = (float*)array_ptr[1];
                    out_l_ptr2 = (float*)array_ptr[2];
                    out_r_ptr2 = (float*)array_ptr[3];
#endif
                }

                out_l1 = out_l_ptr1;
                out_r1 = out_r_ptr1;
                out_l2 = out_l_ptr2;
                out_r2 = out_r_ptr2;

                switch (soundcard)
                {
                    case SoundCard.FIREBOX:
                    case SoundCard.EDIROL_FA_66:
                        if (!mox)
                        {
                            if (!console.RX_IQ_channel_swap)
                            {
                                in_l = in_l_ptr2;
                                in_r = in_r_ptr2;
                            }
                            else
                            {
                                in_r = in_l_ptr2;
                                in_l = in_r_ptr2;
                            }
                        }
                        else
                        {
                            if (!console.TX_IQ_channel_swap)
                            {
                                in_l = in_l_ptr1;
                                in_r = in_r_ptr1;
                            }
                            else
                            {
                                in_r = in_l_ptr1;
                                in_l = in_r_ptr1;
                            }
                        }
                        break;
                    case SoundCard.DELTA_44:
                    default:
                        if (!mox && !voice_message_record)   // rx
                        {
                            if (!console.RX_IQ_channel_swap)
                            {
                                in_l = in_l_ptr1;
                                in_r = in_r_ptr1;
                            }
                            else
                            {
                                in_r = in_l_ptr1;
                                in_l = in_r_ptr1;
                            }
                        }
                        else if (mox && !voice_message_record && !voice_message_playback)  // tx
                        {
                            if (!console.TX_IQ_channel_swap)
                            {
                                in_l = in_l_ptr2;
                                in_r = in_l_ptr2;
                                out_l1 = out_l_ptr1;
                                out_r1 = out_r_ptr1;
                            }
                            else
                            {
                                in_l = in_l_ptr2;
                                in_r = in_l_ptr2;
                                out_l1 = out_r_ptr1;
                                out_r1 = out_l_ptr1;
                            }
                        }
                        else if (voice_message_record)   //voice message record
                        {
                            in_l = in_l_ptr2;
                            in_r = in_l_ptr2;
                        }
                        else if (mox && voice_message_playback) //voice message playback
                        {
                            if (!console.TX_IQ_channel_swap)
                            {
                                in_l = in_l_ptr2;
                                in_r = in_r_ptr2;
                                out_l1 = out_l_ptr1;
                                out_r1 = out_r_ptr1;
                            }
                            else
                            {
                                in_l = in_l_ptr2;
                                in_r = in_r_ptr2;
                                out_l1 = out_r_ptr1;
                                out_r1 = out_l_ptr1;
                            }
                        }
                        break;
                }

                if (voice_message_playback)     // yt7pwr
                    voice_msg_file_reader.GetPlayBuffer(in_l, in_r);

                if (wave_playback)
                    wave_file_reader.GetPlayBuffer(in_l, in_r);

                if (voice_message_record)       // yt7pwr
                    voice_msg_file_writer.AddWriteBuffer(in_l, in_r);

                if (wave_record && !mox && record_rx_preprocessed ||
                    wave_record && mox && record_tx_preprocessed)
                    wave_file_writer.AddWriteBuffer(in_l, in_r);

                if (phase)
                {
                    //phase_mutex.WaitOne();
                    Marshal.Copy(new IntPtr(in_l), phase_buf_l, 0, frameCount);
                    Marshal.Copy(new IntPtr(in_r), phase_buf_r, 0, frameCount);
                    //phase_mutex.ReleaseMutex();
                }

                if (vac_enabled && loopDLL_enabled && mox)
                {
                    int i;

                    in_l_VAC = in_l;    // save pointer
                    in_r_VAC = in_r;    // save pointer

                    fixed (double* loopDLL_inl_ptr = &(loopDLL_inl[0]))
                    {
                        try
                        {
                            if (console.loopDLL.ReadTXBuffer(loopDLL_inl_ptr))
                            {
                                for (i = 0; i < frameCount; i++)
                                {
                                    in_l[0] = (float)(loopDLL_inl[i] / 1e5);
                                    in_r[0] = (float)(loopDLL_inl[i] / 1e5);
                                    in_l++;
                                    in_r++;
                                }
                            }
                            else
                            {

                            }
                        }
                        catch (Exception ex)
                        {
                            console.chkPower.Checked = false;
                            Thread.Sleep(100);
                            MessageBox.Show("loop.dll error!Check your PATH settings!\n" + ex.ToString());
                        }
                    }

                    in_l = in_l_VAC;    // restore pointer
                    in_r = in_r_VAC;    // restore pointer
                }

                switch (current_audio_state1)
                {
                    case AudioState.DTTSP:

                        #region VOX
                        float* vox_l = null, vox_r = null;
                        if (vox_enabled && !vac_enabled)
                        {
                            switch (soundcard)
                            {
                                case SoundCard.FIREBOX:
                                case SoundCard.EDIROL_FA_66:
                                    vox_l = in_l_ptr1;
                                    vox_r = in_r_ptr1;
                                    break;
                                case SoundCard.DELTA_44:
                                default:
                                    vox_l = in_l_ptr2;
                                    vox_r = in_r_ptr2;
                                    break;
                            }

                            if (dsp_mode == DSPMode.LSB ||
                                dsp_mode == DSPMode.USB ||
                                dsp_mode == DSPMode.DSB ||
                                dsp_mode == DSPMode.AM ||
                                dsp_mode == DSPMode.SAM ||
                                dsp_mode == DSPMode.FMN)
                            {
                                ScaleBuffer(vox_l, vox_l, frameCount, (float)mic_preamp);
                                ScaleBuffer(vox_r, vox_r, frameCount, (float)mic_preamp);
                                Peak = MaxSample(vox_l, vox_r, frameCount);

                                // compare power to threshold
                                if (Peak > vox_threshold)
                                    vox_active = true;
                                else
                                    vox_active = false;
                            }
                        }
                        else if (vox_enabled && vac_enabled)
                        {
                            if ((rb_vacIN_l.WriteSpace() >= frameCount) && (rb_vacIN_r.WriteSpace() >= frameCount))
                            {
                                if (mox)
                                {
                                    vox_l = in_l;
                                    vox_r = in_r;
                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacIN_l.ReadPtr(in_l, frameCount);
                                    rb_vacIN_r.ReadPtr(in_r, frameCount);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                                else
                                {
                                    fixed (float* vac_inl_ptr = &(vac_inl[0]))
                                    fixed (float* vac_inr_ptr = &(vac_inr[0]))
                                    {
                                        vox_l = vac_inl_ptr;
                                        vox_r = vac_inr_ptr;
                                        Win32.EnterCriticalSection(cs_vac);
                                        rb_vacIN_l.ReadPtr(vac_inl_ptr, frameCount);
                                        rb_vacIN_r.ReadPtr(vac_inr_ptr, frameCount);
                                        Win32.LeaveCriticalSection(cs_vac);
                                    }
                                }
                            }

                            if (dsp_mode == DSPMode.LSB ||
                                dsp_mode == DSPMode.USB ||
                                dsp_mode == DSPMode.DSB ||
                                dsp_mode == DSPMode.AM ||
                                dsp_mode == DSPMode.SAM ||
                                dsp_mode == DSPMode.FMN)
                            {
                                Peak = MaxSample(vox_l, vox_r, frameCount);

                                // compare power to threshold
                                if (Peak > vox_threshold)
                                    vox_active = true;
                                else
                                    vox_active = false;
                            }
                        }
                        #endregion

                        if (dsp_mode == DSPMode.CWU || dsp_mode == DSPMode.CWL)
                        {
                            DttSP.CWtoneExchange(out_l1, out_r1, frameCount);
                        }

                        // scale input with mic preamp
                        if (mox && !vac_enabled &&
                            (dsp_mode == DSPMode.LSB ||
                            dsp_mode == DSPMode.USB ||
                            dsp_mode == DSPMode.DSB ||
                            dsp_mode == DSPMode.AM ||
                            dsp_mode == DSPMode.SAM ||
                            dsp_mode == DSPMode.FMN))
                        {
                            if (wave_playback)
                            {
                                ScaleBuffer(in_l, in_l, frameCount, (float)wave_preamp);
                                ScaleBuffer(in_r, in_r, frameCount, (float)wave_preamp);
                            }
                            else
                            {
                                ScaleBuffer(in_l, in_l, frameCount, (float)mic_preamp);
                                ScaleBuffer(in_r, in_r, frameCount, (float)mic_preamp);
                            }
                        }

                        #region Input Signal Source

                        switch (current_input_signal)
                        {
                            case SignalSource.SOUNDCARD:
                                break;
                            case SignalSource.SINE:
                                SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
                                phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
                                ScaleBuffer(in_l, in_l, frameCount, (float)input_source_scale);
                                ScaleBuffer(in_r, in_r, frameCount, (float)input_source_scale);
                                break;
                            case SignalSource.NOISE:
                                Noise(in_l, frameCount);
                                Noise(in_r, frameCount);
                                break;
                            case SignalSource.TRIANGLE:
                                Triangle(in_l, frameCount, sine_freq1);
                                CopyBuffer(in_l, in_r, frameCount);
                                break;
                            case SignalSource.SAWTOOTH:
                                Sawtooth(in_l, frameCount, sine_freq1);
                                CopyBuffer(in_l, in_r, frameCount);
                                break;
                        }

                        #endregion

                        if (!loopDLL_enabled && vac_enabled && !voice_message_playback &&
                            rb_vacIN_l != null && rb_vacIN_r != null &&
                            rb_vacOUT_l != null && rb_vacOUT_r != null)
                        {
                            if (mox) // transmit mode
                            {
                                if (rb_vacIN_l.ReadSpace() >= frameCount)
                                {
                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacIN_l.ReadPtr(in_l, frameCount);
                                    rb_vacIN_r.ReadPtr(in_r, frameCount);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                                else
                                {
                                    ClearBuffer(in_l, frameCount);
                                    ClearBuffer(in_r, frameCount);
                                    VACDebug("rb_vacIN underflow 4inTX");
                                }

                                ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
                                ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);
                                DttSP.ExchangeSamples(thread_no, in_l, in_r, out_l_ptr1, out_r_ptr1, frameCount);
                            }
                            else // receive mode
                            {
                                DttSP.ExchangeSamples(thread_no, in_l, in_r, out_l_ptr1, out_r_ptr1, frameCount);

                                if ((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
                                {
                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacIN_l.ReadPtr(in_l, frameCount);
                                    rb_vacIN_r.ReadPtr(in_r, frameCount);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                                else
                                {
                                    ClearBuffer(in_l, frameCount);
                                    ClearBuffer(in_r, frameCount);
                                    VACDebug("rb_vacIN_l underflow 4inRX");
                                }
                            }
                        }
                        else if (loopDLL_enabled && vac_enabled && mox)
                        {
                            ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
                            ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);
                            DttSP.ExchangeSamples(thread_no, in_l, in_r, out_l_ptr1, out_r_ptr1, frameCount);
                        }
                        else
                            DttSP.ExchangeSamples(thread_no, in_l, in_r, out_l_ptr1, out_r_ptr1, frameCount);

                        #region Output Signal Source

                        switch (current_output_signal)
                        {
                            case SignalSource.SOUNDCARD:
                                break;
                            case SignalSource.SINE:
                                switch (ChannelTest)
                                {
                                    case TestChannels.Left:
                                        SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                        phase_accumulator1 = CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                        break;
                                    case TestChannels.Right:
                                        SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                        phase_accumulator1 = CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                        break;
                                    case TestChannels.Both:
                                        SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                        phase_accumulator1 = CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                        SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                        phase_accumulator2 = CosineWave(out_r_ptr1, frameCount, phase_accumulator2, sine_freq1);
                                        break;
                                }
                                break;
                            case SignalSource.NOISE:
                                switch (ChannelTest)
                                {
                                    case TestChannels.Both:
                                        Noise(out_l_ptr1, frameCount);
                                        Noise(out_r_ptr1, frameCount);
                                        break;
                                    case TestChannels.Left:
                                        Noise(out_l_ptr1, frameCount);
                                        break;
                                    case TestChannels.Right:
                                        Noise(out_r_ptr1, frameCount);
                                        break;
                                }
                                break;
                            case SignalSource.TRIANGLE:
                                switch (ChannelTest)
                                {
                                    case TestChannels.Both:
                                        Triangle(out_l_ptr1, frameCount, sine_freq1);
                                        CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                                        break;
                                    case TestChannels.Left:
                                        Triangle(out_l_ptr1, frameCount, sine_freq1);
                                        break;
                                    case TestChannels.Right:
                                        Triangle(out_r_ptr1, frameCount, sine_freq1);
                                        break;
                                }
                                break;
                            case SignalSource.SAWTOOTH:
                                switch (ChannelTest)
                                {
                                    case TestChannels.Both:
                                        Sawtooth(out_l_ptr1, frameCount, sine_freq1);
                                        CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                                        break;
                                    case TestChannels.Left:
                                        Sawtooth(out_l_ptr1, frameCount, sine_freq1);
                                        break;
                                    case TestChannels.Right:
                                        Sawtooth(out_r_ptr1, frameCount, sine_freq1);
                                        break;
                                }
                                break;
                        }

                        #endregion

                        break;
                    case AudioState.CW:
                        if (next_audio_state1 == AudioState.SWITCH)
                        {
                            ClearBuffer(in_l_ptr1, frameCount);
                            ClearBuffer(in_r_ptr1, frameCount);
                            if (vac_enabled)
                            {
                                if ((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
                                {
                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacIN_l.ReadPtr(in_l_ptr1, frameCount);
                                    rb_vacIN_r.ReadPtr(in_r_ptr1, frameCount);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                                else
                                {
                                    VACDebug("rb_vacIN_l underflow 4inTX");
                                }
                            }

                            DttSP.ExchangeSamples(thread_no, in_l_ptr1, in_r_ptr1, out_l_ptr1, out_r_ptr1, frameCount);
                            if (switch_count == 0) next_audio_state1 = AudioState.CW;
                            switch_count--;
                        }

                        if (console.TX_IQ_channel_swap)
                            DttSP.CWtoneExchange(out_l1, out_r1, frameCount);
                        else
                            DttSP.CWtoneExchange(out_r1, out_l1, frameCount);

                        break;
                    case AudioState.SINL_COSR:
                        out_l1 = out_l_ptr1;
                        out_r1 = out_r_ptr1;
                        out_l2 = out_l_ptr2;
                        out_r2 = out_r_ptr2;

                        if (two_tone)
                        {
                            double dump;

                            SineWave2Tone(out_l1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out dump, out dump);

                            CosineWave2Tone(out_r1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                        }
                        else
                        {
                            SineWave(out_l2, frameCount, phase_accumulator1, sine_freq1);
                            phase_accumulator1 = CosineWave(out_r2, frameCount, phase_accumulator1, sine_freq1);
                        }
                        break;
                    case AudioState.SINL_SINR:
                        out_l1 = out_l_ptr1;
                        out_r1 = out_r_ptr1;
                        out_l2 = out_l_ptr2;
                        out_r2 = out_r_ptr2;

                        if (two_tone)
                        {
                            SineWave2Tone(out_l1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                            CopyBuffer(out_l1, out_r1, frameCount);
                        }
                        else
                        {
                            phase_accumulator1 = SineWave(out_l1, frameCount, phase_accumulator1, sine_freq1);
                            CopyBuffer(out_l1, out_r1, frameCount);
                        }
                        break;
                    case AudioState.SINL_NOR:
                        out_l1 = out_l_ptr1;
                        out_r1 = out_r_ptr1;
                        out_l2 = out_l_ptr2;
                        out_r2 = out_r_ptr2;

                        if (two_tone)
                        {
                            SineWave2Tone(out_l1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                            ClearBuffer(out_r1, frameCount);
                        }
                        else
                        {
                            phase_accumulator1 = SineWave(out_l1, frameCount, phase_accumulator1, sine_freq1);
                            ClearBuffer(out_r1, frameCount);
                        }
                        break;
                    case AudioState.CW_COSL_SINR:
                        out_l1 = out_r_ptr1;
                        out_r1 = out_l_ptr1;
                        out_l2 = out_r_ptr2;
                        out_r2 = out_l_ptr2;
                        if (two_tone)
                        {
                            double dump;

                            CosineWave2Tone(out_l1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out dump, out dump);

                            SineWave2Tone(out_r1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                        }
                        else
                        {
                            CosineWave(out_r1, frameCount, phase_accumulator1, sine_freq1);
                            phase_accumulator1 = SineWave(out_l1, frameCount, phase_accumulator1, sine_freq1);
                        }
                        break;
                    case AudioState.COSL_SINR:
                        out_l1 = out_l_ptr1;
                        out_r1 = out_r_ptr1;
                        out_l2 = out_l_ptr2;
                        out_r2 = out_r_ptr2;
                        if (two_tone)
                        {
                            double dump;

                            CosineWave2Tone(out_l1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out dump, out dump);

                            SineWave2Tone(out_r1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                        }
                        else
                        {
                            CosineWave(out_l1, frameCount, phase_accumulator1, sine_freq1);
                            phase_accumulator1 = SineWave(out_r1, frameCount, phase_accumulator1, sine_freq1);
                        }
                        break;
                    case AudioState.NOL_SINR:
                        out_l1 = out_l_ptr1;
                        out_r1 = out_r_ptr1;
                        out_l2 = out_l_ptr2;
                        out_r2 = out_r_ptr2;

                        if (two_tone)
                        {
                            ClearBuffer(out_l1, frameCount);
                            SineWave2Tone(out_r1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                        }
                        else
                        {
                            ClearBuffer(out_l1, frameCount);
                            phase_accumulator1 = SineWave(out_r1, frameCount, phase_accumulator1, sine_freq1);
                        }
                        break;
                    case AudioState.NOL_NOR:
                        ClearBuffer(out_l_ptr1, frameCount);
                        ClearBuffer(out_r_ptr1, frameCount);
                        break;
                    case AudioState.PIPE:
                        CopyBuffer(in_l_ptr1, out_l_ptr1, frameCount);
                        CopyBuffer(in_r_ptr1, out_r_ptr1, frameCount);
                        break;
                    case AudioState.SWITCH:
                        if (!ramp_down && !ramp_up)
                        {
                            switch (dsp_mode)
                            {
                                case DSPMode.CWL:
                                case DSPMode.CWU:
                                    break;
                                default:
                                    ClearBuffer(in_l_ptr1, frameCount);
                                    ClearBuffer(in_r_ptr1, frameCount);
                                    break;
                            }
                            if (mox != next_mox) mox = next_mox;
                        }
                        if (vac_enabled)
                        {
                            if ((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
                            {
                                Win32.EnterCriticalSection(cs_vac);
                                rb_vacIN_l.ReadPtr(in_l_ptr1, frameCount);
                                rb_vacIN_r.ReadPtr(in_r_ptr1, frameCount);
                                Win32.LeaveCriticalSection(cs_vac);
                            }
                            else
                            {
                                VACDebug("rb_vacIN_l underflow 4inTX");
                            }
                        }
                        DttSP.ExchangeSamples(thread_no, in_l_ptr1, in_r_ptr1, out_l_ptr1, out_r_ptr1, frameCount);
                        if (ramp_down)
                        {
                            int i;
                            for (i = 0; i < frameCount; i++)
                            {
                                float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                out_l_ptr1[i] *= w;
                                out_r_ptr1[i] *= w;
                                ramp_val += ramp_step;
                                if (++ramp_count >= ramp_samples)
                                {
                                    ramp_down = false;
                                    break;
                                }
                            }

                            if (ramp_down)
                            {
                                for (; i < frameCount; i++)
                                {
                                    out_l_ptr1[i] = 0.0f;
                                    out_r_ptr1[i] = 0.0f;
                                }
                            }
                        }
                        else if (ramp_up)
                        {
                            for (int i = 0; i < frameCount; i++)
                            {
                                float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                out_l_ptr1[i] *= w;
                                out_r_ptr1[i] *= w;
                                ramp_val += ramp_step;
                                if (++ramp_count >= ramp_samples)
                                {
                                    ramp_up = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ClearBuffer(out_l_ptr1, frameCount);
                            ClearBuffer(out_r_ptr1, frameCount);
                        }

                        if (next_audio_state1 == AudioState.CW)
                        {
                            //cw_delay = 1;
                            DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
                        }
                        else if (switch_count == 1)
                            DttSP.CWRingRestart();

                        switch_count--;
                        //if(switch_count == ramp_up_num) RampUp = true;
                        if (switch_count == 0)
                            current_audio_state1 = next_audio_state1;
                        break;
                }

                // scale output for VAC
                if (vac_enabled && !loopDLL_enabled &&
                    rb_vacIN_l != null && rb_vacIN_r != null &&
                    rb_vacOUT_l != null && rb_vacOUT_r != null)
                {
                    fixed (float* outl_ptr = &(vac_outl[0]))
                    fixed (float* outr_ptr = &(vac_outr[0]))
                    {
                        if (!mox)
                        {
                            ScaleBuffer(out_l1, outl_ptr, frameCount, (float)vac_rx_scale);
                            ScaleBuffer(out_r1, outr_ptr, frameCount, (float)vac_rx_scale);
                        }
                        else // zero samples going back to VAC since TX monitor is off
                        {
                            ScaleBuffer(out_l1, outl_ptr, frameCount, 0.0f);
                            ScaleBuffer(out_r1, outr_ptr, frameCount, 0.0f);
                        }

                        if (sample_rateVAC == sample_rate1)
                        {
                            if ((rb_vacOUT_l.WriteSpace() >= frameCount) && (rb_vacOUT_r.WriteSpace() >= frameCount))
                            {
                                Win32.EnterCriticalSection(cs_vac);
                                rb_vacOUT_l.WritePtr(outl_ptr, frameCount);
                                rb_vacOUT_r.WritePtr(outr_ptr, frameCount);
                                Win32.LeaveCriticalSection(cs_vac);
                            }
                            else
                            {
                                VACDebug("rb_vacOUT_l overflow ");
                                vac_rb_reset = true;
                            }
                        }
                        else
                        {
                            if (vac_stereo)
                            {
                                fixed (float* res_outl_ptr = &(res_outl[0]))
                                fixed (float* res_outr_ptr = &(res_outr[0]))
                                {
                                    int outsamps;
                                    DttSP.DoResamplerF(outl_ptr, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
                                    DttSP.DoResamplerF(outr_ptr, res_outr_ptr, frameCount, &outsamps, resampPtrOut_r);
                                    if ((rb_vacOUT_l.WriteSpace() >= outsamps) && (rb_vacOUT_r.WriteSpace() >= outsamps))
                                    {
                                        Win32.EnterCriticalSection(cs_vac);
                                        rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
                                        rb_vacOUT_r.WritePtr(res_outr_ptr, outsamps);
                                        Win32.LeaveCriticalSection(cs_vac);
                                    }
                                    else
                                    {
                                        vac_rb_reset = true;
                                        VACDebug("rb_vacOUT_l overflow");
                                    }
                                }
                            }
                            else
                            {
                                fixed (float* res_outl_ptr = &(res_outl[0]))
                                {
                                    int outsamps;
                                    DttSP.DoResamplerF(outl_ptr, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
                                    if ((rb_vacOUT_l.WriteSpace() >= outsamps) && (rb_vacOUT_r.WriteSpace() >= outsamps))
                                    {
                                        Win32.EnterCriticalSection(cs_vac);
                                        rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
                                        rb_vacOUT_r.WritePtr(res_outl_ptr, outsamps);
                                        Win32.LeaveCriticalSection(cs_vac);
                                    }
                                    else
                                    {
                                        vac_rb_reset = true;
                                        VACDebug("rb_vacOUT_l overflow");
                                    }
                                }
                            }
                        }
                    }
                }

                if (wave_record && !mox && !record_rx_preprocessed)     // post process audio
                    wave_file_writer.AddWriteBuffer(out_l1, out_r1);
                else if (wave_record && mox && !record_tx_preprocessed)
                    wave_file_writer.AddWriteBuffer(out_l2, out_r2);

                if (loopDLL_enabled && vac_enabled && !mox)
                {
                    int i;
                    double vol = monitor_volume;
                    double[] buffer = new double[frameCount];

                    fixed (double* buffer_ptr = &(buffer[0]))
                    fixed (float* res_outl_ptr = &(res_outl[0]))
                    {
                        ScaleBuffer(out_l1, res_outl_ptr, frameCount, (float)vol);
                        ScaleBuffer(out_l1, out_l1, frameCount, (float)vol);
                        ScaleBuffer(out_r1, out_r1, frameCount, (float)vol);

                        {
                            for (i = 0; i < frameCount; i++)
                            {
                                buffer[i] = (double)1e5 * res_outl[i];
                            }

                            try
                            {
                                console.loopDLL.WriteRXBuffer(buffer_ptr);
                            }
                            catch (Exception ex)
                            {
                                console.chkPower.Checked = false;
                                Thread.Sleep(100);
                                MessageBox.Show("loop.dll error!Check your PATH settings!\n" + ex.ToString());
                            }
                        }
                    }
                }
                else
                {
                    if (!mox)                           // RX
                    {
                        ScaleBuffer(out_l1, out_l1, frameCount, (float)monitor_volume);
                        ScaleBuffer(out_r1, out_r1, frameCount, (float)monitor_volume);
                        ScaleBuffer(out_l1, out_l2, frameCount, (float)monitor_volume);
                        ScaleBuffer(out_r1, out_r2, frameCount, (float)monitor_volume);
                    }
                    else
                    {                                   // TX
                        double tx_vol = TXScale;
                        if (high_pwr_am)
                        {
                            if (dsp_mode == DSPMode.AM ||
                                dsp_mode == DSPMode.SAM)
                                tx_vol *= 1.414;
                        }

                        if (TX_out_1_2)
                        {
                            ScaleBuffer(out_l1, out_l1, frameCount, (float)tx_vol);
                            ScaleBuffer(out_r1, out_r1, frameCount, (float)tx_vol);
                        }
                        else
                        {
                            ScaleBuffer(out_l1, out_l2, frameCount, (float)tx_vol);
                            ScaleBuffer(out_r1, out_r2, frameCount, (float)tx_vol);
                        }

                        if (mon && !vac_mon && (dsp_mode == DSPMode.CWL || dsp_mode == DSPMode.CWU))
                        {
                            if (TX_out_1_2)
                            {
                                DttSP.CWMonitorExchange(out_l2, out_r2, frameCount);
                                ScaleBuffer(out_l2, out_l2, frameCount, (float)monitor_volume);
                                ScaleBuffer(out_r2, out_r2, frameCount, (float)monitor_volume);
                            }
                            else
                            {
                                DttSP.CWMonitorExchange(out_l1, out_r1, frameCount);
                                ScaleBuffer(out_l1, out_l1, frameCount, (float)monitor_volume);
                                ScaleBuffer(out_r1, out_r1, frameCount, (float)monitor_volume);
                            }
                        }
                        else if (mon && !vac_mon)
                        {
                            ScaleBuffer(out_l2, out_l2, frameCount, (float)monitor_volume);
                            ScaleBuffer(out_r2, out_r2, frameCount, (float)monitor_volume);
                            ScaleBuffer(out_l1, out_l1, frameCount, (float)monitor_volume);
                            ScaleBuffer(out_r1, out_r1, frameCount, (float)monitor_volume);
                        }
                        else
                        {
                            if (TX_out_1_2)
                            {
                                ScaleBuffer(out_l2, out_l2, frameCount, 0.0f);
                                ScaleBuffer(out_r2, out_r2, frameCount, 0.0f);
                            }
                            else
                            {
                                ScaleBuffer(out_l1, out_l1, frameCount, 0.0f);
                                ScaleBuffer(out_r1, out_r1, frameCount, 0.0f);
                            }
                        }
                    }
                }

                if (VACPrimaryAudiodevice && !mox && current_audio_state1 == AudioState.DTTSP &&
                    !loopDLL_enabled)
                {
                    if (TX_out_1_2)
                    {
                        ClearBuffer(out_l1, frameCount);
                        ClearBuffer(out_r1, frameCount);
                    }
                    else
                    {
                        ClearBuffer(out_l2, frameCount);
                        ClearBuffer(out_r2, frameCount);
                    }
                }

                if (!testing && soundcard != SoundCard.DELTA_44)
                {
                    // clip radio output to prevent overdrive
                    float clip_thresh = (float)(1.5f / audio_volts1);
                    for (int i = 0; i < frameCount; i++)
                    {
                        if (out_l2[i] > clip_thresh)
                        {
                            Debug.WriteLine("Clip Left High: " + out_l2[i].ToString("f5"));
                            out_l2[i] = clip_thresh;
                        }
                        else if (out_l2[i] < -clip_thresh)
                        {
                            Debug.WriteLine("Clip Left Low: " + out_l2[i].ToString("f5"));
                            out_l2[i] = -clip_thresh;
                        }

                        if (out_r2[i] > clip_thresh)
                        {
                            Debug.WriteLine("Clip Right High: " + out_l2[i].ToString("f5"));
                            out_r2[i] = clip_thresh;
                        }
                        else if (out_r2[i] < -clip_thresh)
                        {
                            Debug.WriteLine("Clip Right Low: " + out_l2[i].ToString("f5"));
                            out_r2[i] = -clip_thresh;
                        }
                    }

                    if (soundcard != SoundCard.DELTA_44)
                    {
                        // scale FireBox monitor output to prevent overdrive
                        ScaleBuffer(out_l1, out_l1, frameCount, (float)(1.5f / audio_volts1));
                        ScaleBuffer(out_r1, out_r1, frameCount, (float)(1.5f / audio_volts1));
                    }
                }

                return callback_return;
            }

            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return 0;
            }
        }

        unsafe public static int CallbackVAC(void* input, void* output, int frameCount,         // changes yt7pwr
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            try
            {
#if(WIN64)
                Int64* array_ptr = (Int64*)input;
                float* in_l_ptr1 = (float*)array_ptr[0];
                float* in_r_ptr1 = null;
                if (vac_stereo) in_r_ptr1 = (float*)array_ptr[1];
                array_ptr = (Int64*)output;
                float* out_l_ptr1 = (float*)array_ptr[0];
                float* out_r_ptr1 = null;
                if (vac_stereo) out_r_ptr1 = (float*)array_ptr[1];
#endif

#if(WIN32)
                int* array_ptr = (int*)input;
                float* in_l_ptr1 = (float*)array_ptr[0];
                float* in_r_ptr1 = null;
                if (vac_stereo) in_r_ptr1 = (float*)array_ptr[1];
                array_ptr = (int*)output;
                float* out_l_ptr1 = (float*)array_ptr[0];
                float* out_r_ptr1 = null;
                if (vac_stereo) out_r_ptr1 = (float*)array_ptr[1];
#endif

                if (vac_rb_reset)
                {
                    vac_rb_reset = false;
                    ClearBuffer(out_l_ptr1, frameCount);
                    if (vac_stereo) ClearBuffer(out_r_ptr1, frameCount);
                    Win32.EnterCriticalSection(cs_vac);
                    rb_vacIN_l.Reset();
                    rb_vacIN_r.Reset();
                    rb_vacOUT_l.Reset();
                    rb_vacOUT_r.Reset();
                    Win32.LeaveCriticalSection(cs_vac);
                    return 0;
                }

                //[patch_7
                #region VOX
                if (vox_enabled)
                {
                    float* vox_l = null, vox_r = null;
                    vox_l = in_l_ptr1;
                    vox_r = in_r_ptr1;

                    if (dsp_mode == DSPMode.LSB ||
                        dsp_mode == DSPMode.USB ||
                        dsp_mode == DSPMode.DSB ||
                        dsp_mode == DSPMode.AM ||
                        dsp_mode == DSPMode.SAM ||
                        dsp_mode == DSPMode.FMN)
                    {
                        Peak = MaxSample(vox_l, (vac_stereo) ? vox_r : vox_l, frameCount);

                        // compare power to threshold
                        if (Peak > vox_threshold)
                            vox_active = true;
                        else
                            vox_active = false;
                    }
                }
                #endregion
                //patch_7]

                if (vac_stereo)
                {
                    if (mox || vox_enabled)
                    {
                        if (vac_resample)
                        {
                            if ((console.CurrentDSPMode == DSPMode.CWU ||
                                console.CurrentDSPMode == DSPMode.CWL) && vac_mon)
                            {
                                DttSP.CWMonitorExchange(out_l_ptr1, out_r_ptr1, frameCount);
                            }

                            int outsamps;
                            fixed (float* res_inl_ptr = &(res_inl[0]))
                            fixed (float* res_inr_ptr = &(res_inr[0]))
                            {
                                DttSP.DoResamplerF(in_l_ptr1, res_inl_ptr, frameCount, &outsamps, resampPtrIn_l);
                                DttSP.DoResamplerF(in_r_ptr1, res_inr_ptr, frameCount, &outsamps, resampPtrIn_r);
                                if ((rb_vacIN_l.WriteSpace() >= outsamps) && (rb_vacIN_r.WriteSpace() >= outsamps))
                                {
                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacIN_l.WritePtr(res_inl_ptr, outsamps);
                                    rb_vacIN_r.WritePtr(res_inr_ptr, outsamps);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                                else
                                {
                                    vac_rb_reset = true;
                                    VACDebug("rb_vacIN overflow stereo CBvac");
                                }
                            }
                        }
                        else
                        {
                            if ((console.CurrentDSPMode == DSPMode.CWU ||
                                console.CurrentDSPMode == DSPMode.CWL) && vac_mon)
                            {
                                DttSP.CWMonitorExchange(out_l_ptr1, out_r_ptr1, frameCount);
                            }
                            else
                            {
                                if ((rb_vacIN_l.WriteSpace() >= frameCount) && (rb_vacIN_r.WriteSpace() >= frameCount))
                                {
                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacIN_l.WritePtr(in_l_ptr1, frameCount);
                                    rb_vacIN_r.WritePtr(in_r_ptr1, frameCount);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                                else
                                {
                                    //vac_rb_reset = true;
                                    VACDebug("rb_vacIN overflow mono CBvac");
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((rb_vacOUT_l.ReadSpace() >= frameCount) && (rb_vacOUT_r.ReadSpace() >= frameCount))
                        {
                            Win32.EnterCriticalSection(cs_vac);
                            rb_vacOUT_l.ReadPtr(out_l_ptr1, frameCount);
                            rb_vacOUT_r.ReadPtr(out_r_ptr1, frameCount);
                            Win32.LeaveCriticalSection(cs_vac);
                        }
                        else
                        {
                            ClearBuffer(out_l_ptr1, frameCount);
                            ClearBuffer(out_r_ptr1, frameCount);
                            VACDebug("rb_vacOUT underflow");
                        }
                    }
                }
                else
                {
                    if (vac_resample)
                    {
                        int outsamps;
                        fixed (float* res_inl_ptr = &(res_inl[0]))
                        {
                            DttSP.DoResamplerF(in_l_ptr1, res_inl_ptr, frameCount, &outsamps, resampPtrIn_l);
                            if ((rb_vacIN_l.WriteSpace() >= outsamps) && (rb_vacIN_r.WriteSpace() >= outsamps))
                            {
                                Win32.EnterCriticalSection(cs_vac);
                                rb_vacIN_l.WritePtr(res_inl_ptr, outsamps);
                                rb_vacIN_r.WritePtr(res_inl_ptr, outsamps);
                                Win32.LeaveCriticalSection(cs_vac);
                            }
                            else
                            {
                                //vac_rb_reset = true;
                                VACDebug("rb_vacIN_l overflow");
                            }
                        }
                    }
                    else
                    {
                        if ((rb_vacIN_l.WriteSpace() >= frameCount) && (rb_vacIN_r.WriteSpace() >= frameCount))
                        {
                            Win32.EnterCriticalSection(cs_vac);
                            rb_vacIN_l.WritePtr(in_l_ptr1, frameCount);
                            rb_vacIN_r.WritePtr(in_l_ptr1, frameCount);
                            Win32.LeaveCriticalSection(cs_vac);
                        }
                        else
                        {
                            //vac_rb_reset = true;
                            VACDebug("rb_vacIN_l overflow");
                        }
                    }
                    if ((rb_vacOUT_l.ReadSpace() >= frameCount) && (rb_vacOUT_r.ReadSpace() >= frameCount))
                    {
                        Win32.EnterCriticalSection(cs_vac);
                        rb_vacOUT_l.ReadPtr(out_l_ptr1, frameCount);
                        rb_vacOUT_r.ReadPtr(out_l_ptr1, frameCount);
                        Win32.LeaveCriticalSection(cs_vac);
                    }
                    else
                    {
                        ClearBuffer(out_l_ptr1, frameCount);
                        VACDebug("rb_vacOUT_l underflow");
                    }
                }

                //[patch_7
                if (VACPrimaryAudiodevice && mox && !mon)
                {
                    ClearBuffer(out_l_ptr1, frameCount);
                    if (vac_stereo) ClearBuffer(out_r_ptr1, frameCount);
                }
                //patch_7]

                double vol = monitor_volume;
                ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)vol);
                if (vac_stereo)
                    ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)vol);

                return 0;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return 0;
            }
        }

        #endregion

        #region Network client callback      // yt7pwr

        unsafe public static int NetworkClientCallback1(void* input, void* output, int frameCount,
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            try
            {
                double* VAC_in = (double*)input;

                int* out_array_ptr = (int*)output;
                float* out_l_ptr1 = (float*)out_array_ptr[0];
                float* out_r_ptr1 = (float*)out_array_ptr[1];
                byte* out_left = (byte*)out_l_ptr1;
                byte* out_right = (byte*)out_r_ptr1;

                float* tmp_input_l = stackalloc float[frameCount * sizeof(float)];
                float* tmp_input_r = stackalloc float[frameCount * sizeof(float)];
                float* tmp_in_l = (float*)tmp_input_l;
                float* tmp_in_r = (float*)tmp_input_r;
                byte* left = (byte*)tmp_in_l;
                byte* right = (byte*)tmp_in_r;

                if (client_rf_spectar)
                {
                    CATNetwork_mutex.WaitOne();
                    Marshal.Copy(network_input_bufer_l, 0, new IntPtr(tmp_in_l), 8192);
                    Marshal.Copy(network_input_bufer_r, 0, new IntPtr(tmp_in_r), 8192);
                    CATNetwork_mutex.ReleaseMutex();
                }
                else
                {
                    for (int i = 0; i < 1024; i++)
                    {
                        left[0] = network_input_bufer_l[i];
                        right[0] = network_input_bufer_l[i + 1];
                        left++;
                        right++;

                        i++;
                    }

                    fixed (float* res_outr_ptr = &(res_outr[0]))
                    fixed (float* res_outl_ptr = &(res_outl[0]))
                    {
                        int outsamps;
                        DttSP.DoResamplerF(tmp_in_l, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
                        DttSP.DoResamplerF(tmp_in_r, res_outr_ptr, frameCount, &outsamps, resampPtrOut_r);
                        tmp_in_l = res_outl_ptr;
                        tmp_in_r = res_outr_ptr;
                    }
                }

                if (wave_playback)
                {
                    tmp_in_l = (float*)tmp_input_l;
                    tmp_in_r = (float*)tmp_input_r;
                    wave_file_reader.GetPlayBuffer(tmp_in_l, tmp_in_r);
                }
                else if ((wave_record && !mox && record_rx_preprocessed) ||
                    (wave_record && mox && record_tx_preprocessed))
                {
                    tmp_in_l = (float*)tmp_input_l;
                    tmp_in_r = (float*)tmp_input_r;
                    wave_file_writer.AddWriteBuffer(tmp_in_l, tmp_in_r);
                }
                else if (voice_message_record && !console.MOX)
                {
                    tmp_in_l = (float*)tmp_input_l;
                    tmp_in_r = (float*)tmp_input_r;
                    wave_file_writer.AddWriteBuffer(tmp_in_l, tmp_in_r);
                }
                if (phase)
                {
                    //phase_mutex.WaitOne();
                    Marshal.Copy(new IntPtr(tmp_in_l), phase_buf_l, 0, frameCount);
                    Marshal.Copy(new IntPtr(tmp_in_r), phase_buf_r, 0, frameCount);
                    //phase_mutex.ReleaseMutex();
                }

                float* in_l = null, in_r = null, out_l = null, out_r = null, in_l_VAC = null, in_r_VAC = null;

                if (!console.RX_IQ_channel_swap)
                {
                    in_l = (float*)tmp_in_l;
                    in_r = (float*)tmp_in_r;

                    out_l = out_l_ptr1;
                    out_r = out_r_ptr1;
                }
                else
                {
                    in_l = (float*)tmp_in_r;
                    in_r = (float*)tmp_in_l;

                    out_l = out_r_ptr1;
                    out_r = out_l_ptr1;
                }

                if (vac_enabled && loopDLL_enabled && mox)
                {
                    int i;

                    in_l_VAC = in_l;    // save pointer
                    in_r_VAC = in_r;    // save pointer

                    fixed (double* loopDLL_inl_ptr = &(loopDLL_inl[0]))
                    {
                        if (console.loopDLL.ReadTXBuffer(loopDLL_inl_ptr))
                        {
                            for (i = 0; i < frameCount; i++)
                            {
                                in_l[0] = (float)(loopDLL_inl[i] / 1e4);
                                in_r[0] = (float)(loopDLL_inl[i] / 1e4);
                                in_l++;
                                in_r++;
                            }
                        }
                        else
                        {

                        }
                    }

                    in_l = in_l_VAC;    // restore pointer
                    in_r = in_r_VAC;    // restore pointer
                }

                switch (current_audio_state1)
                {
                    case AudioState.DTTSP:
                        if (dsp_mode == DSPMode.CWU || dsp_mode == DSPMode.CWL)
                        {
                            DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
                        }

                        // scale input with mic preamp
                        if (mox && !vac_enabled &&
                            (dsp_mode == DSPMode.LSB ||
                            dsp_mode == DSPMode.USB ||
                            dsp_mode == DSPMode.DSB ||
                            dsp_mode == DSPMode.AM ||
                            dsp_mode == DSPMode.SAM ||
                            dsp_mode == DSPMode.FMN))
                        {
                            if (wave_playback)
                            {
                                ScaleBuffer(in_l, in_l, frameCount, (float)wave_preamp);
                                ScaleBuffer(in_r, in_r, frameCount, (float)wave_preamp);
                            }
                            else
                            {
                                ScaleBuffer(in_l, in_l, frameCount, (float)mic_preamp);
                                ScaleBuffer(in_r, in_r, frameCount, (float)mic_preamp);
                            }
                        }

                        #region Input Signal Source

                        switch (current_input_signal)
                        {
                            case SignalSource.SOUNDCARD:
                                break;
                            case SignalSource.SINE:
                                SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
                                phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
                                ScaleBuffer(in_l, in_l, frameCount, (float)input_source_scale);
                                ScaleBuffer(in_r, in_r, frameCount, (float)input_source_scale);
                                break;
                            case SignalSource.NOISE:
                                Noise(in_l, frameCount);
                                Noise(in_r, frameCount);
                                break;
                            case SignalSource.TRIANGLE:
                                Triangle(in_l, frameCount, sine_freq1);
                                CopyBuffer(in_l, in_r, frameCount);
                                break;
                            case SignalSource.SAWTOOTH:
                                Sawtooth(in_l, frameCount, sine_freq1);
                                CopyBuffer(in_l, in_r, frameCount);
                                break;
                        }

                        #endregion

                        if (vac_enabled &&
                            rb_vacIN_l != null && rb_vacIN_r != null &&
                            rb_vacOUT_l != null && rb_vacOUT_r != null)
                        {
                            if (mox)
                            {
                                if (rb_vacIN_l.ReadSpace() >= frameCount) rb_vacIN_l.ReadPtr(in_l, frameCount);
                                else
                                {
                                    ClearBuffer(in_l, frameCount);
                                    VACDebug("rb_vacIN_l underflow");
                                }
                                if (rb_vacIN_r.ReadSpace() >= frameCount) rb_vacIN_r.ReadPtr(in_r, frameCount);
                                else
                                {
                                    ClearBuffer(in_r, frameCount);
                                    VACDebug("rb_vacIN_r underflow");
                                }
                                ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
                                ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);
                                DttSP.ExchangeSamples(thread_no, in_l, in_r, out_l, out_r, frameCount);
                            }
                            else
                            {
                                DttSP.ExchangeSamples(thread_no, in_l, in_r, out_l, out_r, frameCount);

                                if ((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
                                {
                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacIN_l.ReadPtr(in_l, frameCount);
                                    rb_vacIN_r.ReadPtr(in_r, frameCount);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                                else
                                {
                                    ClearBuffer(in_l, frameCount);
                                    ClearBuffer(in_r, frameCount);
                                    VACDebug("rb_vacIN_l underflow");
                                    VACDebug("rb_vacIN_r underflow");
                                }
                            }
                        }
                        else
                            DttSP.ExchangeSamples(thread_no, in_l, in_r, out_l, out_r, frameCount);

                        #region Output Signal Source

                        switch (current_output_signal)
                        {
                            case SignalSource.SOUNDCARD:
                                break;
                            case SignalSource.SINE:
                                SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                phase_accumulator1 = CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                break;
                            case SignalSource.NOISE:
                                Noise(out_l_ptr1, frameCount);
                                Noise(out_r_ptr1, frameCount);
                                break;
                            case SignalSource.TRIANGLE:
                                Triangle(out_l_ptr1, frameCount, sine_freq1);
                                CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                                break;
                            case SignalSource.SAWTOOTH:
                                Sawtooth(out_l_ptr1, frameCount, sine_freq1);
                                CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                                break;
                        }

                        #endregion

                        break;
                    case AudioState.CW:
                        if (next_audio_state1 == AudioState.SWITCH)
                        {
                            Win32.memset(tmp_in_l, 0, frameCount * sizeof(float));
                            Win32.memset(tmp_in_r, 0, frameCount * sizeof(float));
                            if (vac_enabled)
                            {
                                if ((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
                                {
                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacIN_l.ReadPtr(tmp_in_l, frameCount);
                                    rb_vacIN_r.ReadPtr(tmp_in_r, frameCount);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                                else
                                {
                                    VACDebug("rb_vacIN_l underflow 4inTX");
                                }
                            }

                            DttSP.ExchangeSamples(thread_no, tmp_in_l, tmp_in_r, out_l_ptr1, out_r_ptr1, frameCount);
                            if (switch_count == 0) next_audio_state1 = AudioState.CW;
                            switch_count--;
                        }

                        DttSP.CWtoneExchange(out_r_ptr1, out_l_ptr1, frameCount);

                        break;
                    case AudioState.SINL_COSR:
                        if (two_tone)
                        {
                            double dump;

                            SineWave2Tone(out_l_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out dump, out dump);

                            CosineWave2Tone(out_r_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                        }
                        else
                        {
                            SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            phase_accumulator1 = CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                        }
                        break;
                    case AudioState.SINL_SINR:
                        if (two_tone)
                        {
                            SineWave2Tone(out_l_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);

                            CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                        }
                        else
                        {
                            phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                        }
                        break;
                    case AudioState.SINL_NOR:
                        if (two_tone)
                        {
                            SineWave2Tone(out_l_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                            ClearBuffer(out_r_ptr1, frameCount);
                        }
                        else
                        {
                            phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            ClearBuffer(out_r_ptr1, frameCount);
                        }
                        break;
                    case AudioState.CW_COSL_SINR:
                        if (two_tone)
                        {
                            double dump;

                            CosineWave2Tone(out_r_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out dump, out dump);

                            SineWave2Tone(out_l_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                        }
                        else
                        {
                            CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                        }
                        break;
                    case AudioState.COSL_SINR:
                        if (two_tone)
                        {
                            double dump;

                            CosineWave2Tone(out_l_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out dump, out dump);

                            SineWave2Tone(out_r_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                        }
                        else
                        {
                            CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            phase_accumulator1 = SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                        }
                        break;
                    case AudioState.NOL_SINR:
                        if (two_tone)
                        {
                            ClearBuffer(out_l_ptr1, frameCount);
                            SineWave2Tone(out_r_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                        }
                        else
                        {
                            ClearBuffer(out_l_ptr1, frameCount);
                            phase_accumulator1 = SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                        }
                        break;
                    case AudioState.NOL_NOR:
                        ClearBuffer(out_l_ptr1, frameCount);
                        ClearBuffer(out_r_ptr1, frameCount);
                        break;
                    case AudioState.PIPE:
                        CopyBuffer(tmp_in_l, out_l_ptr1, frameCount);
                        CopyBuffer(tmp_in_r, out_r_ptr1, frameCount);
                        break;
                    case AudioState.SWITCH:
                        if (!ramp_down && !ramp_up)
                        {
                            ClearBuffer(tmp_in_l, frameCount);
                            ClearBuffer(tmp_in_r, frameCount);
                            if (mox != next_mox) mox = next_mox;
                        }
                        if (vac_enabled)
                        {
                            if ((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
                            {
                                Win32.EnterCriticalSection(cs_vac);
                                rb_vacIN_l.ReadPtr(tmp_in_l, frameCount);
                                rb_vacIN_r.ReadPtr(tmp_in_r, frameCount);
                                Win32.LeaveCriticalSection(cs_vac);
                            }
                            else
                            {
                                VACDebug("rb_vacIN_l underflow 4inTX");
                            }
                        }
                        DttSP.ExchangeSamples(thread_no, tmp_in_l, tmp_in_r, out_l_ptr1, out_r_ptr1, frameCount);
                        if (ramp_down)
                        {
                            int i;
                            for (i = 0; i < frameCount; i++)
                            {
                                float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                out_l_ptr1[i] *= w;
                                out_r_ptr1[i] *= w;
                                ramp_val += ramp_step;
                                if (++ramp_count >= ramp_samples)
                                {
                                    ramp_down = false;
                                    break;
                                }
                            }

                            if (ramp_down)
                            {
                                for (; i < frameCount; i++)
                                {
                                    out_l_ptr1[i] = 0.0f;
                                    out_r_ptr1[i] = 0.0f;
                                }
                            }
                        }
                        else if (ramp_up)
                        {
                            for (int i = 0; i < frameCount; i++)
                            {
                                float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                out_l_ptr1[i] *= w;
                                out_r_ptr1[i] *= w;
                                ramp_val += ramp_step;
                                if (++ramp_count >= ramp_samples)
                                {
                                    ramp_up = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ClearBuffer(out_l_ptr1, frameCount);
                            ClearBuffer(out_r_ptr1, frameCount);
                        }

                        if (next_audio_state1 == AudioState.CW)
                        {
                            //cw_delay = 1;
                            DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
                        }
                        else if (switch_count == 1)
                            DttSP.CWRingRestart();

                        switch_count--;
                        //if(switch_count == ramp_up_num) RampUp = true;
                        if (switch_count == 0)
                            current_audio_state1 = next_audio_state1;
                        break;
                }

                double vol = monitor_volume;
                if (mox)
                {
                    vol = TXScale;

                    if (high_pwr_am)
                    {
                        if (dsp_mode == DSPMode.AM ||
                            dsp_mode == DSPMode.SAM)
                            vol *= 1.414;
                    }
                }

                if ((wave_record && !mox && !record_rx_preprocessed) ||     // post process audio
                    (wave_record && mox && !record_tx_preprocessed))
                    wave_file_writer.AddWriteBuffer(out_l_ptr1, out_r_ptr1);

                if (VACPrimaryAudiodevice && !mox && current_audio_state1 == AudioState.DTTSP &&
                    !loopDLL_enabled)
                {
                    ClearBuffer(out_l_ptr1, frameCount);
                    ClearBuffer(out_r_ptr1, frameCount);
                }

                if (loopDLL_enabled && vac_enabled && !mox)     // rx
                {

                    int i;
                    double[] buffer = new double[frameCount];

                    fixed (double* buffer_ptr = &(buffer[0]))
                    fixed (float* res_outl_ptr = &(res_outl[0]))
                    {
                        ScaleBuffer(out_l, res_outl_ptr, frameCount, (float)vol);
                        ScaleBuffer(out_l, out_l, frameCount, 0.0f);
                        ScaleBuffer(out_r, out_r, frameCount, 0.0f);

                        {
                            for (i = 0; i < frameCount; i++)
                            {
                                buffer[i] = (double)1e5 * res_outl[i];
                            }

                            console.loopDLL.WriteRXBuffer(buffer_ptr);
                        }
                    }
                }
                else if (loopDLL_enabled && vac_enabled && mox)     // tx
                {
                    ScaleBuffer(out_l, out_l, frameCount, (float)vol);
                    ScaleBuffer(out_r, out_r, frameCount, (float)vol);

                    fixed (float* res_inl_ptr = &(res_inl[0]))
                    fixed (float* res_inr_ptr = &(res_inr[0]))
                    {
                        int outsamps;
                        DttSP.DoResamplerF(out_l, res_inl_ptr, frameCount, &outsamps, resampServerPtrIn_l);     // down to 6000
                        DttSP.DoResamplerF(out_r, res_inr_ptr, frameCount, &outsamps, resampServerPtrIn_r);

                        byte* tmp_l_ptr = (byte*)res_inl_ptr;
                        byte* tmp_r_ptr = (byte*)res_inr_ptr;
                        byte[] buffer = new byte[8192];

                        for (int i = 2; i < buffer.Length; i++)
                        {
                            buffer[i] = tmp_l_ptr[0];
                            buffer[i + 1] = tmp_r_ptr[0];
                            tmp_l_ptr++;
                            tmp_r_ptr++;

                            i++;
                        }

                        CATNetwork_mutex.WaitOne();

                        fixed (void* rptr = &buffer[0])
                        fixed (void* wptr = &console.ClientSocket.send_buffer[0])
                            Win32.memcpy(wptr, rptr, buffer.Length);

                        CATNetwork_mutex.WaitOne();
                    }

                    console.ClientSocket.sendEvent.Set();
                }
                else
                {
                    ScaleBuffer(out_l, out_l, frameCount, (float)vol);
                    ScaleBuffer(out_r, out_r, frameCount, (float)vol);
                }

                return callback_return;
            }
            catch (Exception  ex)
            {
                Debug.Write(ex.ToString());
                return 0;
            }
        }

        unsafe public static int NetworkClientCallback4(byte* buffer, int frameCount)
        {
            return 0;
        }

        #endregion

        #region Network server callback     // yt7pwr

        unsafe public static int NetworkServerCallbackAFSpectar(void* input, void* output, int frameCount,
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            try
            {
                int* array_ptr = (int*)input;
                float* in_l_ptr1 = (float*)array_ptr[0];
                float* in_r_ptr1 = (float*)array_ptr[1];
                int* array_out_ptr = (int*)output;
                float* out_l_ptr1 = (float*)array_out_ptr[0];
                float* out_r_ptr1 = (float*)array_out_ptr[1];

                if (wave_playback)
                    wave_file_reader.GetPlayBuffer(in_l_ptr1, in_r_ptr1);
                else if ((wave_record && !mox && record_rx_preprocessed) ||
                    (wave_record && mox && record_tx_preprocessed))
                    wave_file_writer.AddWriteBuffer(in_l_ptr1, in_r_ptr1);
                else if (voice_message_record && !console.MOX)
                    wave_file_writer.AddWriteBuffer(in_l_ptr1, in_r_ptr1);
                if (phase)
                {
                    //phase_mutex.WaitOne();
                    Marshal.Copy(new IntPtr(in_l_ptr1), phase_buf_l, 0, frameCount);
                    Marshal.Copy(new IntPtr(in_r_ptr1), phase_buf_r, 0, frameCount);
                    //phase_mutex.ReleaseMutex();
                }

                float* in_l = null, in_r = null, out_l = null, out_r = null;

                if (!mox)
                {
                    if (!console.RX_IQ_channel_swap)
                    {
                        in_l = in_l_ptr1;
                        in_r = in_r_ptr1;

                        out_l = out_l_ptr1;
                        out_r = out_r_ptr1;
                    }
                    else
                    {
                        in_l = in_r_ptr1;
                        in_r = in_l_ptr1;

                        out_l = out_r_ptr1;
                        out_r = out_l_ptr1;
                    }
                }
                else
                {
                    if (voice_message_playback)
                        voice_msg_file_reader.GetPlayBuffer(in_l_ptr1, in_r_ptr1);

                    if (!console.TX_IQ_channel_swap)
                    {
                        in_r = in_l_ptr1;
                        in_l = in_r_ptr1;

                        out_r = out_l_ptr1;
                        out_l = out_r_ptr1;
                    }
                    else
                    {
                        in_l = in_l_ptr1;
                        in_r = in_r_ptr1;

                        out_l = out_l_ptr1;
                        out_r = out_r_ptr1;
                    }

                }

                switch (current_audio_state1)
                {
                    case AudioState.DTTSP:
                        if (dsp_mode == DSPMode.CWU || dsp_mode == DSPMode.CWL)
                        {
                            DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
                        }

                        // scale input with mic preamp
                        if (mox && !vac_enabled &&
                            (dsp_mode == DSPMode.LSB ||
                            dsp_mode == DSPMode.USB ||
                            dsp_mode == DSPMode.DSB ||
                            dsp_mode == DSPMode.AM ||
                            dsp_mode == DSPMode.SAM ||
                            dsp_mode == DSPMode.FMN))
                        {
                            ScaleBuffer(in_l, in_l, frameCount, (float)mic_preamp);
                            ScaleBuffer(in_r, in_r, frameCount, (float)mic_preamp);
                        }

                        if (vac_enabled &&
                            rb_vacIN_l != null && rb_vacIN_r != null &&
                            rb_vacOUT_l != null && rb_vacOUT_r != null)
                        {
                            if (mox)
                            {
                                if (rb_vacIN_l.ReadSpace() >= frameCount) rb_vacIN_l.ReadPtr(in_l, frameCount);
                                else
                                {
                                    ClearBuffer(in_l, frameCount);
                                    VACDebug("rb_vacIN_l underflow");
                                }
                                if (rb_vacIN_r.ReadSpace() >= frameCount) rb_vacIN_r.ReadPtr(in_r, frameCount);
                                else
                                {
                                    ClearBuffer(in_r, frameCount);
                                    VACDebug("rb_vacIN_r underflow");
                                }
                                ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
                                ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);
                                DttSP.ExchangeSamples(thread_no, in_l, in_r, out_l, out_r, frameCount);
                            }
                            else
                            {
                                DttSP.ExchangeSamples(thread_no, in_l, in_r, out_l, out_r, frameCount);

                                if ((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
                                {
                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacIN_l.ReadPtr(in_l, frameCount);
                                    rb_vacIN_r.ReadPtr(in_r, frameCount);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                                else
                                {
                                    ClearBuffer(in_l, frameCount);
                                    ClearBuffer(in_r, frameCount);
                                    VACDebug("rb_vacIN_l underflow");
                                    VACDebug("rb_vacIN_r underflow");
                                }
                            }
                        }
                        else

                            DttSP.ExchangeSamples(thread_no, in_l, in_r, out_l, out_r, frameCount);

                        break;
                    case AudioState.CW:
                        if (next_audio_state1 == AudioState.SWITCH)
                        {
                            Win32.memset(in_l_ptr1, 0, frameCount * sizeof(float));
                            Win32.memset(in_r_ptr1, 0, frameCount * sizeof(float));

                            DttSP.ExchangeSamples(thread_no, in_l_ptr1, in_r_ptr1, out_l_ptr1, out_r_ptr1, frameCount);
                            if (switch_count == 0) next_audio_state1 = AudioState.CW;
                            switch_count--;
                        }

                        DttSP.CWtoneExchange(out_r_ptr1, out_l_ptr1, frameCount);

                        break;
                    case AudioState.SINL_COSR:
                        if (two_tone)
                        {
                            double dump;

                            SineWave2Tone(out_l_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out dump, out dump);

                            CosineWave2Tone(out_r_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                        }
                        else
                        {
                            SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            phase_accumulator1 = CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                        }
                        break;
                    case AudioState.SINL_SINR:
                        if (two_tone)
                        {
                            SineWave2Tone(out_l_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);

                            CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                        }
                        else
                        {
                            phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                        }
                        break;
                    case AudioState.SINL_NOR:
                        if (two_tone)
                        {
                            SineWave2Tone(out_l_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                            ClearBuffer(out_r_ptr1, frameCount);
                        }
                        else
                        {
                            phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            ClearBuffer(out_r_ptr1, frameCount);
                        }
                        break;
                    case AudioState.CW_COSL_SINR:
                        if (two_tone)
                        {
                            double dump;

                            CosineWave2Tone(out_r_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out dump, out dump);

                            SineWave2Tone(out_l_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                        }
                        else
                        {
                            CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                        }
                        break;
                    case AudioState.COSL_SINR:
                        if (two_tone)
                        {
                            double dump;

                            CosineWave2Tone(out_l_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out dump, out dump);

                            SineWave2Tone(out_r_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                        }
                        else
                        {
                            CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            phase_accumulator1 = SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                        }
                        break;
                    case AudioState.NOL_SINR:
                        if (two_tone)
                        {
                            ClearBuffer(out_l_ptr1, frameCount);
                            SineWave2Tone(out_r_ptr1, frameCount,
                                phase_accumulator1, phase_accumulator2,
                                sine_freq1, sine_freq2,
                                out phase_accumulator1, out phase_accumulator2);
                        }
                        else
                        {
                            ClearBuffer(out_l_ptr1, frameCount);
                            phase_accumulator1 = SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                        }
                        break;
                    case AudioState.NOL_NOR:
                        ClearBuffer(out_l_ptr1, frameCount);
                        ClearBuffer(out_r_ptr1, frameCount);
                        break;
                    case AudioState.PIPE:
                        CopyBuffer(in_l_ptr1, out_l_ptr1, frameCount);
                        CopyBuffer(in_r_ptr1, out_r_ptr1, frameCount);
                        break;
                    case AudioState.SWITCH:
                        if (!ramp_down && !ramp_up)
                        {
                            ClearBuffer(in_l_ptr1, frameCount);
                            ClearBuffer(in_r_ptr1, frameCount);
                            if (mox != next_mox) mox = next_mox;
                        }

                        DttSP.ExchangeSamples(thread_no, in_l_ptr1, in_r_ptr1, out_l_ptr1, out_r_ptr1, frameCount);

                        if (ramp_down)
                        {
                            int i;
                            for (i = 0; i < frameCount; i++)
                            {
                                float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                out_l_ptr1[i] *= w;
                                out_r_ptr1[i] *= w;
                                ramp_val += ramp_step;
                                if (++ramp_count >= ramp_samples)
                                {
                                    ramp_down = false;
                                    break;
                                }
                            }

                            if (ramp_down)
                            {
                                for (; i < frameCount; i++)
                                {
                                    out_l_ptr1[i] = 0.0f;
                                    out_r_ptr1[i] = 0.0f;
                                }
                            }
                        }
                        else if (ramp_up)
                        {
                            for (int i = 0; i < frameCount; i++)
                            {
                                float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                out_l_ptr1[i] *= w;
                                out_r_ptr1[i] *= w;
                                ramp_val += ramp_step;
                                if (++ramp_count >= ramp_samples)
                                {
                                    ramp_up = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ClearBuffer(out_l_ptr1, frameCount);
                            ClearBuffer(out_r_ptr1, frameCount);
                        }

                        if (next_audio_state1 == AudioState.CW)
                        {
                            //cw_delay = 1;
                            DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
                        }
                        else if (switch_count == 1)
                            DttSP.CWRingRestart();

                        switch_count--;
                        //if(switch_count == ramp_up_num) RampUp = true;
                        if (switch_count == 0)
                            current_audio_state1 = next_audio_state1;
                        break;
                }

                fixed (float* res_inl_ptr = &(res_inl[0]))
                fixed (float* res_inr_ptr = &(res_inr[0]))
                {
                    int outsamps;
                    DttSP.DoResamplerF(out_l, res_inl_ptr, frameCount, &outsamps, resampServerPtrIn_l);     // down to 6000
                    DttSP.DoResamplerF(out_r, res_inr_ptr, frameCount, &outsamps, resampServerPtrIn_r);

                    byte* tmp_l_ptr = (byte*)res_inl_ptr;
                    byte* tmp_r_ptr = (byte*)res_inr_ptr;
                    byte[] buffer = new byte[8192];

                    for (int i = 2; i < buffer.Length; i++)
                    {
                        buffer[i] = tmp_l_ptr[0];
                        buffer[i + 1] = tmp_r_ptr[0];
                        tmp_l_ptr++;
                        tmp_r_ptr++;

                        i++;
                    }

                    CATNetwork_mutex.WaitOne();

                    fixed (void* rptr = &buffer[0])
                    fixed (void* wptr = &console.ServerSocket.send_buffer[0])
                        Win32.memcpy(wptr, rptr, buffer.Length);

                    CATNetwork_mutex.WaitOne();
                }

                console.ServerSocket.sendEvent.Set();

                double vol = monitor_volume;
                if (mox)
                {
                    vol = TXScale;

                    if (high_pwr_am)
                    {
                        if (dsp_mode == DSPMode.AM ||
                            dsp_mode == DSPMode.SAM)
                            vol *= 1.414;
                    }
                }

                ScaleBuffer(out_l, out_l, frameCount, (float)vol);
                ScaleBuffer(out_r, out_r, frameCount, (float)vol);

                return 0;
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return -1;
            }
        }

        unsafe public static int NetworkServerCallbackFullSpectar(void* input, void* output, int frameCount,
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            try
            {
                int* array_ptr = (int*)input;
                float* in_ptr = (float*)array_ptr[0];
                byte* tmp_ptr = (byte*)in_ptr;
                byte[] buffer = new byte[frameCount * sizeof(float) * 2 + 2];

                for (int i = 2; i < buffer.Length; i++)
                {
                    buffer[i] = tmp_ptr[0];
                    tmp_ptr++;
                }

                CATNetwork_mutex.WaitOne();

                // prepare for sending new data
                fixed (void* rptr = &buffer[0])
                fixed (void* wptr = &console.ServerSocket.send_buffer[0])
                    Win32.memcpy(wptr, rptr, buffer.Length);

                CATNetwork_mutex.ReleaseMutex();

                console.ServerSocket.sendEvent.Set();

                return 0;
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return -1;
            }
        }

        unsafe public static int NetworkServerCallback4PortAFSpectar(void* input, void* output, int frameCount,
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            return 0;
        }

		unsafe public static int NetworkServerCallback4PortFullSpectar(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
        {
            try
            {
                int* array_ptr = (int*)input;
                float* in_ptr = (float*)array_ptr[0];
                byte* tmp_ptr = (byte*)in_ptr;
                byte[] buffer = new byte[frameCount * sizeof(float) * 2 + 2];

                for (int i = 2; i < buffer.Length; i++)
                {
                    buffer[i] = tmp_ptr[0];
                    tmp_ptr++;
                }

                CATNetwork_mutex.WaitOne();

                // prepare for sending new data
                fixed (void* rptr = &buffer[0])
                fixed (void* wptr = &console.ServerSocket.send_buffer[0])
                    Win32.memcpy(wptr, rptr, buffer.Length);

                CATNetwork_mutex.ReleaseMutex();

                console.ServerSocket.sendEvent.Set();
                return 0;
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return -1;
            }
        }

        unsafe public static int NetworkServerCallbackVACAFSpectar(void* input, void* output, int frameCount,
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            return 0;
        }

        unsafe public static int NetworkServerCallbackVACFullSpectar(void* input, void* output, int frameCount,
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            return 0;
        }

        #endregion

        #region MultiPSK callback   // yt7pwr

        unsafe public static int MultiPSKServerCallbackFunction(void* input, void* output, int frameCount,
    PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            try
            {
                int* array_ptr = (int*)input;
                float* in_l_ptr1 = (float*)array_ptr[0];
                float* in_r_ptr1 = (float*)array_ptr[1];
                double* VAC_in = (double*)input;
                array_ptr = (int*)output;
                float* out_l_ptr1 = (float*)array_ptr[1];
                float* out_r_ptr1 = (float*)array_ptr[0];

                float* in_l = null, in_l_VAC = null, in_r = null, in_r_VAC = null, out_l = null, out_r = null;

                if (!mox)                                    // rx
                {
                    if (!console.RX_IQ_channel_swap)
                    {
                        in_l = in_l_ptr1;
                        in_r = in_r_ptr1;

                        out_l = out_l_ptr1;
                        out_r = out_r_ptr1;
                    }
                    else
                    {
                        in_l = in_r_ptr1;
                        in_r = in_l_ptr1;

                        out_l = out_r_ptr1;
                        out_r = out_l_ptr1;
                    }
                }
                else if (mox)
                {                                            // tx

                    if (!console.TX_IQ_channel_swap)
                    {
                        in_r = in_l_ptr1;
                        in_l = in_r_ptr1;

                        out_r = out_l_ptr1;
                        out_l = out_r_ptr1;
                    }
                    else
                    {
                        in_r = in_l_ptr1;
                        in_l = in_r_ptr1;

                        out_l = out_l_ptr1;
                        out_r = out_r_ptr1;
                    }
                }

                switch (current_audio_state1)
                {
                    case AudioState.DTTSP:

                        // scale input with mic preamp
                        if (mox && (dsp_mode == DSPMode.USB ||
                            dsp_mode == DSPMode.DIGU))
                        {
                            fixed (float* res_inl_ptr = &(res_inl[0]))
                            fixed (float* res_outl_ptr = &(res_outl[0]))
                            {
                                int outsamps, i;

                                console.MultiPSKServer.send_mutex.WaitOne();

                                for (i = 0; i < 2048; i++)
                                {
                                    res_inl[i] = (float)network_input_bufer_l[i] / 1000;
                                }

                                console.MultiPSKServer.send_mutex.ReleaseMutex();

                                DttSP.DoResamplerF(res_inl_ptr, res_outl_ptr, frameCount, &outsamps, resampPtrIn_l);

                                ScaleBuffer(res_outl_ptr, in_l, frameCount, (float)mic_preamp);
                                ScaleBuffer(res_outl_ptr, in_r, frameCount, (float)mic_preamp);
                            }
                        }

                        DttSP.ExchangeSamples(thread_no, in_l, in_r, out_l, out_r, frameCount);
                        break;
                    case AudioState.SWITCH:
                        mox = next_mox;
                        if (!ramp_down && !ramp_up)
                        {
                            ClearBuffer(in_l_ptr1, frameCount);
                            ClearBuffer(in_r_ptr1, frameCount);
                        }

                        DttSP.ExchangeSamples(thread_no, in_l_ptr1, in_r_ptr1, out_l_ptr1, out_r_ptr1, frameCount);

                        if (ramp_down)
                        {
                            int i;
                            for (i = 0; i < frameCount; i++)
                            {
                                float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                out_l_ptr1[i] *= w;
                                out_r_ptr1[i] *= w;
                                ramp_val += ramp_step;
                                if (++ramp_count >= ramp_samples)
                                {
                                    ramp_down = false;
                                    break;
                                }
                            }

                            if (ramp_down)
                            {
                                for (; i < frameCount; i++)
                                {
                                    out_l_ptr1[i] = 0.0f;
                                    out_r_ptr1[i] = 0.0f;
                                }
                            }
                        }
                        else if (ramp_up)
                        {
                            for (int i = 0; i < frameCount; i++)
                            {
                                float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                out_l_ptr1[i] *= w;
                                out_r_ptr1[i] *= w;
                                ramp_val += ramp_step;
                                if (++ramp_count >= ramp_samples)
                                {
                                    ramp_up = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ClearBuffer(out_l_ptr1, frameCount);
                            ClearBuffer(out_r_ptr1, frameCount);
                        }

                        if (next_audio_state1 == AudioState.CW)
                        {
                            //cw_delay = 1;
                            DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
                        }
                        else if (switch_count == 1)
                            DttSP.CWRingRestart();

                        switch_count--;
                        //if(switch_count == ramp_up_num) RampUp = true;
                        if (switch_count == 0)
                            current_audio_state1 = next_audio_state1;
                        break;
                }

                double vol = monitor_volume;
                if (!mox && console.MultiPSKServer.ClientConnected)
                {
                    fixed (float* res_outl_ptr = &(res_outl[0]))
                    {
                        int outsamps;

                        DttSP.DoResamplerF(out_l, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
                        ScaleBuffer(res_outl_ptr, res_outl_ptr, frameCount, (float)vol);

                        byte[] buffer = new byte[console.MultiPSKServer.send_buffer.Length];

                        for (int i = 0; i < buffer.Length; i++)
                        {
                            buffer[i] = (byte)(100 * res_outl[i]);
                        }

                        console.MultiPSKServer.send_mutex.WaitOne();
                        
                        // prepare for sending new data
                        fixed (void* rptr = &buffer[0])
                        fixed (void* wptr = &console.MultiPSKServer.send_buffer[0])
                            Win32.memcpy(wptr, rptr, buffer.Length);

                        console.MultiPSKServer.send_mutex.ReleaseMutex();

                        if (!mox)
                            console.MultiPSKServer.send_event.Set();
                    }

                    ScaleBuffer(out_l, out_l, frameCount, 0.0f);
                    ScaleBuffer(out_r, out_r, frameCount, 0.0f);
                }
                else if (!mox)
                {
                    ScaleBuffer(out_l, out_l, frameCount, (float)vol);
                    ScaleBuffer(out_r, out_r, frameCount, (float)vol);
                }

                return callback_return;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return 0;
            }

        }

        #endregion

        #region Spectrum Analyzer callback  // yt7pwr

        unsafe public static int SA_Callback(void* input, void* output, int frameCount,
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            try
            {
                int* array_ptr = (int*)input;
                float* in_l_ptr1 = (float*)array_ptr[0];
                float* in_r_ptr1 = (float*)array_ptr[1];
                double* VAC_in = (double*)input;
                array_ptr = (int*)output;
                float* out_l_ptr1 = (float*)array_ptr[1];
                float* out_r_ptr1 = (float*)array_ptr[0];

                if ((wave_record && !mox && record_rx_preprocessed) ||
                    (wave_record && mox && record_tx_preprocessed))
                    wave_file_writer.AddWriteBuffer(in_l_ptr1, in_r_ptr1);

                float* in_l = null, in_l_VAC = null, in_r = null, in_r_VAC = null, out_l = null, out_r = null;

                if (!console.RX_IQ_channel_swap)
                {
                    in_l = in_l_ptr1;
                    in_r = in_r_ptr1;

                    out_l = out_l_ptr1;
                    out_r = out_r_ptr1;
                }
                else
                {
                    in_l = in_r_ptr1;
                    in_r = in_l_ptr1;

                    out_l = out_r_ptr1;
                    out_r = out_l_ptr1;
                }

                if (voice_message_record)
                    voice_msg_file_writer.AddWriteBuffer(in_l_ptr1, in_r_ptr1);

                DttSP.ExchangeSamples(thread_no, in_l, in_r, out_l, out_r, frameCount);

                ScaleBuffer(out_l, out_l, frameCount, 0.0f);
                ScaleBuffer(out_r, out_r, frameCount, 0.0f);

                return callback_return;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return 0;
            }
        }

        unsafe public static int SA_Callback4Port(void* input, void* output, int frameCount,
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            return 1;
        }

        #endregion

        #region Win7 callback   // yt7pwr

        unsafe public static int input_Callback1(void* input, void* output, int frameCount,     // yt7pwr
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            try
            {
                if (num_channels == 4 && mox)
                {
                }
                else if (num_channels == 2 || (num_channels == 4 && !mox))
                {
#if(WIN64)
                    Int64* array_ptr = (Int64*)input;
                    float* in_l_ptr1 = (float*)array_ptr[0];
                    float* in_r_ptr1 = (float*)array_ptr[1];
                    double* VAC_in = (double*)input;
#endif

#if(WIN32)
                    int* array_ptr = (int*)input;
                    float* in_l_ptr1 = (float*)array_ptr[0];
                    float* in_r_ptr1 = (float*)array_ptr[1];
                    double* VAC_in = (double*)input;
#endif

                    if (wave_playback)
                        wave_file_reader.GetPlayBuffer(in_l_ptr1, in_r_ptr1);
                    else if ((wave_record && !mox && record_rx_preprocessed) ||
                        (wave_record && mox && record_tx_preprocessed))
                        wave_file_writer.AddWriteBuffer(in_l_ptr1, in_r_ptr1);

                    float* in_l = null, in_l_VAC = null, in_r = null, in_r_VAC = null, out_l = null, out_r = null;

                    if (!mox && !voice_message_record)   // rx
                    {
                        if (!console.RX_IQ_channel_swap)
                        {
                            in_l = in_l_ptr1;
                            in_r = in_r_ptr1;
                        }
                        else
                        {
                            in_l = in_r_ptr1;
                            in_r = in_l_ptr1;
                        }
                    }
                    else if (mox && !voice_message_record)
                    {       // tx
                        if (voice_message_playback)
                            voice_msg_file_reader.GetPlayBuffer(in_l_ptr1, in_r_ptr1);

                        if (!console.TX_IQ_channel_swap)
                        {
                            in_r = in_l_ptr1;
                            in_l = in_r_ptr1;
                        }
                        else
                        {
                            in_r = in_l_ptr1;
                            in_l = in_r_ptr1;
                        }
                    }
                    else if (voice_message_record)
                    {
                        in_l = in_l_ptr1;
                        in_r = in_r_ptr1;
                    }

                    if (voice_message_record)
                        voice_msg_file_writer.AddWriteBuffer(in_l_ptr1, in_r_ptr1);

                    if (vac_enabled && loopDLL_enabled && mox)
                    {
                        int i;

                        in_l_VAC = in_l;    // save pointer
                        in_r_VAC = in_r;    // save pointer

                        fixed (double* loopDLL_inl_ptr = &(loopDLL_inl[0]))
                        {
                            if (console.loopDLL.ReadTXBuffer(loopDLL_inl_ptr))
                            {
                                for (i = 0; i < frameCount; i++)
                                {
                                    in_l[0] = (float)(loopDLL_inl[i] / 1e4);
                                    in_r[0] = (float)(loopDLL_inl[i] / 1e4);
                                    in_l++;
                                    in_r++;
                                }
                            }
                        }

                        in_l = in_l_VAC;    // restore pointer
                        in_r = in_r_VAC;    // restore pointer
                    }

                    switch (current_audio_state1)
                    {
                        case AudioState.DTTSP:

                            // scale input with mic preamp
                            if (mox && !vac_enabled &&
                                (dsp_mode == DSPMode.LSB ||
                                dsp_mode == DSPMode.USB ||
                                dsp_mode == DSPMode.DSB ||
                                dsp_mode == DSPMode.AM ||
                                dsp_mode == DSPMode.SAM ||
                                dsp_mode == DSPMode.FMN))
                            {
                                if (wave_playback)
                                {
                                    ScaleBuffer(in_l, in_l, frameCount, (float)wave_preamp);
                                    ScaleBuffer(in_r, in_r, frameCount, (float)wave_preamp);
                                }
                                else
                                {
                                    ScaleBuffer(in_l, in_l, frameCount, (float)mic_preamp);
                                    ScaleBuffer(in_r, in_r, frameCount, (float)mic_preamp);
                                }
                            }

                            #region Input Signal Source

                            switch (current_input_signal)
                            {
                                case SignalSource.SOUNDCARD:
                                    break;
                                case SignalSource.SINE:
                                    SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
                                    phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
                                    ScaleBuffer(in_l, in_l, frameCount, (float)input_source_scale);
                                    ScaleBuffer(in_r, in_r, frameCount, (float)input_source_scale);
                                    break;
                                case SignalSource.NOISE:
                                    Noise(in_l, frameCount);
                                    Noise(in_r, frameCount);
                                    break;
                                case SignalSource.TRIANGLE:
                                    Triangle(in_l, frameCount, sine_freq1);
                                    CopyBuffer(in_l, in_r, frameCount);
                                    break;
                                case SignalSource.SAWTOOTH:
                                    Sawtooth(in_l, frameCount, sine_freq1);
                                    CopyBuffer(in_l, in_r, frameCount);
                                    break;
                            }

                            #endregion

                            if (!loopDLL_enabled && vac_enabled &&
                                rb_vacIN_l != null && rb_vacIN_r != null &&
                                rb_vacOUT_l != null && rb_vacOUT_r != null)
                            {
                                if (mox)
                                {
                                    if (rb_vacIN_l.ReadSpace() >= frameCount)
                                    {
                                        rb_vacIN_l.ReadPtr(in_l, frameCount);
                                    }
                                    else
                                    {
                                        ClearBuffer(in_l, frameCount);
                                        VACDebug("rb_vacIN_l underflow");
                                    }
                                    if (rb_vacIN_r.ReadSpace() >= frameCount)
                                    {
                                        rb_vacIN_r.ReadPtr(in_r, frameCount);
                                    }
                                    else
                                    {
                                        ClearBuffer(in_r, frameCount);
                                        VACDebug("rb_vacIN_r underflow");
                                    }

                                    ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
                                    ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);

                                    DttSP.ExchangeInputSamples(thread_no, in_l, in_r, frameCount);
                                }
                                else
                                {
                                    DttSP.ExchangeInputSamples(thread_no, in_l, in_r, frameCount);

                                    if ((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
                                    {
                                        Win32.EnterCriticalSection(cs_vac);
                                        rb_vacIN_l.ReadPtr(in_l, frameCount);
                                        rb_vacIN_r.ReadPtr(in_r, frameCount);
                                        Win32.LeaveCriticalSection(cs_vac);
                                    }
                                    else
                                    {
                                        ClearBuffer(in_l, frameCount);
                                        ClearBuffer(in_r, frameCount);
                                        VACDebug("rb_vacIN_l underflow");
                                        VACDebug("rb_vacIN_r underflow");
                                    }
                                }
                            }
                            else if (loopDLL_enabled && mox)
                            {
                                ScaleBuffer(in_l, in_l, frameCount, (float)vac_preamp);
                                ScaleBuffer(in_r, in_r, frameCount, (float)vac_preamp);

                                DttSP.ExchangeInputSamples(thread_no, in_l, in_r, frameCount);
                            }
                            else
                            {
                                DttSP_mutex.WaitOne();
                                DttSP.ExchangeInputSamples(thread_no, in_l, in_r, frameCount);
                                DttSP_mutex.ReleaseMutex();
                            }
                            break;
                        case AudioState.CW:
                            if (next_audio_state1 == AudioState.SWITCH)
                            {
                                Win32.memset(in_l_ptr1, 0, frameCount * sizeof(float));
                                Win32.memset(in_r_ptr1, 0, frameCount * sizeof(float));
                                if (vac_enabled)
                                {
                                    if ((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
                                    {
                                        Win32.EnterCriticalSection(cs_vac);
                                        rb_vacIN_l.ReadPtr(in_l_ptr1, frameCount);
                                        rb_vacIN_r.ReadPtr(in_r_ptr1, frameCount);
                                        Win32.LeaveCriticalSection(cs_vac);
                                    }
                                    else
                                    {
                                        VACDebug("rb_vacIN_l underflow 4inTX");
                                    }
                                }

                                DttSP_mutex.WaitOne();
                                DttSP.ExchangeInputSamples(thread_no, in_l_ptr1, in_r_ptr1, frameCount);
                                DttSP_mutex.ReleaseMutex();

                                if (switch_count == 0) next_audio_state1 = AudioState.CW;
                                switch_count--;
                            }
                            break;
                        case AudioState.SWITCH:
                            if (!ramp_down && !ramp_up)
                            {
                                ClearBuffer(in_l_ptr1, frameCount);
                                ClearBuffer(in_r_ptr1, frameCount);
                                if (mox != next_mox) mox = next_mox;
                            }
                            if (vac_enabled)
                            {
                                if ((rb_vacIN_l.ReadSpace() >= frameCount) && (rb_vacIN_r.ReadSpace() >= frameCount))
                                {
                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacIN_l.ReadPtr(in_l_ptr1, frameCount);
                                    rb_vacIN_r.ReadPtr(in_r_ptr1, frameCount);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                                else
                                {
                                    VACDebug("rb_vacIN_l underflow 4inTX");
                                }
                            }

                            DttSP_mutex.WaitOne();
                            DttSP.ExchangeInputSamples(thread_no, in_l_ptr1, in_r_ptr1, frameCount);
                            DttSP_mutex.ReleaseMutex();

                            if (ramp_down)
                            {
                                int i;
                                for (i = 0; i < frameCount; i++)
                                {
                                    float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                    in_l[i] *= w;
                                    in_r[i] *= w;
                                    ramp_val += ramp_step;
                                    if (++ramp_count >= ramp_samples)
                                    {
                                        ramp_down = false;
                                        break;
                                    }
                                }

                                if (ramp_down)
                                {
                                    for (; i < frameCount; i++)
                                    {
                                        in_l[i] = 0.0f;
                                        in_r[i] = 0.0f;
                                    }
                                }
                            }
                            else if (ramp_up)
                            {
                                for (int i = 0; i < frameCount; i++)
                                {
                                    float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                    in_l[i] *= w;
                                    in_r[i] *= w;
                                    ramp_val += ramp_step;
                                    if (++ramp_count >= ramp_samples)
                                    {
                                        ramp_up = false;
                                        break;
                                    }
                                }
                            }

                            switch_count--;
                            if (switch_count == ramp_up_num) RampUp = true;
                            if (switch_count == 0)
                                current_audio_state1 = next_audio_state1;
                            break;
                    }
                }

                return callback_return;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return 0;
            }
        }

        unsafe public static int output_Callback1(void* input, void* output, int frameCount,    // yt7pwr
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            try
            {
#if(WIN64)
                Int64* array_ptr = (Int64*)output;
                float* out_l_ptr1 = (float*)array_ptr[0];
                float* out_r_ptr1 = (float*)array_ptr[1];
#endif

#if(WIN32)
                int* array_ptr = (int*)output;
                float* out_r_ptr1 = (float*)array_ptr[0];
                float* out_l_ptr1 = (float*)array_ptr[1];
#endif

                if (num_channels == 4 && mox && !TX_out_1_2)
                {
                    if (mon)
                    {
                        DttSP.CWMonitorExchange(out_l_ptr1, out_r_ptr1, frameCount);
                        ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)monitor_volume);
                        ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)monitor_volume);
                    }
                    else
                    {
                        ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, 0.0f);
                        ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, 0.0f);
                    }
                }
                else if (num_channels == 2 || (num_channels ==4 && !mox) ||
                    (num_channels == 4 && mox && TX_out_1_2))
                {
                    float* in_l_VAC = null, in_r_VAC = null, out_l = null, out_r = null;

                    if (!mox && !voice_message_record)   // rx
                    {
                        if (!console.RX_IQ_channel_swap)
                        {
                            out_l = out_l_ptr1;
                            out_r = out_r_ptr1;
                        }
                        else
                        {
                            out_l = out_r_ptr1;
                            out_r = out_l_ptr1;
                        }
                    }
                    else if (mox && !voice_message_record)
                    {       // tx

                        if (!console.TX_IQ_channel_swap)
                        {
                            out_r = out_l_ptr1;
                            out_l = out_r_ptr1;
                        }
                        else
                        {
                            out_l = out_l_ptr1;
                            out_r = out_r_ptr1;
                        }
                    }
                    else if (voice_message_record)
                    {
                        out_l = out_l_ptr1;
                        out_r = out_r_ptr1;
                    }

                    switch (current_audio_state1)
                    {
                        case AudioState.DTTSP:
                            if (dsp_mode == DSPMode.CWU || dsp_mode == DSPMode.CWL)
                            {
                                DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
                            }

                            DttSP_mutex.WaitOne();
                            DttSP.ExchangeOutputSamples(thread_no, out_l, out_r, frameCount);
                            DttSP_mutex.ReleaseMutex();

                            #region Output Signal Source

                            switch (current_output_signal)
                            {
                                case SignalSource.SOUNDCARD:
                                    break;
                                case SignalSource.SINE:
                                    switch (ChannelTest)
                                    {
                                        case TestChannels.Left:
                                            SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                            phase_accumulator1 = CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                            break;
                                        case TestChannels.Right:
                                            SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                            phase_accumulator1 = CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                            break;
                                        case TestChannels.Both:
                                            SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                            phase_accumulator1 = CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                            SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                            phase_accumulator2 = CosineWave(out_r_ptr1, frameCount, phase_accumulator2, sine_freq1);
                                            break;
                                    }
                                    break;
                                case SignalSource.NOISE:
                                    switch (ChannelTest)
                                    {
                                        case TestChannels.Both:
                                            Noise(out_l_ptr1, frameCount);
                                            Noise(out_r_ptr1, frameCount);
                                            break;
                                        case TestChannels.Left:
                                            Noise(out_l_ptr1, frameCount);
                                            break;
                                        case TestChannels.Right:
                                            Noise(out_r_ptr1, frameCount);
                                            break;
                                    }
                                    break;
                                case SignalSource.TRIANGLE:
                                    switch (ChannelTest)
                                    {
                                        case TestChannels.Both:
                                            Triangle(out_l_ptr1, frameCount, sine_freq1);
                                            CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                                            break;
                                        case TestChannels.Left:
                                            Triangle(out_l_ptr1, frameCount, sine_freq1);
                                            break;
                                        case TestChannels.Right:
                                            Triangle(out_r_ptr1, frameCount, sine_freq1);
                                            break;
                                    }
                                    break;
                                case SignalSource.SAWTOOTH:
                                    switch (ChannelTest)
                                    {
                                        case TestChannels.Both:
                                            Sawtooth(out_l_ptr1, frameCount, sine_freq1);
                                            CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                                            break;
                                        case TestChannels.Left:
                                            Sawtooth(out_l_ptr1, frameCount, sine_freq1);
                                            break;
                                        case TestChannels.Right:
                                            Sawtooth(out_r_ptr1, frameCount, sine_freq1);
                                            break;
                                    }
                                    break;
                            }

                            #endregion

                            break;
                        case AudioState.CW:
                            if (next_audio_state1 == AudioState.SWITCH)
                            {
                                DttSP.ExchangeOutputSamples(thread_no, out_l_ptr1, out_r_ptr1, frameCount);

                                if (switch_count == 0) next_audio_state1 = AudioState.CW;
                                switch_count--;
                            }

                            DttSP.CWtoneExchange(out_r, out_l, frameCount);

                            break;
                        case AudioState.SINL_COSR:
                            if (two_tone)
                            {
                                double dump;

                                SineWave2Tone(out_l_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out dump, out dump);

                                CosineWave2Tone(out_r_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out phase_accumulator1, out phase_accumulator2);
                            }
                            else
                            {
                                SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                phase_accumulator1 = CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            }
                            break;
                        case AudioState.SINL_SINR:
                            if (two_tone)
                            {
                                SineWave2Tone(out_l_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out phase_accumulator1, out phase_accumulator2);

                                CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                            }
                            else
                            {
                                phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                            }
                            break;
                        case AudioState.SINL_NOR:
                            if (two_tone)
                            {
                                SineWave2Tone(out_l_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out phase_accumulator1, out phase_accumulator2);
                                ClearBuffer(out_r_ptr1, frameCount);
                            }
                            else
                            {
                                phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                ClearBuffer(out_r_ptr1, frameCount);
                            }
                            break;
                        case AudioState.CW_COSL_SINR:
                            if (two_tone)
                            {
                                double dump;

                                CosineWave2Tone(out_r_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out dump, out dump);

                                SineWave2Tone(out_l_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out phase_accumulator1, out phase_accumulator2);
                            }
                            else
                            {
                                CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            }
                            break;
                        case AudioState.COSL_SINR:
                            if (two_tone)
                            {
                                double dump;

                                CosineWave2Tone(out_l_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out dump, out dump);

                                SineWave2Tone(out_r_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out phase_accumulator1, out phase_accumulator2);
                            }
                            else
                            {
                                CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                phase_accumulator1 = SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            }
                            break;
                        case AudioState.NOL_SINR:
                            if (two_tone)
                            {
                                ClearBuffer(out_l_ptr1, frameCount);
                                SineWave2Tone(out_r_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out phase_accumulator1, out phase_accumulator2);
                            }
                            else
                            {
                                ClearBuffer(out_l_ptr1, frameCount);
                                phase_accumulator1 = SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            }
                            break;
                        case AudioState.NOL_NOR:
                            ClearBuffer(out_l_ptr1, frameCount);
                            ClearBuffer(out_r_ptr1, frameCount);
                            break;
                        case AudioState.SWITCH:
                            if (!ramp_down && !ramp_up)
                            {
                                ClearBuffer(out_l_ptr1, frameCount);
                                ClearBuffer(out_r_ptr1, frameCount);
                                if (mox != next_mox) mox = next_mox;
                            }

                            DttSP_mutex.WaitOne();
                            DttSP.ExchangeOutputSamples(thread_no, out_l_ptr1, out_r_ptr1, frameCount);
                            DttSP_mutex.ReleaseMutex();

                            if (ramp_down)
                            {
                                int i;
                                for (i = 0; i < frameCount; i++)
                                {
                                    float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                    out_l_ptr1[i] *= w;
                                    out_r_ptr1[i] *= w;
                                    ramp_val += ramp_step;
                                    if (++ramp_count >= ramp_samples)
                                    {
                                        ramp_down = false;
                                        break;
                                    }
                                }

                                if (ramp_down)
                                {
                                    for (; i < frameCount; i++)
                                    {
                                        out_l_ptr1[i] = 0.0f;
                                        out_r_ptr1[i] = 0.0f;
                                    }
                                }
                            }
                            else if (ramp_up)
                            {
                                for (int i = 0; i < frameCount; i++)
                                {
                                    float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                    out_l_ptr1[i] *= w;
                                    out_r_ptr1[i] *= w;
                                    ramp_val += ramp_step;
                                    if (++ramp_count >= ramp_samples)
                                    {
                                        ramp_up = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ClearBuffer(out_l_ptr1, frameCount);
                                ClearBuffer(out_r_ptr1, frameCount);
                            }

                            if (next_audio_state1 == AudioState.CW)
                            {
                                //cw_delay = 1;
                                DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
                            }
                            else if (switch_count == 1)
                                DttSP.CWRingRestart();

                            switch_count--;
                            if (switch_count == ramp_up_num) RampUp = true;
                            if (switch_count == 0)
                                current_audio_state1 = next_audio_state1;
                            break;
                    }

                    if ((!mox || (mox && (!mon || !vac_mon))) && (vac_enabled && !loopDLL_enabled &&
                        rb_vacIN_l != null && rb_vacIN_r != null && rb_vacOUT_l != null && rb_vacOUT_r != null))
                    {
                        fixed (float* outl_ptr = &(vac_outl[0]))
                        fixed (float* outr_ptr = &(vac_outr[0]))
                        {
                            if (!mox)
                            {
                                ScaleBuffer(out_l, outl_ptr, frameCount, (float)vac_rx_scale);  // RX gain
                                ScaleBuffer(out_r, outr_ptr, frameCount, (float)vac_rx_scale);
                            }
                            else
                            {
                                ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)vac_preamp); // TX gain
                                ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)vac_preamp);
                            }

                            if (sample_rateVAC == sample_rate1)
                            {
                                if ((rb_vacOUT_l.WriteSpace() >= frameCount) && (rb_vacOUT_r.WriteSpace() >= frameCount))
                                {
                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacOUT_l.WritePtr(out_l_ptr1, frameCount);
                                    rb_vacOUT_r.WritePtr(out_r_ptr1, frameCount);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                                else
                                {
                                    VACDebug("rb_vacOUT_l overflow");
                                    VACDebug("rb_vacOUT_r overflow");
                                }
                            }
                            else
                            {
                                if (vac_stereo)
                                {
                                    fixed (float* res_outl_ptr = &(res_outl[0]))
                                    fixed (float* res_outr_ptr = &(res_outr[0]))
                                    {
                                        int outsamps;
                                        DttSP.DoResamplerF(out_l_ptr1, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
                                        DttSP.DoResamplerF(out_r_ptr1, res_outr_ptr, frameCount, &outsamps, resampPtrOut_r);
                                        if ((rb_vacOUT_l.WriteSpace() >= outsamps) && (rb_vacOUT_r.WriteSpace() >= outsamps))
                                        {
                                            Win32.EnterCriticalSection(cs_vac);
                                            rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
                                            rb_vacOUT_r.WritePtr(res_outr_ptr, outsamps);
                                            Win32.LeaveCriticalSection(cs_vac);
                                        }
                                        else
                                        {
                                            VACDebug("rb_vacOUT_l overflow");
                                            VACDebug("rb_vacOUT_r overflow");
                                        }
                                    }
                                }
                                else
                                {
                                    fixed (float* res_outl_ptr = &(res_outl[0]))
                                    {
                                        int outsamps;
                                        DttSP.DoResamplerF(out_l_ptr1, res_outl_ptr, frameCount, &outsamps, resampPtrOut_l);
                                        if ((rb_vacOUT_l.WriteSpace() >= outsamps) && (rb_vacOUT_r.WriteSpace() >= outsamps))
                                        {
                                            Win32.EnterCriticalSection(cs_vac);
                                            rb_vacOUT_l.WritePtr(res_outl_ptr, outsamps);
                                            rb_vacOUT_r.WritePtr(res_outl_ptr, outsamps);
                                            Win32.LeaveCriticalSection(cs_vac);
                                        }
                                        else
                                        {
                                            VACDebug("rb_vacOUT_l overflow");
                                            VACDebug("rb_vacOUT_r overflow");
                                        }
                                    }
                                }
                            }
                        }
                    }

                    double vol = monitor_volume;
                    if (mox)
                    {
                        vol = TXScale;

                        if (high_pwr_am)
                        {
                            if (dsp_mode == DSPMode.AM ||
                                dsp_mode == DSPMode.SAM)
                                vol *= 1.414;
                        }
                    }

                    if ((wave_record && !mox && !record_rx_preprocessed) ||     // post process audio
                        (wave_record && mox && !record_tx_preprocessed))
                        wave_file_writer.AddWriteBuffer(out_r_ptr1, out_l_ptr1);

                    if (VACPrimaryAudiodevice && !mox && current_audio_state1 == AudioState.DTTSP &&
                        !loopDLL_enabled)
                    {
                        ClearBuffer(out_l_ptr1, frameCount);
                        ClearBuffer(out_r_ptr1, frameCount);
                    }

                    if (loopDLL_enabled && vac_enabled && !mox)
                    {

                        int i;
                        double[] buffer = new double[frameCount];

                        fixed (double* buffer_ptr = &(buffer[0]))
                        fixed (float* res_outl_ptr = &(res_outl[0]))
                        {
                            ScaleBuffer(out_l, res_outl_ptr, frameCount, (float)vol);
                            ScaleBuffer(out_l, out_l, frameCount, 0.0f);
                            ScaleBuffer(out_r, out_r, frameCount, 0.0f);

                            {
                                for (i = 0; i < frameCount; i++)
                                {
                                    buffer[i] = (double)1e5 * res_outl[i];
                                }

                                console.loopDLL.WriteRXBuffer(buffer_ptr);
                            }
                        }
                    }
                    else
                    {
                        ScaleBuffer(out_l, out_l, frameCount, (float)vol);
                        ScaleBuffer(out_r, out_r, frameCount, (float)vol);
                    }
                }

                return callback_return;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return 0;
            }
        }

        unsafe public static int input_Callback4Port(void* input, void* output, int frameCount, // yt7pwr
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            try
            {
                if (mox || (vox_enabled && !vac_enabled))
                {
#if(WIN64)
                    Int64* array_ptr = (Int64*)input;
                    float* in_l_ptr1 = (float*)array_ptr[0];
                    double* VAC_in = (double*)input;
#endif

#if(WIN32)
                    int* array_ptr = (int*)input;
                    float* in_l_ptr1 = (float*)array_ptr[0];
                    double* VAC_in = (double*)input;
#endif

                    float* in_l = null, in_l_VAC = null, in_r = null, in_r_VAC = null;

                    if (!mox && !voice_message_record)   // rx
                    {
                        in_l = in_l_ptr1;
                        in_r = in_l_ptr1;
                    }
                    else if (mox && !voice_message_record)
                    {       // tx
                        if (voice_message_playback)
                            voice_msg_file_reader.GetPlayBuffer(in_l_ptr1, in_l_ptr1);

                        in_r = in_l_ptr1;
                        in_l = in_l_ptr1;
                    }
                    else if (voice_message_record)
                    {
                        in_l = in_l_ptr1;
                        in_r = in_l_ptr1;
                    }

                    if (voice_message_record)
                        voice_msg_file_writer.AddWriteBuffer(in_l, in_r);

                    if (loopDLL_enabled)
                    {
                        int i;

                        in_l_VAC = in_l;    // save pointer
                        in_r_VAC = in_r;    // save pointer

                        fixed (double* loopDLL_inl_ptr = &(loopDLL_inl[0]))
                        {
                            if (console.loopDLL.ReadTXBuffer(loopDLL_inl_ptr))
                            {
                                for (i = 0; i < frameCount; i++)
                                {
                                    in_l[0] = (float)(loopDLL_inl[i] / 1e4);
                                    in_r[0] = (float)(loopDLL_inl[i] / 1e4);
                                    in_l++;
                                    in_r++;
                                }
                            }
                            else
                            {

                            }
                        }

                        in_l = in_l_VAC;    // restore pointer
                        in_r = in_r_VAC;    // restore pointer
                    }

                    switch (current_audio_state1)
                    {
                        case AudioState.DTTSP:

                            #region VOX

                            float* vox_l = null, vox_r = null;
                            vox_l = in_l_ptr1;
                            vox_r = in_l_ptr1;
                            if (vox_enabled && !vac_enabled)
                            {
                                if (dsp_mode == DSPMode.LSB ||
                                    dsp_mode == DSPMode.USB ||
                                    dsp_mode == DSPMode.DSB ||
                                    dsp_mode == DSPMode.AM ||
                                    dsp_mode == DSPMode.SAM ||
                                    dsp_mode == DSPMode.FMN)
                                {
                                    ScaleBuffer(vox_l, vox_l, frameCount, (float)mic_preamp);
                                    ScaleBuffer(vox_r, vox_r, frameCount, (float)mic_preamp);
                                    Peak = MaxSample(vox_l, vox_r, frameCount);

                                    // compare power to threshold
                                    if (Peak > vox_threshold)
                                        vox_active = true;
                                    else
                                        vox_active = false;
                                }
                            }

                            #endregion

                            else
                            {
                                // scale input with mic preamp
                                if (dsp_mode == DSPMode.LSB ||
                                    dsp_mode == DSPMode.USB ||
                                    dsp_mode == DSPMode.DSB ||
                                    dsp_mode == DSPMode.AM ||
                                    dsp_mode == DSPMode.SAM ||
                                    dsp_mode == DSPMode.FMN)
                                {
                                    ScaleBuffer(in_l, in_l, frameCount, (float)mic_preamp);
                                    ScaleBuffer(in_r, in_r, frameCount, (float)mic_preamp);
                                }
                                else
                                {
                                    ScaleBuffer(in_l, in_l, frameCount, 0.0f);
                                    ScaleBuffer(in_r, in_r, frameCount, 0.0f);
                                }
                            }

                            #region Input Signal Source

                            switch (current_input_signal)
                            {
                                case SignalSource.SOUNDCARD:
                                    break;
                                case SignalSource.SINE:
                                    SineWave(in_l, frameCount, phase_accumulator1, sine_freq1);
                                    phase_accumulator1 = CosineWave(in_r, frameCount, phase_accumulator1, sine_freq1);
                                    ScaleBuffer(in_l, in_l, frameCount, (float)input_source_scale);
                                    ScaleBuffer(in_r, in_r, frameCount, (float)input_source_scale);
                                    break;
                                case SignalSource.NOISE:
                                    Noise(in_l, frameCount);
                                    Noise(in_r, frameCount);
                                    break;
                                case SignalSource.TRIANGLE:
                                    Triangle(in_l, frameCount, sine_freq1);
                                    CopyBuffer(in_l, in_r, frameCount);
                                    break;
                                case SignalSource.SAWTOOTH:
                                    Sawtooth(in_l, frameCount, sine_freq1);
                                    CopyBuffer(in_l, in_r, frameCount);
                                    break;
                            }

                            #endregion

                            if (mox)
                                DttSP.ExchangeInputSamples(thread_no, in_l, in_r, frameCount);

                            break;
                        case AudioState.CW:
                            if (next_audio_state1 == AudioState.SWITCH)
                            {
                                if (mox)
                                    DttSP.ExchangeInputSamples(thread_no, in_l_ptr1, in_l_ptr1, frameCount);

                                if (switch_count == 0) next_audio_state1 = AudioState.CW;
                                switch_count--;
                            }
                            break;
                        case AudioState.SWITCH:
                            if (!ramp_down && !ramp_up)
                            {
                                ClearBuffer(in_l_ptr1, frameCount);
                                ClearBuffer(in_l_ptr1, frameCount);
                                if (mox != next_mox) mox = next_mox;
                            }

                            if (mox)
                                DttSP.ExchangeInputSamples(thread_no, in_l_ptr1, in_l_ptr1, frameCount);

                            if (ramp_down)
                            {
                                int i;
                                for (i = 0; i < frameCount; i++)
                                {
                                    float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                    in_l[i] *= w;
                                    in_r[i] *= w;
                                    ramp_val += ramp_step;
                                    if (++ramp_count >= ramp_samples)
                                    {
                                        ramp_down = false;
                                        break;
                                    }
                                }

                                if (ramp_down)
                                {
                                    for (; i < frameCount; i++)
                                    {
                                        in_l[i] = 0.0f;
                                        in_r[i] = 0.0f;
                                    }
                                }
                            }
                            else if (ramp_up)
                            {
                                for (int i = 0; i < frameCount; i++)
                                {
                                    float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                    in_l[i] *= w;
                                    in_r[i] *= w;
                                    ramp_val += ramp_step;
                                    if (++ramp_count >= ramp_samples)
                                    {
                                        ramp_up = false;
                                        break;
                                    }
                                }
                            }

                            switch_count--;
                            if (switch_count == ramp_up_num) RampUp = true;
                            if (switch_count == 0)
                                current_audio_state1 = next_audio_state1;
                            break;
                    }
                }

                return callback_return;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return 0;
            }
        }

        unsafe public static int output_Callback4Port(void* input, void* output, int frameCount,// yt7pwr
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            try
            {
#if(WIN64)
                Int64* array_ptr = (Int64*)output;
                float* out_l_ptr1 = (float*)array_ptr[0];
                float* out_r_ptr1 = (float*)array_ptr[1];
                float* in_l_ptr1 = null;
                float* in_r_ptr1 = null;
#endif

#if(WIN32)
                int* array_ptr = (int*)output;
                float* out_l_ptr1 = (float*)array_ptr[0];
                float* out_r_ptr1 = (float*)array_ptr[1];
                float* in_l_ptr1 = null;
                float* in_r_ptr1 = null;
#endif

                if (mox && !TX_out_1_2)
                {
                    float* in_l_VAC = null, in_r_VAC = null, out_l = null, out_r = null;

                    if (!voice_message_record)
                    {
                        if (voice_message_playback)
                            voice_msg_file_reader.GetPlayBuffer(in_l_ptr1, in_r_ptr1);

                        if (!console.TX_IQ_channel_swap)
                        {
                            out_r = out_l_ptr1;
                            out_l = out_r_ptr1;
                        }
                        else
                        {
                            out_l = out_l_ptr1;
                            out_r = out_r_ptr1;
                        }
                    }
                    else if (voice_message_record)
                    {
                        out_l = out_l_ptr1;
                        out_r = out_r_ptr1;
                    }

                    switch (current_audio_state1)
                    {
                        case AudioState.DTTSP:
                            if (dsp_mode == DSPMode.CWU || dsp_mode == DSPMode.CWL)
                            {
                                DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
                            }

                            DttSP.ExchangeOutputSamples(thread_no, out_l, out_r, frameCount);

                            #region Output Signal Source

                            switch (current_output_signal)
                            {
                                case SignalSource.SOUNDCARD:
                                    break;
                                case SignalSource.SINE:
                                    SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                    phase_accumulator1 = CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                    break;
                                case SignalSource.NOISE:
                                    Noise(out_l_ptr1, frameCount);
                                    Noise(out_r_ptr1, frameCount);
                                    break;
                                case SignalSource.TRIANGLE:
                                    Triangle(out_l_ptr1, frameCount, sine_freq1);
                                    CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                                    break;
                                case SignalSource.SAWTOOTH:
                                    Sawtooth(out_l_ptr1, frameCount, sine_freq1);
                                    CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                                    break;
                            }

                            #endregion

                            break;
                        case AudioState.CW:
                            if (next_audio_state1 == AudioState.SWITCH)
                            {
                                DttSP.ExchangeOutputSamples(thread_no, out_l_ptr1, out_r_ptr1, frameCount);

                                if (switch_count == 0) next_audio_state1 = AudioState.CW;
                                switch_count--;
                            }

                            DttSP.CWtoneExchange(out_r, out_l, frameCount);

                            break;
                        case AudioState.SINL_COSR:
                            if (two_tone)
                            {
                                double dump;

                                SineWave2Tone(out_l_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out dump, out dump);

                                CosineWave2Tone(out_r_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out phase_accumulator1, out phase_accumulator2);
                            }
                            else
                            {
                                SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                phase_accumulator1 = CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            }
                            break;
                        case AudioState.SINL_SINR:
                            if (two_tone)
                            {
                                SineWave2Tone(out_l_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out phase_accumulator1, out phase_accumulator2);

                                CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                            }
                            else
                            {
                                phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                CopyBuffer(out_l_ptr1, out_r_ptr1, frameCount);
                            }
                            break;
                        case AudioState.SINL_NOR:
                            if (two_tone)
                            {
                                SineWave2Tone(out_l_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out phase_accumulator1, out phase_accumulator2);
                                ClearBuffer(out_r_ptr1, frameCount);
                            }
                            else
                            {
                                phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                ClearBuffer(out_r_ptr1, frameCount);
                            }
                            break;
                        case AudioState.CW_COSL_SINR:
                            if (two_tone)
                            {
                                double dump;

                                CosineWave2Tone(out_r_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out dump, out dump);

                                SineWave2Tone(out_l_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out phase_accumulator1, out phase_accumulator2);
                            }
                            else
                            {
                                CosineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                phase_accumulator1 = SineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            }
                            break;
                        case AudioState.COSL_SINR:
                            if (two_tone)
                            {
                                double dump;

                                CosineWave2Tone(out_l_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out dump, out dump);

                                SineWave2Tone(out_r_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out phase_accumulator1, out phase_accumulator2);
                            }
                            else
                            {
                                CosineWave(out_l_ptr1, frameCount, phase_accumulator1, sine_freq1);
                                phase_accumulator1 = SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            }
                            break;
                        case AudioState.NOL_SINR:
                            if (two_tone)
                            {
                                ClearBuffer(out_l_ptr1, frameCount);
                                SineWave2Tone(out_r_ptr1, frameCount,
                                    phase_accumulator1, phase_accumulator2,
                                    sine_freq1, sine_freq2,
                                    out phase_accumulator1, out phase_accumulator2);
                            }
                            else
                            {
                                ClearBuffer(out_l_ptr1, frameCount);
                                phase_accumulator1 = SineWave(out_r_ptr1, frameCount, phase_accumulator1, sine_freq1);
                            }
                            break;
                        case AudioState.NOL_NOR:
                            ClearBuffer(out_l_ptr1, frameCount);
                            ClearBuffer(out_r_ptr1, frameCount);
                            break;
                        case AudioState.PIPE:
                            CopyBuffer(in_l_ptr1, out_l_ptr1, frameCount);
                            CopyBuffer(in_r_ptr1, out_r_ptr1, frameCount);
                            break;
                        case AudioState.SWITCH:
                            if (!ramp_down && !ramp_up)
                            {
                                ClearBuffer(out_l_ptr1, frameCount);
                                ClearBuffer(out_r_ptr1, frameCount);
                                if (mox != next_mox) mox = next_mox;
                            }

                            DttSP.ExchangeOutputSamples(thread_no, out_l_ptr1, out_r_ptr1, frameCount);

                            if (ramp_down)
                            {
                                int i;
                                for (i = 0; i < frameCount; i++)
                                {
                                    float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                    out_l_ptr1[i] *= w;
                                    out_r_ptr1[i] *= w;
                                    ramp_val += ramp_step;
                                    if (++ramp_count >= ramp_samples)
                                    {
                                        ramp_down = false;
                                        break;
                                    }
                                }

                                if (ramp_down)
                                {
                                    for (; i < frameCount; i++)
                                    {
                                        out_l_ptr1[i] = 0.0f;
                                        out_r_ptr1[i] = 0.0f;
                                    }
                                }
                            }
                            else if (ramp_up)
                            {
                                for (int i = 0; i < frameCount; i++)
                                {
                                    float w = (float)Math.Sin(ramp_val * Math.PI / 2.0);
                                    out_l_ptr1[i] *= w;
                                    out_r_ptr1[i] *= w;
                                    ramp_val += ramp_step;
                                    if (++ramp_count >= ramp_samples)
                                    {
                                        ramp_up = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ClearBuffer(out_l_ptr1, frameCount);
                                ClearBuffer(out_r_ptr1, frameCount);
                            }

                            if (next_audio_state1 == AudioState.CW)
                            {
                                //cw_delay = 1;
                                DttSP.CWtoneExchange(out_l_ptr1, out_r_ptr1, frameCount);
                            }
                            else if (switch_count == 1)
                                DttSP.CWRingRestart();

                            switch_count--;
                            if (switch_count == ramp_up_num) RampUp = true;
                            if (switch_count == 0)
                                current_audio_state1 = next_audio_state1;
                            break;
                    }

                    if ((wave_record && !mox && !record_rx_preprocessed) ||
                        (wave_record && mox && !record_tx_preprocessed))
                        wave_file_writer.AddWriteBuffer(out_l_ptr1, out_r_ptr1);

                    double vol = monitor_volume;
                    if (mox)
                    {
                        vol = TXScale;

                        if (high_pwr_am)
                        {
                            if (dsp_mode == DSPMode.AM ||
                                dsp_mode == DSPMode.SAM)
                                vol *= 1.414;
                        }
                    }
                    ScaleBuffer(out_l, out_l, frameCount, (float)vol);
                    ScaleBuffer(out_r, out_r, frameCount, (float)vol);

                }
                else
                {
                    if ((wave_record && !mox && !record_rx_preprocessed) ||
                        (wave_record && mox && !record_tx_preprocessed))
                        wave_file_writer.AddWriteBuffer(out_l_ptr1, out_r_ptr1);

                    if (mon)
                    {
                        DttSP.CWMonitorExchange(out_l_ptr1, out_r_ptr1, frameCount);
                        ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)monitor_volume);
                        ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)monitor_volume);
                    }
                    else
                    {
                        ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, 0.0f);
                        ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, 0.0f);
                    }
                }

                return callback_return;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return 0;
            }
        }

		unsafe public static int Pipe(void* input, void* output, int frameCount,
			PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void *userData)
		{
			float *inptr = (float *)input;
			float *outptr = (float *)output;

			for(int i=0; i<frameCount; i++)
			{
				*outptr++ = *inptr++;
				*outptr++ = *inptr++;
			}
			return 0;
        }

        unsafe public static int input_CallbackVAC(void* input, void* output, int frameCount,   // yt7pwr
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            try
            {
#if(WIN64)
                Int64* array_ptr = (Int64*)input;
                float* in_l_ptr1 = (float*)array_ptr[0];
                float* in_r_ptr1 = null;
                if (vac_stereo) in_r_ptr1 = (float*)array_ptr[1];
#endif

#if(WIN32)
                int* array_ptr = (int*)input;
                float* in_l_ptr1 = (float*)array_ptr[0];
                float* in_r_ptr1 = null;
                if (vac_stereo) in_r_ptr1 = (float*)array_ptr[1];
#endif

                if (vac_rb_reset)
                {
                    vac_rb_reset = false;
                    Win32.EnterCriticalSection(cs_vac);
                    rb_vacIN_l.Reset();
                    rb_vacIN_r.Reset();
                    rb_vacOUT_l.Reset();
                    rb_vacOUT_r.Reset();
                    Win32.LeaveCriticalSection(cs_vac);
                    return 0;
                }

                //[patch_7
                #region VOX
                if (vox_enabled)
                {
                    float* vox_l = null, vox_r = null;
                    vox_l = in_l_ptr1;
                    vox_r = in_r_ptr1;

                    if (dsp_mode == DSPMode.LSB ||
                        dsp_mode == DSPMode.USB ||
                        dsp_mode == DSPMode.DSB ||
                        dsp_mode == DSPMode.AM ||
                        dsp_mode == DSPMode.SAM ||
                        dsp_mode == DSPMode.FMN)
                    {
                        Peak = MaxSample(vox_l, (vac_stereo) ? vox_r : vox_l, frameCount);

                        // compare power to threshold
                        if (Peak > vox_threshold)
                            vox_active = true;
                        else
                            vox_active = false;
                    }
                }
                #endregion
                //patch_7]

                if (vac_stereo)
                {
                    if (mox || vox_enabled)
                    {
                        if (vac_resample)
                        {
                            int outsamps;
                            fixed (float* res_inl_ptr = &(res_inl[0]))
                            fixed (float* res_inr_ptr = &(res_inr[0]))
                            {
                                DttSP.DoResamplerF(in_l_ptr1, res_inl_ptr, frameCount, &outsamps, resampPtrIn_l);
                                DttSP.DoResamplerF(in_r_ptr1, res_inr_ptr, frameCount, &outsamps, resampPtrIn_r);
                                if ((rb_vacIN_l.WriteSpace() >= outsamps) && (rb_vacIN_r.WriteSpace() >= outsamps))
                                {
                                    Win32.EnterCriticalSection(cs_vac);
                                    rb_vacIN_l.WritePtr(res_inl_ptr, outsamps);
                                    rb_vacIN_r.WritePtr(res_inr_ptr, outsamps);
                                    Win32.LeaveCriticalSection(cs_vac);
                                }
                                else
                                {
                                    vac_rb_reset = true;
                                    VACDebug("rb_vacIN overflow stereo CBvac");
                                }
                            }
                        }
                        else
                        {
                            if ((rb_vacIN_l.WriteSpace() >= frameCount) && (rb_vacIN_r.WriteSpace() >= frameCount))
                            {
                                Win32.EnterCriticalSection(cs_vac);
                                rb_vacIN_l.WritePtr(in_l_ptr1, frameCount);
                                rb_vacIN_r.WritePtr(in_r_ptr1, frameCount);
                                Win32.LeaveCriticalSection(cs_vac);
                            }
                            else
                            {
                                //vac_rb_reset = true;
                                VACDebug("rb_vacIN overflow mono CBvac");
                            }
                        }
                    }
                }
                else
                {
                    if (vac_resample)
                    {
                        int outsamps;
                        fixed (float* res_inl_ptr = &(res_inl[0]))
                        {
                            DttSP.DoResamplerF(in_l_ptr1, res_inl_ptr, frameCount, &outsamps, resampPtrIn_l);
                            if ((rb_vacIN_l.WriteSpace() >= outsamps) && (rb_vacIN_r.WriteSpace() >= outsamps))
                            {
                                Win32.EnterCriticalSection(cs_vac);
                                rb_vacIN_l.WritePtr(res_inl_ptr, outsamps);
                                rb_vacIN_r.WritePtr(res_inl_ptr, outsamps);
                                Win32.LeaveCriticalSection(cs_vac);
                            }
                            else
                            {
                                //vac_rb_reset = true;
                                VACDebug("rb_vacIN_l overflow");
                            }
                        }
                    }
                    else
                    {
                        if ((rb_vacIN_l.WriteSpace() >= frameCount) && (rb_vacIN_r.WriteSpace() >= frameCount))
                        {
                            Win32.EnterCriticalSection(cs_vac);
                            rb_vacIN_l.WritePtr(in_l_ptr1, frameCount);
                            rb_vacIN_r.WritePtr(in_l_ptr1, frameCount);
                            Win32.LeaveCriticalSection(cs_vac);
                        }
                        else
                        {
                            //vac_rb_reset = true;
                            VACDebug("rb_vacIN_l overflow");
                        }
                    }
                }

                return 0;
            }


            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return 0;
            }
        }

        unsafe public static int output_CallbackVAC(void* input, void* output, int frameCount,  // yt7pwr
            PA19.PaStreamCallbackTimeInfo* timeInfo, int statusFlags, void* userData)
        {
            try
            {
#if(WIN64)
                Int64* array_ptr = (Int64*)output;
                float* out_l_ptr1 = (float*)array_ptr[0];
                float* out_r_ptr1 = null;
                if (vac_stereo) out_r_ptr1 = (float*)array_ptr[1];
#endif

#if(WIN32)
                int* array_ptr = (int*)output;
                float* out_l_ptr1 = (float*)array_ptr[0];
                float* out_r_ptr1 = null;
                if (vac_stereo) out_r_ptr1 = (float*)array_ptr[1];
#endif

                if (vac_rb_reset)
                {
                    vac_rb_reset = false;
                    ClearBuffer(out_l_ptr1, frameCount);
                    if (vac_stereo) ClearBuffer(out_r_ptr1, frameCount);
                    Win32.EnterCriticalSection(cs_vac);
                    rb_vacIN_l.Reset();
                    rb_vacIN_r.Reset();
                    rb_vacOUT_l.Reset();
                    rb_vacOUT_r.Reset();
                    Win32.LeaveCriticalSection(cs_vac);
                    return 0;
                }

                double vol = monitor_volume;

                if (mox && mon && vac_mon && (dsp_mode == DSPMode.CWU || dsp_mode == DSPMode.CWL))
                {
                    if (vac_stereo)
                    {
                        DttSP.CWMonitorExchange(out_l_ptr1, out_r_ptr1, frameCount);
                        for (int i = 0; i < frameCount; i++)
                        {
                            out_l_ptr1[i] = out_l_ptr1[i] * (float)monitor_volume;
                            out_r_ptr1[i] = out_r_ptr1[i] * (float)monitor_volume;
                        }
                    }
                    else
                    {
                        DttSP.CWMonitorExchange(out_l_ptr1, out_l_ptr1, frameCount);
                        for (int i = 0; i < frameCount; i++)
                        {
                            out_l_ptr1[i] = out_l_ptr1[i] * (float)monitor_volume;
                        }
                    }
                }
                else if (!mox)
                {
                    if (vac_stereo)
                    {
                        if ((rb_vacOUT_l.ReadSpace() >= frameCount) && (rb_vacOUT_r.ReadSpace() >= frameCount))
                        {
                            Win32.EnterCriticalSection(cs_vac);
                            rb_vacOUT_l.ReadPtr(out_l_ptr1, frameCount);
                            rb_vacOUT_r.ReadPtr(out_r_ptr1, frameCount);
                            Win32.LeaveCriticalSection(cs_vac);
                        }
                        else
                        {
                            ClearBuffer(out_l_ptr1, frameCount);
                            ClearBuffer(out_r_ptr1, frameCount);
                            VACDebug("rb_vacOUT underflow");
                        }
                    }
                    else
                    {
                        if ((rb_vacOUT_l.ReadSpace() >= frameCount) && (rb_vacOUT_r.ReadSpace() >= frameCount))     // RX
                        {
                            Win32.EnterCriticalSection(cs_vac);
                            rb_vacOUT_l.ReadPtr(out_l_ptr1, frameCount);
                            rb_vacOUT_r.ReadPtr(out_l_ptr1, frameCount);
                            Win32.LeaveCriticalSection(cs_vac);
                        }
                        else
                        {
                            ClearBuffer(out_l_ptr1, frameCount);
                            VACDebug("rb_vacOUT_l underflow");
                        }
                    }

                    ScaleBuffer(out_l_ptr1, out_l_ptr1, frameCount, (float)vol);
                    if (vac_stereo)
                        ScaleBuffer(out_r_ptr1, out_r_ptr1, frameCount, (float)vol);
                }

                if (VACPrimaryAudiodevice && mox && (!vac_mon || !mon))   // TX
                {
                    ClearBuffer(out_l_ptr1, frameCount);
                    if (vac_stereo) ClearBuffer(out_r_ptr1, frameCount);
                }

                return 0;
            }


            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return 0;
            }
        }

        #endregion

        #endregion

        #region Buffer Operations

        unsafe private static void ClearBuffer(float *buf, int samples)
		{
			Win32.memset(buf, 0, samples*sizeof(float));
		}

		unsafe private static void CopyBuffer(float *inbuf, float *outbuf, int samples)
		{
			Win32.memcpy(outbuf, inbuf, samples*sizeof(float));
		}

		unsafe private static void ScaleBuffer(float *inbuf, float *outbuf, int samples, float scale)
		{
			for(int i=0; i<samples; i++)
				outbuf[i] = inbuf[i] * scale;
		}

		unsafe public static float MaxSample(float* buf, int samples)
		{
			float max = float.MinValue;
			for(int i=0; i<samples; i++)
				if(buf[i] > max) max = buf[i];
				
			return max;
		}

		unsafe public static float MaxSample(float* buf1, float* buf2, int samples)
		{
			float max = float.MinValue;
			for(int i=0; i<samples; i++)
			{
				if(buf1[i] > max) max = buf1[i];
				if(buf2[i] > max) max = buf2[i];
			}
			return max;
		}

		unsafe public static float MinSample(float* buf, int samples)
		{
			float min = float.MaxValue;
			for(int i=0; i<samples; i++)
				if(buf[i] < min) min = buf[i];
				
			return min;
		}

		unsafe public static float MinSample(float* buf1, float* buf2, int samples)
		{
			float min = float.MaxValue;
			for(int i=0; i<samples; i++)
			{
				if(buf1[i] < min) min = buf1[i];
				if(buf2[i] < min) min = buf2[i];
			}
				
			return min;
		}

		// returns updated phase accumulator
		unsafe public static double SineWave(float* buf, int samples, double phase, double freq)
		{
			double phase_step = freq/sample_rate1*2*Math.PI;
			double cosval = Math.Cos(phase);
			double sinval = Math.Sin(phase);
			double cosdelta = Math.Cos(phase_step);
			double sindelta = Math.Sin(phase_step);
			double tmpval;

			for(int i=0; i<samples; i++ )
			{
				tmpval = cosval*cosdelta - sinval*sindelta;
				sinval = cosval*sindelta + sinval*cosdelta;
				cosval = tmpval;
						
				buf[i] = (float)(sinval);
											 
				phase += phase_step;
			}

			return phase;
		}

		// returns updated phase accumulator
		unsafe public static double CosineWave(float* buf, int samples, double phase, double freq)
		{
			double phase_step = freq/sample_rate1*2*Math.PI;
			double cosval = Math.Cos(phase);
			double sinval = Math.Sin(phase);
			double cosdelta = Math.Cos(phase_step);
			double sindelta = Math.Sin(phase_step);
			double tmpval;

			for(int i=0; i<samples; i++ )
			{
				tmpval = cosval*cosdelta - sinval*sindelta;
				sinval = cosval*sindelta + sinval*cosdelta;
				cosval = tmpval;
						
				buf[i] = (float)(cosval);
											 
				phase += phase_step;
			}

			return phase;
		}

		unsafe public static void SineWave2Tone(float* buf, int samples, 
			double phase1, double phase2, 
			double freq1, double freq2,
			out double updated_phase1, out double updated_phase2)
		{
			double phase_step1 = freq1/sample_rate1*2*Math.PI;
			double cosval1 = Math.Cos(phase1);
			double sinval1 = Math.Sin(phase1);
			double cosdelta1 = Math.Cos(phase_step1);
			double sindelta1 = Math.Sin(phase_step1);

			double phase_step2 = freq2/sample_rate1*2*Math.PI;
			double cosval2 = Math.Cos(phase2);
			double sinval2 = Math.Sin(phase2);
			double cosdelta2 = Math.Cos(phase_step2);
			double sindelta2 = Math.Sin(phase_step2);
			double tmpval;

			for(int i=0; i<samples; i++ )
			{
				tmpval = cosval1*cosdelta1 - sinval1*sindelta1;
				sinval1 = cosval1*sindelta1 + sinval1*cosdelta1;
				cosval1 = tmpval;

				tmpval = cosval2*cosdelta2 - sinval2*sindelta2;
				sinval2 = cosval2*sindelta2 + sinval2*cosdelta2;
				cosval2 = tmpval;
						
				buf[i] = (float)(sinval1*0.5 + sinval2*0.5);
											 
				phase1 += phase_step1;
				phase2 += phase_step2;
			}

			updated_phase1 = phase1;
			updated_phase2 = phase2;
		}

		unsafe public static void CosineWave2Tone(float* buf, int samples, 
			double phase1, double phase2, 
			double freq1, double freq2,
			out double updated_phase1, out double updated_phase2)
		{
			double phase_step1 = freq1/sample_rate1*2*Math.PI;
			double cosval1 = Math.Cos(phase1);
			double sinval1 = Math.Sin(phase1);
			double cosdelta1 = Math.Cos(phase_step1);
			double sindelta1 = Math.Sin(phase_step1);

			double phase_step2 = freq2/sample_rate1*2*Math.PI;
			double cosval2 = Math.Cos(phase2);
			double sinval2 = Math.Sin(phase2);
			double cosdelta2 = Math.Cos(phase_step2);
			double sindelta2 = Math.Sin(phase_step2);
			double tmpval;

			for(int i=0; i<samples; i++ )
			{
				tmpval = cosval1*cosdelta1 - sinval1*sindelta1;
				sinval1 = cosval1*sindelta1 + sinval1*cosdelta1;
				cosval1 = tmpval;

				tmpval = cosval2*cosdelta2 - sinval2*sindelta2;
				sinval2 = cosval2*sindelta2 + sinval2*cosdelta2;
				cosval2 = tmpval;
						
				buf[i] = (float)(cosval1*0.5 + cosval2*0.5);
											 
				phase1 += phase_step1;
				phase2 += phase_step2;
			}

			updated_phase1 = phase1;
			updated_phase2 = phase2;
		}

		
		private static Random r = new Random();
		private static double y2=0.0;
		private static bool use_last = false;
		private static double boxmuller (double m,double s)
		{
			double x1,x2,w,y1;
			if (use_last)		        /* use value from previous call */
			{
				y1 = y2;
				use_last = false;
			}
			else
			{
				do 
				{
					x1 = (2.0 * r.NextDouble() - 1.0);
					x2 = (2.0 * r.NextDouble() - 1.0);
					w = x1 * x1 + x2 * x2;
				} while ( w >= 1.0 );

				w = Math.Sqrt( (-2.0 * Math.Log( w ) ) / w );
				y1 = x1 * w;
				y2 = x2 * w;
				use_last = true;
			}

			return( m + y1 * s );
		}
		unsafe public static void Noise(float* buf, int samples)
		{
			for(int i=0; i<samples; i++)
			{
				buf[i] = (float)boxmuller(0.0,0.2);
			}
		}

		private static double tri_val = 0.0;
		private static int tri_direction = 1;
		unsafe public static void Triangle(float* buf, int samples, double freq)
		{
			double step = freq/sample_rate1*2*tri_direction;
			for(int i=0; i<samples; i++)
			{
				buf[i] = (float)tri_val;
				tri_val += step;
				if(tri_val >= 1.0 || tri_val <= -1.0)
				{
					step = -step;
					tri_val += 2*step;
					if(step < 0) tri_direction = -1;
					else tri_direction = 1;
				}
			}
		}

		private static double saw_val = 0.0;
		private static int saw_direction = 1;
		unsafe public static void Sawtooth(float* buf, int samples, double freq)
		{
			double step = freq/sample_rate1*saw_direction;
			for(int i=0; i<samples; i++)
			{
				buf[i] = (float)saw_val;
				saw_val += step;
				if(saw_val >= 1.0) saw_val -= 2.0;
				if(saw_val <= -1.0) saw_val += 2.0;
			}
		}

		unsafe public static void AddConstant(float* buf, int samples, float val)
		{
			for(int i=0; i<samples; i++)
				buf[i] += val;
		}

		#endregion

		#region Misc Routines
		// ======================================================
		// Misc Routines
		// ======================================================

		private static void VACDebug(string s)
		{
#if(VAC_DEBUG)
			Debug.WriteLine(s);
#endif
		}

		unsafe private static void InitVAC()
		{
            if (rb_vacOUT_l == null) rb_vacOUT_l = new RingBufferFloat(2 * 65536);
			rb_vacOUT_l.Restart(block_size2);

            if (rb_vacOUT_r == null) rb_vacOUT_r = new RingBufferFloat(2 * 65536);
			rb_vacOUT_r.Restart(block_size2);

            if (rb_vacIN_l == null) rb_vacIN_l = new RingBufferFloat(4 * 65536);
			rb_vacIN_l.Restart(block_size2);

            if (rb_vacIN_r == null) rb_vacIN_r = new RingBufferFloat(4 * 65536);
			rb_vacIN_r.Restart(block_size2);

			if (sample_rateVAC != sample_rate1) 
			{
				vac_resample = true;
				if(res_outl == null) res_outl = new float [65536];
				if(res_outr == null) res_outr = new float [65536];
				if(res_inl == null) res_inl  = new float [4*65536];
				if(res_inr == null) res_inr  = new float [4*65536];

				resampPtrIn_l  = DttSP.NewResamplerF(sample_rateVAC,sample_rate1);
				resampPtrIn_r  = DttSP.NewResamplerF(sample_rateVAC,sample_rate1);
				resampPtrOut_l = DttSP.NewResamplerF(sample_rate1,sample_rateVAC);
				resampPtrOut_r = DttSP.NewResamplerF(sample_rate1,sample_rateVAC);
			}
			else vac_resample = false;

			cs_vac = (void *)0x0;
			cs_vac = Win32.NewCriticalSection();
			if (Win32.InitializeCriticalSectionAndSpinCount(cs_vac, 0x00000080) == 0)
			{
				vac_enabled = false;
				Debug.WriteLine("CriticalSection Failed");
			}
		}

		unsafe private static void CleanUpVAC()
		{
			Win32.DeleteCriticalSection(cs_vac);
			rb_vacOUT_l = null;
			rb_vacOUT_r = null;
			rb_vacIN_l = null;
			rb_vacIN_r = null;

			res_outl = null;
			res_outr = null;
			res_inl = null;
			res_inr = null;

			resampPtrIn_l = null;
			resampPtrIn_r = null;
			resampPtrOut_l = null;
			resampPtrOut_r = null;

            loopDLL_inl = null;
            loopDLL_inr = null;
            loopDLL_outl = null;
            loopDLL_outr = null;
			Win32.DestroyCriticalSection(cs_vac);
		}

		unsafe public static double GetCPULoad()
		{
			return PA19.PA_GetStreamCpuLoad(stream1);
		}

		public static ArrayList GetPAHosts() // returns a text list of driver types
		{
			ArrayList a = new ArrayList();
			for(int i=0; i<PA19.PA_GetHostApiCount(); i++)
			{
				PA19.PaHostApiInfo info = PA19.PA_GetHostApiInfo(i);
				a.Add(info.name);
			}
			return a;
		}

		public static ArrayList GetPAInputdevices(int hostIndex)
		{
			ArrayList a = new ArrayList();
			PA19.PaHostApiInfo hostInfo = PA19.PA_GetHostApiInfo(hostIndex);
			for(int i=0; i<hostInfo.deviceCount; i++)
			{
				int devIndex = PA19.PA_HostApiDeviceIndexToDeviceIndex(hostIndex, i);
				PA19.PADeviceInfo devInfo = PA19.PA_GetDeviceInfo(devIndex);
				if(devInfo.maxInputChannels > 0)
					a.Add(new PADeviceInfo(devInfo.name, i)/* + " - " + devIndex*/);
			}
			return a;
		}	

		public static ArrayList GetPAOutputdevices(int hostIndex)
		{
			ArrayList a = new ArrayList();
			PA19.PaHostApiInfo hostInfo = PA19.PA_GetHostApiInfo(hostIndex);
			for(int i=0; i<hostInfo.deviceCount; i++)
			{
				int devIndex = PA19.PA_HostApiDeviceIndexToDeviceIndex(hostIndex, i);
				PA19.PADeviceInfo devInfo = PA19.PA_GetDeviceInfo(devIndex);
				if(devInfo.maxOutputChannels > 0)
					a.Add(new PADeviceInfo(devInfo.name, i)/* + " - " + devIndex*/);
			}
			return a;
		}	

		unsafe public static bool Start()      // changes yt7pwr
		{
			bool retval = false;
			phase_buf_l = new float[block_size1];
			phase_buf_r = new float[block_size1];

            if (CATNetwork_mutex == null)
                CATNetwork_mutex = new Mutex(false);
            if (MultiPSK_mutex == null)
                MultiPSK_mutex = new Mutex(false);
            if (DttSP_mutex == null)
                DttSP_mutex = new Mutex(false);

            #region Spectrum analyzer
            if (enable_spectrum_analyzer)
            {
/*
                if (num_channels == 2)
                    retval = StartAudio(ref SA_callback, (uint)block_size1, sample_rate1,
                        host1, input_dev1, output_dev1, num_channels, 0, latency1);
                else if (num_channels == 4)
                    retval = StartAudio(ref SA_callback4port, (uint)block_size1, sample_rate1,
                        host1, input_dev1, output_dev1, num_channels, 0, latency1);

                return true;*/
            }
            #endregion

            #region MultiPSK server
            else if (enable_MultiPSK_server)    // act as MultiPSK network server
            {
                network_input_bufer_l = new byte[block_size1 * sizeof(float) * 2];
                network_input_bufer_r = new byte[block_size1 * sizeof(float) * 2];

                retval = StartAudio(ref MultiPSKServerCallback, (uint)block_size1, sample_rate1,
                    host1, input_dev1, output_dev1, num_channels, 0, latency1);
            }
            #endregion

            #region ethernet server
            else if (enable_ethernet_server)         // act as network server
            {
                if (num_channels == 2 && server_rf_spectar)
                    retval = StartAudio(ref ServerCallbackFullSpectar, (uint)block_size1, sample_rate1, host1,
                        input_dev1, output_dev1, num_channels, 0, latency1);
                else if (num_channels == 2 && !server_rf_spectar)
                    retval = StartAudio(ref ServerCallbackAFSpectar, (uint)block_size1, sample_rate1, host1,
                        input_dev1, output_dev1, num_channels, 0, latency1);
                else if (num_channels == 4 && server_rf_spectar)
                    retval = StartAudio(ref ServerCallback4portFullSpectar, (uint)block_size1, sample_rate1, host1,
                        input_dev1, output_dev1, num_channels, 0, latency1);
                else if (num_channels == 4 && !server_rf_spectar)
                    retval = StartAudio(ref ServerCallback4portAFSpectar, (uint)block_size1, sample_rate1, host1,
                        input_dev1, output_dev1, num_channels, 0, latency1);
            }
            #endregion

            #region local host
            else if(enable_local_host)          // no network function
            {
                if (audio_exclusive && console.CurrentSoundCard == SoundCard.UNSUPPORTED_CARD)
                {
                    if (num_channels == 2)
                    {
                        retval = StartAudioExclusive(ref input_callback1, (uint)block_size1,
                            sample_rate1, host1, input_dev1, 0xffff, 0, 0, latency1);
                        retval = StartAudioExclusive(ref output_callback1, (uint)block_size1,
                            sample_rate1, host1, 0xffff, output_dev1, 1, 0, latency1);
                    }
                    else if (num_channels == 4 || num_channels == 6)
                    {
                        retval = StartAudioExclusive(ref input_callback1, (uint)block_size1,
                            sample_rate1, host1, input_dev1, 0xffff, 0, 0, latency1);
                        retval = StartAudioExclusive(ref output_callback1, (uint)block_size1,
                            sample_rate1, host1, 0xffff, output_dev1, 1, 0, latency1);
                        retval = StartAudioExclusive(ref input_callback4port, (uint)block_size1, sample_rate1,
                            host1, input_dev3, 0xf0f0, 2, 0, latency1);
                        retval = StartAudioExclusive(ref output_callback4port, (uint)block_size1, sample_rate1,
                            host1, 0xf0f0, output_dev3, 3, 0, latency1);
                    }
                }
                else
                {
                    if (num_channels == 2)
                    {
                        retval = StartAudio(ref callback1, (uint)block_size1,
                            sample_rate1, host1, input_dev1, output_dev1, num_channels, 0, latency1);
                    }
                    else if (num_channels == 4)
                    {
                        retval = StartAudio(ref callback4port, (uint)block_size1, sample_rate1,
                            host1, input_dev1, output_dev1, num_channels, 0, latency1);
                    }
                }
            }
            #endregion

            #region ethernet client
            else if (enable_ethernet_client)    // act as network client
            {
/*
                network_input_bufer_l = new byte[block_size1 * sizeof(float) * 2];
                network_input_bufer_r = new byte[block_size1 * sizeof(float) * 2];

                if (client_rf_spectar)
                {
                    retval = StartAudio(ref ClientCallback1, (uint)block_size1, sample_rate1,
                        host1, input_dev1, output_dev1, num_channels, 1, latency1);
                }
                else
                {
                    retval = StartAudio(ref ClientCallback1, (uint)block_size1, sample_rate1,
                        host1, input_dev1, output_dev1, num_channels, 0, latency1);
                }*/
            }
            #endregion

            if (!retval) return retval;

            #region loop.dll

            if (vac_enabled && !enable_ethernet_server && !enable_ethernet_client && loopDLL_enabled)
            {
                if (loopDLL_inl == null) 
                    loopDLL_inl = new double[block_size_vac];
                else
                {
                    loopDLL_inl = null;
                    loopDLL_inl = new double[block_size_vac];
                }
                if (loopDLL_inr == null)
                    loopDLL_inr = new double[block_size_vac];
                else
                {
                    loopDLL_inr = null;
                    loopDLL_inr = new double[block_size_vac];
                }
                if (loopDLL_outl == null)
                    loopDLL_outl = new double[block_size_vac];
                else
                {
                    loopDLL_outl = null;
                    loopDLL_outl = new double[block_size_vac];
                }
                if (loopDLL_outr == null)
                    loopDLL_outr = new double[block_size_vac];
                else
                {
                    loopDLL_outr = null;
                    loopDLL_outr = new double[block_size_vac];
                }

                console.loopDLL.InitRXPlay(sample_rate1, block_size1);
                console.loopDLL.InitRXRec(sample_rate1, block_size1);
                console.loopDLL.InitTXRec(sample_rate1, block_size1);
                console.loopDLL.InitTXPlay(sample_rate1, block_size1);

                resampPtrOut_l = DttSP.NewResamplerF(sample_rate1, 11025);
            }
            #endregion

            #region VAC
            else if (vac_enabled && !enable_ethernet_server && !enable_ethernet_client)
            {
                int num_chan = 1;
                if (vac_stereo) num_chan = 2;
                vac_rb_reset = true;
                if (VAC_audio_exclusive && console.CurrentSoundCard == SoundCard.UNSUPPORTED_CARD)
                {
                    retval = StartAudioExclusive(ref input_callbackVAC, (uint)block_size_vac, sample_rateVAC,
                        host2, input_dev2, 0xffff, 4, 1, latency2);
                    retval = StartAudioExclusive(ref output_callbackVAC, (uint)block_size_vac, sample_rateVAC,
                        host2, 0xffff, output_dev2, 5, 1, latency2);
                }
                else
                {
                    retval = StartAudio(ref callbackVAC, (uint)block_size_vac, sample_rateVAC,
                        host2, input_dev2, output_dev2, num_chan, 1, latency2);
                }
                Thread.Sleep(100);
            }
            else if (vac_enabled && enable_ethernet_server && !enable_ethernet_client)
            {
                int num_chan = 1;
                if (vac_stereo) num_chan = 2;
                vac_rb_reset = true;
                retval = StartAudio(ref ServerCallbackVACFullSpectar, (uint)block_size2, sample_rateVAC,
                    host2, input_dev2, output_dev2, num_chan, 1, latency2);
                Thread.Sleep(100);
            }
            else if (vac_enabled && !enable_ethernet_server && enable_ethernet_client)
            {
                int num_chan = 1;
                if (vac_stereo) num_chan = 2;
                vac_rb_reset = true;
                vac_callback = true;
                retval = StartAudio(ref callbackVAC, (uint)block_size2, sample_rateVAC,
                    host2, input_dev2, output_dev2, num_chan, 1, latency2);
                Thread.Sleep(100);
                vac_callback = false;
            }
            #endregion

            return retval;
		}

        public unsafe static bool StartAudio(ref PA19.PaStreamCallback callback,
            uint block_size, double sample_rate, int host_api_index, int input_dev_index,
            int output_dev_index, int num_channels, int callback_num, int latency_ms)         // changes yt7pwr
        {
            try
            {
                if (enable_spectrum_analyzer)
                {
                    int in_dev = PA19.PA_HostApiDeviceIndexToDeviceIndex(host_api_index, input_dev_index);
                    int out_dev = PA19.PA_HostApiDeviceIndexToDeviceIndex(host_api_index, output_dev_index);

                    PA19.PaStreamParameters inparam = new PA19.PaStreamParameters();
                    PA19.PaStreamParameters outparam = new PA19.PaStreamParameters();

                    inparam.device = in_dev;
                    inparam.channelCount = num_channels;
                    inparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;
                    inparam.suggestedLatency = ((float)latency_ms / 1000);

                    outparam.device = out_dev;
                    outparam.channelCount = num_channels;
                    outparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;
                    outparam.suggestedLatency = ((float)latency_ms / 1000);

                    int error = 0;
                    if (callback_num == 0)
                        error = PA19.PA_OpenStream(out stream3, &inparam, &outparam, sample_rate, block_size, 0, callback, 0, 2);
                    else
                        error = PA19.PA_OpenStream(out stream5, &inparam, &outparam, sample_rate, block_size, 0, callback, 0, 4);   // VAC

                    if (error != 0)
                    {
                        MessageBox.Show(PA19.PA_GetErrorText(error), "PortAudio Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (callback_num == 0)
                        error = PA19.PA_StartStream(stream3);
                    else
                        error = PA19.PA_StartStream(stream5);

                    if (error != 0)
                    {
                        MessageBox.Show(PA19.PA_GetErrorText(error), "PortAudio Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    return true;
                }
                else if (enable_MultiPSK_server)        // MultiPSK server mode
                {
                    if (res_outl == null) res_outl = new float[65536];
                    if (res_inl == null) res_inl = new float[65536];
                    if (vac_outl == null) vac_outl = new float[65536];
                    if (vac_outr == null) vac_outr = new float[65536];
                    if (vac_inl == null) vac_inl = new float[65536];
                    if (vac_inr == null) vac_inr = new float[65536];
                    resampPtrOut_l = DttSP.NewResamplerF(sample_rate1, 6000);
                    resampPtrOut_r = DttSP.NewResamplerF(sample_rate1, 6000);
                    resampPtrIn_l = DttSP.NewResamplerF(6000, sample_rate1);
                    resampPtrIn_r = DttSP.NewResamplerF(6000, sample_rate1);

                    if (sample_rate1 == 48000)
                        console.MultiPSKServer.send_bytes = 470;
                    else if (sample_rate1 == 96000)
                        console.MultiPSKServer.send_bytes = 235;
                    else
                        console.MultiPSKServer.send_bytes = 512;

                    int in_dev = PA19.PA_HostApiDeviceIndexToDeviceIndex(host_api_index, input_dev_index);
                    int out_dev = PA19.PA_HostApiDeviceIndexToDeviceIndex(host_api_index, output_dev_index);

                    PA19.PaStreamParameters inparam = new PA19.PaStreamParameters();
                    PA19.PaStreamParameters outparam = new PA19.PaStreamParameters();

                    inparam.device = in_dev;
                    inparam.channelCount = num_channels;
                    inparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;
                    inparam.suggestedLatency = ((float)latency_ms / 1000);

                    outparam.device = out_dev;
                    outparam.channelCount = num_channels;
                    outparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;
                    outparam.suggestedLatency = ((float)latency_ms / 1000);

                    int error = 0;
                    if (callback_num == 0)
                        error = PA19.PA_OpenStream(out stream3, &inparam, &outparam, sample_rate, block_size, 0, callback, 0, 2);
                    else
                        error = PA19.PA_OpenStream(out stream5, &inparam, &outparam, sample_rate, block_size, 0, callback, 0, 4);       // VAC

                    if (error != 0)
                    {
                        MessageBox.Show(PA19.PA_GetErrorText(error), "PortAudio Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (callback_num == 0)
                        error = PA19.PA_StartStream(stream3);
                    else
                        error = PA19.PA_StartStream(stream5);

                    if (error != 0)
                    {
                        MessageBox.Show(PA19.PA_GetErrorText(error), "PortAudio Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    return true;
                }
                else if (enable_local_host)         // normal operation
                {
                    if (res_outl == null) res_outl = new float[65536];
                    if (res_inl == null) res_inl = new float[65536];
                    if (vac_outl == null) vac_outl = new float[65536];
                    if (vac_outr == null) vac_outr = new float[65536];
                    if (vac_inl == null) vac_inl = new float[65536];
                    if (vac_inr == null) vac_inr = new float[65536];

                    int in_dev = PA19.PA_HostApiDeviceIndexToDeviceIndex(host_api_index, input_dev_index);
                    int out_dev = PA19.PA_HostApiDeviceIndexToDeviceIndex(host_api_index, output_dev_index);

                    PA19.PaStreamParameters inparam = new PA19.PaStreamParameters();
                    PA19.PaStreamParameters outparam = new PA19.PaStreamParameters();

                    inparam.device = in_dev;
                    inparam.channelCount = num_channels;
                    inparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;
                    inparam.suggestedLatency = ((float)latency_ms / 1000);

                    outparam.device = out_dev;
                    outparam.channelCount = num_channels;
                    outparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;
                    outparam.suggestedLatency = ((float)latency_ms / 1000);

                    if (host_api_index == PA19.PA_HostApiTypeIdToHostApiIndex(PA19.PaHostApiTypeId.paWASAPI) &&
                        (console.WinVer == WindowsVersion.Windows7 || console.WinVer == WindowsVersion.WindowsVista))
                    {
                        PA19.PaWasapiStreamInfo stream_info = new PA19.PaWasapiStreamInfo();
                        stream_info.hostApiType = PA19.PaHostApiTypeId.paWASAPI;
//                        stream_info.threadPriority = PA19.PaWasapiThreadPriority.eThreadPriorityProAudio;
                        stream_info.version = 1;
                        stream_info.size = (UInt32)sizeof(PA19.PaWasapiStreamInfo);
//                        stream_info.flags = (UInt32)PA19.PaWasapiFlags.paWinWasapiExclusive |
//                            PA19.AUDCLNT_STREAMFLAGS_EVENTCALLBACK;
                        inparam.hostApiSpecificStreamInfo = &stream_info;
                        outparam.hostApiSpecificStreamInfo = &stream_info;
                    }

                    int error = 0;
                    if (callback_num == 0)
                        error = PA19.PA_OpenStream(out stream3, &inparam, &outparam, sample_rate, block_size, 0, callback, 0, 2);
                    else
                        error = PA19.PA_OpenStream(out stream5, &inparam, &outparam, sample_rate, block_size, 0, callback, 0, 4);   // VAC

                    if (error != 0)
                    {
                        MessageBox.Show(PA19.PA_GetErrorText(error), "PortAudio Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (callback_num == 0)
                        error = PA19.PA_StartStream(stream3);
                    else
                        error = PA19.PA_StartStream(stream5);

                    if (error != 0)
                    {
                        MessageBox.Show(PA19.PA_GetErrorText(error), "PortAudio Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    return true;
                }
                else if (enable_ethernet_server)
                {
                    if (res_inl == null) res_inl = new float[65536];
                    if (res_inr == null) res_inr = new float[65536];
                    if (vac_outl == null) vac_outl = new float[65536];
                    if (vac_outr == null) vac_outr = new float[65536];
                    if (vac_inl == null) vac_inl = new float[65536];
                    if (vac_inr == null) vac_inr = new float[65536];
                    resampServerPtrIn_l = DttSP.NewResamplerF(sample_rate1, 6000);
                    resampServerPtrIn_r = DttSP.NewResamplerF(sample_rate1, 6000);
                    resampPtrOut_l = DttSP.NewResamplerF(6000, sample_rate1);
                    resampPtrOut_r = DttSP.NewResamplerF(6000, sample_rate1);

                    int in_dev = PA19.PA_HostApiDeviceIndexToDeviceIndex(host_api_index, input_dev_index);
                    int out_dev = PA19.PA_HostApiDeviceIndexToDeviceIndex(host_api_index, output_dev_index);

                    PA19.PaStreamParameters inparam = new PA19.PaStreamParameters();
                    PA19.PaStreamParameters outparam = new PA19.PaStreamParameters();

                    inparam.device = in_dev;
                    inparam.channelCount = num_channels;

                    inparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;

                    inparam.suggestedLatency = ((float)latency_ms / 1000);

                    outparam.device = out_dev;
                    outparam.channelCount = num_channels;

                    outparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;

                    outparam.suggestedLatency = ((float)latency_ms / 1000);

                    int error = 0;
                    error = PA19.PA_OpenStream(out stream3, &inparam, &outparam, sample_rate, block_size, 0, callback, 0, 2);

                    if (error != 0)
                    {
                        MessageBox.Show(PA19.PA_GetErrorText(error), "PortAudio Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (callback_num == 0)
                        error = PA19.PA_StartStream(stream3);

                    if (error != 0)
                    {
                        MessageBox.Show(PA19.PA_GetErrorText(error), "PortAudio Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    return true;
                }
                else if (enable_ethernet_client)        // network client mode
                {
                    if (res_outl == null) res_outl = new float[65536];
                    if (res_outr == null) res_outr = new float[65536];
                    if (vac_outl == null) vac_outl = new float[65536];
                    if (vac_outr == null) vac_outr = new float[65536];
                    if (vac_inl == null) vac_inl = new float[65536];
                    if (vac_inr == null) vac_inr = new float[65536];
                    resampPtrOut_l = DttSP.NewResamplerF(6000, sample_rate1);
                    resampPtrOut_r = DttSP.NewResamplerF(6000, sample_rate1);
                    resampServerPtrIn_l = DttSP.NewResamplerF(sample_rate1, 6000);
                    resampServerPtrIn_r = DttSP.NewResamplerF(sample_rate1, 6000);

                    int in_dev = PA19.PA_HostApiDeviceIndexToDeviceIndex(host_api_index, input_dev_index);
                    int out_dev = PA19.PA_HostApiDeviceIndexToDeviceIndex(host_api_index, output_dev_index);

                    PA19.PaStreamParameters inparam = new PA19.PaStreamParameters();
                    PA19.PaStreamParameters outparam = new PA19.PaStreamParameters();

                    inparam.device = in_dev;
                    inparam.channelCount = num_channels;

                    inparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;

                    inparam.suggestedLatency = ((float)latency_ms / 1000);

                    outparam.device = out_dev;
                    outparam.channelCount = num_channels;

                    outparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;

                    outparam.suggestedLatency = ((float)latency_ms / 1000);

                    int error = 0;
                    if (vac_callback)
                        error = PA19.PA_OpenStream(out stream3, &inparam, &outparam, sample_rate, block_size, 0, callback, 0, 2);
                    else
                        error = PA19.PA_OpenStream(out stream5, null, &outparam, sample_rate, block_size, 0, callback, 0, 4);       // VAC

                    if (error != 0)
                    {
                        MessageBox.Show(PA19.PA_GetErrorText(error), "PortAudio Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (callback_num == 0)
                        error = PA19.PA_StartStream(stream3);
                    else
                        error = PA19.PA_StartStream(stream5);

                    if (error != 0)
                    {
                        MessageBox.Show(PA19.PA_GetErrorText(error), "PortAudio Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
                return false;
            }
        }

        public unsafe static bool StartAudioExclusive(ref PA19.PaStreamCallback callback,
            uint block_size, double sample_rate, int host_api_index, int input_dev_index,
            int output_dev_index, int callback_id, int callback_num, int latency_ms)         // yt7pwr
        {
            try
            {
                #region local host
                if (enable_local_host)         // normal operation
                {
                    int error = 0;
                    if (res_outl == null) res_outl = new float[65536];
                    if (res_inl == null) res_inl = new float[65536];
                    if (vac_outl == null) vac_outl = new float[65536];
                    if (vac_outr == null) vac_outr = new float[65536];
                    if (vac_inl == null) vac_inl = new float[65536];
                    if (vac_inr == null) vac_inr = new float[65536];

                    int in_dev = PA19.PA_HostApiDeviceIndexToDeviceIndex(host_api_index, input_dev_index);
                    int out_dev = PA19.PA_HostApiDeviceIndexToDeviceIndex(host_api_index, output_dev_index);

                    PA19.PaStreamParameters inparam = new PA19.PaStreamParameters();
                    PA19.PaStreamParameters outparam = new PA19.PaStreamParameters();

                    if (output_dev_index == 0xffff)
                    {
                        inparam.device = in_dev;
                        inparam.channelCount = 2;
                        inparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;
                        inparam.suggestedLatency = ((float)latency_ms / 1000);

                        if (host_api_index == PA19.PA_HostApiTypeIdToHostApiIndex(PA19.PaHostApiTypeId.paWASAPI) &&
                            audio_exclusive && (console.WinVer == WindowsVersion.Windows7
                            || console.WinVer == WindowsVersion.WindowsVista))
                        {
                            PA19.PaWasapiStreamInfo input_stream_info = new PA19.PaWasapiStreamInfo();
                            input_stream_info.hostApiType = PA19.PaHostApiTypeId.paWASAPI;
                            input_stream_info.threadPriority = PA19.PaWasapiThreadPriority.eThreadPriorityProAudio;
                            input_stream_info.version = 1;
                            input_stream_info.size = (UInt32)sizeof(PA19.PaWasapiStreamInfo);
                            input_stream_info.flags = (UInt32)PA19.PaWasapiFlags.paWinWasapiExclusive |
                                PA19.AUDCLNT_STREAMFLAGS_EVENTCALLBACK;
                            inparam.hostApiSpecificStreamInfo = &input_stream_info;
                        }

                        error = 0;
                        if (callback_num == 0)
                            error = PA19.PA_OpenStream(out stream1, &inparam, null,
                                sample_rate, block_size, 0, callback, 0, callback_id);
                        else
                            error = PA19.PA_OpenStream(out stream5, &inparam, null,
                                sample_rate, block_size, 0, callback, 0, callback_id);  // input for excl. VAC

                        if (error != 0)
                        {
                            MessageBox.Show("opening input fails!\n\n" + PA19.PA_GetErrorText(error).ToString(),
                                "PortAudio Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        if (callback_num == 0)
                            error = PA19.PA_StartStream(stream1);
                        else
                            error = PA19.PA_StartStream(stream5);

                        if (error != 0)
                        {
                            MessageBox.Show("Starting input fails!\n" + PA19.PA_GetErrorText(error).ToString(),
                                "PortAudio Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                    }
                    else if (output_dev_index == 0xf0f0)
                    {
                        inparam.device = in_dev;
                        inparam.channelCount = 2;
                        inparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;
                        inparam.suggestedLatency = ((float)latency_ms / 1000);

                        if (host_api_index == PA19.PA_HostApiTypeIdToHostApiIndex(PA19.PaHostApiTypeId.paWASAPI) &&
                            audio_exclusive && (console.WinVer == WindowsVersion.Windows7
                            || console.WinVer == WindowsVersion.WindowsVista))
                        {
                            PA19.PaWasapiStreamInfo input_stream_info = new PA19.PaWasapiStreamInfo();
                            input_stream_info.hostApiType = PA19.PaHostApiTypeId.paWASAPI;
                            input_stream_info.threadPriority = PA19.PaWasapiThreadPriority.eThreadPriorityProAudio;
                            input_stream_info.version = 1;
                            input_stream_info.size = (UInt32)sizeof(PA19.PaWasapiStreamInfo);
                            input_stream_info.flags = (UInt32)PA19.PaWasapiFlags.paWinWasapiExclusive |
                                PA19.AUDCLNT_STREAMFLAGS_EVENTCALLBACK;
                            inparam.hostApiSpecificStreamInfo = &input_stream_info;
                        }

                        error = 0;
                        error = PA19.PA_OpenStream(out stream2, &inparam, null,
                            sample_rate, block_size, 0, callback, 0, callback_id);

                        if (error != 0)
                        {
                            MessageBox.Show("opening input fails!\n\n" + PA19.PA_GetErrorText(error).ToString(),
                                "PortAudio Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        error = PA19.PA_StartStream(stream2);
                        if (error != 0)
                        {
                            MessageBox.Show("Starting input fails!\n" + PA19.PA_GetErrorText(error).ToString(),
                                "PortAudio Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                    }
                    else if (input_dev_index == 0xffff)
                    {
                        outparam.device = out_dev;
                        outparam.channelCount = 2;
                        outparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;
                        outparam.suggestedLatency = ((float)latency_ms / 1000);

                        if (host_api_index == PA19.PA_HostApiTypeIdToHostApiIndex(PA19.PaHostApiTypeId.paWASAPI) &&
                            (console.WinVer == WindowsVersion.Windows7 || console.WinVer == WindowsVersion.WindowsVista))
                        {
                            PA19.PaWasapiStreamInfo output_stream_info = new PA19.PaWasapiStreamInfo();
                            output_stream_info.hostApiType = PA19.PaHostApiTypeId.paWASAPI;
                            output_stream_info.threadPriority = PA19.PaWasapiThreadPriority.eThreadPriorityProAudio;
                            output_stream_info.version = 1;
                            output_stream_info.size = (UInt32)sizeof(PA19.PaWasapiStreamInfo);
                            output_stream_info.flags = (UInt32)PA19.PaWasapiFlags.paWinWasapiExclusive |
                                PA19.AUDCLNT_STREAMFLAGS_EVENTCALLBACK;
                            outparam.hostApiSpecificStreamInfo = &output_stream_info;
                        }

                        if (callback_num == 0)
                            error = PA19.PA_OpenStream(out stream3, null, &outparam,
                                sample_rate, block_size, 0, callback, 0, callback_id);
                        else
                            error = PA19.PA_OpenStream(out stream6, null, &outparam,
                                sample_rate, block_size, 0, callback, 0, callback_id);

                        if (error != 0)
                        {
                            MessageBox.Show("Opening output fails!\n" + PA19.PA_GetErrorText(error).ToString(),
                                "PortAudio Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        if (callback_num == 0)
                            error = PA19.PA_StartStream(stream3);
                        else
                            error = PA19.PA_StartStream(stream6);

                        if (error != 0)
                        {
                            MessageBox.Show("Starting output fails!\n" + PA19.PA_GetErrorText(error).ToString(),
                                "PortAudio Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    else if (input_dev_index == 0xf0f0)
                    {
                        outparam.device = out_dev;
                        outparam.channelCount = 2;
                        outparam.sampleFormat = PA19.paFloat32 | PA19.paNonInterleaved;
                        outparam.suggestedLatency = ((float)latency_ms / 1000);

                        if (host_api_index == PA19.PA_HostApiTypeIdToHostApiIndex(PA19.PaHostApiTypeId.paWASAPI) &&
                            (console.WinVer == WindowsVersion.Windows7 || console.WinVer == WindowsVersion.WindowsVista))
                        {
                            PA19.PaWasapiStreamInfo output_stream_info = new PA19.PaWasapiStreamInfo();
                            output_stream_info.hostApiType = PA19.PaHostApiTypeId.paWASAPI;
                            output_stream_info.threadPriority = PA19.PaWasapiThreadPriority.eThreadPriorityProAudio;
                            output_stream_info.version = 1;
                            output_stream_info.size = (UInt32)sizeof(PA19.PaWasapiStreamInfo);
                            output_stream_info.flags = (UInt32)PA19.PaWasapiFlags.paWinWasapiExclusive |
                                PA19.AUDCLNT_STREAMFLAGS_EVENTCALLBACK;
                            outparam.hostApiSpecificStreamInfo = &output_stream_info;
                        }

                        error = PA19.PA_OpenStream(out stream4, null, &outparam,
                            sample_rate, block_size, 0, callback, 0, callback_id);

                        if (error != 0)
                        {
                            MessageBox.Show("Opening output fails!\n" + PA19.PA_GetErrorText(error).ToString(),
                                "PortAudio Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        error = PA19.PA_StartStream(stream4);

                        if (error != 0)
                        {
                            MessageBox.Show("Starting output fails!\n" + PA19.PA_GetErrorText(error).ToString(),
                                "PortAudio Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }

                    return true;
                }
                #endregion

                return false;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
                return false;
            }
        }

		public unsafe static void StopAudio1()
		{
            int error = 0;

            error = PA19.PA_StopStream(stream1);
            if (error == (int)PA19.PaErrorCode.paNoError ||
                error == (int)PA19.PaErrorCode.paStreamIsStopped)
            {
                PA19.PA_AbortStream(stream1);
                PA19.PA_CloseStream(stream1);
            }
            else
            {
//                Debug.Write(PA19.PA_GetErrorText(error).ToString());
            }
            error = PA19.PA_StopStream(stream2);
            if (error == (int)PA19.PaErrorCode.paNoError ||
                error == (int)PA19.PaErrorCode.paStreamIsStopped)
            {
                PA19.PA_AbortStream(stream2);
                PA19.PA_CloseStream(stream2);
            }
            else
            {
//                Debug.Write(PA19.PA_GetErrorText(error).ToString());
            }
            error = PA19.PA_StopStream(stream3);
            if (error == (int)PA19.PaErrorCode.paNoError ||
                error == (int)PA19.PaErrorCode.paStreamIsStopped)
            {
                PA19.PA_AbortStream(stream3);
                PA19.PA_CloseStream(stream3);
            }
            else
            {
//                Debug.Write(PA19.PA_GetErrorText(error).ToString());
            }
            error = PA19.PA_StopStream(stream4);
            if (error == (int)PA19.PaErrorCode.paNoError ||
                error == (int)PA19.PaErrorCode.paStreamIsStopped)
            {
                PA19.PA_AbortStream(stream4);
                PA19.PA_CloseStream(stream4);
            }
            else
            {
//                Debug.Write(PA19.PA_GetErrorText(error).ToString());
            }

            CATNetwork_mutex.Close();
            CATNetwork_mutex = null;
            MultiPSK_mutex.Close();
            MultiPSK_mutex = null;
            DttSP_mutex.Close();
            DttSP_mutex = null;
		}

		public unsafe static void StopAudioVAC()
		{
            int error = PA19.PA_StopStream(stream5);
            if (error == (int)PA19.PaErrorCode.paNoError ||
                error == (int)PA19.PaErrorCode.paStreamIsStopped)
            {
                PA19.PA_AbortStream(stream5);
                PA19.PA_CloseStream(stream5);
            }
            else
            {
//                Debug.Write(PA19.PA_GetErrorText(error).ToString());
            }
            error = PA19.PA_StopStream(stream6);
            if (error == (int)PA19.PaErrorCode.paNoError ||
                error == (int)PA19.PaErrorCode.paStreamIsStopped)
            {
                PA19.PA_AbortStream(stream6);
                PA19.PA_CloseStream(stream6);
            }
            else
            {
                //                Debug.Write(PA19.PA_GetErrorText(error).ToString());
            }
		}

		#endregion		
	}
}
