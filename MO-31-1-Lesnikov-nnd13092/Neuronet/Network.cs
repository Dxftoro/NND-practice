using System;
using System.IO;
using System.Security.Cryptography;

namespace MO_31_1_Lesnikov_nnd13092.Neuronet
{
	class Network
	{
		/* Network layers */
		private InputLayer inputLayer = new InputLayer(NetworkMode.TRAIN);
		private HiddenLayer hiddenLayer1 = new HiddenLayer(71, 15, nameof(hiddenLayer1));
		private HiddenLayer hiddenLayer2 = new HiddenLayer(32, 71, nameof(hiddenLayer2));
		private OutputLayer outputLayer = new OutputLayer(10, 32, nameof(outputLayer));

		private double[] fact = new double[10];

		/* Average error energy on each epoch */
		private double[] errorEnAvg;

		public double[] Fact { get => fact; }
		public double[] ErrorEnAvg { get => errorEnAvg; set => errorEnAvg = value; }

		public Network() { }

		/* Network forward pass */
		public void ForwardPass(Network network, double[] networkInput)
		{
			for (int i = 0; i < networkInput.Length; i++)
			{
				if (networkInput[i] == 1.0) networkInput[i] = 0.75;
				else if (networkInput[i] == 0.0) networkInput[i] = -0.75;
			}

			network.hiddenLayer1.Data = networkInput;
			network.hiddenLayer1.Recognize(null, network.hiddenLayer2);
			network.hiddenLayer2.Recognize(null, network.outputLayer);
			network.outputLayer.Recognize(network, null);
		}

		public void Train(Network network, int epoches = 38)
		{
			network.inputLayer = new InputLayer(NetworkMode.TRAIN);
			double errorSum = 0.0;
			double[] errors;
			double[] layerGrad1;
			double[] layerGrad2;

			errorEnAvg = new double[epoches];

			for (int k = 0; k < epoches; k++)
			{
				errorEnAvg[k] = 0.0;
				network.inputLayer.ShuffleArrayRows(network.inputLayer.Trainset);

				for (int i = 0; i < network.inputLayer.Trainset.GetLength(0); i++)
				{
					double[] tempTrain = new double[15];
					for (int j = 0; j < tempTrain.Length; j++)
					{
						tempTrain[j] = network.inputLayer.Trainset[i, j + 1];
					}

					ForwardPass(network, tempTrain);

					errorSum = 0.0;
					errors = new double[network.Fact.Length];
					for (int x = 0; x < errors.Length; x++)
					{
						if (x == network.inputLayer.Trainset[i, 0])
							errors[x] = 1.0 - network.Fact[x];
						else errors[x] = -network.Fact[x];
						errorSum += errors[x] * errors[x] / 2;
					}
					errorEnAvg[k] += errorSum / errors.Length;

					layerGrad2 = network.outputLayer.BackwardPass(errors);
					layerGrad1 = network.hiddenLayer2.BackwardPass(layerGrad2);
					network.hiddenLayer1.BackwardPass(layerGrad1);
				}

				errorEnAvg[k] /= network.inputLayer.Trainset.GetLength(0);
			}

			string weightPath = AppDomain.CurrentDomain.BaseDirectory + "memory\\";

			//network.inputLayer = null;
			network.hiddenLayer1.InitializeWeights(MemoryMode.SET, nameof(hiddenLayer1) + "_memory.csv");
			network.hiddenLayer2.InitializeWeights(MemoryMode.SET, nameof(hiddenLayer2) + "_memory.csv");
			network.outputLayer.InitializeWeights(MemoryMode.SET, nameof(outputLayer) + "_memory.csv");
		}
	}
}
