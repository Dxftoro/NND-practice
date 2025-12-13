using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

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
		private bool isTraining = false;

		/* Average error energy on each epoch */
		private double[] errorEnAvg;

		/* Array wich contains percentage of right answers on each test epoch */
		private	double[] epochPrecisions;

		/* Runtime configurable flags */
		private bool regularisationEnabled = false;
		private bool dropOutEnabled = false;

		/* Properties */
		public double[] Fact { get => fact; }
		public double[] ErrorEnAvg { get => errorEnAvg; set => errorEnAvg = value; }
		public double[] EpochPrecisions { get => epochPrecisions; set => epochPrecisions = value; }
		public bool RegularisationEnabled { get => regularisationEnabled; set => regularisationEnabled = value; }
		public bool DropOutEnabled { get => dropOutEnabled; set => dropOutEnabled = value; }
		
		public void SetRegularisationLambda(double lambda)
		{

		}

        public void SetDropOutRate(double dropOutRate)
        {

        }

        public Network() { }

		/* Network forward pass */
		public void ForwardPass(Network network, double[] networkInput)
		{
			for (int i = 0; i < networkInput.Length; i++)
			{
				if (networkInput[i] == 1.0) networkInput[i] = 0.8;
				else if (networkInput[i] == 0.0) networkInput[i] = -0.8;
			}

			network.hiddenLayer1.Data = networkInput;
			network.hiddenLayer1.Recognize(null, network.hiddenLayer2, dropOutEnabled, isTraining);
			network.hiddenLayer2.Recognize(null, network.outputLayer, dropOutEnabled, isTraining);
			network.outputLayer.Recognize(network, null);
		}

		public void Train(Network network, int epoches = 10)
		{
			isTraining = true;
			network.inputLayer = new InputLayer(NetworkMode.TRAIN);
			double errorSum;
			double[] errors;
			double[] layerGrad1;
			double[] layerGrad2;

			errorEnAvg = new double[epoches];
            epochPrecisions = new double[epoches];

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

                    int maxFactIndex = network.Fact.ToList().IndexOf(network.Fact.Max());
                    if (maxFactIndex == network.inputLayer.Trainset[i, 0]) epochPrecisions[k]++;

                    layerGrad2 = network.outputLayer.BackwardPass(errors, regularisationEnabled);
					layerGrad1 = network.hiddenLayer2.BackwardPass(layerGrad2, regularisationEnabled);
					network.hiddenLayer1.BackwardPass(layerGrad1, regularisationEnabled);
				}

				errorEnAvg[k] /= network.inputLayer.Trainset.GetLength(0);
                epochPrecisions[k] /= network.inputLayer.Trainset.GetLength(0);
            }

			isTraining = false;

            string weightPath = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
			//network.inputLayer = null;
			network.hiddenLayer1.InitializeWeights(MemoryMode.SET, nameof(hiddenLayer1) + "_memory.csv");
			network.hiddenLayer2.InitializeWeights(MemoryMode.SET, nameof(hiddenLayer2) + "_memory.csv");
			network.outputLayer.InitializeWeights(MemoryMode.SET, nameof(outputLayer) + "_memory.csv");
		}

		public double Test(Network network, int epoches = 10)
        {
			network.inputLayer = new InputLayer(NetworkMode.TEST);
			double errorSum;
			double[] errors;
			errorEnAvg = new double[epoches];
			epochPrecisions = new double[epoches];

			for (int k = 0; k < epoches; k++)
            {
				errorEnAvg[k] = 0.0;
				network.inputLayer.ShuffleArrayRows(network.inputLayer.Testset);

				for (int i = 0; i < network.inputLayer.Testset.GetLength(0); i++)
                {
					double[] tempTest = new double[15];
					for (int j = 0; j < tempTest.Length; j++)
					{
						tempTest[j] = network.inputLayer.Testset[i, j + 1];
					}

					ForwardPass(network, tempTest);

					errorSum = 0.0;
					errors = new double[network.Fact.Length];
					for (int x = 0; x < errors.Length; x++)
					{
						if (x == network.inputLayer.Testset[i, 0])
							errors[x] = 1.0 - network.Fact[x];
						else errors[x] = -network.Fact[x];
						errorSum += errors[x] * errors[x] / 2;
					}
					errorEnAvg[k] += errorSum / errors.Length;

					int maxFactIndex = network.Fact.ToList().IndexOf(network.Fact.Max());
					if (maxFactIndex == network.inputLayer.Testset[i, 0]) epochPrecisions[k]++;
				}

				errorEnAvg[k] /= network.inputLayer.Testset.GetLength(0);
				epochPrecisions[k] /= network.inputLayer.Testset.GetLength(0);
			}

			double resultErrorEn = 0.0;
			for (int i = 0; i < errorEnAvg.Length; i++) resultErrorEn += errorEnAvg[i];
			return resultErrorEn / errorEnAvg.Length;
		}
	}
}
