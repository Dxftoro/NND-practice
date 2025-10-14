namespace MO_31_1_Lesnikov_nnd13092.Neuronet
{
    class Network
    {
        /* Network layers */
        private InputLayer inputLayer = null;
        private HiddenLayer hiddenLayer1 = new HiddenLayer(71, 15, nameof(hiddenLayer1));
        private HiddenLayer hiddenLayer2 = new HiddenLayer(32, 71, nameof(hiddenLayer2));
        private OutputLayer outputLayer = new OutputLayer(10, 32, nameof(outputLayer));

        private double[] fact = new double[10];
        private double[] errorEnAvg;

        public double[] Fact { get => fact; }

        public double[] ErrorEnAvg { get => errorEnAvg; set => errorEnAvg = value; }

        public Network() { }
    }
}
