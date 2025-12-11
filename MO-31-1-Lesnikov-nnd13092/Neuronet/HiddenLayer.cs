namespace MO_31_1_Lesnikov_nnd13092.Neuronet
{
	class HiddenLayer : Layer
	{
		public HiddenLayer(int size, int prevSize, string layerName)
			: base(size, prevSize, NeuronType.HIDDEN, layerName)
		{

		}

		/* Forward pass */
		public override void Recognize(Network network, Layer nextLayer)
		{
			double[] hiddenOut = new double[size];
			for (int i = 0; i < size; i++)
			{
				hiddenOut[i] = neurons[i].Output;
			}

			/* Transmitting output signal to the next layer's input */
			nextLayer.Data = hiddenOut;
		}

		/* Backward pass */
		public override double[] BackwardPass(double[] gradientSums)
		{
			double[] gradientSum = new double[prevSize];
			for (int i = 0; i < prevSize; i++)
			{
				double sum = 0;
				for (int j = 0; j < size; j++)
				{
					sum += neurons[j].Weights[i + 1] * neurons[j].Derivative * gradientSums[j];
				}
				gradientSum[i] = sum;
			}
			
			/* Weight correction cycle */
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < prevSize + 1; j++) {
					double delta;
					if (j == 0)
					{
						delta = momentum * latestWeights[i, 0] 
							+ learningRate * neurons[i].Derivative * gradientSums[i];
					}
					else
					{
						double gradient = neurons[i].Inputs[j - 1] * neurons[i].Derivative * gradientSums[i];
						double regularization = lambda * neurons[i].Weights[j];

						delta = momentum * latestWeights[i, j]
							+ learningRate * (gradient + regularization);
					}

					latestWeights[i, j] = delta;
					neurons[i].Weights[j] += delta;
				}
			}

			return gradientSum;
		}
	}
}
