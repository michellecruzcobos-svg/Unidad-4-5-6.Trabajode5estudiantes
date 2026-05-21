using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRACTICA_2_INTENTO_1
{
    public partial class Form1 : Form
    {
        SerialPort serialPort;       
        bool puertoCerrado = false;  

        public Form1()
        {
            InitializeComponent();

            serialPort = new SerialPort();
            serialPort.PortName = "COM4"; 
            serialPort.BaudRate = 9600;

            serialPort.DataReceived += DataReceivedHandler; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!serialPort.IsOpen)
            {
                conectar();
            }
            else
            {
                noConectado();
            }
        }

        private void conectar()
        {
            try
            {
                serialPort.Open();
                puertoCerrado = false;
                MessageBox.Show("Conectado");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar: " + ex.Message);
            }
        }

        private void noConectado()
        {
            try
            {
                serialPort.Close();
                puertoCerrado = true;
                MessageBox.Show("Desconectado");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al desconectar: " + ex.Message);
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = serialPort.ReadExisting();

                this.Invoke(new MethodInvoker(delegate
                {
                    listBox1.Items.Add(data);
                }));
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}