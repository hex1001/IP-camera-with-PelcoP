using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
namespace IpCamera
{
    public partial class mainForm : Form
    {
        Image<Bgr, Byte> img;
        SerialPort mySerialPort = new SerialPort();
        byte adresDev = 0;
        PelcoP pelco;       
        

        public mainForm()
        {
            InitializeComponent();
            ////////////////////////////////////////
            string[] ports = SerialPort.GetPortNames();
            cbComPort.Items.AddRange(ports);
            cbComPort.SelectedIndex = ports.Length - 1;
            /////////////////// =/////////////////
            cbBaudRate.SelectedIndex = 4;
            cbParity.SelectedIndex = 0;
            cbStopBits.SelectedIndex = 0;
            cbDataBits.SelectedIndex = 1;
            cbAddres.SelectedIndex = 0;
            ///////////////////////////////////////
            mySerialPort = new SerialPort();
            adresDev = (byte)(cbAddres.SelectedIndex);
            this.txtCommand.Focus();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += processFrameAndUpdateGUI;
        }
        private void processFrameAndUpdateGUI(object sender, EventArgs arg)
        {
            NetworkCredential nc = new NetworkCredential();            
            nc.UserName = "admin";
            nc.Password = "12345";
            WebRequest request = WebRequest.Create("http://172.16.30.64/PSIA/Streaming/channels/1/picture");
            request.Credentials = nc;
            request.PreAuthenticate = true;
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            try
            {
                img = new Image<Bgr, byte>(new Bitmap(response.GetResponseStream()));
                img = img.Rotate(180, new Bgr(255,255,255));
                pictureBox1.Image = img.ToBitmap();
            }
            catch { MessageBox.Show("Konvertatsiya bulmadi"); }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            img = null;
            if (mySerialPort.IsOpen == true)
                mySerialPort.Close();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (mySerialPort.IsOpen == false)
            {
                try
                {
                    btnConnect.Text = "Портни ёпиш";
                    btnConnect.ForeColor = Color.Red;
                    mySerialPort.PortName = cbComPort.Text;
                    mySerialPort.BaudRate = Convert.ToInt32(cbBaudRate.Text);
                    mySerialPort.Parity = (Parity)Enum.Parse(typeof(Parity), cbParity.Text);//Parity.None;
                    mySerialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cbStopBits.Text);//StopBits.One;
                    mySerialPort.DataBits = Convert.ToInt32(cbDataBits.Text);
                    mySerialPort.Handshake = Handshake.None;
                    mySerialPort.Open();
                    gBoxParams.Enabled = false;
                    pButtons.Enabled = true;
                    pelco = new PelcoP(adresDev);
                }
                catch (Exception exc) 
                { 
                    MessageBox.Show("Aloqa o'rnatilmadi.\n" + exc.Message); 
                }
            }
            else
            {
                btnConnect.Text = "Портни очиш";
                mySerialPort.Close();
                gBoxParams.Enabled = true;
                pButtons.Enabled = false;
                btnConnect.ForeColor = Color.LightGreen;
            }
            this.txtCommand.Focus();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
            this.txtCommand.Focus();

        }       

        

        private void button1_Click(object sender, EventArgs e)
        {
            mySerialPort.Write(pelco.CamStop(), 0, pelco.CommandLength);            
        }

        private void btnUp_MouseDown(object sender, MouseEventArgs e)
        {
            mySerialPort.Write(pelco.CamToUp(), 0, pelco.CommandLength);
            this.txtCommand.Focus();
        }

        private void btnUp_MouseUp(object sender, MouseEventArgs e)
        {
            mySerialPort.Write(pelco.CamStop(), 0, pelco.CommandLength);
            this.txtCommand.Focus();
        }

        private void btnDown_MouseDown(object sender, MouseEventArgs e)
        {
            mySerialPort.Write(pelco.CamToDown(), 0, pelco.CommandLength);
            this.txtCommand.Focus();
        }

        private void btnRight_MouseDown(object sender, MouseEventArgs e)
        {
            mySerialPort.Write(pelco.CamToRight(), 0, pelco.CommandLength);
            this.txtCommand.Focus();
        }

        private void btnLeft_MouseDown(object sender, MouseEventArgs e)
        {
            mySerialPort.Write(pelco.CamToLeft(), 0, pelco.CommandLength);
            this.txtCommand.Focus();
        }

        private void KeyDown(object sender, KeyEventArgs e)
        {
            if (mySerialPort.IsOpen == true)
            {
                switch (e.KeyCode)
                {
                    case Keys.Right: mySerialPort.Write(pelco.CamToRight(), 0, pelco.CommandLength);
                        break;
                    case Keys.Left: mySerialPort.Write(pelco.CamToLeft(), 0, pelco.CommandLength);
                        break;
                    case Keys.Up: mySerialPort.Write(pelco.CamToUp(), 0, pelco.CommandLength);
                        break;
                    case Keys.Down: mySerialPort.Write(pelco.CamToDown(), 0, pelco.CommandLength);
                        break;
                }
            }
           
            this.txtCommand.Focus();
        }

        private void KeyUp(object sender, KeyEventArgs e)
        {
            if (mySerialPort.IsOpen == true)
            {
                switch (e.KeyCode)
                {
                    case Keys.Right: mySerialPort.Write(pelco.CamStop(), 0, pelco.CommandLength);
                        break;
                    case Keys.Left: mySerialPort.Write(pelco.CamStop(), 0, pelco.CommandLength);
                        break;
                    case Keys.Up: mySerialPort.Write(pelco.CamStop(), 0, pelco.CommandLength);
                        break;
                    case Keys.Down: mySerialPort.Write(pelco.CamStop(), 0, pelco.CommandLength);
                        break;
                }
            }
            this.txtCommand.Focus();
        }

        private void btnPlay_KeyDown(object sender, KeyEventArgs e)
        {
            mySerialPort.Write(pelco.CamToUp(), 0, pelco.CommandLength);
            Thread.Sleep(300);
            mySerialPort.Write(pelco.CamStop(), 0, pelco.CommandLength);
            mySerialPort.Write(pelco.CamToUp(), 0, pelco.CommandLength);
            Thread.Sleep(300);
            mySerialPort.Write(pelco.CamStop(), 0, pelco.CommandLength);
        }
    }
}
