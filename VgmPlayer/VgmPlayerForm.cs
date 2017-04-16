using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using ChipTuna.IO;
using ChipTuna.Rendering;
using ChipTuna.Vgm.Reading;
using NAudio.Wave;

namespace ChipTuna.FmPlayer
{
    public partial class VgmPlayerForm : Form
    {
        private readonly WaveOut _player = new WaveOut();
        private BinaryReader _binaryReader;

        public VgmPlayerForm()
        {
            InitializeComponent();
        }

        private void Stop()
        {
            _player.Stop();
            _binaryReader?.Dispose();
        }

        private static BinaryReader OpenVgm(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            var stream = new GZipStream(fileInfo.OpenRead(), CompressionMode.Decompress);
            var binaryReader = new BinaryReader(stream);

            return binaryReader;
        }

        private static ISampleProvider CreateSampleProvider(BinaryReader binaryReader)
        {
            var reader = new StreamSequentialReader(binaryReader);
            var header = HeaderReader.Read(reader);
            var commands = CommandsReader.Read(header, reader);
            var values = Renderer.Render(commands);
            var provider = new EnumerableSampleProvider(values);

            return provider;
        }

        private void OnDropVgmPanelDragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            Stop();

            _binaryReader = OpenVgm(files[0]);
            var sampleProvider = CreateSampleProvider(_binaryReader);
            _player.Init(sampleProvider);
            _player.Play();
        }

        private void OnDropVgmPanelDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void OnStopButtonClick(object sender, EventArgs e)
        {
            Stop();
        }
    }

    public class EnumerableSampleProvider: ISampleProvider
    {
        private readonly IEnumerator<float> _enumerator;

        public EnumerableSampleProvider(IEnumerable<float> values)
        {
            _enumerator = values.GetEnumerator();
        }

        public int Read(float[] buffer, int offset, int count)
        {
            var i = 0;
            while (i < count && _enumerator.MoveNext())
            {
                buffer[offset + i] = _enumerator.Current;
                i++;
            }
            return i;
        }

        public WaveFormat WaveFormat => WaveFormat.CreateIeeeFloatWaveFormat(44100, 1);
    }
}
