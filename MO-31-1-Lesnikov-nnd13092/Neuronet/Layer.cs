using System.IO;
using System;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace MO_31_1_Lesnikov_nnd13092.Neuronet
{
	abstract class Layer
	{
		protected string layerName;
		string weightPath;
		string weightFile;
		protected int size;                             // Count of neurons on this layer
		protected int prevSize;                         // Count of neurons on previous layer
		protected const double learningRate = 0.0505;    // How fast neurons will be learning
		protected const double momentum = 0.000002d;       // Inertion moment
		protected double[,] latestWeights;              // 2-dim. array of weights calculated on previous iteration
		protected Neuron[] neurons;

		/* Properties */
		public Neuron[] Neurons { get => neurons; set => neurons = value; }

		/* Setting neuron data througth activating */
		public double[] Data
		{
			set
			{
				for (int i  = 0; i < size; i++)
				{
					Neurons[i].Activator(value);
				}
			}
		}

		/* Methods */
		protected Layer(int size, int prevSize, NeuronType type, string layerName)
		{
			this.size = size;
			this.prevSize = prevSize;
			Neurons = new Neuron[size];
			this.layerName = layerName;
			weightPath = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
			weightFile = weightPath + layerName + "_memory.csv";

			double[,] weights;

			if (File.Exists(weightFile))
			{
				weights = InitializeWeights(MemoryMode.GET, weightFile);
			}
			else
			{
				Directory.CreateDirectory(weightPath);
				weights = InitializeWeights(MemoryMode.INIT, weightFile);
			}

			latestWeights = new double[size, prevSize + 1];
			for (int i = 0; i < size; i++)
			{
				double[] tempWeights = new double[prevSize + 1];
				for (int j = 0; j < prevSize + 1; j++)
				{
					tempWeights[j] = weights[i, j];
				}
				Neurons[i] = new Neuron(tempWeights, type);
			}
		}

		public double[,] InitializeWeights(MemoryMode memoryMode, string path)
		{
			char[] delim = new char[] { ';', ' ' };
			//string buffer;
			string[] tempStrWeights;
			double[,] weights = new double[size, prevSize + 1];

			switch (memoryMode)
			{
				case MemoryMode.GET:
					//LayerMessage("Memory will be get");
					tempStrWeights = File.ReadAllLines(path);
					string[] memoryElement;
					for (int i = 0; i < size; i++)
					{
						memoryElement = tempStrWeights[i].Split(delim);
						for (int j = 0; j < prevSize + 1; j++)
						{
							weights[i, j] = double.Parse(memoryElement[j].Replace(',', '.'),
								System.Globalization.CultureInfo.InvariantCulture);
						}
					}
					break;

				case MemoryMode.SET:
					//LayerMessage("Memory will be set");

					tempStrWeights = new string[size];

					if (!File.Exists(path))
					{
						LayerMessage("Weight file not found!");
					}

					for (int i = 0; i < size; i++)
					{
						for (int j = 0; j < prevSize + 1; j++)
						{
							tempStrWeights[i] += weights[i, j].ToString() + ";";
						}
					}

					File.WriteAllLines(path, tempStrWeights);
					break;

				case MemoryMode.INIT:
					LayerMessage("Memory will be init");

					tempStrWeights = new string[size];
					Random random = new Random();

					for (int i = 0; i < size; i++)
					{
						double weightSum = 0.0;
						double weightSquaredSum = 0.0;

						/* Picking random weights from [-1; +1] */
						for (int j = 0; j < prevSize + 1; j++)
						{
							weights[i, j] = random.NextDouble() * 2.0 - 1.0;
							weightSum += weights[i, j];
							weightSquaredSum += weights[i, j] * weights[i, j];
						}

						/* Calculating average weight and base offset */
						double averageWeight = weightSum / (prevSize + 1);
						double baseOffset = Math.Sqrt((weightSquaredSum / (prevSize + 1))
							- (averageWeight * averageWeight));

						/* Standartizing weights and writing them to file */
						for (int j = 0; j < prevSize + 1; j++)
						{
							weights[i, j] = (weights[i, j] - averageWeight) / baseOffset;
							tempStrWeights[i] += weights[i, j].ToString().Replace(',', '.') + ";";
						}
					}

					File.WriteAllLines(path, tempStrWeights);
					break;
			}

			return weights;
		}

		protected void LayerMessage(string message)
		{
			MessageBox.Show(message, layerName);
		}

		abstract public void Recognize(Network network, Layer nextLayer);
		abstract public double[] BackwardPass(double[] stuff);
	}
}
