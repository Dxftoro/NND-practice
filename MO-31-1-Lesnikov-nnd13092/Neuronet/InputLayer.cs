using System;
using System.Drawing.Text;
using System.IO;

namespace MO_31_1_Lesnikov_nnd13092.Neuronet
{
    class InputLayer
    {
        private double[,] trainset;
        private double[,] testset;

        /* Properties */

        public double[,] Trainset { get => trainset; }
        public double[,] Testset { get => testset; }

        /*Methods*/

        public InputLayer(NetworkMode networkMode)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string[] tempArrayLines;
            string[] tempLine;

            switch (networkMode) {
                case NetworkMode.TEST:
                    tempArrayLines = File.ReadAllLines(basePath + "train.txt");
                    trainset = new double[tempArrayLines.Length, 16];

                    for (int i = 0; i < tempArrayLines.Length; i++)
                    {
                        tempLine = tempArrayLines[i].Split(' ');
                        for (int j = 0; j < 16; j++)
                        {
                            trainset[i, j] = double.Parse(tempLine[j]);
                        }
                    }
                    ShuffleArrayRows(trainset);
                    break;

                case NetworkMode.TRAIN:
                    tempArrayLines = File.ReadAllLines(basePath + "test.txt");
                    testset = new double[tempArrayLines.Length, 16];

                    for (int i = 0; i < tempArrayLines.Length; i++)
                    {
                        tempLine = tempArrayLines[i].Split(' ');
                        for (int j = 0; j < 16; j++)
                        {
                            testset[i, j] = double.Parse(tempLine[j]);
                        }
                    }
                    ShuffleArrayRows(testset);
                    break;

                case NetworkMode.DEMO:
                    break;
            }
        }

        public void ShuffleArrayRows(double[,] data)
        {

        }
    }
}
