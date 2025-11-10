using System.CodeDom;
using static System.Math;

namespace MO_31_1_Lesnikov_nnd13092.Neuronet
{
    class Neuron
    {
        private NeuronType type;
        private double[] weights;
        private double[] inputs;
        private double output;
        private double derivative; // a value of activation function derivative

        /* Activation function constant */
        private const double a = 0.01d;

        /* Properties */
        public double[] Weights { get => weights; set => weights = value; }
        public double[] Inputs { get => inputs; set => inputs = value; }
        public double Output { get => output; }
        public double Derivative { get => derivative; }

        /* Methods */
        public Neuron(double[] weights, NeuronType type)
        {
            this.type = type;
            this.weights = weights;
        }

        public void Activator(double[] _input)
        {
            inputs = _input;
            double weightSum = weights[0];

            for (int j = 0; j < inputs.Length; j++)
            {
                weightSum += inputs[j] * weights[j + 1];
            }

            switch (type)
            {
                case NeuronType.HIDDEN:
                    output = HyperbolicTan(weightSum);
                    derivative = HyperbolicTanDerivative(weightSum);
                    break;
                case NeuronType.OUTPUT:
                    output = Exp(weightSum);
                    break;
            }
        }

        private double HyperbolicTan(double x)
        {
            double exp = Exp(2 * x);
            return (exp - 1) / (exp + 1);
        }

        private double HyperbolicTanDerivative(double x)
        {
            double exp = Exp(2 * x);
            return (4 * exp) / Pow(exp + 1, 2);
        }
    }
}
