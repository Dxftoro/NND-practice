using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MO_31_1_Lesnikov_nnd13092.Neuronet;

namespace MO_31_1_Lesnikov_nnd13092
{
    public partial class Form1 : Form
    {
        private double[] inputPixels;
        private Network network;

        public Form1()
        {
            InitializeComponent();
            inputPixels = new double[15];
            network = new Network();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void ChangeStateOnClick(object sender, EventArgs e)
        {
            /* white pixel state */
            if (((Button)sender).BackColor == Color.White)
            {
                ((Button)sender).BackColor = Color.Black;
                inputPixels[((Button)sender).TabIndex] = 1d;
            }

            /* black pixel state */
            else
            {
                ((Button)sender).BackColor = Color.White;
                inputPixels[((Button)sender).TabIndex] = 0d;
            }
        }

        private string PrepareNumberInfo()
        {
            string buffer = actualNumber.Value.ToString();

            for (int i = 0; i < inputPixels.Length; i++)
            {
                buffer += " " + inputPixels[i].ToString();
            }
            buffer += "\n";
            return buffer;
        }

        private void SaveTrainOnClick(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "train.txt";
            string buffer = PrepareNumberInfo();
            File.AppendAllText(path, buffer);
        }

        private void SaveTestOnClick(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "test.txt";
            string buffer = PrepareNumberInfo();
            File.AppendAllText(path, buffer);
        }

    }
}
