using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace PaniService
{

    public partial class Form1 : Form
    {
        //Connect and go through ATCRA320
        //Before send the command start with ATCRA777 - send the command - restart from ATCRA300
        byte[] buffer = new byte[1024];
        byte[] AT = { 0x41, 0x54, 0x0D };
        byte[] ATZ = { 0x41, 0x54,0x5A, 0x0D };
        byte[] ATE0 = { 0x41, 0x54, 0x45, 0x30, 0x0D };
        byte[] ATAR = { 0x41, 0x54, 0x41, 0x52, 0x0D };
        byte[] ATI = { 0x41, 0x54, 0x49, 0x0D };
        byte[] ATH1 = { 0x41, 0x54, 0x48, 0x31, 0x0D };
        byte[] ATCAF1 = { 0x41, 0x54, 0x43, 0x41, 0x46, 0x31, 0x0D };
        byte[] ATL0 = { 0x41, 0x54, 0x4C, 0x30, 0x0D };
        byte[] ATAL = { 0x41, 0x54, 0x41, 0x4C, 0x0D };
        byte[] ATSP6 = { 0x41, 0x54, 0x53, 0x50, 0x36, 0x0D };
        byte[] ATSH7E0 = { 0x41, 0x54, 0x53, 0x48, 0x37, 0x45, 0x30, 0x0D };
        byte[] ATSH7DF = { 0x41, 0x54, 0x53, 0x48, 0x37, 0x44, 0x46, 0x0D };
        byte[] ATCRA7E8 = { 0x41, 0x54, 0x43, 0x52, 0x41, 0x37, 0x45, 0x38, 0x0D };
        byte[] DATA1 = { 0x30, 0x31, 0x0D };
        byte[] ATCAF0 = { 0x41, 0x54, 0x43, 0x41, 0x46, 0x30, 0x0D };
        byte[] ATRV = { 0x41, 0x54, 0x52, 0x56, 0x0D };
        byte[] ATCRA300 = { 0x41, 0x54, 0x43, 0x52, 0x41, 0x33, 0x30, 0x30, 0x0D };
        byte[] DATA2 = { 0x30, 0x30, 0x20, 0x31, 0x0D };
        byte[] DATA3 = { 0x30,0x30,0x20,0x33,0x0D };
        byte[] ATCRA310 = { 0x41, 0x54, 0x43, 0x52, 0x41, 0x33, 0x31, 0x30, 0x0D };
        byte[] ATCRA320 = { 0x41, 0x54, 0x43, 0x52, 0x41, 0x33, 0x32, 0x30, 0x0D };
        byte[] ATCRA777 = { 0x41, 0x54, 0x43, 0x52, 0x41, 0x37, 0x37, 0x37, 0x0D };
        byte[] ATSH500 = { 0x41, 0x54, 0x53, 0x48, 0x35, 0x30, 0x30, 0x0D };
        byte[] ATCRA500 = { 0x41, 0x54, 0x43, 0x52, 0x41, 0x35, 0x30, 0x30, 0x0D };
        byte[] OIL = { 0x36, 0x63, 0x30, 0x30, 0x30, 0x30, 0x31, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30,0x0D };
        byte[] DESMO = { 0x36, 0x63, 0x30, 0x30, 0x31, 0x30, 0x31, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30,0x0D };
        byte[] LIGHT_TEST = { 0x36, 0x63, 0x30, 0x30, 0x30, 0x30, 0x30, 0x32, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x0D };
        byte[] TPS_RESET = { 0x36, 0x63, 0x38, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x0D };
        byte[] APS_RESET = { 0x36, 0x63, 0x34, 0x30, 0x30, 0x30, 0x30, 0x32, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x0D };
        byte[] DTC = { 0x36, 0x63, 0x30, 0x30, 0x30, 0x30, 0x36, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x0D };
        byte[] OH = { 0x36, 0x63, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x38, 0x30, 0x30, 0x0D };

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label4.Text = "Connecting please wait";
            serialPort1.Close();
            serialPort1.BaudRate = Convert.ToInt32(comboBox1.Text);
            serialPort1.PortName = comboBox2.Text;
            serialPort1.ReadTimeout = 5000;
            //serialPort1.WriteTimeout = 1000;
            try
            {
                serialPort1.Open();
                Thread readThread = new Thread(ReadBus);
                ReadBus();
            }
            catch (System.Exception excep) { }


            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int neu = 0;
            serialPort1.Write(ATCRA777,0,ATCRA777.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATSH500,0,ATSH500.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(LIGHT_TEST, 0, LIGHT_TEST.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA300, 0, ATCRA300.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA310, 0, ATCRA310.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA320, 0, ATCRA320.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int neu = 0;
            serialPort1.Write(ATCRA777, 0, ATCRA777.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATSH500, 0, ATSH500.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(OIL, 0, OIL.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA300, 0, ATCRA300.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA310, 0, ATCRA310.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA320, 0, ATCRA320.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int neu = 0;
            serialPort1.Write(ATCRA777, 0, ATCRA777.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATSH500, 0, ATSH500.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DESMO, 0, DESMO.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA300, 0, ATCRA300.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA310, 0, ATCRA310.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA320, 0, ATCRA320.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int neu = 0;
            serialPort1.Write(ATCRA777, 0, ATCRA777.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATSH500, 0, ATSH500.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(APS_RESET, 0, APS_RESET.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA300, 0, ATCRA300.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA310, 0, ATCRA310.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA320, 0, ATCRA320.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int neu = 0;
            serialPort1.Write(ATCRA777, 0, ATCRA777.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATSH500, 0, ATSH500.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(TPS_RESET, 0, TPS_RESET.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA300, 0, ATCRA300.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA310, 0, ATCRA310.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA320, 0, ATCRA320.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);
        }
        public void ReadBus()
        {
            if (serialPort1.IsOpen)
            {
                int neu = 0;
                try
                {
                    serialPort1.Write(AT, 0, AT.Length);
                    System.Threading.Thread.Sleep(1000);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    richTextBox1.Text = Encoding.UTF8.GetString(buffer);
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
                    Array.Clear(buffer, 0, 1024);

                    serialPort1.Write(ATZ, 0, ATZ.Length);
                    System.Threading.Thread.Sleep(2000);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
                    Array.Clear(buffer, 0, 1024);

                    serialPort1.Write(ATE0, 0, ATE0.Length);
                    System.Threading.Thread.Sleep(1000);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
                    Array.Clear(buffer, 0, 1024);

                    serialPort1.Write(ATAR, 0, ATAR.Length);
                    System.Threading.Thread.Sleep(500);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
                    Array.Clear(buffer, 0, 1024);

                    serialPort1.Write(ATI, 0, ATI.Length);
                    System.Threading.Thread.Sleep(500);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
                    Array.Clear(buffer, 0, 1024);

                    serialPort1.Write(ATI, 0, ATI.Length);
                    System.Threading.Thread.Sleep(500);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
                    Array.Clear(buffer, 0, 1024);

                    serialPort1.Write(ATH1, 0, ATH1.Length);
                    System.Threading.Thread.Sleep(500);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
                    Array.Clear(buffer, 0, 1024);

                    serialPort1.Write(ATCAF1, 0, ATCAF1.Length);
                    System.Threading.Thread.Sleep(500);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
                    Array.Clear(buffer, 0, 1024);

                    serialPort1.Write(ATL0, 0, ATL0.Length);
                    System.Threading.Thread.Sleep(500);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
                    Array.Clear(buffer, 0, 1024);

                    serialPort1.Write(ATAL, 0, ATAL.Length);
                    System.Threading.Thread.Sleep(500);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
                    Array.Clear(buffer, 0, 1024);

                    serialPort1.Write(ATSP6, 0, ATSP6.Length);
                    System.Threading.Thread.Sleep(500);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
                    Array.Clear(buffer, 0, 1024);

                    serialPort1.Write(ATSH7E0, 0, ATSH7E0.Length);
                    System.Threading.Thread.Sleep(500);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
                    Array.Clear(buffer, 0, 1024);

                    serialPort1.Write(ATCRA7E8, 0, ATCRA7E8.Length);
                    System.Threading.Thread.Sleep(500);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
                    Array.Clear(buffer, 0, 1024);
                    
                    serialPort1.Write(DATA1, 0, DATA1.Length);
                    System.Threading.Thread.Sleep(500);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
                    Array.Clear(buffer, 0, 1024);

                    serialPort1.Write(ATCAF0, 0, ATCAF0.Length);
                    System.Threading.Thread.Sleep(500);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
                    Array.Clear(buffer, 0, 1024);

                    serialPort1.Write(ATRV, 0, ATRV.Length);
                    System.Threading.Thread.Sleep(500);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
                    Array.Clear(buffer, 0, 1024);

                    serialPort1.Write(ATCRA300, 0, ATCRA300.Length);
                    System.Threading.Thread.Sleep(500);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                    //serialPort1.DiscardInBuffer();
                    //serialPort1.DiscardOutBuffer();
                    Array.Clear(buffer, 0, 1024);

                    serialPort1.Write(DATA2, 0, DATA2.Length);
                    System.Threading.Thread.Sleep(500);
                    neu = serialPort1.Read(buffer, 0, 1024);
                    String test = System.Text.Encoding.UTF8.GetString(buffer, 16, 2);
                    test += System.Text.Encoding.UTF8.GetString(buffer, 19, 2);
                    int odo = Convert.ToInt16(test,16);
                    label6.Text = Convert.ToString(odo);
                    if (buffer[0] == 'C')
                    {
                        serialPort1.Close();
                        button1.Enabled = true;
                        label4.Text = "CANBUS ERROR";
                        Array.Clear(buffer, 0, 1024);
                    }
                    else
                    {
                        richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                        //serialPort1.DiscardInBuffer();
                        //serialPort1.DiscardOutBuffer();
                        Array.Clear(buffer, 0, 1024);

                        serialPort1.Write(ATCRA310, 0, ATCRA310.Length);
                        System.Threading.Thread.Sleep(500);
                        neu = serialPort1.Read(buffer, 0, 1024);
                        richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                        //serialPort1.DiscardInBuffer();
                        //serialPort1.DiscardOutBuffer();
                        Array.Clear(buffer, 0, 1024);

                        serialPort1.Write(DATA2, 0, DATA2.Length);
                        System.Threading.Thread.Sleep(500);
                        neu = serialPort1.Read(buffer, 0, 1024);
                        richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                        //serialPort1.DiscardInBuffer();
                        //serialPort1.DiscardOutBuffer();
                        Array.Clear(buffer, 0, 1024);

                        serialPort1.Write(ATCRA320, 0, ATCRA320.Length);
                        System.Threading.Thread.Sleep(500);
                        neu = serialPort1.Read(buffer, 0, 1024);
                        richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                        //serialPort1.DiscardInBuffer();
                        //serialPort1.DiscardOutBuffer();
                        Array.Clear(buffer, 0, 1024);

                        serialPort1.Write(DATA2, 0, DATA2.Length);
                        System.Threading.Thread.Sleep(500);
                        neu = serialPort1.Read(buffer, 0, 1024);
                        richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
                        //serialPort1.DiscardInBuffer();
                        //serialPort1.DiscardOutBuffer();
                        Array.Clear(buffer, 0, 1024);

                        button2.Enabled = true;
                        button3.Enabled = true;
                        button4.Enabled = true;
                        button5.Enabled = true;
                        button6.Enabled = true;
                        label4.Text = "Connected";
                        button1.Enabled = false;
                    }
                }
                catch (System.Exception excep)
                {
                    label4.Text = "Connection error";
                    serialPort1.Close();
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button4.Enabled = false;
                    button5.Enabled = false;
                    button6.Enabled = false;
                    button1.Enabled = true;
                }
            }
            else
            {
                label4.Text = "ELM327 not connected";
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            int neu = 0;
            serialPort1.Write(ATCRA777, 0, ATCRA777.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATSH500, 0, ATSH500.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024); 
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(OH, 0, OH.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA300, 0, ATCRA300.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA310, 0, ATCRA310.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(ATCRA320, 0, ATCRA320.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);

            serialPort1.Write(DATA2, 0, DATA2.Length);
            System.Threading.Thread.Sleep(500);
            neu = serialPort1.Read(buffer, 0, 1024);
            richTextBox1.AppendText(Environment.NewLine + Encoding.UTF8.GetString(buffer));
            //serialPort1.DiscardInBuffer();
            //serialPort1.DiscardOutBuffer();
            Array.Clear(buffer, 0, 1024);
        }
    }
}
