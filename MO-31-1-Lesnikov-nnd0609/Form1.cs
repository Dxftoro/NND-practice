namespace MO_31_1_Lesnikov_nnd0609
{
    public partial class Form1 : Form
    {
        private double[] inputPixels;
        //private Network network;

        public Form1()
        {
            InitializeComponent();
            inputPixels = new double[15];
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

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
