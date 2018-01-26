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

            //spA.NewLine = Encoding.Default.GetString(new byte[] {0xFF, 0xFF, 0xFF});
            //spB.NewLine = Encoding.Default.GetString(new byte[] { 0xFF, 0xFF, 0xFF });

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

                    if (b == 0xFF)
                        eolCounter++;
                    else
                        eolCounter = 0;

                    if (eolCounter == 3)
                    {
                        var lineBytes = buffer.Take(bPos - 2).ToArray();
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

                    if (b == 0xFF)
                        eolCounter++;
                    else
                        eolCounter = 0;

                    if (eolCounter == 3)
                    {
                        var lineBytes = buffer.Take(bPos - 2).ToArray();
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
            _spAListenerThread.Abort();
            _spBListenerThread.Abort();
        }
    }
}
