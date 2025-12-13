using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MO_31_1_Lesnikov_nnd13092.Neuronet
{
	class HiddenLayer : Layer
	{
        private const double dropOutRate = 0.1;		// The chance with wich neuron will be removed by dropout
		private bool[] dropOutMask = null;			// This mask determines if i-neuron	was removed by dropout or not

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

		public void Recognize(Network network, Layer nextLayer, bool dropOutEnabled, bool isTraining)
		{
			/* Applying default Recognize overload if dropOutEnabled is not specified */
			if (!dropOutEnabled)
			{
				Recognize(network, nextLayer);
				return;
			}

			if (isTraining)
			{
                Random random = new Random();
                dropOutMask = new bool[size];
				int removedCount = 0;

				for (int i = 0; i < size; i++)
				{
					dropOutMask[i] = (random.NextDouble() > dropOutRate);

					if (dropOutMask[i]) removedCount++;
				}

                Debug.WriteLine("Dropout: removed " + removedCount + "/" + size + " neurons");
            }

			double[] hiddenOut = new double[size];
			for (int i = 0; i < size; i++)
			{
				double output = neurons[i].Output;

				/* Scaling if network is not training but dropOutEnabled was set as true */
				if (isTraining)
				{
					output = dropOutMask[i] ? neurons[i].Output / (1.0 - dropOutRate) : 0.0;
				}

				hiddenOut[i] = output;
			}

            nextLayer.Data = hiddenOut;
        }

        /* Backward pass */
        public override double[] BackwardPass(double[] gradientSums, bool doRegularization)
		{
			double[] gradientSum = new double[prevSize];
			double regToggler = doRegularization ? 1.0 : 0.0;

			for (int i = 0; i < prevSize; i++)
			{
				double sum = 0;
				for (int j = 0; j < size; j++)
				{
					if (dropOutMask == null || dropOutMask[j])
					{
						sum += neurons[j].Weights[i + 1] * neurons[j].Derivative * gradientSums[j];
					}
				}
				gradientSum[i] = sum;
			}
			
			/* Weight correction cycle */
			for (int i = 0; i < size; i++)
			{
				if (dropOutMask != null && !dropOutMask[i]) continue;

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
						double regularization = regToggler * lambda * neurons[i].Weights[j];

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
