using System;
using System.IO;
using System.IO.Ports;
using System.Media;
using System.Windows.Forms;

namespace P3_INTENTO_3
{
    public partial class Form1 : Form
    {
        private void label1_Click(object sender, EventArgs e) { }

        private void label2_Click(object sender, EventArgs e) { }

        private void pictureBox1_Click(object sender, EventArgs e) { }

        private void Form1_Load(object sender, EventArgs e) { }

        private SerialPort serialPort;
        private bool isBlinking;
        private SoundPlayer soundPlayer;

        public Form1()
        {
            InitializeComponent();

            serialPort = new SerialPort("COM4", 9600);
            serialPort.DataReceived += DataReceivedHandler;

            timer1.Interval = 500;

            var audioBytes = Properties.Resources.AnimalAmbienceMonkeysInJungle01;
            var stream = new MemoryStream(audioBytes);

            soundPlayer = new SoundPlayer(stream);

            label1.Text = "Detector de Agua";
            label2.Text = "Sensor Status: Waiting...";
            pictureBox1.BackColor = System.Drawing.Color.Gray;
        }

        private void button1_Click(object sender, EventArgs e) // COMENZAR
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                    MessageBox.Show("Puerto abierto");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e) // SALIR
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }

            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.BackColor =
                pictureBox1.BackColor == System.Drawing.Color.Gray
                ? System.Drawing.Color.Yellow
                : System.Drawing.Color.Gray;
        }

        void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            string data = serialPort.ReadLine();

            this.Invoke(new Action(() =>
            {
                label2.Text = "Sensor Status: " + (data.Trim() == "1" ? "Water Detected" : "No Water");

                    if (data.Trim() == "1")
                {
                    if (!isBlinking)
                    {
                        timer1.Start();
                        soundPlayer.PlayLooping();
                        pictureBox1.BackColor = System.Drawing.Color.Red;
                        isBlinking = true;
                    }
                }
                else
                {
                    if (isBlinking)
                    {
                        timer1.Stop();
                        soundPlayer.Stop();
                        pictureBox1.BackColor = System.Drawing.Color.Gray;
                        isBlinking = false;
                    }
                }
            }));
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            base.OnFormClosing(e);
        }
    }
}