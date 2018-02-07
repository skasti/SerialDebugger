using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialDebugger
{
    public partial class MainForm : Form
    {
        private SerialPort _spA;
        private SerialPort _spB;

        private Thread _spAListenerThread;
        private Thread _spBListenerThread;

        List<byte> _eol = new List<byte>();
        private bool _running;

        public MainForm()
        {
            InitializeComponent();

            var names = SerialPort.GetPortNames();
            spBSelector.Items.Clear();
            spBSelector.Items.AddRange(names);
            spASelector.Items.Clear();
            spASelector.Items.AddRange(names);

            if (names.Length > 1)
            {
                spASelector.SelectedItem = names[names.Length - 2];
                spBSelector.SelectedItem = names[names.Length - 1];
            }
        }

        private void spARefresh_Click(object sender, EventArgs e)
        {
            var names = SerialPort.GetPortNames();
            spASelector.Items.Clear();
            spASelector.Items.AddRange(names);
            spBSelector.Items.Clear();
            spBSelector.Items.AddRange(names);
        }

        private void spBRefresh_Click(object sender, EventArgs e)
        {
            var names = SerialPort.GetPortNames();
            spBSelector.Items.Clear();
            spBSelector.Items.AddRange(names);
            spASelector.Items.Clear();
            spASelector.Items.AddRange(names);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            var aName = spASelector.SelectedItem as string;
            var bName = spBSelector.SelectedItem as string;

            if (aName == bName)
            {
                MessageBox.Show("Must select two separate port names", "Duplicate ports selected", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            int baudRate;
            if (!int.TryParse(baudRateInput.Text, out baudRate))
            {
                MessageBox.Show($"Baudrate not valid: {baudRateInput.Text}", "Invalid baudrate", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            _spA = new SerialPort(aName, baudRate);
            _spB = new SerialPort(bName, baudRate);

            _eol.Clear();

            if (eolInput.Text.StartsWith("\\h("))
            {
                var hexStart = eolInput.Text.IndexOf('(') + 1;
                var hexLength = eolInput.Text.IndexOf(')', hexStart) - hexStart;

                var eolHex = eolInput.Text.Substring(hexStart, hexLength).Split(' ', '-', ',');

                foreach (var hexStr in eolHex)
                {
                    _eol.Add(Convert.ToByte(hexStr, 16));
                }
            }
            else
            {
                _eol.AddRange(Encoding.Default.GetBytes(eolInput.Text));
            }

            _spA.ReadBufferSize = 1024;
            _spB.ReadBufferSize = 1024;

            _spA.Open();
            _spB.Open();

            _spAListenerThread = new Thread(SPAListener);
            _spBListenerThread = new Thread(SPBListener);

            _spAListenerThread.Start();
            _spBListenerThread.Start();

            spASelector.Enabled = false;
            spBSelector.Enabled = false;
            eolInput.Enabled = false;
            startButton.Enabled = false;
            stopButton.Enabled = true;

            spTextLog.Text = "";
            spHexLog.Text = "";

            _running = true;
        }

        private void SPAListener()
        {
            var portName = "A";
            var buffer = new byte[40];
            var bPos = 0;
            var eolCounter = 0;

            while (true)
            {
                try
                {
                    if (!_spA.IsOpen)
                    {
                        StopDebugging();
                        return;
                    }

                    while (_spA.BytesToRead > 0)
                    {
                        if (buffer[0] == 0)
                            bPos = 0;

                        if (bPos >= 40)
                        {
                            bPos = 0;
                            eolCounter = 0;
                        }

                        var b = (byte)_spA.ReadByte();
                        buffer[bPos] = b;

                        if (b == _eol[eolCounter])
                            eolCounter++;
                        else
                            eolCounter = 0;

                        if (eolCounter == _eol.Count)
                        {
                            var lineBytes = buffer.Take(bPos - (_eol.Count - 1)).ToArray();
                            var line = Encoding.Default.GetString(lineBytes);

                            for (int i = 0; i < bPos; i++)
                                buffer[i] = 0x00;

                            bPos = 0;
                            eolCounter = 0;

                            LogLine(portName, line, lineBytes);

                            WriteLine(_spB, line);
                        }

                        bPos++;
                    }
                }
                catch (Exception)
                {
                    StopDebugging();
                    return;
                }
            }
        }

        private void WriteLine(SerialPort toPort, string line)
        {
            toPort.Write(line);
            toPort.Write(new byte[] {0xFF, 0xFF, 0xFF},0,3);
        }

        private void SPBListener()
        {
            var portName = "B";
            var buffer = new byte[40];
            var bPos = 0;
            var eolCounter = 0;

            while (true)
            {
                try
                {
                    if (!_spB.IsOpen)
                    {
                        StopDebugging();
                        return;
                    }

                    while (_spB.BytesToRead > 0)
                    {
                        if (buffer[0] == 0)
                            bPos = 0;

                        if (bPos >= 40)
                        {
                            bPos = 0;
                            eolCounter = 0;
                        }

                        var b = (byte) _spB.ReadByte();
                        buffer[bPos] = b;

                        if (b == _eol[eolCounter])
                            eolCounter++;
                        else
                            eolCounter = 0;

                        if (eolCounter == _eol.Count)
                        {
                            var lineBytes = buffer.Take(bPos - (_eol.Count - 1)).ToArray();
                            var line = Encoding.Default.GetString(lineBytes);

                            for (int i = 0; i < bPos; i++)
                                buffer[i] = 0x00;

                            bPos = 0;
                            eolCounter = 0;

                            LogLine(portName, line, lineBytes);

                            WriteLine(_spA, line);
                        }

                        bPos++;
                    }
                }
                catch (Exception)
                {
                    StopDebugging();
                    return;
                }
            }
        }

        private void LogLine(string portName, string line, byte[] lineBytes)
        {
            this.Invoke((MethodInvoker) delegate
            {
                if (!logEnabled.Checked)
                    return;

                spTextLog.AppendText($"\n[{DateTime.Now.ToLongTimeString()}][{portName}]: {line}");
                spHexLog.AppendText($"\n[{DateTime.Now.ToLongTimeString()}][{portName}]: {BitConverter.ToString(lineBytes).Replace('-', ' ')}");

                spTextLog.SelectionStart = spTextLog.TextLength;
                spHexLog.SelectionStart = spHexLog.TextLength;

                spTextLog.ScrollToCaret();
                spHexLog.ScrollToCaret();
            });
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopDebugging();
        }

        private void StopDebugging()
        {
            this.Invoke((MethodInvoker) delegate
            {
                if (!_running)
                    return;

                spASelector.Enabled = true;
                spBSelector.Enabled = true;
                eolInput.Enabled = true;
                startButton.Enabled = true;
                stopButton.Enabled = false;

                _spAListenerThread.Abort();
                _spBListenerThread.Abort();
                _spA.Close();
                _spB.Close();

                _running = false;
            });
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            StopDebugging();
        }

        private void clearLogsButton_Click(object sender, EventArgs e)
        {
            spHexLog.Clear();
            spTextLog.Clear();
        }
    }
}
