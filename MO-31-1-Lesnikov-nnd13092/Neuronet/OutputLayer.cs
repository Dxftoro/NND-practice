namespace MO_31_1_Lesnikov_nnd13092.Neuronet
{
    class OutputLayer : Layer
    {
        public OutputLayer(int size, int prevSize, string layerName) 
            : base(size, prevSize, NeuronType.OUTPUT, layerName )
        {
        }

        /* Forward pass */
        public override void Recognize(Network network, Layer nextLayer)
        {
            double eSum = 0;
            for (int i = 0; i < neurons.Length; i++)
            {
                eSum += neurons[i].Output;
            }

            for (int i = 0; i < neurons.Length; i++)
            {
                network.Fact[i] = neurons[i].Output / eSum;
            }
        }

        /* Backward pass */
        public override double[] BackwardPass(double[] errors)
        {
            double[] grSum = new double[prevSize + 1];
            for (int i = 0; i < prevSize + 1; i++)
            {
                double sum = 0;
                for (int j = 0; j < size; j++)
                {
                    sum += neurons[j].Weights[j] * errors[j];
                }
                grSum[i] = sum;
            }

            for (int i = 0; i < size; i++)
            {
                for (int n = 0; n < prevSize + 1; n++)
                {
                    double delta;
                    if (n == 0) delta = momentum * latestWeights[i, 0] + learningRate * errors[i];
                    else delta = momentum * latestWeights[i, n] + learningRate * neurons[i].Inputs[n - 1] * errors[i];

                    latestWeights[i, n] = delta;
                    neurons[i].Weights[n] += delta;
                }
            }

            return grSum;
        }
    }
}
