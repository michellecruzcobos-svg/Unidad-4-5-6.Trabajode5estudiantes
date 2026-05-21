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

namespace PRACTICA_1_INTENTO_3
{
    public partial class Form1 : Form
    {
        System.IO.Ports.SerialPort Arduino;
        public Form1()
        {
            InitializeComponent();
            Arduino = new System.IO.Ports.SerialPort();
            Arduino = new SerialPort("COM3", 9600); // Ajusta el puerto COM según tu Arduino
            Arduino.Open();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Arduino.Write("E"); // Señal para encender el LED

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Arduino.Write("F"); // Señal para apagar el LED

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
