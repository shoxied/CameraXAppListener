using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace курсач2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new Task(() =>
            {
                Socket receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint hostIpEndPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 49000);

                receiveSocket.Bind(hostIpEndPoint);
                receiveSocket.Listen(0);
                while (true)
                {
                    Socket hostSocket = receiveSocket.Accept();
                    var datanow = System.DateTime.Now.Ticks;
                    FileStream fs = File.Create(@"C:\Users\User\Desktop\photos\Image-" + datanow + ".JPG");

                    while (true)
                    {
                        byte[] b = new byte[1000];
                        int n = hostSocket.Receive(b);
                        if (n <= 0)
                        {
                            break;
                        }
                        fs.Write(b, 0, n);
                    }

                    fs.Close();
                    hostSocket.Close();
                    pictureBox1.Load("C:\\Users\\User\\Desktop\\photos\\Image-" + datanow + ".JPG");
                }
                receiveSocket.Close();
            }).Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog.FileName);
            }
        }
    }
}
