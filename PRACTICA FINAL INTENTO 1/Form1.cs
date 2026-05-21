using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;

namespace PRACTICA_FINAL_INTENTO_1
{
    public partial class Form1 : Form
    {
        SerialPort puerto = new SerialPort();

        // Guardamos ángulo y distancia para mostrarlos juntos
        private string _angulo = "--";
        private string _distancia = "--";

        public Form1()
        {
            InitializeComponent();
        }

        // =====================================================
        // LOAD
        // =====================================================
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(SerialPort.GetPortNames());

            comboBox2.Items.Add("9600");
            comboBox2.Items.Add("115200");
            comboBox2.Text = "9600";

            timer1.Interval = 1000;
            timer1.Enabled = true;

            puerto.DataReceived += Puerto_DataReceived;
        }

        // =====================================================
        // RECIBIR Y PARSEAR DATOS DEL ARDUINO
        // =====================================================
        private void Puerto_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string dato = puerto.ReadLine().Trim();

                this.Invoke(new MethodInvoker(delegate
                {
                    ProcesarDato(dato);
                }));
            }
            catch { }
        }

        private void ProcesarDato(string dato)
        {
            // --- ÁNGULO ---
            if (dato.StartsWith("ANG:"))
            {
                _angulo = dato.Substring(4);
                ActualizarRadar();
                return;
            }

            // --- DISTANCIA ---
            if (dato.StartsWith("DIST:"))
            {
                _distancia = dato.Substring(5);
                ActualizarRadar();
                return;
            }

            // --- HUMEDAD ---
            if (dato.StartsWith("HUM:"))
            {
                string hum = dato.Substring(4);
                label7.Text = "Humedad: " + hum;

                // Color visual según estado
                label7.ForeColor = hum == "HUMEDO"
                    ? Color.Blue
                    : Color.Black;
                return;
            }

            // --- AGUA ---
            if (dato.StartsWith("AGUA:"))
            {
                string agua = dato.Substring(5);
                // Usa label6 para agua (antes era temperatura)
                label6.Text = "Agua: " + agua;
                label6.ForeColor = agua == "ALERTA"
                    ? Color.Red
                    : Color.Green;
                return;
            }

            // --- RELAY ---
            if (dato.StartsWith("RELAY:"))
            {
                string estado = dato.Substring(6).Trim();
                label11.Text = estado == "1" ? "RELAY: ON" : "RELAY: OFF";
                label11.ForeColor = estado == "1" ? Color.Green : Color.Red;
                return;
            }
        }

        private void ActualizarRadar()
        {
            label8.Text = "PWM / Radar"
                        + "\nÁngulo:    " + _angulo + "°"
                        + "\nDistancia: " + _distancia + " cm";
        }

        // =====================================================
        // CONNECT
        // =====================================================
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!puerto.IsOpen)
                {
                    puerto.PortName = comboBox1.Text;
                    puerto.BaudRate = Convert.ToInt32(comboBox2.Text);
                    puerto.NewLine = "\n";   // Arduino envía \n al final de println
                    puerto.Open();
                    MessageBox.Show("CONEXIÓN EXITOSA");
                }
                else
                {
                    MessageBox.Show("YA ESTÁ CONECTADO");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // =====================================================
        // REFRESH PORTS
        // =====================================================
        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(SerialPort.GetPortNames());
        }

        // =====================================================
        // LEDS
        // =====================================================
        private void button3_Click(object sender, EventArgs e) => EnviarComando("E");
        private void button4_Click(object sender, EventArgs e) => EnviarComando("R");
        private void button5_Click(object sender, EventArgs e) => EnviarComando("T");
        private void button6_Click(object sender, EventArgs e) => EnviarComando("Y");
        private void button7_Click(object sender, EventArgs e) => EnviarComando("U");

        private void EnviarComando(string cmd)
        {
            if (puerto.IsOpen)
                puerto.Write(cmd);
            else
                MessageBox.Show("PUERTO NO CONECTADO");
        }

        // =====================================================
        // CLOCK
        // =====================================================
        private void timer1_Tick(object sender, EventArgs e)
        {
            label5.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        // =====================================================
        // EVENTOS VACÍOS (mantener para que el designer no rompa)
        // =====================================================
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void pictureBox4_Click(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void groupBox2_Enter(object sender, EventArgs e) { }
        private void groupBox3_Enter(object sender, EventArgs e) { }
        private void groupBox4_Enter(object sender, EventArgs e) { }
        private void groupBox5_Enter(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void label11_Click(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}