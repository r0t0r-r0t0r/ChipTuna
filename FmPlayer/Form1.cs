using System;
using System.Windows.Forms;
using ChipTuna.Sound;
using NAudio.Wave;

namespace ChipTuna.FmPlayer
{
    public partial class Form1 : Form
    {
        public static volatile float Frequency1 = 100;
        public static volatile float Frequency2 = 100;
        public static volatile float Frequency3 = 100;
        public static volatile float Frequency4 = 100;
        public static volatile float Frequency5 = 100;
        public static volatile float Frequency6 = 100;

        public Form1()
        {
            InitializeComponent();

            var player = new WaveOutEvent();
            var generator = new SignalGenerator();
            player.Init(generator);
            player.Play();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Frequency1 = GetValue(trackBar1.Value);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            Frequency2 = GetValue(trackBar2.Value);
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            Frequency3 = GetValue(trackBar3.Value);
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            Frequency4 = GetValue(trackBar4.Value);
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            Frequency5 = GetValue(trackBar5.Value);
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            Frequency6 = GetValue(trackBar6.Value);
        }

        private static float GetValue(int power)
        {
            return (float) Math.Pow(10, power / 100f);
        }
    }

    public class SignalGenerator : ISampleProvider
    {
        private readonly SinOscillator _oscillator1 = new SinOscillator(44100) {Amplitude = 1f};
        private readonly SinOscillator _oscillator2 = new SinOscillator(44100) {Amplitude = 1f};
        private readonly SinOscillator _oscillator3 = new SinOscillator(44100) {Amplitude = 1f};
        private readonly SinOscillator _oscillator4 = new SinOscillator(44100) {Amplitude = 1f};
        private readonly SinOscillator _oscillator5 = new SinOscillator(44100) {Amplitude = 1f};
        private readonly SinOscillator _oscillator6 = new SinOscillator(44100) {Amplitude = 0.5f};

        public int Read(float[] buffer, int offset, int count)
        {
            for (var i = 0; i < count; i++)
            {
                _oscillator1.Frequency = Form1.Frequency1;
                _oscillator2.Frequency = Form1.Frequency2 * Math.Abs(_oscillator1.Step());
                _oscillator3.Frequency = Form1.Frequency3 * Math.Abs(_oscillator2.Step());
                _oscillator4.Frequency = Form1.Frequency4 * Math.Abs(_oscillator3.Step());
                _oscillator5.Frequency = Form1.Frequency5 * Math.Abs(_oscillator4.Step());
                _oscillator6.Frequency = Form1.Frequency6 * Math.Abs(_oscillator5.Step());

                buffer[offset + i] = _oscillator6.Step();
            }

            return count;
        }

        public WaveFormat WaveFormat => WaveFormat.CreateIeeeFloatWaveFormat(44100, 1);
    }
}
