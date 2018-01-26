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

        public MainForm()
        {
            InitializeComponent();

            var names = SerialPort.GetPortNames();
            spBSelector.Items.Clear();
            spBSelector.Items.AddRange(names);
            spASelector.Items.Clear();
            spASelector.Items.AddRange(names);
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

            spA = new SerialPort(aName, 9600);
            spB = new SerialPort(bName, 9600);

            _eol.Clear();

            if (eolInput.Text.StartsWith("\\h("))
            {
                var hexStart = eolInput.Text.IndexOf('(') + 1;
                var hexLength = eolInput.Text.IndexOf(')', hexStart) - (hexStart + 1);

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

            spA.ReadBufferSize = 1024;
            spB.ReadBufferSize = 1024;

            spA.Open();
            spB.Open();

            _spAListenerThread = new Thread(SPAListener);
            _spBListenerThread = new Thread(SPBListener);

            _spAListenerThread.Start();
            _spBListenerThread.Start();

            spASelector.Enabled = false;
            spBSelector.Enabled = false;
            eolInput.Enabled = false;
            startButton.Enabled = false;
            stopButton.Enabled = true;
        }

        private void SPAListener()
        {
            var portName = "A";
            var buffer = new byte[40];
            var bPos = 0;
            var eolCounter = 0;

            while (true)
            {
                while (spA.BytesToRead > 0)
                {
                    if (buffer[0] == 0)
                        bPos = 0;

                    if (bPos >= 40)
                    {
                        bPos = 0;
                        eolCounter = 0;
                    }

                    var b = (byte)spA.ReadByte();
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

                        this.Invoke((MethodInvoker)delegate
                        {
                            spTextLog.Text += $"\n[{DateTime.Now.ToShortTimeString()}][{portName}]: {line}";
                            spHexLog.Text += $"\n[{DateTime.Now.ToShortTimeString()}][{portName}]: {BitConverter.ToString(lineBytes).Replace('-',' ')}";
                        });

                        WriteLine(spB, line);
                    }

                    bPos++;
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
                while (spB.BytesToRead > 0)
                {
                    if (buffer[0] == 0)
                        bPos = 0;

                    if (bPos >= 40)
                    {
                        bPos = 0;
                        eolCounter = 0;
                    }

                    var b = (byte)spB.ReadByte();
                    buffer[bPos] = b;

                    if (b == _eol[eolCounter])
                        eolCounter++;
                    else
                        eolCounter = 0;

                    if (eolCounter == _eol.Count)
                    {
                        var lineBytes = buffer.Take(bPos - (_eol.Count-1)).ToArray();
                        var line = Encoding.Default.GetString(lineBytes);

                        for (int i = 0; i < bPos; i++)
                            buffer[i] = 0x00;

                        bPos = 0;
                        eolCounter = 0;

                        this.Invoke((MethodInvoker)delegate
                        {
                            spTextLog.Text += $"\n[{DateTime.Now.ToShortTimeString()}][{portName}]: {line}";
                            spHexLog.Text += $"\n[{DateTime.Now.ToShortTimeString()}][{portName}]: {BitConverter.ToString(lineBytes).Replace('-', ' ')}";
                        });

                        WriteLine(spA, line);
                    }

                    bPos++;
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopDebugging();
        }

        private void StopDebugging()
        {
            _spAListenerThread.Abort();
            _spBListenerThread.Abort();
            _spA.Close();
            _spB.Close();
            spASelector.Enabled = true;
            spBSelector.Enabled = true;
            eolInput.Enabled = true;
            startButton.Enabled = true;
            stopButton.Enabled = false;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            StopDebugging();
        }
    }
}
