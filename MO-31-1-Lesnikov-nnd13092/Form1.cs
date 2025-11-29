using System;
using System.Drawing;
using System.Linq;
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

        private void buttonRecognize_Click(object sender, EventArgs e)
        {
            network.ForwardPass(network, inputPixels);
            labelOut.Text = "Out: " + network.Fact.ToList().IndexOf(network.Fact.Max()).ToString();
            labelProbability.Text = "Probability: " + (100 * network.Fact.Max()).ToString("0.00") + " %";
        }

        private void TrainOnClick(object sender, EventArgs e)
        {
            network.Train(network);

            for (int i = 0; i < network.ErrorEnAvg.Length; i++)
            {
                chartEnAvr.Series[0].Points.AddY(network.ErrorEnAvg[i]);
                chartEnAvr.Series[1].Points.AddY(network.EpochPrecisions[i]);
            }

            MessageBox.Show("Training completed!", "Info", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TestOnClick(object sender, EventArgs e)
        {
            double averageErrorEn = network.Test(network);

            for (int i = 0; i < network.ErrorEnAvg.Length; i++)
            {
                chartEnAvr.Series[0].Points.AddY(network.ErrorEnAvg[i]);
                chartEnAvr.Series[1].Points.AddY(network.EpochPrecisions[i]);
            }

            string stringValue = averageErrorEn.ToString("0.0000");
            testAee.Text = "Test AEE: " + stringValue;
            MessageBox.Show("Testing completed with AEE = " + stringValue, "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
